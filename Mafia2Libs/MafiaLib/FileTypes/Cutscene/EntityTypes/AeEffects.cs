using System;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeEffects : AeBase
    {
        public short isValid { get; set; }

        public override int GetEntityDefinitionType()
        {
            return 25;
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
            EntityData = new AeEffectsBase();
            EntityData.ReadFromFile(stream, isBigEndian);
            return true;
        }

        public override bool WriteDataFromFile(MemoryStream stream, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }

    public class AeEffectsBase : AeBaseData
    {
        public int Unk02 { get; set; }
        public int Unk03 { get; set; }
        public int Unk04 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk02 = stream.ReadInt32(isBigEndian);
            Unk03 = stream.ReadInt32(isBigEndian);
            Unk04 = stream.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk02, isBigEndian);
            stream.Write(Unk03, isBigEndian);
            stream.Write(Unk04, isBigEndian);
        }
    }
}
