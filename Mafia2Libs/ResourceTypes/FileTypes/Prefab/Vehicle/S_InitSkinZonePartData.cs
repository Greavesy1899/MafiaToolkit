using BitStreams;
using System.ComponentModel;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.Vehicle
{
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitSkinZoneSettings
    {
        [PropertyForceAsAttribute]
        public ushort SkinZoneIndex { get; set; }
        [PropertyForceAsAttribute]
        public ushort MaterialGroupIndex { get; set; }
        [PropertyForceAsAttribute]
        public float Intensity { get; set; }

        public void Load(BitStream MemStream)
        {
            SkinZoneIndex = MemStream.ReadUInt16();
            MaterialGroupIndex = MemStream.ReadUInt16();
            Intensity = MemStream.ReadSingle();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt16(SkinZoneIndex);
            MemStream.WriteUInt16(MaterialGroupIndex);
            MemStream.WriteSingle(Intensity);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitSkinZoneFrameData
    {
        [PropertyForceAsAttribute]
        public ulong FrameName { get; set; }
        public S_InitSkinZoneSettings[] SZSettings { get; set; }

        public void Load(BitStream MemStream)
        {
            FrameName = MemStream.ReadUInt64();

            uint NumSkinZoneSettings = MemStream.ReadUInt32();
            SZSettings = new S_InitSkinZoneSettings[NumSkinZoneSettings];
            for (uint i = 0; i < SZSettings.Length; i++)
            {
                S_InitSkinZoneSettings SZSetting = new S_InitSkinZoneSettings();
                SZSetting.Load(MemStream);
                SZSettings[i] = SZSetting;
            }
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(FrameName);

            MemStream.WriteUInt32((uint)SZSettings.Length);
            foreach(S_InitSkinZoneSettings Setting in SZSettings)
            {
                Setting.Save(MemStream);
            }
        }

    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitSkinZonePartData
    {
        [PropertyForceAsAttribute]
        public ulong MainPartFrameName { get; set; }
        public S_InitSkinZoneFrameData[] SkinZoneFrameData { get; set; }

        public S_InitSkinZonePartData() { }
        public void Load(BitStream MemStream)
        {
            MainPartFrameName = MemStream.ReadUInt64();

            uint NumInitSkinZoneFrameData = MemStream.ReadUInt32();
            SkinZoneFrameData = new S_InitSkinZoneFrameData[NumInitSkinZoneFrameData];         
            for(uint i = 0; i < SkinZoneFrameData.Length; i++)
            {
                S_InitSkinZoneFrameData NewSZFrameData = new S_InitSkinZoneFrameData();
                NewSZFrameData.Load(MemStream);
                SkinZoneFrameData[i] = NewSZFrameData;
            }
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(MainPartFrameName);

            MemStream.WriteUInt32((uint)SkinZoneFrameData.Length);
            foreach (S_InitSkinZoneFrameData SZFrameData in SkinZoneFrameData)
            {
                SZFrameData.Save(MemStream);
            }
        }
    }
}
