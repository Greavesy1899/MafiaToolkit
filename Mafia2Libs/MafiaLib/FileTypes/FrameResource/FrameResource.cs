using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public int GetIndexOfObject(int refID)
        {
            for (int i = 0; i != frameObjects.Count; i++)
            {
                if (frameObjects.ElementAt(i).Key == refID)
                    return i;
            }
            return -1;
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
            using (MemoryStream reader = new MemoryStream(File.ReadAllBytes(file), false))
            {
                ReadFromFile(reader, false);
            }
        }

        public FrameResource()
        {
            header = new FrameHeader();
            frameScenes = new Dictionary<int, FrameHeaderScene>();
            frameGeometries = new Dictionary<int, FrameGeometry>();
            frameMaterials = new Dictionary<int, FrameMaterial>();
            frameBlendInfos = new Dictionary<int, FrameBlendInfo>();
            frameSkeletons = new Dictionary<int, FrameSkeleton>();
            frameSkeletonHierachies = new Dictionary<int, FrameSkeletonHierachy>();
            frameObjects = new Dictionary<int, object>();
            newFrames = new List<FrameHolder>();
        }

        public FrameHeaderScene AddSceneFolder(string name)
        {
            FrameHeaderScene scene = new FrameHeaderScene();
            scene.Name = new Utils.Types.Hash(name);
            header.SceneFolders.Add(scene);
            frameScenes.Add(scene.RefID, scene);
            return scene;
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            header = new FrameHeader();
            header.ReadFromFile(reader, isBigEndian);

            int j = 0;

            for (int i = 0; i != header.SceneFolders.Count; i++)
            {
                frameScenes.Add(header.SceneFolders[i].RefID, header.SceneFolders[i]);
                NewFrames.Add(new FrameHolder(j++, header.SceneFolders[i]));
            }
            for (int i = 0; i != header.NumGeometries; i++)
            {
                FrameGeometry geo = new FrameGeometry(reader, isBigEndian);
                frameGeometries.Add(geo.RefID, geo);
                NewFrames.Add(new FrameHolder(j++, geo));
            }
            for (int i = 0; i != header.NumMaterialResources; i++)
            {
                FrameMaterial mat = new FrameMaterial(reader, isBigEndian);
                frameMaterials.Add(mat.RefID, mat);
                NewFrames.Add(new FrameHolder(j++, mat));
            }
            for (int i = 0; i != header.NumBlendInfos; i++)
            {
                FrameBlendInfo blendInfo = new FrameBlendInfo(reader, isBigEndian);
                frameBlendInfos.Add(blendInfo.RefID, blendInfo);
                NewFrames.Add(new FrameHolder(j++, blendInfo));
            }
            for (int i = 0; i != header.NumSkeletons; i++)
            {
                FrameSkeleton skeleton = new FrameSkeleton(reader, isBigEndian);
                frameSkeletons.Add(skeleton.RefID, skeleton);
                NewFrames.Add(new FrameHolder(j++, skeleton));
            }
            for (int i = 0; i != header.NumSkelHierachies; i++)
            {
                FrameSkeletonHierachy skeletonHierachy = new FrameSkeletonHierachy(reader, isBigEndian);
                frameSkeletonHierachies.Add(skeletonHierachy.RefID, skeletonHierachy);
                NewFrames.Add(new FrameHolder(j++, skeletonHierachy));
            }

            int[] objectTypes = new int[header.NumObjects];
            int numBlocks = header.NumFolderNames + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies;

            if (header.NumObjects > 0)
            {
                for (int i = 0; i != header.NumObjects; i++)
                    objectTypes[i] = reader.ReadInt32(isBigEndian);

                for (int i = 0; i != header.NumObjects; i++)
                {
                    FrameObjectBase newObject = new FrameObjectBase();
                    if (objectTypes[i] == (int)ObjectType.Joint)
                        newObject = new FrameObjectJoint(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.SingleMesh)
                    {
                        newObject = new FrameObjectSingleMesh(reader, isBigEndian);
                        FrameObjectSingleMesh mesh = newObject as FrameObjectSingleMesh;

                        if (mesh.MeshIndex != -1)
                        {
                            mesh.AddRef(FrameEntryRefTypes.Mesh, GetEntryFromIdx(mesh.MeshIndex).Data.RefID);
                            mesh.Geometry = frameGeometries[mesh.Refs["Mesh"]];
                        }

                        if (mesh.MaterialIndex != -1)
                        {
                            mesh.AddRef(FrameEntryRefTypes.Material, GetEntryFromIdx(mesh.MaterialIndex).Data.RefID);
                            mesh.Material = frameMaterials[mesh.Refs["Material"]];
                        }
                    }
                    else if (objectTypes[i] == (int)ObjectType.Frame)
                        newObject = new FrameObjectFrame(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.Light)
                        newObject = new FrameObjectLight(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.Camera)
                        newObject = new FrameObjectCamera(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.Component_U00000005)
                        newObject = new FrameObjectComponent_U005(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.Sector)
                        newObject = new FrameObjectSector(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.Dummy)
                        newObject = new FrameObjectDummy(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.ParticleDeflector)
                        newObject = new FrameObjectDeflector(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.Area)
                        newObject = new FrameObjectArea(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.Target)
                        newObject = new FrameObjectTarget(reader, isBigEndian);

                    else if (objectTypes[i] == (int)ObjectType.Model)
                    {
                        FrameObjectModel mesh = new FrameObjectModel(reader, isBigEndian);
                        mesh.ReadFromFile(reader, isBigEndian);
                        mesh.ReadFromFilePart2(reader, isBigEndian, (FrameSkeleton)GetEntryFromIdx(mesh.SkeletonIndex).Data, (FrameBlendInfo)GetEntryFromIdx(mesh.BlendInfoIndex).Data);
                        mesh.AddRef(FrameEntryRefTypes.Mesh, GetEntryFromIdx(mesh.MeshIndex).Data.RefID);
                        mesh.Geometry = frameGeometries[mesh.Refs[FrameEntry.MeshRef]];
                        mesh.AddRef(FrameEntryRefTypes.Material, GetEntryFromIdx(mesh.MaterialIndex).Data.RefID);
                        mesh.Material = frameMaterials[mesh.Refs[FrameEntry.MaterialRef]];
                        mesh.AddRef(FrameEntryRefTypes.BlendInfo, GetEntryFromIdx(mesh.BlendInfoIndex).Data.RefID);
                        mesh.BlendInfo = frameBlendInfos[mesh.Refs[FrameEntry.BlendInfoRef]];
                        mesh.AddRef(FrameEntryRefTypes.Skeleton, GetEntryFromIdx(mesh.SkeletonIndex).Data.RefID);
                        mesh.Skeleton = frameSkeletons[mesh.Refs[FrameEntry.SkeletonRef]];
                        mesh.AddRef(FrameEntryRefTypes.SkeletonHierachy, GetEntryFromIdx(mesh.SkeletonHierachyIndex).Data.RefID);
                        mesh.SkeletonHierarchy = frameSkeletonHierachies[mesh.Refs[FrameEntry.SkeletonHierRef]];
                        newObject = mesh;
                    }
                    else if (objectTypes[i] == (int)ObjectType.Collision)
                        newObject = new FrameObjectCollision(reader, isBigEndian);

                    frameObjects.Add(newObject.RefID, newObject);
                    NewFrames.Add(new FrameHolder(i + numBlocks, newObject));
                }
            }
            objectTypes = null;
            DefineFrameBlockParents();
        }

        public void WriteToFile(string name)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(name, FileMode.Create)))
                WriteToFile(writer);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            //BEFORE WE WRITE, WE NEED TO COMPILE AND UPDATE THE FRAME.
            UpdateFrameData();
            header.WriteToFile(writer);

            int totalBlockCount = header.NumFolderNames + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies;
            int currentIdx = header.SceneFolders.Count;

            for (; currentIdx != header.SceneFolders.Count + header.NumGeometries; currentIdx++)
                (NewFrames[currentIdx].Data as FrameGeometry).WriteToFile(writer);

            for (; currentIdx != header.SceneFolders.Count + header.NumGeometries + header.NumMaterialResources; currentIdx++)
                (NewFrames[currentIdx].Data as FrameMaterial).WriteToFile(writer);

            for (; currentIdx != header.SceneFolders.Count + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos; currentIdx++)
                (NewFrames[currentIdx].Data as FrameBlendInfo).WriteToFile(writer);

            for (; currentIdx != header.SceneFolders.Count + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons; currentIdx++)
                (NewFrames[currentIdx].Data as FrameSkeleton).WriteToFile(writer);

            for (; currentIdx != header.SceneFolders.Count + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies; currentIdx++)
                (NewFrames[currentIdx].Data as FrameSkeletonHierachy).WriteToFile(writer);

            int savedIdx = currentIdx;

            for (; savedIdx != header.SceneFolders.Count + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies + header.NumObjects; savedIdx++)
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

            for (; savedIdx != header.SceneFolders.Count + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies + header.NumObjects; savedIdx++)
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
            TreeNode root = new TreeNode("FrameResource Contents");
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
                parsedNodes.Add(i, node);
                root.Nodes.Add(node);
            }

            ////add entries from the table, add table data and then add to scene viewer.
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

            foreach (FrameHolder holder in NewFrames)
            {
                FrameObjectBase fObject = holder.Data as FrameObjectBase;

                if (fObject == null)
                    continue;

                TreeNode node = (!parsedNodes.ContainsKey(holder.Idx)) ? new TreeNode(fObject.ToString()) : parsedNodes[holder.Idx];
                node.Tag = fObject;
                node.Name = fObject.RefID.ToString();

                if (!parsedNodes.ContainsKey(holder.Idx))
                    parsedNodes.Add(holder.Idx, node);
            }

            foreach (FrameHolder holder in NewFrames)
            {
                FrameObjectBase fObject = holder.Data as FrameObjectBase;

                if (fObject == null)
                    continue;

                if (fObject.ParentIndex1.Index != -1)
                    parsedNodes[fObject.ParentIndex1.Index].Nodes.Add(parsedNodes[holder.Idx]);
                else if (fObject.ParentIndex2.Index != -1)
                    parsedNodes[fObject.ParentIndex2.Index].Nodes.Add(parsedNodes[holder.Idx]);
                else if (fObject.ParentIndex1.Index == -1 && fObject.ParentIndex2.Index == -1)
                    root.Nodes.Add(parsedNodes[holder.Idx]);
                else
                    Debug.WriteLine("Not added {0}", holder.Data);
            }
            return root;
        }

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
                        obj.ParentIndex1.Name = (frameScenes.ElementAt(obj.ParentIndex1.Index).Value as FrameHeaderScene).Name.ToString();
                    }
                    else if (obj.ParentIndex1.Index >= numBlocks)
                    {
                        obj.ParentIndex1.RefID = (GetEntryFromIdx(obj.ParentIndex1.Index).Data as FrameObjectBase).RefID;
                        obj.ParentIndex1.Name = (GetEntryFromIdx(obj.ParentIndex1.Index).Data as FrameObjectBase).Name.ToString();
                    }
                    obj.AddRef(FrameEntryRefTypes.Parent1, obj.ParentIndex1.RefID);
                }

                if (obj.ParentIndex2.Index > -1)
                {
                    if (obj.ParentIndex2.Index <= (frameScenes.Count - 1) && (frameScenes.Count - 1) != -1)
                    {
                        obj.ParentIndex2.RefID = (frameScenes.ElementAt(obj.ParentIndex2.Index).Value as FrameHeaderScene).RefID;
                        obj.ParentIndex2.Name = (frameScenes.ElementAt(obj.ParentIndex2.Index).Value as FrameHeaderScene).Name.ToString();
                    }
                    else if (obj.ParentIndex2.Index >= numBlocks)
                    {
                        obj.ParentIndex2.RefID = (GetEntryFromIdx(obj.ParentIndex2.Index).Data as FrameObjectBase).RefID;
                        obj.ParentIndex2.Name = (GetEntryFromIdx(obj.ParentIndex2.Index).Data as FrameObjectBase).Name.ToString();
                    }

                    obj.AddRef(FrameEntryRefTypes.Parent2, obj.ParentIndex2.RefID);
                }
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
                isGeomUsed.Add(entry.Key, false);

            foreach (KeyValuePair<int, FrameMaterial> entry in frameMaterials)
                isMatUsed.Add(entry.Key, false);

            foreach (KeyValuePair<int, FrameBlendInfo> entry in frameBlendInfos)
                isBlendInfoUsed.Add(entry.Key, false);

            foreach (KeyValuePair<int, FrameSkeleton> entry in frameSkeletons)
                isSkelUsed.Add(entry.Key, false);

            foreach (KeyValuePair<int, FrameSkeletonHierachy> entry in frameSkeletonHierachies)
                isSkelHierUsed.Add(entry.Key, false);

            foreach (KeyValuePair<int, object> entry in frameObjects)
            {
                if (entry.Value is FrameObjectModel)
                {
                    FrameObjectModel mesh = (entry.Value as FrameObjectModel);
                    isGeomUsed[mesh.Refs[FrameEntry.MeshRef]] = true;
                    isMatUsed[mesh.Refs[FrameEntry.MaterialRef]] = true;
                    isBlendInfoUsed[mesh.Refs[FrameEntry.BlendInfoRef]] = true;
                    isSkelHierUsed[mesh.Refs[FrameEntry.SkeletonHierRef]] = true;
                    isSkelUsed[mesh.Refs[FrameEntry.SkeletonRef]] = true;
                }
                else if (entry.Value is FrameObjectSingleMesh)
                {
                    FrameObjectSingleMesh mesh = (entry.Value as FrameObjectSingleMesh);

                    if (mesh.MeshIndex > -1)
                        isGeomUsed[mesh.Refs[FrameEntry.MeshRef]] = true;

                    if (mesh.MaterialIndex > -1)
                        isMatUsed[mesh.Refs[FrameEntry.MaterialRef]] = true;
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

            header.NumFolderNames = frameScenes.Count;
            header.NumGeometries = frameGeometries.Count;
            header.NumMaterialResources = frameMaterials.Count;
            header.NumBlendInfos = frameBlendInfos.Count;
            header.NumSkeletons = frameSkeletons.Count;
            header.NumSkelHierachies = frameSkeletonHierachies.Count;
            header.NumObjects = frameObjects.Count;
            header.NumFolderNames = header.SceneFolders.Count;
            NewFrames = updatedFrames;
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
