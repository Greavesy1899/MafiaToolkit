using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mafia2
{
    public class Model
    {
        Lod[] lods; //Holds the models which can be exported, all EDM content is saved here.
        FrameObjectSingleMesh frameMesh; //model can be either "FrameObjectSingleMesh"
        FrameObjectModel frameModel; //Or "FrameObjectModel"
        FrameGeometry frameGeometry; //Holds geometry data, all content is built into here.
        FrameMaterial frameMaterial; //Data related to material goes into here.
        IndexBuffer[] indexBuffers; //Holds the buffer which will then be saved/replaced later
        VertexBuffer[] vertexBuffers; //Holds the buffers which will then be saved/replaced later
        private bool useSingleMesh; //False means ModelMesh, True means SingleMesh;
        CustomEDM edm; //mesh stored here. Doesn't store all LODs, just the most recent.

        public Lod[] Lods {
            get { return lods; }
            set { lods = value; }
        }
        public FrameObjectSingleMesh FrameMesh {
            get { return frameMesh; }
            set { frameMesh = value; }
        }
        public FrameObjectModel FrameModel {
            get { return frameModel; }
            set { frameModel = value; }
        }
        public FrameGeometry FrameGeometry {
            get { return frameGeometry; }
            set { frameGeometry = value; }
        }
        public FrameMaterial FrameMaterial {
            get { return frameMaterial; }
            set { frameMaterial = value; }
        }
        public IndexBuffer[] IndexBuffers {
            get { return indexBuffers; }
            set { indexBuffers = value; }
        }
        public VertexBuffer[] VertexBuffers {
            get { return vertexBuffers; }
            set { vertexBuffers = value; }
        }
        public CustomEDM EDM {
            get { return edm; }
            set { edm = value; }
        }

        /// <summary>
        /// Constructor used to build Lods. This is used when you want to compile all mesh data together, ready for exporting.
        /// </summary>
        public Model(FrameObjectSingleMesh frameMesh, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers, FrameGeometry frameGeometry, FrameMaterial frameMaterial)
        {
            this.frameMesh = frameMesh;
            this.indexBuffers = indexBuffers;
            this.vertexBuffers = vertexBuffers;
            this.frameGeometry = frameGeometry;
            this.frameMaterial = frameMaterial;
            this.useSingleMesh = true;

            BuildLods();
        }

        /// <summary>
        /// Constructor used to build Lods. This is used when you want to compile all mesh data together, ready for exporting.
        /// </summary>
        public Model(FrameObjectModel frameModel, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers, FrameGeometry frameGeometry, FrameMaterial frameMaterial)
        {
            this.frameModel = frameModel;
            this.indexBuffers = indexBuffers;
            this.vertexBuffers = vertexBuffers;
            this.frameGeometry = frameGeometry;
            this.frameMaterial = frameMaterial;
            this.useSingleMesh = false;

            BuildLods();
        }

        /// <summary>
        /// Construct an empty model.
        /// </summary>
        public Model()
        {

        }

        /// <summary>
        /// Build Lods from retrieved data.
        /// </summary>
        public void BuildLods()
        {
            lods = new Lod[frameGeometry.NumLods];
            for (int i = 0; i != lods.Length; i++)
            {
                FrameLOD frameLod = frameGeometry.LOD[i];
                lods[i] = new Lod();
                IndexBuffer indexBuffer = indexBuffers[i];
                VertexBuffer vertexBuffer = vertexBuffers[i];

                int stride;
                Dictionary<VertexFlags, FrameLOD.VertexOffset> vertexOffsets = frameLod.GetVertexOffsets(out stride);

                int length1 = 0;
                if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    length1++;
                if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    length1++;
                if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    length1++;
                if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords7))
                    length1++;

                lods[i].NumUVChannels = length1;
                lods[i].Vertices = new Vertex[frameLod.NumVertsPr];

                for (int v = 0; v != lods[i].Vertices.Length; v++)
                {
                    int num1 = 0;
                    Vertex vertex = new Vertex();
                    vertex.UVs = new UVVector2[length1];
                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Position))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Position].Offset;
                        vertex.ReadPositionData(vertexBuffer.Data, startIndex, frameGeometry.DecompressionFactor, frameGeometry.DecompressionOffset);
                    }
                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Tangent))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Position].Offset;
                        vertex.ReadTangentData(vertexBuffer.Data, startIndex);
                    }
                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Normals].Offset;
                        vertex.ReadNormalData(vertexBuffer.Data, startIndex);
                    }
                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.BlendData))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.BlendData].Offset;
                        vertex.BlendWeight = (BitConverter.ToSingle(vertexBuffer.Data, startIndex) / byte.MaxValue);
                        vertex.BoneID = BitConverter.ToInt32(vertexBuffer.Data, startIndex + 4);
                    }
                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords0].Offset;
                        vertex.UVs[num1] = new UVVector2(Half.ToHalf(vertexBuffer.Data, startIndex), Half.ToHalf(vertexBuffer.Data, startIndex + 2));
                        num1++;
                    }
                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords1].Offset;
                        vertex.UVs[num1] = new UVVector2(Half.ToHalf(vertexBuffer.Data, startIndex), Half.ToHalf(vertexBuffer.Data, startIndex + 2));
                        num1++;
                    }
                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords2].Offset;
                        vertex.UVs[num1] = new UVVector2(Half.ToHalf(vertexBuffer.Data, startIndex), Half.ToHalf(vertexBuffer.Data, startIndex + 2));
                        num1++;
                    }
                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords7))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords7].Offset;
                        vertex.UVs[num1] = new UVVector2(Half.ToHalf(vertexBuffer.Data, startIndex), Half.ToHalf(vertexBuffer.Data, startIndex + 2));
                        num1++;
                    }
                    if (lods[i].NormalMapInfoPresent)
                    {
                        vertex.Binormal = vertex.Normal;
                        vertex.Binormal.CrossProduct(vertex.Tangent);
                        vertex.Binormal *= 2;
                        vertex.Binormal.Normalize();
                    }
                    lods[i].Vertices[v] = vertex;

                    MaterialStruct[] materials = frameMaterial.Materials[i];
                    lods[i].Parts = new ModelPart[materials.Length];
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
                            indice.S1 = (short)indexBuffer.Data[startIndex + 0];
                            indice.S2 = (short)indexBuffer.Data[startIndex + 1];
                            indice.S3 = (short)indexBuffer.Data[startIndex + 2];
                            intList.Add(indice);
                            startIndex += 3;
                        }
                        modelPart.Indices = intList.ToArray();
                        lods[i].Parts[x] = modelPart;
                    }
                }
            }
        }

        public void CompileEDM(Lod lod, string name)
        {
            EDM = new CustomEDM(name, lod.Parts.Length);
            Console.WriteLine("Working on " + name);
            try
            {
                for (int i = 0; i != EDM.PartCount; i++)
                {
                    #region convert
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    List<short> vertlist = new List<short>();
                    bool[] hasBeenAdded = new bool[lod.Parts[i].Indices.Length * 3];

                    for (int x = 0; x != lod.Parts[i].Indices.Length; x++)
                    {
                        vertlist.Add(lod.Parts[i].Indices[x].S1);
                        vertlist.Add(lod.Parts[i].Indices[x].S2);
                        vertlist.Add(lod.Parts[i].Indices[x].S3);
                    }
                    List<Vertex> newVerts = new List<Vertex>();
                    List<short> newFacesI = new List<short>();
                    List<Short3> newShort3 = new List<Short3>();

                    int hbaIndex = 0;

                    foreach (short s in vertlist)
                    {
                        if (!hasBeenAdded[hbaIndex])
                        {
                            newVerts.Add(lod.Vertices[s]);
                            newFacesI.Add((short)newVerts.IndexOf(lod.Vertices[s]));
                            hasBeenAdded[hbaIndex] = true;
                        }
                        else
                        {
                            newFacesI.Add((short)newVerts.IndexOf(lod.Vertices[s]));
                        }
                        hbaIndex++;
                    }

                    int num = 0;
                    while (num != newFacesI.Count)
                    {
                        Short3 face = new Short3();
                        face.S1 = newFacesI[num];
                        num++;
                        face.S2 = newFacesI[num];
                        num++;
                        face.S3 = newFacesI[num];
                        num++;
                        newShort3.Add(face);
                    }
                    watch.Stop();
                    Console.WriteLine("{0}", watch.Elapsed);
                    #endregion

                    EDM.AddPart(newVerts, newShort3, lod.Parts[i].Material, i);
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Failed to convert mesh and add it. " + ex.Message);
            }
        }

        public void ExportToEDM(Lod lod, string name)
        {
            //check if edm isn't null.
            if (edm == null)
                CompileEDM(lod, name);

            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/" + name + ".edm")))
            {
                edm.WriteToFile(writer);
            }
        }

        public void ExportToM2T()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/" + "test" + ".m2t")))
            {
                //An absolute overhaul on the mesh exportation.
                //file header; M2T\0
                string header = "M2T ";

                writer.Write(header.ToCharArray());

                //mesh name
                writer.Write(frameMesh.Name.String);

                //Number of Lods
                writer.Write(frameGeometry.NumLods);

                for (int i = 0; i != frameGeometry.NumLods; i++)
                {
                    FrameLOD lod = frameGeometry.LOD[i];

                    //Write section for VertexFlags. 
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.Position));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.Normals));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.Tangent));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.BlendData));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.flag_0x80));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords7));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.flag_0x20000));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.flag_0x40000));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.DamageGroup));

                    //write length and then all vertices.
                    writer.Write(lods[i].Vertices.Length);
                    for (int x = 0; x != lods[i].Vertices.Length; x++)
                    {
                        Vertex vert = lods[i].Vertices[x];

                        if(lod.VertexDeclaration.HasFlag(VertexFlags.Position))
                            vert.Position.WriteToFile(writer);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Normals))
                            vert.Normal.WriteToFile(writer);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Tangent))
                            vert.Tangent.WriteToFile(writer);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                            vert.UVs[0].WriteToFile(writer);
                    }

                    //write mesh count and texture names.
                    writer.Write(frameMaterial.Materials[i].Length);
                    for (int x = 0; x != frameMaterial.Materials[i].Length; x++)
                        writer.Write(lods[i].Parts[x].Material);

                    //write triangle data.
                    for (int x = 0; x != frameMaterial.Materials[i].Length; x++)
                    {
                        writer.Write(lods[i].Parts[x].Indices.Length);
                        for (int z = 0; z != lods[i].Parts[x].Indices.Length; z++)
                        {
                            //write triangle, and then material
                            lods[i].Parts[x].Indices[z].WriteToFile(writer);
                            writer.Write((byte)x);
                        }
                    }
                }
            }
        }

        public void UpdateModelFromEDM()
        {
            int totalVerts = 0;
            int totalFaces = 0;

            for (int i = 0; i != edm.Parts.Length; i++)
            {
                totalVerts += edm.Parts[i].Vertices.Length;
                totalFaces += edm.Parts[i].Indices.Count;
            }

            frameGeometry.LOD[0].BuildNewPartition();
            frameGeometry.LOD[0].BuildNewMaterialSplit();
            frameGeometry.DecompressionFactor = edm.PositionFactor;
            frameGeometry.DecompressionOffset = edm.PositionOffset;
            frameMesh.Boundings = edm.Bound;
            frameGeometry.LOD[0].SplitInfo.NumVerts = totalVerts;
            frameGeometry.LOD[0].NumVertsPr = totalVerts;
            frameGeometry.LOD[0].SplitInfo.NumFaces = totalFaces;

            //burst split info.
            frameGeometry.LOD[0].SplitInfo.NumMatSplit = edm.PartCount;
            frameGeometry.LOD[0].SplitInfo.NumMatBurst = edm.PartCount;
            frameGeometry.LOD[0].SplitInfo.MaterialSplits = new FrameLOD.MaterialSplit[edm.PartCount];
            frameGeometry.LOD[0].SplitInfo.MaterialBursts = new FrameLOD.MaterialBurst[edm.PartCount];

            int faceIndex = 0;
            for (int i = 0; i != edm.PartCount; i++)
            {
                frameMaterial.Materials[0][i].StartIndex = faceIndex;
                frameMaterial.Materials[0][i].NumFaces = edm.Parts[i].Indices.Count;
                frameMaterial.Materials[0][i].Unk3 = 0;
                faceIndex = edm.Parts[0].Indices.Count * 3;

                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].FirstIndex = 0;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].LeftIndex = -1;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].RightIndex = -1;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].SecondIndex = Convert.ToUInt16(edm.Parts[i].Indices.Count - 1);
                frameGeometry.LOD[0].SplitInfo.MaterialSplits[i].BaseIndex = faceIndex;
                frameGeometry.LOD[0].SplitInfo.MaterialSplits[i].FirstBurst = i;
                frameGeometry.LOD[0].SplitInfo.MaterialSplits[i].NumBurst = 1;
            }
            
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

    public class ModelPart
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

    public class BufferLocationStruct
    {
        private int poolLoc;
        private int bufferLoc;

        public int PoolLocation {
            get { return poolLoc; }
            set { poolLoc = value; }
        }
        public int BufferLocation {
            get { return bufferLoc; }
            set { bufferLoc = value; }
        }

        public BufferLocationStruct(int i, int c)
        {
            poolLoc = i;
            bufferLoc = c;
        }
    }
}
