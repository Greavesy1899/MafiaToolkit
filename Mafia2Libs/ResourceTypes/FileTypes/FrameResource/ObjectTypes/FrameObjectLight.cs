using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectLight : FrameObjectJoint
    {
        [Category("Lights")]
        public int Flags { get; set; }
        [Category("Lights")]
        public float LUnk0 { get; set; }
        [Category("Lights")]
        public float LUnk1 { get; set; }
        [Category("Lights")]
        public float LUnk2 { get; set; }
        [Category("Lights")]
        public float LUnk3 { get; set; }
        [Category("Lights")]
        public float LUnk4 { get; set; }
        [Category("Lights")]
        public float LUnk5 { get; set; }
        [Category("Lights")]
        public float LUnk6 { get; set; }
        [Category("Lights")]
        public int UnkInt1 { get; set; }
        [Category("Lights")]
        public Vector3 UnkVector_0 { get; set; }
        [Category("Lights")]
        public float LUnk7 { get; set; }
        [Category("Lights")]
        public float LUnk8 { get; set; }
        [Category("Lights")]
        public byte UnkByte1 { get; set; }
        [Category("Lights")]
        public float LUnk9 { get; set; }
        [Category("Lights")]
        public float LUnk10 { get; set; }
        [Category("Lights")]
        public float LUnk11 { get; set; }
        [Category("Lights")]
        public float LUnk12 { get; set; }
        [Category("Lights")]
        public Vector3 UnkVector_1 { get; set; }
        [Category("Lights")]
        public Vector3 UnkVector_2 { get; set; }
        [Category("Lights")]
        public float LUnk13 { get; set; }
        [Category("Lights")]
        public float LUnk14 { get; set; }
        [Category("Lights")]
        public float LUnk15 { get; set; }
        [Category("Lights")]
        public Vector3 UnkVector_3 { get; set; }
        [Category("Lights")]
        public float LUnk16 { get; set; }
        [Category("Lights")]
        public float LUnk17 { get; set; }
        [Category("Lights")]
        public float LUnk18 { get; set; }
        [Category("Lights")]
        public byte UnkByte2 { get; set; }
        [Category("Lights")]
        public float LUnk19 { get; set; }
        [Category("Lights")]
        public float LUnk20 { get; set; }
        [Category("Lights")]
        public float LUnk21 { get; set; }
        [Category("Lights")]
        public float LUnk22 { get; set; }
        [Category("Lights")]
        public float LUnk23 { get; set; }
        [Category("Lights")]
        public HashName ProjectionTexture { get; set; }
        [Category("Lights")]
        public int UnkInt2 { get; set; }
        [Category("Lights")]
        public float LUnk24 { get; set; }
        [Category("Lights")]
        public float LUnk25 { get; set; }
        [Category("Lights")]
        public Vector3 UnkVector_4 { get; set; }
        [Category("Lights")]
        public float LUnk26 { get; set; }
        [Category("Lights")]
        public float LUnk27 { get; set; }
        [Category("Lights")]
        public float LUnk28 { get; set; }
        [Category("Lights")]
        public float LUnk29 { get; set; }
        [Category("Lights")]
        public float LUnk30 { get; set; }
        [Category("Lights")]
        public Vector3 UnkVector_5 { get; set; }
        [Category("Lights")]
        public float LUnk31 { get; set; }
        [Category("Lights")]
        public float LUnk32 { get; set; }
        [Category("Lights")]
        public float LUnk33 { get; set; }
        [Category("Lights")]
        public float LUnk34 { get; set; }
        [Category("Lights")]
        public float LUnk35 { get; set; }
        [Category("Lights")]
        public HashName[] TextureHashes { get; set; }
        [Category("Lights")]
        public BoundingBox UnkBox { get; set; }
        [Category("Lights")]
        public byte UnkByte3 { get; set; }
        [Category("Lights")]
        public Matrix4x4 UnknownMatrix { get; set; }

        public FrameObjectLight(FrameResource OwningResource) : base(OwningResource)
        {
            Flags = 0;
            UnkVector_0 = new Vector3();
            UnkVector_1 = new Vector3();
            UnkVector_2 = new Vector3();
            UnkVector_3 = new Vector3();
            UnkVector_4 = new Vector3();
            UnkVector_5 = new Vector3();
            TextureHashes = new HashName[4];
            for (int i = 0; i < 4; i++)
            {
                TextureHashes[i] = new HashName();
            }

            UnkBox = new BoundingBox(new Vector3(float.MinValue), new Vector3(float.MaxValue));
            UnkByte3 = 0;
            UnknownMatrix = Matrix4x4.Identity;
        }

        public FrameObjectLight(FrameObjectLight other) : base(other)
        {
            Flags = other.Flags;
            LUnk1 = other.LUnk1;
            LUnk2 = other.LUnk2;
            LUnk3 = other.LUnk3;
            LUnk4 = other.LUnk4;
            LUnk5 = other.LUnk5;
            LUnk6 = other.LUnk6;
            UnkInt1 = other.UnkInt1;
            UnkVector_0 = other.UnkVector_0;
            LUnk7 = other.LUnk7;
            LUnk8 = other.LUnk8;
            UnkByte1 = other.UnkByte1;
            LUnk9 = other.LUnk9;
            LUnk10 = other.LUnk10;
            LUnk11 = other.LUnk11;
            LUnk12 = other.LUnk12;
            UnkVector_1 = other.UnkVector_1;
            UnkVector_2 = other.UnkVector_2;
            LUnk13 = other.LUnk13;
            LUnk14 = other.LUnk14;
            LUnk15 = other.LUnk15;
            UnkVector_3 = other.UnkVector_3;
            LUnk16 = other.LUnk16;
            LUnk17 = other.LUnk17;
            LUnk18 = other.LUnk18;
            UnkByte2 = other.UnkByte2;
            LUnk19 = other.LUnk19;
            LUnk20 = other.LUnk20;
            LUnk21 = other.LUnk21;
            LUnk22 = other.LUnk22;
            LUnk23 = other.LUnk23;
            ProjectionTexture = new HashName(other.ProjectionTexture);
            UnkInt2 = other.UnkInt2;
            LUnk24 = other.LUnk24;
            LUnk25 = other.LUnk25;
            UnkVector_4 = other.UnkVector_4;
            LUnk26 = other.LUnk26;
            LUnk27 = other.LUnk27;
            LUnk28 = other.LUnk28;
            LUnk29 = other.LUnk29;
            LUnk30 = other.LUnk30;
            UnkVector_5 = other.UnkVector_5;
            LUnk31 = other.LUnk31;
            LUnk32 = other.LUnk32;
            LUnk33 = other.LUnk33;
            LUnk34 = other.LUnk34;
            LUnk35 = other.LUnk35;

            TextureHashes = new HashName[4];
            for (int i = 0; i < 4; i++)
            {
                TextureHashes[i] = new HashName(other.TextureHashes[i].String);
            }

            UnkBox = other.UnkBox;
            UnkByte3 = other.UnkByte3;
            UnknownMatrix = MatrixUtils.CopyFrom(other.UnknownMatrix);
        }

        public override void ReadFromFile(MemoryStream reader, bool bIsBigEndian)
        {
            base.ReadFromFile(reader, bIsBigEndian);

            Flags = reader.ReadInt32(bIsBigEndian);
            LUnk0 = reader.ReadSingle(bIsBigEndian);
            LUnk1 = reader.ReadSingle(bIsBigEndian);
            LUnk2 = reader.ReadSingle(bIsBigEndian);
            LUnk3 = reader.ReadSingle(bIsBigEndian);
            LUnk4 = reader.ReadSingle(bIsBigEndian);
            LUnk5 = reader.ReadSingle(bIsBigEndian);
            LUnk6 = reader.ReadSingle(bIsBigEndian);
            UnkInt1 = reader.ReadInt32(bIsBigEndian);
            UnkVector_0 = Vector3Utils.ReadFromFile(reader, bIsBigEndian);
            LUnk7 = reader.ReadSingle(bIsBigEndian);
            LUnk8 = reader.ReadSingle(bIsBigEndian);
            UnkByte1 = reader.ReadByte8();
            LUnk9 = reader.ReadSingle(bIsBigEndian);
            LUnk10 = reader.ReadSingle(bIsBigEndian);
            LUnk11 = reader.ReadSingle(bIsBigEndian);
            LUnk12 = reader.ReadSingle(bIsBigEndian);
            UnkVector_1 = Vector3Utils.ReadFromFile(reader, bIsBigEndian);
            UnkVector_2 = Vector3Utils.ReadFromFile(reader, bIsBigEndian);
            LUnk13 = reader.ReadSingle(bIsBigEndian);
            UnkVector_3 = Vector3Utils.ReadFromFile(reader, bIsBigEndian);
            LUnk14 = reader.ReadSingle(bIsBigEndian);
            LUnk15 = reader.ReadSingle(bIsBigEndian);
            LUnk16 = reader.ReadSingle(bIsBigEndian);
            UnkByte2 = reader.ReadByte8();
            LUnk17 = reader.ReadSingle(bIsBigEndian);
            LUnk18 = reader.ReadSingle(bIsBigEndian);
            LUnk19 = reader.ReadSingle(bIsBigEndian);
            LUnk20 = reader.ReadSingle(bIsBigEndian);
            LUnk21 = reader.ReadSingle(bIsBigEndian);
            ProjectionTexture = new HashName(reader, bIsBigEndian);
            UnkInt2 = reader.ReadInt32(bIsBigEndian);
            LUnk22 = reader.ReadSingle(bIsBigEndian);
            LUnk23 = reader.ReadSingle(bIsBigEndian);
            UnkVector_4 = Vector3Utils.ReadFromFile(reader, bIsBigEndian);
            LUnk24 = reader.ReadSingle(bIsBigEndian);
            LUnk25 = reader.ReadSingle(bIsBigEndian);
            LUnk26 = reader.ReadSingle(bIsBigEndian);
            LUnk27 = reader.ReadSingle(bIsBigEndian);
            LUnk28 = reader.ReadSingle(bIsBigEndian);
            UnkVector_5 = Vector3Utils.ReadFromFile(reader, bIsBigEndian);
            LUnk29 = reader.ReadSingle(bIsBigEndian);
            LUnk30 = reader.ReadSingle(bIsBigEndian);
            LUnk31 = reader.ReadSingle(bIsBigEndian);
            LUnk32 = reader.ReadSingle(bIsBigEndian);
            LUnk33 = reader.ReadSingle(bIsBigEndian);
            LUnk34 = reader.ReadSingle(bIsBigEndian);
            LUnk35 = reader.ReadSingle(bIsBigEndian);

            // Read Textures
            for (int i = 0; i < 4; i++)
            {
                TextureHashes[i] = new HashName(reader, bIsBigEndian);
            }

            UnkBox = BoundingBoxExtenders.ReadFromFile(reader, bIsBigEndian);
            UnkByte3 = reader.ReadByte8();
            UnknownMatrix = MatrixUtils.ReadFromFile(reader, bIsBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);

            writer.Write(Flags);
            writer.Write(LUnk0);
            writer.Write(LUnk1);
            writer.Write(LUnk2);
            writer.Write(LUnk3);
            writer.Write(LUnk4);
            writer.Write(LUnk5);
            writer.Write(LUnk6);
            writer.Write(UnkInt1);
            UnkVector_0.WriteToFile(writer);
            writer.Write(LUnk7);
            writer.Write(LUnk8);
            writer.Write(UnkByte1);
            writer.Write(LUnk9);
            writer.Write(LUnk10);
            writer.Write(LUnk11);
            writer.Write(LUnk12);
            UnkVector_1.WriteToFile(writer);
            UnkVector_2.WriteToFile(writer);
            writer.Write(LUnk13);
            UnkVector_3.WriteToFile(writer);
            writer.Write(LUnk14);
            writer.Write(LUnk15);
            writer.Write(LUnk16);
            writer.Write(UnkByte2);
            writer.Write(LUnk17);
            writer.Write(LUnk18);
            writer.Write(LUnk19);
            writer.Write(LUnk20);
            writer.Write(LUnk21);
            ProjectionTexture.WriteToFile(writer);
            writer.Write(UnkInt2);
            writer.Write(LUnk22);
            writer.Write(LUnk23);
            UnkVector_4.WriteToFile(writer);
            writer.Write(LUnk24);
            writer.Write(LUnk25);
            writer.Write(LUnk26);
            writer.Write(LUnk27);
            writer.Write(LUnk28);
            UnkVector_5.WriteToFile(writer);
            writer.Write(LUnk29);
            writer.Write(LUnk30);
            writer.Write(LUnk31);
            writer.Write(LUnk32);
            writer.Write(LUnk33);
            writer.Write(LUnk34);
            writer.Write(LUnk35);

            foreach (HashName Value in TextureHashes)
            {
                Value.WriteToFile(writer);
            }

            UnkBox.WriteToFile(writer);
            writer.Write(UnkByte3);
            UnknownMatrix.WriteToFile(writer);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
