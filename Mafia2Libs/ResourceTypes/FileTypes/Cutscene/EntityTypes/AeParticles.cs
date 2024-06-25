using System.IO;
using Utils.Extensions;
using Utils.Logging;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeParticlesWrapper : AnimEntityWrapper
    {
        public AeParticlesWrapper() : base()
        {
            AnimEntityData = new AeParticlesData();
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeParticles;
        }
    }

    public class AeParticlesData : AeBaseData
    {
        public int Unk02 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk02 = stream.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk02, isBigEndian);
        }
    }
}
