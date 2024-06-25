using System.IO;
using Utils.Extensions;
using Utils.Logging;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeUnk32Wrapper : AnimEntityWrapper
    {
        public AeUnk32Wrapper() : base()
        {
            AnimEntityData = new AeUnk32Data();
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeUnk32;
        }
    }

    public class AeUnk32Data : AeBaseData
    {
        public uint Unk02 { get; set; }

        private bool bHasDerivedData;


        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            if (stream.Position != stream.Length)
            {
                Unk02 = stream.ReadUInt32(isBigEndian);
                bHasDerivedData = true;
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            if (bHasDerivedData)
            {
                stream.Write(Unk02, isBigEndian);
            }
        }
    }
}
