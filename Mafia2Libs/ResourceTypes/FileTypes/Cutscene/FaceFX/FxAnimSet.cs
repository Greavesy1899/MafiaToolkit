using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxAnimGroup
    {
        public uint GroupName { get; set; }
        public FxAnim[] Animations { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            GroupName = reader.ReadUInt32();
            uint NumAnims = reader.ReadUInt32();

            Animations = new FxAnim[NumAnims];
            for (int x = 0; x < NumAnims; x++)
            {
                FxAnim AnimObject = new FxAnim();
                AnimObject.ReadFromFile(reader);
                Animations[x] = AnimObject;
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(GroupName);
            writer.Write(Animations.Length);

            foreach(FxAnim Animation in Animations)
            {
                Animation.WriteToFile(writer);
            }
        }
    }

    public class FxAnimSet
    {
        FxClass[] OC3Types;
        string[] StringTable;
        public uint AnimSetName;
        public uint Unk04;
        FxAnimGroup[] AnimGroups;

        uint NumAnimSets;
        uint Size;

        public void ReadFromFile(string filename)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            // TODO: remove this.
            NumAnimSets = reader.ReadUInt32();
            Size = reader.ReadUInt32();

            uint FaceMagic = reader.ReadUInt32();
            if (FaceMagic != 0x45434146)
            {
                // Invalid 'FACE' Magic
                return;
            }

            uint SDKVersion = reader.ReadUInt32();
            if (SDKVersion != 1730)
            {
                // Invalid SDKVersion. 1730 == 1073
                return;
            }

            // 0 == Little, 1 == Big
            uint EndianOrder = reader.ReadUInt32();
            if (EndianOrder != 0)
            {
                // Invalid EndianOrder
                return;
            }

            string LicenseeName = StringHelpers.ReadString32(reader);           // 'Illusion Softworks'
            string LicenseeProjectName = StringHelpers.ReadString32(reader);    // 'Mafia 2'

            uint Unk01 = reader.ReadUInt32(); // 1000
            ushort Unk02 = reader.ReadUInt16(); // 1
            uint NumTypes = reader.ReadUInt32(); // 10 - 7. Types of serialized OC3 classes

            // Read used OC3 types
            OC3Types = new FxClass[NumTypes];
            for (int i = 0; i < OC3Types.Length; i++)
            {
                FxClass ClassType = new FxClass();
                ClassType.ReadFromFile(reader);
                OC3Types[i] = ClassType;
            }

            // Read StringTable
            uint NumStrings = reader.ReadUInt32();
            StringTable = new string[NumStrings];
            for (int i = 0; i < StringTable.Length; i++)
            {
                StringTable[i] = StringHelpers.ReadString32(reader);
            }

            // AnimSet ID
            AnimSetName = reader.ReadUInt32();
            Unk04 = reader.ReadUInt32(); // Unknown; could be data about published or not.

            // Read groups.
            uint NumAnimGroups = reader.ReadUInt32();
            AnimGroups = new FxAnimGroup[NumAnimGroups];
            for(int i = 0; i < NumAnimGroups; i++)
            {
                FxAnimGroup GroupObject = new FxAnimGroup();
                GroupObject.ReadFromFile(reader);
                AnimGroups[i] = GroupObject;
            }
        }

        public void WriteToFile(string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            // TODO: Remove this
            writer.Write(NumAnimSets);
            writer.Write(Size);

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
            foreach(var TypeObject in OC3Types)
            {
                TypeObject.WriteToFile(writer);
            }

            // Write StringTable
            writer.Write(StringTable.Length);
            foreach(var Name in StringTable)
            {
                StringHelpers.WriteString32(writer, Name);
            }

            // Write AnimSet ID
            writer.Write(AnimSetName);
            writer.Write(Unk04);

            writer.Write(AnimGroups.Length);
            foreach(FxAnimGroup Group in AnimGroups)
            {
                Group.WriteToFile(writer);
            }
        }
    }
}
