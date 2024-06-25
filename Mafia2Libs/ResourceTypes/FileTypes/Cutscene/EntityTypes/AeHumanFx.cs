using System.ComponentModel;
using System.IO;
using Toolkit.Mathematics;
using Utils.Extensions;
using Utils.Logging;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeHumanFxWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeHumanFx HumanEntity { get; set; }

        public AeHumanFxWrapper() : base()
        {
            HumanEntity = new AeHumanFx();
            AnimEntityData = new AeUnk23Data();
        }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            HumanEntity.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            HumanEntity.WriteToFile(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeHumanFx;
        }
    }

    public class AeHumanFx : AnimEntity
    {
        public byte Unk06 { get; set; }
        public ulong Unk07 { get; set; } //Parent Hash?
        public ulong Unk08 { get; set; }
        public Matrix44 Transform { get; set; } = new();
        public string Name4 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk06 = stream.ReadByte8();
            Unk07 = stream.ReadUInt64(isBigEndian);

            if (Unk07 != 0)
            {
                Unk08 = stream.ReadUInt64(isBigEndian);
            }

            Transform.ReadFromFile(stream, isBigEndian);
            Name4 = stream.ReadString16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk06);
            stream.Write(Unk07, isBigEndian);

            if (Unk07 != 0)
            {
                stream.Write(Unk08, isBigEndian);
            }

            Transform.WriteToFile(stream, isBigEndian);
            stream.WriteString16(Name4, isBigEndian);
            UpdateSize(stream, isBigEndian);
        }
        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeHumanFx;
        }
    }

    public class AeUnk23Data : AeBaseData
    {
        public string ActorName { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            ActorName = stream.ReadString16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteString16(ActorName, isBigEndian);
        }
    }
}
