using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Utils.MathHelpers;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class NodeBase
    {
        [Browsable(false)]
        public HIRCObject Parent { get; set; }
        public int OverrideParentFX { get; set; }
        public byte UnkByte { get; set; }
        public List<FXChunk> FXChunks { get; set; }
        public byte OverrideAttachmentParams { get; set; }
        public uint OverrideBusID { get; set; }
        public uint DirectParentID { get; set; }
        public byte NodeFXBitVector { get; set; } //bit0 = "bPriorityOverrideParent", bit1 = "bPriorityApplyDistFactor", bit2 = "bOverrideMidiEventsBehavior", bit3 = "bOverrideMidiNoteTracking", bit4 = "bEnableMidiNoteTracking", bit5 = "bIsMidiBreakLoopOnNoteOff"
        public List<Prop> Props { get; set; }
        public List<RangedModifier> RangedModifiers { get; set; }
        public byte PositioningVector { get; set; } //bit0 = "bPositioningInfoOverrideParent", bit1 = "bHasListenerRelativeRouting", bit1 = "unknown2d", bit2 = "unknown2d", bit3 = "cbIs3DPositioningAvailable"
        public byte Bits3D { get; set; }
        public uint AttenuationID { get; set; }
        public byte PathMode { get; set; }
        public uint TransitionTime { get; set; }
        public List<Vertex> Vertices { get; set; }
        public List<PlaylistItem> PlaylistItems { get; set; }
        public List<AutomationParam> AutomationParams { get; set; }
        public byte AuxBitVector { get; set; } //Start of Aux Parameters section. bit2 = "bOverrideUserAuxSends", bit3 = "bHasAux", bit4 = "bOverrideReflectionsAuxBus"
        public List<uint> AuxParams { get; set; }
        public int ReflectionsAuxBus { get; set; }
        public byte AdvSettingsBitVector { get; set; } //Start of Advanced Settings Section. bit0 = "bKillNewest", bit1 = "bUseVirtualBehavior", bit3 = "bIgnoreParentMaxNumInst", bit4 = "bIsVVoicesOptOverrideParent"
        public byte VirtualQueueBehavior { get; set; } //0x01 = FromElapsedTime
        public int MaxInstanceCount { get; set; }
        public byte BelowThresholdBehavior { get; set; } //0x00 = ContinueToPlay
        public byte AdvSettingsEndBitVector { get; set; } //bit0 = "bOverrideHdrEnvelope", bit1 = "bOverrideAnalysis", bit2 = "bNormalizeLoudness", bit3 = "bEnableEnvelope"
        public List<StateChunk> StateChunks { get; set; }
        public List<RTPC> rtpc { get; set; }
        public int FeedbackBusID { get; set; }
        public NodeBase(BinaryReader br, HIRCObject ParentObject)
        {
            Parent = ParentObject;
            OverrideParentFX = br.ReadByte();
            int numFx = br.ReadByte();
            FXChunks = new List<FXChunk>();

            if (numFx != 0)
            {
                UnkByte = br.ReadByte();
            }

            for (int i = 0; i < numFx; i++)
            {
                byte index = br.ReadByte();
                uint ID = br.ReadUInt32();
                byte IsShareSet = br.ReadByte();
                byte IsRendered = br.ReadByte();
                FXChunks.Add(new FXChunk(index, ID, IsShareSet, IsRendered));
            }

            OverrideAttachmentParams = br.ReadByte();
            OverrideBusID = br.ReadUInt32();
            DirectParentID = br.ReadUInt32();
            NodeFXBitVector = br.ReadByte();
            Props = new List<Prop>();
            int PropsCount = br.ReadByte();

            for (int i = 0; i < PropsCount; i++)
            {
                int ID = br.ReadByte();
                Props.Add(new Prop(ID));
            }

            foreach (Prop prop in Props)
            {
                prop.Value = br.ReadUInt32();
            }

            RangedModifiers = new List<RangedModifier>();
            int RangedModifiersCount = br.ReadByte();

            for (int i = 0; i < RangedModifiersCount; i++)
            {
                byte ID = br.ReadByte();
                uint min = br.ReadUInt32();
                uint max = br.ReadUInt32();
                RangedModifiers.Add(new RangedModifier(ID, min, max));
            }

            PositioningVector = br.ReadByte();
            Vertices = new List<Vertex>();
            PlaylistItems = new List<PlaylistItem>();
            AutomationParams = new List<AutomationParam>();
            uint numPlayListItem = 0;
            uint VertexCount = 0;

            switch (PositioningVector)
            {
                case 3:
                case 7:
                    Bits3D = br.ReadByte();
                    break;

                case 201:
                case 217:
                case 219:
                case 223:
                    Bits3D = br.ReadByte();
                    AttenuationID = br.ReadUInt32();
                    break;

                case 35:
                case 39:
                case 67:
                case 71:
                    Bits3D = br.ReadByte();
                    PathMode = br.ReadByte();
                    TransitionTime = br.ReadUInt32();
                    VertexCount = br.ReadUInt32();

                    for (int i = 0; i < VertexCount; i++)
                    {
                        float xValue = br.ReadSingle();
                        float yValue = br.ReadSingle();
                        float zValue = br.ReadSingle();
                        uint Duration = br.ReadUInt32();
                        Vertices.Add(new Vertex(xValue, yValue, zValue, Duration));
                    }

                    numPlayListItem = br.ReadUInt32();

                    for (int i = 0; i < numPlayListItem; i++)
                    {
                        uint VertexOffset = br.ReadUInt32();
                        uint numPlaylistVertices = br.ReadUInt32();
                        PlaylistItems.Add(new PlaylistItem(VertexOffset, numPlaylistVertices));
                    }

                    for (int i = 0; i < numPlayListItem; i++)
                    {
                        float xValue = br.ReadSingle();
                        float yValue = br.ReadSingle();
                        float zValue = br.ReadSingle();
                        AutomationParams.Add(new AutomationParam(xValue, yValue, zValue));
                    }
                    break;

                case 89:
                case 121:
                case 249:
                    Bits3D = br.ReadByte();
                    AttenuationID = br.ReadUInt32();
                    PathMode = br.ReadByte();
                    TransitionTime = br.ReadUInt32();
                    VertexCount = br.ReadUInt32();

                    for (int i = 0; i < VertexCount; i++)
                    {
                        float xValue = br.ReadSingle();
                        float yValue = br.ReadSingle();
                        float zValue = br.ReadSingle();
                        uint Duration = br.ReadUInt32();
                        Vertices.Add(new Vertex(xValue, yValue, zValue, Duration));
                    }

                    numPlayListItem = br.ReadUInt32();

                    for (int i = 0; i < numPlayListItem; i++)
                    {
                        uint VertexOffset = br.ReadUInt32();
                        uint numPlaylistVertices = br.ReadUInt32();
                        PlaylistItems.Add(new PlaylistItem(VertexOffset, numPlaylistVertices));
                    }

                    for (int i = 0; i < numPlayListItem; i++)
                    {
                        float xValue = br.ReadSingle();
                        float yValue = br.ReadSingle();
                        float zValue = br.ReadSingle();
                        AutomationParams.Add(new AutomationParam(xValue, yValue, zValue));
                    }
                    break;

                case 0:
                case 1:
                case 5:
                case 192:
                case 199:
                case 195:
                    //Do nothing
                    break;
            }

            AuxBitVector = br.ReadByte();
            AuxParams = new List<uint>();

            if (MathHelpers.GetBit(AuxBitVector, 3))
            {
                AuxParams.Add(br.ReadUInt32());
                AuxParams.Add(br.ReadUInt32());
                AuxParams.Add(br.ReadUInt32());
                AuxParams.Add(br.ReadUInt32());
            }

            switch (PositioningVector)
            {
                case 0:
                case 1:
                case 3:
                case 5:
                case 7:
                case 35:
                case 39:
                case 67:
                case 71:
                    if (Parent.Bnk.Header.Version != 134)
                    {
                        ReflectionsAuxBus = br.ReadInt32();
                    }
                    break;
            }

            AdvSettingsBitVector = br.ReadByte();
            VirtualQueueBehavior = br.ReadByte();
            MaxInstanceCount = br.ReadUInt16();
            BelowThresholdBehavior = br.ReadByte();
            AdvSettingsEndBitVector = br.ReadByte();
            StateChunks = new List<StateChunk>();
            uint stateChunkCount = br.ReadUInt32();

            for (int i = 0; i < stateChunkCount; i++)
            {
                StateChunks.Add(new StateChunk(br, Parent));
            }

            rtpc = new List<RTPC>();
            int stateRTPCCount = br.ReadUInt16();

            for (int i = 0; i < stateRTPCCount; i++)
            {
                rtpc.Add(new RTPC(br));
            }

            switch (Parent.Bnk.Header.Feedback)
            {
                case 0:
                case 16:

                    break;

                default:
                    FeedbackBusID = br.ReadInt32();
                    break;
            }
        }

        public NodeBase(HIRCObject ParentObject)
        {
            Parent = ParentObject;
            OverrideParentFX = 0;
            FXChunks = new List<FXChunk>();
            UnkByte = 0;
            OverrideAttachmentParams = 0;
            OverrideBusID = 0;
            DirectParentID = 0;
            NodeFXBitVector = 0;
            Props = new List<Prop>();
            RangedModifiers = new List<RangedModifier>();
            PositioningVector = 0;
            Vertices = new List<Vertex>();
            PlaylistItems = new List<PlaylistItem>();
            AutomationParams = new List<AutomationParam>();
            Bits3D = 0;
            AttenuationID = 0;
            PathMode = 0;
            TransitionTime = 0;
            AuxBitVector = 0;
            AuxParams = new List<uint>();
            ReflectionsAuxBus = 0;
            AdvSettingsBitVector = 0;
            VirtualQueueBehavior = 0;
            MaxInstanceCount = 0;
            BelowThresholdBehavior = 0;
            AdvSettingsEndBitVector = 0;
            StateChunks = new List<StateChunk>();
            rtpc = new List<RTPC>();
            FeedbackBusID = 0;
        }

        public int GetLength()
        {
            int NodeBaseLength = 28 + FXChunks.Count * 7 + Props.Count * 5 + RangedModifiers.Count * 9;

            if (FXChunks.Count != 0)
            {
                NodeBaseLength += 1;
            }

            switch (PositioningVector)
            {
                case 3:
                case 7:
                    NodeBaseLength += 1;
                    break;

                case 201:
                case 217:
                case 219:
                case 223:
                    NodeBaseLength += 5;
                    break;

                case 35:
                case 39:
                case 67:
                case 71:
                    NodeBaseLength += 14 + Vertices.Count * 16 + PlaylistItems.Count * 8 + AutomationParams.Count * 12;
                    break;

                case 89:
                case 121:
                case 249:
                    NodeBaseLength += 18 + Vertices.Count * 16 + PlaylistItems.Count * 8 + AutomationParams.Count * 12;

                    break;

                case 0:
                case 1:
                case 5:
                case 192:
                case 199:
                case 195:
                    //Do nothing
                    break;

            }

            if (MathHelpers.GetBit(AuxBitVector, 3))
            {
                NodeBaseLength += 16;
            }

            switch (PositioningVector)
            {
                case 0:
                case 1:
                case 3:
                case 5:
                case 7:
                case 35:
                case 39:
                case 67:
                case 71:
                    if (Parent.Bnk.Header.Version != 134)
                    {
                        NodeBaseLength += 4;
                    }
                    break;
            }

            foreach (StateChunk chunk in StateChunks)
            {
                NodeBaseLength += chunk.GetLength();
            }

            foreach (RTPC value in rtpc)
            {
                NodeBaseLength += value.GetLength();
            }

            switch (Parent.Bnk.Header.Feedback)
            {
                case 0:
                case 16:

                    break;

                default:
                    NodeBaseLength += 4;
                    break;
            }

            return NodeBaseLength;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)OverrideParentFX);
            bw.Write((byte)FXChunks.Count);

            if (FXChunks.Count != 0)
            {
                bw.Write(UnkByte);
            }

            foreach (FXChunk chunk in FXChunks)
            {
                bw.Write((byte)chunk.Index);
                bw.Write(chunk.ID);
                bw.Write((byte)chunk.IsShareSet);
                bw.Write((byte)chunk.IsRendered);
            }

            bw.Write(OverrideAttachmentParams);
            bw.Write(OverrideBusID);
            bw.Write(DirectParentID);
            bw.Write(NodeFXBitVector);
            bw.Write((byte)Props.Count);

            foreach(Prop prop in Props)
            {
                bw.Write((byte)prop.ID);
            }

            foreach (Prop prop in Props)
            {
                bw.Write(prop.Value);
            }

            bw.Write((byte)RangedModifiers.Count);

            foreach (RangedModifier mod in RangedModifiers)
            {
                bw.Write(mod.ID);
                bw.Write(mod.Min);
                bw.Write(mod.Max);
            }

            bw.Write(PositioningVector);

            switch (PositioningVector)
            {
                case 3:
                case 7:
                    bw.Write(Bits3D);
                    break;

                case 201:
                case 217:
                case 219:
                case 223:
                    bw.Write(Bits3D);
                    bw.Write(AttenuationID);
                    break;

                case 35:
                case 39:
                case 67:
                case 71:
                    bw.Write(Bits3D);
                    bw.Write(PathMode);
                    bw.Write(TransitionTime);
                    bw.Write(Vertices.Count);

                    foreach (Vertex vert in Vertices)
                    {
                        bw.Write(vert.xValue);
                        bw.Write(vert.yValue);
                        bw.Write(vert.zValue);
                        bw.Write(vert.Duration);
                    }

                    bw.Write(PlaylistItems.Count);

                    foreach (PlaylistItem item in PlaylistItems)
                    {
                        bw.Write(item.VertexOffset);
                        bw.Write(item.VertexCount);
                    }

                    foreach (AutomationParam param in AutomationParams)
                    {
                        bw.Write(param.xRange);
                        bw.Write(param.yRange);
                        bw.Write(param.zRange);
                    }
                    break;

                case 89:
                case 121:
                case 249:
                    bw.Write(Bits3D);
                    bw.Write(AttenuationID);
                    bw.Write(PathMode);
                    bw.Write(TransitionTime);
                    bw.Write(Vertices.Count);

                    foreach (Vertex vert in Vertices)
                    {
                        bw.Write(vert.xValue);
                        bw.Write(vert.yValue);
                        bw.Write(vert.zValue);
                        bw.Write(vert.Duration);
                    }

                    bw.Write(PlaylistItems.Count);

                    foreach (PlaylistItem item in PlaylistItems)
                    {
                        bw.Write(item.VertexOffset);
                        bw.Write(item.VertexCount);
                    }

                    foreach (AutomationParam param in AutomationParams)
                    {
                        bw.Write(param.xRange);
                        bw.Write(param.yRange);
                        bw.Write(param.zRange);
                    }
                    break;
            }

            bw.Write(AuxBitVector);

            if (MathHelpers.GetBit(AuxBitVector, 3))
            {
                foreach (uint param in AuxParams)
                {
                    bw.Write(param);
                }
            }

            switch (PositioningVector)
            {
                case 0:
                case 1:
                case 3:
                case 5:
                case 7:
                case 35:
                case 39:
                case 67:
                case 71:
                    if (Parent.Bnk.Header.Version != 134)
                    {
                        bw.Write(ReflectionsAuxBus);
                    }
                    break;
            }

            bw.Write(AdvSettingsBitVector);
            bw.Write(VirtualQueueBehavior);
            bw.Write((short)MaxInstanceCount);
            bw.Write(BelowThresholdBehavior);
            bw.Write(AdvSettingsEndBitVector);
            bw.Write(StateChunks.Count);

            foreach (StateChunk chunk in StateChunks)
            {
                chunk.WriteToFile(bw);
            }

            bw.Write((short)rtpc.Count);

            foreach (RTPC value in rtpc)
            {
                value.WriteToFile(bw);
            }

            switch (Parent.Bnk.Header.Feedback)
            {
                case 0:
                case 16:

                    break;

                default:
                    bw.Write(FeedbackBusID);
                    break;
            }
        }
    }
}
