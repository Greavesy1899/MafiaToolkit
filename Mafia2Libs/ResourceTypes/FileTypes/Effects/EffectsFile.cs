using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml;

namespace ResourceTypes.Effects
{
    /// <summary>
    /// Reader/writer for the Mafia II ".eff" file — the payload of an SDS "Effects" resource.
    ///
    /// Format (little-endian), reverse-engineered from the game's
    /// ue::sys::utils::C_InputChunk reader and ue::gfx::effects::C_EffectsLibrary::Load:
    ///
    ///   chunk := u32 tag, u32 size, byte[size - 8] payload
    ///
    ///   * 'size' INCLUDES the 8-byte header. The minimum legal size is 8 (an empty chunk);
    ///     0xFFFFFFFF is explicitly rejected by C_InputChunk::Ascend.
    ///   * A *container* chunk's payload is a sequence of child chunks packed back-to-back
    ///     that fills the payload exactly. There is NO padding/alignment between chunks.
    ///   * A *leaf* chunk's payload is raw field data (ints/floats/strings/vectors...).
    ///   * The root chunk uses tag 666 (C_EffectsLibrary). Every other tag is a
    ///     context-local enumeration (e.g. inside a pattern: 1=frames, 2=generations, 3..11
    ///     are scalar params, default=sounds; an operator uses 300/350). Because a tag's
    ///     meaning depends on its parent, this reader keeps the generic (tag, payload/children)
    ///     tree rather than hard-coding semantics.
    ///
    /// This class never mutates bytes it does not understand: after parsing it re-serializes
    /// and compares to the original, falling back to a verbatim passthrough on any mismatch.
    /// </summary>
    public class EffectChunk
    {
        public const uint InvalidSize = 0xFFFFFFFF;
        public const int HeaderSize = 8;

        /// <summary>Tag of the chunk that stores an operator's E_OperatorType id (C_Operator::Load, 0x12C).</summary>
        public const uint OperatorTypeTag = 300;

        [Category("Chunk"), Description("32-bit chunk tag/id. Meaning is relative to the parent chunk.")]
        public uint Tag { get; set; }

        /// <summary>Child chunks when this is a container; null for a leaf.</summary>
        [Browsable(false)]
        public List<EffectChunk> Children { get; set; }

        /// <summary>Raw payload bytes when this is a leaf; null for a container.</summary>
        [Browsable(false)]
        public byte[] Data { get; set; }

        [Category("Chunk"), Description("True when this chunk holds child chunks rather than raw data.")]
        public bool IsContainer => Children != null;

        [Category("Chunk"), Description("Number of immediate child chunks.")]
        public int ChildCount => Children?.Count ?? 0;

        [Category("Chunk"), Description("Total on-disk size in bytes, including the 8-byte header.")]
        public uint Size => (uint)(HeaderSize + GetPayloadLength());

        [Category("Chunk"), Description("If this is an operator-type chunk (tag 300), the decoded E_OperatorType name.")]
        public string OperatorType
        {
            get
            {
                if (Tag == OperatorTypeTag && Data != null && Data.Length >= 4)
                {
                    return EffectsOperatorTypes.Lookup(BitConverter.ToUInt32(Data, 0));
                }
                return null;
            }
        }

        [Category("Chunk"), Description("Leaf payload reinterpreted as 32-bit floats (read-only view).")]
        public string DataFloats
        {
            get
            {
                if (Data == null || Data.Length == 0 || (Data.Length & 3) != 0)
                {
                    return string.Empty;
                }

                int count = Data.Length / 4;
                string[] parts = new string[count];
                for (int i = 0; i < count; i++)
                {
                    parts[i] = BitConverter.ToSingle(Data, i * 4).ToString("0.######", CultureInfo.InvariantCulture);
                }
                return string.Join(", ", parts);
            }
        }

        [Category("Chunk"), Description("Leaf payload reinterpreted as 32-bit signed integers (read-only view).")]
        public string DataInts
        {
            get
            {
                if (Data == null || Data.Length == 0 || (Data.Length & 3) != 0)
                {
                    return string.Empty;
                }

                int count = Data.Length / 4;
                string[] parts = new string[count];
                for (int i = 0; i < count; i++)
                {
                    parts[i] = BitConverter.ToInt32(Data, i * 4).ToString(CultureInfo.InvariantCulture);
                }
                return string.Join(", ", parts);
            }
        }

        [Category("Chunk"), Description("Leaf payload as a hex string. Editable for leaf chunks only.")]
        public string DataHex
        {
            get { return Data == null ? string.Empty : Convert.ToHexString(Data); }
            set
            {
                if (Children != null)
                {
                    // Containers derive their bytes from children; ignore manual edits.
                    return;
                }

                Data = string.IsNullOrWhiteSpace(value)
                    ? Array.Empty<byte>()
                    : Convert.FromHexString(value.Trim());
            }
        }

        public EffectChunk() { }

        public EffectChunk(uint tag)
        {
            Tag = tag;
            Data = Array.Empty<byte>();
        }

        private int GetPayloadLength()
        {
            if (Children != null)
            {
                int total = 0;
                foreach (EffectChunk child in Children)
                {
                    total += (int)child.Size;
                }
                return total;
            }

            return Data?.Length ?? 0;
        }

        public static EffectChunk Read(BinaryReader reader)
        {
            long start = reader.BaseStream.Position;
            uint tag = reader.ReadUInt32();
            uint size = reader.ReadUInt32();

            if (size == InvalidSize || size < HeaderSize)
            {
                throw new InvalidDataException(
                    string.Format(CultureInfo.InvariantCulture,
                        "Invalid .eff chunk size {0} at offset {1} (tag {2}).", size, start, tag));
            }

            int payloadLength = (int)(size - HeaderSize);
            byte[] payload = reader.ReadBytes(payloadLength);
            if (payload.Length != payloadLength)
            {
                throw new EndOfStreamException(
                    string.Format(CultureInfo.InvariantCulture,
                        "Truncated .eff chunk at offset {0} (tag {1}).", start, tag));
            }

            EffectChunk chunk = new EffectChunk { Tag = tag };
            if (TryParseChildren(payload, out List<EffectChunk> children))
            {
                chunk.Children = children;
            }
            else
            {
                chunk.Data = payload;
            }

            return chunk;
        }

        /// <summary>
        /// Tries to interpret <paramref name="payload"/> as a contiguous run of child chunks.
        /// Succeeds only when the children tile the payload exactly (no leftover, no overrun),
        /// which is how the game stores container chunks. Round-trip correctness does not rely
        /// on this being semantically perfect: a leaf misread as a container still re-serializes
        /// to identical bytes because there is no padding.
        /// </summary>
        private static bool TryParseChildren(byte[] payload, out List<EffectChunk> children)
        {
            children = null;
            if (payload.Length < HeaderSize)
            {
                return false;
            }

            List<EffectChunk> result = new List<EffectChunk>();
            using (MemoryStream ms = new MemoryStream(payload, false))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                while (ms.Position < payload.Length)
                {
                    long remaining = payload.Length - ms.Position;
                    if (remaining < HeaderSize)
                    {
                        return false;
                    }

                    long childStart = ms.Position;
                    ms.Position += 4; // skip tag, peek at size
                    uint size = reader.ReadUInt32();
                    if (size == InvalidSize || size < HeaderSize || size > remaining)
                    {
                        return false;
                    }

                    ms.Position = childStart;
                    EffectChunk child;
                    try
                    {
                        child = Read(reader);
                    }
                    catch
                    {
                        return false;
                    }

                    result.Add(child);
                }
            }

            if (result.Count == 0)
            {
                return false;
            }

            children = result;
            return true;
        }

        public void Write(BinaryWriter writer)
        {
            long start = writer.BaseStream.Position;
            writer.Write(Tag);
            writer.Write((uint)0); // size placeholder, patched below

            if (Children != null)
            {
                foreach (EffectChunk child in Children)
                {
                    child.Write(writer);
                }
            }
            else if (Data != null)
            {
                writer.Write(Data);
            }

            long end = writer.BaseStream.Position;
            uint size = (uint)(end - start);
            writer.BaseStream.Position = start + 4;
            writer.Write(size);
            writer.BaseStream.Position = end;
        }

        public void WriteXml(XmlWriter xml)
        {
            xml.WriteStartElement("Chunk");
            xml.WriteAttributeString("tag", Tag.ToString(CultureInfo.InvariantCulture));

            string name = EffectsTagNames.Lookup(Tag);
            if (!string.IsNullOrEmpty(name))
            {
                xml.WriteAttributeString("name", name);
            }

            string operatorType = OperatorType;
            if (!string.IsNullOrEmpty(operatorType))
            {
                xml.WriteAttributeString("operator", operatorType);
            }

            if (Children != null)
            {
                foreach (EffectChunk child in Children)
                {
                    child.WriteXml(xml);
                }
            }
            else if (Data != null && Data.Length > 0)
            {
                xml.WriteAttributeString("data", Convert.ToHexString(Data));
            }

            xml.WriteEndElement();
        }

        public static EffectChunk ReadXml(XmlNode node)
        {
            EffectChunk chunk = new EffectChunk
            {
                Tag = uint.Parse(node.Attributes["tag"].Value, CultureInfo.InvariantCulture)
            };

            List<XmlNode> childElements = new List<XmlNode>();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element && child.Name == "Chunk")
                {
                    childElements.Add(child);
                }
            }

            if (childElements.Count > 0)
            {
                chunk.Children = new List<EffectChunk>();
                foreach (XmlNode childElement in childElements)
                {
                    chunk.Children.Add(ReadXml(childElement));
                }
            }
            else
            {
                XmlAttribute dataAttr = node.Attributes["data"];
                chunk.Data = dataAttr != null && !string.IsNullOrEmpty(dataAttr.Value)
                    ? Convert.FromHexString(dataAttr.Value)
                    : Array.Empty<byte>();
            }

            return chunk;
        }

        public override string ToString()
        {
            string operatorType = OperatorType;
            string name = !string.IsNullOrEmpty(operatorType)
                ? "Operator:" + operatorType
                : EffectsTagNames.Lookup(Tag);

            string label = string.IsNullOrEmpty(name)
                ? string.Format(CultureInfo.InvariantCulture, "Chunk [{0}]", Tag)
                : string.Format(CultureInfo.InvariantCulture, "{0} [{1}]", name, Tag);

            return IsContainer
                ? string.Format(CultureInfo.InvariantCulture, "{0} ({1} children, {2} bytes)", label, ChildCount, Size)
                : string.Format(CultureInfo.InvariantCulture, "{0} ({1} bytes)", label, Size);
        }
    }

    /// <summary>
    /// Best-effort, RE-derived names for a handful of tags that have a fixed meaning regardless
    /// of context. These are display hints only; the parser is fully generic and does not depend
    /// on them. (See C_EffectsLibrary::Load and C_Operator::Load.)
    /// </summary>
    public static class EffectsTagNames
    {
        private static readonly Dictionary<uint, string> Names = new Dictionary<uint, string>
        {
            { 666, "EffectsLibrary" }, // root magic (0x29A)
            { 667, "LibraryHeader" },  // u32 read into the library
            { 668, "Patterns" },       // container of C_EffectPattern chunks
            { 669, "Version" },        // u32 version, expected == 2
            { 300, "OperatorType" },   // 0x12C: operator type id chunk
            { 350, "OperatorParams" }, // 0x15E: operator-specific parameter block
            { 200, "Generation" },     // 0xC8: C_Generation chunk
            { 123, "CurveKeys" },      // 0x7B: AnimParam keyframe-curve container
            { 124, "CurveKey" },       // 0x7C: a single keyframe
        };

        public static string Lookup(uint tag)
        {
            return Names.TryGetValue(tag, out string name) ? name : null;
        }
    }

    /// <summary>
    /// ue::gfx::effects::E_OperatorType, recovered from C_Operator::CreateOperator. The id is the
    /// first u32 inside an operator-type chunk (<see cref="EffectChunk.OperatorTypeTag"/>).
    /// </summary>
    public static class EffectsOperatorTypes
    {
        private static readonly string[] Names =
        {
            "Birth",            // 0
            "Position",         // 1
            "Speed",            // 2
            "Texture",          // 3
            "Rotation",         // 4
            "Scale",            // 5
            "Color",            // 6
            "Shape",            // 7
            "EmittingParticle", // 8
            "Acceleration",     // 9
            "Physics",          // 10
            "LOD",              // 11
            "Light",            // 12
            "ScaleNU",          // 13
            "MotionInheritance",// 14
            "ColorHSV",         // 15
            "Emissivity",       // 16
            "ShiftedTexture",   // 17
        };

        public static string Lookup(uint id)
        {
            return id < Names.Length ? Names[id] : null;
        }
    }

    /// <summary>A single parameter of an operator: either a scalar (floats) or an animated keyframe curve.</summary>
    public class EffectParamInfo
    {
        public uint Tag { get; set; }            // operator-specific parameter slot
        public string Name { get; set; }         // RE-inferred field name (may be null)
        public bool IsAnimated { get; set; }     // true => keyframe curve, false => scalar
        [Browsable(false)] public float[] Scalars { get; set; }     // when !IsAnimated
        [Browsable(false)] public List<float[]> Keys { get; set; }  // when IsAnimated: each = [time, value0, ...]
        public string Text { get; set; }         // embedded string (e.g. a resource name), if any

        // Absolute byte offsets into the edit buffer for in-place editing.
        [Browsable(false)] public int ScalarOffset { get; set; } = -1;          // first scalar float
        [Browsable(false)] public List<int> KeyOffsets { get; set; }            // start of each key's floats

        // For keyframe add/remove (only set when the displayed keys form one simple [123] curve).
        [Browsable(false)] public int CurveHeaderOffset { get; set; } = -1;     // header of the innermost [123]
        [Browsable(false)] public int KeyCountOffset { get; set; } = -1;        // u32 key count in the [123] body
        [Browsable(false)] public List<int> KeyChunkLengths { get; set; }       // full [124] chunk size per key
        [Browsable(false)] public bool MultiCurve { get; set; }                 // keys span >1 inner curve -> no add/remove

        [Browsable(false)]
        public bool CanAddRemoveKeys =>
            !MultiCurve && CurveHeaderOffset >= 0 && KeyCountOffset >= 0 && KeyChunkLengths != null && KeyOffsets != null;

        [Category("Parameter")]
        public int KeyCount => Keys?.Count ?? 0;

        [Category("Parameter")]
        public string Values
        {
            get
            {
                if (Scalars != null && Scalars.Length > 0)
                    return string.Join(", ", Array.ConvertAll(Scalars, f => f.ToString("0.######", CultureInfo.InvariantCulture)));
                if (Keys != null && Keys.Count > 0)
                {
                    string[] parts = new string[Keys.Count];
                    for (int i = 0; i < Keys.Count; i++)
                        parts[i] = "[" + string.Join("/", Array.ConvertAll(Keys[i], f => f.ToString("0.###", CultureInfo.InvariantCulture))) + "]";
                    return string.Join("  ", parts);
                }
                return string.Empty;
            }
        }

        [Category("Parameter")]
        public string Summary
        {
            get
            {
                if (!string.IsNullOrEmpty(Text)) return "\"" + Text + "\"";
                if (IsAnimated)
                {
                    int n = Keys?.Count ?? 0;
                    if (n == 0) return "curve (empty)";
                    float[] first = Keys[0];
                    string firstStr = string.Join("/", Array.ConvertAll(first, f => f.ToString("0.###", CultureInfo.InvariantCulture)));
                    return n == 1
                        ? "constant " + firstStr
                        : string.Format(CultureInfo.InvariantCulture, "curve, {0} keys (first {1})", n, firstStr);
                }
                if (Scalars != null && Scalars.Length > 0)
                {
                    return string.Join(", ", Array.ConvertAll(Scalars, f => f.ToString("0.###", CultureInfo.InvariantCulture)));
                }
                return "(empty)";
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrEmpty(Name)
                ? "param " + Tag.ToString(CultureInfo.InvariantCulture)
                : Name;
            return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", label, Summary);
        }
    }

    /// <summary>
    /// RE-inferred operator parameter slot names, keyed by E_OperatorType then param slot tag.
    /// Derived from each C_Operator*::LoadSpecific (member offsets + loader kind) and cross-checked
    /// against real effect data. Names are best-effort; unknown slots fall back to "param N".
    /// </summary>
    public static class EffectsOperatorParamNames
    {
        private static readonly Dictionary<uint, Dictionary<uint, string>> Names =
            new Dictionary<uint, Dictionary<uint, string>>
        {
            { 0,  new Dictionary<uint, string> { {0,"Rate"} } },                          // Birth
            { 1,  new Dictionary<uint, string> { {0,"WorldSpace"}, {1,"Space"} } },        // Position
            { 2,  new Dictionary<uint, string> { {0,"Speed"}, {5,"Direction"} } },         // Speed
            { 3,  new Dictionary<uint, string> { {8,"UV Rect"} } },                        // Texture
            { 4,  new Dictionary<uint, string> { {1,"Axis"}, {6,"Angle"} } },              // Rotation
            { 5,  new Dictionary<uint, string> { {0,"Size"}, {1,"Size 2"} } },             // Scale
            { 6,  new Dictionary<uint, string> { {0,"Color (RGB)"}, {1,"Alpha"} } },       // Color
            { 9,  new Dictionary<uint, string> { {0,"Acceleration"}, {1,"Magnitude"} } },  // Acceleration
            { 13, new Dictionary<uint, string> { {0,"Size X"}, {1,"Size Y"} } },           // ScaleNU
            { 15, new Dictionary<uint, string> { {0,"Alpha"}, {1,"Color (HSV)"} } },       // ColorHSV
        };

        public static string Lookup(uint operatorType, uint slot)
        {
            return Names.TryGetValue(operatorType, out var m) && m.TryGetValue(slot, out string n) ? n : null;
        }
    }

    /// <summary>One operator inside a generation (a tag-300 chunk; payload = u32 type, u8 enabled, params).</summary>
    public class EffectOperatorInfo
    {
        [Category("Operator")] public uint TypeId { get; set; }
        [Category("Operator")] public string Type { get; set; }
        [Category("Operator")] public bool Enabled { get; set; }
        [Category("Operator")] public int ParameterCount => Parameters?.Count ?? 0;
        [Browsable(false)] public List<EffectParamInfo> Parameters { get; set; } = new List<EffectParamInfo>();

        public override string ToString()
        {
            string name = Type ?? ("Operator(" + TypeId + ")");
            string en = Enabled ? "" : " (disabled)";
            return Parameters.Count > 0
                ? string.Format(CultureInfo.InvariantCulture, "{0}{1} — {2} params", name, en, Parameters.Count)
                : name + en;
        }
    }

    /// <summary>A named generation (emitter) inside an effect — a tag-200 chunk.</summary>
    public class EffectGenerationInfo
    {
        [Category("Generation")] public string Name { get; set; }
        [Category("Generation")] public int OperatorCount => Operators?.Count ?? 0;
        [Browsable(false)] public List<EffectOperatorInfo> Operators { get; set; } = new List<EffectOperatorInfo>();

        [Category("Generation")]
        public string OperatorList => string.Join(", ", Operators.ConvertAll(o => o.Type ?? o.TypeId.ToString()));

        public override string ToString()
        {
            string ops = Operators.Count == 0 ? "no operators"
                : string.Join(", ", Operators.ConvertAll(o => o.ToString()));
            return string.Format(CultureInfo.InvariantCulture, "Generation \"{0}\"  —  {1}",
                string.IsNullOrEmpty(Name) ? "(unnamed)" : Name, ops);
        }
    }

    /// <summary>A frame (emitter transform node) attached to an effect — a tag-100 chunk.</summary>
    public class EffectFrameInfo
    {
        public int Index;
        public uint ClassId;      // C_FrameClassFactory class id (3rd u32 of the frame header)
        public float[] Transform; // leading floats of the frame header (position/orientation)

        /// <summary>Best-effort frame-class name. 65 is the standard effect emitter frame (most common).</summary>
        public string TypeName
        {
            get
            {
                switch (ClassId)
                {
                    case 65: return "Emitter";
                    default: return "Frame(" + ClassId + ")";
                }
            }
        }

        public override string ToString()
        {
            if (Transform != null && Transform.Length >= 3)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} #{1}  pos=({2}, {3}, {4})",
                    TypeName, Index, Transform[0].ToString("0.###", CultureInfo.InvariantCulture),
                    Transform[1].ToString("0.###", CultureInfo.InvariantCulture),
                    Transform[2].ToString("0.###", CultureInfo.InvariantCulture));
            }
            return TypeName + " #" + Index;
        }
    }

    /// <summary>A sound element attached to an effect — a tag-400 chunk.</summary>
    public class EffectSoundInfo
    {
        public string Name;
        public override string ToString() => "Sound" + (string.IsNullOrEmpty(Name) ? "" : " \"" + Name + "\"");
    }

    /// <summary>An effect (pattern) — a tag-0 chunk under Patterns (668).</summary>
    public class EffectPatternInfo
    {
        [Category("Effect")] public uint Id { get; set; }
        [Category("Effect")] public int FrameCount => Frames?.Count ?? 0;
        [Category("Effect")] public int SoundCount => Sounds?.Count ?? 0;
        [Category("Effect")] public int GenerationCount => Generations?.Count ?? 0;
        [Browsable(false)] public List<EffectFrameInfo> Frames { get; set; } = new List<EffectFrameInfo>();
        [Browsable(false)] public List<EffectSoundInfo> Sounds { get; set; } = new List<EffectSoundInfo>();
        [Browsable(false)] public List<EffectGenerationInfo> Generations { get; set; } = new List<EffectGenerationInfo>();

        /// <summary>Best human-readable name: the first non-default generation name, else the index.</summary>
        [Category("Effect")]
        public string DisplayName
        {
            get
            {
                foreach (EffectGenerationInfo g in Generations)
                {
                    if (!string.IsNullOrEmpty(g.Name) && !g.Name.StartsWith("Generation", StringComparison.Ordinal))
                    {
                        return g.Name;
                    }
                }
                foreach (EffectGenerationInfo g in Generations)
                {
                    if (!string.IsNullOrEmpty(g.Name)) return g.Name;
                }
                return "Effect " + Id;
            }
        }

        /// <summary>Coarse classification derived from the operators present.</summary>
        public string Kind
        {
            get
            {
                HashSet<string> ops = new HashSet<string>();
                foreach (EffectGenerationInfo g in Generations)
                    foreach (EffectOperatorInfo o in g.Operators)
                        if (o.Type != null) ops.Add(o.Type);

                if (ops.Count == 0) return FrameCount > 0 ? "Frames only" : "Empty";

                List<string> traits = new List<string>();
                if (ops.Contains("Texture") || ops.Contains("ShiftedTexture")) traits.Add("Textured");
                if (ops.Contains("Color") || ops.Contains("ColorHSV")) traits.Add("Colored");
                if (ops.Contains("Light")) traits.Add("Light");
                if (ops.Contains("Physics")) traits.Add("Physics");
                bool particle = ops.Contains("Birth");

                string baseKind = particle ? "Particle" : "Effect";
                return traits.Count > 0 ? baseKind + " (" + string.Join(", ", traits) + ")" : baseKind;
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Effect #{0} \"{1}\"  [{2}]  — {3} gen, {4} frames",
                Id, DisplayName, Kind, Generations.Count, FrameCount);
        }
    }

    /// <summary>
    /// Schema-guided reader that turns a parsed .eff into a readable list of effects, generations
    /// and operator types. Unlike the generic chunk tree it follows the known layout
    /// (Patterns 668 -> Pattern[id] -> Generations 2 -> Generation 200{Name 0, Operators 3/4 -> 300}).
    /// </summary>
    public static class EffectsSummary
    {
        private const uint PatternsTag = 668;
        private const uint GenerationsTag = 2;
        private const uint FramesTag = 1;
        private const uint GenerationChunkTag = 200;
        private const uint FrameChunkTag = 100;
        private const uint FrameHeaderTag = 101;
        private const uint SoundChunkTag = 400; // 0x190
        private const uint NameTag = 0;
        private const uint OperatorChunkTag = 300;

        public static List<EffectPatternInfo> Parse(byte[] eff)
        {
            List<EffectPatternInfo> result = new List<EffectPatternInfo>();
            try
            {
                // root chunk -> find Patterns (668)
                foreach (var rootChild in Tile(eff, 8, eff.Length - 8))
                {
                    if (rootChild.Tag != PatternsTag) continue;

                    foreach (var pat in Tile(eff, rootChild.Start, rootChild.Length))
                    {
                        // pattern payload prefix = u32 version, u32 id
                        if (pat.Length < 8) continue;
                        EffectPatternInfo info = new EffectPatternInfo
                        {
                            Id = BitConverter.ToUInt32(eff, pat.Start + 4)
                        };

                        foreach (var sub in Tile(eff, pat.Start + 8, pat.Length - 8))
                        {
                            if (sub.Tag == FramesTag)
                            {
                                int fi = 0;
                                foreach (var fr in Tile(eff, sub.Start, sub.Length))
                                    if (fr.Tag == FrameChunkTag)
                                        info.Frames.Add(ParseFrame(eff, fr.Start, fr.Length, fi++));
                            }
                            else if (sub.Tag == GenerationsTag)
                            {
                                foreach (var gen in Tile(eff, sub.Start, sub.Length))
                                    if (gen.Tag == GenerationChunkTag)
                                        info.Generations.Add(ParseGeneration(eff, gen.Start, gen.Length));
                            }
                            else
                            {
                                // The pattern's "default" branch loads sound elements (tag-400 chunks).
                                foreach (var s in Tile(eff, sub.Start, sub.Length))
                                    if (s.Tag == SoundChunkTag)
                                        info.Sounds.Add(new EffectSoundInfo { Name = ExtractString(eff, s.Start, s.Length) });
                            }
                        }

                        result.Add(info);
                    }
                }
            }
            catch
            {
                // best-effort: return whatever parsed
            }
            return result;
        }

        private static EffectFrameInfo ParseFrame(byte[] eff, int start, int length, int index)
        {
            EffectFrameInfo frame = new EffectFrameInfo { Index = index };
            if (length >= 12) frame.ClassId = BitConverter.ToUInt32(eff, start + 8); // 3rd u32 = frame class id
            // The frame payload may carry a small raw prefix before its [101] header chunk.
            foreach (int pre in new[] { 0, 4, 8, 12, 16 })
            {
                if (pre > length) break;
                foreach (var sub in Tile(eff, start + pre, length - pre))
                {
                    if (sub.Tag == FrameHeaderTag)
                    {
                        frame.Transform = ReadFloats(eff, sub.Start, Math.Min(sub.Length, 48));
                        return frame;
                    }
                }
            }
            return frame;
        }

        private static EffectGenerationInfo ParseGeneration(byte[] eff, int start, int length)
        {
            EffectGenerationInfo gen = new EffectGenerationInfo();
            foreach (var sub in Tile(eff, start, length))
            {
                if (sub.Tag == NameTag)
                {
                    gen.Name = ExtractString(eff, sub.Start, sub.Length);
                }
                else // operators live under tag 3 or 4
                {
                    foreach (var op in Tile(eff, sub.Start, sub.Length))
                    {
                        if (op.Tag == OperatorChunkTag && op.Length >= 4)
                        {
                            gen.Operators.Add(ParseOperator(eff, op.Start, op.Length));
                        }
                    }
                }
            }
            return gen;
        }

        private const uint OperatorParamsTag = 350; // 0x15E
        private const uint CurveKeysTag = 123;       // 0x7B
        private const uint CurveKeyTag = 124;        // 0x7C

        /// <summary>Operator payload = u32 typeId, u8 enabled, then a [350] params block of scalar/curve params.</summary>
        private static EffectOperatorInfo ParseOperator(byte[] eff, int start, int length)
        {
            uint typeId = BitConverter.ToUInt32(eff, start);
            EffectOperatorInfo op = new EffectOperatorInfo
            {
                TypeId = typeId,
                Type = EffectsOperatorTypes.Lookup(typeId),
                Enabled = length >= 5 ? eff[start + 4] != 0 : true
            };

            // Sub-chunks start after the 5-byte (typeId + enabled) prefix.
            foreach (var sub in Tile(eff, start + 5, length - 5))
            {
                if (sub.Tag != OperatorParamsTag) continue; // [350]
                foreach (var p in Tile(eff, sub.Start, sub.Length))
                {
                    op.Parameters.Add(ParseParam(eff, typeId, p.Tag, p.Start, p.Length));
                }
            }
            return op;
        }

        private static EffectParamInfo ParseParam(byte[] eff, uint operatorType, uint tag, int start, int length)
        {
            EffectParamInfo param = new EffectParamInfo { Tag = tag, Name = EffectsOperatorParamNames.Lookup(operatorType, tag) };

            // Animated params carry an 8-byte prefix (mode u32==1, kind u32) then a [123] curve.
            // Detect by looking for a CurveKeys chunk after a 0/8-byte prefix.
            int curveStart, curveLen;
            if (FindCurve(eff, start, length, out curveStart, out curveLen))
            {
                param.IsAnimated = true;
                param.Keys = new List<float[]>();
                param.KeyOffsets = new List<int>();
                param.KeyChunkLengths = new List<int>();
                CollectKeys(eff, curveStart, curveLen, param);
                return param;
            }

            // Otherwise scalar: try a string first, else floats.
            string text = TryReadName(eff, start, length);
            if (text != null && text.Length >= 3)
            {
                param.Text = text;
            }
            else
            {
                param.Scalars = ReadFloats(eff, start, length);
                param.ScalarOffset = start;
            }
            return param;
        }

        /// <summary>Locate a [123] CurveKeys chunk inside a param payload (allowing the 8-byte AnimParam prefix).</summary>
        private static bool FindCurve(byte[] eff, int start, int length, out int cStart, out int cLen)
        {
            foreach (int pre in new[] { 0, 8, 4 })
            {
                if (pre > length) continue;
                foreach (var s in Tile(eff, start + pre, length - pre))
                {
                    if (s.Tag == CurveKeysTag) { cStart = s.Start; cLen = s.Length; return true; }
                }
            }
            cStart = 0; cLen = 0; return false;
        }

        /// <summary>Recursively gather innermost keyframes; each [124] leaf payload yields its leading floats as [time, value...].</summary>
        private static void CollectKeys(byte[] eff, int start, int length, EffectParamInfo param)
        {
            // [123] body = u32 keyCount prefix, then [124] keys.
            foreach (int pre in new[] { 4, 0 })
            {
                if (pre > length) continue;
                var spans = Tile(eff, start + pre, length - pre);
                if (spans.Count == 0) continue;

                bool leafCurveHere = false;
                foreach (var key in spans)
                {
                    if (key.Tag != CurveKeyTag) continue;

                    // A key may nest a deeper [123] (the 2nd curve dimension); recurse to the leaf.
                    int innerStart, innerLen;
                    if (FindCurve(eff, key.Start, key.Length, out innerStart, out innerLen))
                    {
                        CollectKeys(eff, innerStart, innerLen, param);
                    }
                    else
                    {
                        float[] f = ReadFloats(eff, key.Start, Math.Min(key.Length, 20)); // time + up to 4 values
                        if (f.Length > 0)
                        {
                            param.Keys.Add(f);
                            param.KeyOffsets.Add(key.Start);
                            param.KeyChunkLengths.Add(key.Length + HeaderSize); // full [124] chunk size
                            leafCurveHere = true;
                        }
                    }
                }

                if (leafCurveHere)
                {
                    // This [123] directly holds float keys -> it's the editable curve.
                    int curveHeader = start - HeaderSize;
                    if (param.CurveHeaderOffset < 0)
                    {
                        param.CurveHeaderOffset = curveHeader;
                        param.KeyCountOffset = (pre == 4) ? start : -1;
                    }
                    else if (param.CurveHeaderOffset != curveHeader)
                    {
                        param.MultiCurve = true; // keys span multiple curves -> disable add/remove
                    }
                }

                if (param.Keys.Count > 0) return;
            }
        }

        private const int HeaderSize = EffectChunk.HeaderSize;

        private static float[] ReadFloats(byte[] eff, int start, int length)
        {
            // skip a possible small chunk-prefix of zero bytes is not needed; just read whole range
            int count = length / 4;
            if (count <= 0) return Array.Empty<float>();
            float[] f = new float[count];
            for (int i = 0; i < count; i++) f[i] = BitConverter.ToSingle(eff, start + i * 4);
            return f;
        }

        private static string TryReadName(byte[] eff, int start, int length)
        {
            string s = ExtractString(eff, start, length);
            return string.IsNullOrEmpty(s) ? null : s;
        }

        private struct Span { public uint Tag; public int Start; public int Length; }

        /// <summary>Walk [offset, offset+len) as back-to-back chunks; yields header-stripped spans. Stops on any malformed chunk.</summary>
        private static List<Span> Tile(byte[] buf, int offset, int len)
        {
            List<Span> spans = new List<Span>();
            int i = offset;
            int end = offset + len;
            while (i + 8 <= end)
            {
                uint tag = BitConverter.ToUInt32(buf, i);
                uint size = BitConverter.ToUInt32(buf, i + 4);
                if (size < 8 || i + size > end) break;
                spans.Add(new Span { Tag = tag, Start = i + 8, Length = (int)size - 8 });
                i += (int)size;
            }
            return spans;
        }

        /// <summary>Names are stored as [u16 length][ascii]; fall back to the longest printable run.</summary>
        private static string ExtractString(byte[] buf, int start, int length)
        {
            if (length >= 2)
            {
                int slen = BitConverter.ToUInt16(buf, start);
                if (slen > 0 && slen <= length - 2)
                {
                    bool printable = true;
                    for (int j = 0; j < slen; j++)
                    {
                        byte b = buf[start + 2 + j];
                        if (b < 0x20 || b > 0x7e) { printable = false; break; }
                    }
                    if (printable) return System.Text.Encoding.ASCII.GetString(buf, start + 2, slen);
                }
            }

            // fallback: longest printable ASCII run
            string best = string.Empty;
            int runStart = -1;
            for (int j = 0; j <= length; j++)
            {
                bool ok = j < length && buf[start + j] >= 0x20 && buf[start + j] <= 0x7e;
                if (ok && runStart < 0) runStart = j;
                else if (!ok && runStart >= 0)
                {
                    if (j - runStart > best.Length)
                        best = System.Text.Encoding.ASCII.GetString(buf, start + runStart, j - runStart);
                    runStart = -1;
                }
            }
            return best;
        }
    }

    /// <summary>
    /// Top-level container for a ".eff" file. A real .eff is a single root chunk (tag 666) spanning
    /// the whole stream, but this supports multiple top-level chunks for robustness.
    /// </summary>
    public class EffectsFile
    {
        public List<EffectChunk> Roots { get; set; } = new List<EffectChunk>();

        /// <summary>
        /// Set to the verbatim original bytes when the parsed tree could not reproduce them
        /// byte-for-byte. When set, Save writes these bytes unchanged so a file is never corrupted.
        /// </summary>
        public byte[] RawFallback { get; private set; }

        public bool IsRawFallback => RawFallback != null;

        public void ReadFromFile(string path)
        {
            ReadFromBytes(File.ReadAllBytes(path));
        }

        public void ReadFromBytes(byte[] original)
        {
            Roots = new List<EffectChunk>();
            RawFallback = null;

            try
            {
                using (MemoryStream ms = new MemoryStream(original, false))
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    while (ms.Position < original.Length)
                    {
                        if (original.Length - ms.Position < EffectChunk.HeaderSize)
                        {
                            throw new InvalidDataException("Trailing bytes are smaller than a chunk header.");
                        }

                        Roots.Add(EffectChunk.Read(reader));
                    }
                }

                // Guarantee a lossless round-trip; otherwise treat the whole file as opaque.
                if (!BytesEqual(WriteToBytes(), original))
                {
                    RawFallback = original;
                    Roots = new List<EffectChunk>();
                }
            }
            catch
            {
                RawFallback = original;
                Roots = new List<EffectChunk>();
            }
        }

        // Mutable buffer used for in-place typed value edits (keyframes/scalars). The summary
        // records absolute offsets into this exact buffer so edits map back losslessly.
        private byte[] _editBuffer;

        /// <summary>True when a typed value edit (keyframe/scalar) has been applied in place.</summary>
        public bool TypedEdited { get; private set; }

        /// <summary>Readable list of effects (patterns) with their generation names and operator types.</summary>
        public List<EffectPatternInfo> GetSummary()
        {
            if (_editBuffer == null)
            {
                _editBuffer = BuildBytes();
            }
            return EffectsSummary.Parse(_editBuffer);
        }

        /// <summary>Patch a 32-bit float at an absolute offset (in-place, same size — round-trip safe).</summary>
        public void PatchFloat(int offset, float value)
        {
            if (_editBuffer == null)
            {
                _editBuffer = BuildBytes();
            }
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, _editBuffer, offset, 4);
            TypedEdited = true;
        }

        /// <summary>Insert a keyframe by duplicating key <paramref name="keyIndex"/> of a simple curve param.</summary>
        public bool AddKeyframe(EffectParamInfo p, int keyIndex)
        {
            if (p == null || !p.CanAddRemoveKeys) return false;
            if (keyIndex < 0 || keyIndex >= p.KeyOffsets.Count) return false;
            if (_editBuffer == null) _editBuffer = BuildBytes();
            int keyStart = p.KeyOffsets[keyIndex] - EffectChunk.HeaderSize;
            return Splice(p, keyStart, p.KeyChunkLengths[keyIndex], true);
        }

        /// <summary>Delete keyframe <paramref name="keyIndex"/> from a simple curve param (keeps at least one).</summary>
        public bool RemoveKeyframe(EffectParamInfo p, int keyIndex)
        {
            if (p == null || !p.CanAddRemoveKeys) return false;
            if (keyIndex < 0 || keyIndex >= p.KeyOffsets.Count || p.KeyOffsets.Count <= 1) return false;
            if (_editBuffer == null) _editBuffer = BuildBytes();
            int keyStart = p.KeyOffsets[keyIndex] - EffectChunk.HeaderSize;
            return Splice(p, keyStart, p.KeyChunkLengths[keyIndex], false);
        }

        // Insert (duplicate) or remove a [124] key chunk at keyChunkStart, fixing the [123] key count and
        // every enclosing chunk's size. All adjusted fields sit before the edit point, so offsets stay valid.
        private bool Splice(EffectParamInfo p, int keyChunkStart, int keyChunkLen, bool insert)
        {
            List<int> chain = WalkToOffset(_editBuffer, p.CurveHeaderOffset);
            if (chain == null) return false;

            byte[] nb;
            int delta;
            if (insert)
            {
                int insertPos = keyChunkStart + keyChunkLen;
                nb = new byte[_editBuffer.Length + keyChunkLen];
                Array.Copy(_editBuffer, 0, nb, 0, insertPos);
                Array.Copy(_editBuffer, keyChunkStart, nb, insertPos, keyChunkLen);     // duplicated key bytes
                Array.Copy(_editBuffer, insertPos, nb, insertPos + keyChunkLen, _editBuffer.Length - insertPos);
                delta = keyChunkLen;
            }
            else
            {
                nb = new byte[_editBuffer.Length - keyChunkLen];
                Array.Copy(_editBuffer, 0, nb, 0, keyChunkStart);
                Array.Copy(_editBuffer, keyChunkStart + keyChunkLen, nb, keyChunkStart, _editBuffer.Length - keyChunkStart - keyChunkLen);
                delta = -keyChunkLen;
            }

            if (p.KeyCountOffset >= 0)
            {
                uint kc = BitConverter.ToUInt32(nb, p.KeyCountOffset);
                Array.Copy(BitConverter.GetBytes((uint)(kc + (insert ? 1 : -1))), 0, nb, p.KeyCountOffset, 4);
            }

            foreach (int sizeOff in chain)
            {
                uint sz = BitConverter.ToUInt32(nb, sizeOff);
                Array.Copy(BitConverter.GetBytes((uint)(sz + delta)), 0, nb, sizeOff, 4);
            }

            _editBuffer = nb;
            TypedEdited = true;
            return true;
        }

        /// <summary>Walk from the buffer root down to the chunk whose header is at targetHeader; returns the
        /// size-field offsets of every chunk on that path (root..target inclusive).</summary>
        private static List<int> WalkToOffset(byte[] buf, int targetHeader)
        {
            List<int> chain = new List<int>();
            int pStart = 0, pEnd = buf.Length;
            int guard = 0;
            while (guard++ < 64)
            {
                int childHeader = FindChildContaining(buf, pStart, pEnd, targetHeader);
                if (childHeader < 0) return null;
                chain.Add(childHeader + 4);
                if (childHeader == targetHeader) return chain;
                uint size = BitConverter.ToUInt32(buf, childHeader + 4);
                pStart = childHeader + 8;
                pEnd = childHeader + (int)size;
            }
            return null;
        }

        // Prefix-aware: find the child chunk (within [pStart,pEnd)) whose byte range contains target.
        private static int FindChildContaining(byte[] buf, int pStart, int pEnd, int target)
        {
            foreach (int pre in new[] { 0, 4, 5, 8, 12, 16, 20, 24 })
            {
                int s = pStart + pre;
                if (s > pEnd) break;
                int i = s;
                bool ok = true;
                int found = -1;
                while (i < pEnd)
                {
                    if (pEnd - i < 8) { ok = false; break; }
                    uint sz = BitConverter.ToUInt32(buf, i + 4);
                    if (sz < 8 || i + (long)sz > pEnd) { ok = false; break; }
                    if (target >= i && target < i + (int)sz) found = i;
                    i += (int)sz;
                }
                if (ok && i == pEnd && found >= 0) return found;
            }
            return -1;
        }

        private byte[] BuildBytes()
        {
            if (RawFallback != null)
            {
                return (byte[])RawFallback.Clone();
            }

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                foreach (EffectChunk root in Roots)
                {
                    root.Write(writer);
                }

                return ms.ToArray();
            }
        }

        public byte[] WriteToBytes()
        {
            if (TypedEdited && _editBuffer != null)
            {
                return (byte[])_editBuffer.Clone();
            }
            return BuildBytes();
        }

        public void WriteToFile(string path)
        {
            File.WriteAllBytes(path, WriteToBytes());
        }

        public void ConvertToXML(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                OmitXmlDeclaration = false
            };

            using (XmlWriter xml = XmlWriter.Create(path, settings))
            {
                xml.WriteStartElement("Effects");

                if (RawFallback != null)
                {
                    // File could not be parsed losslessly; preserve it verbatim.
                    xml.WriteAttributeString("raw", Convert.ToHexString(RawFallback));
                }
                else
                {
                    foreach (EffectChunk root in Roots)
                    {
                        root.WriteXml(xml);
                    }
                }

                xml.WriteEndElement();
            }
        }

        public void ConvertFromXML(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);

            XmlNode root = document.DocumentElement;
            Roots = new List<EffectChunk>();
            RawFallback = null;

            if (root == null)
            {
                return;
            }

            XmlAttribute rawAttr = root.Attributes["raw"];
            if (rawAttr != null && !string.IsNullOrEmpty(rawAttr.Value))
            {
                RawFallback = Convert.FromHexString(rawAttr.Value);
                return;
            }

            foreach (XmlNode child in root.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element && child.Name == "Chunk")
                {
                    Roots.Add(EffectChunk.ReadXml(child));
                }
            }
        }

        private static bool BytesEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
