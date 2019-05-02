using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Mafia2;
using System.Diagnostics;
using System.Windows.Forms;
using Utils.Extensions;

namespace ResourceTypes.FrameResource
{
    public struct FrameHolder
    {
        public int Idx;
        public FrameEntry Data;

        public FrameHolder(int idx, FrameEntry data)
        {
            Idx = idx;
            Data = data;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Idx, Data.ToString());
        }
    }

    public class FrameResource
    {
        FrameHeader header;
        Dictionary<int, FrameHeaderScene> frameScenes = new Dictionary<int, FrameHeaderScene>();
        Dictionary<int, FrameGeometry> frameGeometries = new Dictionary<int, FrameGeometry>();
        Dictionary<int, FrameMaterial> frameMaterials = new Dictionary<int, FrameMaterial>();
        Dictionary<int, FrameBlendInfo> frameBlendInfos = new Dictionary<int, FrameBlendInfo>();
        Dictionary<int, FrameSkeleton> frameSkeletons = new Dictionary<int, FrameSkeleton>();
        Dictionary<int, FrameSkeletonHierachy> frameSkeletonHierachies = new Dictionary<int, FrameSkeletonHierachy>();
        Dictionary<int, object> frameObjects = new Dictionary<int, object>();
        //Dictionary<int, object> FrameEntries = new Dictionary<int, object>();
        List<FrameHolder> newFrames = new List<FrameHolder>();

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
        public List<FrameHolder> NewFrames {
            get { return newFrames; }
            set { newFrames = value; }
        }

        private FrameHolder GetLocalEntryFromRefID(List<FrameHolder> frames, int refID)
        {
            foreach (FrameHolder holder in frames)
            {
                if (holder.Data.RefID == refID)
                    return holder;
            }

            return new FrameHolder(-1, null);
        }

        public FrameHolder GetEntryFromIdx(int idx)
        {
            foreach (FrameHolder holder in NewFrames)
            {
                if (holder.Idx == idx)
                    return holder;
            }

            return new FrameHolder(-1, null);
        }

        public FrameResource(string file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        /// <summary>
        /// Reads the file into the memory.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            header = new FrameHeader();
            header.ReadFromFile(reader);

            int j = 0;

            for (int i = 0; i != header.SceneFolders.Length; i++)
            {
                frameScenes.Add(header.SceneFolders[i].RefID, header.SceneFolders[i]);
                NewFrames.Add(new FrameHolder(j++, header.SceneFolders[i]));
            }
            for (int i = 0; i != header.NumGeometries; i++)
            {
                FrameGeometry geo = new FrameGeometry(reader);
                frameGeometries.Add(geo.RefID, geo);
                NewFrames.Add(new FrameHolder(j++, geo));
            }
            for (int i = 0; i != header.NumMaterialResources; i++)
            {
                FrameMaterial mat = new FrameMaterial(reader);
                frameMaterials.Add(mat.RefID, mat);
                NewFrames.Add(new FrameHolder(j++, mat));
            }
            for (int i = 0; i != header.NumBlendInfos; i++)
            {
                FrameBlendInfo blendInfo = new FrameBlendInfo(reader);
                frameBlendInfos.Add(blendInfo.RefID, blendInfo);
                NewFrames.Add(new FrameHolder(j++, blendInfo));
            }
            for (int i = 0; i != header.NumSkeletons; i++)
            {
                FrameSkeleton skeleton = new FrameSkeleton(reader);
                frameSkeletons.Add(skeleton.RefID, skeleton);
                NewFrames.Add(new FrameHolder(j++, skeleton));
            }
            for (int i = 0; i != header.NumSkelHierachies; i++)
            {
                FrameSkeletonHierachy skeletonHierachy = new FrameSkeletonHierachy(reader);
                frameSkeletonHierachies.Add(skeletonHierachy.RefID, skeletonHierachy);
                NewFrames.Add(new FrameHolder(j++, skeletonHierachy));
            }

            int[] objectTypes = new int[header.NumObjects];
            int numBlocks = header.NumFolderNames + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies;

            if (header.NumObjects > 0)
            {
                for (int i = 0; i != header.NumObjects; i++)
                    objectTypes[i] = reader.ReadInt32();

                for (int i = 0; i != header.NumObjects; i++)
                {
                    FrameObjectBase newObject = new FrameObjectBase();
                    if (objectTypes[i] == (int)ObjectType.Joint)
                        newObject = new FrameObjectJoint(reader);

                    else if (objectTypes[i] == (int)ObjectType.SingleMesh)
                    {
                        newObject = new FrameObjectSingleMesh(reader);
                        FrameObjectSingleMesh mesh = newObject as FrameObjectSingleMesh;

                        if (mesh.MeshIndex != -1)
                            mesh.AddRef(FrameEntryRefTypes.Mesh, GetEntryFromIdx(mesh.MeshIndex).Data.RefID);

                        if (mesh.MaterialIndex != -1)
                            mesh.AddRef(FrameEntryRefTypes.Material, GetEntryFromIdx(mesh.MaterialIndex).Data.RefID);
                    }
                    else if (objectTypes[i] == (int)ObjectType.Frame)
                        newObject = new FrameObjectFrame(reader);

                    else if (objectTypes[i] == (int)ObjectType.Light)
                        newObject = new FrameObjectLight(reader);

                    else if (objectTypes[i] == (int)ObjectType.Camera)
                        newObject = new FrameObjectCamera(reader);

                    else if (objectTypes[i] == (int)ObjectType.Component_U00000005)
                        newObject = new FrameObjectComponent_U005(reader);

                    else if (objectTypes[i] == (int)ObjectType.Sector)
                        newObject = new FrameObjectSector(reader);

                    else if (objectTypes[i] == (int)ObjectType.Dummy)
                        newObject = new FrameObjectDummy(reader);

                    else if (objectTypes[i] == (int)ObjectType.ParticleDeflector)
                        newObject = new FrameObjectDeflector(reader);

                    else if (objectTypes[i] == (int)ObjectType.Area)
                        newObject = new FrameObjectArea(reader);

                    else if (objectTypes[i] == (int)ObjectType.Target)
                        newObject = new FrameObjectTarget(reader);

                    else if (objectTypes[i] == (int)ObjectType.Model)
                    {
                        FrameObjectModel mesh = new FrameObjectModel(reader);
                        mesh.ReadFromFile(reader);
                        mesh.ReadFromFilePart2(reader, (FrameSkeleton)GetEntryFromIdx(mesh.SkeletonIndex).Data, (FrameBlendInfo)GetEntryFromIdx(mesh.BlendInfoIndex).Data);
                        mesh.AddRef(FrameEntryRefTypes.Mesh, GetEntryFromIdx(mesh.MeshIndex).Data.RefID);
                        mesh.AddRef(FrameEntryRefTypes.Material, GetEntryFromIdx(mesh.MaterialIndex).Data.RefID);
                        mesh.AddRef(FrameEntryRefTypes.BlendInfo, GetEntryFromIdx(mesh.BlendInfoIndex).Data.RefID);
                        mesh.AddRef(FrameEntryRefTypes.Skeleton, GetEntryFromIdx(mesh.SkeletonIndex).Data.RefID);
                        mesh.AddRef(FrameEntryRefTypes.SkeletonHierachy, GetEntryFromIdx(mesh.SkeletonHierachyIndex).Data.RefID);
                        newObject = mesh;
                    }
                    else if (objectTypes[i] == (int)ObjectType.Collision)
                        newObject = new FrameObjectCollision(reader);

                    frameObjects.Add(newObject.RefID, newObject);
                    NewFrames.Add(new FrameHolder(i + numBlocks, newObject));
                }
            }
            objectTypes = null;
            DefineFrameBlockParents();
        }

        /// <summary>
        /// Writes the FrameResource to the file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            //BEFORE WE WRITE, WE NEED TO COMPILE AND UPDATE THE FRAME.
            UpdateFrameData();
            header.WriteToFile(writer);

            int totalBlockCount = header.NumFolderNames + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies;
            int currentIdx = header.SceneFolders.Length;

            for (; currentIdx != header.SceneFolders.Length + header.NumGeometries; currentIdx++)
                (NewFrames[currentIdx].Data as FrameGeometry).WriteToFile(writer);

            for (; currentIdx != header.SceneFolders.Length + header.NumGeometries + header.NumMaterialResources; currentIdx++)
                (NewFrames[currentIdx].Data as FrameMaterial).WriteToFile(writer);

            for (; currentIdx != header.SceneFolders.Length + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos; currentIdx++)
                (NewFrames[currentIdx].Data as FrameBlendInfo).WriteToFile(writer);

            for (; currentIdx != header.SceneFolders.Length + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons; currentIdx++)
                (NewFrames[currentIdx].Data as FrameSkeleton).WriteToFile(writer);

            for (; currentIdx != header.SceneFolders.Length + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies; currentIdx++)
                (NewFrames[currentIdx].Data as FrameSkeletonHierachy).WriteToFile(writer);

            int savedIdx = currentIdx;

            for (; savedIdx != header.SceneFolders.Length + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies + header.NumObjects; savedIdx++)
            {
                FrameEntry entry = NewFrames[savedIdx].Data;

                if (entry.GetType() == typeof(FrameObjectJoint))
                    writer.Write((int)ObjectType.Joint);
                else if (entry.GetType() == typeof(FrameObjectSingleMesh))
                    writer.Write((int)ObjectType.SingleMesh);
                else if (entry.GetType() == typeof(FrameObjectFrame))
                    writer.Write((int)ObjectType.Frame);
                else if (entry.GetType() == typeof(FrameObjectLight))
                    writer.Write((int)ObjectType.Light);
                else if (entry.GetType() == typeof(FrameObjectCamera))
                    writer.Write((int)ObjectType.Camera);
                else if (entry.GetType() == typeof(FrameObjectComponent_U005))
                    writer.Write((int)ObjectType.Component_U00000005);
                else if (entry.GetType() == typeof(FrameObjectSector))
                    writer.Write((int)ObjectType.Sector);
                else if (entry.GetType() == typeof(FrameObjectDummy))
                    writer.Write((int)ObjectType.Dummy);
                else if (entry.GetType() == typeof(FrameObjectDeflector))
                    writer.Write((int)ObjectType.ParticleDeflector);
                else if (entry.GetType() == typeof(FrameObjectArea))
                    writer.Write((int)ObjectType.Area);
                else if (entry.GetType() == typeof(FrameObjectTarget))
                    writer.Write((int)ObjectType.Target);
                else if (entry.GetType() == typeof(FrameObjectModel))
                    writer.Write((int)ObjectType.Model);
                else if (entry.GetType() == typeof(FrameObjectCollision))
                    writer.Write((int)ObjectType.Collision);
            }

            savedIdx = currentIdx;

            for (; savedIdx != header.SceneFolders.Length + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies + header.NumObjects; savedIdx++)
            {
                FrameEntry entry = NewFrames[savedIdx].Data;

                if (entry.GetType() == typeof(FrameObjectJoint))
                    (entry as FrameObjectJoint).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectSingleMesh))
                    (entry as FrameObjectSingleMesh).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectFrame))
                    (entry as FrameObjectFrame).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectLight))
                    (entry as FrameObjectLight).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectCamera))
                    (entry as FrameObjectCamera).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectComponent_U005))
                    (entry as FrameObjectComponent_U005).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectSector))
                    (entry as FrameObjectSector).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectDummy))
                    (entry as FrameObjectDummy).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectDeflector))
                    (entry as FrameObjectDeflector).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectArea))
                    (entry as FrameObjectArea).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectTarget))
                    (entry as FrameObjectTarget).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectModel))
                    (entry as FrameObjectModel).WriteToFile(writer);
                else if (entry.GetType() == typeof(FrameObjectCollision))
                    (entry as FrameObjectCollision).WriteToFile(writer);
            }
        }

        public TreeNode BuildTree(FrameNameTable.FrameNameTable table)
        {
            TreeNode root = new TreeNode("Root");

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

                root.Nodes.Add(node);
            }

            //add entries from the table, add table data and then add to scene viewer.
            for (int i = 0; i != table.FrameData.Length; i++)
            {
                if (table.FrameData[i].FrameIndex == -1)
                    continue;

                var frameData = table.FrameData[i];
                FrameObjectBase fObject = (GetEntryFromIdx(numBlocks + frameData.FrameIndex).Data as FrameObjectBase);
                fObject.IsOnFrameTable = true;
                fObject.FrameNameTableFlags = table.FrameData[i].Flags;
                int p1idx = numBlocks + fObject.ParentIndex1.Index;
                int p2idx = numBlocks + fObject.ParentIndex2.Index;
                int thisKey = numBlocks + table.FrameData[i].FrameIndex;

                TreeNode node = (!parsedNodes.ContainsKey(thisKey)) ? new TreeNode(fObject.ToString()) : parsedNodes[thisKey];
                node.Tag = fObject;
                node.Name = fObject.RefID.ToString();

                if (p1idx == -1 && p2idx == -1)
                {
                    //might be temp? it fixes cars loading in or non binded entries.
                    root.Nodes.Add(node);
                    continue;
                }
                else
                {
                    FrameEntry pBase = (GetEntryFromIdx(p2idx).Data as FrameEntry);
                    TreeNode[] nodes = root.Nodes.Find(pBase.RefID.ToString(), true);

                    if (nodes.Length > 0)
                        nodes[0].Nodes.Add(node);
                }

                if (!parsedNodes.ContainsKey(thisKey))
                    parsedNodes.Add(thisKey, node);
            }

            //add objects from the main dictionary.
            foreach (FrameHolder holder in NewFrames)
            {
                FrameObjectBase fObject = holder.Data as FrameObjectBase;

                if (fObject == null)
                    continue;

                int p1idx = fObject.ParentIndex1.Index;
                int p2idx = fObject.ParentIndex2.Index;
                int thisKey = holder.Idx;

                TreeNode node = (!parsedNodes.ContainsKey(thisKey)) ? new TreeNode(fObject.ToString()) : parsedNodes[thisKey];
                node.Tag = fObject;
                node.Name = fObject.RefID.ToString();

                if (fObject.GetType() == typeof(FrameObjectLight))
                    node.ImageIndex = 1;

                if (root.Nodes.Find(fObject.RefID.ToString(), true).Length > 0)
                    continue;

                if (p2idx != -1)
                {
                    FrameEntry pBase2 = (GetEntryFromIdx(p2idx).Data as FrameEntry);
                    TreeNode[] p2Nodes = root.Nodes.Find(pBase2.RefID.ToString(), true);

                    if (p2Nodes.Length > 0)
                    {
                        p2Nodes[0].Nodes.Add(node);
                    }
                    else
                    {
                        Console.WriteLine("did not add {0}", node.Text);
                        notAddedNodes.Add(thisKey, node);
                    }
                }

                if (!parsedNodes.ContainsKey(thisKey))
                    parsedNodes.Add(thisKey, node);
            }

            //update p1 objects in tree nodes
            foreach (FrameHolder entry in NewFrames)
            {
                FrameObjectBase fObject = (entry.Data as FrameObjectBase);

                if (fObject == null)
                    continue;

                int p1idx = fObject.ParentIndex1.Index;
                int p2idx = fObject.ParentIndex2.Index;
                int thisKey = numBlocks + entry.Idx;

                if (p2idx != -1 && p1idx != -1)
                {
                    TreeNode node = new TreeNode(fObject.ToString());
                    node.Tag = fObject;
                    node.Name = fObject.RefID.ToString();

                    if (fObject.GetType() == typeof(FrameObjectLight))
                        node.ImageIndex = 1;

                    FrameEntry p1Base = GetEntryFromIdx(p1idx).Data as FrameEntry;
                    FrameEntry p2Base = GetEntryFromIdx(p2idx).Data as FrameEntry;

                    TreeNode[] p2Node = root.Nodes.Find(p2Base.RefID.ToString(), true);

                    if (p2Node.Length > 0)
                    {
                        TreeNode[] p1Node = p2Node[0].Nodes.Find(p1Base.RefID.ToString(), true);

                        if (p1Node.Length > 0)
                        {
                            p2Node[0].Nodes.RemoveByKey(fObject.RefID.ToString());
                            p1Node[0].Nodes.Add(node);
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, TreeNode> entry in parsedNodes)
            {
                FrameObjectBase objBase = (entry.Value.Tag as FrameObjectBase);

                if (objBase != null)
                {
                    if (objBase.ParentIndex1.Index == -1 && objBase.ParentIndex2.Index == -1)
                    {
                        root.Nodes.Add(entry.Value);
                    }
                }
            }

            foreach (KeyValuePair<int, TreeNode> entry in notAddedNodes)
            {
                FrameObjectBase objBase = (entry.Value.Tag as FrameObjectBase);
                TreeNode[] nodes = root.Nodes.Find(objBase.ParentIndex2.RefID.ToString(), true);

                if (nodes.Length > 0)
                {
                    nodes[0].Nodes.Add(entry.Value);
                    Debug.WriteLine("Added {0}", objBase.Name);
                }
                else
                {
                    root.Nodes.Add(entry.Value);
                }
            }
            return root;
        }

        /// <summary>
        /// Adds names onto ParentIndex1 and ParentIndex2. Called after the file has been read.
        /// Adds Refs also. These are needed to save a Frame file.
        /// </summary>
        public void DefineFrameBlockParents()
        {
            int numBlocks = header.NumFolderNames + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies;

            for (int i = 0; i != NewFrames.Count; i++)
            {
                FrameObjectBase obj = (GetEntryFromIdx(i).Data as FrameObjectBase);

                if (obj == null)
                    continue;

                if (obj.ParentIndex1.Index > -1)
                {
                    if (obj.ParentIndex1.Index <= (frameScenes.Count - 1) && (frameScenes.Count - 1) != -1)
                    {
                        obj.ParentIndex1.RefID = (frameScenes.ElementAt(obj.ParentIndex1.Index).Value as FrameHeaderScene).RefID;
                        obj.ParentIndex1.Name = (frameScenes.ElementAt(obj.ParentIndex1.Index).Value as FrameHeaderScene).Name.String;
                    }
                    else if (obj.ParentIndex1.Index >= numBlocks)
                    {
                        obj.ParentIndex1.RefID = (GetEntryFromIdx(obj.ParentIndex1.Index).Data as FrameObjectBase).RefID;
                        obj.ParentIndex1.Name = (GetEntryFromIdx(obj.ParentIndex1.Index).Data as FrameObjectBase).Name.String;
                    }
                    obj.AddRef(FrameEntryRefTypes.Parent1, obj.ParentIndex1.RefID);
                }

                if (obj.ParentIndex2.Index > -1)
                {
                    if (obj.ParentIndex2.Index <= (frameScenes.Count - 1) && (frameScenes.Count - 1) != -1)
                    {
                        obj.ParentIndex2.RefID = (frameScenes.ElementAt(obj.ParentIndex2.Index).Value as FrameHeaderScene).RefID;
                        obj.ParentIndex2.Name = (frameScenes.ElementAt(obj.ParentIndex2.Index).Value as FrameHeaderScene).Name.String;
                    }
                    else if (obj.ParentIndex2.Index >= numBlocks)
                    {
                        obj.ParentIndex2.RefID = (GetEntryFromIdx(obj.ParentIndex2.Index).Data as FrameObjectBase).RefID;
                        obj.ParentIndex2.Name = (GetEntryFromIdx(obj.ParentIndex2.Index).Data as FrameObjectBase).Name.String;
                    }

                    obj.AddRef(FrameEntryRefTypes.Parent2, obj.ParentIndex2.RefID);
                }
            }
        }

        public void UpdateFrameData()
        {
            int totalResources = header.NumFolderNames + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies;
            int currentCount = 0;

            List<FrameHolder> updatedFrames = new List<FrameHolder>();

            foreach (KeyValuePair<int, FrameHeaderScene> entry in frameScenes)
                updatedFrames.Add(new FrameHolder(currentCount++, entry.Value));

            foreach (KeyValuePair<int, FrameGeometry> entry in frameGeometries)
                updatedFrames.Add(new FrameHolder(currentCount++, entry.Value));

            foreach (KeyValuePair<int, FrameMaterial> entry in frameMaterials)
                updatedFrames.Add(new FrameHolder(currentCount++, entry.Value));

            foreach (KeyValuePair<int, FrameBlendInfo> entry in frameBlendInfos)
                updatedFrames.Add(new FrameHolder(currentCount++, entry.Value));

            foreach (KeyValuePair<int, FrameSkeleton> entry in frameSkeletons)
                updatedFrames.Add(new FrameHolder(currentCount++, entry.Value));

            foreach (KeyValuePair<int, FrameSkeletonHierachy> entry in frameSkeletonHierachies)
                updatedFrames.Add(new FrameHolder(currentCount++, entry.Value));

            //We have to add the objects to the new frame AND THEN update refs. This is kind of odd, and I think i've done something wrong.
            int objectPosStart = currentCount;
            for (int i = 0; i != frameObjects.Count; i++)
            {
                FrameObjectBase block = (frameObjects.ElementAt(i).Value as FrameObjectBase);
                updatedFrames.Add(new FrameHolder(currentCount++, block));
            }
            for (int i = objectPosStart; i != updatedFrames.Count; i++)
            {
                FrameObjectBase block = (updatedFrames[i].Data as FrameObjectBase);
                Console.WriteLine("Working on block " + block.Name.String);

                if (block.Refs.ContainsKey("Parent1"))
                    block.ParentIndex1.Index = GetLocalEntryFromRefID(updatedFrames, block.Refs["Parent1"]).Idx;
                else
                    block.ParentIndex1.Index = -1;

                if (block.Refs.ContainsKey("Parent2"))
                    block.ParentIndex2.Index = GetLocalEntryFromRefID(updatedFrames, block.Refs["Parent2"]).Idx;
                else
                    block.ParentIndex2.Index = -1;

                if (block.Type == typeof(FrameObjectSingleMesh).ToString())
                {
                    FrameObjectSingleMesh mesh = (block as FrameObjectSingleMesh);
                    Console.WriteLine(string.Format("Updating: {0}, {1}, {2}", block.Name, mesh.MaterialIndex, mesh.MeshIndex));

                    if (mesh.MaterialIndex != -1)
                        mesh.MaterialIndex = GetLocalEntryFromRefID(updatedFrames, block.Refs["Material"]).Idx;

                    if (mesh.MeshIndex != -1)
                        mesh.MeshIndex = GetLocalEntryFromRefID(updatedFrames, block.Refs["Mesh"]).Idx;

                    block = mesh;
                    Console.WriteLine(string.Format("Updated: {0}, {1}, {2}", block.Name, mesh.MaterialIndex, mesh.MeshIndex));
                }
                if (block.Type == typeof(FrameObjectModel).ToString())
                {
                    FrameObjectModel mesh = (block as FrameObjectModel);
                    Console.WriteLine(string.Format("Updating: {0}, {1}, {2}", block.Name, mesh.MaterialIndex, mesh.MeshIndex));
                    mesh.MaterialIndex = GetLocalEntryFromRefID(updatedFrames, block.Refs["Material"]).Idx;
                    mesh.MeshIndex = GetLocalEntryFromRefID(updatedFrames, block.Refs["Mesh"]).Idx;
                    mesh.BlendInfoIndex = GetLocalEntryFromRefID(updatedFrames, block.Refs["BlendInfo"]).Idx;
                    mesh.SkeletonIndex = GetLocalEntryFromRefID(updatedFrames, block.Refs["Skeleton"]).Idx;
                    mesh.SkeletonHierachyIndex = GetLocalEntryFromRefID(updatedFrames, block.Refs["SkeletonHierachy"]).Idx;
                    block = mesh;
                    Console.WriteLine(string.Format("Updated: {0}, {1}, {2}", block.Name, mesh.MaterialIndex, mesh.MeshIndex));
                }
            }

            header.NumFolderNames = FrameScenes.Count;
            header.NumGeometries = frameGeometries.Count;
            header.NumMaterialResources = frameMaterials.Count;
            header.NumBlendInfos = frameBlendInfos.Count;
            header.NumSkeletons = frameSkeletons.Count;
            header.NumSkelHierachies = frameSkeletonHierachies.Count;
            header.NumObjects = frameObjects.Count;
            NewFrames = updatedFrames;
        }
    }
}
