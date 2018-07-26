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

        public Lod[] Lods
        {
            get { return lods; }
            set { lods = value; }
        }

        public FrameObjectSingleMesh FrameMesh
        {
            get { return frameMesh; }
            set { frameMesh = value; }
        }

        public FrameObjectModel FrameModel
        {
            get { return frameModel; }
            set { frameModel = value; }
        }

        public FrameGeometry FrameGeometry
        {
            get { return frameGeometry; }
            set { frameGeometry = value; }
        }

        public FrameMaterial FrameMaterial
        {
            get { return frameMaterial; }
            set { frameMaterial = value; }
        }

        public IndexBuffer[] IndexBuffers
        {
            get { return indexBuffers; }
            set { indexBuffers = value; }
        }

        public VertexBuffer[] VertexBuffers
        {
            get { return vertexBuffers; }
            set { vertexBuffers = value; }
        }

        public CustomEDM EDM
        {
            get { return edm; }
            set { edm = value; }
        }

        /// <summary>
        /// Constructor used to build Lods. This is used when you want to compile all mesh data together, ready for exporting.
        /// </summary>
        public Model(FrameObjectSingleMesh frameMesh, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers,
            FrameGeometry frameGeometry, FrameMaterial frameMaterial)
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
        public Model(FrameObjectModel frameModel, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers,
            FrameGeometry frameGeometry, FrameMaterial frameMaterial)
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
                        vertex.ReadPositionData(vertexBuffer.Data, startIndex, frameGeometry.DecompressionFactor,
                            frameGeometry.DecompressionOffset);
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
                        vertex.ReadBlendData(vertexBuffer.Data, startIndex);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords0].Offset;
                        vertex.ReadUvData(vertexBuffer.Data, startIndex, num1);
                        num1++;
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords1].Offset;
                        vertex.ReadUvData(vertexBuffer.Data, startIndex, num1);
                        num1++;
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords2].Offset;
                        vertex.ReadUvData(vertexBuffer.Data, startIndex, num1);
                        num1++;
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords7))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords7].Offset;
                        vertex.ReadUvData(vertexBuffer.Data, startIndex, num1);
                        num1++;
                    }

                    if (lods[i].NormalMapInfoPresent)
                        vertex.BuildBinormals();

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
                            indice.S1 = (short) indexBuffer.Data[startIndex + 0];
                            indice.S2 = (short) indexBuffer.Data[startIndex + 1];
                            indice.S3 = (short) indexBuffer.Data[startIndex + 2];
                            intList.Add(indice);
                            startIndex += 3;
                        }

                        modelPart.Indices = intList.ToArray();
                        lods[i].Parts[x] = modelPart;
                    }
                }
            }
        }

        public void ExportToM2T()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/" + frameMesh.Name.String + ".m2t")))
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

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Position))
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
                    int totalFaces = 0;
                    foreach (ModelPart part in lods[i].Parts)
                        totalFaces += part.Indices.Length;

                    writer.Write(totalFaces);
                    for (int x = 0; x != frameMaterial.Materials[i].Length; x++)
                    {
                        for (int z = 0; z != lods[i].Parts[x].Indices.Length; z++)
                        {
                            //write triangle, and then material
                            lods[i].Parts[x].Indices[z].WriteToFile(writer);
                            writer.Write((byte) x);
                        }
                    }
                }
            }
        }

        public void ReadFromM2T(BinaryReader reader)
        {
            if (reader.ReadInt32() != 542388813)
                return;

            //mesh name
            reader.ReadString();

            //Number of Lods
            Lods = new Lod[reader.ReadByte()];

            for (int i = 0; i != Lods.Length; i++)
            {
                Lods[i] = new Lod
                {
                    VertexDeclaration = 0
                };

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.Position;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.Normals;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.Tangent;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.BlendData;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.flag_0x80;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.TexCoords0;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.TexCoords1;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.TexCoords2;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.TexCoords7;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.flag_0x20000;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.flag_0x40000;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int) VertexFlags.DamageGroup;

                //write length and then all vertices.
                lods[i].Vertices = new Vertex[reader.ReadInt32()];
                for (int x = 0; x != lods[i].Vertices.Length; x++)
                {
                    Vertex vert = new Vertex();
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Position))
                        vert.Position.ReadfromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Normals))
                        vert.Normal.ReadfromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Tangent))
                        vert.Tangent.ReadfromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        vert.UVs = new UVVector2[1];
                        vert.UVs[0] = new UVVector2();
                        vert.UVs[0].ReadFromFile(reader);
                    }

                    lods[i].Vertices[x] = vert;
                }

                //write mesh count and texture names.
                Lods[i].Parts = new ModelPart[reader.ReadInt32()];
                for (int x = 0; x != Lods[i].Parts.Length; x++)
                {
                    Lods[i].Parts[x] = new ModelPart();
                    Lods[i].Parts[x].Material = reader.ReadString();
                }

                List<List<Short3>> partTriangles = new List<List<Short3>>(Lods[i].Parts.Length);
                for (int x = 0; x != partTriangles.Capacity; x++)
                    partTriangles.Add(new List<Short3>());

                int totalFaces = reader.ReadInt32();
                for (int x = 0; x != totalFaces; x++)
                {
                    Short3 tri = new Short3(reader);
                    byte matId = reader.ReadByte();
                    partTriangles[matId].Add(tri);
                }

                for (int x = 0; x != Lods[i].Parts.Length; x++)
                    Lods[i].Parts[x].Indices = partTriangles[x].ToArray();
            }
        }

        /// <summary>
        /// Calculate new bounds on the model.
        /// </summary>
        public void CalculateBounds()
        {
            Vector3 min = new Vector3(0);
            Vector3 max = new Vector3(0);

            for (int p = 0; p != lods.Length; p++)
            {
                for (int i = 0; i != lods[p].Vertices.Length; i++)
                {
                    Vector3 pos = lods[p].Vertices[i].Position;

                    if (pos.X < min.X)
                        min.X = pos.X;

                    if (pos.X > max.X)
                        max.X = pos.X;

                    if (pos.Y < min.Y)
                        min.Y = pos.Y;

                    if (pos.Y > max.Y)
                        max.Y = pos.Y;

                    if (pos.Z < min.Z)
                        min.Z = pos.Z;

                    if (pos.Z > max.Z)
                        max.Z = pos.Z;
                }
            }
            frameMesh.Boundings = new Bounds(min, max);
        }

        /// <summary>
        /// Update decompression offset and position.
        /// </summary>
        public void CalculateDecompression()
        {
            Bounds bounds = frameMesh.Boundings;

            float minFloatf = 0.000016f;
            Vector3 minFloat = new Vector3(minFloatf);
            bounds.Min -= minFloat;
            bounds.Max += minFloat;

            frameGeometry.DecompressionOffset = bounds.Min;
            float fMaxSize = Math.Max(bounds.Max.X - bounds.Min.X + minFloatf, Math.Max(bounds.Max.Y - bounds.Min.Y + minFloatf, (bounds.Max.Z - bounds.Min.Y + minFloatf) * 2.0f));

            //positionFactor = fMaxSize / 0x10000;
            frameGeometry.DecompressionFactor = (float)256 / 0x10000;
        }

        /// <summary>
        /// Builds Index buffer from the mesh data.
        /// </summary>
        public void BuildIndexBuffer()
        {
            if(Lods == null)
                return;

            List<ushort> idata = new List<ushort>();

            //todo; allow more LODS.
            for (int i = 0; i != Lods[0].Parts.Length; i++)
            {
                for (int x = 0; x != Lods[0].Parts[i].Indices.Length; x++)
                {
                    idata.Add((ushort)Lods[0].Parts[i].Indices[x].S1);
                    idata.Add((ushort)Lods[0].Parts[i].Indices[x].S2);
                    idata.Add((ushort)Lods[0].Parts[i].Indices[x].S3);
                }
            }

            IndexBuffers[0] = new IndexBuffer(frameGeometry.LOD[0].IndexBufferRef.uHash);
            indexBuffers[0].Data = idata.ToArray();
        }

        /// <summary>
        /// Builds vertex buffer from the mesh data.
        /// </summary>
        public void BuildVertexBuffer()
        {
            if (Lods == null)
                return;

            List<byte> vdata = new List<byte>();

            for (int i = 0; i != Lods[0].Vertices.Length; i++)
            {
                Vertex vert = Lods[0].Vertices[i];

                if (Lods[0].VertexDeclaration.HasFlag(VertexFlags.Position))
                    vdata.AddRange(vert.WritePositionData(frameGeometry.DecompressionFactor, frameGeometry.DecompressionOffset));

                if(Lods[0].VertexDeclaration.HasFlag(VertexFlags.Tangent))
                    vdata.AddRange(vert.WriteTangentData());

                if (Lods[0].VertexDeclaration.HasFlag(VertexFlags.Normals))
                    vdata.AddRange(vert.WriteNormalData(Lods[0].VertexDeclaration.HasFlag(VertexFlags.Tangent)));

                if(Lods[0].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    vdata.AddRange(vert.WriteUvData(0));
            }

            VertexBuffers[0] = new VertexBuffer(frameGeometry.LOD[0].VertexBufferRef.uHash);
            VertexBuffers[0].Data = vdata.ToArray();
        }

        public void UpdateObjectsFromModel()
        {
            int totalFaces = 0;

            for (int i = 0; i != Lods[0].Parts.Length; i++)
                totalFaces += Lods[0].Parts[i].Indices.Length;

            frameGeometry.LOD[0].BuildNewPartition();
            frameGeometry.LOD[0].BuildNewMaterialSplit();
            frameGeometry.LOD[0].SplitInfo.NumVerts = Lods[0].Vertices.Length;
            frameGeometry.LOD[0].NumVertsPr = Lods[0].Vertices.Length;
            frameGeometry.LOD[0].SplitInfo.NumFaces = totalFaces;

            //burst split info.
            frameGeometry.LOD[0].SplitInfo.NumMatSplit = Lods[0].Parts.Length;
            frameGeometry.LOD[0].SplitInfo.NumMatBurst = Lods[0].Parts.Length;
            frameGeometry.LOD[0].SplitInfo.MaterialSplits = new FrameLOD.MaterialSplit[Lods[0].Parts.Length];
            frameGeometry.LOD[0].SplitInfo.MaterialBursts = new FrameLOD.MaterialBurst[Lods[0].Parts.Length];

            int faceIndex = 0;
            for (int i = 0; i != Lods[0].Parts.Length; i++)
            {
                frameMaterial.Materials[0][i].StartIndex = faceIndex;
                frameMaterial.Materials[0][i].NumFaces = Lods[0].Parts[i].Indices.Length;
                frameMaterial.Materials[0][i].Unk3 = 0;
                faceIndex = Lods[0].Parts[i].Indices.Length * 3;

                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].Bounds = new short[6]
                {
                    Convert.ToInt16(frameMesh.Boundings.Min.X),
                    Convert.ToInt16(frameMesh.Boundings.Min.Y),
                    Convert.ToInt16(frameMesh.Boundings.Min.Z),
                    Convert.ToInt16(frameMesh.Boundings.Max.X),
                    Convert.ToInt16(frameMesh.Boundings.Max.Y),
                    Convert.ToInt16(frameMesh.Boundings.Max.Z)

                };

                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].FirstIndex = 0;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].LeftIndex = -1;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].RightIndex = -1;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].SecondIndex =
                    Convert.ToUInt16(Lods[0].Parts[i].Indices.Length - 1);
                frameGeometry.LOD[0].SplitInfo.MaterialSplits[i].BaseIndex = faceIndex;
                frameGeometry.LOD[0].SplitInfo.MaterialSplits[i].FirstBurst = i;
                frameGeometry.LOD[0].SplitInfo.MaterialSplits[i].NumBurst = 1;
            }

        }
    }

    public class Lod
    {
        private VertexFlags vertexDeclaration;
        Vertex[] vertices;
        int numUVChannels;
        bool normalMapInfoPresent;
        ModelPart[] parts;

        public VertexFlags VertexDeclaration 
        {
            get { return vertexDeclaration; }
            set { vertexDeclaration = value; }
        }

        public Vertex[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        public int NumUVChannels
        {
            get { return numUVChannels; }
            set { numUVChannels = value; }
        }

        public bool NormalMapInfoPresent
        {
            get { return normalMapInfoPresent; }
            set { normalMapInfoPresent = value; }
        }

        public ModelPart[] Parts
        {
            get { return parts; }
            set { parts = value; }
        }

        //ADD SKELETON
    }

    public class ModelPart
    {
        string material;
        Short3[] indices;

        public string Material
        {
            get { return material; }
            set { material = value; }
        }

        public Short3[] Indices
        {
            get { return indices; }
            set { indices = value; }
        }
    }

    public class BufferLocationStruct
    {
        private int poolLoc;
        private int bufferLoc;

        public int PoolLocation
        {
            get { return poolLoc; }
            set { poolLoc = value; }
        }

        public int BufferLocation
        {
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
