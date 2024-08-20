using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using System.IO;
using System.Numerics;
using Utils.StringHelpers;
using Utils.VorticeUtils;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    using VERTEXCOLLISION = SharpGLTF.Geometry.VertexTypes.VertexPosition;
    using VERTEXCOLLISIONBUILDER = VertexBuilder<VertexPosition, VertexEmpty, VertexEmpty>;

    using CollisionMeshBuilder = MeshBuilder<VertexPosition, VertexEmpty, VertexEmpty>;

    public class MT_Collision : IValidator
    {
        public Vector3[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public MT_FaceGroup[] FaceGroups { get; set; }

        public CollisionMeshBuilder BuildGLTF()
        {
            CollisionMeshBuilder LodMesh = new CollisionMeshBuilder();

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
                    Vector3 V1 = Vertices[Indices[Idx]];
                    Vector3 V2 = Vertices[Indices[Idx + 1]];
                    Vector3 V3 = Vertices[Indices[Idx + 2]];

                    CurFaceGroup.AddTriangle(BuildRidgedVertex(V1), BuildRidgedVertex(V2), BuildRidgedVertex(V3));
                }
            }

            return LodMesh;
        }

        private VERTEXCOLLISIONBUILDER BuildRidgedVertex(Vector3 InPosition)
        {
            VERTEXCOLLISION VB1 = new VERTEXCOLLISION(InPosition);

            return new VERTEXCOLLISIONBUILDER(VB1);
        }

        //~ IValidator Interface
        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            bool bValidity = true;

            if(Vertices.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This collision object has no vertices.");
                bValidity = false;
            }

            if (Indices.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This collision object has no indices");
                bValidity = false;
            }

            if (FaceGroups.Length == 0)
            {
                AddMessage(MT_MessageType.Error, "This collision object has no face groups.");
                bValidity = false;
            }

            foreach(var FaceGroup in FaceGroups)
            {
                bool bIsValid = FaceGroup.ValidateObject(TrackerObject);
                bValidity &= bIsValid;
            }

            return bValidity;
        }
    }
}
