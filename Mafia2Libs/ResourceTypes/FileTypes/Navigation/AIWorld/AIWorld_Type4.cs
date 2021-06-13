using SharpDX;
using System.IO;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Navigation
{
    public class AIWorld_Type4 : IType
    {
        public byte Unk0 { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public Vector3 Unk4 { get; set; }
        public uint Unk5 { get; set; }
        public byte Unk6 { get; set; }
        public byte Unk7 { get; set; }
        public uint[] Unk8 { get; set; }
        public uint Unk9 { get; set; }

        public AIWorld_Type4()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Unk4 = Vector3.Zero;
            Unk8 = new uint[0];
        }

        public override void Read(BinaryReader Reader)
        {
            base.Read(Reader);

            Unk0 = Reader.ReadByte();
            Position = Vector3Extenders.ReadFromFile(Reader);
            Rotation = Vector3Extenders.ReadFromFile(Reader);
            Unk1 = Reader.ReadUInt32();
            Unk2 = Reader.ReadUInt32();
            Unk3 = Reader.ReadUInt32();
            Unk4 = Vector3Extenders.ReadFromFile(Reader);
            Unk5 = Reader.ReadUInt32();
            Unk6 = Reader.ReadByte();
            Unk7 = Reader.ReadByte();

            ushort Size = Reader.ReadUInt16();
            Unk8 = new uint[Size];
            for (int i = 0; i < Unk8.Length; i++)
            {
                Unk8[i] = Reader.ReadUInt32();
            }

            Unk9 = Reader.ReadUInt32();
        }

        public override void Write(BinaryWriter Writer)
        {
            base.Write(Writer);

            Writer.Write(Unk0);
            Position.WriteToFile(Writer);
            Rotation.WriteToFile(Writer);
            Writer.Write(Unk1);
            Writer.Write(Unk2);
            Writer.Write(Unk3);
            Unk4.WriteToFile(Writer);
            Writer.Write(Unk5);
            Writer.Write(Unk6);
            Writer.Write(Unk7);

            Writer.Write((ushort)Unk8.Length);
            foreach(uint Value in Unk8)
            {
                Writer.Write(Value);
            }

            Writer.Write(Unk9);
        }
    }
}
