using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;
using Utils.Types;

namespace ResourceTypes.ItemDesc
{
    public class CollisionConvex
    {
        ushort size;
        int version;
        int numVertices;
        int numIndices;
        int numEdges;
        int numPolygons;
        int nb;
        int lastOffset;

        int convHullVersion;
        Vector3[] vertices;
        uint[] indices;
        Vector3[] normals;
        Vector3 hullCentre;
        byte[] polygonData;
        byte[] nbData;
        uint[] nbIndices;

        public CollisionConvex(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            size = reader.ReadUInt16();

            if (reader.ReadInt32() != 22239310) //NXS 
                return;

            if (reader.ReadInt32() != 1297634883) //CVXM
                return;

            version = reader.ReadInt32();
            if (version != 3)                   //Mafia II uses PhysX convex 3.
                return;

            reader.ReadInt32();

            if (reader.ReadInt32() != 21316425) //ICE
                return;

            if (reader.ReadInt32() != 1279806531) //CLHL
                return;

            reader.ReadInt32();

            if (reader.ReadInt32() != 21316425) //ICE
                return;

            if (reader.ReadInt32() != 1279809091) //CVHL
                return;

            convHullVersion = reader.ReadInt32();
            numVertices = reader.ReadInt32();
            numIndices = reader.ReadInt32();
            numEdges = reader.ReadInt32();
            numPolygons = reader.ReadInt32();
            int polyDataSize = polyDataSize = 4 + 24 + 2 * 4;
            nb = reader.ReadInt32();
            lastOffset = reader.ReadInt32();

            vertices = new Vector3[numVertices];
            for(int i = 0; i != numVertices; i++)
                vertices[i] = Vector3Extenders.ReadFromFile(reader);

            indices = new uint[numIndices * 3];
            uint maxIndex = reader.ReadUInt32();
            if (maxIndex <= 0xff)
            {
                for (int i = 0; i < indices.Length; i++)
                    indices[i] = reader.ReadByte();
            }
            else if (maxIndex <= 0xffff)
            {
                for (int i = 0; i < indices.Length; i++)
                    indices[i] = reader.ReadUInt16();
            }
            else
            {
                for (int i = 0; i < indices.Length; i++)
                    indices[i] = reader.ReadUInt32();
            }

            normals = new Vector3[numVertices];
            ushort useQuantizedNormals = 0;

            if (convHullVersion >= 5)
                useQuantizedNormals = reader.ReadUInt16();

            if(convHullVersion >= 4 && useQuantizedNormals == 0)
            {
                for (int i = 0; i < numVertices; i++)
                    reader.ReadUInt16();
            }
            else
            {
                throw new System.NotImplementedException();

            }

            hullCentre = Vector3Extenders.ReadFromFile(reader);
            polygonData = reader.ReadBytes(polyDataSize);

            if(convHullVersion >= 3)
            {
                nbData = reader.ReadBytes(nb);
                nbIndices = new uint[nb];
                uint maxIdx = reader.ReadUInt32();
                if (maxIdx <= 0xff)
                {
                    for (int i = 0; i < nb; i++)
                        nbIndices[i] = reader.ReadByte();
                }
                else if (maxIdx <= 0xffff)
                {
                    for (int i = 0; i < nb; i++)
                        nbIndices[i] = reader.ReadUInt16();
                }
            }
            //line 437, CookedMeshReader.h;
        }

        public void WriteToFile(BinaryWriter writer)
        {
           //writer.Write(vector.X);
            //writer.Write(vector.Y);
            //writer.Write(vector.Z);
        }
    }
}
