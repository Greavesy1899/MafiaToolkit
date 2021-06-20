using Rendering.Core;
using Rendering.Graphics;
using SharpDX;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
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

        [Category("BBox Test")]
        public Vector3 Minimum { get; set; }
        [Category("BBox Test")]
        public Vector3 Maximum { get; set; }

        public AIWorld_Type7(AIWorld InWorld) : base(InWorld)
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

            Minimum = new Vector3(-Unk2.Y, -Unk2.X, -Unk2.Z);
            Maximum = new Vector3(Unk2.Y, Unk2.X, Unk2.Z);
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

        public override void ConstructRenderable(PrimitiveBatch BBoxBatcher)
        {
            base.ConstructRenderable(BBoxBatcher);

            RenderBoundingBox navigationBox = new RenderBoundingBox();
            navigationBox.SetColour(System.Drawing.Color.Yellow);

            BoundingBox BBox = new BoundingBox();
            BBox.Minimum = Minimum;
            BBox.Maximum = Maximum;

            Matrix RotationMatrix = MatrixExtensions.ConvertFromDirection(Direction);
            RotationMatrix.TranslationVector = Position;

            navigationBox.Init(BBox);
            navigationBox.SetTransform(RotationMatrix);

            BBoxBatcher.AddObject(RefID, navigationBox);
        }

        public override TreeNode PopulateTreeNode()
        {
            base.PopulateTreeNode();

            TreeNode ThisNode = new TreeNode();
            ThisNode.Text = "Type7";
            ThisNode.Name = RefID.ToString();
            ThisNode.Tag = this;

            return ThisNode;
        }

        public override Vector3 GetPosition()
        {
            return Position;
        }
    }
}
