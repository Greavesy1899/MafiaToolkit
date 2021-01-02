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

    public class FxMasterBoneListEntry
    {
        public uint PossibleIdx { get; set; }
        public FxBoneTransform Transform { get; set; }
        byte[] UnkQuaternion { get; set; } // 4 floats.. could be quat?
        public uint PossibleIdx2 { get; set; }
        public float Unk0 { get; set; }
        public FxBoneLink[] Links { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            PossibleIdx = reader.ReadUInt32();
            Transform = new FxBoneTransform();
            Transform.ReadFromFile(reader);
            UnkQuaternion = reader.ReadBytes(16);
            PossibleIdx2 = reader.ReadUInt32();
            Unk0 = reader.ReadSingle();

            uint NumLinks = reader.ReadUInt32();
            Links = new FxBoneLink[NumLinks];
            for (int i = 0; i < NumLinks; i++)
            {
                FxBoneLink LinkObject = new FxBoneLink();
                LinkObject.ReadFromFile(reader);
                Links[i] = LinkObject;
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(PossibleIdx);
            Transform.WriteToFile(writer);
            writer.Write(UnkQuaternion);
            writer.Write(PossibleIdx2);
            writer.Write(Unk0);

            writer.Write(Links.Length);
            foreach(FxBoneLink Link in Links)
            {
                Link.WriteToFile(writer);
            }
        }

        public override string ToString()
        {
            return PossibleIdx.ToString();
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

    public class FxPhonToNameMap
    {
        public FxPhoneme Phoneme { get; set; }
        public uint Name { get; set; } // FxName
        public float Amount { get; set; }

        public void ReadFromFIle(BinaryReader reader)
        {
            Phoneme = (FxPhoneme)reader.ReadUInt32();
            Name = reader.ReadUInt32();
            Amount = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((uint)Phoneme);
            writer.Write(Name);
            writer.Write(Amount);
        }
    }

    public class FxActor : FxArchive
    {
        private uint ActorNameID;
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

        public void ReadFromFile(string filename)
        {
            using(BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void WriteToFile(string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("face2.facefx", FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);

            // ActorNameID
            ActorNameID = reader.ReadUInt32();
            Unk10 = reader.ReadUInt32(); // Unknown; could be data about published or not.

            // Read FxMasterBoneListEntries
            uint NumMasterBoneEntries = reader.ReadUInt32();
            MasterBoneList = new FxMasterBoneListEntry[NumMasterBoneEntries];
            for (int i = 0; i < MasterBoneList.Length; i++)
            {
                FxMasterBoneListEntry DataObject = new FxMasterBoneListEntry();
                DataObject.ReadFromFile(reader);
                MasterBoneList[i] = DataObject;
            }

            // Read FxCompiledFaceGraphNodes
            uint NumFaceGraphNodes = reader.ReadUInt32();         
            FaceGraphNodes = new FxCompiledFaceGraphNode[NumFaceGraphNodes];
            for (int i = 0; i < FaceGraphNodes.Length; i++)
            {
                FxCompiledFaceGraphNode DataObject = new FxCompiledFaceGraphNode();
                DataObject.ReadFromFile(reader, this);
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
                PhonToName.ReadFromFIle(reader);
                PhoneToNameMap[i] = PhonToName;
            }

            uint NumUnk3 = reader.ReadUInt32();
            Unk3s = new uint[NumUnk3];
            for (int i = 0; i < Unk3s.Length; i++)
            {
                Unk3s[i] = reader.ReadUInt32();
            }
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);

            writer.Write(ActorNameID);
            writer.Write(Unk10);

            // Write FxMasterBoneList
            writer.Write(MasterBoneList.Length);
            foreach(FxMasterBoneListEntry Entry in MasterBoneList)
            {
                Entry.WriteToFile(writer);
            }

            // Write FxCompiledFaceGraphNodes
            writer.Write(FaceGraphNodes.Length);
            foreach (FxCompiledFaceGraphNode GraphNode in FaceGraphNodes)
            {
                GraphNode.WriteToFile(writer);
            }

            // Write Unk03
            writer.Write(Unk03s.Length);
            foreach(FxUnk3 MapNode in Unk03s)
            {
                MapNode.WriteToFile(writer);
            }

            writer.Write(Unk04);
            writer.Write(Unk05);
            writer.Write(Unk06);

            // Write PhonemeMapToNode
            writer.Write(PhoneToNameMap.Length);
            foreach(FxPhonToNameMap Entry in PhoneToNameMap)
            {
                Entry.WriteToFile(writer);
            }

            // Write Unk3s
            writer.Write(Unk3s.Length);
            foreach(uint Value in Unk3s)
            {
                writer.Write(Value);
            }
        }
    }

    public class FxActorContainer
    {
        uint[] ActorSize;
        FxActor[] Actors;

        public void ReadFromFile(BinaryReader reader)
        {
            uint NumActors = reader.ReadUInt32();

            Actors = new FxActor[NumActors];
            ActorSize = new uint[NumActors];
            for (int i = 0; i < NumActors; i++)
            {
                ActorSize[i] = reader.ReadUInt32();
                FxActor ActorObject = new FxActor();
                ActorObject.ReadFromFile(reader);
                Actors[i] = ActorObject;
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Actors.Length);

            for (int i = 0; i < Actors.Length; i++)
            {
                writer.Write(ActorSize[i]);
                Actors[i].WriteToFile(writer);
            }
        }
    }
}
