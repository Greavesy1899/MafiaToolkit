using System.ComponentModel;
using System.IO;
using Toolkit.Mathematics;
using Utils.Extensions;
using Utils.Logging;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeCameraWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeCamera CameraEntity { get; set; }
        public AeCameraWrapper() : base()
        {
            CameraEntity = new AeCamera();
            AnimEntityData = new AeCameraData();
        }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            CameraEntity.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            CameraEntity.WriteToFile(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeCamera;
        }
    }

    //AeCameraLink
    public class AeCamera : AnimEntity
    {
        public byte Unk05 { get; set; }
        public int Unk06 { get; set; }
        public int Unk07 { get; set; }
        public Matrix44 Transform { get; set; } = new();
        public float FOV { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk05 = stream.ReadByte8();
            Unk06 = stream.ReadInt32(isBigEndian);
            Unk07 = stream.ReadInt32(isBigEndian);
            Transform.ReadFromFile(stream, isBigEndian);
            FOV = stream.ReadSingle(isBigEndian);
            Near = stream.ReadSingle(isBigEndian);
            Far = stream.ReadSingle(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk05);
            stream.Write(Unk06, isBigEndian);
            stream.Write(Unk07, isBigEndian);
            Transform.WriteToFile(stream, isBigEndian);
            stream.Write(FOV, isBigEndian);
            stream.Write(Near, isBigEndian);
            stream.Write(Far, isBigEndian);
            UpdateSize(stream, isBigEndian);
        }
        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeCamera;
        }
    }

    public class AeCameraData : AeBaseData
    {
        public int Unk4_01 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            ToolkitAssert.Ensure(stream.Position != stream.Length, "AeUnk4Data's ReadFromFile has reached the eos, but we still need to read an extra piece of data!");
            Unk4_01 = stream.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk4_01, isBigEndian);
        }
    }
}
