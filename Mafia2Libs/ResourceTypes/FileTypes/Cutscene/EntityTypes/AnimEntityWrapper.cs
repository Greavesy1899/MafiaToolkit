using System.ComponentModel;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AnimEntityWrapper
    {
        public ushort NumEntities { get; set; }
        public string EntityName0 { get; set; }
        public string EntityName1 { get; set; }
        public byte Unk0 { get; set; }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeBaseData AnimEntityData { get; set; }

        public AnimEntityWrapper()
        {
            EntityName0 = "";
            EntityName1 = "";
            AnimEntityData = new AeBaseData();
        }

        public virtual void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            NumEntities = stream.ReadUInt16(isBigEndian);

            if (NumEntities == 0)
            {
                // Nothing here. return.
                return;
            }

            EntityName0 = stream.ReadString16(isBigEndian);
            EntityName1 = stream.ReadString16(isBigEndian);

            if (!string.IsNullOrEmpty(EntityName1))
            {
                Unk0 = stream.ReadByte8();
            }
        }

        public virtual void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            stream.Write(NumEntities, isBigEndian);

            if (NumEntities == 0)
            {
                return;
            }

            stream.WriteString16(EntityName0, isBigEndian);
            stream.WriteString16(EntityName1, isBigEndian);

            if (!string.IsNullOrEmpty(EntityName1))
            {
                stream.WriteByte(Unk0);
            }
        }

        public virtual AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AnimEntity;
        }
    }
}
