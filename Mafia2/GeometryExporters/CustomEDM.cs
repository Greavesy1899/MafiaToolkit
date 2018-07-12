using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mafia2
{
    //exclusive file type to load entire frame (hopefully) into 3ds.
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

        public void AddPart(List<Vertex> vertex, List<Short3> indices, string name, int slot)
        {
            parts[slot] = new Part(vertex, indices, name);
        }

        /// <summary>
        /// Build new buffers for new mesh. These will replace existing buffers.
        /// </summary>
        public void BuildBuffers()
        {
            indexBuffer = new IndexBuffer(bufferIndexHash);
            vertexBuffer = new VertexBuffer(bufferVertexHash);

            List<ushort> idata = new List<ushort>();
            List<byte> vdata = new List<byte>(); 

            for(int i  = 0; i != parts[0].Indices.Count; i++)
            {
                idata.Add((ushort)parts[0].Indices[i].s1);
                idata.Add((ushort)parts[0].Indices[i].s2);
                idata.Add((ushort)parts[0].Indices[i].s3);
            }

            indexBuffer.Data = idata.ToArray();
            bool hasTangents = false;
            for (int i = 0; i != parts[0].Vertices.Length; i++)
            {
                byte tz = 0;
                if (flags.HasFlag(VertexFlags.Position))
                {
                    float x = parts[0].Vertices[i].X - positionOffset.X;
                    x = x / positionFactor;
                    float y = parts[0].Vertices[i].Y - positionOffset.Y;
                    y = y / positionFactor;
                    float z = parts[0].Vertices[i].Z - positionOffset.Z;
                    z = z / positionFactor;

                    ushort v1 = Convert.ToUInt16(x);
                    ushort v2 = Convert.ToUInt16(y);
                    ushort v3 = Convert.ToUInt16(z);

                    byte[] bytesv1 = BitConverter.GetBytes(v1);
                    byte[] bytesv2 = BitConverter.GetBytes(v2);
                    byte[] bytesv3 = BitConverter.GetBytes(v3);

                    vdata.Add(bytesv1[0]);
                    vdata.Add(bytesv1[1]);
                    vdata.Add(bytesv2[0]);
                    vdata.Add(bytesv2[1]);
                    vdata.Add(bytesv3[0]);
                    vdata.Add(bytesv3[1]);
                }
                if (flags.HasFlag(VertexFlags.Tangent))
                {
                    hasTangents = true;

                    float fx = parts[0].Tangents[i].X * 127.0f + 127.0f;
                    float fy = parts[0].Tangents[i].Y * 127.0f + 127.0f;
                    float fz = parts[0].Tangents[i].Z * 127.0f + 127.0f;

                    if (float.IsNaN(fx))
                        fx = 0.0f;

                    if (float.IsNaN(fy))
                        fy = 0.0f;
                    
                    if (float.IsNaN(fz))
                        fz = 0.0f;

                    byte x = Convert.ToByte(fx);
                    byte y = Convert.ToByte(fy);
                    tz = Convert.ToByte(fz);

                    vdata.Add(x);
                    vdata.Add(y);

                }
                if (flags.HasFlag(VertexFlags.Normals))
                {
                    byte x = Convert.ToByte(parts[0].Normals[i].X * 127.0f + 127.0f);
                    byte y = Convert.ToByte(parts[0].Normals[i].Y * 127.0f + 127.0f);
                    byte z = Convert.ToByte(parts[0].Normals[i].Z * 127.0f + 127.0f);

                    vdata.Add(x);
                    vdata.Add(y);
                    vdata.Add(z);
                }
                if (hasTangents)
                {
                    vdata.Add(tz);
                }
                if (flags.HasFlag(VertexFlags.TexCoords0))
                {
                    byte[] x = Half.GetBytes(parts[0].UVs[i].X);
                    byte[] y = Half.GetBytes(parts[0].UVs[i].Y);

                    vdata.Add(x[0]);
                    vdata.Add(x[1]);
                    vdata.Add(y[0]);
                    vdata.Add(y[1]);
                }
            }

            vertexBuffer.Data = vdata.ToArray();
        }

        /// <summary>
        /// Calculate new bounds on model;
        /// </summary>
        /// <param name="updateGeom">If this is true, then the "Position Offset" and "Position Scale will be updated."</param>
        public void CalculateBounds(bool updateGeom)
        {
            Vector3 min = new Vector3(0);
            Vector3 max = new Vector3(0);

            for (int i = 0; i != parts[0].Vertices.Length; i++)
            {
                if (parts[0].Vertices[i].X < min.X)
                    min.X = parts[0].Vertices[i].X;

                if (parts[0].Vertices[i].X > max.X)
                    max.X = parts[0].Vertices[i].X;

                if (parts[0].Vertices[i].Y < min.Y)
                    min.Y = parts[0].Vertices[i].Y;

                if (parts[0].Vertices[i].Y > max.Y)
                    max.Y = parts[0].Vertices[i].Y;

                if (parts[0].Vertices[i].Z < min.Z)
                    min.Z = parts[0].Vertices[i].Z;

                if (parts[0].Vertices[i].Z > max.Z)
                    max.Z = parts[0].Vertices[i].Z;
            }

            bounds = new Bounds(min, max);

            if (updateGeom == false)
                return;

            float minFloatf = 0.000016f;
            Vector3 minFloat = new Vector3(minFloatf);

            min -= minFloat;
            max += minFloat;

            positionOffset = min;
            float fMaxSize = Math.Max(max.X - min.X + minFloatf, Math.Max(max.Y - min.Y + minFloatf, (max.Z - min.Y + minFloatf) * 2.0f));

            positionFactor = fMaxSize / 0x10000;
        }

        public class Part
        {
            string name;
            Vector3[] vertices;
            Vector3[] normals;
            Vector3[] tangents;
            UVVector2[] uvs;
            List<Short3> indices;

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

            public Part(List<Vertex> vertex, List<Short3> indices, string name)
            {
                this.vertices = new Vector3[vertex.Count];
                this.normals = new Vector3[vertex.Count];
                this.tangents = new Vector3[vertex.Count];
                this.uvs = new UVVector2[vertex.Count];

                for (int i = 0; i != vertex.Count; i++)
                {
                    vertices[i] = vertex[i].Position;
                    normals[i] = vertex[i].Normal;
                    tangents[i] = vertex[i].Tangent;

                    if (vertex[i].UVs.Length > 0)
                        uvs[i] = vertex[i].UVs[0];
                }

                this.indices = indices;
                this.name = name;
            }
            public Part(Vector3[] vertices, Int3[] triangles, string name)
            {
                this.vertices = vertices;

                indices = new List<Short3>();

                foreach (Int3 tri in triangles)
                {
                    indices.Add(new Short3(tri.i1, tri.i2, tri.i3));
                }

                this.name = name;

                uvs = new UVVector2[0];
            }

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

            public Part(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(name);
                writer.Write(vertices.Length);

                for (int c = 0; c != vertices.Length; c++)
                {
                    vertices[c].WriteToFile(writer);
                }

                for (int c = 0; c != normals.Length; c++)
                {
                    if (normals[c] == null)
                        normals[c] = new Vector3(0f);

                    normals[c].WriteToFile(writer);
                }

                for (int c = 0; c != tangents.Length; c++)
                {
                    if (tangents[c] == null)
                        tangents[c] = new Vector3(0f);

                    tangents[c].WriteToFile(writer);
                }

                writer.Write(uvs.Length);
                for (int c = 0; c != uvs.Length; c++)
                {
                    writer.Write(uvs[c].X);
                    writer.Write(1f - uvs[c].Y);
                }
                writer.Write(indices.Count);
                for (int c = 0; c != indices.Count; c++)
                {
                    writer.Write(indices[c].s1);
                    writer.Write(indices[c].s2);
                    writer.Write(indices[c].s3);
                }
            }

            public void ReadFromFile(BinaryReader reader)
            {
                byte nameSize = reader.ReadByte();
                name = new string(reader.ReadChars(nameSize));

                int size = reader.ReadInt32();
                vertices = new Vector3[size];
                normals = new Vector3[size];
                tangents = new Vector3[size];

                for (int c = 0; c != vertices.Length; c++)
                    vertices[c] = new Vector3(reader);

                for (int c = 0; c != normals.Length; c++)
                    normals[c] = new Vector3(reader);

                for (int c = 0; c != tangents.Length; c++)
                    tangents[c] = new Vector3(reader);

                size = reader.ReadInt32();
                uvs = new UVVector2[size];
                for (int c = 0; c != uvs.Length; c++)
                {
                    uvs[c] = new UVVector2(HalfHelper.SingleToHalf(reader.ReadSingle()), HalfHelper.SingleToHalf(reader.ReadSingle()));
                    uvs[c].Y = (Half)1f - uvs[c].Y;
                }

                size = reader.ReadInt32();
                indices = new List<Short3>();
                for (int c = 0; c != size; c++)
                    indices.Add(new Short3(reader));
            }
        }
    }
}
