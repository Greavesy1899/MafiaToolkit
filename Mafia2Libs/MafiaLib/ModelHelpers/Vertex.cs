using System;
using SharpDX;
using Utils.SharpDXExtensions;

namespace Utils.Models
{
    /*
    0 - position X (0)
    1 - position X (1)
    2 - position Y (0)
    3 - position Y (1)
    4 - position Z (0)
    5 - position Z (1)
    6 - position W (0)
    7 - position W (1)
    8 - Normal X (0)
    9 - Normal Y (0)
    10 - Normal Z (0)
    11 - Normal W (0)
    */

    public class Vertex
    {
        Vector3 position;
        Vector3 normal;
        Vector3 tangent;
        Vector3 binormal;
        float[] boneWeights;
        byte[] boneIDs;
        Half2[] uvs;
        int damageGroup;
        byte[] color0;
        byte[] color1;
        Vector3 bbCoeffs;

        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }
        public Vector3 Normal {
            get { return normal; }
            set { normal = value; }
        }
        public Vector3 Tangent {
            get { return tangent; }
            set { tangent = value; }
        }
        public Vector3 Binormal {
            get { return binormal; }
            set { binormal = value; }
        }
        public Half2[] UVs {
            get { return uvs; }
            set { uvs = value; }
        }
        public float[] BoneWeights {
            get { return boneWeights; }
            set { boneWeights = value; }
        }
        public byte[] BoneIDs {
            get { return boneIDs; }
            set { boneIDs = value; }
        }
        public int DamageGroup {
            get { return damageGroup; }
            set { damageGroup = value; }
        }
        public byte[] Color0 {
            get { return color0; }
            set { color0 = value; }
        }
        public byte[] Color1 {
            get { return color1; }
            set { color1 = value; }
        }
        public Vector3 BBCoeffs {
            get { return bbCoeffs; }
            set { bbCoeffs = value; }
        }

        public Vertex()
        {
            position = new Vector3(0);
            normal = new Vector3(0);
            tangent = new Vector3(0);
            color0 = new byte[4];
            color1 = new byte[4];
            boneWeights = new float[4];
            boneIDs = new byte[4];
            uvs = new Half2[4];

            for (int i = 0; i != uvs.Length; i++)
            {
                uvs[i] = new Half2();
            }
        }

        public void WritePositionData(byte[] data, int i, float factor, Vector3 offset)
        {
            float tempBinormal = 0.0f;
            position -= offset;
            position /= factor;
            byte[] tempPosData;

            //Do X
            tempPosData = BitConverter.GetBytes(Convert.ToUInt16(position.X));
            Array.Copy(tempPosData, 0, data, i, 2);

            //Do Y
            tempPosData = BitConverter.GetBytes(Convert.ToUInt16(position.Y));
            Array.Copy(tempPosData, 0, data, i + 2, 2);

            //Do Z
            // We have to make space for the binormal data (this is stored in the highest bit).
            ushort z = Convert.ToUInt16(position.Z);
            z &= 0x7FFF;
            if (tempBinormal < 0.0f)
            {
                z |= 0x8000;
            }

            tempPosData = BitConverter.GetBytes(z);
            Array.Copy(tempPosData, 0, data, i + 4, 2);

            data[i + 6] = 0x0;
            data[i + 7] = 0x0;
        }

        public void WriteTangentData(byte[] data, int i)
        {
            byte tempByte = 0;
            float tempTangent = 0f;

            //X..
            tempTangent = Tangent.X * 127.0f + 127.0f;
            tempByte = !float.IsNaN(tempTangent) ? Convert.ToByte(tempTangent) : (byte)127;
            data[i + 6] = tempByte;

            //Y..
            tempTangent = Tangent.Y * 127.0f + 127.0f;
            tempByte = !float.IsNaN(tempTangent) ? Convert.ToByte(tempTangent) : (byte)127;
            data[i + 7] = tempByte;

            //Z..
            tempTangent = Tangent.Z * 127.0f + 127.0f;
            tempByte = !float.IsNaN(tempTangent) ? Convert.ToByte(tempTangent) : (byte)255;
            data[i + 11] = tempByte;
        }

        public void WriteNormalData(byte[] data, int i)
        {
            byte tempByte = 0;
            float tempNormal = 0f;

            //X..
            tempNormal = Normal.X * 127.0f + 127.0f;
            tempByte = !float.IsNaN(tempNormal) ? Convert.ToByte(tempNormal) : (byte)127;
            data[i] = tempByte;

            //Y..
            tempNormal = Normal.Y * 127.0f + 127.0f;
            tempByte = !float.IsNaN(tempNormal) ? Convert.ToByte(tempNormal) : (byte)127;
            data[i + 1] = tempByte;

            //Z..
            tempNormal = Normal.Z * 127.0f + 127.0f;
            tempByte = !float.IsNaN(tempNormal) ? Convert.ToByte(tempNormal) : (byte)255;
            data[i + 2] = tempByte;
        }

        public void WriteUvData(byte[] data, int i, int uvNum)
        {
            //Do X
            byte[] tempPosData = HalfExtenders.GetBytes(UVs[uvNum].X);
            Array.Copy(tempPosData, 0, data, i, 2);

            //Do Y
            UVs[uvNum].Y = -uvs[uvNum].Y;
            tempPosData = HalfExtenders.GetBytes(UVs[uvNum].Y);
            Array.Copy(tempPosData, 0, data, i + 2, 2);
        }

        public void WriteColourData(byte[] data, int i, int colourIndex)
        {
            Array.Copy((colourIndex == 0 ? color0 : color1), 0, data, i, 4);
        }

        public void WriteDamageGroup(byte[] data, int i)
        {
            byte[] tempDamageIDData = BitConverter.GetBytes(damageGroup);
            Array.Copy(tempDamageIDData, 0, data, i, 4);
        }

        public override string ToString()
        {
            return string.Format(position.ToString());
        }
    }
}
