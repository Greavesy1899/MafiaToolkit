using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Utils.Models;
using Utils.StringHelpers;
using Utils.VorticeUtils;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    using VERTEXCOLLISION = SharpGLTF.Geometry.VertexTypes.VertexPosition;
    using VERTEXCOLLISIONBUILDER = VertexBuilder<VertexPosition, VertexEmpty, VertexEmpty>;

    using CollisionMeshBuilder = MeshBuilder<VertexPosition, VertexEmpty, VertexEmpty>;

    public class MT_CollisionInstance
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;
    }

    public class MT_Collision : IValidator
    {
        public Vector3[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public MT_FaceGroup[] FaceGroups { get; set; }
        public MT_CollisionInstance[] Instances { get; set; }

        public MT_Collision()
        {
            Vertices = new Vector3[0];
            Indices = new uint[0];
            FaceGroups = new MT_FaceGroup[0];
            Instances = new MT_CollisionInstance[0];
        }

        public void BuildGLTF(SceneBuilder InScene, NodeBuilder RootNode)
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

            int InstanceIdx = 0;
            foreach(MT_CollisionInstance Instance in Instances)
            {
                NodeBuilder InstanceNode = new NodeBuilder().WithLocalTranslation(Instance.Position).WithLocalScale(Instance.Scale).WithLocalRotation(Instance.Rotation);
                InstanceNode.Name = string.Format("INSTANCE_{0}", InstanceIdx);
                RootNode.AddNode(InstanceNode);

                InScene.AddRigidMesh(LodMesh, InstanceNode);

                InstanceIdx++;
            }
        }

        public void BuildCollisionFromNode(Node RootNode)
        {
            Mesh CollisionMesh = null;

            // Load instances
            List<MT_CollisionInstance> NewInstances = new List<MT_CollisionInstance>();
            foreach (Node ChildNode in RootNode.VisualChildren)
            {
                if (ChildNode.Name.Contains("INSTANCE"))
                {
                    MT_CollisionInstance ColInstance = new MT_CollisionInstance();
                    ColInstance.Position = ChildNode.LocalTransform.Translation;
                    ColInstance.Scale = ChildNode.LocalTransform.Scale;
                    ColInstance.Rotation = ChildNode.LocalTransform.Rotation;

                    NewInstances.Add(ColInstance);

                    CollisionMesh = ChildNode.Mesh;
                }
            }

            Instances = NewInstances.ToArray();

            if (CollisionMesh == null)
            {
                // failure!
                return;
            }

            // load mesh from instances
            List<Vector3> FinalVertexBuffer = new List<Vector3>();
            List<uint> FinalIndicesBuffer = new List<uint>();

            FaceGroups = new MT_FaceGroup[CollisionMesh.Primitives.Count];
            for (int Idx = 0; Idx < FaceGroups.Count(); Idx++)
            {
                MeshPrimitive Primitive = CollisionMesh.Primitives[Idx];
                if (Primitive.DrawPrimitiveType != PrimitiveType.TRIANGLES)
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

                IList<Vector3> PosList = null;
                Accessor PositionBuffer = Primitive.GetVertexAccessor("POSITION");
                if (PositionBuffer != null)
                {
                    PosList = PositionBuffer.AsVector3Array();
                }

                uint CurrentOffset = (uint)FinalVertexBuffer.Count;
                Vector3[] TempList = new Vector3[PosList.Count];
                List<uint> IndicesList = new List<uint>();

                // now generate vertex buffer using triangle list
                var TriangleList = Primitive.GetTriangleIndices();
                foreach (var Triangle in TriangleList)
                {
                    TempList[Triangle.A] = PosList[Triangle.A];
                    TempList[Triangle.B] = PosList[Triangle.B];
                    TempList[Triangle.C] = PosList[Triangle.C];

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
