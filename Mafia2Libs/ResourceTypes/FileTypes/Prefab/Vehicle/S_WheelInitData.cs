using BitStreams;
using ResourceTypes.Prefab.CrashObject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.Vehicle
{
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_WheelShaderEffectInit
    {
        public C_GUID GuidTyreDeformMaterial { get; set; }
        public ulong[] FGSCloneVisuals { get; set; }
        public S_InitColorAndDirty[] ColourAndDirty { get; set; }

        public S_WheelShaderEffectInit()
        {
            GuidTyreDeformMaterial = new C_GUID();
            FGSCloneVisuals = new ulong[0];
            ColourAndDirty = new S_InitColorAndDirty[0];
        }

        public void Load(BitStream MemStream)
        {
            GuidTyreDeformMaterial.Load(MemStream);

            uint NumFGSCloneVisuals = MemStream.ReadUInt32();
            FGSCloneVisuals = new ulong[NumFGSCloneVisuals];
            for (uint i = 0; i < NumFGSCloneVisuals; i++)
            {
                FGSCloneVisuals[i] = MemStream.ReadUInt64();
            }

            uint NumColourAndDirty = MemStream.ReadUInt32();
            ColourAndDirty = new S_InitColorAndDirty[NumColourAndDirty];
            for (uint i = 0; i < NumColourAndDirty; i++)
            {
                S_InitColorAndDirty ColorAndDiryEntry = new S_InitColorAndDirty();
                ColorAndDiryEntry.Load(MemStream);
                ColourAndDirty[i] = ColorAndDiryEntry;
            }
        }

        public void Save(BitStream MemStream)
        {
            GuidTyreDeformMaterial.Save(MemStream);

            MemStream.WriteUInt32((uint)FGSCloneVisuals.Length);
            foreach (ulong CloneVisualHash in FGSCloneVisuals)
            {
                MemStream.WriteUInt64(CloneVisualHash);
            }

            MemStream.WriteUInt32((uint)ColourAndDirty.Length);
            foreach (S_InitColorAndDirty ColourAndDirtyEntry in ColourAndDirty)
            {
                ColourAndDirtyEntry.Save(MemStream);
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class S_WheelInitData : S_AddDeformPartsInitData
    {
        public ulong WheelFrameName { get; set; }
        public ulong TyreFrameName { get; set; }
        public float WheelMass { get; set; }
        public float RimMass { get; set; }
        public float RimRadius { get; set; }
        public float SpeedMin { get; set; }
        public float SpeedMax { get; set; }
        public S_WheelShaderEffectInit SWheelShaderEffectInit { get; set; }

        public S_WheelInitData() : base()
        {
            SWheelShaderEffectInit = new S_WheelShaderEffectInit();
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            WheelFrameName = MemStream.ReadUInt64();
            TyreFrameName = MemStream.ReadUInt64();
            WheelMass = MemStream.ReadSingle();
            RimMass = MemStream.ReadSingle();
            RimRadius = MemStream.ReadSingle();
            SpeedMin = MemStream.ReadSingle();
            SpeedMax = MemStream.ReadSingle();

            // Load Shader Effect data
            SWheelShaderEffectInit = new S_WheelShaderEffectInit();
            SWheelShaderEffectInit.Load(MemStream);
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            MemStream.WriteUInt64(WheelFrameName);
            MemStream.WriteUInt64(TyreFrameName);
            MemStream.WriteSingle(WheelMass);
            MemStream.WriteSingle(RimMass);
            MemStream.WriteSingle(RimRadius);
            MemStream.WriteSingle(SpeedMin);
            MemStream.WriteSingle(SpeedMax);

            // Save shader effect data
            SWheelShaderEffectInit.Save(MemStream);
        }
    }
}
