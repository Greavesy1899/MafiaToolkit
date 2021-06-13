using SharpDX;
using System.IO;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Navigation
{
    public class AIWorld_Type11 : IType
    {
        public byte Unk0 { get; set; }
        public Vector3 Unk1 { get; set; }
        public Vector3 Unk2 { get; set; }
        public Vector3 Unk3 { get; set; }
        public float Unk4 { get; set; }

        public AIWorld_Type11()
        {
            Unk1 = Vector3.Zero;
            Unk2 = Vector3.Zero;
            Unk3 = Vector3.Zero;
        }

        public override void Read(BinaryReader Reader)
        {
            base.Read(Reader);

            Unk0 = Reader.ReadByte();
            Unk1 = Vector3Extenders.ReadFromFile(Reader);
            Unk2 = Vector3Extenders.ReadFromFile(Reader);
            Unk3 = Vector3Extenders.ReadFromFile(Reader);
            Unk4 = Reader.ReadSingle(); // int32 (could be split into two shorts)
        }

        public override void Write(BinaryWriter Writer)
        {
            base.Write(Writer);

            Writer.Write(Unk0);
            Unk1.WriteToFile(Writer);
            Unk2.WriteToFile(Writer);
            Unk3.WriteToFile(Writer);
            Writer.Write(Unk4);
        }
    }
}
