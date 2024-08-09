using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Utils.Models;
using Utils.Settings;
using Utils.StringHelpers;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    using VERTEX = SharpGLTF.Geometry.VertexTypes.VertexPositionNormalTangent;
    using VERTEXS4 = VertexJoints4;
    using VERTEXRIDGEDBUILDER = VertexBuilder<VertexPositionNormalTangent, VertexTexture1, VertexEmpty>;
    using VERTEXSKINNEDBUILDER = VertexBuilder<VertexPositionNormalTangent, VertexTexture1, VertexJoints4>;
    using static Octokit.Caching.CachedResponse;

    public class MT_Lod : IValidator
    {
        public VertexFlags VertexDeclaration { get; set; }
        public Vertex[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public MT_FaceGroup[] FaceGroups { get; set; }

        /** IO Functions */
        public bool ReadFromFile(BinaryReader reader)
        {
            VertexDeclaration = (VertexFlags)reader.ReadInt32();

            // Attempt to read Vertices
            uint NumVertices = reader.ReadUInt32();
            Vertices = new Vertex[NumVertices];
            for (int i = 0; i < NumVertices; i++)
            {
                Vertex NewVertex = new Vertex();
                VertexFlags LocalDeclaration = VertexDeclaration;

                // Read Vertex Information using LocalDeclaration
                #region ReadVertexInfo
                if (LocalDeclaration.HasFlag(VertexFlags.Position))
                {
                    NewVertex.Position = Vector3Utils.ReadFromFile(reader);
                }

                if (LocalDeclaration.HasFlag(VertexFlags.Normals))
                {
                    NewVertex.Normal = Vector3Utils.ReadFromFile(reader);
                }

                if (LocalDeclaration.HasFlag(VertexFlags.Tangent))
                {
                    NewVertex.Tangent = Vector3Utils.ReadFromFile(reader);
                }

                if (LocalDeclaration.HasFlag(VertexFlags.Skin))
                {
                    NewVertex.BoneIDs = reader.ReadBytes(4);
                    NewVertex.BoneWeights = new float[4];
                    for (int z = 0; z < 4; z++)
                    {
                        NewVertex.BoneWeights[z] = reader.ReadSingle();
                    }
                }

                if(LocalDeclaration.HasFlag(VertexFlags.DamageGroup))
                {
                    NewVertex.DamageGroup = reader.ReadInt32();
                }

                if (LocalDeclaration.HasFlag(VertexFlags.Color))
                {
                    NewVertex.Color0 = reader.ReadBytes(4);
                }

                if (LocalDeclaration.HasFlag(VertexFlags.Color1))
                {
                    NewVertex.Color1 = reader.ReadBytes(4);
                }

                if (LocalDeclaration.HasFlag(VertexFlags.TexCoords0))
                {
                    NewVertex.UVs[0] = Half2Extenders.ReadFromFile(reader);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.TexCoords1))
                {
                    NewVertex.UVs[1] = Half2Extenders.ReadFromFile(reader);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.TexCoords2))
                {
                    NewVertex.UVs[2] = Half2Extenders.ReadFromFile(reader);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.ShadowTexture))
                {
                    NewVertex.UVs[3] = Half2Extenders.ReadFromFile(reader);
                }
                #endregion

                Vertices[i] = NewVertex;
            }

            // Read FaceGroups
            uint NumFaceGroups = reader.ReadUInt32();
            FaceGroups = new MT_FaceGroup[NumFaceGroups];
            for (int i = 0; i < NumFaceGroups; i++)
            {
                // Attempt to read FaceGroup
                MT_FaceGroup NewFaceGroup = new MT_FaceGroup();
                NewFaceGroup.StartIndex = reader.ReadUInt32();
                NewFaceGroup.NumFaces = reader.ReadUInt32();

                // Read FaceGroup Material
                MT_MaterialInstance NewMaterial = new MT_MaterialInstance();
                NewMaterial.MaterialFlags = (MT_MaterialInstanceFlags)reader.ReadInt32();
                NewMaterial.Name = StringHelpers.ReadString8(reader);

                if (NewMaterial.MaterialFlags.HasFlag(MT_MaterialInstanceFlags.HasDiffuse))
                {
                    string Diffuse = StringHelpers.ReadString8(reader);
                    NewMaterial.DiffuseTexture = Path.GetFileName(Diffuse);
                }

                NewFaceGroup.Material = NewMaterial;
                FaceGroups[i] = NewFaceGroup;
            }

            // Read Indices
            uint NumIndices = reader.ReadUInt32();
            Indices = new uint[NumIndices];
            for (int i = 0; i < NumIndices; i++)
            {
                Indices[i] = reader.ReadUInt32();
            }

            CalculatePartBounds();

            return true;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((int)VertexDeclaration);

            // Attempt to write vertices
            writer.Write(Vertices.Length);
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertex Vert = Vertices[i];
                VertexFlags LocalDeclaration = VertexDeclaration;

                #region WriteVertexInfo
                if (LocalDeclaration.HasFlag(VertexFlags.Position))
                {
                    Vert.Position.WriteToFile(writer);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.Normals))
                {
                    Vert.Normal.WriteToFile(writer);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.Tangent))
                {
                    Vert.Tangent.WriteToFile(writer);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.Skin))
                {
                    writer.Write(Vert.BoneIDs);
                    for (int z = 0; z < 4; z++)
                    {
                        writer.Write(Vert.BoneWeights[z]);
                    }
                }
                if(LocalDeclaration.HasFlag(VertexFlags.DamageGroup))
                {
                    writer.Write(Vert.DamageGroup);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.Color))
                {
                    writer.Write(Vert.Color0);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.Color1))
                {
                    writer.Write(Vert.Color1);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.TexCoords0))
                {
                    Vert.UVs[0].WriteToFile(writer);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.TexCoords1))
                {
                    Vert.UVs[1].WriteToFile(writer);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.TexCoords2))
                {
                    Vert.UVs[2].WriteToFile(writer);
                }
                if (LocalDeclaration.HasFlag(VertexFlags.ShadowTexture))
                {
                    Vert.UVs[3].WriteToFile(writer);
                }
                #endregion EndVertexInfo
            }

            writer.Write(FaceGroups.Length);
            for (int i = 0; i < FaceGroups.Length; i++)
            {
                // Write FaceGroup
                MT_FaceGroup FaceGroup = FaceGroups[i];
                writer.Write(FaceGroup.StartIndex);
                writer.Write(FaceGroup.NumFaces);

                // Write Material Instance
                MT_MaterialInstance MaterialInstance = FaceGroup.Material;
                writer.Write((int)MaterialInstance.MaterialFlags);
                StringHelpers.WriteString8(writer, MaterialInstance.Name);

                if (MaterialInstance.MaterialFlags.HasFlag(MT_MaterialInstanceFlags.HasDiffuse))
                {
                    StringHelpers.WriteString8(writer, MaterialInstance.DiffuseTexture);
                }
            }

            writer.Write(Indices.Length);
            for (int i = 0; i < Indices.Length; i++)
            {
                writer.Write(Indices[i]);
            }
        }

        public MeshBuilder<VERTEX> BuildGLTF()
        {
            MeshBuilder<VERTEX> LodMesh = new MeshBuilder<VERTEX>();

            foreach (MT_FaceGroup FaceGroup in FaceGroups)
            {
                var material1 = new MaterialBuilder(FaceGroup.Material.Name)
                    .WithDoubleSide(true)
                    .WithMetallicRoughnessShader()
                    .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(1, 0, 0, 1));

                var CurFaceGroup = LodMesh.UsePrimitive(material1);

                uint StartIndex = FaceGroup.StartIndex;
                uint EndIndex = StartIndex + (FaceGroup.NumFaces * 3);

                for (uint Idx = StartIndex; Idx < EndIndex; Idx+=3)
                {
                    Vertex V1 = Vertices[Indices[Idx]];
                    Vertex V2 = Vertices[Indices[Idx + 1]];
                    Vertex V3 = Vertices[Indices[Idx + 2]];

                    VERTEX VB1 = new VERTEX(V1.Position, V1.Normal, new Vector4(V1.Tangent, 1.0f));
                    VERTEX VB2 = new VERTEX(V2.Position, V2.Normal, new Vector4(V2.Tangent, 1.0f));
                    VERTEX VB3 = new VERTEX(V3.Position, V3.Normal, new Vector4(V3.Tangent, 1.0f));

                    CurFaceGroup.AddTriangle(new VERTEXRIDGEDBUILDER(VB1), new VERTEXRIDGEDBUILDER(VB2), new VERTEXRIDGEDBUILDER(VB3));
                }
            }

            return LodMesh;
        }

        public MeshBuilder<VERTEX, VertexTexture1, VertexJoints4> BuildSkinnedGLTF()
        {
            MeshBuilder<VERTEX, VertexTexture1, VertexJoints4> LodMesh = new MeshBuilder<VERTEX, VertexTexture1, VertexJoints4>();

            foreach (MT_FaceGroup FaceGroup in FaceGroups)
            {
                var material1 = new MaterialBuilder(FaceGroup.Material.Name)
                    .WithDoubleSide(true)
                    .WithMetallicRoughnessShader()
                    .WithChannelParam(KnownChannel.BaseColor, KnownProperty.RGBA, new Vector4(1, 0, 0, 1));

                var CurFaceGroup = LodMesh.UsePrimitive(material1);

                uint StartIndex = FaceGroup.StartIndex;
                uint EndIndex = StartIndex + (FaceGroup.NumFaces * 3);

                for (uint Idx = StartIndex; Idx < EndIndex; Idx += 3)
                {
                    Vertex V1 = Vertices[Indices[Idx]];
                    Vertex V2 = Vertices[Indices[Idx + 1]];
                    Vertex V3 = Vertices[Indices[Idx + 2]];

                    CurFaceGroup.AddTriangle(BuildVertex(V1), BuildVertex(V2), BuildVertex(V3));
                }
            }

            return LodMesh;
        }

        private VERTEXSKINNEDBUILDER BuildVertex(Vertex GameVertex)
        {
            VERTEX VB1 = new VERTEX(GameVertex.Position, GameVertex.Normal, new Vector4(GameVertex.Tangent, 1.0f));

            VertexTexture1 TB1 = new VertexTexture1(GameVertex.UVs[0]);

            (int JointIndex, float Weight)[] VertexWeights = new (int JointIndex, float Weight)[4];
            VertexWeights[0] = (GameVertex.BoneIDs[0], GameVertex.BoneWeights[0]);
            VertexWeights[1] = (GameVertex.BoneIDs[1], GameVertex.BoneWeights[1]);
            VertexWeights[2] = (GameVertex.BoneIDs[2], GameVertex.BoneWeights[2]);
            VertexWeights[3] = (GameVertex.BoneIDs[3], GameVertex.BoneWeights[3]);

            return new VERTEXSKINNEDBUILDER(VB1, TB1, VertexWeights);
        }


        /** Utility Functions */
        public bool Over16BitLimit()
        {
            return (Vertices.Length > ushort.MaxValue);
        }

        public void CalculatePartBounds()
        {
            for (int i = 0; i < FaceGroups.Length; i++)
            {
                List<Vector3> partVerts = new List<Vector3>();
                for (int x = 0; x < Indices.Length; x++)
                {
                    partVerts.Add(Vertices[Indices[i]].Position);
                }

                FaceGroups[i].Bounds = BoundingBox.CreateFromPoints(partVerts.ToArray());
            }
        }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            bool bValidity = true;

            if(Vertices.Length == 0)
            {
               AddMessage(MT_MessageType.Error, "This LOD object has no vertices.");
                bValidity = false;
            }

            if (Indices.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This LOD object has no indices.");
                bValidity = false;
            }

            // specific game type check.
            // M2DE supports 32bit buffers, original game does not.
            if(GameStorage.IsGameType(GamesEnumerator.MafiaII))
            {
                if(Vertices.Length > ushort.MaxValue)
                {
                    AddMessage(MT_MessageType.Error,
                        "Vertex count is above {0}. Importing this model into Mafia II may prove to be unstable!", ushort.MaxValue);
                }
            }

            if (VertexDeclaration == 0)
            {
                AddMessage(MT_MessageType.Error, "This LOD object has no vertex elements.");
                bValidity = false;
            }

            if(FaceGroups.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This LOD object has no FaceGroups");
                bValidity = false;
            }

            foreach (var FaceGroup in FaceGroups)
            {
                bool bIsFaceGroupValid = FaceGroup.ValidateObject(TrackerObject);
                bValidity &= bIsFaceGroupValid;
            }

            return bValidity;
        }
    }
}
