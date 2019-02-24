using System;
using SharpDX;
namespace Mafia2
{
    public class VertexTranslator
    {
        /*
         * Position.X = 2 Bytes / Half;
         * Position.Y = 2 Bytes / Half;
         * Position.Z = 2 Bytes / Half;
         * Position.W = 2 Bytes / Half; 
         *  Also 
         *      Tangent.X = 1 Bytes (1st Bytes of Position.W)
         *      Tangent.Y = 1 Bytes (2nd Bytes of Position.W)
         * Normal.X = 1 Bytes / Byte.
         * Normal.Y = 1 Bytes / Byte.
         * Normal.Z = 1 Bytes / Byte.
         * Normal.W = 1 Bytes / Bytes.
         * 
         *  Also
         *      Tangent.W = 1 Bytes (1st Bytes of Normal.W)
         * 
         *
         * */
        public static Vector3 ReadPositionDataFromVB(byte[] data, int i, float factor, Vector3 offset)
        {
            Vector3 vec = new Vector3();
            ushort x = BitConverter.ToUInt16(data, i);
            ushort y = BitConverter.ToUInt16(data, i + 2);
            ushort z = (ushort)(BitConverter.ToUInt16(data, i + 4) & short.MaxValue);
            vec = new Vector3(x * factor, y * factor, z * factor);
            vec += offset;
            return vec;
        }

        public static Vector3 ReadTangentDataFromVB(byte[] data, int i)
        {
            Vector3 vec = new Vector3();
            float x = (data[i + 6] - sbyte.MaxValue) * 0.007874f;
            float y = (data[i + 7] - sbyte.MaxValue) * 0.007874f;
            float z = (data[i + 11] - sbyte.MaxValue) * 0.007874f;
            vec = new Vector3(x, y, z);
            vec.Normalize();
            return vec;
        }

        public static Vector3 ReadNormalDataFromVB(byte[] data, int i)
        {
            Vector3 max255 = new Vector3(255);
            SharpDX.Half x = new SharpDX.Half(data[i]);
            SharpDX.Half y = new SharpDX.Half(data[i + 1]);
            SharpDX.Half z = new SharpDX.Half(data[i + 2]);
            Vector3 vec = new Vector3(x, y, z);
            vec /= max255;
            vec *= 2;
            vec -= 1;
            return vec;
        }

        public static Vector2 ReadTexcoordFromVB(byte[] data, int i)
        {
            System.Half x = System.Half.ToHalf(data, i);
            System.Half y = System.Half.ToHalf(data, i + 2);
            y = -y;
            return new Vector2(x, y);
        }

        public static int ReadDamageGroupFromVB(byte[] data, int i)
        {
            return BitConverter.ToInt32(data, i);
        }

        public static Int4 ReadColorFromVB(byte[] data, int i)
        {
            return new Int4(data[i], data[i + 1], data[i + 2], data[i + 3]);
        }

        public static Vector3 ReadBBCoeffsVB(byte[] data, int i)
        {
            Vector3 vec = new Vector3();
            vec.X = BitConverter.ToSingle(data, i);
            vec.Y = BitConverter.ToSingle(data, i + 4);
            vec.Z = BitConverter.ToSingle(data, i + 8);
            return vec;
        }

        public static float ReadBlendWeightFromVB(byte[] data, int i)
        {
            //todo; work on skeleton models.
            return (BitConverter.ToSingle(data, i) / byte.MaxValue);
        }

        public static int ReadBlendIDFromVB(byte[] data, int i)
        {
            //todo; work on skeleton models.
            return BitConverter.ToInt32(data, i + 4);
        }
    }
}
