using System;
using System.Collections.Generic;
using System.Numerics;
using Utils.VorticeUtils;

namespace Utils.Models
{
    public class VertexTranslator
    {
        private static bool isBigEndian;

        public static bool IsBigEndian {
            get { return isBigEndian; }
            set { isBigEndian = value; }
        }

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
        public static Vertex DecompressVertex(byte[] data, VertexFlags declaration, Vector3 offset, float scale, Dictionary<VertexFlags, ResourceTypes.FrameResource.FrameLOD.VertexOffset> offsets)
        {
            Vertex vertex = new Vertex();

            if (declaration.HasFlag(VertexFlags.Position))
            {
                int startIndex = offsets[VertexFlags.Position].Offset;
                var output = ReadPositionDataFromVB(data, startIndex, scale, offset);
                vertex.Position = Vector3Utils.FromVector4(output);
                vertex.Binormal = new Vector3(output.W);
            }

            if (declaration.HasFlag(VertexFlags.Tangent))
            {
                int startIndex = offsets[VertexFlags.Position].Offset;
                vertex.Tangent = ReadTangentDataFromVB(data, startIndex);
            }

            if (declaration.HasFlag(VertexFlags.Normals))
            {
                int startIndex = offsets[VertexFlags.Normals].Offset;
                vertex.Normal = ReadNormalDataFromVB(data, startIndex);
            }

            if (declaration.HasFlag(VertexFlags.Skin))
            {
                int startIndex = offsets[VertexFlags.Skin].Offset;
                vertex.BoneWeights = ReadWeightsFromVB(data, startIndex);
                vertex.BoneIDs = ReadBonesFromVB(data, startIndex + 4);
            }

            if (declaration.HasFlag(VertexFlags.Color))
            {
                int startIndex = offsets[VertexFlags.Color].Offset;
                vertex.Color0 = ReadColorFromVB(data, startIndex);
            }

            if (declaration.HasFlag(VertexFlags.Color1))
            {
                int startIndex = offsets[VertexFlags.Color1].Offset;
                vertex.Color1 = ReadColorFromVB(data, startIndex);
            }

            if (declaration.HasFlag(VertexFlags.TexCoords0))
            {
                int startIndex = offsets[VertexFlags.TexCoords0].Offset;
                vertex.UVs[0] = ReadTexcoordFromVB(data, startIndex);
            }

            if (declaration.HasFlag(VertexFlags.TexCoords1))
            {
                int startIndex = offsets[VertexFlags.TexCoords1].Offset;
                vertex.UVs[1] = ReadTexcoordFromVB(data, startIndex);
            }

            if (declaration.HasFlag(VertexFlags.TexCoords2))
            {
                int startIndex = offsets[VertexFlags.TexCoords2].Offset;
                vertex.UVs[2] = ReadTexcoordFromVB(data, startIndex);
            }

            if (declaration.HasFlag(VertexFlags.ShadowTexture))
            {
                int startIndex = offsets[VertexFlags.ShadowTexture].Offset;
                vertex.UVs[3] = ReadTexcoordFromVB(data, startIndex);
            }

            if (declaration.HasFlag(VertexFlags.BBCoeffs))
            {
                int startIndex = offsets[VertexFlags.BBCoeffs].Offset;
                vertex.BBCoeffs = ReadBBCoeffsVB(data, startIndex);
            }

            if (declaration.HasFlag(VertexFlags.DamageGroup))
            {
                int startIndex = offsets[VertexFlags.DamageGroup].Offset;
                vertex.DamageGroup = ReadDamageGroupFromVB(data, startIndex);
            }

            // We only try to calculate binormal vector if we have the correct tangent space data so far..
            if (declaration.HasFlag(VertexFlags.Normals) && declaration.HasFlag(VertexFlags.Tangent))
            {
                Vector4 positionW = new Vector4(vertex.Position, vertex.Binormal.X);
                vertex.Binormal = CalculateBinormal(positionW, vertex.Tangent, vertex.Normal);
            }
            return vertex;
        }

        private static Vector4 ReadPositionDataFromVB(byte[] data, int i, float factor, Vector3 offset)
        {
            //.w component is binormal
            Vector4 vec = new Vector4();

            //create small arrays
            byte[] xData = new byte[] { data[i + 0], data[i + 1] };
            byte[] yData = new byte[] { data[i + 2], data[i + 3] };
            byte[] zData = new byte[] { data[i + 4], data[i + 5] };
            byte[] wData = new byte[] { data[i + 6], data[i + 7] };

            //reverse if big
            if (isBigEndian)
            {
                Array.Reverse(xData);
                Array.Reverse(yData);
                Array.Reverse(zData);
                Array.Reverse(wData);
            }

            ushort x = BitConverter.ToUInt16(xData, 0);
            ushort y = BitConverter.ToUInt16(yData, 0);
            ushort z = (ushort)(BitConverter.ToUInt16(zData, 0) & short.MaxValue);
            ushort w = (ushort)(BitConverter.ToUInt16(zData, 0) & 0x8000);
            vec = new Vector4(x * factor, y * factor, z * factor, w != 0.0f ? -1.0f : 1.0f);
            vec += new Vector4(offset, 0.0f);
            return vec;
        }

        private static Vector3 ReadTangentDataFromVB(byte[] data, int i)
        {
            Vector3 tan = new Vector3();
            tan.X = (data[i + 6] - 127.0f) * 0.007874f;
            tan.Y = (data[i + 7] - 127.0f) * 0.007874f;
            tan.Z = (data[i + 11] - 127.0f) * 0.007874f;

            return tan;
        }

        private static Vector3 ReadNormalDataFromVB(byte[] data, int i)
        {
            Vector3 norm = new Vector3();
            norm.X = (data[i] - 127.0f) * 0.007874f;
            norm.Y = (data[i + 1] - 127.0f) * 0.007874f;
            norm.Z = (data[i + 2] - 127.0f) * 0.007874f;
            return norm;
        }

        private static Vector3 CalculateBinormal(Vector4 position, Vector3 tangent, Vector3 normal)
        {
            Vector3 binormal = Vector3.Cross(normal, tangent);
            binormal *= -position.W;
            return binormal;
        }

        private static Vector2 ReadTexcoordFromVB(byte[] data, int i)
        {
            byte[] xData = new byte[] { data[i + 0], data[i + 1] };
            byte[] yData = new byte[] { data[i + 2], data[i + 3] };

            if(isBigEndian)
            {
                Array.Reverse(xData);
                Array.Reverse(yData);
            }

            Toolkit.Mathematics.Half X = Toolkit.Mathematics.Half.ToHalf(xData, 0);
            Toolkit.Mathematics.Half Y = Toolkit.Mathematics.Half.ToHalf(yData, 0);

            Y = -Y;
            return new Vector2(X, Y);
        }

        private static int ReadDamageGroupFromVB(byte[] data, int i)
        {
            return BitConverter.ToInt32(data, i);
        }

        // Colour format is BGRA??
        private static byte[] ReadColorFromVB(byte[] data, int i)
        {
            byte[] color = new byte[4];
            color[2] = data[i + 0];
            color[1] = data[i + 1];
            color[0] = data[i + 2];
            color[3] = data[i + 3];
            return color;
        }

        private static Vector3 ReadBBCoeffsVB(byte[] data, int i)
        {
            Vector3 vec = new Vector3();
            vec.X = BitConverter.ToSingle(data, i);
            vec.Y = BitConverter.ToSingle(data, i + 4);
            vec.Z = BitConverter.ToSingle(data, i + 8);
            return vec;
        }

        private static float[] ReadWeightsFromVB(byte[] data, int i)
        {
            float[] weights = new float[4];
            weights[0] = (data[i + 0] / 255.0f);
            weights[1] = (data[i + 1] / 255.0f);
            weights[2] = (data[i + 2] / 255.0f);
            weights[3] = (data[i + 3] / 255.0f);
            return weights;
        }

        private static byte[] ReadBonesFromVB(byte[] data, int i)
        {
            byte[] bones = new byte[4];
            Array.Copy(data, i, bones, 0, 4);
            return bones;
        }
    }
}
