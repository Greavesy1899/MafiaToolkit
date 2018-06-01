using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    //exclusive file type to load entire frame (hopefully) into 3ds.
    public class CustomEDM
    {
        string name;
        Vector3[] vertices;
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
            this.uvs = new UVVector2[vertex.Count];

            for(int i = 0; i != vertex.Count; i++)
            {
                vertices[i] = vertex[i].Position;
                if(vertex[i].UVs.Length > 0)
                    uvs[i] = vertex[i].UVs[0];
            }

            this.indices = indices;
            this.name = name;
        }
    }
}
