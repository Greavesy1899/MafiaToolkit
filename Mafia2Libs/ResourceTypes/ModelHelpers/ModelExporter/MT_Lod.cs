using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Utils.Models;
using Utils.Settings;
using Utils.StringHelpers;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    using VERTEXRIDGED = SharpGLTF.Geometry.VertexTypes.VertexPositionNormalTangent;
    using VERTEXSKINNED = SharpGLTF.Geometry.VertexTypes.VertexPositionNormalTangent;
    using VERTEXS4 = VertexJoints4;
    using VERTEXRIDGEDBUILDER = VertexBuilder<VertexPositionNormalTangent, VertexTexture1, VertexEmpty>;
    using VERTEXSKINNEDBUILDER = VertexBuilder<VertexPositionNormalTangent, VertexTexture1, VertexJoints4>;

    public class MT_Lod : IValidator
    {
        public VertexFlags VertexDeclaration { get; set; }
        public Vertex[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public MT_FaceGroup[] FaceGroups { get; set; }

        public MeshBuilder<VERTEXRIDGED> BuildGLTF()
        {
            MeshBuilder<VERTEXRIDGED> LodMesh = new MeshBuilder<VERTEXRIDGED>();

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

                    CurFaceGroup.AddTriangle(BuildRidgedVertex(V1), BuildRidgedVertex(V2), BuildRidgedVertex(V3));
                }
            }

            return LodMesh;
        }

        public MeshBuilder<VERTEXSKINNED, VertexTexture1, VertexJoints4> BuildSkinnedGLTF()
        {
            MeshBuilder<VERTEXSKINNED, VertexTexture1, VertexJoints4> LodMesh = new MeshBuilder<VERTEXSKINNED, VertexTexture1, VertexJoints4>();

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

                    CurFaceGroup.AddTriangle(BuildSkinnedVertex(V1), BuildSkinnedVertex(V2), BuildSkinnedVertex(V3));
                }
            }

            return LodMesh;
        }

        private VERTEXRIDGEDBUILDER BuildRidgedVertex(Vertex GameVertex)
        {
            VERTEXRIDGED VB1 = new VERTEXRIDGED(GameVertex.Position, GameVertex.Normal, new Vector4(GameVertex.Tangent, 1.0f));

            VertexTexture1 TB1 = new VertexTexture1(GameVertex.UVs[0]);

            return new VERTEXRIDGEDBUILDER(VB1, TB1);
        }

        private VERTEXSKINNEDBUILDER BuildSkinnedVertex(Vertex GameVertex)
        {
            VERTEXSKINNED VB1 = new VERTEXSKINNED(GameVertex.Position, GameVertex.Normal, new Vector4(GameVertex.Tangent, 1.0f));

            VertexTexture1 TB1 = new VertexTexture1(GameVertex.UVs[0]);

            (int JointIndex, float Weight)[] VertexWeights = new (int JointIndex, float Weight)[4];
            VertexWeights[0] = (GameVertex.BoneIDs[0], GameVertex.BoneWeights[0]);
            VertexWeights[1] = (GameVertex.BoneIDs[1], GameVertex.BoneWeights[1]);
            VertexWeights[2] = (GameVertex.BoneIDs[2], GameVertex.BoneWeights[2]);
            VertexWeights[3] = (GameVertex.BoneIDs[3], GameVertex.BoneWeights[3]);

            return new VERTEXSKINNEDBUILDER(VB1, TB1, VertexWeights);
        }

        public void BuildLodFromGLTFMesh(Mesh InMesh)
        {
            // TODO: Improve vertex declaration detection
            VertexDeclaration |= VertexFlags.Position;

            List<Vertex> FinalVertexBuffer = new List<Vertex>();
            List<uint> FinalIndicesBuffer = new List<uint>();

            FaceGroups = new MT_FaceGroup[InMesh.Primitives.Count];
            for(int Idx = 0; Idx < FaceGroups.Count(); Idx++)
            {
                MeshPrimitive Primitive = InMesh.Primitives[Idx];
                if(Primitive.DrawPrimitiveType != PrimitiveType.TRIANGLES)
                {
                    // must be triangles
                    return;
                }

                MT_FaceGroup NewFaceGroup = new MT_FaceGroup();
                FaceGroups[Idx] = NewFaceGroup;

                // convert material (TODO: Could we use this to determine vertex semantics)
                Material SelectedMaterial = Primitive.Material;
                NewFaceGroup.Material = new MT_MaterialInstance();
                NewFaceGroup.Material.Name = SelectedMaterial.Name;

                // TODO: Include other vertex components in buffer
                Accessor PositionBuffer = Primitive.GetVertexAccessor("POSITION");
                IList<Vector3> PosList = PositionBuffer.AsVector3Array();

                uint CurrentOffset = (uint)FinalVertexBuffer.Count;
                Vertex[] TempList = new Vertex[PosList.Count];
                List<uint> IndicesList = new List<uint>();

                // TODO: Add other Vertex components in buffer
                // TODO: Ensure we're not doing multiple work
                // (eg. redoing a vertex we've already done
                var TriangleList = Primitive.GetTriangleIndices();
                foreach(var Triangle in TriangleList)
                {
                    TempList[Triangle.A] = new Vertex();
                    TempList[Triangle.A].Position = PosList[Triangle.A];

                    TempList[Triangle.B] = new Vertex();
                    TempList[Triangle.B].Position = PosList[Triangle.B];

                    TempList[Triangle.C] = new Vertex();
                    TempList[Triangle.C].Position = PosList[Triangle.C];

                    IndicesList.Add((uint)(CurrentOffset + Triangle.A));
                    IndicesList.Add((uint)(CurrentOffset + Triangle.B));
                    IndicesList.Add((uint)(CurrentOffset + Triangle.C));
                }

                // complete facegroup
                NewFaceGroup.StartIndex = (uint)FinalIndicesBuffer.Count;
                NewFaceGroup.NumFaces = (uint)TriangleList.Count();

                // push facegroup data into mesh buffers
                FinalVertexBuffer.AddRange(TempList);
                FinalIndicesBuffer.AddRange(IndicesList);
            }

            Vertices = FinalVertexBuffer.ToArray();
            Indices = FinalIndicesBuffer.ToArray();
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
