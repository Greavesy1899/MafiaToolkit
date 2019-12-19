using System.ComponentModel;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
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

        public FrameBlendInfo(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            int numBones = reader.ReadInt32(isBigEndian);
            byte numLods = reader.ReadByte8();

            //index infos
            boneIndexInfos = new BoneIndexInfo[numLods];
            for (int i = 0; i != boneIndexInfos.Length; i++)
            {
                boneIndexInfos[i].NumIDs = reader.ReadInt32(isBigEndian);
                boneIndexInfos[i].NumMaterials = reader.ReadInt32(isBigEndian);
            }

            //bounds for all bones together?
            bounds = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);

            //Bone Transforms
            boneTransforms = new BoneTransform[numBones];
            for (int i = 0; i != boneTransforms.Length; i++)
            {
                boneTransforms[i] = new BoneTransform();
                boneTransforms[i].ReadFromFile(reader, isBigEndian);
            }

            for (int i = 0; i != boneIndexInfos.Length; i++)
            {
                boneIndexInfos[i].BonesPerPool = reader.ReadBytes(4);

                //IDs..
                boneIndexInfos[i].IDs = reader.ReadBytes(boneIndexInfos[i].NumIDs);
                boneIndexInfos[i].Unk01 = reader.ReadInt32(isBigEndian);

                //Material blendings..
                boneIndexInfos[i].MatBlends = new ushort[boneIndexInfos[i].NumMaterials];
                for (int x = 0; x != boneIndexInfos[i].NumMaterials; x++)
                    boneIndexInfos[i].MatBlends[x] = reader.ReadUInt16(isBigEndian);
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
                writer.Write(boneIndexInfos[i].Unk01);

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
            int unk01;
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
            public int Unk01 {
                get { return unk01; }
                set { unk01 = value; }
            }
            public ushort[] MatBlends {
                get { return matBlends; }
                set { matBlends = value; }
            }
        }

        public struct BoneTransform
        {
            Matrix transform;
            BoundingBox bounds;
            byte isValid;

            public Matrix Transform {
                get { return transform; }
                set { transform = value; }
            }
            public BoundingBox Bounds {
                get { return bounds; }
                set { bounds = value; }
            }
            public byte IsValid {
                get { return isValid; }
                set { isValid = value; }
            }

            public void ReadFromFile(MemoryStream stream, bool isBigEndian)
            {
                transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
                bounds = BoundingBoxExtenders.ReadFromFile(stream, isBigEndian);
                isValid = stream.ReadByte8();
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
