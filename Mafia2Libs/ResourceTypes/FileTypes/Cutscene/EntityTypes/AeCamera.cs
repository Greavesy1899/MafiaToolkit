using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeCameraWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeCamera Unk4Entity { get; set; }
        public AeCameraWrapper() : base()
        {
            Unk4Entity = new AeCamera();
            AnimEntityData = new AeCameraData();
        }
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
            return AnimEntityTypes.AeCamera;
        }
    }

    //AeCameraLink
    public class AeCamera : AnimEntity
    {
        public byte Unk05 { get; set; }
        public int Unk06 { get; set; }
        public int Unk07 { get; set; }
        public Matrix Transform { get; set; }
        public float Unk08 { get; set; }
        public float Unk09 { get; set; }
        public float Unk10 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk05 = stream.ReadByte8();
            Unk06 = stream.ReadInt32(isBigEndian);
            Unk07 = stream.ReadInt32(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Unk08 = stream.ReadSingle(isBigEndian);
            Unk09 = stream.ReadSingle(isBigEndian);
            Unk10 = stream.ReadSingle(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk05);
            stream.Write(Unk06, isBigEndian);
            stream.Write(Unk07, isBigEndian);
            Transform.WriteToFile(stream, isBigEndian);
            stream.Write(Unk08, isBigEndian);
            stream.Write(Unk09, isBigEndian);
            stream.Write(Unk10, isBigEndian);
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

            Debug.Assert(stream.Position != stream.Length, "AeUnk4Data's ReadFromFile has reached the eos, but we still need to read an extra piece of data!");
            Unk4_01 = stream.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk4_01, isBigEndian);
        }
    }
}
