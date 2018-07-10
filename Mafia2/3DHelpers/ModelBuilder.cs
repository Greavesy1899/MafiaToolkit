using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Model(FrameObjectSingleMesh singleMesh, VertexBufferManager vertexBufferPool, IndexBufferManager indexBufferPool, FrameResource frameResource)
        {
            Build(singleMesh, vertexBufferPool, indexBufferPool, frameResource);
        }

        public void Build(FrameObjectSingleMesh singleMesh, VertexBufferManager vertexBufferPool, IndexBufferManager indexBufferPool, FrameResource frameResource)
        {
            meshBlock = singleMesh;
            path = singleMesh.Name.String;
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
                        vertex.ReadPositionData(vertexBuffer.Data, startIndex, mesh.PositionFactor, mesh.PositionOffset);
                    }
                    if (lod1.VertexDeclaration.HasFlag(VertexFlags.Tangent))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Position].Offset;
                        vertex.ReadTangentData(vertexBuffer.Data, startIndex);
                    }
                    if (lod1.VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Normals].Offset;
                        vertex.ReadNormalData(vertexBuffer.Data, startIndex);
                    }
                    if(lod1.VertexDeclaration.HasFlag(VertexFlags.BlendData))
                    {
                        int startIndex = v * stride +vertexOffsets[VertexFlags.BlendData].Offset;
                        vertex.BlendWeight = (BitConverter.ToSingle(vertexBuffer.Data, startIndex) / byte.MaxValue);
                        vertex.BoneID = BitConverter.ToInt32(vertexBuffer.Data, startIndex+4);
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
                    if (lod2.NormalMapInfoPresent)
                    {
                        vertex.Binormal = vertex.Normal;
                        vertex.Binormal.CrossProduct(vertex.Tangent);
                        vertex.Binormal *= 2;
                        vertex.Binormal.Normalize();
                    }
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
            WavefrontOBJ objMesh = new WavefrontOBJ(lod.Vertices, lod.Parts, name);
            objMesh.ExportOBJ();
        }

        public void ExportToEDM(Lod lod, string name)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/" + name + ".edm")))
            {
                CustomEDM EDM = new CustomEDM(name, lod.Parts.Length);

                for (int i = 0; i != EDM.PartCount; i++)
                {
                    #region convert
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    List<short> vertlist = new List<short>();
                    for (int x = 0; x != lod.Parts[i].Indices.Length; x++)
                    {
                        vertlist.Add(lod.Parts[i].Indices[x].s1);
                        vertlist.Add(lod.Parts[i].Indices[x].s2);
                        vertlist.Add(lod.Parts[i].Indices[x].s3);
                    }
                    List<Vertex> newVerts = new List<Vertex>();
                    List<short> newFacesI = new List<short>();
                    List<Short3> newShort3 = new List<Short3>();

                    foreach (short s in vertlist)
                    {
                        if (!newVerts.Contains(lod.Vertices[s]))
                        {
                            newVerts.Add(lod.Vertices[s]);
                            newFacesI.Add((short)newVerts.IndexOf(lod.Vertices[s]));
                        }
                        else
                        {
                            newFacesI.Add((short)newVerts.IndexOf(lod.Vertices[s]));
                        }
                    }
                    
                    int num = 0;
                    while (num != newFacesI.Count)
                    {
                        Short3 face = new Short3();
                        face.s1 = newFacesI[num];
                        num++;
                        face.s2 = newFacesI[num];
                        num++;
                        face.s3 = newFacesI[num];
                        num++;
                        newShort3.Add(face);
                    }
                    watch.Stop();
                    Console.WriteLine("{0}", watch.Elapsed);
                    #endregion

                    EDM.AddPart(newVerts, newShort3, lod.Parts[i].Material, i);
                }
                EDM.WriteToFile(writer);
            }
        }

        public VertexBuffer GetVertexBuffer(VertexBufferManager vBufferManager, ulong indexRef)
        {
            for (int i = 0; i != vBufferManager.BufferPools.Length; i++)
            {
                for (int c = 0; c != vBufferManager.BufferPools[i].Buffers.Length; c++)
                {
                    if (indexRef == vBufferManager.BufferPools[i].Buffers[c].Hash)
                        return vBufferManager.BufferPools[i].Buffers[c];
                }
            }
            return null;
        }
        public IndexBuffer GetIndexBuffer(IndexBufferManager iBufferManager, ulong indexRef)
        {
            for (int i = 0; i != iBufferManager.BufferPools.Length; i++)
            {
                for (int c = 0; c != iBufferManager.BufferPools[i].Buffers.Length; c++)
                {
                    if (indexRef == iBufferManager.BufferPools[i].Buffers[c].Hash)
                        return iBufferManager.BufferPools[i].Buffers[c];
                }
            }
            return null;
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
