using Rendering.Core;
using Rendering.Graphics;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using Vortice.Mathematics;

namespace ResourceTypes.Navigation
{
    public class AIWorld_Type8 : AIWorld_Type9
    {
        public uint Unk6 { get; set; }

        public AIWorld_Type8(AIWorld InWorld) : base(InWorld) { }

        public override void Read(BinaryReader Reader)
        {
            base.Read(Reader);

            Unk6 = Reader.ReadUInt32();
        }

        public override void Write(BinaryWriter Writer)
        {
            base.Write(Writer);

            Writer.Write(Unk6);
        }

        public override void DebugWrite(StreamWriter Writer)
        {
            base.DebugWrite(Writer);

            Writer.WriteLine("Derived (Type 8) Unk6: {0}", Unk6);
        }

        public override void ConstructRenderable(PrimitiveBatch BBoxBatcher)
        {
            // NB: Base class is not called here as it may result in PrimitiveBatch.AddObject conflict

            RenderBoundingBox navigationBox = new RenderBoundingBox();
            navigationBox.SetColour(System.Drawing.Color.Blue);
            navigationBox.Init(new BoundingBox(new Vector3(-0.5f), new Vector3(0.5f)));
            navigationBox.SetTransform(Matrix4x4.CreateTranslation(Unk2));

            BBoxBatcher.AddObject(RefID, navigationBox);
        }

        public override TreeNode PopulateTreeNode()
        {
            base.PopulateTreeNode();

            TreeNode ThisNode = new TreeNode();
            ThisNode.Text = string.Format("Type8 - {0}", Unk1);
            ThisNode.Name = RefID.ToString();
            ThisNode.Tag = this;

            return ThisNode;
        }

        public override Vector3 GetPosition()
        {
            return Unk2;
        }
    }
}
