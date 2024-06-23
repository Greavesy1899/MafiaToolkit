using System.Diagnostics;
using System.IO;
using Utils.Extensions;
using Utils.Logging;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeCutEditWrapper : AnimEntityWrapper
    {
        public AeCutEditWrapper() : base()
        {
            AnimEntityData = new AeCutEditData();
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeCutEdit;
        }
    }

    public class AeCutEditData : AeBaseData
    {
        public int Unk02 { get; set; }
        public byte Unk03 { get; set; }

        private bool bHasDerivedData;
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length || DataType == 104, "I've read the parent class data, although i've hit the eof!");

            if (stream.Position != stream.Length && DataType != 104)
            {
                Unk02 = stream.ReadInt32(isBigEndian);
                Unk03 = stream.ReadByte8();
                bHasDerivedData = true;
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            if (bHasDerivedData)
            {
                stream.Write(Unk02, isBigEndian);
                stream.Write(Unk03, isBigEndian);
            }
        }
    }
}
