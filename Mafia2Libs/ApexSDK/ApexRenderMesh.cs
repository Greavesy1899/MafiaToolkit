using System.IO;
using Mafia2;
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
        }
    }
}
