using System.IO;
using System.ComponentModel;
using Mafia2;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectBase : FrameEntry
    {
        protected Hash name;
        protected int unk0;
        protected TransformMatrix transformMatrix;
        protected short unk3;
        protected ParentStruct parentIndex1;
        protected ParentStruct parentIndex2;
        protected short unk6;
        protected Node node;
        protected bool isOnTable;
        protected NameTableFlags nameTableFlags;


        public Hash Name {
            get { return name; }
            set { name = value; }
        }
        public int Unk0 {
            get { return unk0; }
            set { unk0 = value; }
        }
        public TransformMatrix Matrix {
            get { return transformMatrix; }
            set { transformMatrix = value; }
        }
        [Browsable(false)]
        public short Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }
        public ParentStruct ParentIndex1 {
            get { return parentIndex1; }
            set { parentIndex1 = value; }
        }
        public ParentStruct ParentIndex2 {
            get { return parentIndex2; }
            set { parentIndex2 = value; }
        }
        [Browsable(false)]
        public short Unk6 {
            get { return unk6; }
            set { unk6 = value; }
        }
        public Node NodeData {
            get { return node; }
            set { node = value; }
        }

        [Description("Only use this if the object is going to be saved in the FrameNameTable"), Category("FrameNameTable Data"), Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public NameTableFlags FrameNameTableFlags {
            get { return nameTableFlags; }
            set { nameTableFlags = value; }
        }
        [Description("If this is true, it will be added onto the FrameNameTable and the flags will be saved"), Category("FrameNameTable Data")]
        public bool IsOnFrameTable {
            get { return isOnTable; }
            set { isOnTable = value; }
        }
        [Category("Object Type"), ReadOnly(true)]
        public string Type {
            get { return GetType().ToString(); }
        }

        public FrameObjectBase() : base()
        {
            //do example name.
            name = new Hash();
            name.Set("NewObject");
            unk0 = 1;
            transformMatrix = new TransformMatrix();
            unk3 = -1;
            parentIndex1 = new ParentStruct(-1);
            parentIndex2 = new ParentStruct(-1);
            unk6 = -1;
            UpdateNode();
        }

        public virtual void ReadFromFile(BinaryReader reader)
        {
            name = new Hash(reader);
            unk0 = reader.ReadInt32();
            transformMatrix = new TransformMatrix(reader);
            unk3 = reader.ReadInt16();
            parentIndex1 = new ParentStruct(reader.ReadInt32());
            parentIndex2 = new ParentStruct(reader.ReadInt32());
            unk6 = reader.ReadInt16();
            UpdateNode();
        }

        public virtual void WriteToFile(BinaryWriter writer)
        {
            name.WriteToFile(writer);
            writer.Write(unk0);
            transformMatrix.WriteToFrame(writer);
            writer.Write(unk3);
            writer.Write(parentIndex1.Index);
            writer.Write(parentIndex2.Index);
            writer.Write(unk6);
        }

        public void UpdateNode()
        {
            node = new Node(name.ToString(), refID.ToString(), this);
        }
    }

    public class Node
    {
        string text;
        string name;
        object tag;

        public string Text {
            get { return text; }
            set { text = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }
        public object Tag {
            get { return tag; }
            set { tag = value; }
        }

        public Node(string text, string name, object tag)
        {
            this.name = name;
            this.text = text;
            this.tag = tag;
        }
    }
}
