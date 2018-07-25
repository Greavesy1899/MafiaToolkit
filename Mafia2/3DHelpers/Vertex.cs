using System;
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

        public Vertex()
        {
            position = new Vector3(0);
            normal = new Vector3(0);
            tangent = new Vector3(0);

        }

        public void ReadPositionData(byte[] data, int i, float factor, Vector3 offset)
        {
            ushort uint16_1 = BitConverter.ToUInt16(data, i);
            ushort uint16_2 = BitConverter.ToUInt16(data, i + 2);
            ushort num3 = (ushort)(BitConverter.ToUInt16(data, i + 4) & short.MaxValue);
            position = new Vector3(uint16_1 * factor, uint16_2 * factor, num3 * factor);
            position += offset;
            
        }

        public void ReadTangentData(byte[] data, int i)
        {
            float x = (data[i] - sbyte.MaxValue) * 0.007874f;
            float y = (data[i + 1] - sbyte.MaxValue) * 0.007874f;
            float z = (data[i + 5] - sbyte.MaxValue) * 0.007874f;
            tangent = new Vector3(x, y, z);
            tangent.Normalize();
        }

        public void ReadNormalData(byte[] data, int i)
        {
            float x = (data[i] - 127.0f) * 0.007874f;
            float y = (data[i + 1] - 127.0f) * 0.007874f;
            float z = (data[i + 2] - 127.0f) * 0.007874f;
            normal = new Vector3(x, y, z);
            normal.Normalize();
        }

        public override string ToString()
        {
            return string.Format(position.ToString());
        }
    }
}
