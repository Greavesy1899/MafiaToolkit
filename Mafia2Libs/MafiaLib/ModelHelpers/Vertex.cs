using System;
using SharpDX;
using Utils.SharpDXExtensions;

namespace Utils.Models
{
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

        /// <summary>
        /// Construct empty vertex.
        /// </summary>
        public Vertex()
        {
            position = new Vector3(0);
            normal = new Vector3(0);
            tangent = new Vector3(0);
            uvs = new Half2[4];

            for (int i = 0; i != uvs.Length; i++)
                uvs[i] = new Half2();
        }

        /// <summary>
        /// Write position data into buffer. Uses Decompression factor, and offset.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public void WritePositionData(byte[] data, int i, float factor, Vector3 offset)
        {
            position -= offset;
            position /= factor;
            byte[] tempPosData;

            //Do X
            tempPosData = BitConverter.GetBytes(Convert.ToUInt16(position.X));
            Array.Copy(tempPosData, 0, data, i, 2);

            //Do Y
            tempPosData = BitConverter.GetBytes(Convert.ToUInt16(position.Y));
            Array.Copy(tempPosData, 0, data, i+2, 2);

            //Do Z
            tempPosData = BitConverter.GetBytes(Convert.ToUInt16(position.Z));
            Array.Copy(tempPosData, 0, data, i+4, 2);

            data[i + 6] = 0x0;
            data[i + 7] = 0x0;
        }

        /// <summary>
        /// Write tangent data from buffer.
        /// </summary>
        /// <returns></returns>
        public void WriteTangentData(byte[] data, int i)
        {
            byte tempByte = 0;
            float tempNormal = 0f;

            //X..
            tempNormal = Tangent.X * 127.0f + 127.0f;
            tempByte = !float.IsNaN(tempNormal) ? Convert.ToByte(tempNormal) : (byte)127;
            data[i + 6] = tempByte;

            //Y..
            tempNormal = Tangent.Y * 127.0f + 127.0f;
            tempByte = !float.IsNaN(tempNormal) ? Convert.ToByte(tempNormal) : (byte)127;
            data[i + 7] = tempByte;

            //Z..
            tempNormal = Tangent.Z * 127.0f + 127.0f;
            tempByte = !float.IsNaN(tempNormal) ? Convert.ToByte(tempNormal) : (byte)255;
            data[i + 11] = tempByte;
        }

        /// <summary>
        /// Write tangent data from buffer.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Write UV Data to buffer. uvNum is either TexCoord 0, 1, 2, 7
        /// </summary>
        /// <param name="uvNum"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Write Damage group to buffer
        /// </summary>
        /// <param name="uvNum"></param>
        /// <returns></returns>
        public void WriteDamageGroup(byte[] data, int i)
        {
            byte[] tempDamageIDData = BitConverter.GetBytes(damageGroup);
            Array.Copy(tempDamageIDData, 0, data, i, 4);
        }

        /// <summary>
        /// Read Damage Group from buffer
        /// </summary>
        /// <param name="data">vertex buffer data</param>
        /// <param name="i">current position to read from</param>
        public void ReadDamageGroup(byte[] data, int i)
        {
            //todo; work on skeleton models.
            damageGroup = BitConverter.ToInt32(data, i);
        }

        /// <summary>
        /// Construct binormal data from normal and tangent info.
        /// </summary>
        public void BuildBinormals()
        {
            binormal = normal;           
            binormal = Vector3.Cross(binormal, tangent);
            binormal *= 2;
            binormal.Normalize();
        }

        public override string ToString()
        {
            return string.Format(position.ToString());
        }
    }
}
