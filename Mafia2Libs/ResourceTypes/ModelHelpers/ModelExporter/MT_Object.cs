using ResourceTypes.BufferPools;
using ResourceTypes.Collisions;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json.Nodes;
using System.Windows.Media.Media3D;
using Utils.Models;
using Utils.Types;
using Utils.VorticeUtils;
using Collision = ResourceTypes.Collisions.Collision;
using Quaternion = System.Numerics.Quaternion;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    [Flags]
    public enum MT_ObjectFlags
    {
        HasLODs = 1,
        HasSkinning = 2,
        HasCollisions = 4,
        HasChildren = 8,
        AddToFrameNameTable = 16,
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
        Scene,
    }

    public class MT_Object : IValidator
    {
        private const string PROP_OBJECT_TYPE_ID = "MT_OBJECT_TYPE";
        private const string PROP_OBJECT_NAME = "MT_OBJECT_NAME";
        private const string PROP_OBJECT_ON_FRT = "MT_ON_NAME_TABLE";
        private const string PROP_OBJECT_FRT_FLAGS = "MT_NAME_TABLE_FLAGS";

        public string ObjectName { get; set; }
        public MT_ObjectFlags ObjectFlags { get; set; }
        public MT_ObjectType ObjectType { get; set; }
        public Vector3 Position { get; set; }
        [Obsolete("This needs to be removed, we must rely on Quats")]
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Quaternion RotationQuat { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;
        public int FrameNameTableFlags { get; set; } = 0;
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

            if(ObjectFlags.HasFlag(MT_ObjectFlags.AddToFrameNameTable))
            {
                ThisNode.Extras[PROP_OBJECT_FRT_FLAGS] = (int)FrameNameTableFlags;
                ThisNode.Extras[PROP_OBJECT_ON_FRT] = (bool)true;
            }

            if (Lods != null)
            {
                // TODO: Fix LODs
                // It's simply too noisy to debug right now, we need 1 LOD working first.
                for(int Index = 0; Index < 1; Index++)
                {
                    NodeBuilder LodNode = ThisNode.CreateNode(string.Format("LOD_{0}", Index));
                    if (Skeleton != null)
                    {
                        var BuiltMesh = Lods[Index].BuildSkinnedGLTF();
                        NodeBuilder[] SkeletonJoints = Skeleton.BuildGLTF(Index);
                        RootScene.AddSkinnedMesh(BuiltMesh, ThisNode.WorldMatrix, SkeletonJoints);
                        
                        LodNode.AddNode(SkeletonJoints[0]);
                    }
                    else
                    {
                        var mesh = Lods[Index].BuildGLTF();
                        RootScene.AddRigidMesh(mesh, LodNode);
                    }
                }
            }

            if(Collision != null)
            {
                Collision.BuildGLTF(RootScene, ThisNode);
            }

            if (Children != null)
            {
                foreach (MT_Object ChildObject in Children)
                {
                    ChildObject.BuildGLTF(RootScene, ThisNode);
                }
            }

            return ThisNode;
        }

        public static MT_Object TryBuildObject(FrameHeaderScene InScene)
        {
            MT_Object NewObject = new MT_Object();
            NewObject.BuildFromScene(InScene);

            return NewObject;
        }

        public static MT_Object TryBuildObject(Collision.CollisionModel InModel, Collision.Placement[] InPlacements)
        {
            MT_Object NewObject = new MT_Object();
            NewObject.BuildFromCollision(InModel, InPlacements);

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
            
            // if Frame was on FrameNameTable, we can also add this to our MT_Object.
            // In the GLTF, we'll add as extra properties
            if(InFrame.IsOnFrameTable)
            {
                NewObject.FrameNameTableFlags = (int)InFrame.FrameNameTableFlags;
                NewObject.ObjectFlags |= MT_ObjectFlags.AddToFrameNameTable;
            }

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
                    ChildIBuffers[c] = CastedMesh.GetIndexBuffer(c);
                    ChildVBuffers[c] = CastedMesh.GetVertexBuffer(c);
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

        public static MT_Object TryBuildFromNode(Node CurrentNode, MT_Logger Logger)
        {
            Logger.WriteInfo("Started on Node: [{0}]", CurrentNode.Name);

            int ObjectTypeID = -1;
            if(!GLTFDefines.GetValueFromNode<int>(CurrentNode, PROP_OBJECT_TYPE_ID, out ObjectTypeID))
            {
                Logger.WriteError("Failed to find property [{0}] on node [{1}] cannot determine type.", PROP_OBJECT_TYPE_ID, CurrentNode.Name);
                return null;
            }

            MT_ObjectType DesiredType = (MT_ObjectType)ObjectTypeID;
            if(DesiredType == MT_ObjectType.Null)
            {
                Logger.WriteError("The value [{0}] assigned to [{1}] is invalid on node [{2}]", DesiredType, PROP_OBJECT_TYPE_ID, CurrentNode.Name);
                return null;
            }

            // attempt to import from optional node
            string DesiredName = CurrentNode.Name;
            if(GLTFDefines.GetValueFromNode<string>(CurrentNode, PROP_OBJECT_NAME, out DesiredName))
            {
                Logger.WriteInfo("Detected [{0}], assigning name [{1}]", PROP_OBJECT_NAME, CurrentNode.Name);
            }

            MT_Object NewObject = new MT_Object();
            NewObject.ObjectName = DesiredName;
            NewObject.ObjectType = DesiredType;
            NewObject.Position = CurrentNode.LocalTransform.Translation;
            NewObject.RotationQuat = CurrentNode.LocalTransform.Rotation;
            NewObject.Rotation = NewObject.RotationQuat.ToEuler();
            NewObject.Scale = CurrentNode.LocalTransform.Scale;

            // see if we need to store frame name table data
            if (GLTFDefines.GetValueFromNode<bool>(CurrentNode, PROP_OBJECT_ON_FRT, out bool bOnTable))
            {
                if (GLTFDefines.GetValueFromNode<int>(CurrentNode, PROP_OBJECT_FRT_FLAGS, out int Flags))
                {
                    // apply flags to this object
                    NewObject.FrameNameTableFlags = Flags;

                    Logger.WriteInfo("Detected FrameNameTable flags [{0}] to Node [{1}]", Flags, CurrentNode.Name);
                }
            }

            if(NewObject.ObjectType == MT_ObjectType.StaticCollision)
            {
                NewObject.Collision = new MT_Collision();
            }

            List<MT_Object> ImportedObjects = new List<MT_Object>();
            List<MT_Lod> ObjectLods = new List<MT_Lod>();
            foreach(Node ChildNode in CurrentNode.VisualChildren)
            {
                MT_Object PotentialChildObject = MT_Object.TryBuildFromNode(ChildNode, Logger);
                if(PotentialChildObject != null)
                {
                    ImportedObjects.Add(PotentialChildObject);
                }
                else
                {
                    // TODO: This is a very loose coupling, can we somehow improve this?
                    // Perhaps within the Extras of a node, we can identify that this is LOD0?
                    if (ChildNode.Name.Contains("LOD_0"))
                    {
                        if (DesiredType == MT_ObjectType.RiggedMesh)
                        {
                            // rigged models have mesh as child in LOD node
                            // This is a limitation in GLTF where the skinned mesh is attached to first node in joint array
                            // TODO: Fix SharpGLTF export -> Blender export works fine, which is a first tbh
                            if(ChildNode.VisualChildren.Count() > 0)
                            {
                                foreach (Node SubmeshNode in ChildNode.VisualChildren)
                                {
                                    Mesh FoundMesh = SubmeshNode.Mesh;
                                    Skin FoundSkin = SubmeshNode.Skin;
                                    if(FoundMesh != null && FoundSkin != null)
                                    {
                                        // build lod
                                        MT_Lod NewLod = new MT_Lod();
                                        NewLod.BuildLodFromGLTFMesh(FoundMesh, FoundSkin);
                                        ObjectLods.Add(NewLod);

                                        // then build skeleton
                                        NewObject.Skeleton = new MT_Skeleton();
                                        NewObject.Skeleton.BuildSkeletonFromGLTF(FoundSkin);

                                        // TODO: Attachments. Remember how they are added into GLTF to begin with
                                        // Then somehow add them into the MT_Skeleton.
                                    }
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
                                NewLod.BuildLodFromGLTFMesh(AssociatedMesh, null);
                                ObjectLods.Add(NewLod);
                            }
                        }
                    }
                    else if(ChildNode.Name.Contains("COLINSTANCE"))
                    {
                        if (DesiredType == MT_ObjectType.StaticCollision)
                        {
                            NewObject.Collision.BuildCollisionFromNode(ChildNode);
                        }
                    }
                }
            }

            // we have lods so ensure they are added to object
            // (and that the flag is set)
            if(ObjectLods.Count > 0)
            {
                NewObject.Lods = ObjectLods.ToArray();
                NewObject.ObjectFlags |= MT_ObjectFlags.HasLODs;
            }

            // we have child objects to ensure that they are added to the object
            // (and that the file is set)
            if(ImportedObjects.Count > 0)
            {
                NewObject.Children = ImportedObjects.ToArray();
                NewObject.ObjectFlags |= MT_ObjectFlags.HasChildren;
            }

            if(NewObject.Collision != null)
            {
                NewObject.ObjectFlags |= MT_ObjectFlags.HasCollisions;
            }

            if(NewObject.Skeleton != null)
            {
                NewObject.ObjectFlags |= MT_ObjectFlags.HasSkinning;
            }

            Logger.WriteInfo("Created MT_Object [{0}], type [{1}], from node [{2}]", NewObject.ObjectName, NewObject.ObjectType, CurrentNode.Name);

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

                Matrix4x4 JointTransform = Skeleton.JointTransforms[i];
                Matrix4x4.Decompose(JointTransform, out Vector3 Scale, out Quaternion Rotation, out Vector3 Position);
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

            // Mafia II models do not store direct reference to Bone IDs in vertex buffer.
            // It's more like an "Index to Direct" model, similar to how FBX sometimes sets up buffers.
            // With that said, by using the FrameBlendInfo, we can remap the Bone ID to be direct.
            // I'm assuming it could be to do with 127 limit? But then why could we store [0-255] range in buffers...
            // FrameBlendInfo explicitly defines number of bones per material.
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

        private void BuildFromCollision(Collision.CollisionModel CollisionObject, Collision.Placement[] InPlacements)
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            RotationQuat = Quaternion.Identity;
            Scale = Vector3.One;

            if (CollisionObject == null)
            {
                // Failed
                return;
            }

            ObjectFlags |= MT_ObjectFlags.HasCollisions;
            ObjectName = CollisionObject.Hash.ToString();
            ObjectType = MT_ObjectType.StaticCollision;

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

            // convert placements to instances
            Collision.Instances = new MT_CollisionInstance[InPlacements.Length];
            for(int Idx = 0; Idx < InPlacements.Length;  Idx++)
            {
                MT_CollisionInstance ColInstance = new MT_CollisionInstance();
                ColInstance.Position = InPlacements[Idx].Position;
                ColInstance.Scale = Vector3.One;
                ColInstance.Rotation = Quaternion.Identity;

                Collision.Instances[Idx] = ColInstance;
            }
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
            ObjectName = Scene.Name.ToString();
            ObjectType = MT_ObjectType.Scene;

            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;

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
