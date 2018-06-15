using System.IO;
using System.Collections;
using System;
using System.ComponentModel;

namespace Mafia2 {
    public class FrameObjectBase {
        Hash name;
        int unknown_00_int;
        TransformMatrix transformMatrix;
        short unknown_03_short;
        ParentStruct parentIndex1;
        ParentStruct parentIndex2;
        short unknown_06_short;
        Node node;

        public Hash Name {
            get { return name; }
            set { name = value; }
        }
        public int Unk_00_int {
            get { return unknown_00_int; }
            set { unknown_00_int = value; }
        }
        public TransformMatrix Matrix {
            get { return transformMatrix; }
            set { transformMatrix = value; }
        }
        public short Unk_03 {
            get { return unknown_03_short; }
            set { unknown_03_short = value; }
        }
        public ParentStruct ParentIndex1 {
            get { return parentIndex1; }
            set { parentIndex1 = value; }
        }
        public ParentStruct ParentIndex2 {
            get { return parentIndex2; }
            set { parentIndex2 = value; }
        }
        public short Unk_06 {
            get { return unknown_06_short; }
            set { unknown_06_short = value; }
        }
        public Node NodeData{
            get { return node; }
            set { node = value; }
        }

        [Category("Object Type"), ReadOnly(true)]
        public string Type {
            get { return GetType().ToString(); }
        }

        public FrameObjectBase() { }

        public virtual void ReadFromFile(BinaryReader reader) {
            name = new Hash(reader);
            unknown_00_int = reader.ReadInt32();
            transformMatrix = new TransformMatrix(reader);
            unknown_03_short = reader.ReadInt16();
            parentIndex1 = new ParentStruct(reader.ReadInt32());
            parentIndex2 = new ParentStruct(reader.ReadInt32());
            unknown_06_short = reader.ReadInt16();
            node = new Node(name.Name, this);
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
