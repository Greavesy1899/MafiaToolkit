using BitStreams;
using System.ComponentModel;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.Vehicle
{
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitSkinZoneRange
    {
        public uint Group { get; set; }
        public uint SkinZone { get; set; }
        public float RangeMax { get; set; }
        public float RangeMin { get; set; }

        public void Load(BitStream MemStream)
        {
            Group = MemStream.ReadUInt32();
            SkinZone = MemStream.ReadUInt32();
            RangeMax = MemStream.ReadSingle();
            RangeMin = MemStream.ReadSingle();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt32(Group);
            MemStream.WriteUInt32(SkinZone);
            MemStream.WriteSingle(RangeMax);
            MemStream.WriteSingle(RangeMin);
        }
    }
}
