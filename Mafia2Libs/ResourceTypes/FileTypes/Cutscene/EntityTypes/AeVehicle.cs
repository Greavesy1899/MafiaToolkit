using System.ComponentModel;
using System.IO;
using System.Numerics;
using Toolkit.Mathematics;
using Utils.Extensions;
using Utils.Logging;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeVehicleWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeVehicle VehicleEntity { get; set; }

        public AeVehicleWrapper() : base()
        {
            VehicleEntity = new AeVehicle();
            AnimEntityData = new AeVehicleData();
        }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            VehicleEntity = new AeVehicle();
            VehicleEntity.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            VehicleEntity.WriteToFile(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeVehicle;
        }
    }

    public class AeVehicle : AnimEntity
    {
        public byte Unk06 { get; set; }
        public int Unk07 { get; set; }
        public int Unk08 { get; set; }
        public Matrix44 Transform { get; set; } = new();
        public string Name4 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk06 = stream.ReadByte8();
            Unk07 = stream.ReadInt32(isBigEndian);
            Unk08 = stream.ReadInt32(isBigEndian);
            Transform.ReadFromFile(stream, isBigEndian);
            Name4 = stream.ReadString16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk06);
            stream.Write(Unk07, isBigEndian);
            stream.Write(Unk08, isBigEndian);
            Transform.WriteToFile(stream, isBigEndian);
            stream.WriteString16(Name4, isBigEndian);
            UpdateSize(stream, isBigEndian);
        }
        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeVehicle;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AeVehicleData : AeBaseData
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class AeVehicleDataWrapper
        {
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class AeVehicleVectorWrapper
            {
                public byte Unk00 { get; set; }
                public Vector4[] Unk01 { get; set; } = new Vector4[0];
                public AeVehicleVectorWrapper()
                {

                }

                public AeVehicleVectorWrapper(MemoryStream stream, bool isBigEndian)
                {
                    Read(stream, isBigEndian);
                }

                public void Read(MemoryStream stream, bool isBigEndian)
                {
                    Unk00 = stream.ReadByte8();

                    switch (Unk00)
                    {
                        case 1:
                            Unk01 = new Vector4[6];
                            break;

                        default:
                            Unk01 = new Vector4[1];
                            break;
                    }

                    for (int i = 0; i < Unk01.Length; i++)
                    {
                        Unk01[i] = Vector4Extenders.ReadFromFile(stream, isBigEndian);
                    }
                }

                public void Write(MemoryStream stream, bool isBigEndian)
                {
                    stream.WriteByte(Unk00);
                    
                    foreach (var val in Unk01)
                    {
                        val.WriteToFile(stream, isBigEndian);
                    }
                }
            }

            public ulong Hash { get; set; }
            public AeVehicleVectorWrapper[] Vectors { get; set; } = new AeVehicleVectorWrapper[0];
            public AeVehicleDataWrapper()
            {

            }

            public AeVehicleDataWrapper(MemoryStream stream, bool isBigEndian)
            {
                Read(stream, isBigEndian);
            }

            public void Read(MemoryStream stream, bool isBigEndian)
            {
                Hash = stream.ReadUInt64(isBigEndian);
                int Count = stream.ReadInt32(isBigEndian);

                Vectors = new AeVehicleVectorWrapper[Count];

                for (int i = 0; i < Count; i++)
                {
                    Vectors[i] = new(stream, isBigEndian);
                }
            }

            public void Write(MemoryStream stream, bool isBigEndian)
            {
                stream.Write(Hash, isBigEndian);
                stream.Write(Vectors.Length, isBigEndian);

                foreach (var val in Vectors)
                {
                    val.Write(stream, isBigEndian);
                }
            }
        }

        public AeVehicleDataWrapper[] UnkVehicleData { get; set; } = new AeVehicleDataWrapper[0];

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            if ((Unk01 >> 16) < 7)
            {
                return;
            }

            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            int Count = stream.ReadInt32(isBigEndian);
            UnkVehicleData = new AeVehicleDataWrapper[Count];

            for (int i = 0; i < Count; i++)
            {
                UnkVehicleData[i] = new(stream, isBigEndian);
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            if ((Unk01 >> 16) < 7)
            {
                return;
            }

            stream.Write(UnkVehicleData.Length, isBigEndian);

            foreach (var val in UnkVehicleData)
            {
                val.Write(stream, isBigEndian);
            }
        }
    }
}
