using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    //exclusive file type to load entire frame (hopefully) into 3ds.
    public class CustomEDM
    {
        string name;
        Vector3[] vertices;
        Vector3[] normals;
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
        public UVVector2[] UVs {
            get { return uvs; }
            set { uvs = value; }
        }
        public List<Short3> Indices {
            get { return indices; }
            set { indices = value; }
        }

        public CustomEDM(List<Vertex> vertex, List<Short3> indices, string name)
        {
            this.vertices = new Vector3[vertex.Count];
            this.normals = new Vector3[vertex.Count];
            this.uvs = new UVVector2[vertex.Count];

            for (int i = 0; i != vertex.Count; i++)
            {
                vertices[i] = vertex[i].Position;
                normals[i] = vertex[i].Normal;

                if (vertex[i].UVs.Length > 0)
                    uvs[i] = vertex[i].UVs[0];
            }

            this.indices = indices;
            this.name = name;
        }
        public CustomEDM(Vector3[] vertices, Int3[] triangles, string name)
        {
            this.vertices = vertices;

            indices = new List<Short3>();

            foreach(Int3 tri in triangles)
            {
                indices.Add(new Short3(tri.i1, tri.i2,  tri.i3));
            }

            this.name = name;

            uvs = new UVVector2[0];
        }

        public CustomEDM(Vector3[] vertices, Short3[] triangles, string name)
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

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(name);
            writer.Write(vertices.Length);
            for (int c = 0; c != vertices.Length; c++)
                vertices[c].WriteToFile(writer);

            for (int c = 0; c != normals.Length; c++)
                normals[c].WriteToFile(writer);

            writer.Write(uvs.Length);
            for (int c = 0; c != uvs.Length; c++)
            {
                writer.Write(uvs[c].X);
                writer.Write(1f - uvs[c].Y);
            }
            writer.Write(indices.Count);
            for (int c = 0; c != indices.Count; c++)
            {
                writer.Write(indices[c].s1 + 1);
                writer.Write(indices[c].s2 + 1);
                writer.Write(indices[c].s3 + 1);
            }
        }
    }
}
