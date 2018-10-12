using Fbx;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Mafia2.FBX
{
    public class FbxElement
    {
        FbxModel model;
        FbxGeometry geometry;
        Dictionary<long, FbxVideo> videos = new Dictionary<long, FbxVideo>();
        Dictionary<long, FbxMaterial> materials = new Dictionary<long, FbxMaterial>();
        Dictionary<long, FbxTexture> textures = new Dictionary<long, FbxTexture>();
        Lod lod;

        public FbxModel Model {
            get { return model; }
            set { model = value; }
        }
        public FbxGeometry Geometry {
            get { return geometry; }
            set { geometry = value; }
        }
        public Dictionary<long, FbxVideo> Videos {
            get { return videos; }
            set { videos = value; }
        }
        public Dictionary<long, FbxMaterial> Materials {
            get { return materials; }
            set { materials = value; }
        }
        public Dictionary<long, FbxTexture> Textures {
            get { return textures; }
            set { textures = value; }
        }

        public Lod BuildM2ModelFromElement()
        {
            lod = new Lod();
            lod.VertexDeclaration = VertexFlags.Position;
            if (geometry.LayerNormal != null)
                lod.VertexDeclaration |= VertexFlags.Normals;

            if (geometry.LayerUV != null)
                lod.VertexDeclaration |= VertexFlags.TexCoords0;
            lod.Vertices = ConvertFBXVertices();

            lod.Parts = new ModelPart[materials.Count];

            Short3[] allTriangles = ConvertFBXTriangles();

            List<List<Short3>> partTriangles = new List<List<Short3>>();

            for (int i = 0; i != lod.Parts.Length; i++)
                partTriangles.Add(new List<Short3>());

            if (geometry.LayerMaterial.MappingInformationType == "AllSame")
            {
                partTriangles[0].AddRange(allTriangles.ToList());
            }
            else
            {
                for (int i = 0; i != allTriangles.Length; i++)
                    partTriangles[geometry.LayerMaterial.Materials[i]].Add(allTriangles[i]);
            }

            for (int i = 0; i != lod.Parts.Length; i++)
            {
                string name = materials.ElementAt(i).Value.Name.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1];
                lod.Parts[i] = new ModelPart();

                //quick fix if .dds exists.
                if (name.EndsWith(".dds"))
                    name.Remove(name.Length - 4, 4);

                lod.Parts[i].Material = name;
                lod.Parts[i].Indices = partTriangles[i].ToArray();
                lod.Parts[i].CalculatePartBounds(lod.Vertices);
            }
            
            return lod;
        }

        private Vertex[] ConvertFBXVertices()
        {
            Vertex[] vertices = new Vertex[geometry.Vertices.Length/3];
            
            int vertIndex = 0;
            int normalIndex = 0;
            int uvIndex = 0;
            for(int i = 0; i != vertices.Length; i++)
            {
                vertices[i] = new Vertex();
                vertices[i].Position.X = Convert.ToSingle(geometry.Vertices[vertIndex]);
                vertices[i].Position.Y = Convert.ToSingle(geometry.Vertices[++vertIndex]);
                vertices[i].Position.Z = Convert.ToSingle(geometry.Vertices[++vertIndex]);
                if (lod.VertexDeclaration.HasFlag(VertexFlags.Normals))
                {
                    vertices[i].Normal.X = Convert.ToSingle(geometry.LayerNormal.Normals[normalIndex]);
                    vertices[i].Normal.Y = Convert.ToSingle(geometry.LayerNormal.Normals[++normalIndex]);
                    vertices[i].Normal.Z = Convert.ToSingle(geometry.LayerNormal.Normals[++normalIndex]);
                }
                if (lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                {
                    vertices[i].UVs[0].X = HalfHelper.SingleToHalf(Convert.ToSingle(geometry.LayerUV.UVs[uvIndex]));
                    vertices[i].UVs[0].Y = HalfHelper.SingleToHalf(Convert.ToSingle(geometry.LayerUV.UVs[++uvIndex]));
                }
                vertIndex++;
                normalIndex++;
                uvIndex++;
            }
            return vertices;
        }

        private Short3[] ConvertFBXTriangles()
        {
            Short3[] triangles = new Short3[geometry.Triangles.Length / 3];

            int triangleIndex = 0;
            for (int i = 0; i != triangles.Length; i++)
            {
                triangles[i] = new Short3();
                triangles[i].S1 = Convert.ToUInt16(geometry.Triangles[triangleIndex]);
                triangles[i].S2 = Convert.ToUInt16(geometry.Triangles[++triangleIndex]);
                triangles[i].S3 = Convert.ToUInt16(~geometry.Triangles[++triangleIndex]);
                triangleIndex++;
            }

            return triangles;
        }
    }
}
