using System;
using System.Collections.Generic;
using System.IO;

namespace ResourceTypes.Materials
{
    public class MaterialParameter
    {
        string id;
        float[] paramaters;

        public string ID {
            get { return id; }
            set { id = value; }
        }

        private string _name { get => MaterialParameterNames.GetName(ID); }

        public float[] Paramaters {
            get { return paramaters; }
            set { paramaters = value; }
        }

        public MaterialParameter()
        {
            id = "";
            paramaters = new float[0];
        }

        public MaterialParameter(MaterialParameter OtherParameter)
        {
            ID = OtherParameter.ID;
            Paramaters = OtherParameter.paramaters;
        }

        public MaterialParameter(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            id = new string(reader.ReadChars(4));
            int paramCount = reader.ReadInt32() / 4;
            paramaters = new float[paramCount];
            for (int i = 0; i != paramCount; i++)
            {
                paramaters[i] = reader.ReadSingle();
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(id.ToCharArray());
            writer.Write(paramaters.Length * 4);
            for (int i = 0; i != paramaters.Length; i++)
            {
                writer.Write(paramaters[i]);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", id, _name, paramaters.Length);
        }
    }

    public static class MaterialParameterNames
    {
        private static Dictionary<string, string> names = new()
        {
            { "B000", "UseReflTexCoords" },
            { "B111", "Zero" },
            { "C000", "AmbientColor" },
            { "C001", "AmbientColor2" },
            { "C002", "MaterialColor" },
            { "C003", "EmissiveColorAndIntensity" },
            { "C004", "EmissiveColorAndIntensity1" },
            { "C005", "EmissiveFacadeColorAndIntensity" },
            { "C008", "SpecifiedColor" },
            { "C009", "MaterialColor1" },
            { "C015", "MaterialColor2" },
            { "C020", "MaterialColorAndIntensity" },
            { "C025", "VisualColorModulator" },
            { "C030", "SpecularColor" },
            { "C031", "SpecularColor1" },
            { "C033", "CarEmissiveColorsAndCharOffsets" },
            { "C036", "CharacterColors" },
            { "C040", "AlphaRefVal" },
            { "C060", "ConstEnvColor" },
            { "C070", "DynamicEnvMapParams" },
            { "C123", "DebugVal" },
            { "C130", "GlobalSceneParams" },
            { "D000", "Lights" },
            { "D001", "CameraPosition" },
            { "D002", "CameraNearAndFarAndNearRelAndFarInv" },
            { "D004", "CamWorldMatRight" },
            { "D005", "CamWorldMatUp" },
            { "D006", "CamWorldDir" },
            { "D007", "PosDecompressionScaleAndOffset" },
            { "D009", "CounterLight" },
            { "D010", "PositionScale" },
            { "D011", "GlowScaleAndOffset" },
            { "D013", "SpecularPowerAndLevel" },
            { "D014", "DualSpecularPowerAndLevel" },
            { "D015", "SpecularPowerMods" },
            { "D016", "EnvMapBias" },
            { "D017", "EnvMapAmount" },
            { "D018", "EnvMapAmountAndSpecLevelRef" },
            { "D019", "SrcTexture0SizeInv" },
            { "D020", "SrcTexture1SizeInv" },
            { "D021", "SecondTextureAmount" },
            { "D022", "BlendingMaxAndLength" },
            { "D024", "HeightScaleAndOffsetAndDims" },
            { "D025", "Bumpiness" },
            { "D026", "WaterInvDepthAndInvSoftBorder" },
            { "D027", "FresnelBiasAndPower" },
            { "D028", "WaterFoamAmpAndSpeedXY" },
            { "D030", "FogEndAndDistInv" },
            { "D033", "GlowOffsetToCenterAndDistFade" },
            { "D034", "GlowDirToCenter" },
            { "D040", "DirAtten" },
            { "D041", "ConstAtten" },
            { "D045", "SrcTexture2SizeInv" },
            { "D046", "SrcTexture3SizeInv" },
            { "D047", "SrcTexture4SizeInv" },
            { "D050", "Pos2DScaleAndOffset" },
            { "D055", "SpecularShift" },
            { "D057", "Texcoord0Offset" },
            { "D060", "AlphaDissolveCoefs" },
            { "D065", "BlendAmount" },
            { "D067", "DistToMaxOpacityInv" },
            { "D068", "DistMaxMinOpacity" },
            { "D075", "DistToMaxSSAOInv" },
            { "D076", "HalfDistToMaxSSAOInv" },
            { "D082", "SkinZonesStatePacked" },
            { "D085", "GlassDestructionSwitches" },
            { "D087", "GlassDestructionSpiderData0" },
            { "D088", "GlassDestructionSpiderData1" },
            { "D089", "GlassDestructionMappingTrans" },
            { "D090", "Wave0FreqAmpPhase" },
            { "D091", "Wave0Dir" },
            { "D092", "Wave1FreqAmpPhase" },
            { "D093", "Wave1Dir" },
            { "D094", "WaveFreqAmpPhaseMin" },
            { "D095", "Near" },
            { "D096", "BranchWave0FreqAmpPhase" },
            { "D097", "BranchWave1FreqAmpPhase" },
            { "D098", "TreeSizeScaleRangeMin" },
            { "D099", "BranchWave2FreqAmpPhase" },
            { "D100", "ShallowColor" },
            { "D101", "RipplesScale" },
            { "D102", "RipplesVelocity" },
            { "D103", "Wave2FreqAmpPhase" },
            { "D104", "Wave2Dir" },
            { "D105", "Camera2WorldPosition" },
            { "D106", "PlayerPos" },
            { "D107", "PlayerSizeForce" },
            { "D108", "ScaleNearFNRangeCellsizeNoiserange" },
            { "D109", "Wind" },
            { "D110", "CloudMovementSpeed" },
            { "D112", "ScaleAndAlphaFactor" },
            { "D114", "SunDirInvW" },
            { "D115", "CloudScatteringModulators" },
            { "D120", "AnimTexBlendRatio" },
            { "D123", "TextureBombing" },
            { "D127", "GridSize" },
            { "D130", "TextureBombingComputed" },
            { "D140", "BackdropClouds" },
            { "D141", "BackdropCloudsComputed" },
            { "D142", "SkyboxBlendRatio" },
            { "D150", "ChannelMask" },
            { "D200", "InstanceData" },
            { "D250", "BBRotAnim" },
            { "D251", "BBRotAnimDepthOffsetAndScale" },
            { "D270", "WetRoadEnvMapAmountAndBlurriness" },
            { "D271", "WetRoadLightCutAndIntensity" },
            { "D272", "WetRoadNormalScale" },
            { "D273", "WorldNormalMix" },
            { "D280", "FacadeNumTilesAndDistDividersInv" },
            { "D282", "DissolveTexNumTiles" },
            { "D283", "LODTransitionState" },
            { "D285", "TexCoordsScale" },
            { "D290", "EmptyTireProps" },
            { "D300", "CSMShadows" },
            { "D350", "ForcedWorldNormalZ" },
            { "D360", "Custom2DDepth" },
            { "D400", "RainDropParameters" },
            { "D401", "RainVolumeParameters" },
            { "D402", "RainUp" },
            { "D403", "RainColorParameters" },
            { "D404", "RainSize" },
            { "D405", "VisualColorModulator" },
            { "M000", "WorldMat" },
            { "M001", "ViewProjMat" },
            { "M002", "TexTransformMat" },
            { "M003", "WorldViewProjMat" },
            { "M004", "BoneMats" },
            { "M005", "ViewProjMatInv" },
            { "M006", "WorldMatInv" },
            { "M008", "UbermeshMats" },
            { "M020", "GlassDestructionMappingRotScale" },
            { "M030", "ReflectionViewProjMat" },
            { "M040", "RainMaskViewProjMat" },
            { "M060", "EnvCustomRotMat" },
            { "N003", "Noise" },
            { "N004", "Noise" },
            { "N005", "RenderTargetSizesInv" },
            { "N006", "RenderTargetSizesInvRefl" },
            { "S000", "DiffuseTexture" },
            { "S001", "NormalTexture" },
            { "S002", "SpecularLevelTexture" },
            { "S004", "ReflectionTexture" },
            { "S006", "RefractionTexture" },
            { "S010", "NoiseVolumeTexture" },
            { "S011", "EmissiveTexture" },
            { "S012", "AmbientOcclusionTexture" },
            { "S015", "DiffuseTexture1" },
            { "S016", "NormalTexture1" },
            { "S017", "SpecularLevelTexture1" },
            { "S018", "DiffuseTexture2" },
            { "S019", "NormalTexture2" },
            { "S020", "SpecularLevelTexture2" },
            { "S021", "LookUpTexture" },
            { "S022", "SpecularPowerTexture" },
            { "S023", "HeightTexture" },
            { "S025", "BlendingLevelTexture" },
            { "S040", "DepthTexture" },
            { "S050", "PostProcessSrcTexture" },
            { "S051", "PostProcessSrcTexture1" },
            { "S052", "PostProcessSrcTexture2" },
            { "S053", "PostProcessSrcTexture3" },
            { "S054", "PostProcessSrcTexture4" },
            { "S060", "EnvCubeTexture" },
            { "S065", "DynamicEnvCubeTexture" },
            { "S066", "DynamicEnvCubeModTexture" },
            { "S067", "SecDynamicEnvCubeTexture" },
            { "S070", "GlassDestructionInfoTexture" },
            { "S071", "GlassDestructionSpiderTexture" },
            { "S072", "GlassDestructionVisibilityTex" },
            { "S080", "AnimNextFrameTexture" },
            { "S081", "RandomNumberTexture" },
            { "S082", "TangentSpaceTexture" },
            { "S090", "ReplaceMaterialTexture" },
            { "S100", "RainMaskTexture" },
            { "S110", "DynamicAOTexture" },
            { "T000", "Timer" },
            { "V000", "Scales" },
            { "V001", "Offsets" },
            { "S500", "SpotMap01" },
            { "S510", "SpotMap02" },
            { "S520", "SpotMap03" },
            { "S600", "ShadowMap01" },
            { "S610", "ShadowMap02" },
            { "S620", "ShadowMap03" },
            { "S700", "CSMShadowMap01" },
            { "S710", "CSMShadowMap02" },
            { "S720", "CSMShadowMap03" },
        };

        public static string GetName(string ID)
        {
            if (names.ContainsKey(ID))
            {
                return names[ID];
            }

            return ID;
        }
    }
}
