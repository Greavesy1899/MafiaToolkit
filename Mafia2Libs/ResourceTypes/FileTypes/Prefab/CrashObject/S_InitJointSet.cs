using BitStreams;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Utils.Extensions;
using Vortice.Mathematics;

namespace ResourceTypes.Prefab.CrashObject
{
    [Flags]
    public enum S_InitJointSet_Flags : uint
    {
        ANGULAR_FREE_X = 0x1,
        ANGULAR_FREE_Y = 0x2,
        ANGULAR_FREE_Z = 0x4,
        LINEAR_FREE_X = 0x8,
        LINEAR_FREE_Y = 0x10,
        LINEAR_FREE_Z = 0x20,
        DO_NOT_REMOVE = 0x1000,
        PRECISION = 0x2000,
        NO_IB = 0xFFFFEFFF,
    }

    public class S_InitJointSet
    {
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public S_InitJointSet_Flags Flags { get; set; } // Flag Types - [NO IB = 0xFFFFEFFF, PRECISION = 0x2000] [ANGULAR FREE Z = 0x4, ANGULAR FREE Y = 0x2 , ANGULAR FREE X = 0x1] [LINEAR FREE Z = 0x20, LINEAR FREE Y = 0x10 , LINEAR FREE X = 0x8]
        public float EnergyDeform { get; set; } // Game does [v12 * 0.0099999998]
        public C_Vector3 LinearLimit_0 { get; set; }
        public C_Vector3 LinearLimit_1 { get; set; }
        public C_Vector3 LinearBreak_0 { get; set; }
        public C_Vector3 LinearBreak_1 { get; set; }
        public C_Vector3 LinearSpring_0 { get; set; }
        public C_Vector3 LinearSpring_1 { get; set; }
        public C_Vector3 LinearDamper { get; set; }
        public C_Vector3 LinearMotor_0 { get; set; }
        public C_Vector3 LinearMotor_1 { get; set; }
        public C_Vector3 AngularLimit_0 { get; set; }
        public C_Vector3 AngularLimit_1 { get; set; }
        public C_Vector3 AngularBreak_0 { get; set; }
        public C_Vector3 AngularBreak_1 { get; set; }
        public C_Vector3 AngularSpring_0 { get; set; }
        public C_Vector3 AngularSpring_1 { get; set; }
        public C_Vector3 AngularDamper { get; set; }
        public C_Vector3 AngularMotor_0 { get; set; }
        public C_Vector3 AngularMotor_1 { get; set; }

        private float[,] OriginalFloats;

        public S_InitJointSet() 
        {
            LinearLimit_0 = new C_Vector3();
            LinearLimit_1 = new C_Vector3();
            LinearBreak_0 = new C_Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            LinearBreak_1 = new C_Vector3(float.MinValue, float.MinValue, float.MinValue);
            LinearSpring_0 = new C_Vector3();
            LinearSpring_1 = new C_Vector3();
            LinearDamper = new C_Vector3();
            LinearMotor_0 = new C_Vector3();
            LinearMotor_1 = new C_Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            AngularLimit_0 = new C_Vector3();
            AngularLimit_1 = new C_Vector3();
            AngularBreak_0 = new C_Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            AngularBreak_1 = new C_Vector3(float.MinValue, float.MinValue, float.MinValue);
            AngularSpring_0 = new C_Vector3();
            AngularSpring_1 = new C_Vector3();
            AngularDamper = new C_Vector3();
            AngularMotor_0 = new C_Vector3();
            AngularMotor_1 = new C_Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        public void Load(BitStream MemStream)
        {
            Flags = (S_InitJointSet_Flags)MemStream.ReadUInt32();
            EnergyDeform = MemStream.ReadSingle() / 0.0099999998f;

            float[,] LinearAndAngularData = new float[6,9];
            for (int i = 0; i < 6; i++)
            {
                for (int x = 0; x < 9; x++)
                {
                    LinearAndAngularData[i, x] = MemStream.ReadSingle();
                }
            }
            OriginalFloats = LinearAndAngularData;

            // Create new C_Vector3s by directly grabbing the values from the array.
            // Do Linear variables 
            LinearLimit_0 = new C_Vector3(LinearAndAngularData[2, 0], LinearAndAngularData[1, 0], LinearAndAngularData[0, 0]);
            LinearLimit_1 = new C_Vector3(LinearAndAngularData[2, 1], LinearAndAngularData[1, 1], LinearAndAngularData[0, 1]);
            LinearBreak_0 = new C_Vector3(LinearAndAngularData[2, 2], LinearAndAngularData[1, 2], LinearAndAngularData[0, 2]);
            LinearBreak_1 = new C_Vector3(LinearAndAngularData[2, 3], LinearAndAngularData[1, 3], LinearAndAngularData[0, 3]);
            LinearSpring_0 = new C_Vector3(LinearAndAngularData[2, 4], LinearAndAngularData[1, 4], LinearAndAngularData[0, 4]);
            LinearSpring_1 = new C_Vector3(LinearAndAngularData[2, 5], LinearAndAngularData[1, 5], LinearAndAngularData[0, 5]);
            LinearDamper = new C_Vector3(LinearAndAngularData[2, 6], LinearAndAngularData[1, 6], LinearAndAngularData[0, 6]);
            LinearMotor_0 = new C_Vector3(LinearAndAngularData[2, 7], LinearAndAngularData[1, 7], LinearAndAngularData[0, 7]);
            LinearMotor_1 = new C_Vector3(LinearAndAngularData[2, 8], LinearAndAngularData[1, 8], LinearAndAngularData[0, 8]);

            // Do Angular variables
            AngularLimit_0 = new C_Vector3(LinearAndAngularData[5, 0], LinearAndAngularData[4, 0], LinearAndAngularData[3, 0]);
            AngularLimit_1 = new C_Vector3(LinearAndAngularData[5, 1], LinearAndAngularData[4, 1], LinearAndAngularData[3, 1]);
            AngularBreak_0 = new C_Vector3(LinearAndAngularData[5, 2], LinearAndAngularData[4, 2], LinearAndAngularData[3, 2]);
            AngularBreak_1 = new C_Vector3(LinearAndAngularData[5, 3], LinearAndAngularData[4, 3], LinearAndAngularData[3, 3]);
            AngularSpring_0 = new C_Vector3(LinearAndAngularData[5, 4], LinearAndAngularData[4, 4], LinearAndAngularData[3, 4]);
            AngularSpring_1 = new C_Vector3(LinearAndAngularData[5, 5], LinearAndAngularData[4, 5], LinearAndAngularData[3, 5]);
            AngularDamper = new C_Vector3(LinearAndAngularData[5, 6], LinearAndAngularData[4, 6], LinearAndAngularData[3, 6]);
            AngularMotor_0 = new C_Vector3(LinearAndAngularData[5, 7], LinearAndAngularData[4, 7], LinearAndAngularData[3, 7]);
            AngularMotor_1 = new C_Vector3(LinearAndAngularData[5, 8], LinearAndAngularData[4, 8], LinearAndAngularData[3, 8]);

            AngularLimit_0 = ConvertToDegrees(AngularLimit_0);
            AngularLimit_1 = ConvertToDegrees(AngularLimit_1);
            AngularMotor_0 = ConvertToDegrees(AngularMotor_0);
            AngularSpring_0.Divide(57.295776f);
            AngularSpring_1.Divide(0.017453292f);
            LinearMotor_0.Multiply(3.6f);

            // Array 0
            // [0][0] = Linear Limit Z (0) -> radians to degrees
            // [0][1] = Linear Limit Z (1) ?? (probably same as rest)?
            // [0][2] = Linear Break Z (0) ?? Unknown
            // [0][3] = Linear Break Z (1) ?? Unknown
            // [0][4] = Linear Spring Z (0) -> multiply 0.017453292f to get value, when writing divide by 0.017453292f
            // [0][5] = Linear Spring Z (1) -> radians to degrees
            // [0][6] = Linear Damper Z (0) no conversion required
            // [0][7] = Linear Motor Z (0) -> radians to degrees
            // [0][8] = Linear Motor Z (1) no conversion needed

            // Array 1
            // [1][0] = Linear Limit Y (0) -> radians to degrees
            // [1][1] = Linear Limit Y (1) -> radians to degrees
            // [1][2] = Linear Break Y (0) no conversion required
            // [1][3] = Linear Break Y (1) (is this always inverted?)
            // [1][4] = Linear Spring Y (0) -> multiply 0.017453292f to get value, when writing divide by 0.017453292f
            // [1][5] = Linear Spring Y (1) -> radians to degrees
            // [1][6] = Linear Damper Y (0) no conversion required
            // [1][7] = Linear Motor Y (0) -> radians to degrees
            // [1][8] = Linear Motor Y (1) no conversion needed

            // Array 2
            // [2][0] = Linear Limit X (0) -> radians to degrees
            // [2][1] = Linear Limit X (1) ?? (probably same as rest)?
            // [2][2] = Angular Break X (0) ?? Unknown
            // [2][3] = Linear Break X (1) ?? Unknown
            // [2][4] = Linear Spring X (0) -> multiply 0.017453292f to get value, when writing divide by 0.017453292f
            // [2][5] = Linear Spring X (1) -> radians to degrees
            // [2][6] = Linear Damper X (0) no conversion required
            // [2][7] = Linear Motor X (0) -> radians to degrees
            // [2][8] = Linear Motor X (1) no conversion needed

            // Array 3
            // [3][0] = Angular Limit Z (0) -> radians to degrees
            // [3][1] = Angular Limit Z (1) ?? (probably same as rest)?
            // [3][2] = Angular Break Z (0) ?? Unknown
            // [3][3] = Angular Break Z (1) ?? Unknown
            // [3][4] = Angular Spring Z (0) -> multiply 0.017453292f to get value, when writing divide by 0.017453292f
            // [3][5] = Angular Spring Z (1) -> radians to degrees
            // [3][6] = Angular Damper Z (0) no conversion required
            // [3][7] = Angular Motor Z (0) -> radians to degrees
            // [3][8] = Angular Motor Z (1) no conversion needed

            // Array 4
            // [4][0] = Angular Limit Y (0) -> radians to degrees
            // [4][1] = Angular Limit Y (1) -> radians to degrees
            // [4][2] = Angular Break Y (0) no conversion required
            // [4][3] = Angular Break Y (1) (is this always inverted?)
            // [4][4] = Angular Spring Y (0) -> multiply 0.017453292f to get value, when writing divide by 0.017453292f
            // [4][5] = Angular Spring Y (1) -> radians to degrees
            // [4][6] = Angular Damper Y (0) no conversion required
            // [5][7] = Angular Motor Y (0) -> radians to degrees
            // [5][8] = Angular Motor Y (1) no conversion needed

            // Array 5
            // [5][0] = Angular Limit X (0) -> radians to degrees
            // [5][1] = Angular Limit X (1) ?? (probably same as rest)?
            // [5][2] = Angular Break X (0) ?? Unknown
            // [5][3] = Angular Break X (1) ?? Unknown
            // [5][4] = Angular Spring X (0) -> multiply 0.017453292f to get value, when writing divide by 0.017453292f
            // [5][5] = Angular Spring X (1) -> radians to degrees
            // [5][6] = Angular Damper X (0) no conversion required
            // [5][7] = Angular Motor X (0) -> radians to degrees
            // [5][8] = Angular Motor X (1) no conversion needed

            // fixed size
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt32((uint)Flags);
            MemStream.WriteSingle(EnergyDeform * 0.0099999998f);

            // Convert back to format we save in (Copying is a must here)
            C_Vector3 SaveableAngularLimit_0 = ConvertToRadians(AngularLimit_0);
            C_Vector3 SaveableAngularLimit_1 = ConvertToRadians(AngularLimit_1);
            C_Vector3 SaveableAngularMotor_0 = ConvertToRadians(AngularMotor_0);

            C_Vector3 SaveableAngularSpring_0 = AngularSpring_0.Clone();
            SaveableAngularSpring_0.Multiply(57.295776f);

            C_Vector3 SaveableAngularSpring_1 = AngularSpring_1.Clone();
            SaveableAngularSpring_1.Multiply(0.017453292f);

            C_Vector3 SaveableLinearMotor_0 = LinearMotor_0.Clone();
            SaveableLinearMotor_0.Divide(3.6f);

            // Create float array
            // Do linear
            float[,] LinearAndAngularData = new float[6, 9];
            LinearAndAngularData[2, 0] = LinearLimit_0.X; LinearAndAngularData[1, 0] = LinearLimit_0.Y; LinearAndAngularData[0, 0] = LinearLimit_0.Z;
            LinearAndAngularData[2, 1] = LinearLimit_1.X; LinearAndAngularData[1, 1] = LinearLimit_1.Y; LinearAndAngularData[0, 1] = LinearLimit_1.Z;
            LinearAndAngularData[2, 2] = LinearBreak_0.X; LinearAndAngularData[1, 2] = LinearBreak_0.Y; LinearAndAngularData[0, 2] = LinearBreak_0.Z;
            LinearAndAngularData[2, 3] = LinearBreak_1.X; LinearAndAngularData[1, 3] = LinearBreak_1.Y; LinearAndAngularData[0, 3] = LinearBreak_1.Z;
            LinearAndAngularData[2, 4] = LinearSpring_0.X; LinearAndAngularData[1, 4] = LinearSpring_0.Y; LinearAndAngularData[0, 4] = LinearSpring_0.Z;
            LinearAndAngularData[2, 5] = LinearSpring_1.X; LinearAndAngularData[1, 5] = LinearSpring_1.Y; LinearAndAngularData[0, 5] = LinearSpring_1.Z;
            LinearAndAngularData[2, 6] = LinearDamper.X; LinearAndAngularData[1, 6] = LinearDamper.Y; LinearAndAngularData[0, 6] = LinearDamper.Z;
            LinearAndAngularData[2, 7] = SaveableLinearMotor_0.X; LinearAndAngularData[1, 7] = SaveableLinearMotor_0.Y; LinearAndAngularData[0, 7] = SaveableLinearMotor_0.Z;
            LinearAndAngularData[2, 8] = LinearMotor_1.X; LinearAndAngularData[1, 8] = LinearMotor_1.Y; LinearAndAngularData[0, 8] = LinearMotor_1.Z;

            // Do Angular
            LinearAndAngularData[5, 0] = SaveableAngularLimit_0.X; LinearAndAngularData[4, 0] = SaveableAngularLimit_0.Y; LinearAndAngularData[3, 0] = SaveableAngularLimit_0.Z;
            LinearAndAngularData[5, 1] = SaveableAngularLimit_1.X; LinearAndAngularData[4, 1] = SaveableAngularLimit_1.Y; LinearAndAngularData[3, 1] = SaveableAngularLimit_1.Z;
            LinearAndAngularData[5, 2] = AngularBreak_0.X; LinearAndAngularData[4, 2] = AngularBreak_0.Y; LinearAndAngularData[3, 2] = AngularBreak_0.Z;
            LinearAndAngularData[5, 3] = AngularBreak_1.X; LinearAndAngularData[4, 3] = AngularBreak_1.Y; LinearAndAngularData[3, 3] = AngularBreak_1.Z;
            LinearAndAngularData[5, 4] = SaveableAngularSpring_0.X; LinearAndAngularData[4, 4] = SaveableAngularSpring_0.Y; LinearAndAngularData[3, 4] = SaveableAngularSpring_0.Z;
            LinearAndAngularData[5, 5] = SaveableAngularSpring_1.X; LinearAndAngularData[4, 5] = SaveableAngularSpring_1.Y; LinearAndAngularData[3, 5] = SaveableAngularSpring_1.Z;
            LinearAndAngularData[5, 6] = AngularDamper.X; LinearAndAngularData[4, 6] = AngularDamper.Y; LinearAndAngularData[3, 6] = AngularDamper.Z;
            LinearAndAngularData[5, 7] = SaveableAngularMotor_0.X; LinearAndAngularData[4, 7] = SaveableAngularMotor_0.Y; LinearAndAngularData[3, 7] = SaveableAngularMotor_0.Z;
            LinearAndAngularData[5, 8] = AngularMotor_1.X; LinearAndAngularData[4, 8] = AngularMotor_1.Y; LinearAndAngularData[3, 8] = AngularMotor_1.Z;

            // And then finally write the new values.
            for (int i = 0; i < 6; i++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (OriginalFloats != null && OriginalFloats.Length == LinearAndAngularData.Length)
                    {
                        float Original = OriginalFloats[i, x];
                        float Final = LinearAndAngularData[i, x];
                        if (Math.Abs(Original - Final) > 0.01f)
                        {
                            int z = 0;
                        }
                    }

                    MemStream.WriteSingle(LinearAndAngularData[i, x]);
                }
            }
        }

        private C_Vector3 ConvertToDegrees(C_Vector3 InVector)
        {
            C_Vector3 NewVector = new C_Vector3();
            NewVector.X = MathHelper.ToDegrees(InVector.X);
            NewVector.Y = MathHelper.ToDegrees(InVector.Y);
            NewVector.Z = MathHelper.ToDegrees(InVector.Z);

            return NewVector;
        }

        private C_Vector3 ConvertToRadians(C_Vector3 InVector)
        {
            C_Vector3 NewVector = new C_Vector3();
            NewVector.X = MathHelper.ToRadians(InVector.X);
            NewVector.Y = MathHelper.ToRadians(InVector.Y);
            NewVector.Z = MathHelper.ToRadians(InVector.Z);

            return NewVector;
        }
    }
}
