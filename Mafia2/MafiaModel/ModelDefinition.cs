using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.Illusion.FileFormats.Hashing;

namespace Mafia2
{
    public class Model
    {
        
        FrameObjectSingleMesh frameMesh; //model can be either "FrameObjectSingleMesh"
        FrameObjectModel frameModel; //Or "FrameObjectModel"
        FrameGeometry frameGeometry; //Holds geometry data, all content is built into here.
        FrameMaterial frameMaterial; //Data related to material goes into here.
        IndexBuffer[] indexBuffers; //Holds the buffer which will then be saved/replaced later
        VertexBuffer[] vertexBuffers; //Holds the buffers which will then be saved/replaced later
        M2TStructure model; //split from this file; it now includes M2T format.
        private bool useSingleMesh; //False means ModelMesh, True means SingleMesh;
        
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

        public M2TStructure ModelStructure {
            get { return model; }
            set { model = value; }
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
            model = new M2TStructure();
            model.IsSkinned = false;
            model.Name = frameMesh.Name.String;
            model.BuildLods(frameGeometry, frameMaterial, vertexBuffers, indexBuffers);
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
            model = new M2TStructure();
            model.IsSkinned = true;
            model.Name = frameMesh.Name.String;
            model.BuildLods(frameGeometry, frameMaterial, vertexBuffers, indexBuffers);
        }

        /// <summary>
        /// Construct an empty model.
        /// </summary>
        public Model()
        {
            ModelStructure = new M2TStructure();
        }

        /// <summary>
        /// Update decompression offset and position.
        /// </summary>
        public void CalculateDecompression()
        {
            float minFloatf = 0.000016f;
            Vector3 minFloat = new Vector3(minFloatf);

            BoundingBox bounds = new BoundingBox();
            bounds.Min = frameMesh.Boundings.Min - minFloat;
            bounds.Max = frameMesh.Boundings.Max + minFloat;
            frameGeometry.DecompressionOffset = bounds.Min;

            double MaxX = bounds.Max.X - bounds.Min.X + minFloatf;
            double MaxY = bounds.Max.Y - bounds.Min.Y + minFloatf;
            double MaxZ = bounds.Max.Z - bounds.Min.Z + minFloatf;

            double fMaxSize = Math.Max(MaxX, Math.Max(MaxY, MaxZ * 2.0f));

            //todo fix decompression factors.
            Console.WriteLine("Decompress value before: " + fMaxSize);

            if (fMaxSize <= 16)
                frameGeometry.DecompressionFactor = (float)16 / 0x10000;
            else if (fMaxSize <= 256)
                frameGeometry.DecompressionFactor = (float)256 / 0x10000;
            else if(fMaxSize <= 512)
                frameGeometry.DecompressionFactor = (float)512 / 0x10000;
            else if(fMaxSize <= 1024)
                frameGeometry.DecompressionFactor = (float)1024 / 0x10000;
            else if (fMaxSize <= 2048)
                frameGeometry.DecompressionFactor = (float)2048 / 0x10000;
            else if (fMaxSize <= 4196)
                frameGeometry.DecompressionFactor = (float)4196 / 0x10000;
            else if (fMaxSize <= 8392)
                frameGeometry.DecompressionFactor = (float)8392 / 0x10000;
            else if (fMaxSize <= 16784)
                frameGeometry.DecompressionFactor = (float)16784 / 0x10000;
            else
                frameGeometry.DecompressionFactor = (float)fMaxSize / 0x10000;

            Console.WriteLine("Using decompression value from: " + fMaxSize + " result is: " + frameGeometry.DecompressionFactor);
        }

        /// <summary>
        /// Builds Index buffer from the mesh data.
        /// </summary>
        public void BuildIndexBuffer(string name = null)
        {
            if (model.Lods == null)
                return;

            List<ushort> idata = new List<ushort>();

            //todo; allow more LODS.
            for (int i = 0; i != model.Lods[0].Parts.Length; i++)
            {
                for (int x = 0; x != model.Lods[0].Parts[i].Indices.Length; x++)
                {
                    idata.Add(model.Lods[0].Parts[i].Indices[x].S1);
                    idata.Add(model.Lods[0].Parts[i].Indices[x].S2);
                    idata.Add(model.Lods[0].Parts[i].Indices[x].S3);
                }
            }

            if (name != null)
                IndexBuffers[0] = new IndexBuffer(FNV64.Hash(name));
            else
                IndexBuffers[0] = new IndexBuffer(frameGeometry.LOD[0].IndexBufferRef.uHash);

            indexBuffers[0].Data = idata.ToArray();
        }

        /// <summary>
        /// Builds vertex buffer from the mesh data.
        /// </summary>
        public void BuildVertexBuffer(string name = null)
        {
            if (model.Lods == null)
                return;

            for (int i = 0; i != model.Lods.Length; i++)
            {
                FrameLOD frameLod = frameGeometry.LOD[i];
                int vertexSize;
                Dictionary<VertexFlags, FrameLOD.VertexOffset> vertexOffsets = frameLod.GetVertexOffsets(out vertexSize);
                byte[] vBuffer = new byte[vertexSize * frameLod.NumVertsPr];

                for (int v = 0; v != model.Lods[i].Vertices.Length; v++)
                {
                    Vertex vert = model.Lods[i].Vertices[v];

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Position))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Position].Offset;
                        vert.WritePositionData(vBuffer, startIndex, frameGeometry.DecompressionFactor, frameGeometry.DecompressionOffset);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Tangent))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Position].Offset;
                        vert.WriteTangentData(vBuffer, startIndex);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Normals].Offset;
                        vert.WriteNormalData(vBuffer, startIndex);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords0].Offset;
                        vert.WriteUvData(vBuffer, startIndex, 0);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords1].Offset;
                        vert.WriteUvData(vBuffer, startIndex, 1);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords2].Offset;
                        vert.WriteUvData(vBuffer, startIndex, 2);
                    }

                    if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords7))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords7].Offset;
                        vert.WriteUvData(vBuffer, startIndex, 3);
                    }

                }

                if (name != null)
                    VertexBuffers[0] = new VertexBuffer(FNV64.Hash(name));
                else
                    VertexBuffers[0] = new VertexBuffer(frameLod.VertexBufferRef.uHash);

                VertexBuffers[0].Data = vBuffer;
            }
        }

        /// <summary>
        /// Update all objects from loaded model.
        /// </summary>
        public void UpdateObjectsFromModel()
        {
            int totalFaces = 0;

            for (int i = 0; i != model.Lods[0].Parts.Length; i++)
                totalFaces += model.Lods[0].Parts[i].Indices.Length;

            frameGeometry.NumLods = 1;

            if (frameGeometry.LOD == null)
            {
                frameGeometry.LOD = new FrameLOD[model.Lods.Length];
                frameGeometry.LOD[0] = new FrameLOD();
            }

            frameGeometry.LOD[0].Distance = 1E+12f;
            frameGeometry.LOD[0].BuildNewPartition();
            frameGeometry.LOD[0].BuildNewMaterialSplit();
            frameGeometry.LOD[0].SplitInfo.NumVerts = model.Lods[0].Vertices.Length;
            frameGeometry.LOD[0].NumVertsPr = model.Lods[0].Vertices.Length;
            frameGeometry.LOD[0].SplitInfo.NumFaces = totalFaces;
            frameGeometry.LOD[0].VertexDeclaration = model.Lods[0].VertexDeclaration;

            //burst split info.
            frameGeometry.LOD[0].SplitInfo.NumMatSplit = model.Lods[0].Parts.Length;
            frameGeometry.LOD[0].SplitInfo.NumMatBurst = model.Lods[0].Parts.Length;
            frameGeometry.LOD[0].SplitInfo.MaterialSplits = new FrameLOD.MaterialSplit[model.Lods[0].Parts.Length];
            frameGeometry.LOD[0].SplitInfo.MaterialBursts = new FrameLOD.MaterialBurst[model.Lods[0].Parts.Length];

            int faceIndex = 0;
            int baseIndex = 0;
            frameMaterial.NumLods = 1;
            frameMaterial.LodMatCount = new int[model.Lods.Length];
            frameMaterial.Materials = new List<MaterialStruct[]>();
            FrameMaterial.Materials.Add(new MaterialStruct[frameMaterial.LodMatCount[0]]);
            frameMaterial.LodMatCount[0] = model.Lods[0].Parts.Length;
            frameMaterial.Materials[0] = new MaterialStruct[model.Lods[0].Parts.Length];
            for (int i = 0; i != model.Lods[0].Parts.Length; i++)
            {
                frameMaterial.Materials[0][i] = new MaterialStruct();
                frameMaterial.Materials[0][i].StartIndex = faceIndex;
                frameMaterial.Materials[0][i].NumFaces = model.Lods[0].Parts[i].Indices.Length;
                frameMaterial.Materials[0][i].Unk3 = 0;
                frameMaterial.Materials[0][i].MaterialHash = model.Lods[0].Parts[i].Hash;
                frameMaterial.Materials[0][i].MaterialName = model.Lods[0].Parts[i].Material;
                faceIndex += model.Lods[0].Parts[i].Indices.Length * 3;

                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].Bounds = new short[6]
                {
                    Convert.ToInt16(model.Lods[0].Parts[i].Bounds.Min.X),
                    Convert.ToInt16(model.Lods[0].Parts[i].Bounds.Min.Y),
                    Convert.ToInt16(model.Lods[0].Parts[i].Bounds.Min.Z),
                    Convert.ToInt16(model.Lods[0].Parts[i].Bounds.Max.X),
                    Convert.ToInt16(model.Lods[0].Parts[i].Bounds.Max.Y),
                    Convert.ToInt16(model.Lods[0].Parts[i].Bounds.Max.Z)

                };

                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].FirstIndex = 0;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].LeftIndex = -1;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].RightIndex = -1;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].SecondIndex =
                    Convert.ToInt16(model.Lods[0].Parts[i].Indices.Length - 1);
                frameGeometry.LOD[0].SplitInfo.MaterialSplits[i].BaseIndex = baseIndex;
                frameGeometry.LOD[0].SplitInfo.MaterialSplits[i].FirstBurst = i;
                frameGeometry.LOD[0].SplitInfo.MaterialSplits[i].NumBurst = 1;
                baseIndex += faceIndex;
            }
        }

        /// <summary>
        /// Create objects from model. Requires FrameMesh/FrameModel to be already set and a model already read into the data.
        /// </summary>
        public void CreateObjectsFromModel()
        {
            frameGeometry = new FrameGeometry();
            frameMaterial = new FrameMaterial();
          

            //set lods for all data.
            indexBuffers = new IndexBuffer[model.Lods.Length];
            vertexBuffers = new VertexBuffer[model.Lods.Length];

            List<Vertex[]> vertData = new List<Vertex[]>();
            for (int i = 0; i != model.Lods.Length; i++)
                vertData.Add(model.Lods[i].Vertices);

            frameMesh.Boundings = new BoundingBox();
            frameMesh.Boundings.CalculateBounds(vertData);
            frameMaterial.Bounds = FrameMesh.Boundings;
            CalculateDecompression();
            UpdateObjectsFromModel();
            BuildIndexBuffer("M2TK." + model.Name + ".IB0");
            BuildVertexBuffer("M2TK." + model.Name + ".VB0");
            frameGeometry.LOD[0].IndexBufferRef = new Hash("M2TK." + model.Name + ".IB0");
            frameGeometry.LOD[0].VertexBufferRef = new Hash("M2TK." + model.Name + ".VB0");
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
