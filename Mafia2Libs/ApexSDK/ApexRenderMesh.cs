using System.Collections.Generic;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ApexSDK
{
    public class ApexRenderMesh
    {
        readonly int type = 41;
        readonly string name = "ApexRenderMesh";

        int unk0;
        int unk1;
        int numVerts1;
        long unk2;
        byte[] unkFlags;
        int unk3;
        long unk4;
        byte padding;
        int numVerts2;
        Vector3[] vertices;
        int numVerts3;
        Vector3[] unkVectors1;
        int numVerts4;
        Vector3[] unkVectors2;
        int numVerts5;
        Vector3[] unkVectors3;
        int numVerts6;
        int[] boneIdx;
        int numVerts7;
        short[] unkShorts;
        int unk6;
        int unk7;
        int unk8;
        float[] unk8Collection;
        int[] unk8Collection2;
        int numTriangles;
        Int3[] triangles;

        long unk20;
        int unk20_1;
        long unk21;
        int unk21_1;

        int numMaterials;
        string[] materialNames;

        int unk22;
        BoundingBox bbox;
        int unk23;

        public ApexRenderMesh()
        {
        }

        public ApexRenderMesh(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != type)
                return;

            if (StringHelpers.ReadString32(reader) != name)
                return;

            unk0 = reader.ReadInt32();
            unk1 = reader.ReadInt32();
            numVerts1 = reader.ReadInt32();
            unk2 = reader.ReadInt64();
            unkFlags = reader.ReadBytes(5);
            unk3 = reader.ReadInt32();
            unk4 = reader.ReadInt64();
            padding = reader.ReadByte();
            numVerts2 = reader.ReadInt32();

            vertices = new Vector3[numVerts2];
            for (int i = 0; i != numVerts2; i++)
                vertices[i] = Vector3Extenders.ReadFromFile(reader);

            numVerts3 = reader.ReadInt32();
            unkVectors1 = new Vector3[numVerts3];
            for (int i = 0; i != numVerts3; i++)
                unkVectors1[i] = Vector3Extenders.ReadFromFile(reader);

            numVerts4 = reader.ReadInt32();
            unkVectors2 = new Vector3[numVerts4];
            for (int i = 0; i != numVerts4; i++)
                unkVectors2[i] = Vector3Extenders.ReadFromFile(reader);

            numVerts5 = reader.ReadInt32();
            unkVectors3 = new Vector3[numVerts5];
            for (int i = 0; i != numVerts5; i++)
                unkVectors3[i] = Vector3Extenders.ReadFromFile(reader);

            numVerts6 = reader.ReadInt32();
            boneIdx = new int[numVerts6];
            for (int i = 0; i != numVerts6; i++)
                boneIdx[i] = reader.ReadInt32();

            unkShorts = new short[numVerts6*4];
            for (int i = 0; i != numVerts6*4; i++)
                unkShorts[i] = reader.ReadInt16();

            unk6 = reader.ReadInt16();
            unk7 = reader.ReadInt16();
            unk8 = reader.ReadInt32();
            unk8Collection = new float[unk8];
            unk8Collection2 = new int[unk8];
            for (int i = 0; i != unk8; i++)
                unk8Collection[i] = reader.ReadSingle();

            for (int i = 0; i != unk8; i++)
                unk8Collection2[i] = reader.ReadInt32();

            numTriangles = reader.ReadInt32();
            triangles = new Int3[numTriangles];

            for(int i = 0; i != numTriangles; i++)
            {
                Int3 int3 = new Int3();
                int3.X = reader.ReadInt32();
                int3.Y = reader.ReadInt32();
                int3.Z = reader.ReadInt32();
                triangles[i] = int3;
            }

            unk20 = reader.ReadInt64();
            unk20_1 = reader.ReadInt32();
            unk21 = reader.ReadInt64();
            unk21_1 = reader.ReadInt32();
            numMaterials = reader.ReadInt32();
            materialNames = new string[numMaterials];
            for (int i = 0; i != numMaterials; i++)
            {
                materialNames[i] = StringHelpers.ReadString32(reader);
            }

            unk22 = reader.ReadInt32();
            bbox = BoundingBoxExtenders.ReadFromFile(reader);
            unk23 = reader.ReadInt32();

            List<string> data = new List<string>();

            for (int z = 0; z != unkVectors1.Length; z++)
                data.Add(string.Format("v {0} {1} {2}", unkVectors1[z].X, unkVectors1[z].Y, unkVectors1[z].Z));

            data.Add("");
            data.Add("g " + "Object");
            for (int z = 0; z != numTriangles; z++)
                data.Add(string.Format("f {0} {1} {2}", triangles[z].X + 1, triangles[z].Y + 1, triangles[z].Z + 1));

            File.WriteAllLines("Object.obj", data.ToArray());
        }
    }
}
