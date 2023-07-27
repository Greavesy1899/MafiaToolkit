using System.IO;
using Utils.Extensions;
using Utils.Helpers;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Actors
{
    /*
     * Used with EntityDataStorage, NOT the actor (.act) data file.
     */
    public class ActorCar : IActorExtraDataInterface
    {
        public class GearTableData
        {
            public float GearRatio { get; set; }
            public float RotationsGearUp { get; set; }
            public float RotationsGearDown { get; set; }
            public float SlowStyleGearUpCoeff { get; set; }
            public float MinClutch { get; set; }
            public float MinClutchAngleCoeff { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5}", GearRatio, RotationsGearUp, RotationsGearDown, SlowStyleGearUpCoeff, MinClutch, MinClutchAngleCoeff);
            }
        }
        public class DifferentialTableData
        {
            public float ViscousClutch { get; set; }
            public float ViscousClutchRotLim { get; set; }
            public float DiffInertia { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", ViscousClutch, ViscousClutchRotLim, DiffInertia);
            }
        }
        public class WheelTableData
        {
            public string WheelModel { get; set; }
            public string Tyre { get; set; }
            public float Scale { get; set; }
            public float TyreStiffness { get; set; }
            public float Pressure { get; set; }
            public float SpringK { get; set; }
            public float SpringLength { get; set; }
            public float SpringPreLoad { get; set; }
            public float DamperBound { get; set; }
            public float DamperRebound { get; set; }
            public int DifferentialIndex { get; set; }
            public float AntiRollBarTorque { get; set; }
            public float AxleAngleCorrectCoeff { get; set; }
            public float ToeIn { get; set; }
            public float KPInclination { get; set; }
            public float KPCaster { get; set; }
            public float Camber { get; set; }
            public float RollDamper { get; set; }
            public float SteeringCoeff { get; set; }
            public float BrakeCoeffLeft { get; set; }
            public float BrakeCoeffRight { get; set; }
            public byte Steering { get; set; }
            public byte HandBrake { get; set; }

            public WheelTableData()
            {
                WheelModel = "";
                Tyre = "";
            }
        }
        public class SmokeMotorTableData
        {
            public int SmokeMotorID { get; set; }
            public float SmokeMotorDamage { get; set; }
            public float SmokeMotorUnk01 { get; set; }
        }
        public class EngineTableData
        {
            public int EngineSndCategory { get; set; }
            public int EngineSndId { get; set; }
            public float EngineVolume { get; set; }
            public float EngineVolumeNoPower { get; set; }
            public float EngineFrqLo { get; set; }
            public float EngineFrqHi { get; set; }
            public float EngineRotLo { get; set; }
            public float EngineRotLoVol { get; set; }
            public float EngineRotHiVol { get; set; }
            public float EngineRotHi { get; set; }
            public bool EngineNoClutchLockOnly { get; set; }
        }
        public class EngineFizzTableData
        {
            public int EngineFizzSndID { get; set; }
            public float EngineFizzVolume { get; set; }

        }
        public class KnockingBonnetTableData
        {
            public int KnockingBonnetSndId { get; set; }
            public float KnockingBonnetSpeed { get; set; }
            public float KnockingBonnetVolume { get; set; }
        }
        public class CrashTableData
        {
            public int CrashSndCategory { get; set; }
            public int CrashSndId { get; set; }
            public float CrashVolume { get; set; }
            public float CrashSpeed { get; set; }
        }

        public int[] UnkInts0 { get; set; }
        #region Physics/RigidBody
        [LocalisedCategory("$PHYSICS_RB")]
        public float Mass { get; set; }
        [LocalisedCategory("$PHYSICS_RB")]
        public EDSVector3 CenterOfMass { get; set; }
        [LocalisedCategory("$PHYSICS_RB")]
        public float InteriaMax { get; set; }
        [LocalisedCategory("$PHYSICS_RB")]
        public EDSVector3 Inertia { get; set; }
        [LocalisedCategory("$PHYSICS_RB")]
        public int MaterialID { get; set; }
        [LocalisedCategory("$PHYSICS_RB")]
        public float StaticFriction { get; set; }
        [LocalisedCategory("$PHYSICS_RB")]
        public float DynamicFriction { get; set; }
        [LocalisedCategory("$PHYSICS_RB")]
        public float Restitution { get; set; }
        #endregion // Physics/RigidBody
        #region Motor Torque
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float Power { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float RotationsMin { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float RotationsMax { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float RotationsMaxMax { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float TorqueStart { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float TorqueMax { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float TorqueEnd { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float RotationsTorqueLow { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float RotationsTorqueHigh { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float MotorBrakeTorque { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float MotorInertia { get; set; }
        [LocalisedCategory("$MOTOR_TORQUE")]
        public float MotorOrientation { get; set; }
        #endregion // Motor Torque
        #region Fuel
        [LocalisedCategory("$FUEL")]
        public float FuelConsumption { get; set; }
        [LocalisedCategory("$FUEL")]
        public float FuelTankCapacity { get; set; }
        #endregion // Fuel
        #region Gearbox
        [LocalisedCategory("$GEARBOX")]
        public int FinalGearCount { get; set; }
        [LocalisedCategory("$GEARBOX")]
        public float FinalGearRatio0 { get; set; }
        [LocalisedCategory("$GEARBOX")]
        public float FinalGearRatio1 { get; set; }
        [LocalisedCategory("$GEARBOX")]
        public int GearCount { get; set; }
        [LocalisedCategory("$GEARBOX")]
        public int GearReverseCount { get; set; }
        [LocalisedCategory("$GEARBOX")]
        public GearTableData[] GearData { get; set; }
        [LocalisedCategory("$GEARBOX")]
        public float MinClutchGlobal { get; set; }
        [LocalisedCategory("$GEARBOX")]
        public float MinClutchAngleCoeffGlobal { get; set; }
        #endregion // Gearbox
        #region Steering
        [LocalisedCategory("$STEERING")]
        public float MaxAccelSlowMode { get; set; }
        [LocalisedCategory("$STEERING")]
        public float SteerAngleDiffMax { get; set; }
        [LocalisedCategory("$STEERING")]
        public float SteerAngleMaxLow { get; set; }
        [LocalisedCategory("$STEERING")]
        public float SteerAngleMaxHigh { get; set; }
        [LocalisedCategory("$STEERING")]
        public float AngleChangeBackLow { get; set; }
        [LocalisedCategory("$STEERING")]
        public float AngleChangeBackHigh { get; set; }
        [LocalisedCategory("$STEERING")]
        public float AngleChangeLow { get; set; }
        [LocalisedCategory("$STEERING")]
        public float AngleChangeHigh { get; set; }
        [LocalisedCategory("$STEERING")]
        public float AngleChangeCoeff { get; set; }
        #endregion // Steering
        #region Differential
        [LocalisedCategory("$DIFFERENTIAL")]
        public int MotorDifferentialIndex { get; set; }
        [LocalisedCategory("$DIFFERENTIAL")]
        public DifferentialTableData[] DifferentialData { get; set; }
        [LocalisedCategory("$DIFFERENTIAL")]
        public byte[] UnknownDifferential { get; set; }
        [LocalisedCategory("$DIFFERENTIAL")]
        public float CDRatio { get; set; }
        [LocalisedCategory("$DIFFERENTIAL")]
        public float CDViscousClutch { get; set; }
        [LocalisedCategory("$DIFFERENTIAL")]
        public float CDDiffLock { get; set; }
        [LocalisedCategory("$DIFFERENTIAL")]
        public int IndexAxleDifferentialFront { get; set; }
        [LocalisedCategory("$DIFFERENTIAL")]
        public int IndexAxleDifferentialBack { get; set; }
        [LocalisedCategory("$DIFFERENTIAL")]
        public float TyreLateralStiffnessCoeff { get; set; }
        [LocalisedCategory("$DIFFERENTIAL")]
        public float TypeLateralDamperCoeff { get; set; }
        #endregion // Differential
        #region Wheel Variables 0
        [LocalisedCategory("$WHEELS")]
        public WheelTableData[] WheelData { get; set; }
        #endregion // Wheel Variables 0
        #region Brakes
        [LocalisedCategory("$BRAKES")]
        public float BrakeTorque { get; set; }
        [LocalisedCategory("$BRAKES")]
        public float BrakeReaction { get; set; }
        [LocalisedCategory("$BRAKES")]
        public float HandBrakeTorque { get; set; }
        [LocalisedCategory("$BRAKES")]
        public float BrakeReactionLow { get; set; }
        [LocalisedCategory("$BRAKES")]
        public float BrakeReactionHigh { get; set; }
        #endregion // Brakes
        #region Aerodynamic Variables
        [LocalisedCategory("$AERODYNAMICS")]
        public float AerodynamicSurfaceSize { get; set; }
        [LocalisedCategory("$AERODYNAMICS")]
        public float AerodynamicResistCoeff { get; set; }
        [LocalisedCategory("$AERODYNAMICS")]
        public float FrontSpoilerSurfaceSize { get; set; }
        [LocalisedCategory("$AERODYNAMICS")]
        public float FrontSpoilerResistCoeff { get; set; }
        [LocalisedCategory("$AERODYNAMICS")]
        public float BackSpoilerSurfaceSize { get; set; }
        [LocalisedCategory("$AERODYNAMICS")]
        public float BackSpoilerResistCoeff { get; set; }
        #endregion // Aerodynamic Variables
        public bool ESM { get; set; }
        public bool ASP { get; set; }
        public float ArcadeMinCoeff { get; set; }
        public int AMFakeESP { get; set; }
        public float MinSpeed { get; set; }
        public float MaxSpeedAdd { get; set; }
        public float ESPCoeffMinus { get; set; }
        public float ESPCoeffPlus { get; set; }
        public bool AMFakeESPUseASR { get; set; }
        public bool AMFakeURVM { get; set; }
        public float SpeedMaxEffectivity { get; set; }
        public float RotVelSpeedLimit { get; set; }
        public float Coeff { get; set; }
        public bool AMFakeURVMUseASR { get; set; }
        public bool RightWheelForcePos { get; set; }
        public float FFRideMagnitudeCoeff { get; set; }
        #region EffectID Variables 1
        [LocalisedCategory("$EFFECTS")]
        public SmokeMotorTableData[] SmokeMotorData { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short SmokeExhaustID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short ExplosionID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short FireID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short SlideEffectID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short CrashEffectID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short RimSparksID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short BurnOutID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short BreakTireID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short BreakGlassSideWindowID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short BreakGlassFrontWindowID { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public short[] HedgeIDs { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public int RainID { get; set; }
        #endregion // EffectID Variables 1
        public float TimeFireMax { get; set; }
        #region Engine Variables 1
        [LocalisedCategory("$ENGINE")]
        public int EngineSwitchSndCategory10 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineSwitchSndId10 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineSwitchVolume10 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineSwitchStart10 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineSwitchSndCategory11 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineSwitchSndId11 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineSwitchVolume11 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineSwitchSndCategory12 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineSwitchSndId12 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineSwitchVolume12 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineMinRotSndCategory1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineMinRotSndID1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineMinRotVolume1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineMinRotRotVolHi1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineMinRotRotHi1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineNPCSndCategory1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineNPCSndID1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineNPCVolume1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineNPCFrqLo1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineNPCFrqHi1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineNPCRotLoVol1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineNPCRotHiVol1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineNPCLoVolume1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineVolEnvelopeRotLo1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineVolEnvelopeRotLoVol1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineVolEnvelopeRotHi1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineVolEnvelopeRotHiVol1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineCrossFadeTimePlus1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineCrossFadeTimeMinus1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public EngineTableData[] EngineFData { get; set; }
        [LocalisedCategory("$ENGINE")]
        public EngineTableData[] EngineBData { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineFizzSndCategory { get; set; }
        [LocalisedCategory("$ENGINE")]
        public EngineFizzTableData[] EngineFizzData { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineCoolingSndCategory1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineCoolingSndId1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineCoolingVolume1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineCoolingTemperatureVolume1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float EngineCoolingTemperatureSilencion1 { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineFanSndCategory { get; set; }
        [LocalisedCategory("$ENGINE")]
        public int EngineFanSndId { get; set; }
        [LocalisedCategory("$ENGINE")]
        public float m_EngineFanVolume { get; set; }
        #endregion // Engine Variables 1
        #region Environment Variables
        [LocalisedCategory("$ENVIRONMENT")]
        public int EnvironmentalSndCategory1 { get; set; }
        [LocalisedCategory("$ENVIRONMENT")]
        public int EnvironmentalSndId1 { get; set; }
        [LocalisedCategory("$ENVIRONMENT")]
        public float EnvironmentalVolume1 { get; set; }
        [LocalisedCategory("$ENVIRONMENT")]
        public float EnvironmentalFrqLo1 { get; set; }
        [LocalisedCategory("$ENVIRONMENT")]
        public float EnvironmentalFrqHi1 { get; set; }
        [LocalisedCategory("$ENVIRONMENT")]
        public float EnvironmentalSpeedMaxVolume1 { get; set; }
        #endregion // Environment Variables
        #region AirPump Variables
        [LocalisedCategory("$AIR_PUMP")]
        public int AirPumpSndCategory1 { get; set; }
        [LocalisedCategory("$AIR_PUMP")]
        public int AirPumpSndId1 { get; set; }
        [LocalisedCategory("$AIR_PUMP")]
        public float AirPumpVolume1 { get; set; }
        #endregion // AirPump Variables
        #region Door Variables
        [LocalisedCategory("$DOORS")]
        public int DoorOpenSndCategory1 { get; set; }
        [LocalisedCategory("$DOORS")]
        public int DoorOpenSndId1 { get; set; }
        [LocalisedCategory("$DOORS")]
        public float DoorOpenVolume1 { get; set; }
        [LocalisedCategory("$DOORS")]
        public float DoorOpenSndId1b { get; set; }
        [LocalisedCategory("$DOORS")]
        public float DoorOpenVolume1b { get; set; }
        [LocalisedCategory("$DOORS")]
        public int DoorCloseSndCategory1 { get; set; }
        [LocalisedCategory("$DOORS")]
        public int DoorCloseSndId1 { get; set; }
        [LocalisedCategory("$DOORS")]
        public float DoorCloseVolume1 { get; set; }
        [LocalisedCategory("$DOORS")]
        public float DoorCloseSndId1b { get; set; }
        [LocalisedCategory("$DOORS")]
        public float DoorCloseVolume1b { get; set; }
        #endregion // Door Variables
        #region Bonnet/Trunk Variables
        [LocalisedCategory("$BONNET_TRUNK")]
        public int CoverOpenSndCategory1 { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public int CoverOpenSndId1 { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public float CoverOpenVolume1 { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public int CoverCloseSndCategory1 { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public int CoverCloseSndId1 { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public float CoverCloseVolume1 { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public int KnockingBonnetSndCategory1 { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public KnockingBonnetTableData[] KnockingBonnetData { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public int DropHoodSndCategory1 { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public int DropHoodSndId1 { get; set; }
        [LocalisedCategory("$BONNET_TRUNK")]
        public float DropHoodVolume1 { get; set; }
        #endregion // Bonnet/Trunk Variables
        #region Environment Variables 2
        [LocalisedCategory("$ENVIRONMENT")]
        public CrashTableData[] CrashData { get; set; }
        #endregion // Environment Variables 2
        #region Gearbox Variables 2
        [LocalisedCategory("$GEARBOX")]
        public int GearboxShiftSndCategory1 { get; set; }
        [LocalisedCategory("$GEARBOX")]
        public int GearboxShiftSndId1 { get; set; }
        [LocalisedCategory("$GEARBOX")]
        public float GearboxShiftVolume1 { get; set; }
        #endregion // Gearbox Variables 2
        #region Brakes 1
        [LocalisedCategory("$BRAKES")]
        public int HandbrakeSndCategory1 { get; set; }
        [LocalisedCategory("$BRAKES")]
        public int HandbrakeSndId1 { get; set; }
        [LocalisedCategory("$BRAKES")]
        public float HandbrakeVolume1 { get; set; }
        #endregion // Brakes 1
        #region Wheel Variables 1
        [LocalisedCategory("$WHEELS")]
        public int WheelShankSndCategory1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public int WheelShankSndId11 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public int WheelShankSndId21 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public int WheelShankSndId31 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float WheelShankHeight11 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float WheelShankHeight21 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float WheelShankSpeed11 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float WheelShankSpeed21 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float WheelShankSpeed31 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float WheelShankVolume11 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float WheelShankVolume21 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float WheelShankVolume31 { get; set; }
        #endregion // Wheel Variables 1
        #region Horn/Siren Variables
        [LocalisedCategory("$HORN_SIREN")]
        public int HornSndCategory1 { get; set; }
        [LocalisedCategory("$HORN_SIREN")]
        public int HornSndId1 { get; set; }
        [LocalisedCategory("$HORN_SIREN")]
        public float HornVolume1 { get; set; }
        [LocalisedCategory("$HORN_SIREN")]
        public int SirenSndCategory1 { get; set; }
        [LocalisedCategory("$HORN_SIREN")]
        public int SirenSndId1 { get; set; }
        [LocalisedCategory("$HORN_SIREN")]
        public float SirenVolume1 { get; set; }
        #endregion // Horn/Siren Variables
        #region Wheel Variables 2 (Tyres)
        [LocalisedCategory("$WHEELS")]
        public int TyreBreakSndCategory1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public int TyreBreakSndId1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float TyreBreakVolume1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public int TyreCrashSndCategory1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public int TyreCrashSndId1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float TyreCrashVolume1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float TyreCrashFrqLo1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float TyreCrashFrqHi1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float TyreCrashSpeedLoVol1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float TyreCrashSpeedHiVol1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public int RimRideSndCategory1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public int RimRideSndId1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float RimRideVolume1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float RimRideFrqLo1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float RimRideFrqHi1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float RimRideSpeedLoVol1 { get; set; }
        [LocalisedCategory("$WHEELS")]
        public float RimRideSpeedHiVol1 { get; set; }
        #endregion // Wheel Variables 2 (Tyres)
        #region GlassBreak Variables
        [LocalisedCategory("$GLASS_BREAK")]
        public int GlassBreakSndCategory { get; set; }
        [LocalisedCategory("$GLASS_BREAK")]
        public int GlassBreakSndId { get; set; }
        [LocalisedCategory("$GLASS_BREAK")]
        public float GlassBreakSndVolume { get; set; }
        #endregion GlassBreak Variables
        #region Explosion Variables
        [LocalisedCategory("$EXPLOSION")]
        public int ExplosionSndCategory { get; set; }
        [LocalisedCategory("$EXPLOSION")]
        public int ExplosionSndId { get; set; }
        [LocalisedCategory("$EXPLOSION")]
        public float ExplosionSndVolume { get; set; }
        #endregion Explosion Variables
        #region Fire Variables
        [LocalisedCategory("$FIRE")]
        public int FireSndCategory { get; set; }
        [LocalisedCategory("$FIRE")]
        public int FireSndId { get; set; }
        [LocalisedCategory("$FIRE")]
        public float FireSndVolume { get; set; }
        #endregion // Fire Variables
        #region Brakes 2
        [LocalisedCategory("$BRAKES")]
        public int SqueakingBrakesSndCategory { get; set; }
        [LocalisedCategory("$BRAKES")]
        public int SqueakingBrakesSndId { get; set; }
        [LocalisedCategory("$BRAKES")]
        public float SqueakingBrakesVolume { get; set; }
        [LocalisedCategory("$BRAKES")]
        public float SqueakingBrakesSpeedHi { get; set; }
        #endregion // Brakes 2
        #region Rain Contact
        [LocalisedCategory("$RAIN_CONTACT")]
        public int RainSndCategory { get; set; }
        [LocalisedCategory("$RAIN_CONTACT")]
        public int RainSndId { get; set; }
        [LocalisedCategory("$RAIN_CONTACT")]
        public float RainVolumeLo { get; set; }
        [LocalisedCategory("$RAIN_CONTACT")]
        public float RainVolumeHi { get; set; }
        [LocalisedCategory("$RAIN_CONTACT")]
        public float RainFrqLo { get; set; }
        [LocalisedCategory("$RAIN_CONTACT")]
        public float RainFrqHi { get; set; }
        [LocalisedCategory("$RAIN_CONTACT")]
        public float RainDensityLo { get; set; }
        [LocalisedCategory("$RAIN_CONTACT")]
        public float RainDensityHi { get; set; }
        #endregion // Rain Contact
        #region Hedge Contact
        [LocalisedCategory("$HEDGE_CONTACT")]
        public int HedgeSndCategory { get; set; }
        [LocalisedCategory("$HEDGE_CONTACT")]
        public int HedgeSndId { get; set; }
        [LocalisedCategory("$HEDGE_CONTACT")]
        public float HedgeFrqLo { get; set; }
        [LocalisedCategory("$HEDGE_CONTACT")]
        public float HedgeFrqHi { get; set; }
        [LocalisedCategory("$HEDGE_CONTACT")]
        public float HedgeSpeedMidVolume { get; set; }
        [LocalisedCategory("$HEDGE_CONTACT")]
        public float HedgeVolumeMid { get; set; }
        [LocalisedCategory("$HEDGE_CONTACT")]
        public float HedgeSpeedMaxVolume { get; set; }
        [LocalisedCategory("$HEDGE_CONTACT")]
        public float HedgeVolumeMax { get; set; }
        #endregion // Hedge Contact
        public byte[] UnkBytes { get; set; }
        #region Slide/Skidding Variables
        [LocalisedCategory("$SKIDDING_SLIDING")]
        public float SlideId { get; set; }
        [LocalisedCategory("$SKIDDING_SLIDING")]
        public float SlideSpeedMax { get; set; }
        [LocalisedCategory("$SKIDDING_SLIDING")]
        public float SlideSpeedVolMax { get; set; }
        [LocalisedCategory("$SKIDDING_SLIDING")]
        public int SlideMinVol { get; set; }
        [LocalisedCategory("$SKIDDING_SLIDING")]
        public float SlideMaxVol { get; set; }
        [LocalisedCategory("$SKIDDING_SLIDING")]
        public float SlideFreqLow { get; set; }
        [LocalisedCategory("$SKIDDING_SLIDING")]
        public int SlideFreqHi { get; set; }
        #endregion Slide/Skidding Variables
        #region Roll Variables
        [LocalisedCategory("$ROLL")]
        public float RollId { get; set; }
        [LocalisedCategory("$ROLL")]
        public float RollSpeedMax { get; set; }
        [LocalisedCategory("$ROLL")]
        public float RollSpeedVolMax { get; set; }
        [LocalisedCategory("$ROLL")]
        public int RollMinVol { get; set; }
        [LocalisedCategory("$ROLL")]
        public float RollMaxVol { get; set; }
        [LocalisedCategory("$ROLL")]
        public float RollFreqLow { get; set; }
        [LocalisedCategory("$ROLL")]
        public int RollFreqHi { get; set; }
        #endregion Roll Variables
        public int SndChngVersionId { get; set; }
        public int SndBreakId { get; set; }
        public int SndDelayId { get; set; }
        public int SndDelayDelay { get; set; }
        #region EffectID Variables 2
        [LocalisedCategory("$EFFECTS")]
        public int ParticleHitId { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public int ParticleBreakId { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public int ParticleChngVersionId { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public int ParticleSlideId { get; set; }
        [LocalisedCategory("$EFFECTS")]
        public int ParticleHitSpeedMin { get; set; }
        #endregion // EffectID Variables 2
        [LocalisedCategory("$EXPLOSION")]
        public int ExplodeID { get; set; }

        public ActorCar()
        {
            UnkInts0 = new int[8];
            CenterOfMass = new EDSVector3();
            Inertia = new EDSVector3();
            GearData = new GearTableData[7];
            DifferentialData = new DifferentialTableData[10];
            WheelData = new WheelTableData[10];
            SmokeMotorData = new SmokeMotorTableData[3];
            EngineFData = new EngineTableData[8];
            EngineBData = new EngineTableData[8];
            EngineFizzData = new EngineFizzTableData[3];
            KnockingBonnetData = new KnockingBonnetTableData[2];
            CrashData = new CrashTableData[4];
        }

        public ActorCar(ActorCar OtherCar)
        {
            ReflectionHelpers.Copy(OtherCar, this);
        }

        public int GetSize()
        {
            return 3400;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            UnkInts0 = new int[8];
            for (int z = 0; z < 8; z++)
            {
                UnkInts0[z] = stream.ReadInt32(isBigEndian);
            }

            Mass = stream.ReadSingle(isBigEndian);
            CenterOfMass.ReadFromFile(stream, isBigEndian);
            InteriaMax = stream.ReadSingle(isBigEndian);
            Inertia.ReadFromFile(stream, isBigEndian);
            MaterialID = stream.ReadInt32(isBigEndian);
            StaticFriction = stream.ReadSingle(isBigEndian);
            DynamicFriction = stream.ReadSingle(isBigEndian);
            Restitution = stream.ReadSingle(isBigEndian);
            Power = stream.ReadSingle(isBigEndian);
            RotationsMin = stream.ReadSingle(isBigEndian);
            RotationsMax = stream.ReadSingle(isBigEndian);
            RotationsMaxMax = stream.ReadSingle(isBigEndian);
            TorqueStart = stream.ReadSingle(isBigEndian);
            TorqueMax = stream.ReadSingle(isBigEndian);
            TorqueEnd = stream.ReadSingle(isBigEndian);
            RotationsTorqueLow = stream.ReadSingle(isBigEndian);
            RotationsTorqueHigh = stream.ReadSingle(isBigEndian);
            MotorBrakeTorque = stream.ReadSingle(isBigEndian);
            MotorInertia = stream.ReadSingle(isBigEndian);
            MotorOrientation = stream.ReadSingle(isBigEndian);
            FuelConsumption = stream.ReadSingle(isBigEndian);
            FuelTankCapacity = stream.ReadSingle(isBigEndian);
            FinalGearCount = stream.ReadInt32(isBigEndian);
            FinalGearRatio0 = stream.ReadSingle(isBigEndian);
            FinalGearRatio1 = stream.ReadSingle(isBigEndian);
            GearCount = stream.ReadInt32(isBigEndian);
            GearReverseCount = stream.ReadInt32(isBigEndian);

            #region GearData parsing
            GearData = new GearTableData[7];

            for (int z = 0; z < 7; z++)
            {
                GearTableData gearData = new GearTableData();
                gearData.GearRatio = stream.ReadSingle(isBigEndian);
                GearData[z] = gearData;
            }

            for (int z = 0; z < 7; z++)
            {
                GearData[z].RotationsGearUp = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 7; z++)
            {
                GearData[z].RotationsGearDown = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 7; z++)
            {
                GearData[z].SlowStyleGearUpCoeff = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 7; z++)
            {
                GearData[z].MinClutch = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 7; z++)
            {
                GearData[z].MinClutchAngleCoeff = stream.ReadSingle(isBigEndian);
            }
            #endregion

            MinClutchGlobal = stream.ReadSingle(isBigEndian);
            MinClutchAngleCoeffGlobal = stream.ReadSingle(isBigEndian);
            MaxAccelSlowMode = stream.ReadSingle(isBigEndian);
            SteerAngleDiffMax = stream.ReadSingle(isBigEndian);
            SteerAngleMaxLow = stream.ReadSingle(isBigEndian);
            SteerAngleMaxHigh = stream.ReadSingle(isBigEndian);
            AngleChangeBackLow = stream.ReadSingle(isBigEndian);
            AngleChangeBackHigh = stream.ReadSingle(isBigEndian);
            AngleChangeLow = stream.ReadSingle(isBigEndian);
            AngleChangeHigh = stream.ReadSingle(isBigEndian);
            AngleChangeCoeff = stream.ReadSingle(isBigEndian);
            MotorDifferentialIndex = stream.ReadInt32(isBigEndian);

            #region DiffTableData parsing
            DifferentialData = new DifferentialTableData[10];

            for (int z = 0; z < 10; z++)
            {
                var diffData = new DifferentialTableData();
                diffData.ViscousClutch = stream.ReadSingle(isBigEndian);
                DifferentialData[z] = diffData;
            }

            for (int z = 0; z < 10; z++)
            {
                DifferentialData[z].ViscousClutchRotLim = stream.ReadSingle(isBigEndian);
            }

            UnknownDifferential = stream.ReadBytes(12);

            for (int z = 0; z < 10; z++)
            {
                DifferentialData[z].DiffInertia = stream.ReadSingle(isBigEndian);
            }
            #endregion

            CDRatio = stream.ReadSingle(isBigEndian);
            CDViscousClutch = stream.ReadSingle(isBigEndian);
            CDDiffLock = stream.ReadSingle(isBigEndian);
            IndexAxleDifferentialFront = stream.ReadInt32(isBigEndian);
            IndexAxleDifferentialBack = stream.ReadInt32(isBigEndian);
            TyreLateralStiffnessCoeff = stream.ReadSingle(isBigEndian);
            TypeLateralDamperCoeff = stream.ReadSingle(isBigEndian);

            #region Wheel Data parsing
            WheelData = new WheelTableData[10];
            for (int z = 0; z < 10; z++)
            {
                var wheelData = new WheelTableData();
                wheelData.WheelModel = stream.ReadStringBuffer(32, true);
                WheelData[z] = wheelData;
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].Tyre = stream.ReadStringBuffer(8, true);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].Scale = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].TyreStiffness = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].Pressure = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].SpringK = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].SpringLength = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].SpringPreLoad = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].DamperBound = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].DamperRebound = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].DifferentialIndex = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].AntiRollBarTorque = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].AxleAngleCorrectCoeff = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].ToeIn = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].KPInclination = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].KPCaster = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].Camber = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].RollDamper = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].SteeringCoeff = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].BrakeCoeffLeft = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].BrakeCoeffRight = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].Steering = stream.ReadByte8();
            }

            for (int z = 0; z < 10; z++)
            {
                WheelData[z].HandBrake = stream.ReadByte8();
            }
            #endregion

            BrakeTorque = stream.ReadSingle(isBigEndian);
            BrakeReaction = stream.ReadSingle(isBigEndian);
            HandBrakeTorque = stream.ReadSingle(isBigEndian);
            BrakeReactionLow = stream.ReadSingle(isBigEndian);
            BrakeReactionHigh = stream.ReadSingle(isBigEndian);
            AerodynamicSurfaceSize = stream.ReadSingle(isBigEndian);
            AerodynamicResistCoeff = stream.ReadSingle(isBigEndian);
            FrontSpoilerSurfaceSize = stream.ReadSingle(isBigEndian);
            FrontSpoilerResistCoeff = stream.ReadSingle(isBigEndian);
            BackSpoilerSurfaceSize = stream.ReadSingle(isBigEndian);
            BackSpoilerResistCoeff = stream.ReadSingle(isBigEndian);
            ESM = stream.ReadBoolean();
            ASP = stream.ReadBoolean();

            stream.ReadInt16(isBigEndian);
            //offset in IDA = 1760. sizeof(header) == 68. 1760 + 68.
            //stream.Seek(1760, SeekOrigin.Begin);

            ArcadeMinCoeff = stream.ReadSingle(isBigEndian);
            AMFakeESP = stream.ReadInt32(isBigEndian);
            MinSpeed = stream.ReadSingle(isBigEndian);
            MaxSpeedAdd = stream.ReadSingle(isBigEndian);
            ESPCoeffMinus = stream.ReadSingle(isBigEndian);
            ESPCoeffPlus = stream.ReadSingle(isBigEndian);
            AMFakeESPUseASR = stream.ReadBoolean();
            AMFakeURVM = stream.ReadBoolean();

            stream.ReadInt16(isBigEndian);
            //offset in IDA = 1788. sizeof(header) == 68. 1788 + 68.
            //stream.Seek(1788, SeekOrigin.Begin);

            SpeedMaxEffectivity = stream.ReadSingle(isBigEndian);
            RotVelSpeedLimit = stream.ReadSingle(isBigEndian);
            Coeff = stream.ReadSingle(isBigEndian);
            AMFakeURVMUseASR = stream.ReadBoolean();
            RightWheelForcePos = stream.ReadBoolean();

            stream.ReadInt16(isBigEndian);
            //offset in IDA = 1804. sizeof(header) == 68. 1804 + 68.
            //stream.Seek(1804, SeekOrigin.Begin);

            FFRideMagnitudeCoeff = stream.ReadSingle(isBigEndian);
            SmokeMotorData = new SmokeMotorTableData[3];

            for (int z = 0; z < 3; z++)
            {
                SmokeMotorData[z] = new SmokeMotorTableData();
                SmokeMotorData[z].SmokeMotorID = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < 3; z++)
            {
                SmokeMotorData[z].SmokeMotorDamage = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 3; z++)
            {
                SmokeMotorData[z].SmokeMotorUnk01 = stream.ReadSingle(isBigEndian);
            }

            SmokeExhaustID = stream.ReadInt16(isBigEndian);
            ExplosionID = stream.ReadInt16(isBigEndian);
            FireID = stream.ReadInt16(isBigEndian);
            SlideEffectID = stream.ReadInt16(isBigEndian);
            CrashEffectID = stream.ReadInt16(isBigEndian);
            RimSparksID = stream.ReadInt16(isBigEndian);
            BurnOutID = stream.ReadInt16(isBigEndian);
            BreakTireID = stream.ReadInt16(isBigEndian);
            BreakGlassSideWindowID = stream.ReadInt16(isBigEndian);
            BreakGlassFrontWindowID = stream.ReadInt16(isBigEndian);
            HedgeIDs = new short[2];
            HedgeIDs[0] = stream.ReadInt16(isBigEndian);
            HedgeIDs[1] = stream.ReadInt16(isBigEndian);
            RainID = stream.ReadInt32(isBigEndian);
            TimeFireMax = stream.ReadSingle(isBigEndian);
            EngineSwitchSndCategory10 = stream.ReadInt32(isBigEndian);
            EngineSwitchSndCategory11 = stream.ReadInt32(isBigEndian);
            EngineSwitchSndCategory12 = stream.ReadInt32(isBigEndian);
            EngineSwitchSndId10 = stream.ReadInt32(isBigEndian);
            EngineSwitchSndId11 = stream.ReadInt32(isBigEndian);
            EngineSwitchSndId12 = stream.ReadInt32(isBigEndian);
            EngineSwitchVolume10 = stream.ReadSingle(isBigEndian);
            EngineSwitchVolume11 = stream.ReadSingle(isBigEndian);
            EngineSwitchVolume12 = stream.ReadSingle(isBigEndian);
            EngineSwitchStart10 = stream.ReadSingle(isBigEndian);
            EngineMinRotSndCategory1 = stream.ReadInt32(isBigEndian);
            EngineMinRotSndID1 = stream.ReadInt32(isBigEndian);
            EngineMinRotVolume1 = stream.ReadSingle(isBigEndian);
            EngineMinRotRotVolHi1 = stream.ReadSingle(isBigEndian);
            EngineMinRotRotHi1 = stream.ReadSingle(isBigEndian);
            EngineNPCSndCategory1 = stream.ReadInt32(isBigEndian);
            EngineNPCSndID1 = stream.ReadInt32(isBigEndian);
            EngineNPCVolume1 = stream.ReadSingle(isBigEndian);
            EngineNPCFrqLo1 = stream.ReadSingle(isBigEndian);
            EngineNPCFrqHi1 = stream.ReadSingle(isBigEndian);
            EngineNPCRotLoVol1 = stream.ReadSingle(isBigEndian);
            EngineNPCRotHiVol1 = stream.ReadSingle(isBigEndian);
            EngineNPCLoVolume1 = stream.ReadSingle(isBigEndian);
            EngineVolEnvelopeRotLo1 = stream.ReadSingle(isBigEndian);
            EngineVolEnvelopeRotLoVol1 = stream.ReadSingle(isBigEndian);
            EngineVolEnvelopeRotHi1 = stream.ReadSingle(isBigEndian);
            EngineVolEnvelopeRotHiVol1 = stream.ReadSingle(isBigEndian);
            EngineCrossFadeTimePlus1 = stream.ReadSingle(isBigEndian);
            EngineCrossFadeTimeMinus1 = stream.ReadSingle(isBigEndian);
            EngineFData = new EngineTableData[8];
            EngineBData = new EngineTableData[8];

            #region EngineF Parsing:
            for (int z = 0; z < 8; z++)
            {
                EngineFData[z] = new EngineTableData();
                EngineFData[z].EngineSndCategory = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineSndId = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineVolume = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineVolumeNoPower = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineFrqLo = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineFrqHi = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineRotLo = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineRotLoVol = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineRotHiVol = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineRotHi = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineFData[z].EngineNoClutchLockOnly = stream.ReadBoolean();
            }
            #endregion

            #region EngineB Parsing:
            for (int z = 0; z < 8; z++)
            {
                EngineBData[z] = new EngineTableData();
                EngineBData[z].EngineSndCategory = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineSndId = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineVolume = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineVolumeNoPower = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineFrqLo = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineFrqHi = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineRotLo = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineRotLoVol = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineRotHiVol = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineRotHi = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                EngineBData[z].EngineNoClutchLockOnly = stream.ReadBoolean();
            }
            #endregion

            EngineFizzSndCategory = stream.ReadInt32(isBigEndian);
            EngineFizzData = new EngineFizzTableData[3];

            #region EngineFizzData
            for (int z = 0; z < EngineFizzData.Length; z++)
            {
                EngineFizzData[z] = new EngineFizzTableData();
                EngineFizzData[z].EngineFizzSndID = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < EngineFizzData.Length; z++)
            {
                EngineFizzData[z].EngineFizzVolume = stream.ReadSingle(isBigEndian);
            }
            #endregion

            EngineCoolingSndCategory1 = stream.ReadInt32(isBigEndian);
            EngineCoolingSndId1 = stream.ReadInt32(isBigEndian);
            EngineCoolingVolume1 = stream.ReadSingle(isBigEndian);
            EngineCoolingTemperatureVolume1 = stream.ReadSingle(isBigEndian);
            EngineCoolingTemperatureSilencion1 = stream.ReadSingle(isBigEndian);
            EngineFanSndCategory = stream.ReadInt32(isBigEndian);
            EngineFanSndId = stream.ReadInt32(isBigEndian);
            m_EngineFanVolume = stream.ReadSingle(isBigEndian);
            EnvironmentalSndCategory1 = stream.ReadInt32(isBigEndian);
            EnvironmentalSndId1 = stream.ReadInt32(isBigEndian);
            EnvironmentalVolume1 = stream.ReadSingle(isBigEndian);
            EnvironmentalFrqLo1 = stream.ReadSingle(isBigEndian);
            EnvironmentalFrqHi1 = stream.ReadSingle(isBigEndian);
            EnvironmentalSpeedMaxVolume1 = stream.ReadSingle(isBigEndian);
            AirPumpSndCategory1 = stream.ReadInt32(isBigEndian);
            AirPumpSndId1 = stream.ReadInt32(isBigEndian);
            AirPumpVolume1 = stream.ReadSingle(isBigEndian);
            DoorOpenSndCategory1 = stream.ReadInt32(isBigEndian);
            DoorOpenSndId1 = stream.ReadInt32(isBigEndian);
            DoorOpenVolume1 = stream.ReadSingle(isBigEndian);
            DoorOpenSndId1b = stream.ReadSingle(isBigEndian);
            DoorOpenVolume1b = stream.ReadSingle(isBigEndian);
            DoorCloseSndCategory1 = stream.ReadInt32(isBigEndian);
            DoorCloseSndId1 = stream.ReadInt32(isBigEndian);
            DoorCloseVolume1 = stream.ReadSingle(isBigEndian);
            DoorCloseSndId1b = stream.ReadSingle(isBigEndian);
            DoorCloseVolume1b = stream.ReadSingle(isBigEndian);
            CoverOpenSndCategory1 = stream.ReadInt32(isBigEndian);
            CoverOpenSndId1 = stream.ReadInt32(isBigEndian);
            CoverOpenVolume1 = stream.ReadSingle(isBigEndian);
            CoverCloseSndCategory1 = stream.ReadInt32(isBigEndian);
            CoverCloseSndId1 = stream.ReadInt32(isBigEndian);
            CoverCloseVolume1 = stream.ReadSingle(isBigEndian);
            KnockingBonnetSndCategory1 = stream.ReadInt32(isBigEndian);

            #region KnockingBonnetData
            KnockingBonnetData = new KnockingBonnetTableData[2];
            for (int z = 0; z < KnockingBonnetData.Length; z++)
            {
                KnockingBonnetData[z] = new KnockingBonnetTableData();
                KnockingBonnetData[z].KnockingBonnetSndId = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < KnockingBonnetData.Length; z++)
            {
                KnockingBonnetData[z].KnockingBonnetSpeed = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < KnockingBonnetData.Length; z++)
            {
                KnockingBonnetData[z].KnockingBonnetVolume = stream.ReadSingle(isBigEndian);
            }
            #endregion

            DropHoodSndCategory1 = stream.ReadInt32(isBigEndian);
            DropHoodSndId1 = stream.ReadInt32(isBigEndian);
            DropHoodVolume1 = stream.ReadSingle(isBigEndian);

            #region CrashTable Data
            CrashData = new CrashTableData[4];
            for (int z = 0; z < CrashData.Length; z++)
            {
                CrashData[z] = new CrashTableData();
                CrashData[z].CrashSndCategory = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < CrashData.Length; z++)
            {
                CrashData[z].CrashSndId = stream.ReadInt32(isBigEndian);
            }

            for (int z = 0; z < CrashData.Length; z++)
            {
                CrashData[z].CrashVolume = stream.ReadSingle(isBigEndian);
            }

            for (int z = 0; z < CrashData.Length; z++)
            {
                CrashData[z].CrashSpeed = stream.ReadSingle(isBigEndian);
            }
            #endregion

            GearboxShiftSndCategory1 = stream.ReadInt32(isBigEndian);
            GearboxShiftSndId1 = stream.ReadInt32(isBigEndian);
            GearboxShiftVolume1 = stream.ReadSingle(isBigEndian);
            HandbrakeSndCategory1 = stream.ReadInt32(isBigEndian);
            HandbrakeSndId1 = stream.ReadInt32(isBigEndian);
            HandbrakeVolume1 = stream.ReadSingle(isBigEndian);
            WheelShankSndCategory1 = stream.ReadInt32(isBigEndian);
            WheelShankSndId11 = stream.ReadInt32(isBigEndian);
            WheelShankSndId21 = stream.ReadInt32(isBigEndian);
            WheelShankSndId31 = stream.ReadInt32(isBigEndian);
            WheelShankHeight11 = stream.ReadSingle(isBigEndian);
            WheelShankHeight21 = stream.ReadSingle(isBigEndian);
            WheelShankSpeed11 = stream.ReadSingle(isBigEndian);
            WheelShankSpeed21 = stream.ReadSingle(isBigEndian);
            WheelShankSpeed31 = stream.ReadSingle(isBigEndian);
            WheelShankVolume11 = stream.ReadSingle(isBigEndian);
            WheelShankVolume21 = stream.ReadSingle(isBigEndian);
            WheelShankVolume31 = stream.ReadSingle(isBigEndian);
            HornSndCategory1 = stream.ReadInt32(isBigEndian);
            HornSndId1 = stream.ReadInt32(isBigEndian);
            HornVolume1 = stream.ReadSingle(isBigEndian);
            SirenSndCategory1 = stream.ReadInt32(isBigEndian);
            SirenSndId1 = stream.ReadInt32(isBigEndian);
            SirenVolume1 = stream.ReadSingle(isBigEndian);
            TyreBreakSndCategory1 = stream.ReadInt32(isBigEndian);
            TyreBreakSndId1 = stream.ReadInt32(isBigEndian);
            TyreBreakVolume1 = stream.ReadSingle(isBigEndian);
            TyreCrashSndCategory1 = stream.ReadInt32(isBigEndian);
            TyreCrashSndId1 = stream.ReadInt32(isBigEndian);
            TyreCrashVolume1 = stream.ReadSingle(isBigEndian);
            TyreCrashFrqLo1 = stream.ReadSingle(isBigEndian);
            TyreCrashFrqHi1 = stream.ReadSingle(isBigEndian);
            TyreCrashSpeedLoVol1 = stream.ReadSingle(isBigEndian);
            TyreCrashSpeedHiVol1 = stream.ReadSingle(isBigEndian);
            RimRideSndCategory1 = stream.ReadInt32(isBigEndian);
            RimRideSndId1 = stream.ReadInt32(isBigEndian);
            RimRideVolume1 = stream.ReadSingle(isBigEndian);
            RimRideFrqLo1 = stream.ReadSingle(isBigEndian);
            RimRideFrqHi1 = stream.ReadSingle(isBigEndian);
            RimRideSpeedLoVol1 = stream.ReadSingle(isBigEndian);
            RimRideSpeedHiVol1 = stream.ReadSingle(isBigEndian);
            GlassBreakSndCategory = stream.ReadInt32(isBigEndian);
            GlassBreakSndId = stream.ReadInt32(isBigEndian);
            GlassBreakSndVolume = stream.ReadSingle(isBigEndian);
            ExplosionSndCategory = stream.ReadInt32(isBigEndian);
            ExplosionSndId = stream.ReadInt32(isBigEndian);
            ExplosionSndVolume = stream.ReadSingle(isBigEndian);
            FireSndCategory = stream.ReadInt32(isBigEndian);
            FireSndId = stream.ReadInt32(isBigEndian);
            FireSndVolume = stream.ReadSingle(isBigEndian);
            SqueakingBrakesSndCategory = stream.ReadInt32(isBigEndian);
            SqueakingBrakesSndId = stream.ReadInt32(isBigEndian);
            SqueakingBrakesVolume = stream.ReadSingle(isBigEndian);
            SqueakingBrakesSpeedHi = stream.ReadSingle(isBigEndian);
            RainSndCategory = stream.ReadInt32(isBigEndian);
            RainSndId = stream.ReadInt32(isBigEndian);
            RainVolumeLo = stream.ReadSingle(isBigEndian);
            RainVolumeHi = stream.ReadSingle(isBigEndian);
            RainFrqLo = stream.ReadSingle(isBigEndian);
            RainFrqHi = stream.ReadSingle(isBigEndian);
            RainDensityLo = stream.ReadSingle(isBigEndian);
            RainDensityHi = stream.ReadSingle(isBigEndian);
            HedgeSndCategory = stream.ReadInt32(isBigEndian);
            HedgeSndId = stream.ReadInt32(isBigEndian);
            HedgeFrqLo = stream.ReadSingle(isBigEndian);
            HedgeFrqHi = stream.ReadSingle(isBigEndian);
            HedgeSpeedMidVolume = stream.ReadSingle(isBigEndian);
            HedgeVolumeMid = stream.ReadSingle(isBigEndian);
            HedgeSpeedMaxVolume = stream.ReadSingle(isBigEndian);
            HedgeVolumeMax = stream.ReadSingle(isBigEndian);
            UnkBytes = stream.ReadBytes(112);
            SlideId = stream.ReadSingle(isBigEndian);
            SlideSpeedMax = stream.ReadSingle(isBigEndian);
            SlideSpeedVolMax = stream.ReadSingle(isBigEndian);
            SlideMinVol = stream.ReadInt32(isBigEndian);
            SlideMaxVol = stream.ReadSingle(isBigEndian);
            SlideFreqLow = stream.ReadSingle(isBigEndian);
            SlideFreqHi = stream.ReadInt32(isBigEndian);
            RollId = stream.ReadSingle(isBigEndian);
            RollSpeedMax = stream.ReadSingle(isBigEndian);
            RollSpeedVolMax = stream.ReadSingle(isBigEndian);
            RollMinVol = stream.ReadInt32(isBigEndian);
            RollMaxVol = stream.ReadSingle(isBigEndian);
            RollFreqLow = stream.ReadSingle(isBigEndian);
            RollFreqHi = stream.ReadInt32(isBigEndian);
            SndChngVersionId = stream.ReadInt32(isBigEndian);
            SndBreakId = stream.ReadInt32(isBigEndian);
            SndDelayId = stream.ReadInt32(isBigEndian);
            SndDelayDelay = stream.ReadInt32(isBigEndian);
            ParticleHitId = stream.ReadInt32(isBigEndian);
            ParticleBreakId = stream.ReadInt32(isBigEndian);
            ParticleChngVersionId = stream.ReadInt32(isBigEndian);
            ParticleSlideId = stream.ReadInt32(isBigEndian);
            ParticleHitSpeedMin = stream.ReadInt32(isBigEndian);
            ExplodeID = stream.ReadInt32(isBigEndian);

            // 3180 raw offset in Hex editor, 3180-68 is the offset in IDA.
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            for (int z = 0; z < 8; z++)
            {
                writer.Write(UnkInts0[z], isBigEndian);
            }

            writer.Write(Mass, isBigEndian);
            CenterOfMass.WriteToFile(writer, isBigEndian);
            writer.Write(InteriaMax, isBigEndian);
            Inertia.WriteToFile(writer, isBigEndian);
            writer.Write(MaterialID, isBigEndian);
            writer.Write(StaticFriction, isBigEndian);
            writer.Write(DynamicFriction, isBigEndian);
            writer.Write(Restitution, isBigEndian);
            writer.Write(Power, isBigEndian);
            writer.Write(RotationsMin, isBigEndian);
            writer.Write(RotationsMax, isBigEndian);
            writer.Write(RotationsMaxMax, isBigEndian);
            writer.Write(TorqueStart, isBigEndian);
            writer.Write(TorqueMax, isBigEndian);
            writer.Write(TorqueEnd, isBigEndian);
            writer.Write(RotationsTorqueLow, isBigEndian);
            writer.Write(RotationsTorqueHigh, isBigEndian);
            writer.Write(MotorBrakeTorque, isBigEndian);
            writer.Write(MotorInertia, isBigEndian);
            writer.Write(MotorOrientation, isBigEndian);
            writer.Write(FuelConsumption, isBigEndian);
            writer.Write(FuelTankCapacity, isBigEndian);
            writer.Write(FinalGearCount, isBigEndian);
            writer.Write(FinalGearRatio0, isBigEndian);
            writer.Write(FinalGearRatio1, isBigEndian);
            writer.Write(GearCount, isBigEndian);
            writer.Write(GearReverseCount, isBigEndian);

            #region GearData saving

            for (int z = 0; z < 7; z++)
            {
                writer.Write(GearData[z].GearRatio, isBigEndian);
            }

            for (int z = 0; z < 7; z++)
            {
                writer.Write(GearData[z].RotationsGearUp, isBigEndian);
            }

            for (int z = 0; z < 7; z++)
            {
                writer.Write(GearData[z].RotationsGearDown, isBigEndian);
            }

            for (int z = 0; z < 7; z++)
            {
                writer.Write(GearData[z].SlowStyleGearUpCoeff, isBigEndian);
            }

            for (int z = 0; z < 7; z++)
            {
                writer.Write(GearData[z].MinClutch, isBigEndian);
            }

            for (int z = 0; z < 7; z++)
            {
                writer.Write(GearData[z].MinClutchAngleCoeff, isBigEndian);
            }
            #endregion

            writer.Write(MinClutchGlobal, isBigEndian);
            writer.Write(MinClutchAngleCoeffGlobal, isBigEndian);
            writer.Write(MaxAccelSlowMode, isBigEndian);
            writer.Write(SteerAngleDiffMax, isBigEndian);
            writer.Write(SteerAngleMaxLow, isBigEndian);
            writer.Write(SteerAngleMaxHigh, isBigEndian);
            writer.Write(AngleChangeBackLow, isBigEndian);
            writer.Write(AngleChangeBackHigh, isBigEndian);
            writer.Write(AngleChangeLow, isBigEndian);
            writer.Write(AngleChangeHigh, isBigEndian);
            writer.Write(AngleChangeCoeff, isBigEndian);
            writer.Write(MotorDifferentialIndex, isBigEndian);

            #region DiffTableData saving

            for (int z = 0; z < 10; z++)
            {
                writer.Write(DifferentialData[z].ViscousClutch, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(DifferentialData[z].ViscousClutchRotLim, isBigEndian);
            }

            writer.Write(UnknownDifferential);

            for (int z = 0; z < 10; z++)
            {
                writer.Write(DifferentialData[z].DiffInertia, isBigEndian);
            }
            #endregion

            writer.Write(CDRatio, isBigEndian);
            writer.Write(CDViscousClutch, isBigEndian);
            writer.Write(CDDiffLock, isBigEndian);
            writer.Write(IndexAxleDifferentialFront, isBigEndian);
            writer.Write(IndexAxleDifferentialBack, isBigEndian);
            writer.Write(TyreLateralStiffnessCoeff, isBigEndian);
            writer.Write(TypeLateralDamperCoeff, isBigEndian);

            #region Wheel Data saving
            for (int z = 0; z < 10; z++)
            {
                writer.WriteStringBuffer(32, WheelData[z].WheelModel);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.WriteStringBuffer(8, WheelData[z].Tyre);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].Scale, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].TyreStiffness, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].Pressure, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].SpringK, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].SpringLength, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].SpringPreLoad, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].DamperBound, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].DamperRebound, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].DifferentialIndex, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].AntiRollBarTorque, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].AxleAngleCorrectCoeff, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].ToeIn, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].KPInclination, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].KPCaster, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].Camber, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].RollDamper, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].SteeringCoeff, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].BrakeCoeffLeft, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.Write(WheelData[z].BrakeCoeffRight, isBigEndian);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.WriteByte(WheelData[z].Steering);
            }

            for (int z = 0; z < 10; z++)
            {
                writer.WriteByte(WheelData[z].HandBrake);
            }
            #endregion

            writer.Write(BrakeTorque, isBigEndian);
            writer.Write(BrakeReaction, isBigEndian);
            writer.Write(HandBrakeTorque, isBigEndian);
            writer.Write(BrakeReactionLow, isBigEndian);
            writer.Write(BrakeReactionHigh, isBigEndian);
            writer.Write(AerodynamicSurfaceSize, isBigEndian);
            writer.Write(AerodynamicResistCoeff, isBigEndian);
            writer.Write(FrontSpoilerSurfaceSize, isBigEndian);
            writer.Write(FrontSpoilerResistCoeff, isBigEndian);
            writer.Write(BackSpoilerSurfaceSize, isBigEndian);
            writer.Write(BackSpoilerResistCoeff, isBigEndian);
            writer.Write(ESM);
            writer.Write(ASP);

            //offset in IDA = 1760. sizeof(header) == 68. 1760 + 68.
            writer.Write((ushort)0, isBigEndian);

            writer.Write(ArcadeMinCoeff, isBigEndian);
            writer.Write(AMFakeESP, isBigEndian);
            writer.Write(MinSpeed, isBigEndian);
            writer.Write(MaxSpeedAdd, isBigEndian);
            writer.Write(ESPCoeffMinus, isBigEndian);
            writer.Write(ESPCoeffPlus, isBigEndian);
            writer.Write(AMFakeESPUseASR);
            writer.Write(AMFakeURVM);

            //offset in IDA = 1788. sizeof(header) == 68. 1788 + 68.
            writer.Write((ushort)0, isBigEndian);

            writer.Write(SpeedMaxEffectivity, isBigEndian);
            writer.Write(RotVelSpeedLimit, isBigEndian);
            writer.Write(Coeff, isBigEndian);
            writer.Write(AMFakeURVMUseASR);
            writer.Write(RightWheelForcePos);

            //offset in IDA = 1804. sizeof(header) == 68. 1804 + 68.
            writer.Write((ushort)0, isBigEndian);

            writer.Write(FFRideMagnitudeCoeff, isBigEndian);

            for (int z = 0; z < 3; z++)
            {
                writer.Write(SmokeMotorData[z].SmokeMotorID, isBigEndian);
            }

            for (int z = 0; z < 3; z++)
            {
                writer.Write(SmokeMotorData[z].SmokeMotorDamage, isBigEndian);
            }

            for (int z = 0; z < 3; z++)
            {
                writer.Write(SmokeMotorData[z].SmokeMotorUnk01, isBigEndian);
            }

            writer.Write(SmokeExhaustID, isBigEndian);
            writer.Write(ExplosionID, isBigEndian);
            writer.Write(FireID, isBigEndian);
            writer.Write(SlideEffectID, isBigEndian);
            writer.Write(CrashEffectID, isBigEndian);
            writer.Write(RimSparksID, isBigEndian);
            writer.Write(BurnOutID, isBigEndian);
            writer.Write(BreakTireID, isBigEndian);
            writer.Write(BreakGlassSideWindowID, isBigEndian);
            writer.Write(BreakGlassFrontWindowID, isBigEndian);
            writer.Write(HedgeIDs[0], isBigEndian);
            writer.Write(HedgeIDs[1], isBigEndian);
            writer.Write(RainID, isBigEndian);
            writer.Write(TimeFireMax, isBigEndian);
            writer.Write(EngineSwitchSndCategory10, isBigEndian);
            writer.Write(EngineSwitchSndCategory11, isBigEndian);
            writer.Write(EngineSwitchSndCategory12, isBigEndian);
            writer.Write(EngineSwitchSndId10, isBigEndian);
            writer.Write(EngineSwitchSndId11, isBigEndian);
            writer.Write(EngineSwitchSndId12, isBigEndian);
            writer.Write(EngineSwitchVolume10, isBigEndian);
            writer.Write(EngineSwitchVolume11, isBigEndian);
            writer.Write(EngineSwitchVolume12, isBigEndian);
            writer.Write(EngineSwitchStart10, isBigEndian);
            writer.Write(EngineMinRotSndCategory1, isBigEndian);
            writer.Write(EngineMinRotSndID1, isBigEndian);
            writer.Write(EngineMinRotVolume1, isBigEndian);
            writer.Write(EngineMinRotRotVolHi1, isBigEndian);
            writer.Write(EngineMinRotRotHi1, isBigEndian);
            writer.Write(EngineNPCSndCategory1, isBigEndian);
            writer.Write(EngineNPCSndID1, isBigEndian);
            writer.Write(EngineNPCVolume1, isBigEndian);
            writer.Write(EngineNPCFrqLo1, isBigEndian);
            writer.Write(EngineNPCFrqHi1, isBigEndian);
            writer.Write(EngineNPCRotLoVol1, isBigEndian);
            writer.Write(EngineNPCRotHiVol1, isBigEndian);
            writer.Write(EngineNPCLoVolume1, isBigEndian);
            writer.Write(EngineVolEnvelopeRotLo1, isBigEndian);
            writer.Write(EngineVolEnvelopeRotLoVol1, isBigEndian);
            writer.Write(EngineVolEnvelopeRotHi1, isBigEndian);
            writer.Write(EngineVolEnvelopeRotHiVol1, isBigEndian);
            writer.Write(EngineCrossFadeTimePlus1, isBigEndian);
            writer.Write(EngineCrossFadeTimeMinus1, isBigEndian);

            #region EngineF saving:
            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineSndCategory, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineSndId, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineVolume, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineVolumeNoPower, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineFrqLo, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineFrqHi, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineRotLo, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineRotLoVol, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineRotHiVol, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineRotHi, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineFData[z].EngineNoClutchLockOnly);
            }
            #endregion

            #region EngineB saving:
            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineSndCategory, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineSndId, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineVolume, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineVolumeNoPower, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineFrqLo, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineFrqHi, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineRotLo, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineRotLoVol, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineRotHiVol, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineRotHi, isBigEndian);
            }

            for (int z = 0; z < 8; z++)
            {
                writer.Write(EngineBData[z].EngineNoClutchLockOnly);
            }
            #endregion

            writer.Write(EngineFizzSndCategory, isBigEndian);

            #region EngineFizzData
            for (int z = 0; z < EngineFizzData.Length; z++)
            {
                writer.Write(EngineFizzData[z].EngineFizzSndID, isBigEndian);
            }

            for (int z = 0; z < EngineFizzData.Length; z++)
            {
                writer.Write(EngineFizzData[z].EngineFizzVolume, isBigEndian);
            }
            #endregion

            writer.Write(EngineCoolingSndCategory1, isBigEndian);
            writer.Write(EngineCoolingSndId1, isBigEndian);
            writer.Write(EngineCoolingVolume1, isBigEndian);
            writer.Write(EngineCoolingTemperatureVolume1, isBigEndian);
            writer.Write(EngineCoolingTemperatureSilencion1, isBigEndian);
            writer.Write(EngineFanSndCategory, isBigEndian);
            writer.Write(EngineFanSndId, isBigEndian);
            writer.Write(m_EngineFanVolume, isBigEndian);
            writer.Write(EnvironmentalSndCategory1, isBigEndian);
            writer.Write(EnvironmentalSndId1, isBigEndian);
            writer.Write(EnvironmentalVolume1, isBigEndian);
            writer.Write(EnvironmentalFrqLo1, isBigEndian);
            writer.Write(EnvironmentalFrqHi1, isBigEndian);
            writer.Write(EnvironmentalSpeedMaxVolume1, isBigEndian);
            writer.Write(AirPumpSndCategory1, isBigEndian);
            writer.Write(AirPumpSndId1, isBigEndian);
            writer.Write(AirPumpVolume1, isBigEndian);
            writer.Write(DoorOpenSndCategory1, isBigEndian);
            writer.Write(DoorOpenSndId1, isBigEndian);
            writer.Write(DoorOpenVolume1, isBigEndian);
            writer.Write(DoorOpenSndId1b, isBigEndian);
            writer.Write(DoorOpenVolume1b, isBigEndian);
            writer.Write(DoorCloseSndCategory1, isBigEndian);
            writer.Write(DoorCloseSndId1, isBigEndian);
            writer.Write(DoorCloseVolume1, isBigEndian);
            writer.Write(DoorCloseSndId1b, isBigEndian);
            writer.Write(DoorCloseVolume1b, isBigEndian);
            writer.Write(CoverOpenSndCategory1, isBigEndian);
            writer.Write(CoverOpenSndId1, isBigEndian);
            writer.Write(CoverOpenVolume1, isBigEndian);
            writer.Write(CoverCloseSndCategory1, isBigEndian);
            writer.Write(CoverCloseSndId1, isBigEndian);
            writer.Write(CoverCloseVolume1, isBigEndian);
            writer.Write(KnockingBonnetSndCategory1, isBigEndian);

            #region KnockingBonnetData
            for (int z = 0; z < KnockingBonnetData.Length; z++)
            {
                writer.Write(KnockingBonnetData[z].KnockingBonnetSndId, isBigEndian);
            }

            for (int z = 0; z < KnockingBonnetData.Length; z++)
            {
                writer.Write(KnockingBonnetData[z].KnockingBonnetSpeed, isBigEndian);
            }

            for (int z = 0; z < KnockingBonnetData.Length; z++)
            {
                writer.Write(KnockingBonnetData[z].KnockingBonnetVolume, isBigEndian);
            }
            #endregion

            writer.Write(DropHoodSndCategory1, isBigEndian);
            writer.Write(DropHoodSndId1, isBigEndian);
            writer.Write(DropHoodVolume1, isBigEndian);

            #region CrashTable Data
            for (int z = 0; z < CrashData.Length; z++)
            {
                writer.Write(CrashData[z].CrashSndCategory, isBigEndian);
            }

            for (int z = 0; z < CrashData.Length; z++)
            {
                writer.Write(CrashData[z].CrashSndId, isBigEndian);
            }

            for (int z = 0; z < CrashData.Length; z++)
            {
                writer.Write(CrashData[z].CrashVolume, isBigEndian);
            }

            for (int z = 0; z < CrashData.Length; z++)
            {
                writer.Write(CrashData[z].CrashSpeed, isBigEndian);
            }
            #endregion

            writer.Write(GearboxShiftSndCategory1, isBigEndian);
            writer.Write(GearboxShiftSndId1, isBigEndian);
            writer.Write(GearboxShiftVolume1, isBigEndian);
            writer.Write(HandbrakeSndCategory1, isBigEndian);
            writer.Write(HandbrakeSndId1, isBigEndian);
            writer.Write(HandbrakeVolume1, isBigEndian);
            writer.Write(WheelShankSndCategory1, isBigEndian);
            writer.Write(WheelShankSndId11, isBigEndian);
            writer.Write(WheelShankSndId21, isBigEndian);
            writer.Write(WheelShankSndId31, isBigEndian);
            writer.Write(WheelShankHeight11, isBigEndian);
            writer.Write(WheelShankHeight21, isBigEndian);
            writer.Write(WheelShankSpeed11, isBigEndian);
            writer.Write(WheelShankSpeed21, isBigEndian);
            writer.Write(WheelShankSpeed31, isBigEndian);
            writer.Write(WheelShankVolume11, isBigEndian);
            writer.Write(WheelShankVolume21, isBigEndian);
            writer.Write(WheelShankVolume31, isBigEndian);
            writer.Write(HornSndCategory1, isBigEndian);
            writer.Write(HornSndId1, isBigEndian);
            writer.Write(HornVolume1, isBigEndian);
            writer.Write(SirenSndCategory1, isBigEndian);
            writer.Write(SirenSndId1, isBigEndian);
            writer.Write(SirenVolume1, isBigEndian);
            writer.Write(TyreBreakSndCategory1, isBigEndian);
            writer.Write(TyreBreakSndId1, isBigEndian);
            writer.Write(TyreBreakVolume1, isBigEndian);
            writer.Write(TyreCrashSndCategory1, isBigEndian);
            writer.Write(TyreCrashSndId1, isBigEndian);
            writer.Write(TyreCrashVolume1, isBigEndian);
            writer.Write(TyreCrashFrqLo1, isBigEndian);
            writer.Write(TyreCrashFrqHi1, isBigEndian);
            writer.Write(TyreCrashSpeedLoVol1, isBigEndian);
            writer.Write(TyreCrashSpeedHiVol1, isBigEndian);
            writer.Write(RimRideSndCategory1, isBigEndian);
            writer.Write(RimRideSndId1, isBigEndian);
            writer.Write(RimRideVolume1, isBigEndian);
            writer.Write(RimRideFrqLo1, isBigEndian);
            writer.Write(RimRideFrqHi1, isBigEndian);
            writer.Write(RimRideSpeedLoVol1, isBigEndian);
            writer.Write(RimRideSpeedHiVol1, isBigEndian);
            writer.Write(GlassBreakSndCategory, isBigEndian);
            writer.Write(GlassBreakSndId, isBigEndian);
            writer.Write(GlassBreakSndVolume, isBigEndian);
            writer.Write(ExplosionSndCategory, isBigEndian);
            writer.Write(ExplosionSndId, isBigEndian);
            writer.Write(ExplosionSndVolume, isBigEndian);
            writer.Write(FireSndCategory, isBigEndian);
            writer.Write(FireSndId, isBigEndian);
            writer.Write(FireSndVolume, isBigEndian);
            writer.Write(SqueakingBrakesSndCategory, isBigEndian);
            writer.Write(SqueakingBrakesSndId, isBigEndian);
            writer.Write(SqueakingBrakesVolume, isBigEndian);
            writer.Write(SqueakingBrakesSpeedHi, isBigEndian);
            writer.Write(RainSndCategory, isBigEndian);
            writer.Write(RainSndId, isBigEndian);
            writer.Write(RainVolumeLo, isBigEndian);
            writer.Write(RainVolumeHi, isBigEndian);
            writer.Write(RainFrqLo, isBigEndian);
            writer.Write(RainFrqHi, isBigEndian);
            writer.Write(RainDensityLo, isBigEndian);
            writer.Write(RainDensityHi, isBigEndian);
            writer.Write(HedgeSndCategory, isBigEndian);
            writer.Write(HedgeSndId, isBigEndian);
            writer.Write(HedgeFrqLo, isBigEndian);
            writer.Write(HedgeFrqHi, isBigEndian);
            writer.Write(HedgeSpeedMidVolume, isBigEndian);
            writer.Write(HedgeVolumeMid, isBigEndian);
            writer.Write(HedgeSpeedMaxVolume, isBigEndian);
            writer.Write(HedgeVolumeMax, isBigEndian);
            writer.Write(UnkBytes);

            writer.Write(SlideId, isBigEndian);
            writer.Write(SlideSpeedMax, isBigEndian);
            writer.Write(SlideSpeedVolMax, isBigEndian);
            writer.Write(SlideMinVol, isBigEndian);
            writer.Write(SlideMaxVol, isBigEndian);
            writer.Write(SlideFreqLow, isBigEndian);
            writer.Write(SlideFreqHi, isBigEndian);

            writer.Write(RollId, isBigEndian);
            writer.Write(RollSpeedMax, isBigEndian);
            writer.Write(RollSpeedVolMax, isBigEndian);
            writer.Write(RollMinVol, isBigEndian);
            writer.Write(RollMaxVol, isBigEndian);
            writer.Write(RollFreqLow, isBigEndian);
            writer.Write(RollFreqHi, isBigEndian);

            writer.Write(SndChngVersionId, isBigEndian);
            writer.Write(SndBreakId, isBigEndian);
            writer.Write(SndDelayId, isBigEndian);
            writer.Write(SndDelayDelay, isBigEndian);
            writer.Write(ParticleHitId, isBigEndian);
            writer.Write(ParticleBreakId, isBigEndian);
            writer.Write(ParticleChngVersionId, isBigEndian);
            writer.Write(ParticleSlideId, isBigEndian);
            writer.Write(ParticleHitSpeedMin, isBigEndian);
            writer.Write(ExplodeID, isBigEndian);
        }
    }
}
