using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.CCDB
{
    /// <summary>
    /// Converter for CCDBHashName to allow editing in PropertyGrid
    /// </summary>
    public class CCDBHashNameConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(CCDBHashName) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
            {
                return new CCDBHashName();
            }

            CCDBHashName hashName = new CCDBHashName();

            // Try parsing as hex (0x...) or decimal
            if (stringValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (ulong.TryParse(stringValue.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong hash))
                {
                    hashName.Hash = hash;
                }
            }
            else if (ulong.TryParse(stringValue, out ulong hash))
            {
                hashName.Hash = hash;
            }

            return hashName;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is CCDBHashName hashName)
            {
                return hashName.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    /// <summary>
    /// Represents a 64-bit hash name (C_HashName in CCDB)
    /// </summary>
    [TypeConverter(typeof(CCDBHashNameConverter)), PropertyClassAllowReflection]
    public class CCDBHashName
    {
        [PropertyForceAsAttribute]
        public ulong Hash { get; set; }

        public CCDBHashName()
        {
            Hash = 0;
        }

        public CCDBHashName(ulong hash)
        {
            Hash = hash;
        }

        public static CCDBHashName FromXElement(XElement element)
        {
            CCDBHashName hashName = new CCDBHashName();
            string value = element.Value.Trim();

            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (ulong.TryParse(value.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong hash))
                {
                    hashName.Hash = hash;
                }
            }

            return hashName;
        }

        public XElement ToXElement(string elementName, string typeId = "C_HashName")
        {
            XElement element = new XElement(elementName);
            element.SetAttributeValue("TypeID", typeId);
            element.Value = $"0x{Hash:X16}";
            return element;
        }

        public override string ToString()
        {
            return $"0x{Hash:X16}";
        }

        public static CCDBHashName ReadFromBinary(BinaryReader reader)
        {
            return new CCDBHashName(reader.ReadUInt64());
        }

        public void WriteToBinary(BinaryWriter writer)
        {
            writer.Write(Hash);
        }
    }

    /// <summary>
    /// Represents a combinable piece (mesh part reference)
    /// Binary layout (from IDA analysis):
    /// - m_PieceId at offset 0 (8 bytes, C_HashName)
    /// - m_TargetPackageId at offset 8 (8 bytes, C_HashName)
    /// - m_TemplateId at offset 16 (8 bytes, C_HashName)
    /// Total size: 24 bytes
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBCombinablePiece
    {
        [Category("Identifiers"), Description("Piece identifier hash")]
        public CCDBHashName PieceId { get; set; } = new CCDBHashName();

        [Category("Identifiers"), Description("Target package identifier hash")]
        public CCDBHashName TargetPackageId { get; set; } = new CCDBHashName();

        [Category("Identifiers"), Description("Template identifier hash")]
        public CCDBHashName TemplateId { get; set; } = new CCDBHashName();

        public static CCDBCombinablePiece FromXElement(XElement element)
        {
            CCDBCombinablePiece piece = new CCDBCombinablePiece();

            XElement pieceIdElem = element.Element("m_PieceId");
            XElement targetPkgElem = element.Element("m_TargetPackageId");
            XElement templateElem = element.Element("m_TemplateId");

            if (pieceIdElem != null)
                piece.PieceId = CCDBHashName.FromXElement(pieceIdElem);
            if (targetPkgElem != null)
                piece.TargetPackageId = CCDBHashName.FromXElement(targetPkgElem);
            if (templateElem != null)
                piece.TemplateId = CCDBHashName.FromXElement(templateElem);

            return piece;
        }

        public XElement ToXElement()
        {
            XElement element = new XElement("C_CombinablePiece");
            element.SetAttributeValue("TypeID", "0x42964a0f");

            element.Add(PieceId.ToXElement("m_PieceId"));
            element.Add(TargetPackageId.ToXElement("m_TargetPackageId"));
            element.Add(TemplateId.ToXElement("m_TemplateId"));

            return element;
        }

        public override string ToString()
        {
            return $"Piece: {PieceId}";
        }

        public static CCDBCombinablePiece ReadFromBinary(BinaryReader reader)
        {
            CCDBCombinablePiece piece = new CCDBCombinablePiece();
            piece.PieceId = CCDBHashName.ReadFromBinary(reader);
            piece.TargetPackageId = CCDBHashName.ReadFromBinary(reader);
            piece.TemplateId = CCDBHashName.ReadFromBinary(reader);
            return piece;
        }

        public void WriteToBinary(BinaryWriter writer)
        {
            PieceId.WriteToBinary(writer);
            TargetPackageId.WriteToBinary(writer);
            TemplateId.WriteToBinary(writer);
        }

        public const int BinarySize = 24; // 3 x 8-byte hashes
    }

    /// <summary>
    /// Represents a piece set (C_PieceSet from binary)
    /// Binary layout (from IDA analysis of C_PieceSet at 0x100999a10):
    /// - Size: 64 bytes
    /// - m_Weight at offset 4 (uint)
    /// - m_ChanceToSpawn at offset 8 (float)
    /// - m_PieceIds at offset 16 (vector of C_HashName)
    ///
    /// Note: XML format uses C_Range structure which has different fields (m_Id, m_Ranges, m_RangeCount)
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBPieceSet
    {
        // Binary C_PieceSet fields (type hash 0x75DD667F)
        [Category("Binary Data"), Description("Weight value")]
        public uint Weight { get; set; }

        [Category("Binary Data"), Description("Chance to spawn (0.0-1.0)")]
        public float ChanceToSpawn { get; set; }

        [Browsable(false)]
        public List<CCDBHashName> PieceIds { get; set; } = new List<CCDBHashName>();

        [Category("Binary Data"), Description("Piece IDs (hash names)")]
        public CCDBHashName[] PieceIdsArray
        {
            get => PieceIds.ToArray();
            set => PieceIds = new List<CCDBHashName>(value ?? Array.Empty<CCDBHashName>());
        }

        // XML C_Range fields (for backwards compatibility with XML parsing)
        [Category("XML Data"), Description("Piece set identifier hash (from XML C_Range)")]
        public CCDBHashName Id { get; set; } = new CCDBHashName();

        [Browsable(false)]
        public List<uint> Ranges { get; set; } = new List<uint>();

        [Category("XML Data"), Description("Range indices (from XML C_Range)")]
        public uint[] RangesArray
        {
            get => Ranges.ToArray();
            set => Ranges = new List<uint>(value ?? Array.Empty<uint>());
        }

        [Category("XML Data"), Description("Range count (from XML C_Range)")]
        public uint RangeCount { get; set; }

        public const int BinarySize = 64;

        public static CCDBPieceSet FromXElement(XElement element)
        {
            CCDBPieceSet pieceSet = new CCDBPieceSet();

            XElement idElem = element.Element("m_Id");
            if (idElem != null)
            {
                pieceSet.Id = CCDBHashName.FromXElement(idElem);
            }

            XElement rangesItems = element.Element("m_Ranges")?.Element("Items");
            if (rangesItems != null)
            {
                foreach (XElement item in rangesItems.Elements("u32"))
                {
                    if (uint.TryParse(item.Value, out uint val))
                        pieceSet.Ranges.Add(val);
                }
            }

            XElement rangeCountElem = element.Element("m_RangeCount");
            if (rangeCountElem != null && uint.TryParse(rangeCountElem.Value, out uint count))
            {
                pieceSet.RangeCount = count;
            }

            return pieceSet;
        }

        public static CCDBPieceSet FromBinary(BinaryReader reader)
        {
            CCDBPieceSet pieceSet = new CCDBPieceSet();

            // Skip first 4 bytes (likely padding or base class)
            reader.ReadUInt32();

            pieceSet.Weight = reader.ReadUInt32();
            pieceSet.ChanceToSpawn = reader.ReadSingle();

            // Read vector<C_HashName> at offset 16
            // Vector format: [data_ptr:8][size:8][capacity:8]
            // For serialized data, this is typically: [count:4][hash values...]
            // But we need to handle the GENR vector format

            return pieceSet;
        }

        public override string ToString()
        {
            if (PieceIds.Count > 0)
                return $"PieceSet: Weight={Weight}, Chance={ChanceToSpawn:F2}, {PieceIds.Count} pieces";
            else if (Ranges.Count > 0 || Id.Hash != 0)
                return $"PieceSet: {Id} ({Ranges.Count} ranges)";
            else
                return "PieceSet (empty)";
        }
    }

    /// <summary>
    /// Represents a range with chance
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBRange
    {
        [Category("Data"), Description("Range minimum")]
        public float Min { get; set; }

        [Category("Data"), Description("Range maximum")]
        public float Max { get; set; }

        [Category("Data"), Description("Chance weight")]
        public float Chance { get; set; }

        public static CCDBRange FromXElement(XElement element)
        {
            CCDBRange range = new CCDBRange();

            XElement minElem = element.Element("m_Min");
            if (minElem != null && float.TryParse(minElem.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float min))
            {
                range.Min = min;
            }

            XElement maxElem = element.Element("m_Max");
            if (maxElem != null && float.TryParse(maxElem.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float max))
            {
                range.Max = max;
            }

            XElement chanceElem = element.Element("m_Chance");
            if (chanceElem != null && float.TryParse(chanceElem.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float chance))
            {
                range.Chance = chance;
            }

            return range;
        }

        public override string ToString()
        {
            return $"Range [{Min:F2} - {Max:F2}] @ {Chance:F2}";
        }
    }

    /// <summary>
    /// Represents a tag value
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBTagValue
    {
        [Category("Value"), Description("Tag value hash")]
        public CCDBHashName Value { get; set; } = new CCDBHashName();

        public static CCDBTagValue FromXElement(XElement element)
        {
            CCDBTagValue tagValue = new CCDBTagValue();

            XElement valueElem = element.Element("m_Value")?.Element("m_HashName");
            if (valueElem != null)
            {
                tagValue.Value = CCDBHashName.FromXElement(valueElem);
            }

            return tagValue;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// Represents a tag with multiple values
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBTag
    {
        [Category("Tag"), Description("Tag key hash")]
        public CCDBHashName Key { get; set; } = new CCDBHashName();

        [Browsable(false)]
        public List<CCDBTagValue> Values { get; set; } = new List<CCDBTagValue>();

        [Category("Tag"), Description("Tag values")]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public CCDBTagValue[] ValuesArray
        {
            get => Values.ToArray();
            set => Values = new List<CCDBTagValue>(value ?? Array.Empty<CCDBTagValue>());
        }

        public static CCDBTag FromXElement(XElement pairElement)
        {
            CCDBTag tag = new CCDBTag();

            XElement firstElem = pairElement.Element("First")?.Element("m_HashName");
            if (firstElem != null)
            {
                tag.Key = CCDBHashName.FromXElement(firstElem);
            }

            XElement secondItems = pairElement.Element("Second")?.Element("m_TagValues")?.Element("Items");
            if (secondItems != null)
            {
                foreach (XElement tagValueElem in secondItems.Elements("S_TagValue"))
                {
                    tag.Values.Add(CCDBTagValue.FromXElement(tagValueElem));
                }
            }

            return tag;
        }

        public override string ToString()
        {
            return $"{Key} ({Values.Count} values)";
        }
    }

    /// <summary>
    /// Represents a choice (customization option for spawn profiles)
    /// Binary layout (from IDA analysis):
    /// - m_PieceSets at offset 0 (array)
    /// - m_RangeIndexes at offset 24 (array)
    /// - m_Channels at offset 48 (array)
    /// - m_Tags at offset 72 (array)
    /// - m_Flags at offset 88 (uint)
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBChoice
    {
        [Category("Reference"), Description("Object reference ID")]
        public string Ref { get; set; } = "";

        // Internal storage as Lists
        [Browsable(false)]
        public List<uint> PieceSets { get; set; } = new List<uint>();

        [Browsable(false)]
        public List<ushort> RangeIndexes { get; set; } = new List<ushort>();

        [Browsable(false)]
        public List<uint> Channels { get; set; } = new List<uint>();

        [Browsable(false)]
        public List<CCDBTag> Tags { get; set; } = new List<CCDBTag>();

        // Expose as arrays for PropertyGrid display
        [Category("Data"), Description("Piece set indices")]
        public uint[] PieceSetsArray
        {
            get => PieceSets.ToArray();
            set => PieceSets = new List<uint>(value ?? Array.Empty<uint>());
        }

        [Category("Data"), Description("Range indices")]
        public ushort[] RangeIndexesArray
        {
            get => RangeIndexes.ToArray();
            set => RangeIndexes = new List<ushort>(value ?? Array.Empty<ushort>());
        }

        [Category("Data"), Description("Channel indices")]
        public uint[] ChannelsArray
        {
            get => Channels.ToArray();
            set => Channels = new List<uint>(value ?? Array.Empty<uint>());
        }

        [Category("Data"), Description("Tags")]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public CCDBTag[] TagsArray
        {
            get => Tags.ToArray();
            set => Tags = new List<CCDBTag>(value ?? Array.Empty<CCDBTag>());
        }

        [Category("Data"), Description("Flags")]
        public uint Flags { get; set; }

        public static CCDBChoice FromXElement(XElement element)
        {
            CCDBChoice choice = new CCDBChoice();

            choice.Ref = element.Attribute("Ref")?.Value ?? "";

            // Parse PieceSets
            XElement pieceSetsItems = element.Element("m_PieceSets")?.Element("Items");
            if (pieceSetsItems != null)
            {
                foreach (XElement item in pieceSetsItems.Elements("u32"))
                {
                    if (uint.TryParse(item.Value, out uint val))
                        choice.PieceSets.Add(val);
                }
            }

            // Parse RangeIndexes
            XElement rangeItems = element.Element("m_RangeIndexes")?.Element("Items");
            if (rangeItems != null)
            {
                foreach (XElement item in rangeItems.Elements("u16"))
                {
                    if (ushort.TryParse(item.Value, out ushort val))
                        choice.RangeIndexes.Add(val);
                }
            }

            // Parse Channels
            XElement channelItems = element.Element("m_Channels")?.Element("Items");
            if (channelItems != null)
            {
                foreach (XElement item in channelItems.Elements("u32"))
                {
                    if (uint.TryParse(item.Value, out uint val))
                        choice.Channels.Add(val);
                }
            }

            // Parse Tags
            XElement tagItems = element.Element("m_Tags")?.Element("Items");
            if (tagItems != null)
            {
                foreach (XElement pairElem in tagItems.Elements("pair"))
                {
                    choice.Tags.Add(CCDBTag.FromXElement(pairElem));
                }
            }

            // Parse Flags
            XElement flagsElem = element.Element("m_Flags");
            if (flagsElem != null && uint.TryParse(flagsElem.Value, out uint flags))
            {
                choice.Flags = flags;
            }

            return choice;
        }

        public override string ToString()
        {
            return $"Choice [{Ref}] - {PieceSets.Count} pieces, {Channels.Count} channels";
        }
    }

    /// <summary>
    /// Represents a spawn profile
    /// Binary layout (from IDA analysis):
    /// - m_Id at offset 0 (8 bytes, C_HashName)
    /// - m_Choices at offset 8 (array)
    /// - m_Packages at offset 32 (array)
    /// - m_Priority at offset 56 (int)
    /// - m_MinimumPackagesToLoad at offset 60 (int)
    /// - m_DesiredPackageMinimum at offset 64 (int)
    /// - m_Flags at offset 68 (uint)
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBSpawnProfile
    {
        [Category("Reference"), Description("Object reference ID")]
        public string Ref { get; set; } = "";

        [Category("Identifiers"), Description("Profile identifier hash")]
        public CCDBHashName Id { get; set; } = new CCDBHashName();

        // Internal storage as List for easier manipulation
        [Browsable(false)]
        public List<CCDBWeightedChoice> Choices { get; set; } = new List<CCDBWeightedChoice>();

        [Browsable(false)]
        public List<CCDBPackage> Packages { get; set; } = new List<CCDBPackage>();

        // Expose as arrays for PropertyGrid display
        [Category("Data"), Description("Weighted choices (index + weight) - references SharedChoices")]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public CCDBWeightedChoice[] ChoicesArray
        {
            get => Choices.ToArray();
            set => Choices = new List<CCDBWeightedChoice>(value ?? Array.Empty<CCDBWeightedChoice>());
        }

        [Category("Data"), Description("Package hashes - click to expand and view items")]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public CCDBPackage[] PackagesArray
        {
            get => Packages.ToArray();
            set => Packages = new List<CCDBPackage>(value ?? Array.Empty<CCDBPackage>());
        }

        [Category("Settings"), Description("Priority value")]
        public int Priority { get; set; } = 100;

        [Category("Settings"), Description("Minimum packages to load")]
        public uint MinimumPackagesToLoad { get; set; }

        [Category("Settings"), Description("Desired package minimum")]
        public uint DesiredPackageMinimum { get; set; }

        [Category("Settings"), Description("Flags")]
        public uint Flags { get; set; }

        public static CCDBSpawnProfile FromXElement(XElement element)
        {
            CCDBSpawnProfile profile = new CCDBSpawnProfile();

            profile.Ref = element.Attribute("Ref")?.Value ?? "";

            XElement idElem = element.Element("m_Id");
            if (idElem != null)
            {
                profile.Id = CCDBHashName.FromXElement(idElem);
            }

            // Parse Priority
            XElement priorityElem = element.Element("m_Priority");
            if (priorityElem != null && int.TryParse(priorityElem.Value, out int priority))
            {
                profile.Priority = priority;
            }

            // Parse MinimumPackagesToLoad
            XElement minPkgElem = element.Element("m_MinimumPackagesToLoad");
            if (minPkgElem != null && uint.TryParse(minPkgElem.Value, out uint minPkg))
            {
                profile.MinimumPackagesToLoad = minPkg;
            }

            // Parse DesiredPackageMinimum
            XElement desiredPkgElem = element.Element("m_DesiredPackageMinimum");
            if (desiredPkgElem != null && uint.TryParse(desiredPkgElem.Value, out uint desiredPkg))
            {
                profile.DesiredPackageMinimum = desiredPkg;
            }

            // Parse Flags
            XElement flagsElem = element.Element("m_Flags");
            if (flagsElem != null && uint.TryParse(flagsElem.Value, out uint flags))
            {
                profile.Flags = flags;
            }

            // Parse Choices - these are S_WeightedChoice elements containing index + weight
            XElement choicesElem = element.Element("m_Choices");
            if (choicesElem != null)
            {
                XElement choiceItems = choicesElem.Element("Items");
                if (choiceItems != null)
                {
                    // Parse S_WeightedChoice elements (most common format)
                    foreach (XElement wcElem in choiceItems.Elements("S_WeightedChoice"))
                    {
                        profile.Choices.Add(CCDBWeightedChoice.FromXElement(wcElem));
                    }
                    // Also try u32 elements (direct choice indices without weight)
                    foreach (XElement indexElem in choiceItems.Elements("u32"))
                    {
                        if (uint.TryParse(indexElem.Value, out uint idx))
                        {
                            CCDBWeightedChoice wc = new CCDBWeightedChoice();
                            wc.ChoiceIndex = idx;
                            wc.Weight = 1.0f;
                            profile.Choices.Add(wc);
                        }
                    }
                }
            }

            // Parse Packages - these are C_HashName elements (package ID hashes)
            XElement packagesElem = element.Element("m_Packages");
            if (packagesElem != null)
            {
                XElement packageItems = packagesElem.Element("Items");
                if (packageItems != null)
                {
                    // Parse C_HashName elements (package hashes)
                    foreach (XElement pkgElem in packageItems.Elements("C_HashName"))
                    {
                        CCDBPackage pkg = new CCDBPackage();
                        pkg.Id = CCDBHashName.FromXElement(pkgElem);
                        profile.Packages.Add(pkg);
                    }
                }
            }

            return profile;
        }

        public override string ToString()
        {
            return $"Profile: {Id} (Priority: {Priority}, {Choices.Count} choices, {Packages.Count} packages)";
        }
    }

    /// <summary>
    /// Represents a weighted choice reference (index + weight)
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBWeightedChoice
    {
        [Category("Choice"), Description("Index into SharedChoices array")]
        public uint ChoiceIndex { get; set; }

        [Category("Choice"), Description("Weight for random selection")]
        public float Weight { get; set; } = 1.0f;

        public static CCDBWeightedChoice FromXElement(XElement element)
        {
            CCDBWeightedChoice wc = new CCDBWeightedChoice();

            XElement indexElem = element.Element("m_ChoiceIndex");
            if (indexElem != null && uint.TryParse(indexElem.Value, out uint idx))
            {
                wc.ChoiceIndex = idx;
            }

            XElement weightElem = element.Element("m_Weight");
            if (weightElem != null && float.TryParse(weightElem.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float weight))
            {
                wc.Weight = weight;
            }

            return wc;
        }

        public override string ToString()
        {
            return $"Choice[{ChoiceIndex}] Weight: {Weight:F2}";
        }
    }

    /// <summary>
    /// Represents a package reference in a spawn profile
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBPackage
    {
        [Category("Package"), Description("Package identifier hash")]
        public CCDBHashName Id { get; set; } = new CCDBHashName();

        [Category("Package"), Description("Package string name")]
        public string Name { get; set; } = "";

        public static CCDBPackage FromXElement(XElement element)
        {
            CCDBPackage package = new CCDBPackage();

            // Try to parse as hash name
            XElement idElem = element.Element("m_Id");
            if (idElem != null)
            {
                package.Id = CCDBHashName.FromXElement(idElem);
            }
            else
            {
                // May be a direct hash value
                package.Id = CCDBHashName.FromXElement(element);
            }

            XElement nameElem = element.Element("m_Name");
            if (nameElem != null)
            {
                package.Name = nameElem.Value;
            }

            return package;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return $"Package: {Name}";
            return $"Package: {Id}";
        }
    }

    /// <summary>
    /// Represents a piece set mapping (combinable ID to piece set index)
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBPieceSetMapping
    {
        [Category("Mapping"), Description("Combinable ID hash")]
        public CCDBHashName CombinableId { get; set; } = new CCDBHashName();

        [Category("Mapping"), Description("Piece set index")]
        public uint PieceSetIndex { get; set; }

        public static CCDBPieceSetMapping FromXElement(XElement pairElement)
        {
            CCDBPieceSetMapping mapping = new CCDBPieceSetMapping();

            XElement firstElem = pairElement.Element("First");
            if (firstElem != null)
            {
                mapping.CombinableId = CCDBHashName.FromXElement(firstElem);
            }

            XElement secondElem = pairElement.Element("Second");
            if (secondElem != null && uint.TryParse(secondElem.Value, out uint index))
            {
                mapping.PieceSetIndex = index;
            }

            return mapping;
        }

        public override string ToString()
        {
            return $"{CombinableId} -> {PieceSetIndex}";
        }
    }
}
