using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_ObjectBundle : IValidator
    {
        private const string FileHeader = "MTB";
        private const int FileVersion = 0;

        public MT_Object[] Objects { get; set; }

        public bool ReadFromFile(BinaryReader reader)
        {
            string TempHeader = new string(reader.ReadChars(3));
            if(!TempHeader.Equals(FileHeader))
            {
                return false;
            }

            int TempFileVersion = reader.ReadByte();
            if(TempFileVersion != FileVersion)
            {
                return false;
            }

            uint NumObjects = reader.ReadUInt32();
            Objects = new MT_Object[NumObjects];

            for(int i = 0; i < NumObjects; i++)
            {
                MT_Object NewObject = new MT_Object();
                bool bIsValid = NewObject.ReadFromFile(reader);
                Objects[i] = NewObject;

                // Failed to read Object, return
                if(!bIsValid)
                {
                    return false;
                }
            }

            return true;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            StringHelpers.WriteString(writer, "MTB", false);
            writer.Write((byte)FileVersion);

            // Write Models to file
            writer.Write(Objects.Length);
            foreach(MT_Object ModelObject in Objects)
            {
                ModelObject.WriteToFile(writer);
            }
        }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            bool bIsValid = true;

            foreach(MT_Object ModelObject in Objects)
            {
                bool bIsObjectValid = ModelObject.ValidateObject(TrackerObject);
                bIsValid &= bIsObjectValid;
            }

            return bIsValid;
        }
    }
}
