using Mafia2Tool;
using ResourceTypes.BufferPools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Utils.Extensions;
using Utils.Settings;

namespace ResourceTypes.FrameResource
{
    public class FramePack
    {
        private Dictionary<ulong, List<ulong>> ModelAttachments;

        // Used when saving
        public FrameObjectBase RootFrame { get; private set; }
        private Dictionary<int, object> FrameObjects;
        private Dictionary<int, FrameBlendInfo> FrameBlendInfos;
        private Dictionary<int, FrameSkeleton> FrameSkeletons;
        private Dictionary<int, FrameSkeletonHierachy> FrameSkeletonHierarchy;
        private Dictionary<int, FrameMaterial> FrameMaterials;
        private Dictionary<int, FrameGeometry> FrameGeometries;

        private void SaveFrame(FrameObjectBase frame, BinaryWriter writer)
        {
            //is this even needed? hmm.
            Debug.WriteLine(frame.ToString());
            if (frame.GetType() == typeof(FrameObjectArea))
            {
                writer.Write((ushort)ObjectType.Area);
                (frame as FrameObjectArea).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectCamera))
            {
                writer.Write((ushort)ObjectType.Camera);
                (frame as FrameObjectCamera).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectCollision))
            {
                writer.Write((ushort)ObjectType.Collision);
                (frame as FrameObjectCollision).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectComponent_U005))
            {
                writer.Write((ushort)ObjectType.Component_U00000005);
                (frame as FrameObjectComponent_U005).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectDummy))
            {
                writer.Write((ushort)ObjectType.Dummy);
                (frame as FrameObjectDummy).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectDeflector))
            {
                writer.Write((ushort)ObjectType.ParticleDeflector);
                (frame as FrameObjectDeflector).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectFrame))
            {
                writer.Write((ushort)ObjectType.Frame);
                (frame as FrameObjectFrame).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectJoint))
            {
                writer.Write((ushort)ObjectType.Joint);
                (frame as FrameObjectJoint).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectLight))
            {
                writer.Write((ushort)ObjectType.Light);
                (frame as FrameObjectLight).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectModel))
            {
                var mesh = (frame as FrameObjectModel);
                writer.Write((ushort)ObjectType.Model);
                mesh.WriteToFilePart1(writer);
                mesh.Geometry.WriteToFile(writer);
                mesh.Material.WriteToFile(writer);
                mesh.BlendInfo.WriteToFile(writer);
                mesh.Skeleton.WriteToFile(writer);
                mesh.SkeletonHierarchy.WriteToFile(writer);
                mesh.WriteToFilePart2(writer);

                // Write Attachment hashes to the dictionary
                List<ulong> AttachmentHashes = new List<ulong>();
                foreach(FrameObjectModel.AttachmentReference Attachment in mesh.AttachmentReferences)
                {
                    AttachmentHashes.Add(Attachment.Attachment.Name.Hash);
                }

                ModelAttachments.Add(mesh.Name.Hash, AttachmentHashes);

                foreach (var lod in mesh.Geometry.LOD)
                {
                    using (var stream = new MemoryStream())
                    {
                        SceneData.IndexBufferPool.GetBuffer(lod.IndexBufferRef.Hash).WriteToFile(stream, false);
                        SceneData.VertexBufferPool.GetBuffer(lod.VertexBufferRef.Hash).WriteToFile(stream, false);
                        writer.Write(stream.ToArray());
                    }
                }
            }
            else if (frame.GetType() == typeof(FrameObjectSector))
            {
                writer.Write((ushort)ObjectType.Sector);
                (frame as FrameObjectSector).WriteToFile(writer);
            }
            else if (frame.GetType() == typeof(FrameObjectSingleMesh))
            {
                var mesh = (frame as FrameObjectSingleMesh);
                writer.Write((ushort)ObjectType.SingleMesh);
                mesh.WriteToFile(writer);
                mesh.Geometry.WriteToFile(writer);
                mesh.Material.WriteToFile(writer);

                foreach (var lod in mesh.Geometry.LOD)
                {
                    using (var stream = new MemoryStream())
                    {
                        SceneData.IndexBufferPool.GetBuffer(lod.IndexBufferRef.Hash).WriteToFile(stream, false);
                        SceneData.VertexBufferPool.GetBuffer(lod.VertexBufferRef.Hash).WriteToFile(stream, false);
                        writer.Write(stream.ToArray());
                    }
                }
            }
            else if (frame.GetType() == typeof(FrameObjectTarget))
            {
                writer.Write((ushort)ObjectType.Target);
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

            writer.Write(frame.Children.Count);
            for (int i = 0; i < frame.Children.Count; i++)
            {
                SaveFrame(frame.Children[i], writer);
            }
        }

        private FrameObjectBase ReadFrame(MemoryStream stream)
        {
            ObjectType frameType = (ObjectType)stream.ReadInt16(false);
            FrameObjectBase parent = FrameFactory.ReadFrameByObjectID(stream, frameType, false);
            Debug.WriteLine(parent.ToString());

            if (parent is FrameObjectSingleMesh || parent is FrameObjectModel)
            {
                // Read the required blocks;
                FrameGeometry geometry = new FrameGeometry();
                geometry.ReadFromFile(stream, false);
                FrameMaterial material = new FrameMaterial();
                material.ReadFromFile(stream, false);

                // Add them into our pool of blocks
                FrameGeometries.Add(geometry.RefID, geometry);
                FrameMaterials.Add(material.RefID, material);

                // Add our references onto our mesh
                FrameObjectSingleMesh mesh = (parent as FrameObjectSingleMesh);
                mesh.AddRef(FrameEntryRefTypes.Geometry, geometry.RefID);
                mesh.Geometry = FrameGeometries[geometry.RefID];
                mesh.AddRef(FrameEntryRefTypes.Material, material.RefID);
                mesh.Material = FrameMaterials[material.RefID];

                if (parent is FrameObjectModel)
                {
                    // Read the rigged specific blocks
                    FrameBlendInfo blendInfo = new FrameBlendInfo();
                    blendInfo.ReadFromFile(stream, false);
                    FrameSkeleton skeleton = new FrameSkeleton();
                    skeleton.ReadFromFile(stream, false);
                    FrameSkeletonHierachy hierarchy = new FrameSkeletonHierachy();
                    hierarchy.ReadFromFile(stream, false);

                    // Add our new rigged specific blocks into our pools
                    FrameBlendInfos.Add(blendInfo.RefID, blendInfo);
                    FrameSkeletons.Add(skeleton.RefID, skeleton);
                    FrameSkeletonHierarchy.Add(hierarchy.RefID, hierarchy);

                    // Finally, add our references to the model.
                    FrameObjectModel model = (parent as FrameObjectModel);
                    model.AddRef(FrameEntryRefTypes.BlendInfo, blendInfo.RefID);
                    model.BlendInfo = FrameBlendInfos[blendInfo.RefID];
                    model.AddRef(FrameEntryRefTypes.Skeleton, skeleton.RefID);
                    model.Skeleton = FrameSkeletons[skeleton.RefID];
                    model.AddRef(FrameEntryRefTypes.SkeletonHierachy, hierarchy.RefID);
                    model.SkeletonHierarchy = FrameSkeletonHierarchy[hierarchy.RefID];
                    model.ReadFromFilePart2(stream, false);
                }

                // Read the buffers;
                IndexBuffer indexBuffer = new IndexBuffer(stream, false);
                VertexBuffer vertexBuffer = new VertexBuffer(stream, false);

                // We have to make sure we have index and buffer pools available
                // We have to do it for all LODs too; if any more than 1.
                foreach (var lod in geometry.LOD)
                {
                    SceneData.IndexBufferPool.TryAddBuffer(indexBuffer);
                    SceneData.VertexBufferPool.TryAddBuffer(vertexBuffer);
                }
            }

            // Read FrameNameTable data
            parent.IsOnFrameTable = stream.ReadBoolean();
            parent.FrameNameTableFlags = (FrameNameTable.NameTableFlags)stream.ReadUInt32(false);

            // We can finally add our new frame object
            FrameObjects.Add(parent.RefID, parent);

            // Read how many children this frame has, and proceed to read them too.
            int count = stream.ReadInt32(false);
            for (int i = 0; i < count; i++)
            {
                FrameObjectBase child = ReadFrame(stream);
                child.Parent = parent;
                parent.Children.Add(child);

                // Add References early..
                child.AddRef(FrameEntryRefTypes.Parent1, parent.RefID);
                child.AddRef(FrameEntryRefTypes.Parent2, parent.RefID);
                
                //SetParentOfObject(0, child, parent);
                //SetParentOfObject(1, child, parent);
            }

            return parent;
        }

        public void WriteToFile(FrameObjectBase Frame)
        {
            ModelAttachments = new Dictionary<ulong, List<ulong>>();

            string FrameName = Frame.Name.String;
            string ExportName = Path.Combine(ToolkitSettings.ExportPath, FrameName) + ".framedata";
            using (BinaryWriter writer = new BinaryWriter(File.Open(ExportName, FileMode.Create)))
            {
                SaveFrame(Frame, writer);

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
        }

        public void ReadFramesFromFile(string FileName)
        {
            FrameObjects = new Dictionary<int, object>();
            FrameMaterials = new Dictionary<int, FrameMaterial>();
            FrameGeometries = new Dictionary<int, FrameGeometry>();
            FrameSkeletonHierarchy = new Dictionary<int, FrameSkeletonHierachy>();
            FrameBlendInfos = new Dictionary<int, FrameBlendInfo>();
            FrameSkeletons = new Dictionary<int, FrameSkeleton>();
            ModelAttachments = new Dictionary<ulong, List<ulong>>();

            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(FileName)))
            {
                RootFrame = ReadFrame(stream);

                uint NumModelAttachments = stream.ReadUInt32(false);
                for(int i = 0; i < NumModelAttachments; i++)
                {
                    List<ulong> AttachmentRefs = new List<ulong>();

                    ulong ModelHash = stream.ReadUInt64(false);
                    uint NumAttachmentsInModel = stream.ReadUInt32(false);
                    for(int z = 0; z < NumAttachmentsInModel; z++)
                    {
                        ulong AttachmentHash = stream.ReadUInt64(false);
                        AttachmentRefs.Add(AttachmentHash);
                    }

                    ModelAttachments.Add(ModelHash, AttachmentRefs);
                }
            }
        }

        public void PushPacketIntoFrameResource(FrameResource FrameResource)
        {
            Dictionary<ulong, FrameObjectBase> AttachmentRefLookup = new Dictionary<ulong, FrameObjectBase>();

            FrameResource.FrameBlendInfos.AddRange(FrameBlendInfos);
            FrameResource.FrameGeometries.AddRange(FrameGeometries);
            FrameResource.FrameMaterials.AddRange(FrameMaterials);
            FrameResource.FrameSkeletonHierachies.AddRange(FrameSkeletonHierarchy);
            FrameResource.FrameSkeletons.AddRange(FrameSkeletons);
            FrameResource.FrameObjects.AddRange(FrameObjects);

            // Update child relations
            // Then push Object and hash to AttachmentRefLookup
            foreach (var Pair in FrameObjects)
            {
                FrameObjectBase CurrentObject = (Pair.Value as FrameObjectBase);
                UpdateParentChildRelations(FrameResource, CurrentObject);
                AttachmentRefLookup.Add(CurrentObject.Name.Hash, CurrentObject);
            }

            // Update AttachmentReferences on FrameObjectModels    
            foreach(var Pair in FrameObjects)
            {
                if(Pair.Value is FrameObjectModel)
                {
                    FrameObjectModel ModelObject = (Pair.Value as FrameObjectModel);
                    List<ulong> AttachmentRefs = ModelAttachments[ModelObject.Name.Hash];

                    for(int i = 0; i < AttachmentRefs.Count; i++)
                    {
                        ModelObject.AttachmentReferences[i].Attachment = AttachmentRefLookup[AttachmentRefs[i]];
                    }
                }
            }
        }

        private void UpdateParentChildRelations(FrameResource FrameResource, FrameObjectBase ObjectToUpdate)
        {
            for(int i = 0; i < ObjectToUpdate.Children.Count; i++)
            {
                FrameObjectBase Child = ObjectToUpdate.Children[i];
                FrameResource.SetParentOfObject(0, Child, ObjectToUpdate);
                FrameResource.SetParentOfObject(1, Child, ObjectToUpdate);

                UpdateParentChildRelations(FrameResource, Child);
            }
        }
    }
}
