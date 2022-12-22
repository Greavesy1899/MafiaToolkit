using BitStreams;
using System.ComponentModel;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.Vehicle
{
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitDeformMaterial
    {
        public C_GUID Guid { get; set; }
        public uint Group { get; set; }

        public S_InitDeformMaterial()
        {
            Guid = new C_GUID();
        }

        public void Load(BitStream MemStream)
        {
            Guid = new C_GUID();
            Guid.Load(MemStream);

            Group = MemStream.ReadUInt32();
        }

        public void Save(BitStream MemStream)
        {
            Guid.Save(MemStream);
            MemStream.WriteUInt32(Group);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitColorAndDirty
    {
        public C_GUID Guid { get; set; }
        public ulong TextureName { get; set; }
        public uint Flags { get; set; }

        public S_InitColorAndDirty()
        {
            Guid = new C_GUID();
        }

        public void Load(BitStream MemStream)
        {
            Guid = new C_GUID();
            Guid.Load(MemStream);

            TextureName = MemStream.ReadUInt64();
            Flags = MemStream.ReadUInt32();
        }

        public void Save(BitStream MemStream)
        {
            Guid.Save(MemStream);
            MemStream.WriteUInt64(TextureName);
            MemStream.WriteUInt32(Flags);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitLight
    {
        public ulong FrameName { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public float EmissivePower { get; set; }
        public float EmissiveMiddle { get; set; }
        public float EmissiveSpeed_0 { get; set; }
        public float EmissiveSpeed_1 { get; set; }
        public ulong[] CheckBoneName { get; set; }
        public ulong LightModelHash { get; set; }
        public uint ParticleBreakID { get; set; }
        public uint Unk12 { get; set; }

        public S_InitLight()
        {
            EmissivePower = 2.0f;
            EmissiveMiddle = 0.6f;
            EmissiveSpeed_0 = 8.0f;
            EmissiveSpeed_1 = 3.0f;
            CheckBoneName = new ulong[0];
        }

        public void Load(BitStream MemStream)
        {
            FrameName = MemStream.ReadUInt64();
            Unk1 = MemStream.ReadInt32();
            Unk2 = MemStream.ReadInt32();
            Unk3 = MemStream.ReadUInt32();
            Unk4 = MemStream.ReadUInt32();
            EmissivePower = MemStream.ReadSingle();
            EmissiveMiddle = MemStream.ReadSingle();
            EmissiveSpeed_0 = MemStream.ReadSingle();
            EmissiveSpeed_1 = MemStream.ReadSingle();

            // Read array
            CheckBoneName = PrefabUtils.ReadHashArray(MemStream);

            LightModelHash = MemStream.ReadUInt64();
            ParticleBreakID = MemStream.ReadUInt32();
            Unk12 = MemStream.ReadUInt32();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(FrameName);
            MemStream.WriteInt32(Unk1);
            MemStream.WriteInt32(Unk2);
            MemStream.WriteUInt32(Unk3);
            MemStream.WriteUInt32(Unk4);
            MemStream.WriteSingle(EmissivePower);
            MemStream.WriteSingle(EmissiveMiddle);
            MemStream.WriteSingle(EmissiveSpeed_0);
            MemStream.WriteSingle(EmissiveSpeed_1);

            PrefabUtils.WriteHashArray(MemStream, CheckBoneName);

            MemStream.WriteUInt64(LightModelHash);
            MemStream.WriteUInt32(ParticleBreakID);
            MemStream.WriteUInt32(Unk12);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitSZDefaultRangeOnMatrGroup
    {
        public uint Group { get; set; }
        public float RangeMin { get; set; }
        public float RangeMax { get; set; }

        public void Load(BitStream MemStream)
        {
            Group = MemStream.ReadUInt32();
            RangeMin = MemStream.ReadSingle();
            RangeMax = MemStream.ReadSingle();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt32(Group);
            MemStream.WriteSingle(RangeMin);
            MemStream.WriteSingle(RangeMax);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_ShaderEffectInit
    {
        public ulong[] FGSCloneVisuals { get; set; }
        public S_InitDeformMaterial[] DeformMaterial { get; set; }
        public S_InitColorAndDirty[] ColorAndDirty { get; set; }
        public S_InitLight[] LightInit { get; set; }
        public S_InitSZDefaultRangeOnMatrGroup[] SZDefaultRangeOnMatrGroup { get; set; }
        public S_InitSkinZoneRange[] SkinZoneRanges { get; set; }
        public S_InitSkinZonePartData[] SkinZonePartData { get; set; }
        public S_InitSkinZoneGroup[] SkinZoneGroups { get; set; }
        public C_GUID SPZAndLightGUID { get; set; }
        public float BoneStiffness { get; set; }

        public S_ShaderEffectInit()
        {
            DeformMaterial = new S_InitDeformMaterial[0];
            ColorAndDirty = new S_InitColorAndDirty[0];
            LightInit = new S_InitLight[0];
            SZDefaultRangeOnMatrGroup = new S_InitSZDefaultRangeOnMatrGroup[0];
            SkinZoneRanges = new S_InitSkinZoneRange[0];
            SkinZonePartData = new S_InitSkinZonePartData[0];
            SkinZoneGroups = new S_InitSkinZoneGroup[0];
            SPZAndLightGUID = new C_GUID();
        }
        public void Load(BitStream MemStream)
        {
            FGSCloneVisuals = PrefabUtils.ReadHashArray(MemStream);

            // Read Deforms
            uint NumDeforms = MemStream.ReadUInt32();
            DeformMaterial = new S_InitDeformMaterial[NumDeforms];
            for (uint i = 0; i < DeformMaterial.Length; i++)
            {
                S_InitDeformMaterial NewDeform = new S_InitDeformMaterial();
                NewDeform.Load(MemStream);

                DeformMaterial[i] = NewDeform;
            }

            // Read Colour & Dirty
            uint NumColourAndDirty = MemStream.ReadUInt32();
            ColorAndDirty = new S_InitColorAndDirty[NumColourAndDirty];
            for (uint i = 0; i < ColorAndDirty.Length; i++)
            {
                S_InitColorAndDirty NewColourAndDirty = new S_InitColorAndDirty();
                NewColourAndDirty.Load(MemStream);

                ColorAndDirty[i] = NewColourAndDirty;
            }

            // Read Unknown
            uint NumUnknown = MemStream.ReadUInt32();
            LightInit = new S_InitLight[NumUnknown];
            for (uint i = 0; i < NumUnknown; i++)
            {
                S_InitLight NewUnknown = new S_InitLight();
                NewUnknown.Load(MemStream);

                LightInit[i] = NewUnknown;
            }

            // Read SZDefaulRangeOnMatrGroup
            uint NumSZDefaulRangeOnMatrGroup = MemStream.ReadUInt32();
            SZDefaultRangeOnMatrGroup = new S_InitSZDefaultRangeOnMatrGroup[NumSZDefaulRangeOnMatrGroup];
            for (uint i = 0; i < SZDefaultRangeOnMatrGroup.Length; i++)
            {
                S_InitSZDefaultRangeOnMatrGroup DefaultRangeOnMatrGroup = new S_InitSZDefaultRangeOnMatrGroup();
                DefaultRangeOnMatrGroup.Load(MemStream);

                SZDefaultRangeOnMatrGroup[i] = DefaultRangeOnMatrGroup;
            }

            // Read S_InitSkinZoneRange
            uint NumSZSkinZoneRange = MemStream.ReadUInt32();
            SkinZoneRanges = new S_InitSkinZoneRange[NumSZSkinZoneRange];
            for (uint i = 0; i < SkinZoneRanges.Length; i++)
            {
                S_InitSkinZoneRange SZRange = new S_InitSkinZoneRange();
                SZRange.Load(MemStream);

                SkinZoneRanges[i] = SZRange;
            }

            // Read SkinZonePartData
            uint NumSkinZonePartData = MemStream.ReadUInt32();
            SkinZonePartData = new S_InitSkinZonePartData[NumSkinZonePartData];
            for (uint i = 0; i < SkinZonePartData.Length; i++)
            {
                S_InitSkinZonePartData SZPartData = new S_InitSkinZonePartData();
                SZPartData.Load(MemStream);
                SkinZonePartData[i] = SZPartData;
            }

            // Read S_InitSkinZoneGroup
            uint NumSkinZoneGroups = MemStream.ReadUInt32();
            SkinZoneGroups = new S_InitSkinZoneGroup[NumSkinZoneGroups];
            for (uint i = 0; i < SkinZoneGroups.Length; i++)
            {
                S_InitSkinZoneGroup SZGroup = new S_InitSkinZoneGroup();
                SZGroup.Load(MemStream);
                SkinZoneGroups[i] = SZGroup;
            }

            SPZAndLightGUID = new C_GUID();
            SPZAndLightGUID.Load(MemStream);

            BoneStiffness = MemStream.ReadSingle();
        }

        public void Save(BitStream MemStream)
        {
            PrefabUtils.WriteHashArray(MemStream, FGSCloneVisuals);

            // Write Deform Materials
            MemStream.WriteUInt32((uint)DeformMaterial.Length);
            foreach(S_InitDeformMaterial DeformMat in DeformMaterial)
            {
                DeformMat.Save(MemStream);
            }

            // Write Colour & Dirty
            MemStream.WriteUInt32((uint)ColorAndDirty.Length);
            foreach (S_InitColorAndDirty InitColDirt in ColorAndDirty)
            {
                InitColDirt.Save(MemStream);
            }

            // Write Lights
            MemStream.WriteUInt32((uint)LightInit.Length);
            foreach (S_InitLight UnknownType in LightInit)
            {
                UnknownType.Save(MemStream);
            }

            // Write SZDefaulRangeOnMatrGroup
            MemStream.WriteUInt32((uint)SZDefaultRangeOnMatrGroup.Length);
            foreach (S_InitSZDefaultRangeOnMatrGroup MatrGroup in SZDefaultRangeOnMatrGroup)
            {
                MatrGroup.Save(MemStream);
            }

            // Write S_InitSkinZoneRange
            MemStream.WriteUInt32((uint)SkinZoneRanges.Length);
            foreach (S_InitSkinZoneRange SZRange in SkinZoneRanges)
            {
                SZRange.Save(MemStream);
            }

            // Write SkinZonePartData
            MemStream.WriteUInt32((uint)SkinZonePartData.Length);
            foreach (S_InitSkinZonePartData SZPartData in SkinZonePartData)
            {
                SZPartData.Save(MemStream);
            }

            // Write S_InitSkinZoneGroup
            MemStream.WriteUInt32((uint)SkinZoneGroups.Length);
            foreach (S_InitSkinZoneGroup SZGroup in SkinZoneGroups)
            {
                SZGroup.Save(MemStream);
            }

            SPZAndLightGUID.Save(MemStream);
            MemStream.WriteSingle(BoneStiffness);
        }
    }
}
