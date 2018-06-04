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
                    if(lod1.VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Normals].Offset;
                        float x = (vertexBuffer.Data[startIndex] - sbyte.MaxValue)* 0.007874f;
                        float y = (vertexBuffer.Data[startIndex+1] - sbyte.MaxValue) * 0.007874f;
                        float z = (vertexBuffer.Data[startIndex+2] - sbyte.MaxValue) * 0.007874f;
                        vertex.Normal = new Vector3(x, y, z);
                        vertex.Normal.Normalize();
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
                CustomEDM[] EDMs = new CustomEDM[lod.Parts.Length];

                writer.Write(name);
                writer.Write(EDMs.Length);

                for (int i = 0; i != EDMs.Length; i++)
                {
                    #region convert
                    List<short> vertlist = new List<short>();
                    foreach (Short3 s3 in lod.Parts[i].Indices)
                    {
                        vertlist.Add(s3.s1);
                        vertlist.Add(s3.s2);
                        vertlist.Add(s3.s3);
                    }
                    List<Vertex> newVerts = new List<Vertex>();
                    List<short> newFacesI = new List<short>();
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
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
                    List<Short3> newShort3 = new List<Short3>();
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

                    EDMs[i] = new CustomEDM(newVerts, newShort3, lod.Parts[i].Material);

                    writer.Write(EDMs[i].Name);
                    writer.Write(EDMs[i].Vertices.Length);
                    for (int c = 0; c != EDMs[i].Vertices.Length; c++)
                    {
                        writer.Write(EDMs[i].Vertices[c].X);
                        writer.Write(EDMs[i].Vertices[c].Y);
                        writer.Write(EDMs[i].Vertices[c].Z);
                    }
                    writer.Write(EDMs[i].UVs.Length);
                    for (int c = 0; c != EDMs[i].UVs.Length; c++)
                    {
                        writer.Write((float)EDMs[i].UVs[c].X);
                        writer.Write((float)1f-EDMs[i].UVs[c].Y);
                    }
                    writer.Write(EDMs[i].Indices.Count);
                    for (int c = 0; c != EDMs[i].Indices.Count; c++)
                    {
                        writer.Write(EDMs[i].Indices[c].s1 + 1);
                        writer.Write(EDMs[i].Indices[c].s2 + 1);
                        writer.Write(EDMs[i].Indices[c].s3 + 1);
                    }
                }
            }
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

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", s1, s2, s3);
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
