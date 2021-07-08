using ResourceTypes.Cutscene.AnimEntities.LightTypes;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeOmniLightWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeOmniLight OmniLightEntity { get; set; }

        public AeOmniLightWrapper() : base()
        {
            OmniLightEntity = new AeOmniLight();
            AnimEntityData = new AeOmniLightData();
        }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            OmniLightEntity.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            OmniLightEntity.WriteToFile(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeOmniLight;
        }
    }

    public class AeOmniLight : AnimEntity
    {
        public byte Unk05 { get; set; }
        public int Unk06 { get; set; }
        public int Unk07 { get; set; }
        public Matrix4x4 Transform { get; set; }
        public int Unk09 { get; set; }
        public int Unk10 { get; set; }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IAeLightType LightInfo { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk05 = stream.ReadByte8();
            Unk06 = stream.ReadInt32(isBigEndian);
            Unk07 = stream.ReadInt32(isBigEndian);
            Transform = MatrixUtils.ReadFromFile(stream, isBigEndian);
            Unk09 = stream.ReadInt32(isBigEndian);
            Unk10 = stream.ReadInt32(isBigEndian);

            if (Unk10 == 0) // Mostly the same as AeOmniLight, but with 12 floats rather than 10.
            {
                // TODO: Why does OmniLight and SpotLight have different array sizes?
                AeLightType0 LightType0 = new AeLightType0();
                LightType0.SetNumFloats(12);
                LightType0.ReadFromFile(stream, isBigEndian);
                LightInfo = LightType0;
            }
            else if (Unk10 == 1) // This is exactly the same as AeOmniLight. Maybe this is apart of some big type for lights?
            {
                LightInfo = new AeLightType1();
                LightInfo.ReadFromFile(stream, isBigEndian);
            }
            else if (Unk10 == 2) // Mostly the same as AeOmniLight, but with 12 floats rather than 10.
            {
                LightInfo = new AeLightType2();
                LightInfo.ReadFromFile(stream, isBigEndian);
            }
            else
            {
                throw new FileFormatException();
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk05);
            stream.Write(Unk06, isBigEndian);
            stream.Write(Unk07, isBigEndian);
            Transform.WriteToFile(stream, isBigEndian);
            stream.Write(Unk09, isBigEndian);
            stream.Write(Unk10, isBigEndian);
            LightInfo.WriteToFile(stream, isBigEndian);
            UpdateSize(stream, isBigEndian);
        }
    }

    // Same as AeSpotlightData, could be a base class for lights here.
    public class AeOmniLightData : AeBaseData
    {
        public byte Unk02 { get; set; }
        public int Unk03 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Debug.Assert(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk02 = stream.ReadByte8();

            // Could be an array of integers
            if (Unk02 == 1)
            {
                Unk03 = stream.ReadInt32(isBigEndian);
            }
            else if (Unk02 != 0)
            {
                throw new FileFormatException();
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk02);

            // Could be an array of integers
            if (Unk02 == 1)
            {
                stream.Write(Unk03, isBigEndian);
            }
            else if (Unk02 != 0)
            {
                throw new FileFormatException();
            }
        }
    }

}
