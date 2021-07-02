using SharpDX;
using System.IO;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxBoneTransform
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            Position = Vector3Extenders.ReadFromFile(reader);
            Rotation = QuaternionExtensions.ReadFromFile(reader);
            Scale = Vector3Extenders.ReadFromFile(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            Vector3Extenders.WriteToFile(Position, writer);
            QuaternionExtensions.WriteToFile(Rotation, writer);
            Vector3Extenders.WriteToFile(Scale, writer);
        }
    }

    public class FxBoneLink
    {
        public uint Unk0 { get; set; } // Could be NodeIndex
        public uint Unk1 { get; set; }// Could be NodeIndex
        FxBoneTransform OptimizedTransform { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            Unk0 = reader.ReadUInt32();
            Unk1 = reader.ReadUInt32();

            OptimizedTransform = new FxBoneTransform();
            OptimizedTransform.ReadFromFile(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Unk0);
            writer.Write(Unk1);
            OptimizedTransform.WriteToFile(writer);
        }
    }



    public class FxUnk3
    {
        public class FxUnk3_Node
        {
            public string NodeName { get; set; }
            public uint NodeUnk0 { get; set; }
        }
        public uint Unk0 { get; set; }
        public uint NumNodes { get; set; }
        public FxUnk3_Node[] Nodes { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            Unk0 = reader.ReadUInt32();
            NumNodes = reader.ReadUInt32();

            Nodes = new FxUnk3_Node[NumNodes];
            for (int i = 0; i < NumNodes; i++)
            {
                Nodes[i] = new FxUnk3_Node();
                Nodes[i].NodeName = StringHelpers.ReadString32(reader);
                Nodes[i].NodeUnk0 = reader.ReadUInt32();
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Unk0);

            writer.Write(Nodes.Length);
            foreach(FxUnk3_Node Node in Nodes)
            {
                StringHelpers.WriteString32(writer, Node.NodeName);
                writer.Write(Node.NodeUnk0);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Nodes[0].NodeName, Nodes.Length);
        }
    }

    public class FxPhonToNameMap : FxObject
    {
        public FxPhoneme Phoneme { get; set; }
        public FxName Name { get; set; }
        public float Amount { get; set; }

        public FxPhonToNameMap() : base()
        {
            Name = new FxName();
        }

        public override void Deserialize(FxArchive Owner, BinaryReader Reader)
        {
            base.Deserialize(Owner, Reader);

            Phoneme = (FxPhoneme)Reader.ReadUInt32();
            Name.ReadFromFile(Owner, Reader);
            Amount = Reader.ReadSingle();
        }

        public override void Serialize(FxArchive Owner, BinaryWriter Writer)
        {
            base.Serialize(Owner, Writer);

            Writer.Write((uint)Phoneme);
            Name.WriteToFile(Owner, Writer);
            Writer.Write(Amount);
        }

        public override void PopulateStringTable(FxArchive Owner)
        {
            base.PopulateStringTable(Owner);

            Name.AddToStringTable(Owner);
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    public class FxActor : FxNamedObject
    {
        private uint Unk10;

        public FxMasterBoneListEntry[] MasterBoneList { get; set; }
        public FxUnk3[] Unk03s { get; set; }

        // Data in between these two arrays
        private uint Unk04;
        private uint Unk05;
        private uint Unk06;

        public FxCompiledFaceGraphNode[] FaceGraphNodes { get; set; }
        public FxPhonToNameMap[] PhoneToNameMap { get; set; }
        public uint[] Unk3s { get; set; }

        public override void Deserialize(FxArchive Owner, BinaryReader reader)
        {
            base.Deserialize(Owner, reader);

            Unk10 = reader.ReadUInt32(); // Unknown; could be data about published or not.

            // Read FxMasterBoneListEntries
            uint NumMasterBoneEntries = reader.ReadUInt32();
            MasterBoneList = new FxMasterBoneListEntry[NumMasterBoneEntries];
            for (int i = 0; i < MasterBoneList.Length; i++)
            {
                FxMasterBoneListEntry DataObject = new FxMasterBoneListEntry();
                DataObject.ReadFromFile(Owner, reader);
                MasterBoneList[i] = DataObject;
            }

            // Read FxCompiledFaceGraphNodes
            uint NumFaceGraphNodes = reader.ReadUInt32();
            FaceGraphNodes = new FxCompiledFaceGraphNode[NumFaceGraphNodes];
            for (int i = 0; i < FaceGraphNodes.Length; i++)
            {
                FxCompiledFaceGraphNode DataObject = new FxCompiledFaceGraphNode();
                DataObject.ReadFromFile(Owner, reader);
                FaceGraphNodes[i] = DataObject;
            }

            uint NumUnk2s = reader.ReadUInt32();
            Unk03s = new FxUnk3[NumUnk2s];
            for (int i = 0; i < Unk03s.Length; i++)
            {
                FxUnk3 ClassObject = new FxUnk3();
                ClassObject.ReadFromFile(reader);
                Unk03s[i] = ClassObject;
            }

            Unk04 = reader.ReadUInt32();
            Unk05 = reader.ReadUInt32();
            Unk06 = reader.ReadUInt32();

            uint PhonToNameMapCount = reader.ReadUInt32();
            PhoneToNameMap = new FxPhonToNameMap[PhonToNameMapCount];
            for (int i = 0; i < PhoneToNameMap.Length; i++)
            {
                FxPhonToNameMap PhonToName = new FxPhonToNameMap();
                PhonToName.Deserialize(Owner, reader);
                PhoneToNameMap[i] = PhonToName;
            }

            uint NumUnk3 = reader.ReadUInt32();
            Unk3s = new uint[NumUnk3];
            for (int i = 0; i < Unk3s.Length; i++)
            {
                Unk3s[i] = reader.ReadUInt32();
            }
        }

        public override void Serialize(FxArchive Owner, BinaryWriter writer)
        {
            base.Serialize(Owner, writer);

            writer.Write(Unk10);

            // Write FxMasterBoneList
            writer.Write(MasterBoneList.Length);
            foreach (FxMasterBoneListEntry Entry in MasterBoneList)
            {
                Entry.WriteToFile(Owner, writer);
            }

            // Write FxCompiledFaceGraphNodes
            writer.Write(FaceGraphNodes.Length);
            foreach (FxCompiledFaceGraphNode GraphNode in FaceGraphNodes)
            {
                GraphNode.WriteToFile(Owner, writer);
            }

            // Write Unk03
            writer.Write(Unk03s.Length);
            foreach (FxUnk3 MapNode in Unk03s)
            {
                MapNode.WriteToFile(writer);
            }

            writer.Write(Unk04);
            writer.Write(Unk05);
            writer.Write(Unk06);

            // Write PhonemeMapToNode
            writer.Write(PhoneToNameMap.Length);
            foreach (FxPhonToNameMap Entry in PhoneToNameMap)
            {
                Entry.Serialize(Owner, writer);
            }

            // Write Unk3s
            writer.Write(Unk3s.Length);
            foreach (uint Value in Unk3s)
            {
                writer.Write(Value);
            }
        }

        public override void PopulateStringTable(FxArchive Owner)
        {
            base.PopulateStringTable(Owner);

            foreach(FxMasterBoneListEntry Entry in MasterBoneList)
            {
                Entry.PopulateStringTable(Owner);
            }

            foreach(FxCompiledFaceGraphNode GraphNode in FaceGraphNodes)
            {
                GraphNode.PopulateStringTable(Owner);
            }

            // NB:
            // Unk03s is empty

            foreach (FxPhonToNameMap Entry in PhoneToNameMap)
            {
                Entry.PopulateStringTable(Owner);
            }

            // Seems to be standard?
            Owner.AddToStringTable("Default");
        }
    }
}
