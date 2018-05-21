using System.IO;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Mafia2 {
    public class FrameResource {

        FrameHeader header;
        ArrayList frameBlocks;

        List<int> objectTypeList = new List<int>();

        public FrameHeader Header {
            get { return header; }
            set { header = value; }
        }
        public ArrayList FrameBlocks {
            get { return frameBlocks; }
            set { frameBlocks = value; }
        }

        public void ReadFromFile(BinaryReader reader) {
            frameBlocks = new ArrayList();

            //get header information
            header = new FrameHeader();
            header.ReadFromFile(reader);

            foreach(FrameHeaderScene scene in header.SceneFolders) {
                frameBlocks.Add(scene);
            }

            //get geometry information
            for (int i = 0; i < header.NumGeometries; i++) {
                frameBlocks.Add(new FrameGeometry(reader));
            }

            //get material information
            for (int i = 0; i < header.NumMaterialResources; i++) {
                frameBlocks.Add(new FrameMaterial(reader));
            }

            //get object information
            if(header.NumObjects > 0) {

                for (int i = 0; i != header.NumObjects; i++) {
                    objectTypeList.Add(reader.ReadInt32());
                }

                for(int i = 0; i != header.NumObjects; i++) {
                    FrameObjectBase newObject = new FrameObjectBase();
                    if (objectTypeList[i] == (int)ObjectType.Joint)
                        newObject = new FrameObjectJoint(reader);

                    else if (objectTypeList[i] == (int)ObjectType.SingleMesh) 
                        newObject = new FrameObjectSingleMesh(reader);

                    else if (objectTypeList[i] == (int)ObjectType.Frame)
                        newObject = new FrameObjectFrame(reader);

                    else if (objectTypeList[i] == (int)ObjectType.Light)
                        throw new Exception("Not Implemented");

                    else if (objectTypeList[i] == (int)ObjectType.Camera)
                        throw new Exception("Not Implemented");

                    else if (objectTypeList[i] == (int)ObjectType.Component_U00000005)
                        throw new Exception("Not Implemented");

                    else if (objectTypeList[i] == (int)ObjectType.Sector)
                        newObject = new FrameObjectSector(reader);

                    else if (objectTypeList[i] == (int)ObjectType.Dummy)
                        newObject = new FrameObjectDummy(reader);

                    else if (objectTypeList[i] == (int)ObjectType.ParticleDeflector)
                        throw new Exception("Not Implemented");

                    else if (objectTypeList[i] == (int)ObjectType.Area)
                        throw new Exception("Not Implemented");

                    else if (objectTypeList[i] == (int)ObjectType.Target)
                        throw new Exception("Not Implemented");

                    else if (objectTypeList[i] == (int)ObjectType.Model)
                        throw new Exception("Not Implemented");

                    else if (objectTypeList[i] == (int)ObjectType.Collision)
                        newObject = new FrameObjectCollision(reader);

                    frameBlocks.Add(newObject);
                }
            }

        }

        public void DefineFrameBlockParents() {
            for(int i = 0; i != frameBlocks.Count; i++) {

                FrameObjectBase newObject = frameBlocks[i] as FrameObjectBase;

                if (newObject == null)
                    continue;

                if(newObject.ParentIndex1.Index > -1)
                    newObject.ParentIndex1.Name = (frameBlocks[newObject.ParentIndex1.Index] as FrameObjectBase).Name.Name;
                if (newObject.ParentIndex2.Index > -1) {
                    if (frameBlocks[newObject.ParentIndex2.Index].GetType() == typeof(FrameHeaderScene))
                        newObject.ParentIndex2.Name = (frameBlocks[newObject.ParentIndex2.Index] as FrameHeaderScene).Name.Name;
                    else
                        newObject.ParentIndex2.Name = (frameBlocks[newObject.ParentIndex2.Index] as FrameObjectBase).Name.Name;
                }

                frameBlocks[i] = newObject;
            }
        }
    }
}
