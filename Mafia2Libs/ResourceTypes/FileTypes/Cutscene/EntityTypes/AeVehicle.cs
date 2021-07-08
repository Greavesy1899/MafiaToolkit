using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
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
            return AnimEntityTypes.AeFrame;
        }
    }

    public class AeVehicle : AnimEntity
    {
        public byte Unk06 { get; set; }
        public int Unk07 { get; set; }
        public int Unk08 { get; set; }
        public Matrix4x4 Transform { get; set; }
        public string Name4 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk06 = stream.ReadByte8();
            Unk07 = stream.ReadInt32(isBigEndian);
            Unk08 = stream.ReadInt32(isBigEndian);
            Transform = MatrixUtils.ReadFromFile(stream, isBigEndian);
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
}
