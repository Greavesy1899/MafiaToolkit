using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utils.StringHelpers;

namespace ResourceTypes.OC3.FaceFX
{
    /*
     * Contains the data from a loaded file.
     * Stores the StringTable, FaceFx classes present in the archive,
     * and the object itself.
     */
    public class FxArchive
    {
        // Set of consts for validity.
        private const uint FaceMagic = 0x45434146;
        private const uint ExpectedSDKVersion = 1730; // FaceFX Studio reads this as 1073 // M2C is 1730, M2DE is 1740

        // Interial variables
        private uint SDKVersion;
        private string LicenseeName;
        private string LicenseeProjectName;

        // Don't know what these are for
        private uint Unk0;
        private ushort Unk1; 

        private FxClass[] SerializedTypes;
        private string[] StringTable;

        public FxObject ObjectData { get; set; }

        private Dictionary<string, bool> TempStringTable;

        public virtual void ReadFromFile<T>(BinaryReader reader) where T : FxObject
        {
            uint FileFaceMagic = reader.ReadUInt32();
            if (FileFaceMagic != FaceMagic)
            {
                // Invalid 'FACE' Magic
                return;
            }

            SDKVersion = reader.ReadUInt32();
            if (SDKVersion != ExpectedSDKVersion)
            {
                // Invalid SDKVersion. 1730 == 1073
                //return;
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

            ObjectData = Activator.CreateInstance<T>();
            ObjectData.Deserialize(this, reader);
        }

        public virtual void WriteToFile(BinaryWriter writer)
        {
            PopulateStringTable();

            // Begin to write FaceFX header
            writer.Write(FaceMagic);
            writer.Write(SDKVersion);
            writer.Write(0);
            StringHelpers.WriteString32(writer, LicenseeName);
            StringHelpers.WriteString32(writer, LicenseeProjectName);
            writer.Write(Unk0);
            writer.Write(Unk1);

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

            ObjectData.Serialize(this, writer);
        }

        /*
         * Util function to cast the object as the type you want.
         * Example use case: GetObjectAs<FxAnimSet>().
         */
        public T GetObjectAs<T>() where T : FxObject
        {
            T CastedObject = (ObjectData as T);
            Debug.Assert(CastedObject == null, "This Object was not of type");

            return CastedObject;
        }

        // INTERNAL USE ONLY
        public string GetFromStringTable(uint Index)
        {
            if (StringTable != null && StringTable.Length > 0)
            {
                return StringTable[Index];
            }

            return string.Empty;
        }

        // INTERNAL USE ONLY
        public int GetFromStringTable(string Text)
        {
            return Array.FindIndex(StringTable, e => e == Text);
        }

        // INTERNAL USE ONLY
        public void AddToStringTable(string Text)
        {
            if(!TempStringTable.ContainsKey(Text))
            {
                TempStringTable.Add(Text, false);
            }
        }

        // INTERNAL USE ONLY
        private void PopulateStringTable()
        {
            // Clean existing table and temp table
            TempStringTable = new Dictionary<string, bool>();
            StringTable = new string[0];

            // Generate string table
            ObjectData.PopulateStringTable(this);

            // Move result back into the StringTable,
            // then remove the Temp table
            StringTable = TempStringTable.Keys.ToArray();
            TempStringTable = new Dictionary<string, bool>();
        }
    }
}
