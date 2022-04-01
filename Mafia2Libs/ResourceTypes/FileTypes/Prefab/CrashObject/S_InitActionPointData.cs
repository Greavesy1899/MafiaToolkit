using BitStreams;
using System.Diagnostics;
using Utils.Logging;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_InitActionPointData
    {
        public byte Unk0 { get; set; }
        public string Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public string Unk3 { get; set; } // 100% not a hash
        public float[] Unk4 { get; set; } // Could be two C_Vector3s. A box?

        public void Load(BitStream MemStream)
        {
            Unk0 = MemStream.ReadBit();
            ToolkitAssert.Ensure(Unk0 == 1, "Extra data detected");

            Unk1 = MemStream.ReadString32();

            Unk2 = MemStream.ReadBit();
            ToolkitAssert.Ensure(Unk2 == 1, "Extra data detected");

            Unk3 = MemStream.ReadString32();

            Unk4 = new float[6];
            for (int i = 0; i < Unk4.Length; i++)
            {
                Unk4[i] = MemStream.ReadSingle();
            }
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteBit(Unk0);
            MemStream.WriteString32(Unk1);
            MemStream.WriteBit(Unk2);
            MemStream.WriteString32(Unk3);

            // Write floats
            for(int i = 0; i < 6; i++)
            {
                MemStream.WriteSingle(Unk4[i]);
            }
        }
    }
}
