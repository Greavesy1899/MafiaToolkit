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
                public short Steering { get; set; }
                public float HandBrake { get; set; }
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

                    for(int z = 0; z < 7; z++)
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

                    var scale = stream.ReadSingle(isBigEndian);
                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].Scale = scale;

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
                        table.WheelData[z].Steering = stream.ReadInt16(isBigEndian);

                    for (int z = 0; z < 10; z++)
                        table.WheelData[z].HandBrake = stream.ReadSingle(isBigEndian);
                    #endregion

                    Debug.WriteLine("{0}", stream.Position - 68);
                }


            }
        }

    }
}