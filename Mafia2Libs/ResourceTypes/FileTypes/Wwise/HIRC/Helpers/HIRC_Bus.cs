using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise.Helpers;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class Bus
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        public uint overrideBusId { get; set; }
        public List<Prop> props { get; set; }
        public byte busInitialBitVector1 { get; set; } //bit0 = bMainOutputHierarchy, bit1 = bIsBackgroundMusic
        public byte busInitialBitVector2 { get; set; }  //bit0 = bKillNewest, bit1 = bUseVirtualBehavior
        public uint maxNumInstance { get; set; }
        public uint channelConfig { get; set; } //bit0 = uNumChannels, bit8 = eConfigType, bit12 = uChannelMask
        public byte busInitialBitVector3 { get; set; } //bit0 = bIsHdrBus, bit1 = bHdrReleaseModeExponential
        public uint recoveryTime { get; set; }
        public float maxDuckVolume { get; set; }
        public List<Duck> ducks { get; set; }
        public byte bitsFxBypass { get; set; }
        public List<FXChunk> fxChunks { get; set; }
        public uint fxId { get; set; }
        public int isShareSet { get; set; }
        public byte overrideAttachmentParams { get; set; }
        public List<RTPC> rtpc { get; set; }
        public List<StateChunk> stateChunks { get; set; }
        public List<StateProp> stateProps { get; set; }
        public Bus(HIRCObject parentObject, BinaryReader br, uint length)
        {
            parent = parentObject;
            overrideBusId = br.ReadUInt32();
            props = new List<Prop>();
            int propsCount = br.ReadByte();

            for (int i = 0; i < propsCount; i++)
            {
                byte key = br.ReadByte();
                props.Add(new Prop(key));
            }

            foreach (Prop prop in props)
            {
                prop.value = br.ReadUInt32();
            }

            busInitialBitVector1 = br.ReadByte();
            busInitialBitVector2 = br.ReadByte();

            maxNumInstance = br.ReadUInt16();
            channelConfig = br.ReadUInt32();
            busInitialBitVector3 = br.ReadByte();
            recoveryTime = br.ReadUInt32();
            maxDuckVolume = br.ReadSingle();
            ducks = new List<Duck>();
            uint ducksCount = br.ReadUInt32();

            for (int i = 0; i < ducksCount; i++)
            {
                ducks.Add(new Duck(br));
            }

            int numFx = br.ReadByte();
            fxChunks = new List<FXChunk>();

            if (numFx != 0)
            {
                bitsFxBypass = br.ReadByte();
            }

            for (int i = 0; i < numFx; i++)
            {
                byte Index = br.ReadByte();
                uint Id = br.ReadUInt32();
                byte bIsShareSet = br.ReadByte();
                byte bIsRendered = br.ReadByte();
                fxChunks.Add(new FXChunk(Index, Id, bIsShareSet, bIsRendered));
            }

            fxId = br.ReadUInt32();
            isShareSet = br.ReadByte();
            overrideAttachmentParams = br.ReadByte();
            uint rtpcCount = br.ReadUInt16();
            rtpc = new List<RTPC>();

            for (int i = 0; i < rtpcCount; i++)
            {
                rtpc.Add(new RTPC(br));
            }

            stateChunks = new List<StateChunk>();
            stateProps = new List<StateProp>();

            uint stateChunkCount = br.ReadUInt32();

            for (int i = 0; i < stateChunkCount; i++)
            {
                stateChunks.Add(new StateChunk(br, parent));
            }
        }

        public Bus(HIRCObject parentObject)
        {
            parent = parentObject;
            overrideBusId = 0;
            props = new List<Prop>();
            busInitialBitVector1 = 0;
            busInitialBitVector2 = 0;
            maxNumInstance = 0;
            channelConfig = 0;
            busInitialBitVector3 = 0;
            recoveryTime = 0;
            maxDuckVolume = 0;
            ducks = new List<Duck>();
            bitsFxBypass = 0;
            fxChunks = new List<FXChunk>();
            fxId = 0;
            isShareSet = 0;
            overrideAttachmentParams = 0;
            rtpc = new List<RTPC>();
            stateChunks = new List<StateChunk>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(overrideBusId);
            bw.Write((byte)props.Count);

            foreach (Prop prop in props)
            {
                bw.Write((byte)prop.id);
            }

            foreach (Prop prop in props)
            {
                bw.Write(prop.value);
            }

            bw.Write(busInitialBitVector1);
            bw.Write(busInitialBitVector2);
            bw.Write((short)maxNumInstance);
            bw.Write(channelConfig);
            bw.Write(busInitialBitVector3);
            bw.Write(recoveryTime);
            bw.Write(maxDuckVolume);
            bw.Write(ducks.Count);

            foreach (Duck duck in ducks)
            {
                duck.WriteToFile(bw);
            }

            bw.Write((byte)fxChunks.Count);

            if (fxChunks.Count != 0)
            {
                bw.Write(bitsFxBypass);
            }

            foreach (FXChunk chunk in fxChunks)
            {
                bw.Write((byte)chunk.index);
                bw.Write(chunk.id);
                bw.Write((byte)chunk.isShareSet);
                bw.Write((byte)chunk.isRendered);
            }

            bw.Write(fxId);
            bw.Write((byte)isShareSet);
            bw.Write(overrideAttachmentParams);
            bw.Write((short)rtpc.Count);

            foreach (RTPC value in rtpc)
            {
                value.WriteToFile(bw);
            }

            bw.Write(stateChunks.Count);

            foreach (StateChunk chunk in stateChunks)
            {
                chunk.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int length = 40 + props.Count * 5 + fxChunks.Count * 7 + ducks.Count * 18;

            if (fxChunks.Count == 0)
            {
                length -= 1;
            }

            foreach (RTPC value in rtpc)
            {
                length += value.GetLength();
            }

            foreach (StateChunk chunk in stateChunks)

            {
                length += chunk.GetLength();
            }

            return length;
        }
    }
}
