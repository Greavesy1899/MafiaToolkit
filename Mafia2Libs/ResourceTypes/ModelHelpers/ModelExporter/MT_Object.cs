using Mafia2Tool;
using ResourceTypes.BufferPools;
using ResourceTypes.Collisions;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using SharpGLTF.Geometry;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json.Nodes;
using Utils.Models;
using Utils.StringHelpers;
using Utils.Types;
using Utils.VorticeUtils;
using Collision = ResourceTypes.Collisions.Collision;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    using VERTEX = SharpGLTF.Geometry.VertexTypes.VertexPosition;

    [Flags]
    public enum MT_ObjectFlags
    {
        HasLODs = 1,
        HasSkinning = 2,
        HasCollisions = 4,
        HasChildren = 8
    }

    public enum MT_ObjectType
    {
        Null = 0,
        StaticMesh,
        RiggedMesh,
        Point,
        Actor,
        ItemDesc,
        Dummy,
        StaticCollision,
    }

    public class MT_Object : IValidator
    {
        private const string PROP_OBJECT_TYPE_ID = "MT_OBJECT_TYPE";
        private const string PROP_OBJECT_NAME = "MT_OBJECT_NAME";

        public string ObjectName { get; set; }
        public MT_ObjectFlags ObjectFlags { get; set; }
        public MT_ObjectType ObjectType { get; set; }
        public Vector3 Position { get; set; }
        [Obsolete("This needs to be removed, we must rely on Quats")]
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Quaternion RotationQuat { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;
        public MT_Lod[] Lods { get; set; }
        public MT_Object[] Children { get; set; }
        public MT_Collision Collision { get; set; }
        public MT_Skeleton Skeleton { get; set; }

        public NodeBuilder BuildGLTF(SceneBuilder RootScene, NodeBuilder ParentNode)
        {
            NodeBuilder ThisNode = new NodeBuilder(ObjectName).WithLocalTranslation(Position).WithLocalScale(Scale).WithLocalRotation(RotationQuat);

            if (ParentNode != null)
            {
                ParentNode.AddNode(ThisNode);
            }

            // TODO: Any more required?
            ThisNode.Extras = new JsonObject();
            ThisNode.Extras[PROP_OBJECT_TYPE_ID] = (int)ObjectType;
            ThisNode.Extras[PROP_OBJECT_NAME] = (string)ObjectName;

            if (Lods != null)
            {
                for(int Index = 0; Index < Lods.Length; Index++)
                {
                    NodeBuilder LodNode = ThisNode.CreateNode(string.Format("LOD_{0}", Index));
                    if (Skeleton != null)
                    {
                        var BuiltMesh = Lods[Index].BuildSkinnedGLTF();
                        var SkeletonJoints = Skeleton.BuildGLTF(Index);
                        InstanceBuilder Test = RootScene.AddSkinnedMesh(BuiltMesh, ThisNode.WorldMatrix, SkeletonJoints);
                        
                        LodNode.AddNode(SkeletonJoints[0]);
                    }
                    else
                    {
                        var mesh = Lods[Index].BuildGLTF();
                        RootScene.AddRigidMesh(mesh, LodNode);
                    }
                }
            }

            if (Children != null)
            {
                foreach (MT_Object ChildObject in Children)
                {
                    ChildObject.BuildGLTF(RootScene, ThisNode);
                }
            }

            // TODO: Collisions
            return ThisNode;
        }

        public static MT_Object TryBuildObject(FrameHeaderScene InScene)
        {
            MT_Object NewObject = new MT_Object();
            NewObject.BuildFromScene(InScene);

            return NewObject;
        }

        public static MT_Object TryBuildObject(Collision.CollisionModel InModel)
        {
            MT_Object NewObject = new MT_Object();
            NewObject.BuildFromCollision(InModel);

            return NewObject;
        }

        public static MT_Object TryBuildObject(FrameObjectBase InFrame)
        {
            if(MT_ObjectUtils.GetTypeFromFrame(InFrame) == MT_ObjectType.Point)
            {
                // C_Point is invalid in M_Object.
                // Typically stores an attachment
                // However for MT, attachments are stored in Skeleton
                // When reimporting, the attachment will generate a C_Point
                return null;
            }

            // Construct new MT_Object
            MT_Object NewObject = new MT_Object();
            NewObject.ObjectName = InFrame.Name.ToString();

            // Check if this is a single mesh. If not, build as standard.
            FrameObjectSingleMesh CastedMesh = (InFrame as FrameObjectSingleMesh);
            if (CastedMesh != null)
            {
                // TODO: Remove access of SceneData, Accessing buffer pools will end up becoming deprecated.
                IndexBuffer[] ChildIBuffers = new IndexBuffer[CastedMesh.Geometry.LOD.Length];
                VertexBuffer[] ChildVBuffers = new VertexBuffer[CastedMesh.Geometry.LOD.Length];

                //we need to retrieve buffers first.
                for (int c = 0; c < CastedMesh.Geometry.LOD.Length; c++)
                {
                    ChildIBuffers[c] = SceneData.IndexBufferPool.GetBuffer(CastedMesh.Geometry.LOD[c].IndexBufferRef.Hash);
                    ChildVBuffers[c] = SceneData.VertexBufferPool.GetBuffer(CastedMesh.Geometry.LOD[c].VertexBufferRef.Hash);
                }

                FrameObjectModel CastedModel = (InFrame as FrameObjectModel);
                if(CastedModel != null)
                {
                    NewObject.BuildFromCooked(CastedModel, ChildVBuffers, ChildIBuffers);
                }
                else
                {
                    NewObject.BuildFromCooked(CastedMesh, ChildVBuffers, ChildIBuffers);
                }               
            }
            else
            {
                NewObject.BuildStandardObject(InFrame);
            }

            Vector3 Position = Vector3.Zero;
            Vector3 Scale = Vector3.One;
            Quaternion Rotation = Quaternion.Identity;
            Matrix4x4.Decompose(InFrame.LocalTransform, out Scale, out Rotation, out Position);
            NewObject.Position = Position;
            NewObject.Scale = Vector3.One;
            NewObject.Rotation = Rotation.ToEuler();
            NewObject.RotationQuat = Rotation;

            return NewObject;
        }

        public static MT_Object TryBuildFromNode(Node CurrentNode)
        {
            if (CurrentNode.Extras == null)
            {
                // we ignore any node without our MT data
                return null;
            }

            JsonNode ObjectTypeNode = CurrentNode.Extras[PROP_OBJECT_TYPE_ID];
            if(ObjectTypeNode == null)
            {
                // we have extra but not the object type, thus invalid node
                return null;
            }

            MT_ObjectType DesiredType = (MT_ObjectType)ObjectTypeNode.GetValue<int>();
            if(DesiredType == MT_ObjectType.Null)
            {
                // not valid type found
                return null;
            }

            MT_Object NewObject = new MT_Object();
            NewObject.ObjectName = CurrentNode.Name;
            NewObject.ObjectType = DesiredType;
            NewObject.Position = CurrentNode.LocalTransform.Translation;
            NewObject.RotationQuat = CurrentNode.LocalTransform.Rotation;
            NewObject.Rotation = NewObject.RotationQuat.ToEuler();
            NewObject.Scale = CurrentNode.LocalTransform.Scale;

            List<MT_Object> ImportedObjects = new List<MT_Object>();
            foreach(Node ChildNode in CurrentNode.VisualChildren)
            {
                MT_Object PotentialChildObject = MT_Object.TryBuildFromNode(ChildNode);
                if(PotentialChildObject != null)
                {
                    ImportedObjects.Add(PotentialChildObject);
                }
                else
                {
                    if (ChildNode.Name.Contains("LOD_0"))
                    {
                        if (DesiredType == MT_ObjectType.RiggedMesh)
                        {
                            // rigged models have mesh as child in LOD node
                            // This is a limitation in GLTF where the skinned mesh is attached to first node in joint array
                            if(ChildNode.VisualChildren.Count() > 0)
                            {
                                foreach (Node SubmeshNode in ChildNode.VisualChildren)
                                {
                                    Mesh AssociatedMesh = ChildNode.VisualChildren.ElementAt(0).Mesh;
                                }
                            }

                        }
                        else if(DesiredType == MT_ObjectType.StaticMesh)
                        {
                            // Default meshes are still attached to LOD
                            // TODO: Should we change this...?
                            Mesh AssociatedMesh = ChildNode.Mesh;
                            if (AssociatedMesh != null)
                            {
                                // build lod
                                MT_Lod NewLod = new MT_Lod();
                                NewLod.BuildLodFromGLTFMesh(AssociatedMesh);
                            }
                        }
                    }
                }
            }

            NewObject.Children = ImportedObjects.ToArray();
            NewObject.ObjectFlags |= MT_ObjectFlags.HasChildren;

            return NewObject;
        }

        public void Accept(IVisitor InVisitor)
        {
            InVisitor.Accept(this);

            if(ObjectFlags.HasFlag(MT_ObjectFlags.HasChildren))
            {
                foreach(MT_Object Child in Children)
                {
                    InVisitor.Accept(Child);
                }
            }
        }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            bool bValidity = true;

            if(string.IsNullOrEmpty(ObjectName))
            {
                AddMessage(MT_MessageType.Error, "This Object has no name.");
                bValidity = false;
            }

            if(ObjectFlags == 0)
            {
                AddMessage(MT_MessageType.Error, "This Object has no available flags.");
                bValidity = false;
            }

            if(ObjectType == MT_ObjectType.Null)
            {
                AddMessage(MT_MessageType.Error, "This Object has no valid type and will probably crash the Toolkit!");
                bValidity = false;
            }

            if(ObjectFlags.HasFlag(MT_ObjectFlags.HasLODs))
            {
                foreach (var LodObject in Lods)
                {
                    bool bIsLodValid = LodObject.ValidateObject(TrackerObject);
                    bValidity &= bIsLodValid;
                }
            }

            if (ObjectFlags.HasFlag(MT_ObjectFlags.HasChildren))
            {
                foreach (var ChildObject in Children)
                {
                    bool bIsChildValid = ChildObject.ValidateObject(TrackerObject);
                    bValidity &= bIsChildValid;
                }
            }

            if (ObjectFlags.HasFlag(MT_ObjectFlags.HasCollisions))
            {
                bool bIsColValid = Collision.ValidateObject(TrackerObject);
                bValidity &= bIsColValid;
            }

            if (ObjectFlags.HasFlag(MT_ObjectFlags.HasSkinning))
            {
                bool bIsSkelValid = Skeleton.ValidateObject(TrackerObject);
                bValidity &= bIsSkelValid;
            }

            return bValidity;
        }

        /** Internal functions */
        /** Construction Functions */
        private void BuildFromCooked(FrameObjectSingleMesh SingleMesh, VertexBuffer[] VBuffer, IndexBuffer[] IBuffer)
        {
            BuildStandardObject(SingleMesh);

            FrameGeometry GeometryInfo = SingleMesh.Geometry;
            FrameMaterial MaterialInfo = SingleMesh.Material;
            ObjectFlags |= MT_ObjectFlags.HasLODs;

            Lods = new MT_Lod[GeometryInfo.LOD.Length];
            for (int i = 0; i < Lods.Length; i++)
            {
                // Setup and Collect Lod Info and buffers
                FrameLOD LodInfo = GeometryInfo.LOD[i];
                MT_Lod LodObject = new MT_Lod();

                LodObject.VertexDeclaration = LodInfo.VertexDeclaration;
                IndexBuffer CurrentIBuffer = IBuffer[i];
                VertexBuffer CurrentVBuffer = VBuffer[i];

                // Get Vertex sizes and declaration
                int VertexSize = 0;
                Dictionary<VertexFlags, FrameLOD.VertexOffset> vertexOffsets = LodInfo.GetVertexOffsets(out VertexSize);
                LodObject.Vertices = new Vertex[LodInfo.NumVerts];

                if (VertexSize * LodInfo.NumVerts != CurrentVBuffer.Data.Length)
                {
                    Console.WriteLine("BIG ERROR");
                }

                for (int v = 0; v < LodObject.Vertices.Length; v++)
                {
                    //declare data required and send to decompresser
                    byte[] data = new byte[VertexSize];
                    Array.Copy(CurrentVBuffer.Data, (v * VertexSize), data, 0, VertexSize);
                    LodObject.Vertices[v] = VertexTranslator.DecompressVertex(data, LodInfo.VertexDeclaration, GeometryInfo.DecompressionOffset, GeometryInfo.DecompressionFactor, vertexOffsets);
                }

                // Build Indices and FaceGroups Array
                LodObject.Indices = CurrentIBuffer.GetData();
                MaterialStruct[] FaceGroups = MaterialInfo.Materials[i];

                // NB: We will skip the FaceGroups which don't have faces
                List<MT_FaceGroup> Groups = new List<MT_FaceGroup>();
                for (int v = 0; v < FaceGroups.Length; v++)
                {
                    if (FaceGroups[v].NumFaces == 0)
                    {
                        continue;
                    }

                    MT_FaceGroup FaceGroupObject = new MT_FaceGroup();
                    MT_MaterialInstance MaterialInstanceObject = new MT_MaterialInstance();

                    // TODO: Might be better to just keep this permanently.
                    //if(string.IsNullOrEmpty(FaceGroups[v].MaterialName))
                    //{
                    var Material = MaterialsManager.LookupMaterialByHash(FaceGroups[v].MaterialHash);
                    FaceGroups[v].MaterialName = Material.GetMaterialName();
                    FaceGroups[v].MaterialHash = Material.GetMaterialHash();

                    // Add texture (if applicable)
                    HashName DiffuseHashName = Material.GetTextureByID("S000");
                    if (DiffuseHashName != null)
                    {
                        MaterialInstanceObject.DiffuseTexture = DiffuseHashName.String;
                        MaterialInstanceObject.MaterialFlags |= MT_MaterialInstanceFlags.HasDiffuse;
                    }
                    //}

                    MaterialInstanceObject.Name = FaceGroups[v].MaterialName;
                    FaceGroupObject.StartIndex = (uint)FaceGroups[v].StartIndex;
                    FaceGroupObject.NumFaces = (uint)FaceGroups[v].NumFaces;
                    FaceGroupObject.Material = MaterialInstanceObject;
                    Groups.Add(FaceGroupObject);
                }

                LodObject.FaceGroups = Groups.ToArray();
                Lods[i] = LodObject;
            }
        }

        private void BuildFromCooked(FrameObjectModel RiggedModel, VertexBuffer[] VBuffer, IndexBuffer[] IBuffer)
        {
            BuildFromCooked((FrameObjectSingleMesh)RiggedModel, VBuffer, IBuffer);

            MT_Skeleton ModelSkeleton = new MT_Skeleton();

            FrameBlendInfo BlendInfo = RiggedModel.GetBlendInfoObject();
            FrameSkeleton Skeleton = RiggedModel.GetSkeletonObject();
            FrameSkeletonHierachy SkeletonHierarchy = RiggedModel.GetSkeletonHierarchyObject();

            ModelSkeleton.Joints = new MT_Joint[Skeleton.BoneNames.Length];
            for (int i = 0; i < ModelSkeleton.Joints.Length; i++)
            {
                MT_Joint JointObject = new MT_Joint();
                JointObject.Name = Skeleton.BoneNames[i].ToString();
                JointObject.ParentJointIndex = SkeletonHierarchy.ParentIndices[i];
                JointObject.UsageFlags = Skeleton.BoneLODUsage[i];

                Vector3 Scale, Position;
                Quaternion Rotation;

                Matrix4x4 JointTransform = Skeleton.JointTransforms[i];
                Matrix4x4.Decompose(JointTransform, out Scale, out Rotation, out Position);
                JointObject.Position = Position;
                JointObject.Scale = Scale;
                JointObject.Rotation = Rotation;
                ModelSkeleton.Joints[i] = JointObject;
            }

            // we do not apply attachments to skeleton but apply in GLTF pipeline
            ModelSkeleton.Attachments = new MT_Attachment[RiggedModel.AttachmentReferences.Length];
            for (int i = 0; i < ModelSkeleton.Attachments.Length; i++)
            {
                MT_Attachment NewAttachment = new MT_Attachment();
                NewAttachment.Name = RiggedModel.AttachmentReferences[i].Attachment.Name.ToString();
                NewAttachment.JointIndex = RiggedModel.AttachmentReferences[i].JointIndex;
                ModelSkeleton.Attachments[i] = NewAttachment;
            }

            for (int i = 0; i < BlendInfo.BoneIndexInfos.Length; i++)
            {
                var indexInfos = BlendInfo.BoneIndexInfos[i];
                var lod = Lods[i];
                bool[] remapped = new bool[lod.Vertices.Length];
                for (int x = 0; x < indexInfos.NumMaterials; x++)
                {
                    var part = lod.FaceGroups[x];
                    byte offset = 0;
                    for (int s = 0; s < indexInfos.BonesSlot[x]; s++)
                    {
                        offset += indexInfos.BonesPerPool[s];
                    }

                    for (uint z = part.StartIndex; z < part.StartIndex + (part.NumFaces * 3); z++)
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

            ObjectFlags |= MT_ObjectFlags.HasSkinning;
            this.Skeleton = ModelSkeleton;
        }

        private void BuildFromCollision(Collision.CollisionModel CollisionObject)
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;

            if (CollisionObject == null)
            {
                // Failed
                return;
            }

            ObjectFlags |= MT_ObjectFlags.HasCollisions;
            ObjectName = CollisionObject.Hash.ToString();

            Collision = new MT_Collision();

            TriangleMesh TriMesh = CollisionObject.Mesh;

            // Copy vertices to our array
            Collision.Vertices = new Vector3[TriMesh.Vertices.Count];
            TriMesh.Vertices.CopyTo(Collision.Vertices, 0);

            // sort materials in order:
            // MTO doesn't support unorganised triangles, only triangles in order by material.
            // basically like mafia itself, so we have to reorder them and then save.
            // this doesn't mess anything up, just takes a little longer :)
            Dictionary<string, List<uint>> SortedMats = new Dictionary<string, List<uint>>();
            for (int i = 0; i < TriMesh.MaterialIndices.Count; i++)
            {
                string mat = ((CollisionMaterials)TriMesh.MaterialIndices[i]).ToString();
                if (!SortedMats.ContainsKey(mat))
                {
                    List<uint> list = new List<uint>();
                    list.Add(TriMesh.Triangles[i].v0);
                    list.Add(TriMesh.Triangles[i].v1);
                    list.Add(TriMesh.Triangles[i].v2);
                    SortedMats.Add(mat, list);
                }
                else
                {
                    SortedMats[mat].Add(TriMesh.Triangles[i].v0);
                    SortedMats[mat].Add(TriMesh.Triangles[i].v1);
                    SortedMats[mat].Add(TriMesh.Triangles[i].v2);
                }
            }

            Collision.FaceGroups = new MT_FaceGroup[SortedMats.Count];
            List<uint> inds = new List<uint>();
            for (int x = 0; x < Collision.FaceGroups.Length; x++)
            {
                MT_FaceGroup FaceGroupObject = new MT_FaceGroup();
                FaceGroupObject.StartIndex = (uint)inds.Count;
                inds.AddRange(SortedMats.ElementAt(x).Value);
                FaceGroupObject.NumFaces = (uint)(SortedMats.ElementAt(x).Value.Count / 3);

                MT_MaterialInstance MaterialInstance = new MT_MaterialInstance();
                MaterialInstance.MaterialFlags = MT_MaterialInstanceFlags.IsCollision;
                MaterialInstance.Name = SortedMats.ElementAt(x).Key;

                FaceGroupObject.Material = MaterialInstance;
                Collision.FaceGroups[x] = FaceGroupObject;
            }

            // Copy sorted triangles in our collision object
            Collision.Indices = inds.ToArray();
        }

        private void BuildStandardObject(FrameObjectBase FrameObject)
        {
            // TODO - Possibly add an option where we can ask to export with local transform?
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;

            // Convert type to enumerator
            ObjectType = MT_ObjectUtils.GetTypeFromFrame(FrameObject);

            // Avoid calling if we have no children
            if (FrameObject.Children.Count > 0)
            {
                AddFrameChildrenToObject(FrameObject.Children);
            }
        }

        private void BuildFromScene(FrameHeaderScene Scene)
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;

            // Force a dummy
            ObjectType = MT_ObjectType.Dummy;

            // Avoid calling if we have no children
            if (Scene.Children.Count > 0)
            {
                AddFrameChildrenToObject(Scene.Children);
            }
        }

        private void AddFrameChildrenToObject(List<FrameObjectBase> InChildren)
        {
            // Export Children
            ObjectFlags |= MT_ObjectFlags.HasChildren;
            List<MT_Object> TempChildren = new List<MT_Object>();
            for (int i = 0; i < InChildren.Count; i++)
            {
                // Cache child object
                MT_Object NewObject = MT_Object.TryBuildObject(InChildren[i]);
                if(NewObject != null)
                {
                    // Slot into array
                    TempChildren.Add(NewObject);
                }
            }

            Children = TempChildren.ToArray();
        }

        public override string ToString()
        {
            return ObjectName;
        }
    }
}
