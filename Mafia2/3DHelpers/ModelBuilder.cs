using System;
using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class Model
    {
        Lod[] lods;
        string path;
        FrameObjectSingleMesh meshBlock;

        public Lod[] Lods {
            get { return lods; }
            set { lods = value; }
        }

        public Model(FrameObjectSingleMesh singleMesh, VertexBufferPool vertexBufferPool, IndexBufferPool indexBufferPool, FrameResource frameResource)
        {
            Build(singleMesh, vertexBufferPool, indexBufferPool, frameResource);
        }

        public void Build(FrameObjectSingleMesh singleMesh, VertexBufferPool vertexBufferPool, IndexBufferPool indexBufferPool, FrameResource frameResource)
        {
            meshBlock = singleMesh;
            path = singleMesh.Name.Name;
            FrameGeometry mesh = (frameResource.FrameBlocks[singleMesh.MeshIndex] as FrameGeometry);
            if (mesh == null)
                return;

            lods = new Lod[mesh.LOD.Length];

            for (int i = 0; i != lods.Length; i++)
            {
                FrameLOD lod1 = mesh.LOD[i];
                Lod lod2 = new Lod();
                bool flag = lod1.VertexDeclaration.HasFlag(VertexFlags.BlendData);
                //add blenddata checker.
                VertexBuffer vertexBuffer = GetVertexBuffer(vertexBufferPool, lod1.VertexBufferRef.uHash);
                int stride;
                Dictionary<VertexFlags, FrameLOD.VertexOffset> vertexOffsets = lod1.GetVertexOffsets(out stride);
                int length1 = 0;
                if (lod1.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    length1++;
                if (lod1.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    length1++;
                if (lod1.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    length1++;
                if (lod1.VertexDeclaration.HasFlag(VertexFlags.TexCoords7))
                    length1++;
                lod2.NumUVChannels = length1;
                lod2.Vertices = new Vertex[lod1.NumVertsPr];
                for (int v = 0; v != lod1.NumVertsPr; v++)
                {
                    int num1 = 0;
                    Vertex vertex = new Vertex();
                    vertex.UVs = new UVVector2[length1];
                    float num2 = 1f;
                    if (lod1.VertexDeclaration.HasFlag(VertexFlags.Position))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Position].Offset;
                        ushort uint16_1 = BitConverter.ToUInt16(vertexBuffer.Data, startIndex);
                        ushort uint16_2 = BitConverter.ToUInt16(vertexBuffer.Data, startIndex + 2);
                        ushort num3 = (ushort)((uint)BitConverter.ToUInt16(vertexBuffer.Data, startIndex + 4) & short.MaxValue);
                        Vector3 vector3 = new Vector3(uint16_1 * mesh.PositionFactor, uint16_2 * mesh.PositionFactor, num3 * mesh.PositionFactor);
                        vector3 += mesh.PositionOffset;
                        vertex.Position = vector3;
                    }
                    if (lod1.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords0].Offset;
                        vertex.UVs[num1] = new UVVector2(Half.ToHalf(vertexBuffer.Data, startIndex), Half.ToHalf(vertexBuffer.Data, startIndex + 2));
                        num1++;
                    }
                    if (lod1.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords1].Offset;
                        vertex.UVs[num1] = new UVVector2(Half.ToHalf(vertexBuffer.Data, startIndex), Half.ToHalf(vertexBuffer.Data, startIndex + 2));
                        num1++;
                    }
                    if (lod1.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords2].Offset;
                        vertex.UVs[num1] = new UVVector2(Half.ToHalf(vertexBuffer.Data, startIndex), Half.ToHalf(vertexBuffer.Data, startIndex + 2));
                        num1++;
                    }
                    if (lod1.VertexDeclaration.HasFlag(VertexFlags.TexCoords7))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords7].Offset;
                        vertex.UVs[num1] = new UVVector2(Half.ToHalf(vertexBuffer.Data, startIndex), Half.ToHalf(vertexBuffer.Data, startIndex + 2));
                        num1++;
                    }
                    if (lod1.VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        int index2 = i * stride + vertexOffsets[VertexFlags.Normals].Offset;
                        vertex.Normal = new Vector3(vertexBuffer.Data[index2] - sbyte.MaxValue, vertexBuffer.Data[index2 + 1] - sbyte.MaxValue, vertexBuffer.Data[index2 + 2] - sbyte.MaxValue);
                        vertex.Normal.Normalize();
                    }
                    //if (lod1.VertexDeclaration.HasFlag(VertexFlags.Tangent)) {
                    //    lod2.NormalMapInfoPresent = true;
                    //    int num3 = i * stride + vertexOffsets[VertexFlags.Position].Offset;
                    //    num2 = (BitConverter.ToUInt16(vertexBuffer.Data, num3 + 4) & 32768) == 32768 ? 1f : -1f;
                    //    byte num4 = vertexBuffer.Data[num3 + 6];
                    //    byte num5 = vertexBuffer.Data[num3 + 7];
                    //    int num6 = i * stride + vertexOffsets[VertexFlags.Normals].Offset;
                    //    byte num7 = vertexBuffer.Data[num6 + 3];
                    //    vertex.Tangent = new Vector3(num4 - sbyte.MaxValue, num5 - sbyte.MaxValue, num6 - sbyte.MaxValue);
                    //    vertex.Tangent.Normalize();
                    //}
                    //if (lod1.VertexDeclaration.HasFlag(VertexFlags.BlendData)) {
                    //    throw new Exception("Need to do this.");
                    //}
                    lod2.Vertices[v] = vertex;
                }
                IndexBuffer indexBuffer = GetIndexBuffer(indexBufferPool, lod1.IndexBufferRef.uHash);
                MaterialStruct[] materials = (frameResource.FrameBlocks[singleMesh.MaterialIndex] as FrameMaterial).Materials[i];
                lod2.Parts = new ModelPart[materials.Length];
                for (int x = 0; x != materials.Length; x++)
                {
                    ModelPart modelPart = new ModelPart();
                    modelPart.Material = MaterialsParse.LookupMaterialByHash(materials[x].MaterialHash);
                    int num = materials[x].StartIndex + materials[x].NumFaces * 3;
                    List<Short3> intList = new List<Short3>(materials[x].NumFaces);
                    int startIndex = materials[x].StartIndex;
                    while (startIndex < num)
                    {
                        Short3 indice = new Short3();
                        indice.s1 = (short)indexBuffer.Data[startIndex + 0];
                        indice.s2 = (short)indexBuffer.Data[startIndex + 1];
                        indice.s3 = (short)indexBuffer.Data[startIndex + 2];
                        intList.Add(indice);
                        startIndex += 3;
                    }
                    modelPart.Indices = intList.ToArray();

                    lod2.Parts[x] = modelPart;
                }
                lods[i] = lod2;
            }
        }

        public void ExportToOBJ(Lod lod, string name)
        {
            List<string> newOBJ = new List<string>();
            List<string> newMTL = new List<string>();

            newOBJ.Add(string.Format("mtllib {0}", name + ".mtl"));
            newOBJ.Add("");
            for (int i = 0; i != lod.Vertices.Length; i++)
            {
                newOBJ.Add(string.Format("v {0} {1} {2}", lod.Vertices[i].Position.X, lod.Vertices[i].Position.Y, lod.Vertices[i].Position.Z));
            }
            newOBJ.Add("");
            for (int i = 0; i != lod.Vertices.Length; i++)
            {
                newOBJ.Add(string.Format("vt {0} {1} {2}", lod.Vertices[i].UVs[0].X, 1 - lod.Vertices[i].UVs[0].Y, 0));
            }
            newOBJ.Add("");
            for (int i = 0; i != lod.Vertices.Length; i++)
            {
                newOBJ.Add(string.Format("vn {0} {1} {2}", lod.Vertices[i].Normal.X, lod.Vertices[i].Normal.Y, lod.Vertices[i].Normal.Z));
            }
            newOBJ.Add("");
            for (int i = 0; i != lod.Parts.Length; i++)
            {
                newMTL.Add(string.Format("newmtl {0}", lod.Parts[i].Material));
                newMTL.Add(string.Format("kd 0.6 0.6 0.6"));
                newMTL.Add(string.Format("ka 0 0 0"));
                newMTL.Add(string.Format("ks 0 0 0"));
                newMTL.Add(string.Format("Ns 16"));
                newMTL.Add(string.Format("illum 2"));
                newMTL.Add(string.Format("map_kd {0}", lod.Parts[i].Material));
                newMTL.Add("");

                newOBJ.Add(string.Format("g part{0}", i));
                newOBJ.Add(string.Format("usemtl {0}", lod.Parts[i].Material));
                for (int c = 0; c != lod.Parts[i].Indices.Length; c++)
                {
                    newOBJ.Add(string.Format("f {0}/{0} {1}/{1} {2}/{2}", lod.Parts[i].Indices[c].s1 + 1, lod.Parts[i].Indices[c].s2 + 1, lod.Parts[i].Indices[c].s3 + 1));
                }
                newOBJ.Add("");
            }

            File.WriteAllLines("objects/" + name + ".obj", newOBJ);
            File.WriteAllLines("objects/" + name + ".mtl", newMTL);

        }

        public VertexBuffer GetVertexBuffer(VertexBufferPool vBufferPool, ulong hash)
        {
            foreach (VertexBuffer vBuffer in vBufferPool.Buffers)
            {
                if (vBuffer.Hash == hash)
                    return vBuffer;
            }
            return null;
        }
        public IndexBuffer GetIndexBuffer(IndexBufferPool iBufferPool, ulong hash)
        {
            foreach (IndexBuffer iBuffer in iBufferPool.Buffers)
            {
                if (iBuffer.Hash == hash)
                    return iBuffer;
            }
            return null;
        }
    }

    public struct Vertex
    {
        Vector3 position;
        Vector3 normal;
        Vector3 tangent;
        Vector3 binormal;

        float[] blendWeights;
        byte[] blendIndices;
        int[] JointIndices;
        UVVector2[] uvs;

        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }
        public Vector3 Normal {
            get { return normal; }
            set { normal = value; }
        }
        public Vector3 Tangent {
            get { return tangent; }
            set { tangent = value; }
        }
        public UVVector2[] UVs {
            get { return uvs; }
            set { uvs = value; }
        }

        public override string ToString()
        {
            return string.Format(position.ToString());
        }
    }

    public struct ModelPart
    {
        string material;
        Short3[] indices;

        public string Material {
            get { return material; }
            set { material = value; }
        }
        public Short3[] Indices {
            get { return indices; }
            set { indices = value; }
        }
    }

    public struct Short3
    {
        public short s1;
        public short s2;
        public short s3;
    }

    public class Lod
    {
        Vertex[] vertices;
        int numUVChannels;
        bool normalMapInfoPresent;
        ModelPart[] parts;

        public Vertex[] Vertices {
            get { return vertices; }
            set { vertices = value; }
        }
        public int NumUVChannels {
            get { return numUVChannels; }
            set { numUVChannels = value; }
        }
        public bool NormalMapInfoPresent {
            get { return normalMapInfoPresent; }
            set { normalMapInfoPresent = value; }
        }
        public ModelPart[] Parts {
            get { return parts; }
            set { parts = value; }
        }
        //ADD SKELETON

    }
}
