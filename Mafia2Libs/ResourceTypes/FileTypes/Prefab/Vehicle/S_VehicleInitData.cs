using BitStreams;
using ResourceTypes.Prefab.CrashObject;

namespace ResourceTypes.Prefab.Vehicle
{
    public class S_VehicleInitData : S_DeformationInitData
    {
        public S_OtherInitData OtherInitData { get; set; }
        public S_AxleWheelInit WheelAxle { get; set; }
        public S_ShaderEffectInit ShaderEffects { get; set; }

        public S_VehicleInitData()
        {
            OtherInitData = new S_OtherInitData();
            WheelAxle = new S_AxleWheelInit();
            ShaderEffects = new S_ShaderEffectInit();
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            OtherInitData = new S_OtherInitData();
            OtherInitData.Load(MemStream);

            WheelAxle = new S_AxleWheelInit();
            WheelAxle.Load(MemStream);

            ShaderEffects = new S_ShaderEffectInit();
            ShaderEffects.Load(MemStream);
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            OtherInitData.Save(MemStream);
            WheelAxle.Save(MemStream);
            ShaderEffects.Save(MemStream);
        }
    }
}
