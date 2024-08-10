using ResourceTypes.BufferPools;
using ResourceTypes.Collisions;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using Utils.StringHelpers;
using Utils.VorticeUtils;
using Vortice.Mathematics;
using Vortice.Mathematics.PackedVector;

namespace Utils.Models
{
    public class M2TStructure
    {
        //cannot change.
        private const string fileHeader = "M2T";
        private const byte fileVersion = 2;

        //main header data of the file.
        private string name; //name of model.
        private bool isSkinned;
        private Skeleton skeleton;
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
        public Skeleton SkeletonData {
            get { return skeleton; }
            set { skeleton = value; }
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
        public void BuildLods(FrameObjectSingleMesh Mesh, VertexBuffer[] vertexBuffers, IndexBuffer[] indexBuffers)
        {
            FrameGeometry frameGeometry = Mesh.GetGeometry();
            FrameMaterial frameMaterial = Mesh.GetMaterial();

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
                lods[i].Vertices = new Vertex[frameLod.NumVerts];

                if (vertexSize * frameLod.NumVerts != vertexBuffer.Data.Length) throw new System.Exception();
                for (int v = 0; v != lods[i].Vertices.Length; v++)
                {
                    //declare data required and send to decompresser
                    byte[] data = new byte[vertexSize];
                    Array.Copy(vertexBuffers[i].Data, (v * vertexSize), data, 0, vertexSize);
                    lods[i].Vertices[v] = VertexTranslator.DecompressVertex(data, frameGeometry.LOD[i].VertexDeclaration, frameGeometry.DecompressionOffset, frameGeometry.DecompressionFactor, vertexOffsets);
                }

                lods[i].Indices = indexBuffer.GetData();
                MaterialStruct[] materials = frameMaterial.Materials[i];
                lods[i].Parts = new ModelPart[materials.Length];
                for (int x = 0; x != materials.Length; x++)
                {

                    if (string.IsNullOrEmpty(materials[x].MaterialName))
                    {
                        var material = MaterialsManager.LookupMaterialByHash(materials[x].MaterialHash);
                        materials[x].MaterialName = material.GetMaterialName();
                    }

                    ModelPart modelPart = new ModelPart();
                    modelPart.Material = materials[x].MaterialName;
                    modelPart.StartIndex = (uint)materials[x].StartIndex;
                    modelPart.NumFaces = (uint)materials[x].NumFaces;
                    lods[i].Parts[x] = modelPart;
                }
            }
        }

        public void BuildLods(FrameObjectModel model, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers)
        {
            BuildLods(model, vertexBuffers, indexBuffers);

            if(model.Skeleton != null && model.SkeletonHierarchy != null && model.BlendInfo != null)
            {
                skeleton = new Skeleton();
                skeleton.Joints = new Joint[model.Skeleton.BoneNames.Length];
                for (int i = 0; i < skeleton.Joints.Length; i++)
                {
                    var joint = new Joint();
                    joint.Name = model.Skeleton.BoneNames[i].ToString();
                    joint.ParentIndex = model.SkeletonHierarchy.ParentIndices[i];
                    joint.Parent = (joint.ParentIndex != 0xFF) ? skeleton.Joints[joint.ParentIndex] : null;
                    joint.LocalTransform = model.Skeleton.JointTransforms[i];

                    skeleton.Joints[i] = joint;
                }

                //skeleton.ComputeTransforms();
            }

            for(int i = 0; i < model.BlendInfo.BoneIndexInfos.Length; i++)
            {
                var indexInfos = model.BlendInfo.BoneIndexInfos[i];
                var lod = lods[i];
                bool[] remapped = new bool[lod.Vertices.Length];
                for(int x = 0; x < indexInfos.NumMaterials; x++)
                {
                    var part = lod.Parts[x];
                    byte offset = 0;
                    for(int s = 0; s < indexInfos.BonesSlot[x]; s++)
                    {
                        offset += indexInfos.BonesPerPool[s];
                    }

                    for(uint z = part.StartIndex; z < part.StartIndex+(part.NumFaces*3); z++)
                    {
                        uint index = lod.Indices[z];
                        if (!remapped[index])
                        {
                            for (uint f = 0; f < indexInfos.NumWeightsPerVertex[x]; f++)
                            {
                                var previousBoneID = lod.Vertices[index].BoneIDs[f];
                                lod.Vertices[index].BoneIDs[f] = indexInfos.IDs[offset + previousBoneID];
                            }
                            remapped[index] = true;                      
                        }
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
            lods[0].Indices = new uint[triangleMesh.Triangles.Count * 3];

            for (int triIdx = 0, idxIdx = 0; triIdx < triangleMesh.Triangles.Count; triIdx++, idxIdx += 3)
            {
                lods[0].Indices[idxIdx] = triangleMesh.Triangles[triIdx].v0;
                lods[0].Indices[idxIdx + 1] = triangleMesh.Triangles[triIdx].v1;
                lods[0].Indices[idxIdx + 2] = triangleMesh.Triangles[triIdx].v2;
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
            Dictionary<string, List<uint>> sortedMats = new Dictionary<string, List<uint>>();
            for(int i = 0; i < triangleMesh.MaterialIndices.Count; i++)
            {
                string mat = ((CollisionMaterials)triangleMesh.MaterialIndices[i]).ToString();
                if(!sortedMats.ContainsKey(mat))
                {
                    List<uint> list = new List<uint>();
                    list.Add(collisionModel.Mesh.Triangles[i].v0);
                    list.Add(collisionModel.Mesh.Triangles[i].v1);
                    list.Add(collisionModel.Mesh.Triangles[i].v2);
                    sortedMats.Add(mat, list);
                }
                else
                {
                    sortedMats[mat].Add(collisionModel.Mesh.Triangles[i].v0);
                    sortedMats[mat].Add(collisionModel.Mesh.Triangles[i].v1);
                    sortedMats[mat].Add(collisionModel.Mesh.Triangles[i].v2);
                }
            }

            lods[0].Parts = new ModelPart[sortedMats.Count];
            List<uint> inds = new List<uint>();
            for (int x = 0; x != lods[0].Parts.Length; x++)
            {
                lods[0].Parts[x] = new ModelPart();
                lods[0].Parts[x].Material = sortedMats.ElementAt(x).Key;
                lods[0].Parts[x].StartIndex = (uint)inds.Count;
                inds.AddRange(sortedMats.ElementAt(x).Value);
                lods[0].Parts[x].NumFaces = (uint)(sortedMats.ElementAt(x).Value.Count / 3);
            }
            this.name = name;
            lods[0].Indices = inds.ToArray();
        }

        public void FlipUVs()
        {
            // TODO:
            /*for (int i = 0; i != lods.Length; i++)
            {
                for (int x = 0; x != lods[i].Vertices.Length; x++)
                {
                    Vertex vert = lods[i].Vertices[x];
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        vert.UVs[0].Y = (1f - vert.UVs[0].Y);
                    }

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        vert.UVs[1].Y = (1f - vert.UVs[1].Y);
                    }

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        vert.UVs[2].Y = (1f - vert.UVs[2].Y);
                    }

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                    {
                        vert.UVs[3].Y = (1f - vert.UVs[3].Y);
                    }
                }
            }*/
        }

        public void ExportToM2T(string exportPath)
        {
            // Check if the directory exists
            if(!Directory.Exists(exportPath))
            {
                // Ask if we can create it
                DialogResult Result = MessageBox.Show("The path does not exist. Do you want to create it?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(Result == DialogResult.Yes)
                {
                    Directory.CreateDirectory(exportPath);
                }
                else
                {
                    // Can't export file with no valid directory.
                    MessageBox.Show("Cannot export a mesh with no valid directory. Please change your directory.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            
            using (BinaryWriter writer = new BinaryWriter(File.Create(exportPath + name + ".m2t")))
            {
                writer.Write(fileHeader.ToCharArray());
                writer.Write(fileVersion);

                //mesh name
                writer.Write(name);

                writer.Write(isSkinned);
                if (isSkinned)
                {
                    writer.Write((byte)skeleton.Joints.Length);
                    for (int i = 0; i < skeleton.Joints.Length; i++)
                    {
                        var joint = skeleton.Joints[i];
                        writer.WriteString8(joint.Name);
                        writer.Write(joint.ParentIndex);
                        Quaternion rotation;
                        Vector3 position, scale;
                        Matrix4x4.Decompose(joint.LocalTransform, out scale, out rotation, out position);
                        position.WriteToFile(writer);
                        rotation.WriteToFile(writer);
                        scale.WriteToFile(writer);
                    }
                }

                //Number of Lods
                writer.Write((byte)lods.Length);

                for (int i = 0; i != lods.Length; i++)
                {
                    List<string> exportLog = new List<string>();
                    Lod lod = lods[i];
                    //Write section for VertexFlags. 
                    writer.Write((int)lod.VertexDeclaration);

                    //write length and then all vertices.
                    writer.Write(lods[i].Vertices.Length);
                    for (int x = 0; x != lods[i].Vertices.Length; x++)
                    {
                        Vertex vert = lods[i].Vertices[x];
                        vert.Normal = new Vector3(vert.Normal.X, vert.Normal.Y, vert.Normal.Z);

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Position))
                        {
                            vert.Position.WriteToFile(writer);
                        }
                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Normals))
                        {
                            vert.Normal.WriteToFile(writer);
                        }
                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Tangent))
                        {
                            vert.Tangent.WriteToFile(writer);
                        }
                        if(lod.VertexDeclaration.HasFlag(VertexFlags.Skin))
                        {
                            writer.Write(vert.BoneIDs);
                            for (int z = 0; z < 4; z++)
                            {
                                writer.Write(vert.BoneWeights[z]);
                            }
                        }

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Color))
                        {
                            exportLog.Add(string.Format("{0} {1} {2} {3}", vert.Color0[0], vert.Color0[1], vert.Color0[2], vert.Color0[3]));
                            writer.Write(vert.Color0);
                        }

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.Color1))
                        {
                            exportLog.Add(string.Format("{0} {1} {2} {3}", vert.Color1[0], vert.Color1[1], vert.Color1[2], vert.Color1[3]));
                            writer.Write(vert.Color1);
                        }

                        if (lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                        {
                            vert.UVs[0].WriteToFile(writer);
                        }
                        if (lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                        {
                            vert.UVs[1].WriteToFile(writer);
                        }
                        if (lod.VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                        {
                            vert.UVs[2].WriteToFile(writer);
                        }
                        if (lod.VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                        {
                            vert.UVs[3].WriteToFile(writer);
                        }
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
                    for (int x = 0; x != lod.Indices.Length; x++)
                    {
                        writer.Write(lods[i].Indices[x]);
                    }
                }
            }
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
            {
                return;
            }

            var version = reader.ReadByte();
            if (version > 2)
            {
                return;
            }
            if(version == 1)
            {
                ReadM2TVersionOne(reader);
            }
            else
            {
                ReadM2TVersionTwo(reader);
            }        
        }

        private void ReadM2TVersionOne(BinaryReader reader)
        {
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
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.Tangent;
                }
                if (reader.ReadBoolean())
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.Skin;
                }
                if (reader.ReadBoolean())
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.Color;
                }
                if (reader.ReadBoolean())
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords0;
                }
                if (reader.ReadBoolean())
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords1;
                }
                if (reader.ReadBoolean())
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.TexCoords2;
                }
                if (reader.ReadBoolean())
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.ShadowTexture;
                }
                if (reader.ReadBoolean())
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.Color1;
                }
                if (reader.ReadBoolean())
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.BBCoeffs;
                }
                if (reader.ReadBoolean())
                {
                    Lods[i].VertexDeclaration += (int)VertexFlags.DamageGroup;
                }

                //write length and then all vertices.
                lods[i].Vertices = new Vertex[reader.ReadInt32()];
                for (int x = 0; x != lods[i].Vertices.Length; x++)
                {
                    Vertex vert = new Vertex();
                    vert.UVs = new Half2[4];

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Position))
                    {
                        vert.Position = Vector3Utils.ReadFromFile(reader);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        vert.Normal = Vector3Utils.ReadFromFile(reader);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Tangent))
                    {
                        vert.Tangent = Vector3Utils.ReadFromFile(reader);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        vert.UVs[0] = Half2Extenders.ReadFromFile(reader);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        vert.UVs[1] = Half2Extenders.ReadFromFile(reader);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        vert.UVs[2] = Half2Extenders.ReadFromFile(reader);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                    {
                        vert.UVs[3] = Half2Extenders.ReadFromFile(reader);
                    }
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
                Lods[i].Indices = new uint[numIndices];
                for (int x = 0; x != Lods[i].Indices.Length; x++)
                {
                    Lods[i].Indices[x] = reader.ReadUInt16();
                }
                Lods[i].CalculatePartBounds();

            }
        }

        private void ReadM2TVersionTwo(BinaryReader reader)
        {
            //mesh name
            name = reader.ReadString();

            isSkinned = reader.ReadBoolean();
            if (isSkinned)
            {
                byte size = reader.ReadByte();
                skeleton = new Skeleton();
                skeleton.Joints = new Joint[size];
                for (int i = 0; i < size; i++)
                {
                    Joint joint = new Joint();
                    joint.Name = reader.ReadString8();
                    joint.ParentIndex = reader.ReadByte();
                    joint.Parent = (joint.ParentIndex != 0xFF) ? skeleton.Joints[joint.ParentIndex] : null; //may crash because root will not be in range
                    Vector3 position = Vector3Utils.ReadFromFile(reader);
                    Quaternion rotation = QuaternionExtensions.ReadFromFile(reader);
                    Vector3 scale = Vector3Utils.ReadFromFile(reader);
                    joint.WorldTransform = MatrixUtils.SetMatrix(rotation, scale, position);
                    skeleton.Joints[i] = joint;
                }
            }

            //Number of Lods
            Lods = new Lod[reader.ReadByte()];

            for (int i = 0; i != Lods.Length; i++)
            {
                Lods[i] = new Lod
                {
                    VertexDeclaration = 0
                };

                lods[i].VertexDeclaration = (VertexFlags)reader.ReadInt32();

                //write length and then all vertices.
                lods[i].Vertices = new Vertex[reader.ReadInt32()];
                for (int x = 0; x != lods[i].Vertices.Length; x++)
                {
                    Vertex vert = new Vertex();
                    vert.UVs = new Half2[4];

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Position))
                    {
                        vert.Position = Vector3Utils.ReadFromFile(reader);
                    }

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Normals))
                    {
                        vert.Normal = Vector3Utils.ReadFromFile(reader);
                    }

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Tangent))
                    {
                        vert.Tangent = Vector3Utils.ReadFromFile(reader);
                    }

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Skin))
                    {
                        vert.BoneIDs = reader.ReadBytes(4);
                        vert.BoneWeights = new float[4];
                        for (int z = 0; z < 4; z++)
                        {
                            vert.BoneWeights[z] = reader.ReadSingle();
                        }
                    }

                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Color))
                    {
                        vert.Color0 = reader.ReadBytes(4);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.Color1))
                    {
                        vert.Color1 = reader.ReadBytes(4);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                    {
                        vert.UVs[0] = Half2Extenders.ReadFromFile(reader);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords1))
                    {
                        vert.UVs[1] = Half2Extenders.ReadFromFile(reader);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.TexCoords2))
                    {
                        vert.UVs[2] = Half2Extenders.ReadFromFile(reader);
                    }
                    if (Lods[i].VertexDeclaration.HasFlag(VertexFlags.ShadowTexture))
                    {
                        vert.UVs[3] = Half2Extenders.ReadFromFile(reader);
                    }
                    lods[i].Vertices[x] = vert;
                }

                //read mesh count and texture names.
                Lods[i].Parts = new ModelPart[reader.ReadInt32()];
                for (int x = 0; x != Lods[i].Parts.Length; x++)
                {
                    Lods[i].Parts[x] = new ModelPart();
                    Lods[i].Parts[x].Material = reader.ReadString();
                    Lods[i].Parts[x].StartIndex = reader.ReadUInt32();
                    Lods[i].Parts[x].NumFaces = reader.ReadUInt32();

                    var material = MaterialsManager.LookupMaterialByName(Lods[i].Parts[x].Material);

                    if (material != null)
                    {
                        Lods[i].Parts[x].Hash = material.GetMaterialHash();
                    }
                }

                int numIndices = reader.ReadInt32();
                Lods[i].Indices = new uint[numIndices];
                for (int x = 0; x != Lods[i].Indices.Length; x++)
                {
                    Lods[i].Indices[x] = reader.ReadUInt32();
                }
                Lods[i].CalculatePartBounds();

            }
        }

        public void ExportCollisionToM2T(string directory, string name)
        {
            this.name = name;

            using (BinaryWriter writer = new BinaryWriter(File.Create(directory + "\\" + name + ".m2t")))
            {
                writer.Write(fileHeader.ToCharArray());
                writer.Write(fileVersion);

                //mesh name
                writer.Write(name);
                writer.Write(isSkinned);
                //Number of Lods
                writer.Write((byte)1);

                for (int i = 0; i != 1; i++)
                {
                    //Write section for VertexFlags. 
                    writer.Write(257);

                    //write length and then all vertices.
                    writer.Write(lods[i].Vertices.Length);
                    for (int x = 0; x != lods[i].Vertices.Length; x++)
                    {
                        Vertex vert = lods[i].Vertices[x];

                        vert.Position.WriteToFile(writer);
                        Half2Extenders.WriteToFile(new Half2(0.0f, 0.0f), writer);
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
                    {
                        writer.Write(lods[i].Indices[x]);
                    }
                }
            }
        }

        public class Joint
        {
            string name;
            byte parentIndex;
            Joint parent;
            Matrix4x4 localTransform;
            Matrix4x4 worldTransform;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public Joint Parent {
                get { return parent; }
                set { parent = value; }
            }
            public byte ParentIndex {
                get { return parentIndex; }
                set { parentIndex = value; }
            }
            public Matrix4x4 LocalTransform {
                get { return localTransform; }
                set { localTransform = value; }
            }
            public Matrix4x4 WorldTransform {
                get { return worldTransform; }
                set { worldTransform = value; }
            }

            public Vector3 GetWorldPosition(Vector3 invertedLocal)
            {
                var transposed = GetRotationFromLocalTransform(true);
                var transformed = Vector3.Transform(invertedLocal, transposed);
                return new Vector3(transformed.X, transformed.Y, transformed.Z);
            }

            public void SetWorldPosition(Vector3 position)
            {
                worldTransform.Translation = position;
            }

            private Matrix4x4 GetRotationFromLocalTransform(bool transpose)
            {
                Vector3 scale, position;
                Quaternion rotation;
                Matrix4x4.Decompose(localTransform, out scale, out rotation, out position);  
                
                var matrix = Matrix4x4.CreateFromQuaternion(rotation);
                if (transpose)
                {
                    matrix = Matrix4x4.Transpose(matrix);
                }

                return matrix;
            }

            public void ComputeJointTransform()
            {
                var parentJoint = Parent;
                var invertedLocal = LocalTransform.Translation;
                while (parentJoint != null)
                {
                    invertedLocal = parentJoint.GetWorldPosition(invertedLocal);
                    parentJoint = parentJoint.Parent;
                }
                if (Parent != null)
                {
                    Parent.ComputeJointTransform();
                    invertedLocal += Parent.GetWorldPosition(invertedLocal);
                }
                SetWorldPosition(invertedLocal);
            }
        }

        public class Skeleton
        {
            Joint[] joints;

            public Joint[] Joints {
                get { return joints; }
                set { joints = value; }
            }

            public void ComputeTransforms()
            {
                for(int i = 0; i < Joints.Length; i++)
                {
                    var currentJoint = joints[i];
                    currentJoint.ComputeJointTransform();
                }
            }

        }
        public class Lod
        {
            private VertexFlags vertexDeclaration;
            Vertex[] vertices;
            ModelPart[] parts;
            uint[] indices;

            public VertexFlags VertexDeclaration {
                get { return vertexDeclaration; }
                set { vertexDeclaration = value; }
            }

            public Vertex[] Vertices {
                get { return vertices; }
                set { vertices = value; }
            }
            public uint[] Indices {
                get { return indices; }
                set { indices = value; }
            }
            public ModelPart[] Parts {
                get { return parts; }
                set { parts = value; }
            }

            public Lod()
            {
                vertexDeclaration = 0;
            }


            public void CalculatePartBounds()
            {
                for(int i = 0; i != parts.Length; i++)
                {
                    List<Vector3> partVerts = new List<Vector3>();
                    for (int x = 0; x != indices.Length; x++)
                    {
                        partVerts.Add(vertices[indices[i]].Position);
                    }
                    BoundingBox bounds = BoundingBox.CreateFromPoints(partVerts.ToArray());
                    parts[i].Bounds = bounds;
                }
            }

            public int GetIndexFormat()
            {
                return (indices.Length * 3 > ushort.MaxValue ? 2 : 1);
            }

            public bool Over16BitLimit()
            {
                return (vertices.Length > ushort.MaxValue);
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
                    normals[i] = Vector3.Normalize(normals[i]);
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
