using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Toolkit.Mathematics;
using Utils.Extensions;
using Utils.Logging;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeTargetCameraWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeCamera CameraEntity { get; set; }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeTargetCamera TargetCameraEntity { get; set; }

        public AeTargetCameraWrapper() : base()
        {
            CameraEntity = new AeCamera();
            TargetCameraEntity = new AeTargetCamera();
            AnimEntityData = new AeTargetCameraData();
        }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            CameraEntity.ReadFromFile(stream, isBigEndian);
            TargetCameraEntity.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            CameraEntity.WriteToFile(stream, isBigEndian);
            TargetCameraEntity.WriteToFile(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeTargetCamera;
        }
    }

    public class AeTargetCamera : AnimEntity
    {
        public byte Unk14 { get; set; }
        public ulong Hash6 { get; set; }
        public ulong Hash7 { get; set; }
        public Matrix44 Transform1 { get; set; } = new();

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk14 = stream.ReadByte8();
            Hash6 = stream.ReadUInt64(isBigEndian);

            if (Hash6 != 0)
            {
                Hash7 = stream.ReadUInt64(isBigEndian);
                Transform1.ReadFromFile(stream, isBigEndian);
            }
            else
            {
                Transform1.ReadFromFile(stream, isBigEndian);
                Hash7 = stream.ReadUInt64(isBigEndian);
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk14);
            stream.Write(Hash6, isBigEndian);

            if(Hash6 != 0)
            {
                stream.Write(Hash7, isBigEndian);
                Transform1.WriteToFile(stream, isBigEndian);
            }
            else
            {
                Transform1.WriteToFile(stream, isBigEndian);
                stream.Write(Hash7, isBigEndian);
            }
            UpdateSize(stream, isBigEndian);
            
        }
        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeTargetCamera;
        }
    }

    public class AeTargetCameraData : AeBaseData
    {
        public int Unk02 { get; set; }
        public byte Unk03 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk02 = stream.ReadInt32(isBigEndian);
            Unk03 = stream.ReadByte8();
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk02, isBigEndian);
            stream.WriteByte(Unk03);
        }
    }
}
