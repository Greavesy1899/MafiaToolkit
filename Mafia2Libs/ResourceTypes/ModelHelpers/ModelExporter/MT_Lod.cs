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

    using RidgedMeshBuilder = MeshBuilder<VertexPositionNormalTangent, VertexTexture1>;
    using SkeletalMeshBuilder = MeshBuilder<VertexPositionNormalTangent, VertexTexture1, VertexJoints4>;

    public class MT_Lod : IValidator
    {
        public VertexFlags VertexDeclaration { get; set; }
        public Vertex[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public MT_FaceGroup[] FaceGroups { get; set; }

        public RidgedMeshBuilder BuildGLTF()
        {
            RidgedMeshBuilder LodMesh = new RidgedMeshBuilder();

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

                    (int A, int B, int C) Result = CurFaceGroup.AddTriangle(BuildRidgedVertex(V1), BuildRidgedVertex(V2), BuildRidgedVertex(V3));
                    if(Result.A == -1 || Result.B == -1 || Result.C == -1)
                    {
                        int z = 0;
                    }
                }
            }

            return LodMesh;
        }

        public SkeletalMeshBuilder BuildSkinnedGLTF()
        {
            SkeletalMeshBuilder LodMesh = new SkeletalMeshBuilder();

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

        public void BuildLodFromGLTFMesh(Mesh InMesh, Skin InSkin)
        {
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

                // TODO: Can we clean this, this shit is dreadful
                IList<Vector3> PosList = null;
                Accessor PositionBuffer = Primitive.GetVertexAccessor("POSITION");
                if(PositionBuffer != null)
                {
                    PosList = PositionBuffer.AsVector3Array();
                    VertexDeclaration |= VertexFlags.Position;
                }

                IList<Vector3> NormList = null;
                Accessor NormalAccessor = Primitive.GetVertexAccessor("NORMAL");
                if (NormalAccessor != null)
                {
                    NormList = NormalAccessor.AsVector3Array();
                    VertexDeclaration |= VertexFlags.Normals;
                }

                IList<Vector4> TanList = null;
                Accessor TangentAccessor = Primitive.GetVertexAccessor("TANGENT");
                if (TangentAccessor != null)
                {
                    TanList = TangentAccessor.AsVector4Array();
                    VertexDeclaration |= VertexFlags.Tangent;
                }

                IList<Vector2> Tex0List = null;
                Accessor TexCoord0Accessor = Primitive.GetVertexAccessor("TEXCOORD_0");
                if (TexCoord0Accessor != null)
                {
                    Tex0List = TexCoord0Accessor.AsVector2Array();
                    VertexDeclaration |= VertexFlags.TexCoords0;
                }

                // query whether we are in a good state to support skinned data in vertex buffer
                IList<Vector4> JointsList = null;
                IList<Vector4> BlendWeightsList = null;
                if (InSkin != null)
                {
                    Accessor JointsAccessor = Primitive.GetVertexAccessor("JOINTS_0");
                    if (JointsAccessor != null)
                    {
                        JointsList = JointsAccessor.AsVector4Array();
                        VertexDeclaration |= VertexFlags.Skin;
                    }

                    Accessor WeightsAccessor = Primitive.GetVertexAccessor("WEIGHTS_0");
                    if (WeightsAccessor != null)
                    {
                        BlendWeightsList = WeightsAccessor.AsVector4Array();
                        VertexDeclaration |= VertexFlags.Skin;
                    }

                    // we can consider the fact we have weighted data
                    if(JointsList.Count > 0 && BlendWeightsList.Count > 0)
                    {
                        VertexDeclaration |= VertexFlags.Skin;
                    }
                }

                uint CurrentOffset = (uint)FinalVertexBuffer.Count;
                Vertex[] TempList = new Vertex[PosList.Count];
                List<uint> IndicesList = new List<uint>();

                // now generate vertex buffer using triangle list
                var TriangleList = Primitive.GetTriangleIndices();
                foreach(var Triangle in TriangleList)
                {
                    void ConvertVertex(int VertexIdx)
                    {
                        TempList[VertexIdx] = new Vertex();

                        if (VertexDeclaration.HasFlag(VertexFlags.Position))
                        {
                            TempList[VertexIdx].Position = PosList[VertexIdx];
                        }

                        if (VertexDeclaration.HasFlag(VertexFlags.Normals))
                        {
                            TempList[VertexIdx].Normal = NormList[VertexIdx];
                        }

                        if (VertexDeclaration.HasFlag(VertexFlags.Tangent))
                        {
                            TempList[VertexIdx].Tangent = new Vector3(TanList[VertexIdx].X, TanList[VertexIdx].Y, TanList[VertexIdx].Z);
                        }

                        if(VertexDeclaration.HasFlag(VertexFlags.TexCoords0))
                        {
                            TempList[VertexIdx].UVs[0] = Tex0List[VertexIdx];
                        }

                        if(VertexDeclaration.HasFlag(VertexFlags.Skin))
                        {
                            // TODO: Validation

                            // pack weights
                            Vector4 VertexWeights = BlendWeightsList[VertexIdx];
                            TempList[VertexIdx].BoneWeights = new float[4] { VertexWeights.X, VertexWeights.Y, VertexWeights.Z, VertexWeights.W };

                            // pack indices
                            Vector4 VertexIndices = JointsList[VertexIdx];
                            TempList[VertexIdx].BoneIDs = new byte[4] { (byte)VertexIndices.X, (byte)VertexIndices.Y, (byte)VertexIndices.Z, (byte)VertexIndices.W };
                        }
                    }

                    ConvertVertex(Triangle.A);
                    ConvertVertex(Triangle.B);
                    ConvertVertex(Triangle.C);

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

            // post load functions
            // TODO: This will be slightly more expensive as we iterate through FaceGroups twice
            for (int Idx = 0; Idx < FaceGroups.Count(); Idx++)
            {
                GenerateFaceGroupBounds(FaceGroups[Idx]);
            }
        }


        /** Utility Functions */
        public bool Over16BitLimit()
        {
            return (Vertices.Length > ushort.MaxValue);
        }

        // TODO: Would prefer this to be private, not public
        public void GenerateFaceGroupBounds(MT_FaceGroup FaceGroup)
        {
            Vector3 MinBounds = new(float.MaxValue);
            Vector3 MaxBounds = new(float.MinValue);

            // iterate through all vertices assigned to this material
            uint StartIndex = FaceGroup.StartIndex;
            uint EndIndex = StartIndex + (FaceGroup.NumFaces * 3);
            for (uint Idx = StartIndex; Idx < EndIndex; Idx++)
            {
                // grab position from vertex buffer
                Vector3 Position = Vertices[Indices[Idx]].Position;

                // update min and max.
                MinBounds = Vector3.Min(MinBounds, Position);
                MaxBounds = Vector3.Max(MaxBounds, Position);
            }

            // assign as new bounds for face group
            FaceGroup.Bounds = new BoundingBox(MinBounds, MaxBounds);
        }

        public BoundingBox GetBoundingBox()
        {
            // inverse the initial bounds so as we iterate through the face group, they'll shrink
            BoundingBox LodBounds = new BoundingBox(new Vector3(float.MaxValue), new Vector3(float.MinValue));

            foreach (MT_FaceGroup FaceGroup in FaceGroups)
            {
                LodBounds = BoundingBox.CreateMerged(LodBounds, FaceGroup.Bounds);
            }

            return LodBounds;
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
