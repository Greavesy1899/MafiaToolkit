using System.Diagnostics;
using System.IO;
using Utils.Extensions;

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
        public byte NumUnknownSize { get; set; }
        public byte[] Unk0 { get; set; }

        private bool bHasDerivedData;

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Debug.Assert(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            if(stream.Position != stream.Length)
            {
                NumUnknownSize = stream.ReadByte8();
                Debug.Assert(NumUnknownSize == 0, "Detected AeUnk31Data::NumUnknownSize != 0. This means data we do not understand!");

                bHasDerivedData = true;

                if (NumUnknownSize > 0)
                {
                    Unk0 = stream.ReadBytes(96);
                }
            }         
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            if(bHasDerivedData)
            {
                stream.WriteByte(NumUnknownSize);

                if (NumUnknownSize > 0)
                {
                    stream.Write(Unk0);
                }
            }
        }
    }
}