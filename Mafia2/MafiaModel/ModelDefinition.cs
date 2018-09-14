using System;
using System.Collections.Generic;
using System.IO;
using Fbx;
using Gibbed.Illusion.FileFormats.Hashing;

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
        private string name; //name of model.

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

        public string Name
        {
            get { return name; }
            set { name = value; }
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
            name = FrameMesh.Name.String;
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
            name = frameModel.Name.String;
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

                lods[i].NumUVChannels = 4;
                lods[i].Vertices = new Vertex[frameLod.NumVertsPr];

                try
                {
                    for (int v = 0; v != lods[i].Vertices.Length; v++)
                    {
                        Vertex vertex = new Vertex();
                        vertex.UVs = new UVVector2[lods[i].NumUVChannels];
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

                        if(frameLod.VertexDeclaration.HasFlag(VertexFlags.flag_0x80))
                        {
                            Console.WriteLine("Skip vertex with flag_0x80");
                        }

                        if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                        {
                            int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords0].Offset;
                            vertex.ReadUvData(vertexBuffer.Data, startIndex, 0);
                        }

                        if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                        {
                            int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords1].Offset;
                            vertex.ReadUvData(vertexBuffer.Data, startIndex, 1);
                        }

                        if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                        {
                            int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords2].Offset;
                            vertex.ReadUvData(vertexBuffer.Data, startIndex, 2);
                        }

                        if (frameLod.VertexDeclaration.HasFlag(VertexFlags.TexCoords7))
                        {
                            int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords7].Offset;
                            vertex.ReadUvData(vertexBuffer.Data, startIndex, 3);
                        }

                        if (frameLod.VertexDeclaration.HasFlag(VertexFlags.flag_0x20000))
                        {
                            Console.WriteLine("Skip vertex with flag_0x20000");
                        }

                        if (frameLod.VertexDeclaration.HasFlag(VertexFlags.flag_0x40000))
                        {
                            Console.WriteLine("Skip vertex with flag_0x40000");
                        }

                        if (frameLod.VertexDeclaration.HasFlag(VertexFlags.DamageGroup))
                        {
                            Console.WriteLine("Skip vertex with DamageGroup");
                        }

                        if (lods[i].NormalMapInfoPresent)
                            vertex.BuildBinormals();

                        lods[i].Vertices[v] = vertex;

                        MaterialStruct[] materials = frameMaterial.Materials[i];
                        lods[i].Parts = new ModelPart[materials.Length];
                        for (int x = 0; x != materials.Length; x++)
                        {

                            ModelPart modelPart = new ModelPart();
                            modelPart.Material = MaterialsManager.LookupMaterialByHash(materials[x].MaterialHash);
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void ExportToM2T()
        {
            if (!Directory.Exists("exported"))
                Directory.CreateDirectory("Exported");

            if (frameMesh.Name.String == "")
                name = frameGeometry.LOD[0].VertexBufferRef.String;

            if (File.Exists("exported/" + name + ".m2t"))
                return;

            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/" + name + ".m2t")))
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

        public void ExportToFbx()
        {
            if (!Directory.Exists("FBX"))
                Directory.CreateDirectory("FBX");

            FbxWrangler.BuildFBXFromModel(this);
        }

        public void ExportCollisionToM2T(string name)
        {
            if (!Directory.Exists("Collisions"))
                Directory.CreateDirectory("Collisions");

            if (File.Exists("Collisions/" + name + ".m2t"))
                return;

            using (BinaryWriter writer = new BinaryWriter(File.Create("Collisions/" + name + ".m2t")))
            {
                //An absolute overhaul on the mesh exportation.
                //file header; M2T\0
                string header = "M2T ";

                writer.Write(header.ToCharArray());

                //mesh name
                writer.Write(name);

                //Number of Lods
                writer.Write((byte)1);

                for (int i = 0; i != 1; i++)
                {
                    //Write section for VertexFlags. 
                    writer.Write((byte)1);
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                    writer.Write((byte)0);
                    writer.Write((byte)0);

                    //write length and then all vertices.
                    writer.Write(lods[i].Vertices.Length);
                    for (int x = 0; x != lods[i].Vertices.Length; x++)
                    {
                        Vertex vert = lods[i].Vertices[x];

                        vert.Position.WriteToFile(writer);
                    }

                    //write mesh count and texture names.
                    writer.Write(0);

                    //write triangle data.
                    int totalFaces = 0;
                    foreach (ModelPart part in lods[i].Parts)
                        totalFaces += part.Indices.Length;

                    writer.Write(totalFaces);
                    for (int z = 0; z != lods[i].Parts[0].Indices.Length; z++)
                    {
                        //write triangle, and then material
                        lods[i].Parts[0].Indices[z].WriteToFile(writer);
                        writer.Write((byte)0);
                    }
                }
            }
        }

        public void ReadFromM2T(BinaryReader reader)
        {
            if (reader.ReadInt32() != 542388813)
                return;

            //mesh name
            name = reader.ReadString();

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
                lods[i].NumUVChannels = 4;
                for (int x = 0; x != lods[i].Vertices.Length; x++)
                {
                    Vertex vert = new Vertex();
                    vert.UVs = new UVVector2[lods[i].NumUVChannels];

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Position))
                        vert.Position.ReadfromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Normals))
                        vert.Normal.ReadfromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Tangent))
                        vert.Tangent.ReadfromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
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

                //byte[] matIDs = new byte[totalFaces];
                //Short3[] triangles = new Short3[totalFaces];
                //for (int x = 0; x != totalFaces; x++)
                //{
                //    triangles[x] = new Short3(reader);
                //    matIDs[x] = reader.ReadByte();
                //}

                for (int x = 0; x != Lods[i].Parts.Length; x++)
                {
                    Lods[i].Parts[x].Indices = partTriangles[x].ToArray();
                    Lods[i].Parts[x].CalculatePartBounds(lods[i].Vertices);
                }
            }
        }

        /// <summary>
        /// Update decompression offset and position.
        /// </summary>
        public void CalculateDecompression()
        {
            float minFloatf = 0.000016f;
            Vector3 minFloat = new Vector3(minFloatf);

            Bounds bounds = new Bounds();
            bounds.Min = frameMesh.Boundings.Min/* - minFloat*/;
            bounds.Max = frameMesh.Boundings.Max/* + minFloat*/;

            frameGeometry.DecompressionOffset = bounds.Min;
            float MaxX = bounds.Max.X - bounds.Min.X + minFloatf;
            float MaxY = bounds.Max.Y - bounds.Min.Y + minFloatf;
            float MaxZ = bounds.Max.Z - bounds.Min.Z + minFloatf;

            float fMaxSize = Math.Max(MaxX, Math.Max(MaxY, MaxZ * 2.0f));

            Console.WriteLine("Decompress value before: " + fMaxSize);

            if (fMaxSize <= 16)
                frameGeometry.DecompressionFactor = (float)16 / 0x10000;
            else if (fMaxSize <= 256)
                frameGeometry.DecompressionFactor = (float)256 / 0x10000;
            else
                frameGeometry.DecompressionFactor = fMaxSize / 0x10000;

            Console.WriteLine("Using decompression value from: " + fMaxSize + " result is: " + frameGeometry.DecompressionFactor);
        }

        /// <summary>
        /// Builds Index buffer from the mesh data.
        /// </summary>
        public void BuildIndexBuffer(string name = null)
        {
            if (Lods == null)
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

            if (name != null)
                VertexBuffers[0] = new VertexBuffer(FNV64.Hash(name));
            else
                VertexBuffers[0] = new VertexBuffer(frameGeometry.LOD[0].VertexBufferRef.uHash);

            VertexBuffers[0].Data = vdata.ToArray();
        }

        /// <summary>
        /// Update all objects from loaded model.
        /// </summary>
        public void UpdateObjectsFromModel()
        {
            int totalFaces = 0;

            for (int i = 0; i != Lods[0].Parts.Length; i++)
                totalFaces += Lods[0].Parts[i].Indices.Length;

            frameGeometry.NumLods = 1;

            if (frameGeometry.LOD == null)
            {
                frameGeometry.LOD = new FrameLOD[lods.Length];
                frameGeometry.LOD[0] = new FrameLOD();
            }

            frameGeometry.LOD[0].Distance = 1E+12f;
            frameGeometry.LOD[0].BuildNewPartition();
            frameGeometry.LOD[0].BuildNewMaterialSplit();
            frameGeometry.LOD[0].SplitInfo.NumVerts = Lods[0].Vertices.Length;
            frameGeometry.LOD[0].NumVertsPr = Lods[0].Vertices.Length;
            frameGeometry.LOD[0].SplitInfo.NumFaces = totalFaces;
            frameGeometry.LOD[0].VertexDeclaration = lods[0].VertexDeclaration;

            //burst split info.
            frameGeometry.LOD[0].SplitInfo.NumMatSplit = Lods[0].Parts.Length;
            frameGeometry.LOD[0].SplitInfo.NumMatBurst = Lods[0].Parts.Length;
            frameGeometry.LOD[0].SplitInfo.MaterialSplits = new FrameLOD.MaterialSplit[Lods[0].Parts.Length];
            frameGeometry.LOD[0].SplitInfo.MaterialBursts = new FrameLOD.MaterialBurst[Lods[0].Parts.Length];

            int faceIndex = 0;
            int baseIndex = 0;
            frameMaterial.NumLods = 1;
            frameMaterial.LodMatCount = new int[lods.Length];
            frameMaterial.Materials = new List<MaterialStruct[]>();
            FrameMaterial.Materials.Add(new MaterialStruct[frameMaterial.LodMatCount[0]]);
            frameMaterial.LodMatCount[0] = Lods[0].Parts.Length;
            frameMaterial.Materials[0] = new MaterialStruct[Lods[0].Parts.Length];
            for (int i = 0; i != Lods[0].Parts.Length; i++)
            {
                frameMaterial.Materials[0][i] = new MaterialStruct();
                frameMaterial.Materials[0][i].StartIndex = faceIndex;
                frameMaterial.Materials[0][i].NumFaces = Lods[0].Parts[i].Indices.Length;
                frameMaterial.Materials[0][i].Unk3 = 0;
                frameMaterial.Materials[0][i].MaterialHash = Lods[0].Parts[i].Hash;
                frameMaterial.Materials[0][i].MaterialName = Lods[0].Parts[i].Material;
                faceIndex += Lods[0].Parts[i].Indices.Length * 3;

                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].Bounds = new short[6]
                {
                    Convert.ToInt16(Lods[0].Parts[i].Bounds.Min.X),
                    Convert.ToInt16(Lods[0].Parts[i].Bounds.Min.Y),
                    Convert.ToInt16(Lods[0].Parts[i].Bounds.Min.Z),
                    Convert.ToInt16(Lods[0].Parts[i].Bounds.Max.X),
                    Convert.ToInt16(Lods[0].Parts[i].Bounds.Max.Y),
                    Convert.ToInt16(Lods[0].Parts[i].Bounds.Max.Z)

                };

                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].FirstIndex = 0;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].LeftIndex = -1;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].RightIndex = -1;
                frameGeometry.LOD[0].SplitInfo.MaterialBursts[i].SecondIndex =
                    Convert.ToUInt16(Lods[0].Parts[i].Indices.Length - 1);
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
            indexBuffers = new IndexBuffer[lods.Length];
            vertexBuffers = new VertexBuffer[lods.Length];

            List<Vertex[]> vertData = new List<Vertex[]>();
            for (int i = 0; i != Lods.Length; i++)
                vertData.Add(Lods[i].Vertices);

            frameMesh.Boundings = new Bounds();
            frameMesh.Boundings.CalculateBounds(vertData);
            frameMaterial.Bounds = FrameMesh.Boundings;
            CalculateDecompression();
            BuildIndexBuffer("M2TK." + name + ".IB0");
            BuildVertexBuffer("M2TK." + name + ".VB0");
            UpdateObjectsFromModel();
            frameGeometry.LOD[0].IndexBufferRef = new Hash("M2TK." + name + ".IB0");
            frameGeometry.LOD[0].VertexBufferRef = new Hash("M2TK." + name + ".VB0");
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
        ulong hash;
        Short3[] indices;
        Bounds bounds;

        public string Material
        {
            get { return material; }
            set {
                material = value;
                hash = FNV64.Hash(value);
            }
        }

        public ulong Hash {
            get { return hash; }
        }

        public Short3[] Indices
        {
            get { return indices; }
            set { indices = value; }
        }

        public Bounds Bounds {
            get { return bounds; }
            set { bounds = value; }
        }

        public void CalculatePartBounds(Vertex[] verts)
        {
            bounds = new Bounds();
            List<Vector3> partVerts = new List<Vector3>();
            for(int i = 0; i != indices.Length; i++)
            {
                partVerts.Add(verts[indices[i].S1].Position);
                partVerts.Add(verts[indices[i].S2].Position);
                partVerts.Add(verts[indices[i].S3].Position);
            }
            bounds.CalculateBounds(partVerts.ToArray());
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
