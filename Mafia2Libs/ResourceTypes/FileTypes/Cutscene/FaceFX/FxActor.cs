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

    public class FxCompiledFaceGraphLink
    {
        public uint NodeIndex { get; set; }
        public FxLinkFnType LinkFnType { get; set; }
        public uint NumParameters { get; set; }
        public float[] Parameters { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            NodeIndex = reader.ReadUInt32();
            LinkFnType = (FxLinkFnType)reader.ReadUInt32();
            NumParameters = reader.ReadUInt32();

            Parameters = new float[NumParameters];
            for (int i = 0; i < NumParameters; i++)
            {
                Parameters[i] = reader.ReadSingle();
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(NodeIndex);
            writer.Write((uint)LinkFnType);

            writer.Write(Parameters.Length);
            foreach(float Parameter in Parameters)
            {
                writer.Write(Parameter);
            }
        }
    }

    public class FxCompiledFaceGraphNode
    {
        public FxCompiledFaceGraphNodeType NodeType { get; set; }
        public uint NodeName { get; set; }
        public float NodeMin { get; set; }
        public float OneOverNodeMin { get; set; }
        public float NodeMax { get; set; }
        public float OneOverNodeMax { get; set; }
        public FxInputOp InputOperation { get; set; }
        public uint ArrayUnk0 { get; set; }
        public uint ArrayUnk1 { get; set; }
        public FxCompiledFaceGraphLink[] InputLinks { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            NodeType = (FxCompiledFaceGraphNodeType)reader.ReadUInt32();
            NodeName = reader.ReadUInt32();
            NodeMin = reader.ReadSingle();
            OneOverNodeMin = reader.ReadSingle();
            NodeMax = reader.ReadSingle();
            OneOverNodeMax = reader.ReadSingle();
            InputOperation = (FxInputOp)reader.ReadUInt32();
            ArrayUnk0 = reader.ReadUInt32();

            InputLinks = new FxCompiledFaceGraphLink[ArrayUnk0];
            for(int i = 0; i < ArrayUnk0; i++)
            {
                FxCompiledFaceGraphLink LinkObject = new FxCompiledFaceGraphLink();
                LinkObject.ReadFromFile(reader);
                InputLinks[i] = LinkObject;
            }

            ArrayUnk1 = reader.ReadUInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((uint)NodeType);
            writer.Write(NodeName);
            writer.Write(NodeMin);
            writer.Write(OneOverNodeMin);
            writer.Write(NodeMax);
            writer.Write(OneOverNodeMax);
            writer.Write((uint)InputOperation);

            writer.Write(InputLinks.Length);
            foreach(FxCompiledFaceGraphLink GraphLink in InputLinks)
            {
                GraphLink.WriteToFile(writer);
            }

            writer.Write(ArrayUnk1);
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

    public class FxActor
    {
        FxClass[] OC3Types;
        string[] StringTable;
        FxPhonToNameMap[] PhoneToNameMap;

        uint ActorNameID;
        uint Unk10;

        public FxMasterBoneListEntry[] MasterBoneList { get; set; }
        public FxUnk3[] Unk03s { get; set; }
        private uint Unk04; // Data in between these two arrays
        public uint Unk05;
        public uint Unk06;
        public FxCompiledFaceGraphNode[] FaceGraphNodes { get; set; }
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

        public void ReadFromFile(BinaryReader reader)
        {
            uint FaceMagic = reader.ReadUInt32();
            if(FaceMagic != 0x45434146)
            {
                // Invalid 'FACE' Magic
                return;
            }

            uint SDKVersion = reader.ReadUInt32();
            if(SDKVersion != 1730)
            {
                // Invalid SDKVersion. 1730 == 1073
                return;
            }

            // 0 == Little, 1 == Big
            uint EndianOrder = reader.ReadUInt32();
            if(EndianOrder != 0)
            {
                // Invalid EndianOrder
                return;
            }

            string LicenseeName = StringHelpers.ReadString32(reader);           // 'Illusion Softworks'
            string LicenseeProjectName = StringHelpers.ReadString32(reader);    // 'Mafia II'

            uint Unk01 = reader.ReadUInt32(); // 1000
            ushort Unk02 = reader.ReadUInt16(); // 1
            uint Unk03 = reader.ReadUInt32(); // 10 - 7. Types of serialized OC3 classes

            // Read used OC3 types
            OC3Types = new FxClass[Unk03];
            for(int i = 0; i < OC3Types.Length; i++)
            {
                FxClass ClassType = new FxClass();
                ClassType.Unk01 = reader.ReadUInt32();
                ClassType.Unk02 = reader.ReadUInt32();
                ClassType.Name = StringHelpers.ReadString32(reader);
                ClassType.Unk03 = reader.ReadUInt16();
                OC3Types[i] = ClassType;
            }

            // Read StringTable
            uint NumStrings = reader.ReadUInt32();
            StringTable = new string[NumStrings];
            for (int i = 0; i < StringTable.Length; i++)
            {
                StringTable[i] = StringHelpers.ReadString32(reader);
            }

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
                DataObject.ReadFromFile(reader);
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

        public void WriteToFile(BinaryWriter writer)
        {
            // Begin to write FaceFX header
            writer.Write(0x45434146);
            writer.Write(1730);
            writer.Write(0); // Only support Little-Endian
            StringHelpers.WriteString32(writer, "IllusionSoftworks\0");
            StringHelpers.WriteString32(writer, "Mafia 2\0");
            writer.Write(1000); // Guessed
            writer.Write((ushort)1); // Guessed

            // Write OC3 types
            writer.Write(OC3Types.Length);
            foreach (var TypeObject in OC3Types)
            {
                TypeObject.WriteToFile(writer);
            }

            // Write StringTable
            writer.Write(StringTable.Length);
            foreach (var Name in StringTable)
            {
                StringHelpers.WriteString32(writer, Name);
            }

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
