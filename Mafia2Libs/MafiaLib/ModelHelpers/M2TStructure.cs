using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using SharpDX;
using ResourceTypes.FrameResource;
using Utils.SharpDXExtensions;
using ResourceTypes.BufferPools;
using System.Linq;
using ResourceTypes.Collisions;
using ResourceTypes.Collisions.Opcode;

namespace Utils.Models
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

        private string aoTexture;
        
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
        public string AOTexture {
            get { return aoTexture; }
            set { aoTexture = value; }
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

                int vertexSize;
                Dictionary<VertexFlags, FrameLOD.VertexOffset> vertexOffsets = frameLod.GetVertexOffsets(out vertexSize);

                lods[i].NumUVChannels = 4;
                lods[i].Vertices = new Vertex[frameLod.NumVertsPr];

                if (vertexSize * frameLod.NumVertsPr != vertexBuffer.Data.Length) throw new System.Exception();

                for (int v = 0; v != lods[i].Vertices.Length; v++)
                {
                    Vertex vertex = new Vertex();
                    vertex.UVs = new Half2[lods[i].NumUVChannels];
                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.Position))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Position].Offset;
                        vertex.Position = VertexTranslator.ReadPositionDataFromVB(vertexBuffer.Data, startIndex, frameGeometry.DecompressionFactor, frameGeometry.DecompressionOffset);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.Tangent))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Position].Offset;
                        vertex.Tangent = VertexTranslator.ReadTangentDataFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Normals].Offset;
                        vertex.Normal = VertexTranslator.ReadNormalDataFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.Skin))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Skin].Offset;
                        vertex.BlendWeight = VertexTranslator.ReadBlendWeightFromVB(vertexBuffer.Data, startIndex);
                        vertex.BoneID = VertexTranslator.ReadBlendIDFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.Color))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Color].Offset;
                        vertex.Color0 = VertexTranslator.ReadColorFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords0].Offset;
                        vertex.UVs[0] = VertexTranslator.ReadTexcoordFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords1].Offset;
                        vertex.UVs[1] = VertexTranslator.ReadTexcoordFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.TexCoords2].Offset;
                        vertex.UVs[2] = VertexTranslator.ReadTexcoordFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.ShadowTexture].Offset;
                        vertex.UVs[3] = VertexTranslator.ReadTexcoordFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.Color1))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.Color1].Offset;
                        vertex.Color1 = VertexTranslator.ReadColorFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.BBCoeffs))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.BBCoeffs].Offset;
                        vertex.BBCoeffs = VertexTranslator.ReadBBCoeffsVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].VertexDeclaration.HasFlag(VertexFlags.DamageGroup))
                    {
                        int startIndex = v * vertexSize + vertexOffsets[VertexFlags.DamageGroup].Offset;
                        vertex.DamageGroup = VertexTranslator.ReadDamageGroupFromVB(vertexBuffer.Data, startIndex);
                    }

                    if (lods[i].NormalMapInfoPresent)
                        vertex.BuildBinormals();

                    lods[i].Vertices[v] = vertex;
                    lods[i].Indices = indexBuffer.Data;
                    MaterialStruct[] materials = frameMaterial.Materials[i];
                    lods[i].Parts = new ModelPart[materials.Length];
                    for (int x = 0; x != materials.Length; x++)
                    {

                        ModelPart modelPart = new ModelPart();
                        modelPart.Material = materials[x].MaterialHash.ToString();
                        modelPart.StartIndex = (uint)materials[x].StartIndex;
                        modelPart.NumFaces = (uint)materials[x].NumFaces;
                        lods[i].Parts[x] = modelPart;
                    }
                }
            }
        }

        public void BuildCollision(ResourceTypes.Collisions.Collision.CollisionModel collisionModel, string name)
        {
            lods = new Lod[1];
            lods[0] = new Lod();

            TriangleMesh triangleMesh = collisionModel.Mesh;
            lods[0].Vertices = new Vertex[triangleMesh.Vertices.Count];
            lods[0].Indices = new ushort[triangleMesh.Triangles.Count * 3];

            for (int triIdx = 0, idxIdx = 0; triIdx < triangleMesh.Triangles.Count; triIdx++, idxIdx += 3)
            {
                lods[0].Indices[idxIdx] = (ushort) triangleMesh.Triangles[triIdx].v0;
                lods[0].Indices[idxIdx + 1] = (ushort) triangleMesh.Triangles[triIdx].v1;
                lods[0].Indices[idxIdx + 2] = (ushort) triangleMesh.Triangles[triIdx].v2;
            }

            for (int x = 0; x != lods[0].Vertices.Length; x++)
            {
                lods[0].Vertices[x] = new Vertex();
                lods[0].Vertices[x].Position = triangleMesh.Vertices[x];
            }

            //sort materials in order:
            //M2T doesn't support unorganised triangles, only triangles in order by material.
            //basically like mafia itself, so we have to reorder them and then save.
            //this doesn't mess anything up, just takes a little longer :)
            Dictionary<string, List<ushort>> sortedMats = new Dictionary<string, List<ushort>>();
            for(int i = 0; i < triangleMesh.MaterialIndices.Count; i++)
            {
                string mat = ((CollisionMaterials)triangleMesh.MaterialIndices[i]).ToString();
                if(!sortedMats.ContainsKey(mat))
                {
                    List<ushort> list = new List<ushort>();
                    list.Add((ushort)collisionModel.Mesh.Triangles[i].v0);
                    list.Add((ushort)collisionModel.Mesh.Triangles[i].v1);
                    list.Add((ushort)collisionModel.Mesh.Triangles[i].v2);
                    sortedMats.Add(mat, list);
                }
                else
                {
                    sortedMats[mat].Add((ushort)collisionModel.Mesh.Triangles[i].v0);
                    sortedMats[mat].Add((ushort)collisionModel.Mesh.Triangles[i].v1);
                    sortedMats[mat].Add((ushort)collisionModel.Mesh.Triangles[i].v2);
                }
            }

            lods[0].Parts = new ModelPart[sortedMats.Count];
            List<ushort> inds = new List<ushort>();
            for (int x = 0; x != lods[0].Parts.Length; x++)
            {
                lods[0].Parts[x] = new ModelPart();
                lods[0].Parts[x].Material = sortedMats.ElementAt(x).Key;
                lods[0].Parts[x].StartIndex = (uint)inds.Count;
                inds.AddRange(sortedMats.ElementAt(x).Value);
                lods[0].Parts[x].NumFaces = (uint)Math.Abs(inds.Count - (sortedMats.ElementAt(x).Value.Count / 3));
            }
            this.name = name;
            lods[0].Indices = inds.ToArray();
        }

        public void FlipUVs()
        {
            for (int i = 0; i != lods.Length; i++)
            {
                for (int x = 0; x != lods[i].Vertices.Length; x++)
                {
                    Vertex vert = lods[i].Vertices[x];
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                        vert.UVs[0].Y = (1f - vert.UVs[0].Y);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                        vert.UVs[1].Y = (1f - vert.UVs[1].Y);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                        vert.UVs[2].Y = (1f - vert.UVs[2].Y);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                        vert.UVs[3].Y = (1f - vert.UVs[3].Y);
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
                writer.Write((byte)lods.Length);

                for (int i = 0; i != lods.Length; i++)
                {
                    Lod lod = lods[i];
                    //Write section for VertexFlags. 
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.Position));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.Normals));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.Tangent));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.Skin));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.Color));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.ShadowTexture));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.Color1));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.BBCoeffs));
                    writer.Write(lod.VertexDeclaration.HasFlag(VertexFlags.DamageGroup));

                    //write length and then all vertices.
                    writer.Write(lods[i].Vertices.Length);
                    for (int x = 0; x != lods[i].Vertices.Length; x++)
                    {
                        Vertex vert = lods[i].Vertices[x];
                        vert.Normal = new Vector3(vert.Normal.X, vert.Normal.Y, vert.Normal.Z);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Position))
                            vert.Position.WriteToFile(writer);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Normals))
                            vert.Normal.WriteToFile(writer);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Tangent))
                            vert.Tangent.WriteToFile(writer);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                            vert.UVs[0].WriteToFile(writer);

                        if(lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                            vert.UVs[1].WriteToFile(writer);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                            vert.UVs[2].WriteToFile(writer);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                            vert.UVs[3].WriteToFile(writer);
                    }

                    //write mesh count and texture names.
                    writer.Write(lod.Parts.Length);
                    for (int x = 0; x != lod.Parts.Length; x++)
                    {
                        writer.Write(lods[i].Parts[x].Material);
                        writer.Write(lods[i].Parts[x].StartIndex);
                        writer.Write(lods[i].Parts[x].NumFaces);
                    }

                    //write triangle data.
                    writer.Write(lod.Indices.Length);
                    for(int x = 0; x != lod.Indices.Length; x++)
                        writer.Write(lods[i].Indices[x]);
                }
            }
        }

        public void ExportToFbx(string path, bool saveBinary)
        {
            FBXHelper.ConvertM2T(path + name + ".m2t", path + name + ".fbx");
        }

        public void ReadFromM2T(string file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                ReadFromM2T(reader);
            }
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
                    Lods[i].VertexDeclaration += (int)VertexFlags.Skin;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.Color;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords0;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords1;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords2;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.ShadowTexture;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.Color1;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.BBCoeffs;

                if (reader.ReadBoolean())
                    Lods[i].VertexDeclaration += (int)VertexFlags.DamageGroup;

                //write length and then all vertices.
                lods[i].Vertices = new Vertex[reader.ReadInt32()];
                lods[i].NumUVChannels = 4;
                for (int x = 0; x != lods[i].Vertices.Length; x++)
                {
                    Vertex vert = new Vertex();
                    vert.UVs = new Half2[lods[i].NumUVChannels];

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Position))
                        vert.Position = Vector3Extenders.ReadFromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Normals))
                        vert.Normal = Vector3Extenders.ReadFromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Tangent))
                        vert.Tangent = Vector3Extenders.ReadFromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                        vert.UVs[0] = Half2Extenders.ReadFromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                        vert.UVs[1] = Half2Extenders.ReadFromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                        vert.UVs[2] = Half2Extenders.ReadFromFile(reader);

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                        vert.UVs[3] = Half2Extenders.ReadFromFile(reader);

                    lods[i].Vertices[x] = vert;
                }

                //write mesh count and texture names.
                Lods[i].Parts = new ModelPart[reader.ReadInt32()];
                for (int x = 0; x != Lods[i].Parts.Length; x++)
                {
                    Lods[i].Parts[x] = new ModelPart();
                    Lods[i].Parts[x].Material = reader.ReadString();
                    ulong hash = 0;
                    ulong.TryParse(Lods[i].Parts[x].Material, out hash);
                    Lods[i].Parts[x].Hash = hash;
                    Lods[i].Parts[x].StartIndex = reader.ReadUInt32();
                    Lods[i].Parts[x].NumFaces = reader.ReadUInt32();
                }

                int numIndices = reader.ReadInt32();
                Lods[i].Indices = new ushort[numIndices];
                for (int x = 0; x != Lods[i].Indices.Length; x++)
                    Lods[i].Indices[x] = reader.ReadUInt16();

                Lods[i].CalculatePartBounds();

            }
        }

        public bool ReadFromFbx(string file)
        {
            string args = "-ConvertToM2T ";
            string m2tFile = file.Remove(file.Length - 4, 4) + ".m2t";
            args += ("\"" + file + "\" ");
            args += ("\"" + m2tFile + "\" ");

            if (FBXHelper.ConvertFBX(file, m2tFile) == 0)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(m2tFile, FileMode.Open)))
                    ReadFromM2T(reader);

                if (File.Exists(m2tFile))
                    File.Delete(m2tFile);

                return true;
            }
            return false;
        }

        public void ExportCollisionToM2T(string directory, string name)
        {
            this.name = name;

            using (BinaryWriter writer = new BinaryWriter(File.Create(directory + name + ".m2t")))
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

                    writer.Write(lods[i].Parts.Length);
                    for (int x = 0; x != lods[i].Parts.Length; x++)
                    {
                        writer.Write(lods[i].Parts[x].Material);
                        writer.Write(lods[i].Parts[x].StartIndex);
                        writer.Write(lods[i].Parts[x].NumFaces);
                    }

                    //write triangle data.
                    writer.Write(lods[i].Indices.Length);
                    for (int x = 0; x != lods[i].Indices.Length; x++)
                        writer.Write(lods[i].Indices[x]);
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
            ushort[] indices;

            public VertexFlags VertexDeclaration {
                get { return vertexDeclaration; }
                set { vertexDeclaration = value; }
            }

            public Vertex[] Vertices {
                get { return vertices; }
                set { vertices = value; }
            }

            public ushort[] Indices {
                get { return indices; }
                set { indices = value; }
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


            public void CalculatePartBounds()
            {
                for(int i = 0; i != parts.Length; i++)
                {
                    List<Vector3> partVerts = new List<Vector3>();
                    for (int x = 0; x != indices.Length; x++)
                        partVerts.Add(vertices[indices[i]].Position);
                    BoundingBox bounds;
                    BoundingBox.FromPoints(partVerts.ToArray(), out bounds);
                    parts[i].Bounds = bounds;
                }
            }

            public void CalculateNormals()
            {
                List<Vector3> surfaceNormals = new List<Vector3>();
                Vector3[] normals = new Vector3[vertices.Length];

                for(int i = 0; i < parts.Length; i++)
                {
                    var normal = new Vector3();

                    var index = parts[i].StartIndex;
                    while(index < parts[i].StartIndex+parts[i].NumFaces*3)
                    {
                        var edge1 = vertices[indices[index]].Position - vertices[indices[index + 1]].Position;
                        var edge2 = vertices[indices[index]].Position - vertices[indices[index + 2]].Position;
                        normal = Vector3.Cross(edge1, edge2);
                        normals[indices[index]] += normal;
                        normals[indices[index+1]] += normal;
                        normals[indices[index+2]] += normal;
                        surfaceNormals.Add(normal);
                        index += 3;
                    }
                    surfaceNormals.Add(normal);
                }

                for(int i = 0; i < vertices.Length; i++)
                {
                    normals[i].Normalize();
                    vertices[i].Normal = normals[i];
                }
            }
        }

        public class ModelPart
        {
            string material;
            ulong hash;
            uint startIndex;
            uint numFaces;
            BoundingBox bounds;

            public string Material {
                get { return material; }
                set {material = value; }
            }

            public ulong Hash {
                get { return hash; }
                set { hash = value; }
            }

            public BoundingBox Bounds {
                get { return bounds; }
                set { bounds = value; }
            }

            public uint StartIndex {
                get { return startIndex; }
                set { startIndex = value; }
            }

            public uint NumFaces {
                get { return numFaces; }
                set { numFaces = value; }
            }
        }
    }
}
