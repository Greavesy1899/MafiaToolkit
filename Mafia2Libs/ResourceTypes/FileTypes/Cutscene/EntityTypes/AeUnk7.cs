using System.Diagnostics;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeUnk7 : AeBase
    {
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
        }
        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeUnk7;
        }
    }

    public class AeUnk7Data : AeBaseData
    {
        public uint Unk02 { get; set; }
        public uint Unk03 { get; set; }
        public float Unk04 { get; set; }
        public int Unk05 { get; set; }
        public uint Unk06 { get; set; }
        public byte Unk07 { get; set; }

        private bool bHasDerivedData;


        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Debug.Assert(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            if (stream.Position != stream.Length)
            {
                Unk02 = stream.ReadUInt32(isBigEndian);
                Unk03 = stream.ReadUInt32(isBigEndian);
                Unk04 = stream.ReadSingle(isBigEndian);
                Unk05 = stream.ReadInt32(isBigEndian);
                Unk06 = stream.ReadUInt32(isBigEndian);
                Unk07 = stream.ReadByte8();
                bHasDerivedData = true;
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            if(bHasDerivedData)
            {
                stream.Write(Unk02, isBigEndian);
                stream.Write(Unk03, isBigEndian);
                stream.Write(Unk04, isBigEndian);
                stream.Write(Unk05, isBigEndian);
                stream.Write(Unk06, isBigEndian);
                stream.WriteByte(Unk07);
            }
        }
    }
}
