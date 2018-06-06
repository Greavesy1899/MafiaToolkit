using System.IO;
using System;

namespace Mafia2
{
    public class FrameResource
    {

        FrameHeader header;
        object[] frameBlocks;
        object[] frameObjects;

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

        public FrameResource(string file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                ReadFromFile(reader);
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

        public void DefineFrameBlockParents()
        {
            for (int i = 0; i != frameObjects.Length; i++)
            {

                FrameObjectBase newObject = frameObjects[i] as FrameObjectBase;

                if (newObject == null)
                    continue;

                int index1 = -1;
                int index2 = -1;

                if(newObject.ParentIndex1.Index != -1)
                    index1 = newObject.ParentIndex1.Index - frameBlocks.Length;
                if (newObject.ParentIndex2.Index != -1)
                    index2 = newObject.ParentIndex2.Index - frameBlocks.Length;

                if(index1 != -1)
                    newObject.ParentIndex1.Name = (frameObjects[index1] as FrameObjectBase).Name.Name;

                if (index2 != -1)
                {
                    if (frameObjects[index2].GetType() == typeof(FrameHeaderScene))
                        newObject.ParentIndex2.Name = (frameObjects[index2] as FrameHeaderScene).Name.Name;
                    else
                        newObject.ParentIndex2.Name = (frameObjects[index2] as FrameObjectBase).Name.Name;
                }

                frameObjects[i] = newObject;
            }
        }
    }
}
