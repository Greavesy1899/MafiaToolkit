using System.IO;

namespace ResourceTypes.Navigation
{
    public class AIWorld_Type1 : IType
    {
        public byte Unk01 { get; set; }
        public IType[] AIPoints { get; set; }

        public AIWorld_Type1()
        {
            AIPoints = new IType[0];
        }

        public override void Read(BinaryReader Reader)
        {
            base.Read(Reader);

            Unk01 = Reader.ReadByte();

            uint NumPoints = Reader.ReadUInt32();
            AIPoints = new IType[NumPoints];
            for(uint i = 0; i < NumPoints; i++)
            {
                ushort TypeID = Reader.ReadUInt16();
                AIPoints[i] = AIWorld_Factory.ConstructByTypeID(TypeID);
                AIPoints[i].Read(Reader);
            }
        }

        public override void Write(BinaryWriter Writer)
        {
            base.Write(Writer);

            Writer.Write(Unk01);
            Writer.Write(AIPoints.Length);

            foreach(IType AIPoint in AIPoints)
            {
                ushort TypeID = AIWorld_Factory.GetIDByType(AIPoint);
                Writer.Write(TypeID);
                AIPoint.Write(Writer);
            }
        }

        public override void DebugWrite(StreamWriter Writer)
        {
            base.DebugWrite(Writer);

            Writer.WriteLine("Unk0: {0}", Unk01);
            Writer.WriteLine("NumPoints: {0}", AIPoints.Length);
            foreach(IType AIPoint in AIPoints)
            {
                AIPoint.DebugWrite(Writer);
                Writer.WriteLine("");
            }
        }
    }
}