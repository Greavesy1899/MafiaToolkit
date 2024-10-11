using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using Mafia2Tool;
using Utils.Extensions;
using ResourceTypes.Misc;
using Utils.Logging;

namespace ResourceTypes.FrameResource
{
    public class FrameResource
    {
        private SceneData SceneData = new SceneData();
        FrameHeader header;
        Dictionary<int, FrameHeaderScene> frameScenes = new Dictionary<int, FrameHeaderScene>();
        Dictionary<int, FrameGeometry> frameGeometries = new Dictionary<int, FrameGeometry>();
        Dictionary<int, FrameMaterial> frameMaterials = new Dictionary<int, FrameMaterial>();
        Dictionary<int, FrameBlendInfo> frameBlendInfos = new Dictionary<int, FrameBlendInfo>();
        Dictionary<int, FrameSkeleton> frameSkeletons = new Dictionary<int, FrameSkeleton>();
        Dictionary<int, FrameSkeletonHierachy> frameSkeletonHierachies = new Dictionary<int, FrameSkeletonHierachy>();
        Dictionary<int, object> frameObjects = new Dictionary<int, object>();

        private FrameProps FramePropContents;

        public FrameHeader Header {
            get { return header; }
            set { header = value; }
        }
        public Dictionary<int, FrameHeaderScene> FrameScenes {
            get { return frameScenes; }
            set { frameScenes = value; }
        }
        public Dictionary<int, FrameGeometry> FrameGeometries {
            get { return frameGeometries; }
            set { frameGeometries = value; }
        }
        public Dictionary<int, FrameMaterial> FrameMaterials {
            get { return frameMaterials; }
            set { frameMaterials = value; }
        }
        public Dictionary<int, FrameBlendInfo> FrameBlendInfos {
            get { return frameBlendInfos; }
            set { frameBlendInfos = value; }
        }
        public Dictionary<int, FrameSkeleton> FrameSkeletons {
            get { return frameSkeletons; }
            set { frameSkeletons = value; }
        }
        public Dictionary<int, FrameSkeletonHierachy> FrameSkeletonHierachies {
            get { return frameSkeletonHierachies; }
            set { frameSkeletonHierachies = value; }
        }
        public Dictionary<int, object> FrameObjects {
            get { return frameObjects; }
            set { frameObjects = value; }
        }

        public int GetBlockCount {
            get { return frameBlendInfos.Count + frameGeometries.Count + frameMaterials.Count + frameSkeletons.Count + frameSkeletonHierachies.Count + frameScenes.Count; }
        }

        public int GetIndexOfObject(int refID)
        {
            for (int i = 0; i != frameObjects.Count; i++)
            {
                if (frameObjects.ElementAt(i).Key == refID)
                    return i + (GetBlockCount);
            }
            return -1;
        }

        public FrameObjectBase GetObjectFromIndex(int index)
        {
            return (frameObjects.ElementAt(index).Value as FrameObjectBase);
        }

        public FrameResource()
        {
            header = new FrameHeader(this);
            frameScenes = new Dictionary<int, FrameHeaderScene>();
            frameGeometries = new Dictionary<int, FrameGeometry>();
            frameMaterials = new Dictionary<int, FrameMaterial>();
            frameBlendInfos = new Dictionary<int, FrameBlendInfo>();
            frameSkeletons = new Dictionary<int, FrameSkeleton>();
            frameSkeletonHierachies = new Dictionary<int, FrameSkeletonHierachy>();
            frameObjects = new Dictionary<int, object>();
        }

        public FrameResource(string file, bool isBigEndian = false) : this()
        {
            if (File.Exists("FrameProps.bin"))
            {
                FramePropContents = new FrameProps(new FileInfo("FrameProps.bin"));
            }

            using (MemoryStream reader = new MemoryStream(File.ReadAllBytes(file), false))
            {
                ReadFromFile(reader, isBigEndian);
            }
        }

        public FrameHeaderScene AddSceneFolder(string name)
        {
            FrameHeaderScene scene = ConstructFrameAssetOfType<FrameHeaderScene>();
            scene.Name = new Utils.Types.HashName(name);
            return scene;
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            header = new FrameHeader(this);
            header.ReadFromFile(reader, isBigEndian);
            List<int> refs = new List<int>();

            for (int i = 0; i < header.SceneFolders.Count; i++)
            {
                refs.Add(header.SceneFolders[i].RefID);
            }
            for (int i = 0; i < header.NumGeometries; i++)
            {
                FrameGeometry geo = ConstructFrameAssetOfType<FrameGeometry>();
                geo.ReadFromFile(reader, isBigEndian);
                refs.Add(geo.RefID);
            }
            for (int i = 0; i < header.NumMaterialResources; i++)
            {
                FrameMaterial mat = ConstructFrameAssetOfType<FrameMaterial>();
                mat.ReadFromFile(reader, isBigEndian);
                refs.Add(mat.RefID);
            }
            for (int i = 0; i < header.NumBlendInfos; i++)
            {
                FrameBlendInfo blendInfo = ConstructFrameAssetOfType<FrameBlendInfo>();
                blendInfo.ReadFromFile(reader, isBigEndian);
                refs.Add(blendInfo.RefID);
            }
            for (int i = 0; i < header.NumSkeletons; i++)
            {
                FrameSkeleton skeleton = ConstructFrameAssetOfType<FrameSkeleton>();
                skeleton.ReadFromFile(reader, isBigEndian);
                refs.Add(skeleton.RefID);
            }
            for (int i = 0; i < header.NumSkelHierachies; i++)
            {
                FrameSkeletonHierachy skeletonHierachy = ConstructFrameAssetOfType<FrameSkeletonHierachy>();
                skeletonHierachy.ReadFromFile(reader, isBigEndian);
                refs.Add(skeletonHierachy.RefID);
            }

            FrameResourceObjectType[] objectTypes = new FrameResourceObjectType[header.NumObjects];

            if (header.NumObjects > 0)
            {
                for (int i = 0; i < header.NumObjects; i++)
                {
                    objectTypes[i] = (FrameResourceObjectType)reader.ReadInt32(isBigEndian);
                }

                for (int i = 0; i < header.NumObjects; i++)
                {
                    FrameObjectBase newObject = FrameFactory.ReadFrameByObjectID(reader, isBigEndian, this, objectTypes[i]);

                    newObject.Index = i;

                    if (objectTypes[i] == FrameResourceObjectType.SingleMesh)
                    {
                        FrameObjectSingleMesh mesh = newObject as FrameObjectSingleMesh;

                        if (mesh.MeshIndex != -1)
                        {
                            mesh.AddRef(FrameEntryRefTypes.Geometry, refs[mesh.MeshIndex]);
                            mesh.Geometry = frameGeometries[mesh.Refs[FrameEntryRefTypes.Geometry]];
                        }

                        if (mesh.MaterialIndex != -1)
                        {
                            mesh.AddRef(FrameEntryRefTypes.Material, refs[mesh.MaterialIndex]);
                            mesh.Material = frameMaterials[mesh.Refs[FrameEntryRefTypes.Material]];
                        }
                    }
                    else if (objectTypes[i] == FrameResourceObjectType.Model)
                    {
                        FrameObjectModel mesh = newObject as FrameObjectModel;
                        mesh.AddRef(FrameEntryRefTypes.Geometry, refs[mesh.MeshIndex]);
                        mesh.Geometry = frameGeometries[mesh.Refs[FrameEntryRefTypes.Geometry]];
                        mesh.AddRef(FrameEntryRefTypes.Material, refs[mesh.MaterialIndex]);
                        mesh.Material = frameMaterials[mesh.Refs[FrameEntryRefTypes.Material]];
                        mesh.AddRef(FrameEntryRefTypes.BlendInfo, refs[mesh.BlendInfoIndex]);
                        mesh.BlendInfo = frameBlendInfos[mesh.Refs[FrameEntryRefTypes.BlendInfo]];
                        mesh.AddRef(FrameEntryRefTypes.Skeleton, refs[mesh.SkeletonIndex]);
                        mesh.Skeleton = frameSkeletons[mesh.Refs[FrameEntryRefTypes.Skeleton]];
                        mesh.AddRef(FrameEntryRefTypes.SkeletonHierachy, refs[mesh.SkeletonHierachyIndex]);
                        mesh.SkeletonHierarchy = frameSkeletonHierachies[mesh.Refs[FrameEntryRefTypes.SkeletonHierachy]];

                        mesh.ReadFromFilePart2(reader, isBigEndian);

                        newObject = mesh;
                    }
                }
            }
            objectTypes = null;
            DefineFrameBlockParents();
        }

        public void WriteToFile(string name)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(name, FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            //BEFORE WE WRITE, WE NEED TO COMPILE AND UPDATE THE FRAME.
            UpdateFrameData();
            header.WriteToFile(writer);

            foreach (var Geometry in frameGeometries.Values)
            {
                Geometry.WriteToFile(writer);
            }

            foreach (var Material in frameMaterials.Values)
            {
                Material.WriteToFile(writer);
            }

            foreach (var BlendInfo in frameBlendInfos.Values)
            {
                BlendInfo.WriteToFile(writer);
            }

            foreach (var Skeleton in frameSkeletons.Values)
            {
                Skeleton.WriteToFile(writer);
            }

            foreach (var SkeletonHierarchy in frameSkeletonHierachies.Values)
            {
                SkeletonHierarchy.WriteToFile(writer);
            }

            foreach (var FObject in frameObjects.Values)
            {
                FrameObjectBase entry = (FObject as FrameObjectBase);
                if (entry.GetType() == typeof(FrameObjectJoint))
                    writer.Write((int)FrameResourceObjectType.Joint);
                else if (entry.GetType() == typeof(FrameObjectSingleMesh))
                    writer.Write((int)FrameResourceObjectType.SingleMesh);
                else if (entry.GetType() == typeof(FrameObjectFrame))
                    writer.Write((int)FrameResourceObjectType.Frame);
                else if (entry.GetType() == typeof(FrameObjectLight))
                    writer.Write((int)FrameResourceObjectType.Light);
                else if (entry.GetType() == typeof(FrameObjectCamera))
                    writer.Write((int)FrameResourceObjectType.Camera);
                else if (entry.GetType() == typeof(FrameObjectComponent_U005))
                    writer.Write((int)FrameResourceObjectType.Component_U00000005);
                else if (entry.GetType() == typeof(FrameObjectSector))
                    writer.Write((int)FrameResourceObjectType.Sector);
                else if (entry.GetType() == typeof(FrameObjectDummy))
                    writer.Write((int)FrameResourceObjectType.Dummy);
                else if (entry.GetType() == typeof(FrameObjectDeflector))
                    writer.Write((int)FrameResourceObjectType.ParticleDeflector);
                else if (entry.GetType() == typeof(FrameObjectArea))
                    writer.Write((int)FrameResourceObjectType.Area);
                else if (entry.GetType() == typeof(FrameObjectTarget))
                    writer.Write((int)FrameResourceObjectType.Target);
                else if (entry.GetType() == typeof(FrameObjectModel))
                    writer.Write((int)FrameResourceObjectType.Model);
                else if (entry.GetType() == typeof(FrameObjectCollision))
                    writer.Write((int)FrameResourceObjectType.Collision);
            }

            foreach (var FObject in frameObjects.Values)
            {
                FrameObjectBase entry = (FObject as FrameObjectBase);
                entry.WriteToFile(writer);
            }
        }

        public void DuplicateBlocks(FrameObjectSingleMesh mesh)
        {
            FrameMaterial material = new FrameMaterial(mesh.Material);
            mesh.ReplaceRef(FrameEntryRefTypes.Material, material.RefID);
            mesh.Material = material;
            frameMaterials.Add(material.RefID, material);
        }

        public void DuplicateBlocks(FrameObjectModel model)
        {
            DuplicateBlocks((FrameObjectSingleMesh)model);
        }

        public bool DeleteFrame(FrameEntry EntryToDelete)
        {
            // Early return out if its invalid
            if(EntryToDelete == null)
            {
                return false;
            }

            // Remove Parent reference
            FrameObjectBase BaseObject = (EntryToDelete as FrameObjectBase);
            if(BaseObject != null)
            {
                FrameObjectBase ParentObject = BaseObject.Parent;
                if(ParentObject != null)
                {
                    bool bDeleted = ParentObject.Children.Remove(BaseObject);
                    ToolkitAssert.Ensure(bDeleted, "Failed to delete an object which should be in the child array.");

                    BaseObject.Parent = null;
                }
            }

            return FrameObjects.Remove(EntryToDelete.RefID);
        }

        public T ConstructFrameAssetOfType<T>() where T : FrameEntry
        {
            T NewFrame = (T)Activator.CreateInstance(typeof(T), this);

            if (NewFrame is FrameObjectBase)
            {
                FrameObjects.Add(NewFrame.RefID, NewFrame);
            }

            if (NewFrame is FrameMaterial)
            {
                frameMaterials.Add(NewFrame.RefID, NewFrame as FrameMaterial);
            }

            if (NewFrame is FrameGeometry)
            {
                frameGeometries.Add(NewFrame.RefID, NewFrame as FrameGeometry);
            }

            if (NewFrame is FrameBlendInfo)
            {
                frameBlendInfos.Add(NewFrame.RefID, NewFrame as FrameBlendInfo);
            }

            if (NewFrame is FrameSkeletonHierachy)
            {
                frameSkeletonHierachies.Add(NewFrame.RefID, NewFrame as FrameSkeletonHierachy);
            }

            if (NewFrame is FrameSkeleton)
            {
                frameSkeletons.Add(NewFrame.RefID, NewFrame as FrameSkeleton);
            }

            if(NewFrame is FrameHeaderScene)
            {
                frameScenes.Add(NewFrame.RefID, NewFrame as FrameHeaderScene);
                header.SceneFolders.Add(NewFrame as FrameHeaderScene);
            }

            return NewFrame;
        }

        public void SetParentOfObject(ParentInfo.ParentType ParentType, FrameEntry childEntry, FrameEntry parentEntry)
        {
            //get the index and child object
            FrameObjectBase obj = (childEntry as FrameObjectBase);
            obj.SetParent(ParentType, parentEntry);

            // Update world transform
            foreach (var pair in frameObjects)
            {
                (pair.Value as FrameObjectBase).SetWorldTransform();
            }
        }

        public TreeNode ReadFramesFromFile(string filename,SceneData sceneData)
        {
            FramePack Packet = new FramePack(this);
            Packet.ReadFramesFromFile(filename,sceneData,null);
            Packet.PushPacketIntoFrameResource();
            return BuildFromFrames(null, Packet.RootFrame);
        }
        
        public TreeNode ReadFramesFromImport(MemoryStream fromFR,string name,SceneData OriginalScene)
        {
            FramePack Packet = new FramePack(OriginalScene.FrameResource);
            Packet.ReadFramesFromFile(name,OriginalScene,fromFR);
            Packet.PushPacketIntoFrameResource();
            return BuildFromFrames(null, Packet.RootFrame);
        }

        public void SaveFramesToFile(string FileName, FrameObjectBase frame,SceneData sceneData)
        {
            FramePack Packet = new FramePack(this);
            Packet.WriteToFile(FileName, frame,sceneData);
        }
        
        public MemoryStream SaveFramesStream(FrameObjectBase frame,SceneData ImportedScene)
        {
            FramePack Packet = new FramePack(ImportedScene.FrameResource);
            MemoryStream PacketStream = Packet.WriteToStream(frame, ImportedScene);
            return PacketStream;
        }

        private void AddChildren(Dictionary<int, TreeNode> parsedNodes, List<FrameObjectBase> children, TreeNode parentNode)
        {
            foreach (var child in children)
            {
                if (parsedNodes.ContainsKey(child.RefID))
                {
                    continue;
                }

                TreeNode node = new TreeNode(child.ToString());
                node.Tag = child;
                node.Name = child.RefID.ToString();
                parentNode.Nodes.Add(node);
                parsedNodes.Add(child.RefID, node);
                AddChildren(parsedNodes, child.Children, node);
            }
        }
        public TreeNode BuildTree(FrameNameTable.FrameNameTable table,SceneData sceneData)
        {
            SceneData = sceneData;
            TreeNode root = new TreeNode("FrameResource Contents: "+SceneData.ScenePath.Substring(SceneData.ScenePath.LastIndexOf('\\') + 1));
            root.Tag = header;
            
            int numBlocks = header.NumFolderNames + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies;
            Dictionary<int, TreeNode> parsedNodes = new Dictionary<int, TreeNode>();
            Dictionary<int, TreeNode> notAddedNodes = new Dictionary<int, TreeNode>();

            //Add scene groups into the scene viewer.
            for (int i = 0; i != frameScenes.Count; i++)
            {
                FrameHeaderScene scene = frameScenes.Values.ElementAt(i);
                TreeNode node = new TreeNode(scene.ToString());
                node.Tag = scene;
                node.Name = scene.RefID.ToString();
                parsedNodes.Add(scene.RefID, node);
                root.Nodes.Add(node);

                AddChildren(parsedNodes, scene.Children, node);
            }

            foreach (var pair in frameObjects)
            {
                FrameObjectBase frame = (pair.Value as FrameObjectBase);
                TreeNode node;

                if (parsedNodes.ContainsKey(pair.Key))
                {
                    continue;
                }

                if (frame.ParentIndex1.Index == -1 && frame.ParentIndex2.Index == -1)
                {
                    node = new TreeNode(frame.ToString());
                    node.Tag = frame;
                    node.Name = frame.RefID.ToString();
                    root.Nodes.Add(node);
                    parsedNodes.Add(frame.RefID, node);
                    AddChildren(parsedNodes, frame.Children, node);
                }
            }

            foreach (var pair in frameObjects)
            {
                if (!parsedNodes.ContainsKey(pair.Key))
                {
                    FrameObjectBase frame = (pair.Value as FrameObjectBase);
                    Debug.WriteLine("Failed " + frame.ToString());
                    TreeNode node = new TreeNode(frame.ToString());
                    node.Tag = frame;
                    node.Name = frame.RefID.ToString();
                    root.Nodes.Add(node);
                    //throw new FormatException("Unhandled frame! Name is: " + pair.Value.ToString());
                }
            }
            return root;
        }

        private TreeNode BuildFromFrames(TreeNode parent, FrameObjectBase frame)
        {
            // Create our new node for the frame.
            TreeNode node = new TreeNode(frame.ToString());
            node.Tag = frame;
            node.Name = frame.RefID.ToString();

            // If our parent exists, add it into the node.
            if (parent != null)
            {
                parent.Nodes.Add(node);
            }

            // Iterate and create the frames from our parent frame.
            foreach (var child in frame.Children)
            {
                BuildFromFrames(node, child);
            }

            return node;
        }

        public void DefineFrameBlockParents()
        {
            for (int i = 0; i < frameObjects.Count; i++)
            {
                var entry = (frameObjects.ElementAt(i));
                FrameObjectBase obj = (entry.Value as FrameObjectBase);

                if (obj == null)
                {
                    continue;
                }

                if(FramePropContents != null)
                {
                    obj.OurFrameProps = FramePropContents.GetFramePropsFor(obj.Name.Hash);
                }

                if (obj is FrameObjectModel)
                {
                    FrameObjectModel model = (obj as FrameObjectModel);

                    foreach (var attachment in model.AttachmentReferences)
                    {
                        attachment.Attachment = (frameObjects.ElementAt(attachment.AttachmentIndex - GetBlockCount).Value as FrameObjectBase);
                    }
                }

                if (obj.ParentIndex1.Index > -1)
                {
                    if (obj.ParentIndex1.Index <= (frameScenes.Count - 1) && (frameScenes.Count - 1) != -1)
                    {
                        FrameHeaderScene scene = (frameScenes.ElementAt(obj.ParentIndex1.Index).Value as FrameHeaderScene);
                        obj.ParentIndex1.RefID = scene.RefID;
                        obj.ParentIndex1.Name = scene.Name.ToString();
                        scene.Children.Add(obj);
                    }
                    else if (obj.ParentIndex1.Index >= GetBlockCount)
                    {
                        FrameObjectBase parent = GetObjectFromIndex(obj.ParentIndex1.Index - GetBlockCount);
                        obj.ParentIndex1.RefID = parent.RefID;
                        obj.ParentIndex1.Name = parent.Name.ToString();
                        obj.Parent = parent;
                        parent.Children.Add(obj);
                    }
                    else
                    {
                        throw new Exception("Unhandled Frame!");
                    }
                    obj.AddRef(FrameEntryRefTypes.Parent1, obj.ParentIndex1.RefID);
                }

                if (obj.ParentIndex2.Index > -1)
                {
                    if (obj.ParentIndex2.Index <= (frameScenes.Count - 1) && (frameScenes.Count - 1) != -1)
                    {
                        FrameHeaderScene scene = (frameScenes.ElementAt(obj.ParentIndex2.Index).Value as FrameHeaderScene);
                        obj.ParentIndex2.RefID = scene.RefID;
                        obj.ParentIndex2.Name = scene.Name.ToString();
                        if (obj.Parent == null) scene.Children.Add(obj);
                    }
                    else if (obj.ParentIndex2.Index >= GetBlockCount)
                    {
                        FrameObjectBase parent = GetObjectFromIndex(obj.ParentIndex2.Index - GetBlockCount);
                        obj.ParentIndex2.RefID = parent.RefID;
                        obj.ParentIndex2.Name = parent.Name.ToString();
                        obj.Root = parent;
                        if (obj.Parent == null) parent.Children.Add(obj);
                    }
                    else
                    {
                        throw new Exception("Unhandled Frame!");
                    }

                    obj.AddRef(FrameEntryRefTypes.Parent2, obj.ParentIndex2.RefID);
                }
                obj.SetWorldTransform();
            }
        }

        public void SanitizeFrameData()
        {
            Dictionary<int, bool> isGeomUsed = new Dictionary<int, bool>(frameGeometries.Count);
            Dictionary<int, bool> isMatUsed = new Dictionary<int, bool>(frameMaterials.Count);
            Dictionary<int, bool> isBlendInfoUsed = new Dictionary<int, bool>(frameBlendInfos.Count);
            Dictionary<int, bool> isSkelUsed = new Dictionary<int, bool>(frameSkeletons.Count);
            Dictionary<int, bool> isSkelHierUsed = new Dictionary<int, bool>(frameSkeletonHierachies.Count);

            foreach (KeyValuePair<int, FrameGeometry> entry in frameGeometries)
            {
                isGeomUsed.Add(entry.Key, false);
            }

            foreach (KeyValuePair<int, FrameMaterial> entry in frameMaterials)
            {
                isMatUsed.Add(entry.Key, false);
            }

            foreach (KeyValuePair<int, FrameBlendInfo> entry in frameBlendInfos)
            {
                isBlendInfoUsed.Add(entry.Key, false);
            }

            foreach (KeyValuePair<int, FrameSkeleton> entry in frameSkeletons)
            {
                isSkelUsed.Add(entry.Key, false);
            }

            foreach (KeyValuePair<int, FrameSkeletonHierachy> entry in frameSkeletonHierachies)
            {
                isSkelHierUsed.Add(entry.Key, false);
            }

            foreach (KeyValuePair<int, object> entry in frameObjects)
            {
                if (entry.Value is FrameObjectModel)
                {
                    FrameObjectModel mesh = (entry.Value as FrameObjectModel);
                    isGeomUsed[mesh.Refs[FrameEntryRefTypes.Geometry]] = true;
                    isMatUsed[mesh.Refs[FrameEntryRefTypes.Material]] = true;
                    isBlendInfoUsed[mesh.Refs[FrameEntryRefTypes.BlendInfo]] = true;
                    isSkelHierUsed[mesh.Refs[FrameEntryRefTypes.SkeletonHierachy]] = true;
                    isSkelUsed[mesh.Refs[FrameEntryRefTypes.Skeleton]] = true;

                }
                else if (entry.Value is FrameObjectSingleMesh)
                {
                    FrameObjectSingleMesh mesh = (entry.Value as FrameObjectSingleMesh);

                    if (mesh.MeshIndex > -1)
                    {
                        isGeomUsed[mesh.Refs[FrameEntryRefTypes.Geometry]] = true;
                    }

                    if (mesh.MaterialIndex > -1)
                    {
                        isMatUsed[mesh.Refs[FrameEntryRefTypes.Material]] = true;
                    }
                }
            }

            for (int i = 0; i != isGeomUsed.Count; i++)
            {
                KeyValuePair<int, bool> pair = isGeomUsed.ElementAt(i);
                if (pair.Value != true)
                {
                    frameGeometries.Remove(pair.Key);
                    Console.WriteLine("Deleted with ID: {0}", pair.Key);
                }

            }
            for (int i = 0; i != isMatUsed.Count; i++)
            {
                KeyValuePair<int, bool> pair = isMatUsed.ElementAt(i);
                if (pair.Value != true)
                {
                    frameMaterials.Remove(pair.Key);
                    Console.WriteLine("Deleted with ID: {0}", pair.Key);
                }
            }
            for (int i = 0; i != isBlendInfoUsed.Count; i++)
            {
                KeyValuePair<int, bool> pair = isBlendInfoUsed.ElementAt(i);
                if (pair.Value != true)
                {
                    frameBlendInfos.Remove(pair.Key);
                    Console.WriteLine("Deleted with ID: {0}", pair.Key);
                }
            }
            for (int i = 0; i != isSkelUsed.Count; i++)
            {
                KeyValuePair<int, bool> pair = isSkelUsed.ElementAt(i);
                if (pair.Value != true)
                {
                    frameSkeletons.Remove(pair.Key);
                    Console.WriteLine("Deleted with ID: {0}", pair.Key);
                }
            }
            for (int i = 0; i != isSkelHierUsed.Count; i++)
            {
                KeyValuePair<int, bool> pair = isSkelHierUsed.ElementAt(i);
                if (pair.Value != true)
                {
                    frameSkeletonHierachies.Remove(pair.Key);
                    Console.WriteLine("Deleted with ID: {0}", pair.Key);
                }
            }

        }

        public void UpdateFrameData()
        {
            SanitizeFrameData();

            int[] offsets = new int[7];
            offsets[0] = 0;
            offsets[1] = offsets[0] + frameScenes.Count;
            offsets[2] = offsets[1] + frameGeometries.Count;
            offsets[3] = offsets[2] + frameMaterials.Count;
            offsets[4] = offsets[3] + frameBlendInfos.Count;
            offsets[5] = offsets[4] + frameSkeletons.Count;
            offsets[6] = offsets[5] + frameSkeletonHierachies.Count;

            for (int i = 0; i < frameObjects.Count; i++)
            {
                FrameObjectBase block = (frameObjects.ElementAt(i).Value as FrameObjectBase);
                Console.WriteLine("Working on block " + block.Name.String);

                if (block.Refs.ContainsKey(FrameEntryRefTypes.Parent1))
                {
                    if (frameScenes.ContainsKey(block.Refs[FrameEntryRefTypes.Parent1]))
                    {
                        block.ParentIndex1.Index = frameScenes.IndexOfValue(block.Refs[FrameEntryRefTypes.Parent1]);
                    }
                    else
                    {
                        block.ParentIndex1.Index = offsets[6] + (block.Refs.ContainsKey(FrameEntryRefTypes.Parent1) ? frameObjects.IndexOfValue(block.Refs[FrameEntryRefTypes.Parent1]) : -1);
                    }
                }


                if (block.Refs.ContainsKey(FrameEntryRefTypes.Parent2))
                {
                    if (frameScenes.ContainsKey(block.Refs[FrameEntryRefTypes.Parent2]))
                    {
                        block.ParentIndex2.Index = frameScenes.IndexOfValue(block.Refs[FrameEntryRefTypes.Parent2]);
                    }
                    else
                    {
                        block.ParentIndex2.Index = offsets[6] + (block.Refs.ContainsKey(FrameEntryRefTypes.Parent2) ? frameObjects.IndexOfValue(block.Refs[FrameEntryRefTypes.Parent2]) : -1);
                    }
                }


                if (block.Type == typeof(FrameObjectSingleMesh).ToString())
                {
                    FrameObjectSingleMesh mesh = (block as FrameObjectSingleMesh);
                    Console.WriteLine(string.Format("Updating: {0}, {1}, {2}", block.Name, mesh.MaterialIndex, mesh.MeshIndex));
                    if (mesh.MeshIndex != -1) mesh.MeshIndex = offsets[1] + frameGeometries.IndexOfValue(mesh.Refs[FrameEntryRefTypes.Geometry]);
                    if (mesh.MaterialIndex != -1) mesh.MaterialIndex = offsets[2] + frameMaterials.IndexOfValue(mesh.Refs[FrameEntryRefTypes.Material]);
                    block = mesh;
                    Console.WriteLine(string.Format("Updated: {0}, {1}, {2}", block.Name, mesh.MaterialIndex, mesh.MeshIndex));
                }
                if (block.Type == typeof(FrameObjectModel).ToString())
                {
                    FrameObjectModel mesh = (block as FrameObjectModel);
                    Console.WriteLine(string.Format("Updating: {0}, {1}, {2}", block.Name, mesh.MaterialIndex, mesh.MeshIndex));
                    if (mesh.MeshIndex != -1) mesh.MeshIndex = offsets[1] + frameGeometries.IndexOfValue(mesh.Refs[FrameEntryRefTypes.Geometry]);
                    if (mesh.MaterialIndex != -1) mesh.MaterialIndex = offsets[2] + frameMaterials.IndexOfValue(mesh.Refs[FrameEntryRefTypes.Material]);
                    if (mesh.BlendInfoIndex != -1) mesh.BlendInfoIndex = offsets[3] + frameBlendInfos.IndexOfValue(mesh.Refs[FrameEntryRefTypes.BlendInfo]);
                    if (mesh.SkeletonIndex != -1) mesh.SkeletonIndex = offsets[4] + frameSkeletons.IndexOfValue(mesh.Refs[FrameEntryRefTypes.Skeleton]);
                    if (mesh.SkeletonHierachyIndex != -1) mesh.SkeletonHierachyIndex = offsets[5] + frameSkeletonHierachies.IndexOfValue(mesh.Refs[FrameEntryRefTypes.SkeletonHierachy]);

                    foreach (var attachment in mesh.AttachmentReferences)
                    {
                        attachment.AttachmentIndex = offsets[6] + frameObjects.IndexOfValue(attachment.Attachment.RefID);
                    }

                    block = mesh;
                    Console.WriteLine(string.Format("Updated: {0}, {1}, {2}", block.Name, mesh.MaterialIndex, mesh.MeshIndex));
                }
            }

            header.SceneFolders = frameScenes.Values.ToList();

            header.NumFolderNames = frameScenes.Count;
            header.NumGeometries = frameGeometries.Count;
            header.NumMaterialResources = frameMaterials.Count;
            header.NumBlendInfos = frameBlendInfos.Count;
            header.NumSkeletons = frameSkeletons.Count;
            header.NumSkelHierachies = frameSkeletonHierachies.Count;
            header.NumObjects = frameObjects.Count;
            header.NumFolderNames = frameScenes.Count;
        }

        public static bool IsFrameType(object entry)
        {
            if (entry.GetType() == typeof(FrameObjectJoint) ||
                entry.GetType() == typeof(FrameObjectSingleMesh) ||
                entry.GetType() == typeof(FrameObjectFrame) ||
                entry.GetType() == typeof(FrameObjectLight) ||
                entry.GetType() == typeof(FrameObjectCamera) ||
                entry.GetType() == typeof(FrameObjectComponent_U005) ||
                entry.GetType() == typeof(FrameObjectSector) ||
                entry.GetType() == typeof(FrameObjectDummy) ||
                entry.GetType() == typeof(FrameObjectDeflector) ||
                entry.GetType() == typeof(FrameObjectArea) ||
                entry.GetType() == typeof(FrameObjectTarget) ||
                entry.GetType() == typeof(FrameObjectModel) ||
                entry.GetType() == typeof(FrameObjectCollision))
                return true;

            return false;
        }
    }
}
