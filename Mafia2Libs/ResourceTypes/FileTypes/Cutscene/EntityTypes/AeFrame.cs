using System.ComponentModel;
using System.IO;
using System.Numerics;
using Toolkit.Mathematics;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeFrameWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeFrame FrameEntity { get; set; }

        public AeFrameWrapper() : base()
        {
            FrameEntity = new AeFrame();
            AnimEntityData = new AeFrameData();
        }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            FrameEntity.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            FrameEntity.WriteToFile(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeFrame;
        }
    }

    // TODO: I don't really understand this data; we need to understand it though.
    // It looks to reference prior hashes which can be found in the base class, 
    // and then stores hashes/transforms for these objects? Unknown though, most fail to save.
    public class AeFrame : AnimEntity
    {
        public byte Unk06 { get; set; }
        public ulong Hash2 { get; set; }
        public Matrix44 Transform { get; set; } = new();
        public float Unk07 { get; set; }
        public float Unk08 { get; set; }
        public Matrix44 Transform1 { get; set; } = new();
        public ulong Hash3 { get; set; }
        public byte[] UnknownData { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            UnknownData = stream.ReadBytes((int)Size-8);
            /*
            Unk06 = stream.ReadByte8();

            Hash2 = stream.ReadUInt64(isBigEndian);

            if (!string.IsNullOrEmpty(EntityName1))
            {
                Hash3 = stream.ReadUInt64(isBigEndian);
            }
                
            Transform.ReadFromFile(stream, isBigEndian);

            // TODO: Find out what this actually means.
            // This cannot be distinguished by size alone.
            if(Unk044 == 121)
            {
                Transform1.ReadFromFile(stream, isBigEndian);
            }
            */
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(UnknownData);

            /*
            stream.WriteByte(Unk06);

            stream.Write(Hash2, isBigEndian);

            if (!string.IsNullOrEmpty(FrameName))
            {
                stream.Write(Hash3, isBigEndian);
            }
            
            Transform.WriteToFile(stream, isBigEndian);

            // TODO: Find out what this actually means.
            // This cannot be distinguished by size alone.
            if (Unk044 == 121)
            {
                Transform1.WriteToFile(stream, isBigEndian);
            }
            */

            UpdateSize(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeFrame;
        }
    }

    public class AeFrameData : AeBaseData
    {
        public int Unk02 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk02 = stream.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk02, isBigEndian);
        }
    }
}
