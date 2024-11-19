using System;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

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
        public Vector3[] vertices;
        public uint[] indices;
        Vector3[] normals;
        Vector3 hullCentre;
        byte[] polygonData;
        byte[] nbData;
        uint[] nbIndices;
        int unk0;
        int unk1;
        ushort[] edges; //actually 2 bytes but merged to short/
        Vector3[] edgeNormal;
        int[] edgeNormalEncoded;
        int maxIDX0;
        ushort[] maxIDX0Buffer;
        int maxIDX1;
        ushort[] maxIDX1Buffer;
        int maxIDX2;
        ushort[] maxIDX2Buffer;
        byte[] unkOffsetBuffer;

        int valencyVersion;
        int valeNbVerts;
        int valeNbAdjVerts;
        int[] valeVerts;
        byte[] valeAdjVerts;

        byte[] quantizationBoxBuffer;
        float[] unkTailFloats;


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
                throw new FileFormatException();

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
            int polyDataSize = 36;
            nb = reader.ReadInt32();
            lastOffset = reader.ReadInt32();

            vertices = new Vector3[numVertices];
            for(int i = 0; i != numVertices; i++)
                vertices[i] = Vector3Utils.ReadFromFile(reader);

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
                throw new NotImplementedException();

            }

            hullCentre = Vector3Utils.ReadFromFile(reader);
            polygonData = reader.ReadBytes(numPolygons * polyDataSize);

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
            unk0 = reader.ReadInt32();
            unk1 = reader.ReadInt32();

            if (convHullVersion >= 3)
            {
                edges = new ushort[numEdges];
                for (int i = 0; i < numEdges; i++)
                    edges[i] = reader.ReadUInt16();
            }

            if(convHullVersion >= 4 && useQuantizedNormals == 0)
            {
                edgeNormalEncoded = new int[numEdges];
                for (int i = 0; i < numEdges; i++)
                    edgeNormalEncoded[i] = reader.ReadInt16();
            }
            else
            {
                edgeNormal = new Vector3[numEdges];
                for (int i = 0; i < numEdges; i++)
                    edgeNormal[i] = Vector3Utils.ReadFromFile(reader);
            }

            maxIDX0 = reader.ReadInt32();
            maxIDX0Buffer = new ushort[numEdges];
            if (maxIDX0 <= 0xff)
            {
                for (int i = 0; i < numEdges; i++)
                    maxIDX0Buffer[i] = reader.ReadByte();
            }
            else if ((maxIDX0 <= 0xffff))
            {
                for (int i = 0; i < numEdges; i++)
                    maxIDX0Buffer[i] = reader.ReadUInt16();
            }
            maxIDX1 = reader.ReadInt32();
            maxIDX1Buffer = new ushort[numEdges];
            if (maxIDX1 <= 0xff)
            {
                for (int i = 0; i < numEdges; i++)
                    maxIDX1Buffer[i] = reader.ReadByte();
            }
            else if ((maxIDX1 <= 0xffff))
            {
                for (int i = 0; i < numEdges; i++)
                    maxIDX1Buffer[i] = reader.ReadUInt16();
            }
            maxIDX2 = reader.ReadInt32();
            maxIDX2Buffer = new ushort[numEdges];
            if (maxIDX2 <= 0xff)
            {
                for (int i = 0; i < numEdges; i++)
                    maxIDX2Buffer[i] = reader.ReadByte();
            }
            else if ((maxIDX2 <= 0xffff))
            {
                for (int i = 0; i < numEdges; i++)
                    maxIDX2Buffer[i] = reader.ReadUInt16();
            }

            unkOffsetBuffer = reader.ReadBytes(lastOffset);

            if (reader.ReadInt32() != 21316425) //ICE
                return;

            if (reader.ReadInt32() != 1162625366) //VALE
                return;

            //line 437, CookedMeshReader.h;
            valencyVersion = reader.ReadInt32();
            valeNbVerts = reader.ReadInt32();
            valeNbAdjVerts = reader.ReadInt32();
            int valeMax = reader.ReadInt32();
            valeVerts = new int[valeMax];
            if (maxIDX2 <= 0xff)
            {
                for (int i = 0; i < valeNbVerts; i++)
                    maxIDX2Buffer[i] = reader.ReadByte();
            }
            else if ((maxIDX2 <= 0xffff))
            {
                for (int i = 0; i < valeNbVerts; i++)
                    maxIDX2Buffer[i] = reader.ReadUInt16();
            }
            valeAdjVerts = reader.ReadBytes(valeNbAdjVerts);
            int qSize = reader.ReadInt32();
            quantizationBoxBuffer = reader.ReadBytes(qSize);

            unkTailFloats = new float[24];
            for (int i = 0; i < 24; i++)
                unkTailFloats[i] = reader.ReadSingle();

            //CookConvexCollision();
        }

        public void CookConvexCollision()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("uncooked.bin", FileMode.Create)))
            {
                writer.Write(numVertices);

                for (int i = 0; i < vertices.Length; i++)
                    vertices[i].WriteToFile(writer);

                writer.Write(numIndices);

                for (int i = 0; i < indices.Length; i++)
                    writer.Write(indices[i]);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {

        }
    }
}
