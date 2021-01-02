using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.OC3.FaceFX
{
    /*
     * Contains the data from a loaded file.
     * In the actual SDK, I think this is a container holding the data and classes.
     * But here, its just a base class for FxAnimSet and FxActor.
     * Maybe in the future this can be rearranged into the kind of setup.
     */
    public class FxArchive
    {
        // Set of consts for validity.
        private const uint FaceMagic = 0x45434146;
        private const uint ExpectedSDKVersion = 1730; // FaceFX Studio reads this as 1073

        private string LicenseeName;
        private string LicenseeProjectName;

        // Don't know what these are for
        private uint Unk0;
        private ushort Unk1; 

        private FxClass[] SerializedTypes;
        private string[] StringTable;

        public virtual void ReadFromFile(BinaryReader reader)
        {
            uint FileFaceMagic = reader.ReadUInt32();
            if (FileFaceMagic != FaceMagic)
            {
                // Invalid 'FACE' Magic
                return;
            }

            uint SDKVersion = reader.ReadUInt32();
            if (SDKVersion != ExpectedSDKVersion)
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

            LicenseeName = StringHelpers.ReadString32(reader);           // 'Illusion Softworks'
            LicenseeProjectName = StringHelpers.ReadString32(reader);    // 'Mafia II'

            Unk0 = reader.ReadUInt32(); // 1000
            Unk1 = reader.ReadUInt16(); // 1

            // Read used OC3 types
            uint NumSerializedTypes = reader.ReadUInt32();
            SerializedTypes = new FxClass[NumSerializedTypes];
            for (int i = 0; i < SerializedTypes.Length; i++)
            {
                FxClass ClassType = new FxClass();
                ClassType.ReadFromFile(reader);
                SerializedTypes[i] = ClassType;
            }

            // Read StringTable
            uint NumStrings = reader.ReadUInt32();
            StringTable = new string[NumStrings];
            for (int i = 0; i < StringTable.Length; i++)
            {
                StringTable[i] = StringHelpers.ReadString32(reader);
            }
        }

        public virtual void WriteToFile(BinaryWriter writer)
        {
            // Begin to write FaceFX header
            writer.Write(FaceMagic);
            writer.Write(ExpectedSDKVersion);
            writer.Write(0); // Only support Little-Endian
            StringHelpers.WriteString32(writer, "IllusionSoftworks\0");
            StringHelpers.WriteString32(writer, "Mafia 2\0");
            writer.Write(1000); // Guessed
            writer.Write((ushort)1); // Guessed

            // Write OC3 types
            writer.Write(SerializedTypes.Length);
            foreach (var TypeObject in SerializedTypes)
            {
                TypeObject.WriteToFile(writer);
            }

            // Write StringTable
            writer.Write(StringTable.Length);
            foreach (var Name in StringTable)
            {
                StringHelpers.WriteString32(writer, Name);
            }
        }

        public string GetFileName()
        {
            return GetFromStringTable(0);
        }

        public string GetFromStringTable(uint Index)
        {
            if (StringTable != null && StringTable.Length > 0)
            {
                return StringTable[Index];
            }

            return string.Empty;
        }
    }
}
