using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mafia2
{
    public class Vertex
    {
        Vector3 position;
        Vector3 normal;
        Vector3 tangent;
        Vector3 binormal;

        float blendWeight;
        int boneID;
        UVVector2[] uvs;

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
        public UVVector2[] UVs {
            get { return uvs; }
            set { uvs = value; }
        }
        public float BlendWeight {
            get { return blendWeight; }
            set { blendWeight = value; }
        }
        public int BoneID {
            get { return boneID; }
            set { boneID = value; }
        }

        /// <summary>
        /// Construct empty vertex.
        /// </summary>
        public Vertex()
        {
            position = new Vector3(0);
            normal = new Vector3(0);
            tangent = new Vector3(0);
        }

        /// <summary>
        /// Read position data using buffer data, datapos, decompFactor and the decompOffset
        /// </summary>
        /// <param name="data">vertex buffer data</param>
        /// <param name="i">current position to read from</param>
        /// <param name="factor">Decompression Factor</param>
        /// <param name="offset">Decompression Offset</param>
        public void ReadPositionData(byte[] data, int i, float factor, Vector3 offset)
        {
            ushort uint16_1 = BitConverter.ToUInt16(data, i);
            ushort uint16_2 = BitConverter.ToUInt16(data, i + 2);
            ushort num3 = (ushort)(BitConverter.ToUInt16(data, i + 4) & short.MaxValue);
            position = new Vector3(uint16_1 * factor, uint16_2 * factor, num3 * factor);
            position += offset;
            
        }

        /// <summary>
        /// Write position data into buffer. Uses Decompression factor, and offset.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public byte[] WritePositionData(float factor, Vector3 offset)
        {
            List<byte> posData = new List<byte>();

            position -= offset;
            position /= factor;

            posData.AddRange(BitConverter.GetBytes(Convert.ToUInt16(position.X)));
            posData.AddRange(BitConverter.GetBytes(Convert.ToUInt16(position.Y)));
            posData.AddRange(BitConverter.GetBytes(Convert.ToUInt16(position.Z)));

            return posData.ToArray();
        }

        /// <summary>
        /// Read tangent data from buffer data, datapos.
        /// </summary>
        /// <param name="data">vertex buffer data</param>
        /// <param name="i">current position to read from</param>
        public void ReadTangentData(byte[] data, int i)
        {
            float x = (data[i] - sbyte.MaxValue) * 0.007874f;
            float y = (data[i + 1] - sbyte.MaxValue) * 0.007874f;
            float z = (data[i + 5] - sbyte.MaxValue) * 0.007874f;
            tangent = new Vector3(x, y, z);
            tangent.Normalize();
        }

        /// <summary>
        /// Write tangent data from buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] WriteTangentData()
        {
            List<byte> tanData = new List<byte>();

            //X..
            if (float.IsNaN(Tangent.X * 127.0f + 127.0f))
                tanData.Add(0);
            else
                tanData.Add(Convert.ToByte(Tangent.X * 127.0f + 127.0f));

            //Y..
            if (float.IsNaN(Tangent.Y * 127.0f + 127.0f))
                tanData.Add(0);
            else
                tanData.Add(Convert.ToByte(Tangent.Y * 127.0f + 127.0f));

            //Z is done in normal data.

            return tanData.ToArray();
        }

        /// <summary>
        /// Read normal data from buffer data, datapos.
        /// </summary>
        /// <param name="data">vertex buffer data</param>
        /// <param name="i">current position to read from</param>
        public void ReadNormalData(byte[] data, int i)
        {
            float x = (data[i] - 127.0f) * 0.007874f;
            float y = (data[i + 1] - 127.0f) * 0.007874f;
            float z = (data[i + 2] - 127.0f) * 0.007874f;
            normal = new Vector3(x, y, z);
            normal.Normalize();
        }

        /// <summary>
        /// Write tangent data from buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] WriteNormalData(bool hasTangents)
        {
            List<byte> normData = new List<byte>();

            //X..
            if (float.IsNaN(Normal.X * 127.0f + 127.0f))
                normData.Add(0);
            else
                normData.Add(Convert.ToByte(Normal.X * 127.0f + 127.0f));

            //Y..
            if (float.IsNaN(Normal.Y * 127.0f + 127.0f))
                normData.Add(0);
            else
                normData.Add(Convert.ToByte(Normal.Y * 127.0f + 127.0f));

            //Z..
            if (float.IsNaN(Normal.Z * 127.0f + 127.0f))
                normData.Add(0);
            else
                normData.Add(Convert.ToByte(Normal.Z * 127.0f + 127.0f));

            if (hasTangents)
            {
                //Tangent Z..
                if (float.IsNaN(Tangent.Z * 127.0f + 127.0f))
                    normData.Add(0);
                else
                    normData.Add(Convert.ToByte(Tangent.Z * 127.0f + 127.0f));
            }

            return normData.ToArray();
        }

        /// <summary>
        /// This is WIP.
        /// </summary>
        /// <param name="data">vertex buffer data</param>
        /// <param name="i">current position to read from</param>
        public void ReadBlendData(byte[] data, int i)
        {
            //todo; work on skeleton models.
            blendWeight = (BitConverter.ToSingle(data, i) / byte.MaxValue);
            boneID = BitConverter.ToInt32(data, i + 4);
        }

        /// <summary>
        /// Read UV data from buffer data, datapos, and numuvs.
        /// </summary>
        /// <param name="data">vertex buffer data</param>
        /// <param name="i">current position to read from</param>
        /// <param name="uvpos">numuvs</param>
        public void ReadUvData(byte[] data, int i, int uvpos)
        {
            Half x = Half.ToHalf(data, i);
            Half y = Half.ToHalf(data, i + 2);
            uvs[uvpos] = new UVVector2(x, y);
        }

        /// <summary>
        /// Write UV Data to buffer. uvNum is either TexCoord 0, 1, 2, 7
        /// </summary>
        /// <param name="uvNum"></param>
        /// <returns></returns>
        public byte[] WriteUvData(int uvNum)
        {
            List<byte> uvData = new List<byte>();

            byte[] x = Half.GetBytes(UVs[uvNum].X);
            byte[] y = Half.GetBytes(UVs[uvNum].Y);

            uvData.AddRange(x);
            uvData.AddRange(y);
            return uvData.ToArray();
        }

        /// <summary>
        /// Construct binormal data from normal and tangent info.
        /// </summary>
        public void BuildBinormals()
        {
            binormal = normal;
            binormal.CrossProduct(tangent);
            binormal *= 2;
            binormal.Normalize();
        }

        public override string ToString()
        {
            return string.Format(position.ToString());
        }
    }
}
