using System.IO;
using System.ComponentModel;
using Mafia2;
using Utils.Extensions;
using Utils.Types;
using System.Xml;

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
            name = new Hash("NewObject");
            unk0 = 1;
            transformMatrix = new TransformMatrix();
            unk3 = -1;
            parentIndex1 = new ParentStruct(-1);
            parentIndex2 = new ParentStruct(-1);
            unk6 = -1;
        }

        public FrameObjectBase(FrameObjectBase other) : base(other)
        {
            name = new Hash(other.name.String);
            unk0 = other.unk0;
            transformMatrix = new TransformMatrix(other.transformMatrix);
            unk3 = other.unk3;
            parentIndex1 = new ParentStruct(other.parentIndex1);
            parentIndex2 = new ParentStruct(other.parentIndex2);
            unk6 = -1;
            isOnTable = other.isOnTable;
            nameTableFlags = other.nameTableFlags;
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
    }
}
