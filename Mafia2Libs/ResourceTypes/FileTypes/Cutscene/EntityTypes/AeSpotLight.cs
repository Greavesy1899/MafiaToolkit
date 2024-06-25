using ResourceTypes.Cutscene.AnimEntities.LightTypes;
using System.ComponentModel;
using System.IO;
using Toolkit.Mathematics;
using Utils.Extensions;
using Utils.Logging;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeSpotLightWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeSpotLight SpotLightEntity { get; set; }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeSpotLightTarget SpotLightTargetEntity { get; set; }

        public AeSpotLightWrapper() : base()
        {
            SpotLightEntity = new AeSpotLight();
            SpotLightTargetEntity = new AeSpotLightTarget();
            AnimEntityData = new AeSpotLightData();
        }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            SpotLightEntity.ReadFromFile(stream, isBigEndian);
            SpotLightTargetEntity.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            SpotLightEntity.WriteToFile(stream, isBigEndian);
            SpotLightTargetEntity.WriteToFile(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeSpotLight;
        }
    }

    //AeSpotLight
    public class AeSpotLight : AnimEntity
    {
        public byte Unk05 { get; set; }
        public ulong Unk06 { get; set; }
        public ulong Unk07 { get; set; }     
        public Matrix44 Transform { get; set; } = new();
        public int Unk09 { get; set; }
        public int UnknownSize { get; set; }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IAeLightType LightInfo { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk05 = stream.ReadByte8();
            Unk06 = stream.ReadUInt64(isBigEndian);

            if (Unk06 != 0)
            {
                Unk07 = stream.ReadUInt64(isBigEndian);
            }
            
            Transform.ReadFromFile(stream, isBigEndian);
            Unk09 = stream.ReadInt32(isBigEndian);
            UnknownSize = stream.ReadInt32(isBigEndian);

            if (UnknownSize == 0) // Mostly the same as AeOmniLight, but with 12 floats rather than 10.
            {
                // TODO: Why does OmniLight and SpotLight have different array sizes?
                AeLightType0 LightType0 = new AeLightType0();
                LightType0.SetNumFloats(12);
                LightType0.ReadFromFile(stream, isBigEndian);
                LightInfo = LightType0;
            }
            else if(UnknownSize == 1) // This is exactly the same as AeOmniLight. Maybe this is apart of some big type for lights?
            {
                LightInfo = new AeLightType1();
                LightInfo.ReadFromFile(stream, isBigEndian);
            }
            else if(UnknownSize == 2) // Mostly the same as AeOmniLight, but with 12 floats rather than 10.
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

            if (Unk06 != 0)
            {
                stream.Write(Unk07, isBigEndian);
            }
            
            Transform.WriteToFile(stream, isBigEndian);
            stream.Write(Unk09, isBigEndian);
            stream.Write(UnknownSize, isBigEndian);
            LightInfo.WriteToFile(stream, isBigEndian);
            UpdateSize(stream, isBigEndian);
        }
        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeSpotLight;
        }
    }

    public class AeSpotLightData : AeBaseData
    {
        public byte Unk02 { get; set; }
        public int Unk03 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

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
