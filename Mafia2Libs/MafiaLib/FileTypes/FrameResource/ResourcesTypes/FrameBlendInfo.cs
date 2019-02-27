﻿using System.IO;
using Mafia2;
using SharpDX;

namespace ResourceTypes.FrameResource
{
    public class FrameBlendInfo : FrameEntry
    {
        BoneIndexInfo[] boneIndexInfos;
        BoundingBox bounds;
        BoneTransform[] boneTransforms;

        public BoneIndexInfo[] BoneIndexInfos {
            get { return boneIndexInfos; }
            set { boneIndexInfos = value; }
        }
        public BoneTransform[] BoneTransforms {
            get { return boneTransforms; }
            set { boneTransforms = value; }
        }
        public BoundingBox Bound {
            get { return bounds; }
            set { bounds = value; }
        }

        public FrameBlendInfo(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int numBones = reader.ReadInt32();
            byte numLods = reader.ReadByte();

            //index infos
            boneIndexInfos = new BoneIndexInfo[numLods];
            for (int i = 0; i != boneIndexInfos.Length; i++)
            {
                boneIndexInfos[i].NumIDs = reader.ReadInt32();
                boneIndexInfos[i].NumMaterials = reader.ReadInt32();
            }

            //bounds for all bones together?
            bounds = BoundingBoxExtenders.ReadFromFile(reader);

            //Bone Transforms
            boneTransforms = new BoneTransform[numBones];
            for (int i = 0; i != boneTransforms.Length; i++)
            {
                boneTransforms[i] = new BoneTransform();
                boneTransforms[i].ReadFromFile(reader);
            }

            for (int i = 0; i != boneIndexInfos.Length; i++)
            {
                boneIndexInfos[i].BonesPerPool = reader.ReadBytes(4);

                //IDs..
                boneIndexInfos[i].IDs = reader.ReadBytes(boneIndexInfos[i].NumIDs);
                reader.ReadInt32(); //zero;
                //Material blendings..
                boneIndexInfos[i].MatBlends = new ushort[boneIndexInfos[i].NumMaterials];
                for (int x = 0; x != boneIndexInfos[i].NumMaterials; x++)
                    boneIndexInfos[i].MatBlends[x] = reader.ReadUInt16();
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(boneTransforms.Length);
            writer.Write((byte)boneIndexInfos.Length);

            //index infos
            for (int i = 0; i != boneIndexInfos.Length; i++)
            {
                writer.Write(boneIndexInfos[i].NumIDs);
                writer.Write(boneIndexInfos[i].NumMaterials);
            }

            //bounds for all bones together?
            bounds.WriteToFile(writer);

            //Bone Transforms
            for (int i = 0; i != boneTransforms.Length; i++)
                boneTransforms[i].WriteToFile(writer);

            for (int i = 0; i != boneIndexInfos.Length; i++)
            {
                writer.Write(boneIndexInfos[i].BonesPerPool);

                //IDs..
                writer.Write(boneIndexInfos[i].IDs);
                writer.Write(0);

                //Material blendings..
                for (int x = 0; x != boneIndexInfos[i].NumMaterials; x++)
                    writer.Write(boneIndexInfos[i].MatBlends[x]);
            }
        }

        public override string ToString()
        {
            return string.Format("Blend Info");
        }

        public struct BoneIndexInfo
        {
            int numIDs;
            int numMaterials;
            byte[] bonesPerPool;
            byte[] ids;
            ushort[] matBlends;

            public int NumIDs {
                get { return numIDs; }
                set { numIDs = value; }
            }
            public int NumMaterials {
                get { return numMaterials; }
                set { numMaterials = value; }
            }
            public byte[] BonesPerPool {
                get { return bonesPerPool; }
                set { bonesPerPool = value; }
            }
            public byte[] IDs {
                get { return ids; }
                set { ids = value; }
            }
            public ushort[] MatBlends {
                get { return matBlends; }
                set { matBlends = value; }
            }
        }

        public struct BoneTransform
        {
            TransformMatrix transform;
            BoundingBox bounds;
            bool isValid;

            public TransformMatrix Transform {
                get { return transform; }
                set { transform = value; }
            }
            public BoundingBox Bounds {
                get { return bounds; }
                set { bounds = value; }
            }
            public bool IsValid {
                get { return isValid; }
                set { isValid = value; }
            }

            public void ReadFromFile(BinaryReader reader)
            {
                transform = new TransformMatrix(reader);
                bounds = BoundingBoxExtenders.ReadFromFile(reader);
                isValid = reader.ReadBoolean();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                transform.WriteToFile(writer);
                bounds.WriteToFile(writer);
                writer.Write(IsValid);
            }
        }
    }
}