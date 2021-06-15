using Rendering.Core;
using System.IO;
using System.Windows.Forms;

namespace ResourceTypes.Navigation
{
    public class AIWorld_Type1 : IType
    {
        public byte Unk01 { get; set; }
        public IType[] AIPoints { get; set; }

        public AIWorld_Type1(AIWorld InWorld) : base(InWorld)
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
                AIPoints[i] = AIWorld_Factory.ConstructByTypeID(OwnWorld, TypeID);
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

        public override void ConstructRenderable(PrimitiveBatch BBoxBatcher)
        {
            base.ConstructRenderable(BBoxBatcher);

            foreach (IType AIPoint in AIPoints)
            {
                AIPoint.ConstructRenderable(BBoxBatcher);
            }
        }

        public override TreeNode PopulateTreeNode()
        {
            base.PopulateTreeNode();

            TreeNode ThisNode = new TreeNode();
            ThisNode.Text = "Type1 Group";
            ThisNode.Name = RefID.ToString();
            ThisNode.Tag = this;

            foreach (IType AIPoint in AIPoints)
            {
                ThisNode.Nodes.Add(AIPoint.PopulateTreeNode());
            }

            return ThisNode;
        }
    }
}