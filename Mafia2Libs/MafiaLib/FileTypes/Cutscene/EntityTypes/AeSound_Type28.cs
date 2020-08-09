using System;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeSound_Type28 : AeBase
    {
        public short isValid { get; set; }

        public override int GetEntityDefinitionType()
        {
            return 28;
        }

        public override int GetEntityDataType()
        {
            throw new NotImplementedException();
        }

        public override bool ReadDefinitionFromFile(MemoryStream stream, bool isBigEndian)
        {
            isValid = stream.ReadInt16(isBigEndian);
            return true;
        }

        public override bool WriteDefinitionToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(isBigEndian);
            return true;
        }

        public override bool ReadDataFromFile(MemoryStream stream, bool isBigEndian)
        {
            EntityData = new AeBaseData();
            EntityData.ReadFromFile(stream, isBigEndian);
            return true;
        }

        public override bool WriteDataFromFile(MemoryStream stream, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }
}
