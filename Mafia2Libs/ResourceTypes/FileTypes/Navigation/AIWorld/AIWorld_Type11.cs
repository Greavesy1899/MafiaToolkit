using Rendering.Core;
using Rendering.Graphics;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.Navigation
{
    public class AIWorld_Type11 : IType
    {
        public byte Unk0 { get; set; }
        public Vector3 Unk1 { get; set; }
        public Vector3 Unk2 { get; set; }
        public Vector3 Unk3 { get; set; }
        public uint Unk4 { get; set; }

        public AIWorld_Type11(AIWorld InWorld) : base(InWorld)
        {
            Unk1 = Vector3.Zero;
            Unk2 = Vector3.Zero;
            Unk3 = Vector3.Zero;
        }

        public override void Read(BinaryReader Reader)
        {
            base.Read(Reader);

            Unk0 = Reader.ReadByte();
            Unk1 = Vector3Utils.ReadFromFile(Reader);
            Unk2 = Vector3Utils.ReadFromFile(Reader);
            Unk3 = Vector3Utils.ReadFromFile(Reader);
            Unk4 = Reader.ReadUInt32(); // int32 (could be split into two shorts)
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

        public override void DebugWrite(StreamWriter Writer)
        {
            base.DebugWrite(Writer);

            Writer.WriteLine("Type11: ");
            Writer.WriteLine("Unk1: {0}", Unk1);
            Writer.WriteLine("Unk2: {0}", Unk2);
            Writer.WriteLine("Unk3: {0}", Unk3);
            Writer.WriteLine("Unk4: {0}", Unk4);
        }

        public override void ConstructRenderable(PrimitiveBatch BBoxBatcher)
        {
            base.ConstructRenderable(BBoxBatcher);

            RenderBoundingBox navigationBox = new RenderBoundingBox();
            navigationBox.SetColour(System.Drawing.Color.LightBlue);
            navigationBox.Init(new BoundingBox(new Vector3(-0.5f), new Vector3(0.5f)));
            navigationBox.SetTransform(Matrix4x4.CreateTranslation(Unk1));

            BBoxBatcher.AddObject(RefID, navigationBox);
        }

        public override TreeNode PopulateTreeNode()
        {
            base.PopulateTreeNode();

            TreeNode ThisNode = new TreeNode();
            ThisNode.Text = "Type11";
            ThisNode.Name = RefID.ToString();
            ThisNode.Tag = this;

            return ThisNode;
        }

        public override Vector3 GetPosition()
        {
            return Unk1;
        }
    }
}
