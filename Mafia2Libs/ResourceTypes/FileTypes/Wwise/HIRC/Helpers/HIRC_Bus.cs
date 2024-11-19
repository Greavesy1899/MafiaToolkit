using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Bus
    {
        [Browsable(false)]
        public HIRCObject Parent { get; set; }
        public uint OverrideBusID { get; set; }
        public List<Prop> Props { get; set; }
        public byte BusInitialBitVector1 { get; set; } //bit0 = bMainOutputHierarchy, bit1 = bIsBackgroundMusic
        public byte BusInitialBitVector2 { get; set; }  //bit0 = bKillNewest, bit1 = bUseVirtualBehavior
        public uint MaxInstanceCount { get; set; }
        public uint ChannelConfig { get; set; } //bit0 = uNumChannels, bit8 = eConfigType, bit12 = uChannelMask
        public byte BusInitialBitVector3 { get; set; } //bit0 = bIsHdrBus, bit1 = bHdrReleaseModeExponential
        public uint RecoveryTime { get; set; }
        public float MaxDuckVolume { get; set; }
        public List<Duck> Ducks { get; set; }
        public byte BitsFXBypass { get; set; }
        public List<FXChunk> FXChunks { get; set; }
        public uint FXID { get; set; }
        public int IsShareSet { get; set; }
        public byte OverrideAttachmentParams { get; set; }
        public List<RTPC> rtpc { get; set; }
        public List<StateChunk> StateChunks { get; set; }
        public uint FeedbackBusId { get; set; }
        public Bus(HIRCObject ParentObject, BinaryReader br, uint length)
        {
            Parent = ParentObject;
            long initPos = br.BaseStream.Position - 4;
            OverrideBusID = br.ReadUInt32();
            Props = new List<Prop>();
            int PropsCount = br.ReadByte();

            for (int i = 0; i < PropsCount; i++)
            {
                byte key = br.ReadByte();
                Props.Add(new Prop(key));
            }

            foreach (Prop prop in Props)
            {
                prop.Value = br.ReadUInt32();
            }

            BusInitialBitVector1 = br.ReadByte();
            BusInitialBitVector2 = br.ReadByte();
            MaxInstanceCount = br.ReadUInt16();
            ChannelConfig = br.ReadUInt32();
            BusInitialBitVector3 = br.ReadByte();
            RecoveryTime = br.ReadUInt32();
            MaxDuckVolume = br.ReadSingle();
            Ducks = new List<Duck>();
            uint DucksCount = br.ReadUInt32();

            for (int i = 0; i < DucksCount; i++)
            {
                Ducks.Add(new Duck(br));
            }

            int numFx = br.ReadByte();
            FXChunks = new List<FXChunk>();

            if (numFx != 0)
            {
                BitsFXBypass = br.ReadByte();
            }

            for (int i = 0; i < numFx; i++)
            {
                byte Index = br.ReadByte();
                uint Id = br.ReadUInt32();
                byte bIsShareSet = br.ReadByte();
                byte bIsRendered = br.ReadByte();
                FXChunks.Add(new FXChunk(Index, Id, bIsShareSet, bIsRendered));
            }

            FXID = br.ReadUInt32();
            IsShareSet = br.ReadByte();
            OverrideAttachmentParams = br.ReadByte();
            uint rtpcCount = br.ReadUInt16();
            rtpc = new List<RTPC>();

            for (int i = 0; i < rtpcCount; i++)
            {
                rtpc.Add(new RTPC(br));
            }

            StateChunks = new List<StateChunk>();
            uint stateChunkCount = br.ReadUInt32();

            for (int i = 0; i < stateChunkCount; i++)
            {
                StateChunks.Add(new StateChunk(br, Parent));
            }

            switch (Parent.Bnk.Header.Feedback)
            {
                case 0:
                case 16:

                    break;

                default:
                    FeedbackBusId = br.ReadUInt32();
                    break;
            }
        }

        public Bus(HIRCObject ParentObject)
        {
            Parent = ParentObject;
            OverrideBusID = 0;
            Props = new List<Prop>();
            BusInitialBitVector1 = 0;
            BusInitialBitVector2 = 0;
            MaxInstanceCount = 0;
            ChannelConfig = 0;
            BusInitialBitVector3 = 0;
            RecoveryTime = 0;
            MaxDuckVolume = 0;
            Ducks = new List<Duck>();
            BitsFXBypass = 0;
            FXChunks = new List<FXChunk>();
            FXID = 0;
            IsShareSet = 0;
            OverrideAttachmentParams = 0;
            rtpc = new List<RTPC>();
            StateChunks = new List<StateChunk>();
            FeedbackBusId = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(OverrideBusID);
            bw.Write((byte)Props.Count);

            foreach (Prop prop in Props)
            {
                bw.Write((byte)prop.ID);
            }

            foreach (Prop prop in Props)
            {
                bw.Write(prop.Value);
            }
            bw.Write(BusInitialBitVector1);
            bw.Write(BusInitialBitVector2);
            bw.Write((short)MaxInstanceCount);
            bw.Write(ChannelConfig);
            bw.Write(BusInitialBitVector3);
            bw.Write(RecoveryTime);
            bw.Write(MaxDuckVolume);
            bw.Write(Ducks.Count);

            foreach (Duck duck in Ducks)
            {
                duck.WriteToFile(bw);
            }

            bw.Write((byte)FXChunks.Count);

            if (FXChunks.Count != 0)
            {
                bw.Write(BitsFXBypass);
            }

            foreach (FXChunk chunk in FXChunks)
            {
                bw.Write((byte)chunk.Index);
                bw.Write(chunk.ID);
                bw.Write((byte)chunk.IsShareSet);
                bw.Write((byte)chunk.IsRendered);
            }

            bw.Write(FXID);
            bw.Write((byte)IsShareSet);
            bw.Write(OverrideAttachmentParams);
            bw.Write((short)rtpc.Count);

            foreach (RTPC value in rtpc)
            {
                value.WriteToFile(bw);
            }

            bw.Write(StateChunks.Count);

            foreach (StateChunk chunk in StateChunks)
            {
                chunk.WriteToFile(bw);
            }

            switch (Parent.Bnk.Header.Feedback)
            {
                case 0:
                case 16:

                    break;

                default:
                    bw.Write(FeedbackBusId);
                    break;
            }
        }

        public int GetLength()
        {
            int length = 40 + Props.Count * 5 + FXChunks.Count * 7 + Ducks.Count * 18;

            if (FXChunks.Count == 0)
            {
                length -= 1;
            }

            foreach (RTPC value in rtpc)
            {
                length += value.GetLength();
            }

            foreach (StateChunk chunk in StateChunks)

            {
                length += chunk.GetLength();
            }

            switch (Parent.Bnk.Header.Feedback)
            {
                case 0:
                case 16:

                    break;

                default:
                    length += 4;
                    break;
            }

            return length;
        }
    }
}
