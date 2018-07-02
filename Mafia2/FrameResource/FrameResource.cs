using System.IO;
using System;

namespace Mafia2
{
    public class FrameResource
    {

        FrameHeader header;
        object[] frameBlocks;
        object[] frameObjects;
        object[] entireFrame; //very bad

        int[] objectTypes;

        public FrameHeader Header {
            get { return header; }
            set { header = value; }
        }
        public object[] FrameBlocks {
            get { return frameBlocks; }
            set { frameBlocks = value; }
        }
        public object[] FrameObjects {
            get { return frameObjects; }
            set { frameObjects = value; }
        }
        public object[] EntireFrame {
            get { return entireFrame; }
        }

        public FrameResource(string file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
            using (BinaryWriter writer = new BinaryWriter(File.Open("CITYAREAS.ara", FileMode.Create)))
            {
                int areaCount = 0;
                for(int i = 0; i != frameObjects.Length; i++)
                {
                    if (frameObjects[i].GetType() == typeof(FrameObjectArea))
                        areaCount++;
                }
                writer.Write(areaCount);
                for (int i = 0; i != frameObjects.Length; i++)
                {
                    if (frameObjects[i].GetType() == typeof(FrameObjectArea))
                        (frameObjects[i] as FrameObjectArea).WriteARAFile(writer);
                }
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            header = new FrameHeader();
            header.ReadFromFile(reader);

            objectTypes = new int[header.NumObjects];
            frameBlocks = new object[header.SceneFolders.Length + header.NumGeometries + header.NumMaterialResources + header.NumBlendInfos + header.NumSkeletons + header.NumSkelHierachies];
            frameObjects = new object[header.NumObjects];

            int j = 0;

            for (int i = 0; i != header.SceneFolders.Length; i++)
            { frameBlocks[j] = header.SceneFolders[i]; j++; }

            for (int i = 0; i != header.NumGeometries; i++)
            { frameBlocks[j] = new FrameGeometry(reader); j++; }

            for (int i = 0; i != header.NumMaterialResources; i++)
            { frameBlocks[j] = new FrameMaterial(reader); j++; }

            for (int i = 0; i != header.NumBlendInfos; i++)
            { frameBlocks[j] = new FrameBlendInfo(reader); j++; }

            for (int i = 0; i != header.NumSkeletons; i++)
            { frameBlocks[j] = new FrameSkeleton(reader); j++; }

            for (int i = 0; i != header.NumSkelHierachies; i++)
            { frameBlocks[j] = new FrameSkeletonHierachy(reader); j++; }
            
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
                        newObject = new FrameObjectSingleMesh(reader);

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
                        throw new Exception("Not Implemented");

                    else if (objectTypes[i] == (int)ObjectType.Model)
                        newObject = new FrameObjectModel(reader, frameBlocks);

                    else if (objectTypes[i] == (int)ObjectType.Collision)
                        newObject = new FrameObjectCollision(reader);

                    frameObjects[i] = newObject;
                }
            }
            DefineFrameBlockParents();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            header.WriteToFile(writer);

            for(int i = 0; i != frameBlocks.Length; i++)
            {
                if(frameBlocks[i].GetType() == typeof(FrameGeometry))
                    (frameBlocks[i] as FrameGeometry).WriteToFile(writer);

                if (frameBlocks[i].GetType() == typeof(FrameMaterial))
                    (frameBlocks[i] as FrameMaterial).WriteToFile(writer);

                if (frameBlocks[i].GetType() == typeof(FrameBlendInfo))
                    throw new NotImplementedException("Not implemented");

                if (frameBlocks[i].GetType() == typeof(FrameSkeleton))
                    throw new NotImplementedException("Not implemented");

                if (frameBlocks[i].GetType() == typeof(FrameSkeletonHierachy))
                    throw new NotImplementedException("Not implemented");
            }

            for(int i = 0; i != frameObjects.Length; i++)
            {
                if (frameObjects[i].GetType() == typeof(FrameObjectJoint))
                    writer.Write((int)ObjectType.Joint);

                if (frameObjects[i].GetType() == typeof(FrameObjectSingleMesh))
                    writer.Write((int)ObjectType.SingleMesh);

                if (frameObjects[i].GetType() == typeof(FrameObjectFrame))
                    writer.Write((int)ObjectType.Frame);

                if (frameObjects[i].GetType() == typeof(FrameObjectLight))
                    writer.Write((int)ObjectType.Light);

                if (frameObjects[i].GetType() == typeof(FrameObjectCamera))
                    writer.Write((int)ObjectType.Camera);

                if (frameObjects[i].GetType() == typeof(FrameObjectComponent_U005))
                    writer.Write((int)ObjectType.Component_U00000005);

                if (frameObjects[i].GetType() == typeof(FrameObjectSector))
                    writer.Write((int)ObjectType.Sector);

                if (frameObjects[i].GetType() == typeof(FrameObjectDummy))
                    writer.Write((int)ObjectType.Dummy);

                if (frameObjects[i].GetType() == typeof(FrameObjectDeflector))
                    writer.Write((int)ObjectType.ParticleDeflector);

                if (frameObjects[i].GetType() == typeof(FrameObjectArea))
                    writer.Write((int)ObjectType.Area);

                if (frameObjects[i].GetType() == typeof(string))
                    writer.Write((int)ObjectType.Target);

                if (frameObjects[i].GetType() == typeof(FrameObjectModel))
                    writer.Write((int)ObjectType.Model);

                if (frameObjects[i].GetType() == typeof(FrameObjectCollision))
                    writer.Write((int)ObjectType.Collision);
            }

            for (int i = 0; i != frameObjects.Length; i++)
            {
                if (frameObjects[i].GetType() == typeof(FrameObjectJoint))
                    (frameObjects[i] as FrameObjectJoint).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectSingleMesh))
                    (frameObjects[i] as FrameObjectSingleMesh).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectFrame))
                    (frameObjects[i] as FrameObjectFrame).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectLight))
                    (frameObjects[i] as FrameObjectLight).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectCamera))
                    (frameObjects[i] as FrameObjectCamera).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectComponent_U005))
                    (frameObjects[i] as FrameObjectComponent_U005).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectSector))
                    (frameObjects[i] as FrameObjectSector).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectDummy))
                    (frameObjects[i] as FrameObjectDummy).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectDeflector))
                    (frameObjects[i] as FrameObjectDeflector).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectArea))
                    (frameObjects[i] as FrameObjectArea).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(string))
                    writer.Write((int)ObjectType.Target);

                if (frameObjects[i].GetType() == typeof(FrameObjectModel))
                    (frameObjects[i] as FrameObjectModel).WriteToFile(writer);

                if (frameObjects[i].GetType() == typeof(FrameObjectCollision))
                    (frameObjects[i] as FrameObjectCollision).WriteToFile(writer);
            }
        }

        public void DefineFrameBlockParents()
        {
            UpdateEntireFrame();

            for (int i = 0; i != frameObjects.Length; i++)
            {

                FrameObjectBase newObject = frameObjects[i] as FrameObjectBase;

                if (newObject == null)
                    continue;

                if(newObject.ParentIndex1.Index > -1)
                    newObject.ParentIndex1.Name = (entireFrame[newObject.ParentIndex1.Index] as FrameObjectBase).Name.String;

                if (newObject.ParentIndex2.Index > -1)
                {
                    if (entireFrame[newObject.ParentIndex2.Index].GetType() == typeof(FrameHeaderScene))
                        newObject.ParentIndex2.Name = (entireFrame[newObject.ParentIndex2.Index] as FrameHeaderScene).Name.String;
                    else
                        newObject.ParentIndex2.Name = (entireFrame[newObject.ParentIndex2.Index] as FrameObjectBase).Name.String;
                }

                frameObjects[i] = newObject;
            }
        }

        public void UpdateEntireFrame()
        {
            entireFrame = new object[frameBlocks.Length + frameObjects.Length];
            Array.Copy(frameBlocks, entireFrame, frameBlocks.Length);
            Array.Copy(frameObjects, 0, entireFrame, frameBlocks.Length, frameObjects.Length);
        }
    }
}
