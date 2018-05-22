using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    //constructor via mafia2 format.
    public class WavefrontOBJ
    {
        Vector3[] positions;
        Vector3[] normals;
        UVVector2[] uvs;
        WavefrontGroup[] groups;

        string name;

        public WavefrontOBJ(Vertex[] vertices, ModelPart[] parts, string name)
        {
            this.name = name;

            positions = new Vector3[vertices.Length];
            normals = new Vector3[vertices.Length];
            uvs = new UVVector2[vertices.Length];

            for (int i = 0; i != vertices.Length; i++)
            {
                positions[i] = vertices[i].Position;
                normals[i] = vertices[i].Normal;
                uvs[i] = vertices[i].UVs[0];
            }

            groups = new WavefrontGroup[parts.Length];
            for(int i = 0; i != parts.Length; i++)
            {
                groups[i] = new WavefrontGroup(parts[i]);
            }
        }

        public WavefrontOBJ(Vector3[] positions, Short3[] indices)
        {
            this.positions = positions;
            name = "mesh";

            groups = new WavefrontGroup[1];
            groups[0] = new WavefrontGroup();
            groups[0].Indices = indices;
            groups[0].Material = "NULL";
            groups[0].Name = "NULL";
        }

        public void ExportOBJ()
        {
            List<string> newObj = new List<string>();
            List<string> newMtl = new List<string>();

            newObj.Add(string.Format("mtllib {0}", name + ".mtl"));
            newObj.Add("");
            ExportChunkVertices(ref newObj);

            if(uvs != null)
                ExportChunkUVs(ref newObj);

            if (normals != null)
                ExportChunkNormals(ref newObj);

            ExportChunkMaterials(ref newMtl);
            ExportChunkGroups(ref newObj);

            File.WriteAllLines("objects/" + name + ".obj", newObj);
            File.WriteAllLines("objects/" + name + ".mtl", newMtl);
        }

        private void ExportChunkVertices(ref List<string> newObj)
        {
            for (int i = 0; i != positions.Length; i++)
            {
                newObj.Add(string.Format("v {0} {1} {2}", positions[i].X, positions[i].Y, positions[i].Z));
            }
            newObj.Add("");
        }
        private void ExportChunkUVs(ref List<string> newObj)
        {
            for (int i = 0; i != uvs.Length; i++)
            {
                newObj.Add(string.Format("vt {0} {1} {2}", uvs[i].X, 1 - uvs[i].Y, 0));
            }
            newObj.Add("");
        }
        private void ExportChunkNormals(ref List<string> newObj)
        {
            for (int i = 0; i != normals.Length; i++)
            {
                newObj.Add(string.Format("vn {0} {1} {2}", normals[i].X, normals[i].Y, normals[i].Z));
            }
            newObj.Add("");
        }
        private void ExportChunkGroups(ref List<string> newObj)
        {
            for (int i = 0; i != groups.Length; i++)
            {
                newObj.Add(string.Format("g part{0}", i));
                newObj.Add(string.Format("usemtl {0}", groups[i].Material));

                for (int c = 0; c != groups[i].Indices.Length; c++)
                {
                    if(positions != null && normals != null && uvs != null)
                        newObj.Add(string.Format("f {0}/{0} {1}/{1} {2}/{2}", groups[i].Indices[c].s1 + 1, groups[i].Indices[c].s2 + 1, groups[i].Indices[c].s3 + 1));
                    else
                        newObj.Add(string.Format("f {0} {1} {2}", groups[i].Indices[c].s1 + 1, groups[i].Indices[c].s2 + 1, groups[i].Indices[c].s3 + 1));

                }
                newObj.Add("");
            }
        }
        private void ExportChunkMaterials(ref List<string> newMtl)
        {
            for (int i = 0; i != groups.Length; i++)
            {
                newMtl.Add(string.Format("newmtl {0}", groups[i].Material));
                newMtl.Add(string.Format("kd 0.6 0.6 0.6"));
                newMtl.Add(string.Format("ka 0 0 0"));
                newMtl.Add(string.Format("ks 0 0 0"));
                newMtl.Add(string.Format("Ns 16"));
                newMtl.Add(string.Format("illum 2"));
                newMtl.Add(string.Format("map_kd {0}", groups[i].Material));
                newMtl.Add("");
            }
        }
    }

    class WavefrontGroup
    {
        string material;
        string name;
        Short3[] indices;

        public string Material {
            get { return material; }
            set { material = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }
        public Short3[] Indices {
            get { return indices; }
            set { indices = value; }
        }

        public WavefrontGroup() { }

        //converstion from mafia2 modelpart
        public WavefrontGroup(ModelPart modelPart)
        {
            material = modelPart.Material;
            name = modelPart.Material;
            indices = modelPart.Indices;
        }
    }
}
