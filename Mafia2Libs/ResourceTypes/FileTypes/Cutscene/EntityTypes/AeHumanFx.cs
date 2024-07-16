using System.ComponentModel;
using System.IO;
using System.Numerics;
using Toolkit.Mathematics;
using Utils.Extensions;
using Utils.Logging;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeHumanFxWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeHumanFx HumanEntity { get; set; }

        public AeHumanFxWrapper() : base()
        {
            HumanEntity = new AeHumanFx();
            AnimEntityData = new AeHumanFxData();
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

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AeHumanFxData : AeBaseData
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class AeHumanFxUnkData
        {
            public ulong Hash { get; set; }
            public byte Unk00 { get; set; }
            public int Unk01 { get; set; }
            public Vector4[] Unk02 { get; set; } = new Vector4[0];
            public AeHumanFxUnkData()
            {

            }

            public AeHumanFxUnkData(MemoryStream stream, bool isBigEndian)
            {
                Read(stream, isBigEndian);
            }

            public void Read(MemoryStream stream, bool isBigEndian)
            {
                Hash = stream.ReadUInt64(isBigEndian);
                Unk00 = stream.ReadByte8();
                Unk01 = stream.ReadInt32(isBigEndian);

                switch (Unk01)
                {
                    default:
                        Unk02 = new Vector4[3];
                        break;
                }

                for (int i = 0; i < Unk02.Length; i++)
                {
                    Unk02[i] = Vector4Extenders.ReadFromFile(stream, isBigEndian);
                }
            }

            public void Write(MemoryStream stream, bool isBigEndian)
            {
                stream.Write(Hash, isBigEndian);
                stream.WriteByte(Unk00);
                stream.Write(Unk01, isBigEndian);

                foreach (var val in Unk02)
                {
                    val.WriteToFile(stream, isBigEndian);
                }
            }
        }

        public string ActorName { get; set; }
        public AeHumanFxUnkData[] UnkHumanFxData { get; set; } = new AeHumanFxUnkData[0];
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            ToolkitAssert.Ensure(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            ActorName = stream.ReadString16(isBigEndian);

            if ((Unk01 >> 16) != 2)
            {
                return;
            }

            int Count = stream.ReadInt32(isBigEndian);
            UnkHumanFxData = new AeHumanFxUnkData[Count];

            for (int i = 0; i < Count; i++)
            {
                UnkHumanFxData[i] = new(stream, isBigEndian);
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteString16(ActorName, isBigEndian);

            if ((Unk01 >> 16) != 2)
            {
                return;
            }

            stream.Write(UnkHumanFxData.Length, isBigEndian);

            foreach (var val in UnkHumanFxData)
            {
                val.Write(stream, isBigEndian);
            }
        }
    }
}
