using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mafia2
{
    public class CustomEDM
    {
        string name;
        int partCount;
        Part[] parts;


        ulong bufferIndexHash;
        ulong bufferVertexHash;
        VertexFlags flags;
        float positionFactor;
        Vector3 positionOffset;
        IndexBuffer indexBuffer;
        VertexBuffer vertexBuffer;
        Bounds bounds;


        public string Name {
            get { return name; }
            set { name = value; }
        }
        public int PartCount {
            get { return partCount; }
            set { partCount = value; }
        }
        public Part[] Parts {
            get { return parts; }
            set { parts = value; }
        }

        //This part is todo with putting the data into buffers.
        public ulong BufferIndexHash {
            get { return bufferIndexHash; }
            set { bufferIndexHash = value; }
        }
        public ulong BufferVertexHash {
            get { return bufferVertexHash; }
            set { bufferVertexHash = value; }
        }
        public Bounds Bound {
            get { return bounds; }
            set { bounds = value; }
        }
        public VertexFlags BufferFlags {
            get { return flags; }
            set { flags = value; }
        }
        public float PositionFactor {
            get { return positionFactor; }
            set { positionFactor = value; }
        }
        public Vector3 PositionOffset {
            get { return positionOffset; }
            set { positionOffset = value; }
        }
        public IndexBuffer IndexBuffer {
            get { return indexBuffer; }
            set { indexBuffer = value; }
        }
        public VertexBuffer VertexBuffer {
            get { return vertexBuffer; }
            set { vertexBuffer = value; }
        }

        public CustomEDM(BinaryReader reader)
        {
            int size = reader.ReadByte();
            name = new string(reader.ReadChars(size));

            int partCount = reader.ReadInt32();
            parts = new Part[partCount];

            for(int i = 0; i != partCount; i++)
            {
                parts[i] = new Part(reader);
            }
        }

        public CustomEDM(string name, int numParts)
        {
            this.name = name;
            this.partCount = numParts;
            parts = new Part[numParts];
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((byte)name.Length);
            writer.Write(name.ToCharArray());
            writer.Write(partCount);
            for (int i = 0; i != partCount; i++)
                parts[i].WriteToFile(writer);
        }

        /// <summary>
        /// Overwrite a part of the EDM. This is a better way to create a new part.
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="indices"></param>
        /// <param name="name"></param>
        /// <param name="slot"></param>
        public void AddPart(List<Vertex> vertex, List<Short3> indices, string name, int slot)
        {
            parts[slot] = new Part(vertex, indices, name);
        }

        public class Part
        {
            string name;
            Vector3[] vertices;
            Vector3[] normals;
            Vector3[] tangents;
            UVVector2[] uvs;
            List<Short3> indices;
            bool hasNormals = true;
            bool hasTangents = true;
            bool hasUVs = true;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public Vector3[] Vertices {
                get { return vertices; }
                set { vertices = value; }
            }
            public Vector3[] Normals {
                get { return normals; }
                set { normals = value; }
            }
            public Vector3[] Tangents {
                get { return tangents; }
                set { tangents = value; }
            }
            public UVVector2[] UVs {
                get { return uvs; }
                set { uvs = value; }
            }
            public List<Short3> Indices {
                get { return indices; }
                set { indices = value; }
            }
            public bool HasNormals {
                get { return hasNormals; }
                set { hasNormals = value; }
            }
            public bool HasTangents {
                get { return hasTangents; }
                set { hasTangents = value; }
            }
            public bool HasUVs {
                get { return hasUVs; }
                set { hasUVs = value; }
            }

            /// <summary>
            /// Create an empty part.
            /// </summary>
            public Part() { }

            /// <summary>
            /// Create a part with vertexes, indices and a name.
            /// </summary>
            /// <param name="vertex">Vertexes from the buffers</param>
            /// <param name="indices">Indices from the parts</param>
            /// <param name="name">Name of the part</param>
            public Part(List<Vertex> vertex, List<Short3> indices, string name)
            {
                this.vertices = new Vector3[vertex.Count];

                if (vertex[0].Normal == null)
                    hasNormals = false;
                else
                    this.normals = new Vector3[vertex.Count];

                if (vertex[0].Tangent == null)
                    hasTangents = false;
                else
                    this.tangents = new Vector3[vertex.Count];

                if (vertex[0].UVs.Length == 0)
                    hasUVs = false;
                else
                    this.uvs = new UVVector2[vertex.Count];

                for (int i = 0; i != vertex.Count; i++)
                {
                    vertices[i] = vertex[i].Position;

                    if(hasNormals)
                        normals[i] = vertex[i].Normal;

                    if(hasTangents)
                        tangents[i] = vertex[i].Tangent;

                    if(hasUVs)
                        uvs[i] = vertex[i].UVs[0];
                }

                this.indices = indices;
                this.name = name;
            }

            /// <summary>
            /// Usually used for Collisions.
            /// </summary>
            /// <param name="vertex">Vertexes from the buffers</param>
            /// <param name="indices">Indices from the parts</param>
            /// <param name="name">Name of the part</param>
            public Part(Vector3[] vertices, Int3[] triangles, string name)
            {
                this.vertices = vertices;
                hasTangents = false;
                hasNormals = false;
                hasUVs = false;
                indices = new List<Short3>();

                foreach (Int3 tri in triangles)
                {
                    indices.Add(new Short3(tri.I1, tri.I2, tri.I3));
                }

                this.name = name;
            }

            /// <summary>
            /// Unknown
            /// </summary>
            /// <param name="vertex">Vertexes from the buffers</param>
            /// <param name="indices">Indices from the parts</param>
            /// <param name="name">Name of the part</param>
            public Part(Vector3[] vertices, Short3[] triangles, string name)
            {
                this.vertices = vertices;

                indices = new List<Short3>();

                foreach (Short3 tri in triangles)
                {
                    indices.Add(tri);
                }

                this.name = name;

                uvs = new UVVector2[0];
            }

            /// <summary>
            /// Read the EDM part from a mesh file. 
            /// </summary>
            /// <param name="reader"></param>
            public Part(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            /// <summary>
            /// Write part to file.
            /// </summary>
            /// <param name="writer"></param>
            public void WriteToFile(BinaryWriter writer)
            {
                if (name == null)
                    return;

                writer.Write(name);
                writer.Write(hasNormals);
                writer.Write(hasTangents);
                writer.Write(hasUVs);
                writer.Write(vertices.Length);

                for (int c = 0; c != vertices.Length; c++)
                    vertices[c].WriteToFile(writer);

                if (hasNormals)
                {
                    for (int c = 0; c != normals.Length; c++)
                        normals[c].WriteToFile(writer);
                }

                if (hasTangents)
                {
                    for (int c = 0; c != tangents.Length; c++)
                        tangents[c].WriteToFile(writer);
                }

                if (hasUVs)
                {
                    writer.Write(uvs.Length);
                    for (int c = 0; c != uvs.Length; c++)
                    {
                        writer.Write(uvs[c].X);
                        writer.Write(1f - uvs[c].Y);
                    }
                }

                writer.Write(indices.Count);
                for (int c = 0; c != indices.Count; c++)
                    indices[c].WriteToFile(writer);
            }

            /// <summary>
            /// Read part from file.
            /// </summary>
            /// <param name="reader"></param>
            public void ReadFromFile(BinaryReader reader)
            {
                byte nameSize = reader.ReadByte();
                name = new string(reader.ReadChars(nameSize));
                hasNormals = reader.ReadBoolean();
                hasTangents = reader.ReadBoolean();
                hasUVs = reader.ReadBoolean();
                int size = reader.ReadInt32();
                vertices = new Vector3[size];

                if(hasNormals)
                    normals = new Vector3[size];

                if(hasTangents)
                    tangents = new Vector3[size];

                for (int c = 0; c != vertices.Length; c++)
                    vertices[c] = new Vector3(reader);

                if (hasNormals)
                {
                    for (int c = 0; c != normals.Length; c++)
                        normals[c] = new Vector3(reader);
                }

                if (hasTangents)
                {
                    for (int c = 0; c != tangents.Length; c++)
                        tangents[c] = new Vector3(reader);
                }

                if (hasUVs)
                {
                    size = reader.ReadInt32();
                    uvs = new UVVector2[size];
                    for (int c = 0; c != uvs.Length; c++)
                    {
                        uvs[c] = new UVVector2(HalfHelper.SingleToHalf(reader.ReadSingle()), HalfHelper.SingleToHalf(reader.ReadSingle()));
                        uvs[c].Y = (Half)1f - uvs[c].Y;
                    }
                }
                size = reader.ReadInt32();
                indices = new List<Short3>();
                for (int c = 0; c != size; c++)
                    indices.Add(new Short3(reader));
            }
        }
    }
}
