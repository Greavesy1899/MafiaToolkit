using System.Diagnostics;
using System.IO;
using Utils.Extensions;
using Utils.Logging;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeSoundEntityWrapper : AnimEntityWrapper
    {
        public AeSoundEntityWrapper() : base()
        {
            AnimEntityData = new AeSoundEntityData();
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeSoundEntity;
        }
    }

    public class AeSoundEntityData : AeBaseData
    {
        public ulong[] UnkHashes_0 { get; set; }

        private bool bHasDerivedData;

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            if(stream.Position != stream.Length)
            {
                byte Size = stream.ReadByte8();             
                UnkHashes_0 = new ulong[Size];
                for(int i = 0; i < Size; i++)
                {
                    UnkHashes_0[i] = stream.ReadUInt64(isBigEndian);
                }

                bHasDerivedData = true;
            }         
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            if(bHasDerivedData)
            {
                stream.WriteByte((byte)UnkHashes_0.Length);
                foreach(var HashValue in UnkHashes_0)
                {
                    stream.Write(HashValue, isBigEndian);
                }
            }
        }
    }
}