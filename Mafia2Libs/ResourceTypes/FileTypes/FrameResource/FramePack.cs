using System;
using Mafia2Tool;
using ResourceTypes.BufferPools;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Utils.Extensions;
using Utils.Settings;

namespace ResourceTypes.FrameResource
{
    /*
     * Toolkit Creation to give the user the ability to export sections of frames and re-import into another file.
     */
    public class FramePack
    {
        // Used for both
        private Dictionary<ulong, List<int>> ModelAttachments;
        private FrameResource OwningResource;

        // Used for loading
        private Dictionary<int, FrameObjectBase> OldRefIDLookupTable;

        // Used when saving
        public FrameObjectBase RootFrame { get; private set; }
        private Dictionary<int, object> FrameObjects;
        private Dictionary<int, FrameBlendInfo> FrameBlendInfos;
        private Dictionary<int, FrameSkeleton> FrameSkeletons;
        private Dictionary<int, FrameSkeletonHierachy> FrameSkeletonHierarchy;
        private Dictionary<int, FrameMaterial> FrameMaterials;
        private Dictionary<int, FrameGeometry> FrameGeometries;

        public FramePack(FrameResource OwningResource)
        {
            this.OwningResource = OwningResource;
        }

        private void SaveFrame(FrameObjectBase frame, BinaryWriter writer)
        {
            //is this even needed? hmm.
            writer.Write(frame.RefID); // Save old RefID
            Debug.WriteLine(frame.ToString());
            if (frame.GetType() == typeof(FrameObjectArea))
            {
                writer.Write((ushort)FrameResourceObjectType.Area);
                (frame as FrameObjectArea).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectCamera))
            {
                writer.Write((ushort)FrameResourceObjectType.Camera);
                (frame as FrameObjectCamera).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectCollision))
            {
                writer.Write((ushort)FrameResourceObjectType.Collision);
                (frame as FrameObjectCollision).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectComponent_U005))
            {
                writer.Write((ushort)FrameResourceObjectType.Component_U00000005);
                (frame as FrameObjectComponent_U005).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectDummy))
            {
                writer.Write((ushort)FrameResourceObjectType.Dummy);
                (frame as FrameObjectDummy).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectDeflector))
            {
                writer.Write((ushort)FrameResourceObjectType.ParticleDeflector);
                (frame as FrameObjectDeflector).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectFrame))
            {
                writer.Write((ushort)FrameResourceObjectType.Frame);
                (frame as FrameObjectFrame).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectPoint))
            {
                writer.Write((ushort)FrameResourceObjectType.Point);
                (frame as FrameObjectJoint).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectLight))
            {
                writer.Write((ushort)FrameResourceObjectType.Light);
                (frame as FrameObjectLight).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectModel))
            {
                var mesh = (frame as FrameObjectModel);
                writer.Write((ushort)FrameResourceObjectType.Model);
                mesh.WriteToFilePart1(writer);
                mesh.Geometry.WriteToFile(writer);
                mesh.Material.WriteToFile(writer);
                mesh.BlendInfo.WriteToFile(writer);
                mesh.Skeleton.WriteToFile(writer);
                mesh.SkeletonHierarchy.WriteToFile(writer);
                mesh.WriteToFilePart2(writer);

                // Write Attachment hashes to the dictionary
                List<int> AttachmentHashes = new List<int>();
                foreach(FrameObjectModel.AttachmentReference Attachment in mesh.AttachmentReferences)
                {
                    AttachmentHashes.Add(Attachment.Attachment.RefID);
                }

                ModelAttachments.Add(mesh.Name.Hash, AttachmentHashes);

                foreach (var lod in mesh.Geometry.LOD)
                {
                    using (var stream = new MemoryStream())
                    {//todo: next two lines and their duplicates need to be rewritten to not use scenedata
                        OwningResource.SceneData.IndexBufferPool.GetBuffer(lod.IndexBufferRef.Hash).WriteToFile(stream, false);
                        OwningResource.SceneData.VertexBufferPool.GetBuffer(lod.VertexBufferRef.Hash).WriteToFile(stream, false);
                        writer.Write(stream.ToArray());
                    }
                }
            }
            else if (frame.GetType() == typeof(FrameObjectSector))
            {
                writer.Write((ushort)FrameResourceObjectType.Sector);
                (frame as FrameObjectSector).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectSingleMesh))
            {
                var mesh = (frame as FrameObjectSingleMesh);
                writer.Write((ushort)FrameResourceObjectType.SingleMesh);
                mesh.WriteToFile(writer);
                mesh.Geometry.WriteToFile(writer);
                mesh.Material.WriteToFile(writer);

                foreach (var lod in mesh.Geometry.LOD)
                {
                    using (var stream = new MemoryStream())
                    {
                        OwningResource.SceneData.IndexBufferPool.GetBuffer(lod.IndexBufferRef.Hash).WriteToFile(stream, false);
                        OwningResource.SceneData.VertexBufferPool.GetBuffer(lod.VertexBufferRef.Hash).WriteToFile(stream, false);
                        writer.Write(stream.ToArray());
                    }
                }
            }
            else if (frame.GetType() == typeof(FrameObjectTarget))
            {
                writer.Write((ushort)FrameResourceObjectType.Target);
                (frame as FrameObjectTarget).WriteToFile(writer);
            }
            else
            {
                writer.Write(frame.Type);
                frame.WriteToFile(writer);
            }

            // write FrameNameTable info
            writer.Write(frame.IsOnFrameTable);
            writer.Write((uint)frame.FrameNameTableFlags);

            // Write ParentIndex1 and ParentIndex2 info
            writer.Write(frame.ParentIndex1.RefID);
            writer.Write(frame.ParentIndex2.RefID);

            writer.Write(frame.Children.Count);
            for (int i = 0; i < frame.Children.Count; i++)
            {
                SaveFrame(frame.Children[i], writer);
            }
        }

        private FrameObjectBase ReadFrame(MemoryStream stream)
        {
            int OldRefID = stream.ReadInt32(false); // read old RefID so we can make lookup dictionary

            FrameResourceObjectType frameType = (FrameResourceObjectType)stream.ReadInt16(false);
            FrameObjectBase parent = FrameFactory.ReadFrameByObjectID(stream, false, OwningResource, frameType);
            Debug.WriteLine(parent.ToString());

            if (parent is FrameObjectSingleMesh || parent is FrameObjectModel)
            {
                FrameObjectSingleMesh SingleMesh = (parent as FrameObjectSingleMesh);

                // Read the required blocks;
                FrameGeometry geometry = SingleMesh.GetGeometry();
                geometry.ReadFromFile(stream, false);
                FrameMaterial material = SingleMesh.GetMaterial();
                material.ReadFromFile(stream, false);

                if (parent is FrameObjectModel)
                {
                    FrameObjectModel RiggedMesh = (parent as FrameObjectModel);

                    // Read the rigged specific blocks
                    FrameBlendInfo blendInfo = RiggedMesh.BlendInfo;
                    blendInfo.ReadFromFile(stream, false);
                    FrameSkeleton skeleton = RiggedMesh.Skeleton;
                    skeleton.ReadFromFile(stream, false);
                    FrameSkeletonHierachy hierarchy = RiggedMesh.SkeletonHierarchy;
                    hierarchy.ReadFromFile(stream, false);

                    // read end of mesh
                    RiggedMesh.ReadFromFilePart2(stream, false);
                }

                // We have to make sure we have index and buffer pools available
                // We have to do it for all LODs too; if any more than 1.
                foreach (var lod in geometry.LOD)
                {
                    IndexBuffer indexBuffer = new IndexBuffer(stream, false);
                    VertexBuffer vertexBuffer = new VertexBuffer(stream, false);

                    OwningResource.SceneData.IndexBufferPool.TryAddBuffer(indexBuffer);
                    OwningResource.SceneData.VertexBufferPool.TryAddBuffer(vertexBuffer);
                }
            }

            // Read FrameNameTable data
            parent.IsOnFrameTable = stream.ReadBoolean();
            parent.FrameNameTableFlags = (FrameNameTable.NameTableFlags)stream.ReadUInt32(false);

            // Read ParentIndex from previous SDS
            int OldParentIndex1RefId = stream.ReadInt32(false);
            int OldParentIndex2RefId = stream.ReadInt32(false);

            // Temporarily store it as a reference.
            parent.AddRef(FrameEntryRefTypes.Parent1, OldParentIndex1RefId);
            parent.AddRef(FrameEntryRefTypes.Parent2, OldParentIndex2RefId);

            // We can finally add our new frame object
            FrameObjects.Add(parent.RefID, parent);

            // Push new FrameObject int OldRefLookupTable
            OldRefIDLookupTable.Add(OldRefID, parent);

            // Read how many children this frame has, and proceed to read them too.
            int count = stream.ReadInt32(false);
            for (int i = 0; i < count; i++)
            {
                FrameObjectBase child = ReadFrame(stream);
            }

            return parent;
        }

        public void WriteToFile(string ExportName, FrameObjectBase Frame)
        {
            using (MemoryStream mainMemoryStream = new MemoryStream())
            {
                WriteToStream(Frame, mainMemoryStream);
                File.WriteAllBytes(ExportName, mainMemoryStream.ToArray());
            }
        }
        
        public void WriteToStream(FrameObjectBase Frame,Stream MainStream)
        {
            ModelAttachments = new Dictionary<ulong, List<int>>();
            
                using (BinaryWriter writer = new BinaryWriter(MainStream, Encoding.UTF8,leaveOpen:true))
                {
                    MemoryStream frameStream = SaveFrameStream(Frame, null);
                    writer.Write(frameStream.ToArray());

                    writer.Write(ModelAttachments.Count);
                    foreach(var Entry in ModelAttachments)
                    {
                        writer.Write(Entry.Key);
                        writer.Write(Entry.Value.Count);
                        foreach(var ListEntry in Entry.Value)
                        {
                            writer.Write(ListEntry);
                        }
                    }
                }

                MainStream.Position = 0;
        }
        
        private MemoryStream SaveFrameStream(FrameObjectBase frame,MemoryStream memoryStream)
        {
            if (memoryStream == null)
            {
             memoryStream = new MemoryStream();   
            }
            
            using (BinaryWriter writer = new BinaryWriter(memoryStream, Encoding.UTF8,leaveOpen:true))
            {
                writer.Write(frame.RefID); // Save old RefID
                Debug.WriteLine(frame.ToString());
                if (frame.GetType() == typeof(FrameObjectArea))
                {
                    writer.Write((ushort)FrameResourceObjectType.Area);
                    (frame as FrameObjectArea).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectCamera))
                {
                    writer.Write((ushort)FrameResourceObjectType.Camera);
                    (frame as FrameObjectCamera).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectCollision))
                {
                    writer.Write((ushort)FrameResourceObjectType.Collision);
                    (frame as FrameObjectCollision).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectComponent_U005))
                {
                    writer.Write((ushort)FrameResourceObjectType.Component_U00000005);
                    (frame as FrameObjectComponent_U005).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectDummy))
                {
                    writer.Write((ushort)FrameResourceObjectType.Dummy);
                    (frame as FrameObjectDummy).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectDeflector))
                {
                    writer.Write((ushort)FrameResourceObjectType.ParticleDeflector);
                    (frame as FrameObjectDeflector).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectFrame))
                {
                    writer.Write((ushort)FrameResourceObjectType.Frame);
                    (frame as FrameObjectFrame).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectJoint))
                {
                    writer.Write((ushort)FrameResourceObjectType.Point);
                    (frame as FrameObjectJoint).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectLight))
                {
                    writer.Write((ushort)FrameResourceObjectType.Light);
                    (frame as FrameObjectLight).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectModel))
                {
                    var mesh = (frame as FrameObjectModel);
                    writer.Write((ushort)FrameResourceObjectType.Model);
                    mesh.WriteToFilePart1(writer);
                    mesh.Geometry.WriteToFile(writer);
                    mesh.Material.WriteToFile(writer);
                    mesh.BlendInfo.WriteToFile(writer);
                    mesh.Skeleton.WriteToFile(writer);
                    mesh.SkeletonHierarchy.WriteToFile(writer);
                    mesh.WriteToFilePart2(writer);

                    // Write Attachment hashes to the dictionary
                    List<int> AttachmentHashes = new List<int>();
                    foreach (FrameObjectModel.AttachmentReference Attachment in mesh.AttachmentReferences)
                    {
                        AttachmentHashes.Add(Attachment.Attachment.RefID);
                    }

                    ModelAttachments.Add(mesh.Name.Hash, AttachmentHashes);

                    foreach (var lod in mesh.Geometry.LOD)
                    {
                        using (var stream = new MemoryStream())
                        {
                            OwningResource.SceneData.IndexBufferPool.GetBuffer(lod.IndexBufferRef.Hash).WriteToFile(stream, false);
                            OwningResource.SceneData.VertexBufferPool.GetBuffer(lod.VertexBufferRef.Hash).WriteToFile(stream, false);
                            writer.Write(stream.ToArray());
                        }
                    }
                }
                else if (frame.GetType() == typeof(FrameObjectSector))
                {
                    writer.Write((ushort)FrameResourceObjectType.Sector);
                    (frame as FrameObjectSector).WriteToFile(writer);
                }
                else if (frame.GetType() == typeof(FrameObjectSingleMesh))
                {
                    var mesh = (frame as FrameObjectSingleMesh);
                    writer.Write((ushort)FrameResourceObjectType.SingleMesh);
                    mesh.WriteToFile(writer);
                    mesh.Geometry.WriteToFile(writer);
                    mesh.Material.WriteToFile(writer);

                    foreach (var lod in mesh.Geometry.LOD)
                    {
                        using (var stream = new MemoryStream())
                        {
                            OwningResource.SceneData.IndexBufferPool.GetBuffer(lod.IndexBufferRef.Hash).WriteToFile(stream, false);
                            OwningResource.SceneData.VertexBufferPool.GetBuffer(lod.VertexBufferRef.Hash).WriteToFile(stream, false);
                            writer.Write(stream.ToArray());
                        }
                    }
                }
                else if (frame.GetType() == typeof(FrameObjectTarget))
                {
                    writer.Write((ushort)FrameResourceObjectType.Target);
                    (frame as FrameObjectTarget).WriteToFile(writer);
                }
                else
                {
                    writer.Write(frame.Type);
                    frame.WriteToFile(writer);
                }

                // write FrameNameTable info
                writer.Write(frame.IsOnFrameTable);
                writer.Write((uint)frame.FrameNameTableFlags);

                // Write ParentIndex1 and ParentIndex2 info
                writer.Write(frame.ParentIndex1.RefID);
                writer.Write(frame.ParentIndex2.RefID);

                writer.Write(frame.Children.Count);
                for (int i = 0; i < frame.Children.Count; i++)
                {
                    SaveFrameStream(frame.Children[i], memoryStream);
                }
                
            }

            return memoryStream;
        }

        public void ReadFramesFromFile(string name)
        {
            byte[] packData = File.ReadAllBytes(name); // Loading from file
            ReadFramesFromFile(packData);
        }

        public void ReadFramesFromFile(Stream frImportData)
        {
            byte[] packData = new byte[frImportData.Length]; // Loading from stream
            frImportData.Read(packData, 0, packData.Length);
            ReadFramesFromFile(packData);
        }

        private void ReadFramesFromFile(byte[] PackData)
        {
            FrameObjects = new Dictionary<int, object>();
            FrameMaterials = new Dictionary<int, FrameMaterial>();
            FrameGeometries = new Dictionary<int, FrameGeometry>();
            FrameSkeletonHierarchy = new Dictionary<int, FrameSkeletonHierachy>();
            FrameBlendInfos = new Dictionary<int, FrameBlendInfo>();
            FrameSkeletons = new Dictionary<int, FrameSkeleton>();
            ModelAttachments = new Dictionary<ulong, List<int>>();
            OldRefIDLookupTable = new Dictionary<int, FrameObjectBase>();
            
            using (MemoryStream stream = new MemoryStream(PackData))
            {
                RootFrame = ReadFrame(stream);

                uint NumModelAttachments = stream.ReadUInt32(false);
                for(int i = 0; i < NumModelAttachments; i++)
                {
                    List<int> AttachmentRefs = new List<int>();

                    ulong ModelHash = stream.ReadUInt64(false);
                    uint NumAttachmentsInModel = stream.ReadUInt32(false);
                    for(int z = 0; z < NumAttachmentsInModel; z++)
                    {
                        int AttachmentRefID = stream.ReadInt32(false);
                        AttachmentRefs.Add(AttachmentRefID);
                    }

                    ModelAttachments.Add(ModelHash, AttachmentRefs);
                }
            }
        }

        public void PushPacketIntoFrameResource()
        {
            Dictionary<int, FrameObjectBase> AttachmentRefLookup = new Dictionary<int, FrameObjectBase>();

            OwningResource.FrameBlendInfos.AddRange(FrameBlendInfos);
            OwningResource.FrameGeometries.AddRange(FrameGeometries);
            OwningResource.FrameMaterials.AddRange(FrameMaterials);
            OwningResource.FrameSkeletonHierachies.AddRange(FrameSkeletonHierarchy);
            OwningResource.FrameSkeletons.AddRange(FrameSkeletons);
            OwningResource.FrameObjects.AddRange(FrameObjects);

            // Update child relations
            // Then push Object and hash to AttachmentRefLookup
            foreach (var Pair in FrameObjects)
            {
                FrameObjectBase CurrentObject = (Pair.Value as FrameObjectBase);
                UpdateParentChildRelations(OwningResource, CurrentObject);
            }

            // Update AttachmentReferences on FrameObjectModels    
            foreach(var Pair in FrameObjects)
            {
                if(Pair.Value is FrameObjectModel)
                {
                    FrameObjectModel ModelObject = (Pair.Value as FrameObjectModel);
                    List<int> AttachmentRefs = ModelAttachments[ModelObject.Name.Hash];

                    for(int i = 0; i < AttachmentRefs.Count; i++)
                    {
                        ModelObject.AttachmentReferences[i].Attachment = OldRefIDLookupTable[AttachmentRefs[i]];
                    }
                }
            }
        }

        private void UpdateParentChildRelations(FrameResource FrameResource, FrameObjectBase ObjectToUpdate)
        {
            FrameObjectBase Child = ObjectToUpdate;

            // Parent Frames
            FrameObjectBase Parent1 = null;
            FrameObjectBase Parent2 = null;

            if (Child.Refs.ContainsKey(FrameEntryRefTypes.Parent1))
            {
                if (OldRefIDLookupTable.TryGetValue(Child.Refs[FrameEntryRefTypes.Parent1], out Parent1))
                {
                    Child.SubRef(FrameEntryRefTypes.Parent1);
                    FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex1, Child, Parent1);
                }
                else
                {
                    Child.SubRef(FrameEntryRefTypes.Parent1);
                    FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex1, Child, null);
                }
            }

            if (Child.Refs.ContainsKey(FrameEntryRefTypes.Parent2))
            {
                if (OldRefIDLookupTable.TryGetValue(Child.Refs[FrameEntryRefTypes.Parent2], out Parent2))
                {
                    Child.SubRef(FrameEntryRefTypes.Parent2);
                    FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex2, Child, Parent2);
                }
                else
                {
                    Child.SubRef(FrameEntryRefTypes.Parent2);
                    FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex2, Child, null);
                }
            }
        }
    }
}
