using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise;
using Utils.MathHelpers;

namespace ResourceTypes.Wwise.Helpers
{
    public class NodeBase
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        public int overrideParentFX { get; set; }
        public byte unkByte { get; set; }
        public List<FXChunk> fxChunks { get; set; }
        public byte overrideAttachmentParams { get; set; }
        public uint overrideBusId { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public uint directParentId { get; set; }
        public byte nodeFXBitVector { get; set; } //bit0 = "bPriorityOverrideParent", bit1 = "bPriorityApplyDistFactor", bit2 = "bOverrideMidiEventsBehavior", bit3 = "bOverrideMidiNoteTracking", bit4 = "bEnableMidiNoteTracking", bit5 = "bIsMidiBreakLoopOnNoteOff"
        public List<Prop> props { get; set; }
        public List<RangedModifier> rangedModifiers { get; set; }
        public byte posByVector { get; set; } //bit0 = "bPositioningInfoOverrideParent", bit1 = "bHasListenerRelativeRouting", bit1 = "unknown2d", bit2 = "unknown2d", bit3 = "cbIs3DPositioningAvailable"
        public byte bits3d { get; set; }
        public uint attenuationId { get; set; }
        public byte pathMode { get; set; }
        public uint transitionTime { get; set; }
        public List<Vertex> vertices { get; set; }
        public List<PlaylistItem> playListItems { get; set; }
        public List<AutomationParam> automationParams { get; set; }
        public byte auxBitVector { get; set; } //Start of Aux Parameters section. bit2 = "bOverrideUserAuxSends", bit3 = "bHasAux", bit4 = "bOverrideReflectionsAuxBus"
        public List<uint> auxParams { get; set; }
        public int reflectionsAuxBus { get; set; }
        public byte advSettingsBitVector { get; set; } //Start of Advanced Settings Section. bit0 = "bKillNewest", bit1 = "bUseVirtualBehavior", bit3 = "bIgnoreParentMaxNumInst", bit4 = "bIsVVoicesOptOverrideParent"
        public byte virtualQueueBehavior { get; set; } //0x01 = FromElapsedTime
        public int maxNumInstance { get; set; }
        public byte belowThresholdBehavior { get; set; } //0x00 = ContinueToPlay
        public byte advSettingsEndBitVector { get; set; } //bit0 = "bOverrideHdrEnvelope", bit1 = "bOverrideAnalysis", bit2 = "bNormalizeLoudness", bit3 = "bEnableEnvelope"
        public List<StateChunk> stateChunks { get; set; }
        public List<StateProp> stateProps { get; set; }
        public List<RTPC> rtpc { get; set; }
        public int feedbackBusId { get; set; }
        public NodeBase(BinaryReader br, HIRCObject parentObject)
        {
            parent = parentObject;
            overrideParentFX = br.ReadByte();
            int numFx = br.ReadByte();
            fxChunks = new List<FXChunk>();

            if (numFx != 0)
            {
                unkByte = br.ReadByte();
            }

            for (int i = 0; i < numFx; i++)
            {
                byte index = br.ReadByte();
                uint id = br.ReadUInt32();
                byte isShareSet = br.ReadByte();
                byte isRendered = br.ReadByte();
                fxChunks.Add(new FXChunk(index, id, isShareSet, isRendered));
            }

            overrideAttachmentParams = br.ReadByte();
            overrideBusId = br.ReadUInt32();
            directParentId = br.ReadUInt32();
            nodeFXBitVector = br.ReadByte();
            props = new List<Prop>();
            int propsCount = br.ReadByte();

            for (int i = 0; i < propsCount; i++)
            {
                int id = br.ReadByte();
                props.Add(new Prop(id));
            }

            foreach (Prop prop in props)
            {
                prop.value = br.ReadUInt32();
            }

            rangedModifiers = new List<RangedModifier>();
            int rangedModifiersCount = br.ReadByte();

            for (int i = 0; i < rangedModifiersCount; i++)
            {
                byte id = br.ReadByte();
                uint min = br.ReadUInt32();
                uint max = br.ReadUInt32();
                rangedModifiers.Add(new RangedModifier(id, min, max));
            }

            posByVector = br.ReadByte();
            vertices = new List<Vertex>();
            playListItems = new List<PlaylistItem>();
            automationParams = new List<AutomationParam>();
            uint numPlayListItem = 0;
            uint numVertices = 0;

            switch (posByVector)
            {
                case 3:
                case 7:
                    bits3d = br.ReadByte();
                    break;

                case 201:
                case 217:
                case 219:
                case 223:
                    bits3d = br.ReadByte();
                    attenuationId = br.ReadUInt32();
                    break;

                case 35:
                case 39:
                case 67:
                case 71:
                    bits3d = br.ReadByte();
                    pathMode = br.ReadByte();
                    transitionTime = br.ReadUInt32();
                    numVertices = br.ReadUInt32();

                    for (int i = 0; i < numVertices; i++)
                    {
                        float xValue = br.ReadSingle();
                        float yValue = br.ReadSingle();
                        float zValue = br.ReadSingle();
                        uint duration = br.ReadUInt32();
                        vertices.Add(new Vertex(xValue, yValue, zValue, duration));
                    }

                    numPlayListItem = br.ReadUInt32();

                    for (int i = 0; i < numPlayListItem; i++)
                    {
                        uint vertexOffset = br.ReadUInt32();
                        uint numPlaylistVertices = br.ReadUInt32();
                        playListItems.Add(new PlaylistItem(vertexOffset, numPlaylistVertices));
                    }

                    for (int i = 0; i < numPlayListItem; i++)
                    {
                        float xValue = br.ReadSingle();
                        float yValue = br.ReadSingle();
                        float zValue = br.ReadSingle();
                        automationParams.Add(new AutomationParam(xValue, yValue, zValue));
                    }
                    break;

                case 89:
                case 121:
                case 249:
                    bits3d = br.ReadByte();
                    attenuationId = br.ReadUInt32();
                    pathMode = br.ReadByte();
                    transitionTime = br.ReadUInt32();
                    numVertices = br.ReadUInt32();

                    for (int i = 0; i < numVertices; i++)
                    {
                        float xValue = br.ReadSingle();
                        float yValue = br.ReadSingle();
                        float zValue = br.ReadSingle();
                        uint duration = br.ReadUInt32();
                        vertices.Add(new Vertex(xValue, yValue, zValue, duration));
                    }

                    numPlayListItem = br.ReadUInt32();

                    for (int i = 0; i < numPlayListItem; i++)
                    {
                        uint vertexOffset = br.ReadUInt32();
                        uint numPlaylistVertices = br.ReadUInt32();
                        playListItems.Add(new PlaylistItem(vertexOffset, numPlaylistVertices));
                    }

                    for (int i = 0; i < numPlayListItem; i++)
                    {
                        float xValue = br.ReadSingle();
                        float yValue = br.ReadSingle();
                        float zValue = br.ReadSingle();
                        automationParams.Add(new AutomationParam(xValue, yValue, zValue));
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

            auxBitVector = br.ReadByte();
            auxParams = new List<uint>();

            if (MathHelpers.GetBit(auxBitVector, 3))
            {
                auxParams.Add(br.ReadUInt32());
                auxParams.Add(br.ReadUInt32());
                auxParams.Add(br.ReadUInt32());
                auxParams.Add(br.ReadUInt32());
            }

            switch (posByVector)
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
                    if (parent.bnk.Header.Version != 134)
                    {
                        reflectionsAuxBus = br.ReadInt32();
                    }
                    break;
            }

            advSettingsBitVector = br.ReadByte();
            virtualQueueBehavior = br.ReadByte();
            maxNumInstance = br.ReadUInt16();
            belowThresholdBehavior = br.ReadByte();
            advSettingsEndBitVector = br.ReadByte();
            stateChunks = new List<StateChunk>();
            stateProps = new List<StateProp>();
            uint stateChunkCount = br.ReadUInt32();

            for (int i = 0; i < stateChunkCount; i++)
            {
                stateChunks.Add(new StateChunk(br, parent));
            }

            rtpc = new List<RTPC>();
            int stateRTPCCount = br.ReadUInt16();

            for (int i = 0; i < stateRTPCCount; i++)
            {
                rtpc.Add(new RTPC(br));
            }

            switch (parent.bnk.Header.Feedback)
            {
                case 0:
                case 16:

                    break;

                default:
                    feedbackBusId = br.ReadInt32();
                    break;
            }
        }

        public NodeBase(HIRCObject parentObject)
        {
            parent = parentObject;
            overrideParentFX = 0;
            fxChunks = new List<FXChunk>();
            unkByte = 0;
            overrideAttachmentParams = 0;
            overrideBusId = 0;
            directParentId = 0;
            nodeFXBitVector = 0;
            props = new List<Prop>();
            rangedModifiers = new List<RangedModifier>();
            posByVector = 0;
            vertices = new List<Vertex>();
            playListItems = new List<PlaylistItem>();
            automationParams = new List<AutomationParam>();
            bits3d = 0;
            attenuationId = 0;
            pathMode = 0;
            transitionTime = 0;
            auxBitVector = 0;
            auxParams = new List<uint>();
            reflectionsAuxBus = 0;
            advSettingsBitVector = 0;
            virtualQueueBehavior = 0;
            maxNumInstance = 0;
            belowThresholdBehavior = 0;
            advSettingsEndBitVector = 0;
            stateChunks = new List<StateChunk>();
            rtpc = new List<RTPC>();
            feedbackBusId = 0;
            stateProps = new List<StateProp>();
        }

        public int GetLength()
        {
            int nodeBaseLength = 28 + fxChunks.Count * 7 + props.Count * 5 + rangedModifiers.Count * 9;

            if (fxChunks.Count != 0)
            {
                nodeBaseLength += 1;
            }

            switch (posByVector)
            {
                case 3:
                case 7:
                    nodeBaseLength += 1;
                    break;

                case 201:
                case 217:
                case 219:
                case 223:
                    nodeBaseLength += 5;
                    break;

                case 35:
                case 39:
                case 67:
                case 71:
                    nodeBaseLength += 14 + vertices.Count * 16 + playListItems.Count * 8 + automationParams.Count * 12;
                    break;

                case 89:
                case 121:
                case 249:
                    nodeBaseLength += 18 + vertices.Count * 16 + playListItems.Count * 8 + automationParams.Count * 12;

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

            if (MathHelpers.GetBit(auxBitVector, 3))
            {
                nodeBaseLength += 16;
            }

            switch (posByVector)
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
                    if (parent.bnk.Header.Version != 134)
                    {
                        nodeBaseLength += 4;
                    }
                    break;
            }

            foreach (StateChunk chunk in stateChunks)
            {
                nodeBaseLength += chunk.GetLength();
            }

            foreach (RTPC value in rtpc)
            {
                nodeBaseLength += value.GetLength();
            }

            switch (parent.bnk.Header.Feedback)
            {
                case 0:
                case 16:

                    break;

                default:
                    nodeBaseLength += 4;
                    break;
            }

            return nodeBaseLength;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)overrideParentFX);
            bw.Write((byte)fxChunks.Count);

            if (fxChunks.Count != 0)
            {
                bw.Write(unkByte);
            }

            foreach (FXChunk chunk in fxChunks)
            {
                bw.Write((byte)chunk.index);
                bw.Write(chunk.id);
                bw.Write((byte)chunk.isShareSet);
                bw.Write((byte)chunk.isRendered);
            }

            bw.Write(overrideAttachmentParams);
            bw.Write(overrideBusId);
            bw.Write(directParentId);
            bw.Write(nodeFXBitVector);
            bw.Write((byte)props.Count);

            foreach(Prop prop in props)
            {
                bw.Write((byte)prop.id);
            }

            foreach (Prop prop in props)
            {
                bw.Write(prop.value);
            }

            bw.Write((byte)rangedModifiers.Count);

            foreach (RangedModifier mod in rangedModifiers)
            {
                bw.Write(mod.id);
                bw.Write(mod.min);
                bw.Write(mod.max);
            }

            bw.Write(posByVector);

            switch (posByVector)
            {
                case 3:
                case 7:
                    bw.Write(bits3d);
                    break;

                case 201:
                case 217:
                case 219:
                case 223:
                    bw.Write(bits3d);
                    bw.Write(attenuationId);
                    break;

                case 35:
                case 39:
                case 67:
                case 71:
                    bw.Write(bits3d);
                    bw.Write(pathMode);
                    bw.Write(transitionTime);
                    bw.Write(vertices.Count);

                    foreach (Vertex vert in vertices)
                    {
                        bw.Write(vert.xValue);
                        bw.Write(vert.yValue);
                        bw.Write(vert.zValue);
                        bw.Write(vert.duration);
                    }

                    bw.Write(playListItems.Count);

                    foreach (PlaylistItem item in playListItems)
                    {
                        bw.Write(item.vertexOffset);
                        bw.Write(item.numVertices);
                    }

                    foreach (AutomationParam param in automationParams)
                    {
                        bw.Write(param.xRange);
                        bw.Write(param.yRange);
                        bw.Write(param.zRange);
                    }
                    break;

                case 89:
                case 121:
                case 249:
                    bw.Write(bits3d);
                    bw.Write(attenuationId);
                    bw.Write(pathMode);
                    bw.Write(transitionTime);
                    bw.Write(vertices.Count);

                    foreach (Vertex vert in vertices)
                    {
                        bw.Write(vert.xValue);
                        bw.Write(vert.yValue);
                        bw.Write(vert.zValue);
                        bw.Write(vert.duration);
                    }

                    bw.Write(playListItems.Count);

                    foreach (PlaylistItem item in playListItems)
                    {
                        bw.Write(item.vertexOffset);
                        bw.Write(item.numVertices);
                    }

                    foreach (AutomationParam param in automationParams)
                    {
                        bw.Write(param.xRange);
                        bw.Write(param.yRange);
                        bw.Write(param.zRange);
                    }
                    break;
            }

            bw.Write(auxBitVector);

            if (MathHelpers.GetBit(auxBitVector, 3))
            {
                foreach (uint param in auxParams)
                {
                    bw.Write(param);
                }
            }

            switch (posByVector)
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
                    if (parent.bnk.Header.Version != 134)
                    {
                        bw.Write(reflectionsAuxBus);
                    }
                    break;
            }

            bw.Write(advSettingsBitVector);
            bw.Write(virtualQueueBehavior);
            bw.Write((short)maxNumInstance);
            bw.Write(belowThresholdBehavior);
            bw.Write(advSettingsEndBitVector);
            bw.Write(stateChunks.Count);

            foreach (StateChunk chunk in stateChunks)
            {
                chunk.WriteToFile(bw);
            }

            bw.Write((short)rtpc.Count);

            foreach (RTPC value in rtpc)
            {
                value.WriteToFile(bw);
            }

            switch (parent.bnk.Header.Feedback)
            {
                case 0:
                case 16:

                    break;

                default:
                    bw.Write(feedbackBusId);
                    break;
            }
        }
    }
}
