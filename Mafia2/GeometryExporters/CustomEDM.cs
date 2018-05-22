using System.IO;

namespace Mafia2
{
    //exclusive file type to load entire frame (hopefully) into 3ds.
    public class CustomEDM
    {
        string name;
        Vector3[] vertices;
        UVVector2[] uvs;
        Short3[] indices;

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
        public Short3[] Indices {
            get { return indices; }
            set { indices = value; }
        }

        public CustomEDM(Vertex[] vertex, Short3[] indices, string name)
        {
            this.vertices = new Vector3[vertex.Length];
            this.uvs = new UVVector2[vertex.Length];

            for(int i = 0; i != vertex.Length; i++)
            {
                vertices[i] = vertex[i].Position;
                uvs[i] = vertex[i].UVs[0];
            }

            this.indices = indices;
            this.name = name;
        }
    }
}
