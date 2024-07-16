using ResourceTypes.Cutscene.AnimEntities.LightTypes;
using System.ComponentModel;
using System.IO;
using Toolkit.Mathematics;
using Utils.Extensions;
using Utils.Logging;

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
        public ulong Unk06 { get; set; }
        public ulong Unk07 { get; set; }
        public Matrix44 Transform { get; set; } = new();
        public int Unk09 { get; set; }
        public int Unk10 { get; set; }
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
            Unk10 = stream.ReadInt32(isBigEndian);

            if (Unk10 == 0) // Mostly the same as AeOmniLight, but with 12 floats rather than 10.
            {
                // TODO: Why does OmniLight and SpotLight have different array sizes?
                AeLightType0 LightType0 = new AeLightType0();
                LightType0.SetNumFloats(10);
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
                var LightType2 = new AeLightType2();

                if (Unk09 == 0)
                {
                    LightType2.SetNumFloats(10);
                }

                LightType2.ReadFromFile(stream, isBigEndian);
                LightInfo = LightType2;
            }
            else
            {
                //throw new FileFormatException();
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
            stream.Write(Unk10, isBigEndian);
            LightInfo.WriteToFile(stream, isBigEndian);
            UpdateSize(stream, isBigEndian);
        }
    }

    // Same as AeSpotlightData, could be a base class for lights here.
    public class AeOmniLightData : AeBaseData
    {
        public byte Unk02 { get; set; }
        public ulong[] Unk03 { get; set; } = new ulong[0];
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk02 = stream.ReadByte8();

            // Could be an array of integers
            if (Unk02 == 1)
            {
                int Count = stream.ReadInt32(isBigEndian);
                Unk03 = new ulong[Count];

                for (int i = 0; i < Count; i++)
                {
                    Unk03[i] = stream.ReadUInt64(isBigEndian);
                }
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
                stream.Write(Unk03.Length, isBigEndian);

                foreach (var val in Unk03)
                {
                    stream.Write(val, isBigEndian);
                }
            }
            else if (Unk02 != 0)
            {
                throw new FileFormatException();
            }
        }
    }

}
