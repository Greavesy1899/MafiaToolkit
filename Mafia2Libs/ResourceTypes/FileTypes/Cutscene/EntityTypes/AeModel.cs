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
    public class AeModelWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeModel ModelEntity { get; set; }

        public AeModelWrapper()
        {
            ModelEntity = new AeModel();
            AnimEntityData = new AeModelData();
        }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            ModelEntity = new AeModel();
            ModelEntity.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            ModelEntity.WriteToFile(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeModel;
        }
    }

    public class AeModel : AnimEntity
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
    }

    public class AeModelData : AeBaseData
    {
        public int Unk02 { get; set; } // 0
        public Vector3 Unk03 { get; set; } // 0.2f, 0.2f, 0.2f
        public float Unk04 { get; set; } // 7.0f
        public byte[] Unk05 { get; set; } // For me, this was all empty
        public float Unk06 { get; set; } // 1.0f;
        public int Unk07 { get; set; }
        public byte Unk08 { get; set; }

        // Not saved in file, but required.
        private bool bDoesHaveExtraData;

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length || DataType == 105, "I've read the parent class data, although i've hit the eof!");

            if (stream.Position != stream.Length)
            {
                Unk02 = stream.ReadInt32(isBigEndian);
                Unk03 = Vector3Utils.ReadFromFile(stream, isBigEndian);
                Unk04 = stream.ReadSingle(isBigEndian);
                Unk05 = stream.ReadBytes(24);
                Unk06 = stream.ReadSingle(isBigEndian);
                Unk07 = stream.ReadInt32(isBigEndian);
                Unk08 = stream.ReadByte8();

                bDoesHaveExtraData = true;
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            if (bDoesHaveExtraData)
            {
                stream.Write(Unk02, isBigEndian);
                Unk03.WriteToFile(stream, isBigEndian);
                stream.Write(Unk04, isBigEndian);
                stream.Write(Unk05);
                stream.Write(Unk06, isBigEndian);
                stream.Write(Unk07, isBigEndian);
                stream.WriteByte(Unk08);
            }
        }
    }
}
