using BitStreams;
using System.ComponentModel;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_InitOwnerDeform
    {
        public class DataPacket
        {
            public ulong Unk0 { get; set; }
            public C_Transform Unk1 { get; set; } // transform?

            public DataPacket()
            {
                Unk1 = new C_Transform();
            }

            public void Load(BitStream MemStream)
            {
                Unk0 = MemStream.ReadUInt64();
                Unk1.Load(MemStream);
            }

            public void Save(BitStream MemStream)
            {
                MemStream.WriteUInt64(Unk0);
                Unk1.Save(MemStream);
            }
        }

        public ulong Unk0 { get; set; }
        public ulong Unk1 { get; set; }
        public C_Transform Unk2 { get; set; }
        [Description("Could be center of mass")]
        public C_Vector3 Unk11 { get; set; }
        [Description("Could be Intertia Factor")]
        public C_Vector3 Unk12 { get; set; }
        public ushort[] Unk4 { get; set; } // array of index?
        public ushort[] Unk6 { get; set; }
        public DataPacket[] Unk9 { get; set; }
        public C_Transform Unk10 { get; set; } // transform?

        public S_InitOwnerDeform()
        {
            Unk2 = new C_Transform();
            Unk11 = new C_Vector3();
            Unk12 = new C_Vector3();
            Unk4 = new ushort[0];
            Unk9 = new DataPacket[0];
            Unk10 = new C_Transform();
        }

        public void Load(BitStream MemStream)
        {
            // two hashes (is 2nd one is usually empty?)
            Unk0 = MemStream.ReadUInt64();
            Unk1 = MemStream.ReadUInt64();

            // Read transform
            Unk2.Load(MemStream);

            // Two Vec3s
            Unk11.Load(MemStream);
            Unk12.Load(MemStream);

            // Array of Indexes?
            uint Unk4Count = MemStream.ReadUInt32();
            Unk4 = new ushort[Unk4Count];
            for (int i = 0; i < Unk4.Length; i++)
            {
                Unk4[i] = MemStream.ReadUInt16();
            }

            // More unknown
            uint Unk6Count = MemStream.ReadUInt32();
            Unk6 = new ushort[Unk6Count];
            for (int i = 0; i < Unk6Count; i++)
            {
                Unk6[i] = MemStream.ReadUInt16();
            }

            // Read unknown data
            uint NumDataPackets = MemStream.ReadUInt32();
            Unk9 = new DataPacket[NumDataPackets];
            for(int i = 0; i < Unk9.Length; i++)
            {
                DataPacket NewPacket = new DataPacket();
                NewPacket.Load(MemStream);
                Unk9[i] = NewPacket;
            }

            // Read transform
            Unk10.Load(MemStream);
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(Unk0);
            MemStream.WriteUInt64(Unk1);

            // Write Transform
            Unk2.Save(MemStream);

            // Two Vec3s
            Unk11.Save(MemStream);
            Unk12.Save(MemStream);

            MemStream.WriteUInt32((uint)Unk4.Length);
            foreach (ushort Value in Unk4)
            {
                MemStream.WriteUInt16(Value);
            }

            MemStream.WriteUInt32((uint)Unk6.Length);
            foreach (ushort Value in Unk6)
            {
                MemStream.WriteUInt16(Value);
            }

            // Write unknown data
            MemStream.WriteUInt32((uint)Unk9.Length);
            foreach (DataPacket Value in Unk9)
            {
                Value.Save(MemStream);
            }

            // Write Transform
            Unk10.Save(MemStream);
        }
    }
}
