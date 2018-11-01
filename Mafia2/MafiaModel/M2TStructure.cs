using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Mafia2
{
    public class M2TStructure
    {
        //cannot change.
        private const string fileHeader = "M2T";
        private const byte fileVersion = 1;

        //main header data of the file.
        private string name; //name of model.
        private bool isSkinned;
        Lod[] lods; //Holds the models which can be exported, all EDM content is saved here.
        
        public Lod[] Lods {
            get { return lods; }
            set { lods = value; }
        }
        public bool IsSkinned {
            get { return isSkinned; }
            set { isSkinned = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Build Lods from retrieved data.
        /// </summary>
        public void BuildLods(FrameGeometry frameGeometry, FrameMaterial frameMaterial, VertexBuffer[] vertexBuffers, IndexBuffer[] indexBuffers)
        {
            lods = new Lod[frameGeometry.NumLods];
            for (int i = 0; i != lods.Length; i++)
            {
                FrameLOD frameLod = frameGeometry.LOD[i];
                lods[i] = new Lod
                {
                    VertexDeclaration = frameGeometry.LOD[i].VertexDeclaration
                };
                IndexBuffer indexBuffer = indexBuffers[i];
                VertexBuffer vertexBuffer = vertexBuffers[i];

                int stride;
                Dictionary<VertexFlags, FrameLOD.VertexOffset> vertexOffsets = frameLod.GetVertexOffsets(out stride);

                lods[i].NumUVChannels = 4;
                lods[i].Vertices = new Vertex[frameLod.NumVertsPr];

                for (int v = 0; v != lods[i].Vertices.Length; v++)
                {
                    Vertex vertex = new Vertex();
                    vertex.UVs = new UVVector2[lods[i].NumUVChannels];
                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.Position))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Position].Offset;
                        vertex.ReadPositionData(vertexBuffer.Data, startIndex, frameGeometry.DecompressionFactor,
                            frameGeometry.DecompressionOffset);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.Tangent))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Position].Offset;
                        vertex.ReadTangentData(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.Normals].Offset;
                        vertex.ReadNormalData(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.BlendData))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.BlendData].Offset;
                        vertex.ReadBlendData(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.flag_0x80))
                    {
                        Console.WriteLine("Skip vertex with flag_0x80");
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords0].Offset;
                        vertex.ReadUvData(vertexBuffer.Data, startIndex, 0);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords1].Offset;
                        vertex.ReadUvData(vertexBuffer.Data, startIndex, 1);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords2].Offset;
                        vertex.ReadUvData(vertexBuffer.Data, startIndex, 2);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords7))
                    {
                        int startIndex = v * stride + vertexOffsets[VertexFlags.TexCoords7].Offset;
                        vertex.ReadUvData(vertexBuffer.Data, startIndex, 3);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.flag_0x20000))
                    {
                        Console.WriteLine("Skip vertex with flag_0x20000");
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.flag_0x40000))
                    {
                        Console.WriteLine("Skip vertex with flag_0x40000");
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.DamageGroup))
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
                        Material mat = MaterialsManager.LookupMaterialByHash(materials[x].MaterialHash);
                        modelPart.Material = (mat == null) ? "null" : mat.SPS[0].File;

                        int num = materials[x].StartIndex + materials[x].NumFaces * 3;
                        List<Short3> intList = new List<Short3>(materials[x].NumFaces);
                        int startIndex = materials[x].StartIndex;
                        while (startIndex < num)
                        {
                            Short3 indice = new Short3();
                            indice.S1 = indexBuffer.Data[startIndex + 0];
                            indice.S2 = indexBuffer.Data[startIndex + 1];
                            indice.S3 = indexBuffer.Data[startIndex + 2];
                            intList.Add(indice);
                            startIndex += 3;
                        }

                        modelPart.Indices = intList.ToArray();
                        lods[i].Parts[x] = modelPart;
                    }
                }
            }
        }

        public void ExportToM2T(string exportPath)
        {
            if (File.Exists(exportPath + name + ".m2t"))
                return;

            using (BinaryWriter writer = new BinaryWriter(File.Create(exportPath + name + ".m2t")))
            {
                writer.Write(fileHeader.ToCharArray());
                writer.Write(fileVersion);

                //mesh name
                writer.Write(name);

                //Number of Lods
                writer.Write(lods.Length);

                for (int i = 0; i != lods.Length; i++)
                {
                    Lod lod = lods[i];
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
                    writer.Write(lod.Parts.Length);
                    for (int x = 0; x != lod.Parts.Length; x++)
                        writer.Write(lods[i].Parts[x].Material);

                    //write triangle data.
                    int totalFaces = 0;
                    foreach (ModelPart part in lods[i].Parts)
                        totalFaces += part.Indices.Length;

                    writer.Write(totalFaces);
                    for (int x = 0; x != lod.Parts.Length; x++)
                    {
                        for (int z = 0; z != lods[i].Parts[x].Indices.Length; z++)
                        {
                            //write triangle, and then material
                            lods[i].Parts[x].Indices[z].WriteToFile(writer);
                            writer.Write((ushort)x);
                        }
                    }
                }
            }
        }

        public void ExportToFbx(string path, bool saveBinary)
        {
            string args = "-ConvertToFBX ";
            args += ("\"" + path + name + ".m2t\" ");
            args += ("\"" + path + name + ".fbx\"");
            ProcessStartInfo processStartInfo = new ProcessStartInfo("M2FBX.exe", args)
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process FbxTool = Process.Start(processStartInfo);
            FbxTool.Dispose();
        }

        public void ReadFromM2T(BinaryReader reader)
        {
            if (new string(reader.ReadChars(3)) != fileHeader)
                return;

            if (reader.ReadByte() != fileVersion)
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
                    Lods[i].VertexDeclaration += (int)VertexFlags.Position;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.Normals;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.Tangent;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.BlendData;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.flag_0x80;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords0;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords1;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords2;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords7;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.flag_0x20000;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.flag_0x40000;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.DamageGroup;

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
                    short matId = reader.ReadInt16();
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

        public void ReadFromFbx(string file)
        {
            string args = "-ConvertToM2T ";
            string m2tFile = file.Remove(file.Length - 4, 4) + ".m2t";
            args += ("\"" + file + "\" ");
            args += ("\"" + m2tFile + "\" ");
            ProcessStartInfo processStartInfo = new ProcessStartInfo("M2FBX.exe", args)
            {
                CreateNoWindow = false,
                UseShellExecute = false
            };
            Process FbxTool = Process.Start(processStartInfo);
            while (!FbxTool.HasExited) ;
            FbxTool.Dispose();

            if (!File.Exists(m2tFile))
            {
                MessageBox.Show("Error Occured. Not importing.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (BinaryReader reader = new BinaryReader(File.Open(m2tFile, FileMode.Open)))
                ReadFromM2T(reader);

            if (File.Exists(m2tFile))
                File.Delete(m2tFile);
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
                writer.Write(fileHeader.ToCharArray());
                writer.Write(fileVersion);

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
                    for (int x = 0; x != lods[i].Parts.Length; x++)
                    {
                        for (int z = 0; z != lods[i].Parts[x].Indices.Length; z++)
                        {
                            lods[i].Parts[x].Indices[z].WriteToFile(writer);
                            writer.Write((short)x);
                        }
                    }
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

            public VertexFlags VertexDeclaration {
                get { return vertexDeclaration; }
                set { vertexDeclaration = value; }
            }

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

            public Lod()
            {
                vertexDeclaration = 0;
                numUVChannels = 4;
            }

            //ADD SKELETON
        }

        public class ModelPart
        {
            string material;
            ulong hash;
            Short3[] indices;
            BoundingBox bounds;

            public string Material {
                get { return material; }
                set {
                    material = value;
                    hash = FNV64.Hash(value);
                }
            }

            public ulong Hash {
                get { return hash; }
            }

            public Short3[] Indices {
                get { return indices; }
                set { indices = value; }
            }

            public BoundingBox Bounds {
                get { return bounds; }
                set { bounds = value; }
            }

            public void CalculatePartBounds(Vertex[] verts)
            {
                bounds = new BoundingBox();
                List<Vector3> partVerts = new List<Vector3>();
                for (int i = 0; i != indices.Length; i++)
                {
                    partVerts.Add(verts[indices[i].S1].Position);
                    partVerts.Add(verts[indices[i].S2].Position);
                    partVerts.Add(verts[indices[i].S3].Position);
                }
                bounds.CalculateBounds(partVerts.ToArray());
            }
        }
    }
}
