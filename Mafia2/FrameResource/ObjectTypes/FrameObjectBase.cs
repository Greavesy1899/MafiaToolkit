using System.IO;
using System.Collections;
using System;
using System.ComponentModel;

namespace Mafia2
{
    public class FrameObjectBase
    {
        Hash name;
        int unk0;
        TransformMatrix transformMatrix;
        short unk3;
        ParentStruct parentIndex1;
        ParentStruct parentIndex2;
        short unk6;
        Node node;

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
        public short Unk6 {
            get { return unk6; }
            set { unk6 = value; }
        }
        public Node NodeData {
            get { return node; }
            set { node = value; }
        }

        [Category("Object Type"), ReadOnly(true)]
        public string Type {
            get { return GetType().ToString(); }
        }

        public FrameObjectBase() { }

        public virtual void ReadFromFile(BinaryReader reader)
        {
            name = new Hash(reader);
            unk0 = reader.ReadInt32();
            transformMatrix = new TransformMatrix(reader);
            unk3 = reader.ReadInt16();
            parentIndex1 = new ParentStruct(reader.ReadInt32());
            parentIndex2 = new ParentStruct(reader.ReadInt32());
            unk6 = reader.ReadInt16();
            node = new Node(name.String, this);
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

    public class Node
    {
        string nameText;
        object tag;

        public string NameText {
            get { return nameText; }
            set { nameText = value; }
        }
        public object Tag {
            get { return tag; }
            set { tag = value; }
        }

        public Node(string nameText, object tag)
        {
            if (nameText == "")
                this.nameText = tag.GetType().ToString();
            else
                this.nameText = nameText;

            this.tag = tag;
        }
    }
}
