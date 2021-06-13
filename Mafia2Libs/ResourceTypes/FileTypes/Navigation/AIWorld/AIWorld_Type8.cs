using SharpDX;
using System.IO;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Navigation
{
    // Also type 9
    public class AIWorld_Type8 : IType
    {
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public Vector3 Unk2 { get; set; }
        public float Unk3 { get; set; }
        public float Unk4 { get; set; }
        public uint[] Unk5 { get; set; }

        public AIWorld_Type8()
        {
            Unk2 = Vector3.Zero;
            Unk5 = new uint[0];
        }

        public override void Read(BinaryReader Reader)
        {
            base.Read(Reader);

            Unk0 = Reader.ReadByte();
            Unk1 = Reader.ReadUInt32();
            Unk2 = Vector3Extenders.ReadFromFile(Reader);
            Unk3 = Reader.ReadSingle();
            Unk4 = Reader.ReadSingle();

            ushort Size = Reader.ReadUInt16();
            Unk5 = new uint[Size];
            for (int i = 0; i < Size; i++)
            {
                Unk5[i] = Reader.ReadUInt32();
            }
        }

        public override void Write(BinaryWriter Writer)
        {
            base.Write(Writer);

            Writer.Write(Unk0);
            Writer.Write(Unk1);
            Unk2.WriteToFile(Writer);
            Writer.Write(Unk3);
            Writer.Write(Unk4);

            Writer.Write((ushort)Unk5.Length);
            foreach(uint Value in Unk5)
            {
                Writer.Write(Value);
            }
        }

        public override void DebugWrite(StreamWriter Writer)
        {
            base.DebugWrite(Writer);

            Writer.WriteLine("Type 8 (Could also be Type 9):");
            Writer.WriteLine("Unk0: {0}", Unk0);
            Writer.WriteLine("Unk1: {0}", Unk1);
            Writer.WriteLine("Unk2: {0}", Unk2.ToString());
            Writer.WriteLine("Unk3: {0}", Unk3);
            Writer.WriteLine("Unk4: {0}", Unk4);

            Writer.WriteLine("Unk5 Size: {0}", Unk5.Length);
            foreach (uint Value in Unk5)
            {
                Writer.WriteLine("Value: {0}", Value);
            }
        }
    }
}
