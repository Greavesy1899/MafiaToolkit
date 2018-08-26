using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mafia2
{
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
        Dictionary<int, object> entireFrame = new Dictionary<int, object>();

        int[] frameBlocks;
        int[] objectTypes;

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

            objectTypes = new int[header.NumObjects];
            frameBlocks = new int[header.SceneFolders.Length + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies];

            int j = 0;

            for (int i = 0; i != header.SceneFolders.Length; i++)
            {
                frameScenes.Add(header.SceneFolders[i].RefID, header.SceneFolders[i]);
                frameBlocks[j++] = header.SceneFolders[i].RefID;
            }
            for (int i = 0; i != header.NumGeometries; i++)
            {
                FrameGeometry geo = new FrameGeometry(reader);
                frameGeometries.Add(geo.RefID, geo);
                frameBlocks[j++] = geo.RefID;
            }
            for (int i = 0; i != header.NumMaterialResources; i++)
            {
                FrameMaterial mat = new FrameMaterial(reader);
                frameMaterials.Add(mat.RefID, mat);
                frameBlocks[j++] = mat.RefID;
            }
            for (int i = 0; i != header.NumBlendInfos; i++)
            {
                FrameBlendInfo blendInfo = new FrameBlendInfo(reader);
                frameBlendInfos.Add(blendInfo.RefID, blendInfo);
                frameBlocks[j++] = blendInfo.RefID;
            }
            for (int i = 0; i != header.NumSkeletons; i++)
            {
                FrameSkeleton skeleton = new FrameSkeleton(reader);
                frameSkeletons.Add(skeleton.RefID, skeleton);
                frameBlocks[j++] = skeleton.RefID;
            }
            for (int i = 0; i != header.NumSkelHierachies; i++)
            {
                FrameSkeletonHierachy skeletonHierachy = new FrameSkeletonHierachy(reader);
                frameSkeletonHierachies.Add(skeletonHierachy.RefID, skeletonHierachy);
                frameBlocks[j++] = skeletonHierachy.RefID;
            }

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

                        if(mesh.MeshIndex != -1)
                            mesh.AddRef(FrameEntryRefTypes.Mesh, frameBlocks[mesh.MeshIndex]);

                        if(mesh.MaterialIndex != -1)
                            mesh.AddRef(FrameEntryRefTypes.Material, frameBlocks[mesh.MaterialIndex]);
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
                        mesh.ReadFromFilePart2(reader, frameSkeletons[frameBlocks[mesh.SkeletonIndex]], frameBlendInfos[frameBlocks[mesh.BlendInfoIndex]]);
                        mesh.AddRef(FrameEntryRefTypes.Mesh, frameBlocks[mesh.MeshIndex]);
                        mesh.AddRef(FrameEntryRefTypes.Material, frameBlocks[mesh.MaterialIndex]);
                        mesh.AddRef(FrameEntryRefTypes.BlendInfo, frameBlocks[mesh.BlendInfoIndex]);
                        mesh.AddRef(FrameEntryRefTypes.Skeleton, frameBlocks[mesh.SkeletonIndex]);
                        mesh.AddRef(FrameEntryRefTypes.SkeletonHierachy, frameBlocks[mesh.SkeletonHierachyIndex]);
                        newObject = mesh;
                    }
                    else if (objectTypes[i] == (int)ObjectType.Collision)
                        newObject = new FrameObjectCollision(reader);

                    frameObjects.Add(newObject.RefID, newObject);
                }
            }
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

            foreach (KeyValuePair<int, FrameGeometry> entry in frameGeometries)
            {
                entry.Value.WriteToFile(writer);
            }
            foreach (KeyValuePair<int, FrameMaterial> entry in frameMaterials)
            {
                entry.Value.WriteToFile(writer);
            }
            foreach (KeyValuePair<int, FrameBlendInfo> entry in frameBlendInfos)
            {
                entry.Value.WriteToFile(writer);
            }
            foreach (KeyValuePair<int, FrameSkeleton> entry in frameSkeletons)
            {
                entry.Value.WriteToFile(writer);
            }
            foreach (KeyValuePair<int, FrameSkeletonHierachy> entry in frameSkeletonHierachies)
            {
                entry.Value.WriteToFile(writer);
            }
            foreach(KeyValuePair<int, object> entry in frameObjects)
            {
                if (entry.Value.GetType() == typeof(FrameObjectJoint))
                    writer.Write((int)ObjectType.Joint);
                else if (entry.Value.GetType() == typeof(FrameObjectSingleMesh))
                    writer.Write((int)ObjectType.SingleMesh);
                else if (entry.Value.GetType() == typeof(FrameObjectFrame))
                    writer.Write((int)ObjectType.Frame);
                else if (entry.Value.GetType() == typeof(FrameObjectLight))
                    writer.Write((int)ObjectType.Light);
                else if (entry.Value.GetType() == typeof(FrameObjectCamera))
                    writer.Write((int)ObjectType.Camera);
                else if (entry.Value.GetType() == typeof(FrameObjectComponent_U005))
                    writer.Write((int)ObjectType.Component_U00000005);
                else if (entry.Value.GetType() == typeof(FrameObjectSector))
                    writer.Write((int)ObjectType.Sector);
                else if (entry.Value.GetType() == typeof(FrameObjectDummy))
                    writer.Write((int)ObjectType.Dummy);
                else if (entry.Value.GetType() == typeof(FrameObjectDeflector))
                    writer.Write((int)ObjectType.ParticleDeflector);
                else if (entry.Value.GetType() == typeof(FrameObjectArea))
                    writer.Write((int)ObjectType.Area);
                else if (entry.Value.GetType() == typeof(FrameObjectTarget))
                    writer.Write((int)ObjectType.Target);
                else if (entry.Value.GetType() == typeof(FrameObjectModel))
                    writer.Write((int)ObjectType.Model);
                else if (entry.Value.GetType() == typeof(FrameObjectCollision))
                    writer.Write((int)ObjectType.Collision);
            }
            foreach (KeyValuePair<int, object> entry in frameObjects)
            {
                if (entry.Value.GetType() == typeof(FrameObjectJoint))
                    (entry.Value as FrameObjectJoint).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectSingleMesh))
                    (entry.Value as FrameObjectSingleMesh).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectFrame))
                    (entry.Value as FrameObjectFrame).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectLight))
                    (entry.Value as FrameObjectLight).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectCamera))
                    (entry.Value as FrameObjectCamera).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectComponent_U005))
                    (entry.Value as FrameObjectComponent_U005).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectSector))
                    (entry.Value as FrameObjectSector).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectDummy))
                    (entry.Value as FrameObjectDummy).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectDeflector))
                    (entry.Value as FrameObjectDeflector).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectArea))
                    (entry.Value as FrameObjectArea).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectTarget))
                    (entry.Value as FrameObjectTarget).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectModel))
                    (entry.Value as FrameObjectModel).WriteToFile(writer);
                else if (entry.Value.GetType() == typeof(FrameObjectCollision))
                    (entry.Value as FrameObjectCollision).WriteToFile(writer);
            }
        }

        /// <summary>
        /// Adds names onto ParentIndex1 and ParentIndex2. Called after the file has been read.
        /// This won't be required later, so expect it to be removed.
        /// </summary>
        public void DefineFrameBlockParents()
        {
            int numBlocks = header.NumFolderNames + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies;

            for (int i = 0; i != frameObjects.Count; i++)
            {
                FrameObjectBase obj = (frameObjects.ElementAt(i).Value as FrameObjectBase);

                if (obj == null)
                    continue;


                if (obj.ParentIndex1.Index > -1)
                {
                    if (obj.ParentIndex1.Index > numBlocks)
                        obj.ParentIndex1.RefID = (frameObjects.ElementAt(obj.ParentIndex1.Index - numBlocks).Value as FrameObjectBase).RefID;
                    else
                        obj.ParentIndex1.RefID = (frameObjects.ElementAt(obj.ParentIndex1.Index).Value as FrameObjectBase).RefID;
                }

                if (obj.ParentIndex2.Index > -1)
                {
                    if (obj.ParentIndex2.Index > numBlocks)
                        obj.ParentIndex2.RefID = (frameObjects.ElementAt(obj.ParentIndex2.Index - numBlocks).Value as FrameObjectBase).RefID;
                    else
                        obj.ParentIndex2.RefID = (frameObjects.ElementAt(obj.ParentIndex2.Index).Value as FrameObjectBase).RefID;
                }
            }
        }

        /// <summary>
        /// This reconstructs the data.
        /// </summary>
        public void UpdateFrameData()
        {
            int totalResources = header.NumFolderNames + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies;

            Dictionary<int, object> newFrame = new Dictionary<int, object>();

            foreach (KeyValuePair<int, FrameHeaderScene> entry in frameScenes)
            {
                newFrame.Add(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<int, FrameGeometry> entry in frameGeometries)
            {
                newFrame.Add(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<int, FrameMaterial> entry in frameMaterials)
            {
                newFrame.Add(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<int, FrameBlendInfo> entry in frameBlendInfos)
            {
                newFrame.Add(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<int, FrameSkeleton> entry in frameSkeletons)
            {
                newFrame.Add(entry.Key, entry.Value);
            }
            foreach (KeyValuePair<int, FrameSkeletonHierachy> entry in frameSkeletonHierachies)
            {
                newFrame.Add(entry.Key, entry.Value);
            }
            for (int i = 0; i != frameObjects.Count; i++)
            {
                FrameObjectBase block = (frameObjects.ElementAt(i).Value as FrameObjectBase);

                if (block.GetType() == typeof(FrameObjectSingleMesh))
                {
                    FrameObjectSingleMesh mesh = (block as FrameObjectSingleMesh);
                    mesh.MaterialIndex = newFrame.IndexOfValue(frameBlocks[mesh.MaterialIndex]);
                    mesh.MeshIndex = newFrame.IndexOfValue(frameBlocks[mesh.MeshIndex]);
                    newFrame.Add(mesh.RefID, mesh);
                }
                else if (block.GetType() == typeof(FrameObjectModel))
                {
                    FrameObjectModel mesh = (block as FrameObjectModel);
                    mesh.MaterialIndex = newFrame.IndexOfValue(frameBlocks[mesh.MaterialIndex]);
                    mesh.MeshIndex = newFrame.IndexOfValue(frameBlocks[mesh.MeshIndex]);
                    mesh.BlendInfoIndex = newFrame.IndexOfValue(frameBlocks[mesh.BlendInfoIndex]);
                    mesh.SkeletonIndex = newFrame.IndexOfValue(frameBlocks[mesh.SkeletonIndex]);
                    mesh.SkeletonHierachyIndex = newFrame.IndexOfValue(frameBlocks[mesh.SkeletonHierachyIndex]);
                    newFrame.Add(mesh.RefID, mesh);
                }
                else
                {
                    newFrame.Add(block.RefID, block);
                }
            }

            header.NumFolderNames = FrameScenes.Count;
            header.NumGeometries = frameGeometries.Count;
            header.NumMaterialResources = frameMaterials.Count;
            header.NumBlendInfos = frameBlendInfos.Count;
            header.NumSkeletons = frameSkeletons.Count;
            header.NumSkelHierachies = frameSkeletonHierachies.Count;
            header.NumObjects = frameObjects.Count;
        }
    }
}
