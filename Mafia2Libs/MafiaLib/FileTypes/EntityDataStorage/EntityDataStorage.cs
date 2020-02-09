using SharpDX;
using System;
using System.Diagnostics;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.EntityDataStorage
{
    public class EntityDataStorageLoader
    {
        public class EntityTable
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
                public byte[] DiffLock { get; set; }
                public float DiffInertia { get; set; }

                public override string ToString()
                {
                    return string.Format("{0} {1} {2} {3}", ViscousClutch, ViscousClutchRotLim, DiffLock, DiffInertia);
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
            }
            public class SmokeMotorTableData
            {
                public int SmokeMotorID;
                public float SmokeMotorDamage;
                public float SmokeMotorUnk01;
            }
            public class EngineTableData
            {
                public int EngineFSndCategory { get; set; }
                public int EngineFSndId { get; set; }
                public float EngineFVolume { get; set; }
                public float EngineFVolumeNoPower { get; set; }
                public float EngineFFrqLo { get; set; }
                public float EngineFFrqHi { get; set; }
                public float EngineFRotLo { get; set; }
                public float EngineFRotLoVol { get; set; }
                public float EngineFRotHiVol { get; set; }
                public float EngineFRotHi { get; set; }
                public float EngineFNoClutchLockOnly { get; set; }
            }

            public ulong Hash { get; set;}
            public int[] UnkInts0 { get; set; }
            public float Mass { get; set; }
            public Vector3 CenterOfMass { get; set; }
            public float InteriaMax { get; set; }
            public Vector3 Inertia { get; set; }
            public int MaterialID { get; set; }
            public float StaticFriction { get; set; }
            public float DynamicFriction { get; set; }
            public float Restitution { get; set; }
            public float Power { get; set; }
            public float RotationsMin { get; set; }
            public float RotationsMax { get; set; }
            public float RotationsMaxMax { get; set; }
            public float TorqueStart { get; set; }
            public float TorqueMax { get; set; }
            public float TorqueEnd { get; set; }
            public float RotationsTorqueLow { get; set; }
            public float RotationsTorqueHigh { get; set; }
            public float MotorBrakeTorque { get; set; }
            public float MotorInertia { get; set; }
            public float MotorOrientation { get; set; }
            public float FuelConsumption { get; set; }
            public float FuelTankCapacity { get; set; }
            public int FinalGearCount { get; set; }
            public float FinalGearRatio0 { get; set; }
            public float FinalGearRatio1 { get; set; }
            public int GearCount { get; set; }
            public int GearReverseCount { get; set; }
            public GearTableData[] GearData { get; set; }
            public float MinClutchGlobal { get; set; }
            public float MinClutchAngleCoeffGlobal { get; set; }
            public float MaxAccelSlowMode { get; set; }
            public float SteerAngleDiffMax { get; set; }
            public float SteerAngleMaxLow { get; set; }
            public float SteerAngleMaxHigh { get; set; }
            public float AngleChangeBackLow { get; set; }
            public float AngleChangeBackHigh { get; set; }
            public float AngleChangeLow { get; set; }
            public float AngleChangeHigh { get; set; }
            public float AngleChangeCoeff { get; set; }
            public int MotorDifferentialIndex { get; set; }
            public DifferentialTableData[] DifferentialData { get; set; }
            public byte[] UnknownDifferential { get; set; }
            public float CDRatio { get; set; }
            public float CDViscousClutch { get; set; }
            public float CDDiffLock { get; set; }
            public int IndexAxleDifferentialFront { get; set; }
            public int IndexAxleDifferentialBack { get; set; }
            public float TyreLateralStiffnessCoeff { get; set; }
            public float TypeLateralDamperCoeff { get; set; }
            public WheelTableData[] WheelData { get; set; }
            public float BrakeTorque { get; set; }
            public float BrakeReaction { get; set; }
            public float HandBrakeTorque { get; set; }
            public float BrakeReactionLow { get; set; }
            public float BrakeReactionHigh { get; set; }
            public float AerodynamicSurfaceSize { get; set; }
            public float AerodynamicResistCoeff { get; set; }
            public float FrontSpoilerSurfaceSize { get; set; }
            public float FrontSpoilerResistCoeff { get; set; }
            public float BackSpoilerSurfaceSize { get; set; }
            public float BackSpoilerResistCoeff { get; set; }
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
            public SmokeMotorTableData[] SmokeMotorData { get; set; }
            public short SmokeExhaustID { get; set; }
            public short ExplosionID { get; set; }
            public short FireID { get; set; }
            public short SlideEffectID { get; set; }
            public short CrashEffectID { get; set; }
            public short RimSparksID { get; set; }
            public short BurnOutID { get; set; }
            public short BreakTireID { get; set; }
            public short BreakGlassSideWindowID { get; set; }
            public short BreakGlassFrontWindowID { get; set; }
            public short[] HedgeIDs { get; set; }
            public int RainID { get; set; }
            public float TimeFireMax { get; set; }
            public int EngineSwitchSndCategory10 { get; set; }
            public int EngineSwitchSndId10 { get; set; }
            public float EngineSwitchVolume10 { get; set; }
            public float EngineSwitchStart10 { get; set; }
            public int EngineSwitchSndCategory11 { get; set; }
            public int EngineSwitchSndId11 { get; set; }
            public float EngineSwitchVolume11 { get; set; }
            public int EngineSwitchSndCategory12 { get; set; }
            public int EngineSwitchSndId12 { get; set; }
            public float EngineSwitchVolume12 { get; set; }
            public int EngineMinRotSndCategory1 { get; set; }
            public int EngineMinRotSndID1 { get; set; }
            public float EngineMinRotVolume1 { get; set; }
            public float EngineMinRotRotVolHi1 { get; set; }
            public float EngineMinRotRotHi1 { get; set; }
            public int EngineNPCSndCategory1 { get; set; }
            public int EngineNPCSndID1 { get; set; }
            public float EngineNPCVolume1 { get; set; }
            public float EngineNPCFrqLo1 { get; set; }
            public float EngineNPCFrqHi1 { get; set; }
            public float EngineNPCRotLoVol1 { get; set; }
            public float EngineNPCRotHiVol1 { get; set; }
            public float EngineNPCLoVolume1 { get; set; }
            public float EngineVolEnvelopeRotLo1 { get; set; }
            public float EngineVolEnvelopeRotLoVol1 { get; set; }
            public float EngineVolEnvelopeRotHi1 { get; set; }
            public float EngineVolEnvelopeRotHiVol1 { get; set; }
            public float EngineCrossFadeTimePlus1 { get; set; }
            public float EngineCrossFadeTimeMinus1 { get; set; }
            public EngineTableData[] EngineFData { get; set; }
            public EngineTableData[] EngineBData { get; set; }
        }

        int entityType; //Cars have 18, City_Universe has 13.
        ulong hash; //Seems to exist for both types.
        uint tableSize; //size for each tableset.
        EntityTable[] Tables;

        public EntityDataStorageLoader()
        {
        }

        public void ReadFromFile(string fileName, bool isBigEndian)
        {
            using (var stream = new MemoryStream(File.ReadAllBytes(fileName)))
            {
                entityType = stream.ReadInt32(isBigEndian);
                hash = stream.ReadUInt64(isBigEndian);
                tableSize = stream.ReadUInt32(isBigEndian);

                var numTables = stream.ReadInt32(isBigEndian);
                Tables = new EntityTable[numTables];

                for (int i = 0; i < numTables; i++)
                {
                    var table = new EntityTable();
                    table.Hash = stream.ReadUInt64(isBigEndian);
                    Tables[i] = table;
                }

                for (int i = 0; i < numTables; i++)
                {
                    var table = Tables[i];
                    table.UnkInts0 = new int[8];
                    for (int z = 0; z < 8; z++)
                        table.UnkInts0[z] = stream.ReadInt32(isBigEndian);

                    table.Mass = stream.ReadSingle(isBigEndian);
                    table.CenterOfMass = Vector3Extenders.ReadFromFile(stream, isBigEndian);
                    table.InteriaMax = stream.ReadSingle(isBigEndian);
                    table.Inertia = Vector3Extenders.ReadFromFile(stream, isBigEndian);
                    table.MaterialID = stream.ReadInt32(isBigEndian);
                    table.StaticFriction = stream.ReadSingle(isBigEndian);
                    table.DynamicFriction = stream.ReadSingle(isBigEndian);
                    table.Restitution = stream.ReadSingle(isBigEndian);
                    table.Power = stream.ReadSingle(isBigEndian);
                    table.RotationsMin = stream.ReadSingle(isBigEndian);
                    table.RotationsMax = stream.ReadSingle(isBigEndian);
                    table.RotationsMaxMax = stream.ReadSingle(isBigEndian);
                    table.TorqueStart = stream.ReadSingle(isBigEndian);
                    table.TorqueMax = stream.ReadSingle(isBigEndian);
                    table.TorqueEnd = stream.ReadSingle(isBigEndian);
                    table.RotationsTorqueLow = stream.ReadSingle(isBigEndian);
                    table.RotationsTorqueHigh = stream.ReadSingle(isBigEndian);
                    table.MotorBrakeTorque = stream.ReadSingle(isBigEndian);
                    table.MotorInertia = stream.ReadSingle(isBigEndian);
                    table.MotorOrientation = stream.ReadSingle(isBigEndian);
                    table.FuelConsumption = stream.ReadSingle(isBigEndian);
                    table.FuelTankCapacity = stream.ReadSingle(isBigEndian);
                    table.FinalGearCount = stream.ReadInt32(isBigEndian);
                    table.FinalGearRatio0 = stream.ReadSingle(isBigEndian);
                    table.FinalGearRatio1 = stream.ReadSingle(isBigEndian);
                    table.GearCount = stream.ReadInt32(isBigEndian);
                    table.GearReverseCount = stream.ReadInt32(isBigEndian);

                    #region GearData parsing
                    table.GearData = new EntityTable.GearTableData[7];

                    for (int z = 0; z < 7; z++)
                    {
                        var gearData = new EntityTable.GearTableData();
                        gearData.GearRatio = stream.ReadSingle(isBigEndian);
                        table.GearData[z] = gearData;
                    }

                    for (int z = 0; z < 7; z++)
                        table.GearData[z].RotationsGearUp = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 7; z++)
                        table.GearData[z].RotationsGearDown = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 7; z++)
                        table.GearData[z].SlowStyleGearUpCoeff = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 7; z++)
                        table.GearData[z].MinClutch = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 7; z++)
                        table.GearData[z].MinClutchAngleCoeff = stream.ReadSingle(isBigEndian);
                    #endregion

                    table.MinClutchGlobal = stream.ReadSingle(isBigEndian);
                    table.MinClutchAngleCoeffGlobal = stream.ReadSingle(isBigEndian);
                    table.MaxAccelSlowMode = stream.ReadSingle(isBigEndian);
                    table.SteerAngleDiffMax = stream.ReadSingle(isBigEndian);
                    table.SteerAngleMaxLow = stream.ReadSingle(isBigEndian);
                    table.SteerAngleMaxHigh = stream.ReadSingle(isBigEndian);
                    table.AngleChangeBackLow = stream.ReadSingle(isBigEndian);
                    table.AngleChangeBackHigh = stream.ReadSingle(isBigEndian);
                    table.AngleChangeLow = stream.ReadSingle(isBigEndian);
                    table.AngleChangeHigh = stream.ReadSingle(isBigEndian);
                    table.AngleChangeCoeff = stream.ReadSingle(isBigEndian);
                    table.MotorDifferentialIndex = stream.ReadInt32(isBigEndian);

                    #region DiffTableData parsing
                    table.DifferentialData = new EntityTable.DifferentialTableData[10];

                    for (int z = 0; z < 10; z++)
                    {
                        var diffData = new EntityTable.DifferentialTableData();
                        diffData.ViscousClutch = stream.ReadSingle(isBigEndian);
                        table.DifferentialData[z] = diffData;
                    }

                    for (int z = 0; z < 10; z++)
                        table.DifferentialData[z].ViscousClutchRotLim = stream.ReadSingle(isBigEndian);

                    table.UnknownDifferential = stream.ReadBytes(12);

                    for (int z = 0; z < 10; z++)
                        table.DifferentialData[z].DiffInertia = stream.ReadSingle(isBigEndian);
                    #endregion

                    table.CDRatio = stream.ReadSingle(isBigEndian);
                    table.CDViscousClutch = stream.ReadSingle(isBigEndian);
                    table.CDDiffLock = stream.ReadSingle(isBigEndian);
                    table.IndexAxleDifferentialFront = stream.ReadInt32(isBigEndian);
                    table.IndexAxleDifferentialBack = stream.ReadInt32(isBigEndian);
                    table.TyreLateralStiffnessCoeff = stream.ReadSingle(isBigEndian);
                    table.TypeLateralDamperCoeff = stream.ReadSingle(isBigEndian);

                    #region Wheel Data parsing
                    table.WheelData = new EntityTable.WheelTableData[10];
                    for (int z = 0; z < 10; z++)
                    {
                        var wheelData = new EntityTable.WheelTableData();
                        wheelData.WheelModel = stream.ReadStringBuffer(32);
                        table.WheelData[z] = wheelData;
                    }

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].Tyre = stream.ReadStringBuffer(8);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].Scale = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].TyreStiffness = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].Pressure = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].SpringK = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].SpringLength = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].SpringPreLoad = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].DamperBound = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].DamperRebound = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].DifferentialIndex = stream.ReadInt32(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].AntiRollBarTorque = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].AxleAngleCorrectCoeff = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].ToeIn = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].KPInclination = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].KPCaster = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].Camber = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].RollDamper = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].SteeringCoeff = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].BrakeCoeffLeft = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].BrakeCoeffRight = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].Steering = stream.ReadByte8();

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].HandBrake = stream.ReadByte8();
                    #endregion

                    table.BrakeTorque = stream.ReadSingle(isBigEndian);
                    table.BrakeReaction = stream.ReadSingle(isBigEndian);
                    table.HandBrakeTorque = stream.ReadSingle(isBigEndian);
                    table.BrakeReactionLow = stream.ReadSingle(isBigEndian);
                    table.BrakeReactionHigh = stream.ReadSingle(isBigEndian);
                    table.AerodynamicSurfaceSize = stream.ReadSingle(isBigEndian);
                    table.AerodynamicResistCoeff = stream.ReadSingle(isBigEndian);
                    table.FrontSpoilerSurfaceSize = stream.ReadSingle(isBigEndian);
                    table.FrontSpoilerResistCoeff = stream.ReadSingle(isBigEndian);
                    table.BackSpoilerSurfaceSize = stream.ReadSingle(isBigEndian);
                    table.BackSpoilerResistCoeff = stream.ReadSingle(isBigEndian);
                    table.ESM = stream.ReadBoolean();
                    table.ASP = stream.ReadBoolean();

                    //offset in IDA = 1760. sizeof(header) == 68. 1760 + 68.
                    stream.Seek(1828, SeekOrigin.Begin);

                    table.ArcadeMinCoeff = stream.ReadSingle(isBigEndian);
                    table.AMFakeESP = stream.ReadInt32(isBigEndian);
                    table.MinSpeed = stream.ReadSingle(isBigEndian);
                    table.MaxSpeedAdd = stream.ReadSingle(isBigEndian);
                    table.ESPCoeffMinus = stream.ReadSingle(isBigEndian);
                    table.ESPCoeffPlus = stream.ReadSingle(isBigEndian);
                    table.AMFakeESPUseASR = stream.ReadBoolean();
                    table.AMFakeURVM = stream.ReadBoolean();

                    //offset in IDA = 1788. sizeof(header) == 68. 1788 + 68.
                    stream.Seek(1856, SeekOrigin.Begin);

                    table.SpeedMaxEffectivity = stream.ReadSingle(isBigEndian);
                    table.RotVelSpeedLimit = stream.ReadSingle(isBigEndian);
                    table.Coeff = stream.ReadSingle(isBigEndian);
                    table.AMFakeURVMUseASR = stream.ReadBoolean();
                    table.RightWheelForcePos = stream.ReadBoolean();

                    //offset in IDA = 1804. sizeof(header) == 68. 1804 + 68.
                    stream.Seek(1872, SeekOrigin.Begin);

                    table.FFRideMagnitudeCoeff = stream.ReadSingle(isBigEndian);
                    table.SmokeMotorData = new EntityTable.SmokeMotorTableData[3];

                    for (int z = 0; z < 3; z++)
                    {
                        table.SmokeMotorData[z] = new EntityTable.SmokeMotorTableData();
                        table.SmokeMotorData[z].SmokeMotorID = stream.ReadInt32(isBigEndian);
                    }

                    for (int z = 0; z < 3; z++)
                        table.SmokeMotorData[z].SmokeMotorDamage = stream.ReadSingle(isBigEndian);

                    for (int z = 0; z < 3; z++)
                        table.SmokeMotorData[z].SmokeMotorUnk01 = stream.ReadSingle(isBigEndian);

                    table.SmokeExhaustID = stream.ReadInt16(isBigEndian);
                    table.ExplosionID = stream.ReadInt16(isBigEndian);
                    table.FireID = stream.ReadInt16(isBigEndian);
                    table.SlideEffectID = stream.ReadInt16(isBigEndian);
                    table.CrashEffectID = stream.ReadInt16(isBigEndian);
                    table.RimSparksID = stream.ReadInt16(isBigEndian);
                    table.BurnOutID = stream.ReadInt16(isBigEndian);
                    table.BreakTireID = stream.ReadInt16(isBigEndian);
                    table.BreakGlassSideWindowID = stream.ReadInt16(isBigEndian);
                    table.BreakGlassFrontWindowID = stream.ReadInt16(isBigEndian);
                    table.HedgeIDs = new short[2];
                    table.HedgeIDs[0] = stream.ReadInt16(isBigEndian);
                    table.HedgeIDs[1] = stream.ReadInt16(isBigEndian);
                    table.RainID = stream.ReadInt32(isBigEndian);
                    table.TimeFireMax = stream.ReadSingle(isBigEndian);
                    table.EngineSwitchSndCategory10 = stream.ReadInt32(isBigEndian);
                    table.EngineSwitchSndCategory11 = stream.ReadInt32(isBigEndian);
                    table.EngineSwitchSndCategory12 = stream.ReadInt32(isBigEndian);
                    table.EngineSwitchSndId10 = stream.ReadInt32(isBigEndian);
                    table.EngineSwitchSndId11 = stream.ReadInt32(isBigEndian);
                    table.EngineSwitchSndId12 = stream.ReadInt32(isBigEndian);
                    table.EngineSwitchVolume10 = stream.ReadSingle(isBigEndian);
                    table.EngineSwitchVolume11 = stream.ReadSingle(isBigEndian);
                    table.EngineSwitchVolume12 = stream.ReadSingle(isBigEndian);
                    table.EngineSwitchStart10 = stream.ReadSingle(isBigEndian);
                    table.EngineMinRotSndCategory1 = stream.ReadInt32(isBigEndian);
                    table.EngineMinRotSndID1 = stream.ReadInt32(isBigEndian);
                    table.EngineMinRotVolume1 = stream.ReadSingle(isBigEndian);
                    table.EngineMinRotRotVolHi1 = stream.ReadSingle(isBigEndian);
                    table.EngineMinRotRotHi1 = stream.ReadSingle(isBigEndian);
                    table.EngineNPCSndCategory1 = stream.ReadInt32(isBigEndian);
                    table.EngineNPCSndID1 = stream.ReadInt32(isBigEndian);
                    table.EngineNPCVolume1 = stream.ReadSingle(isBigEndian);
                    table.EngineNPCFrqLo1 = stream.ReadSingle(isBigEndian);
                    table.EngineNPCFrqHi1 = stream.ReadSingle(isBigEndian);
                    table.EngineNPCRotLoVol1 = stream.ReadSingle(isBigEndian);
                    table.EngineNPCRotHiVol1 = stream.ReadSingle(isBigEndian);
                    table.EngineNPCLoVolume1 = stream.ReadSingle(isBigEndian);
                    table.EngineVolEnvelopeRotLo1 = stream.ReadSingle(isBigEndian);
                    table.EngineVolEnvelopeRotLoVol1 = stream.ReadSingle(isBigEndian);
                    table.EngineVolEnvelopeRotHi1 = stream.ReadSingle(isBigEndian);
                    table.EngineVolEnvelopeRotHiVol1 = stream.ReadSingle(isBigEndian);
                    table.EngineCrossFadeTimePlus1 = stream.ReadSingle(isBigEndian);
                    table.EngineCrossFadeTimeMinus1 = stream.ReadSingle(isBigEndian);
                    table.EngineFData = new EntityTable.EngineTableData[8];
                    table.EngineBData = new EntityTable.EngineTableData[8];

                    #region EngineF Parsing:
                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z] = new EntityTable.EngineTableData();
                        table.EngineFData[z].EngineFSndCategory = stream.ReadInt32(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++) 
                    {
                        table.EngineFData[z].EngineFSndId = stream.ReadInt32(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z].EngineFVolume = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z].EngineFVolumeNoPower = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z].EngineFFrqLo = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z].EngineFFrqHi = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z].EngineFRotLo = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z].EngineFRotLoVol = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z].EngineFRotHiVol = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z].EngineFRotHi = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineFData[z].EngineFNoClutchLockOnly = stream.ReadSingle(isBigEndian);
                    }
                    #endregion

                    #region EngineB Parsing:
                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z] = new EntityTable.EngineTableData();
                        table.EngineBData[z].EngineFSndCategory = stream.ReadInt32(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFSndId = stream.ReadInt32(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFVolume = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFVolumeNoPower = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFFrqLo = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFFrqHi = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFRotLo = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFRotLoVol = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFRotHiVol = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFRotHi = stream.ReadSingle(isBigEndian);
                    }

                    for (int z = 0; z < 8; z++)
                    {
                        table.EngineBData[z].EngineFNoClutchLockOnly = stream.ReadSingle(isBigEndian);
                    }
                    #endregion

                    Debug.WriteLine("{0}", stream.Position - 68);
                }
            }
        }

    }
}