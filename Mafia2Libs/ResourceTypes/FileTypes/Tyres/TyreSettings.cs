using System.IO;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Tyres
{
    [PropertyClassAllowReflection]
    public class TyreSettings
    {
        public int Index { get; set; }
        public float Unk00 { get; set; }
        public float Unk01 { get; set; }
        public float Unk02 { get; set; }
        public float Unk03 { get; set; }
        public float Unk04 { get; set; }
        public float Unk05 { get; set; }
        public float Unk06 { get; set; }
        public float Unk07 { get; set; }
        public float Unk08 { get; set; }
        public float Unk09 { get; set; }
        public float Unk10 { get; set; }
        public float Unk11 { get; set; }
        public float Unk12 { get; set; }
        public float Unk13 { get; set; }
        public float Unk14 { get; set; }
        public float Unk15 { get; set; }
        public float Unk16 { get; set; }
        public float Unk17 { get; set; }
        public float Unk18 { get; set; }
        public float Unk19 { get; set; }
        public float Unk20 { get; set; }
        public float Unk21 { get; set; }
        public float Unk22 { get; set; }
        public float Unk23 { get; set; }
        public float Unk24 { get; set; }

        public TyreSettings()
        {

        }

        public TyreSettings(MemoryStream stream)
        {
            Read(stream);
        }

        public void Read(MemoryStream stream)
        {
            Index = stream.ReadInt32(false);
            Unk00 = stream.ReadSingle(false);
            Unk01 = stream.ReadSingle(false);
            Unk02 = stream.ReadSingle(false);
            Unk03 = stream.ReadSingle(false);
            Unk04 = stream.ReadSingle(false);
            Unk05 = stream.ReadSingle(false);
            Unk06 = stream.ReadSingle(false);
            Unk07 = stream.ReadSingle(false);
            Unk08 = stream.ReadSingle(false);
            Unk09 = stream.ReadSingle(false);
            Unk10 = stream.ReadSingle(false);
            Unk11 = stream.ReadSingle(false);
            Unk12 = stream.ReadSingle(false);
            Unk13 = stream.ReadSingle(false);
            Unk14 = stream.ReadSingle(false);
            Unk15 = stream.ReadSingle(false);
            Unk16 = stream.ReadSingle(false);
            Unk17 = stream.ReadSingle(false);
            Unk18 = stream.ReadSingle(false);
            Unk19 = stream.ReadSingle(false);
            Unk20 = stream.ReadSingle(false);
            Unk21 = stream.ReadSingle(false);
            Unk22 = stream.ReadSingle(false);
            Unk23 = stream.ReadSingle(false);
            Unk24 = stream.ReadSingle(false);
        }

        public void Write(MemoryStream stream, bool isBigEndian)
        {
            stream.Write(Index, isBigEndian);
            stream.Write(Unk00, isBigEndian);
            stream.Write(Unk01, isBigEndian);
            stream.Write(Unk02, isBigEndian);
            stream.Write(Unk03, isBigEndian);
            stream.Write(Unk04, isBigEndian);
            stream.Write(Unk05, isBigEndian);
            stream.Write(Unk06, isBigEndian);
            stream.Write(Unk07, isBigEndian);
            stream.Write(Unk08, isBigEndian);
            stream.Write(Unk09, isBigEndian);
            stream.Write(Unk10, isBigEndian);
            stream.Write(Unk11, isBigEndian);
            stream.Write(Unk12, isBigEndian);
            stream.Write(Unk13, isBigEndian);
            stream.Write(Unk14, isBigEndian);
            stream.Write(Unk15, isBigEndian);
            stream.Write(Unk16, isBigEndian);
            stream.Write(Unk17, isBigEndian);
            stream.Write(Unk18, isBigEndian);
            stream.Write(Unk19, isBigEndian);
            stream.Write(Unk20, isBigEndian);
            stream.Write(Unk21, isBigEndian);
            stream.Write(Unk22, isBigEndian);
            stream.Write(Unk23, isBigEndian);
            stream.Write(Unk24, isBigEndian);
        }
    }
}
