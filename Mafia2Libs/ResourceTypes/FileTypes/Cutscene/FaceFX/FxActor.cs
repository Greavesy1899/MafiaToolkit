using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }

    public class SerializedOC3Type
    {
        public uint Unk01 { get; set; }
        public uint Unk02 { get; set; }
        public string Name { get; set; }
        public ushort Unk03 { get; set; }

        public override string ToString()
        {
            return Name;
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

        public override string ToString()
        {
            return PossibleIdx.ToString();
        }
    }

    public class FxUnk02
    {
        public byte[] DataPart0 { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            DataPart0 = reader.ReadBytes(36);
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

        public override string ToString()
        {
            return string.Format("{0}", Nodes[0].NodeName);
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
    }

    public class FxActor
    {
        SerializedOC3Type[] OC3Types;
        string[] StringTable;
        FxPhonToNameMap[] PhoneToNameMap;

        public void ReadFromFile(string filename)
        {
            using(BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                ReadFromFile(reader);
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
            OC3Types = new SerializedOC3Type[Unk03];
            for(int i = 0; i < OC3Types.Length; i++)
            {
                SerializedOC3Type ClassType = new SerializedOC3Type();
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
            uint ActorNameIndex = reader.ReadUInt32();
            uint Unk04 = reader.ReadUInt32(); // Unknown; could be data about published or not.

            // Read FxUnk0
            uint NumUnk0s = reader.ReadUInt32();
            FxMasterBoneListEntry[] Unk01s = new FxMasterBoneListEntry[NumUnk0s];
            for (int i = 0; i < Unk01s.Length; i++)
            {
                FxMasterBoneListEntry DataObject = new FxMasterBoneListEntry();
                DataObject.ReadFromFile(reader);
                Unk01s[i] = DataObject;
            }

            // Read FxUnk1
            uint NumUnk1s = reader.ReadUInt32();
            FxUnk02[] Unk02s = new FxUnk02[NumUnk1s];
            for (int i = 0; i < Unk02s.Length; i++)
            {
                FxUnk02 DataObject = new FxUnk02();
                DataObject.ReadFromFile(reader);
                Unk02s[i] = DataObject;
            }

            reader.BaseStream.Seek(0xa4e9, SeekOrigin.Begin);
            uint NumUnk2s = reader.ReadUInt32();
            FxUnk3[] Unk03s = new FxUnk3[NumUnk2s];
            for (int i = 0; i < Unk03s.Length; i++)
            {
                FxUnk3 ClassObject = new FxUnk3();
                ClassObject.ReadFromFile(reader);
                Unk03s[i] = ClassObject;
            }

            reader.BaseStream.Seek(0xb252, SeekOrigin.Begin);
            uint PhonToNameMapCount = reader.ReadUInt32();
            PhoneToNameMap = new FxPhonToNameMap[PhonToNameMapCount];
            for (int i = 0; i < PhonToNameMapCount; i++)
            {
                FxPhonToNameMap PhonToName = new FxPhonToNameMap();
                PhonToName.ReadFromFIle(reader);
                PhoneToNameMap[i] = PhonToName;
            }
        }
    }
}
