using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectLight : FrameObjectJoint
    {
        int flags;
        float[] unkFloat1 = new float[7];
        int unk_int;
        float[] unkFloat2 = new float[5];
        byte unk_byte1;
        float[] unkFloat3 = new float[17];
        byte unk_byte2;
        float[] unkFloat4 = new float[5];
        Hash nameLight;
        int unk_int2;
        float[] unkFloat5 = new float[20];
        Hash[] names = new Hash[4];
        Vector3 unkVector1;
        Vector3 unkVector2;
        byte unk_byte3;
        Vector3 unkVector3;
        Vector3 unkVector4;
        Vector3 unkVector5;
        Vector3 unkVector6;

        public int Flags {
            get { return flags; }
            set { flags = value; }
        }
        public float[] UnkFloats1 {
            get { return unkFloat1; }
            set { unkFloat1 = value; }
        }
        public int UnkInt1 {
            get { return unk_int; }
            set { unk_int = value; }
        }
        public float[] UnkFloats2 {
            get { return unkFloat2; }
            set { unkFloat2 = value; }
        }
        public byte UnkByte1 {
            get { return unk_byte1; }
            set { unk_byte1 = value; }
        }
        public float[] UnkFloats3 {
            get { return unkFloat3; }
            set { unkFloat3 = value; }
        }
        public byte UnkByte2 {
            get { return unk_byte2; }
            set { unk_byte2 = value; }
        }
        public float[] UnkFloats4 {
            get { return unkFloat4; }
            set { unkFloat4 = value; }
        }
        public Hash NameLight {
            get { return nameLight; }
            set { nameLight = value; }
        }
        public int UnkInt2 {
            get { return unk_int2; }
            set { unk_int2 = value; }
        }
        public float[] UnkFloats5 {
            get { return unkFloat5; }
            set { unkFloat5 = value; }
        }
        public Hash[] UnkHashes {
            get { return names; }
            set { names = value; }
        }
        public Vector3 OnkVector1 {
            get { return unkVector1; }
            set { unkVector1 = value; }
        }
        public Vector3 OnkVector2 {
            get { return unkVector2; }
            set { unkVector2 = value; }
        }
        public byte UnkByte3 {
            get { return unk_byte3; }
            set { unk_byte3 = value; }
        }
        public Vector3 UnkVector3 {
            get { return unkVector3; }
            set { unkVector3 = value; }
        }
        public Vector3 UnkVector4 {
            get { return unkVector4; }
            set { unkVector4 = value; }
        }
        public Vector3 UnkVector5 {
            get { return unkVector5; }
            set { unkVector5 = value; }
        }
        public Vector3 UnkVector6 {
            get { return unkVector6; }
            set { unkVector6 = value; }
        }

        public FrameObjectLight() : base()
        {
            flags = 0;
            unkFloat1 = new float[7];
            unk_int = 0;
            unkFloat2 = new float[5];
            unk_byte1 = 0;
            unkFloat3 = new float[17];
            unk_byte2 = 0;
            unkFloat4 = new float[5];
            nameLight = new Hash();
            unk_int2 = 0;
            unkFloat5 = new float[20];
            names = new Hash[4];

            for (int i = 0; i != 4; i++)
                names[i] = new Hash();

            unkVector1 = new Vector3(0);
            unkVector2 = new Vector3(0);
            unk_byte3 = 0;
            unkVector3 = new Vector3(0);
            unkVector4 = new Vector3(0);
            unkVector5 = new Vector3(0);
            unkVector6 = new Vector3(0);
        }

        public FrameObjectLight(FrameObjectLight other) : base(other)
        {
            flags = other.flags;
            unkFloat1 = other.unkFloat1;
            unk_int = other.unk_int;
            unkFloat2 = other.unkFloat1;
            unk_byte1 = other.unk_byte1;
            unkFloat3 = other.unkFloat1;
            unk_byte2 = other.unk_byte2;
            unkFloat4 = other.unkFloat4;
            nameLight = new Hash(other.name.String);
            unk_int2 = other.unk_int2;
            unkFloat5 = other.unkFloat5;

            names = new Hash[4];
            for (int i = 0; i != 4; i++)
                names[i] = new Hash(names[i].String);

            unkVector1 = other.unkVector1;
            unkVector2 = other.unkVector2;
            unk_byte3 = other.unk_byte3;
            unkVector3 = other.unkVector3;
            unkVector4 = other.unkVector4;
            unkVector5 = other.unkVector5;
            unkVector6 = other.unkVector6;
        }

        public FrameObjectLight(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            flags = reader.ReadInt32(isBigEndian);

            for (int i = 0; i < 7; i++)
                unkFloat1[i] = reader.ReadSingle(isBigEndian);

            unk_int = reader.ReadInt32(isBigEndian);

            for (int i = 0; i < 5; i++)
                unkFloat2[i] = reader.ReadSingle(isBigEndian);

            unk_byte1 = reader.ReadByte8();

            for (int i = 0; i < 17; i++)
                unkFloat3[i] = reader.ReadSingle(isBigEndian);

            unk_byte2 = reader.ReadByte8();

            for (int i = 0; i < 5; i++)
                unkFloat4[i] = reader.ReadSingle(isBigEndian);

            nameLight = new Hash(reader, isBigEndian);

            unk_int2 = reader.ReadInt32(isBigEndian);

            for (int i = 0; i < 20; i++)
                unkFloat5[i] = reader.ReadSingle(isBigEndian);

            for (int i = 0; i < 4; i++)
                names[i] = new Hash(reader, isBigEndian);

            unkVector1 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            unkVector2 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            unk_byte3 = reader.ReadByte8();
            unkVector3 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            unkVector4 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            unkVector5 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            unkVector6 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
        }
        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(flags);

            for (int i = 0; i < 7; i++)
                writer.Write(unkFloat1[i]);

            writer.Write(unk_int);

            for (int i = 0; i < 5; i++)
                writer.Write(unkFloat2[i]);

            writer.Write(unk_byte1);

            for (int i = 0; i < 17; i++)
                writer.Write(unkFloat3[i]);

            writer.Write(unk_byte2);

            for (int i = 0; i < 5; i++)
                writer.Write(unkFloat4[i]);

            nameLight.WriteToFile(writer);

            writer.Write(unk_int2);

            for (int i = 0; i != 20; i++)
                writer.Write(unkFloat5[i]);

            for (int i = 0; i != 4; i++)
                names[i].WriteToFile(writer);

            unkVector1.WriteToFile(writer);
            unkVector2.WriteToFile(writer);
            writer.Write(unk_byte3);
            unkVector3.WriteToFile(writer);
            unkVector4.WriteToFile(writer);
            unkVector5.WriteToFile(writer);
            unkVector6.WriteToFile(writer);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
