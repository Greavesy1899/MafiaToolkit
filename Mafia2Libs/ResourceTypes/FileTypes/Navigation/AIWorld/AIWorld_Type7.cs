using SharpDX;
using System.IO;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Navigation
{
    public class AIWorld_Type7 : IType
    {
        public ushort Unk0 { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 Unk2 { get; set; }
        public uint Unk3 { get; set; }

        public AIWorld_Type7()
        {
            Position = Vector3.Zero;
            Direction = Vector3.Zero;
            Unk2 = Vector3.Zero;
        }

        public override void Read(BinaryReader Reader)
        {
            base.Read(Reader);

            Unk0 = Reader.ReadUInt16();
            Position = Vector3Extenders.ReadFromFile(Reader);
            Direction = Vector3Extenders.ReadFromFile(Reader);
            Unk2 = Vector3Extenders.ReadFromFile(Reader);
            Unk3 = Reader.ReadUInt32();
        }

        public override void Write(BinaryWriter Writer)
        {
            base.Write(Writer);

            Writer.Write(Unk0);
            Position.WriteToFile(Writer);
            Direction.WriteToFile(Writer);
            Unk2.WriteToFile(Writer);
            Writer.Write(Unk3);
        }

        public override void DebugWrite(StreamWriter Writer)
        {
            base.DebugWrite(Writer);

            Writer.WriteLine("Type 7:");
            Writer.WriteLine("Unk0: {0}", Unk0);
            Writer.WriteLine("Position: {0}", Position.ToString());
            Writer.WriteLine("Direction: {0}", Direction.ToString());
            Writer.WriteLine("Unk2: {0}", Unk2.ToString());
            Writer.WriteLine("Unke3: {0}", Unk3);
        }
    }
}
