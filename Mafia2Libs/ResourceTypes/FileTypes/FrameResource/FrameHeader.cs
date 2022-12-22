using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameHeader
    {

        bool isScene = false;
        int numFolderNames = 0;
        int numGeometries = 0;
        int numMaterialResources = 0;
        int numBlendInfos = 0;
        int numSkeletons = 0;
        int numSkelHierachies = 0;
        int numObjects = 0;

        HashName sceneName;
        List<FrameHeaderScene> sceneFolders;

        float unk1;
        float unk2;
        bool unk3;
        float[] unkData = new float[4 * 3];

        private FrameResource OwningResource;

        public bool IsScene {
            get { return isScene; }
            set { isScene = value; }
        }
        [Browsable(false)]
        public int NumFolderNames {
            get { return numFolderNames; }
            set { numFolderNames = value; }
        }
        [Browsable(false)]
        public int NumGeometries {
            get { return numGeometries; }
            set { numGeometries = value; }
        }
        [Browsable(false)]
        public int NumMaterialResources {
            get { return numMaterialResources; }
            set { numMaterialResources = value; }
        }
        [Browsable(false)]
        public int NumObjects {
            get { return numObjects; }
            set { numObjects = value; }
        }
        [Browsable(false)]
        public int NumBlendInfos {
            get { return numBlendInfos; }
            set { numBlendInfos = value; }
        }
        [Browsable(false)]
        public int NumSkeletons {
            get { return numSkeletons; }
            set { numSkeletons = value; }
        }
        [Browsable(false)]
        public int NumSkelHierachies {
            get { return numSkelHierachies; }
            set { numSkelHierachies = value; }
        }
        public HashName SceneName {
            get { return sceneName; }
            set { sceneName = value; }
        }
        public float Unk1 {
            get { return unk1; }
            set { unk1 = value; }
        }
        public float Unk2 {
            get { return unk2; }
            set { unk2 = value; }
        }
        public bool Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }
        public float[] UnkFloats {
            get { return unkData; }
            set { unkData = value; }
        }
        [Browsable(false)]
        public List<FrameHeaderScene> SceneFolders {
            get { return sceneFolders; }
            set { sceneFolders = value; }
        }

        public FrameHeader(FrameResource OwningResource)
        {
            sceneFolders = new List<FrameHeaderScene>();
            sceneName = new HashName();
            unkData = new float[4 * 3];

            this.OwningResource = OwningResource;
        }

        /// <summary>
        /// Read data from file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            isScene = reader.ReadBoolean();
            numFolderNames = reader.ReadInt32(isBigEndian);
            numGeometries = reader.ReadInt32(isBigEndian);
            numMaterialResources = reader.ReadInt32(isBigEndian);
            numBlendInfos = reader.ReadInt32(isBigEndian);
            numSkeletons = reader.ReadInt32(isBigEndian);
            numSkelHierachies = reader.ReadInt32(isBigEndian);
            numObjects = reader.ReadInt32(isBigEndian);

            if (isScene)
            {
                unk1 = reader.ReadSingle(isBigEndian);
                unk2 = reader.ReadSingle(isBigEndian);
                sceneName = new HashName(reader, isBigEndian);

                for (int i = 0; i < (4 * 3); i++)
                {
                    unkData[i] = reader.ReadSingle(isBigEndian);
                }
                unk3 = reader.ReadBoolean();
            }
        
            for (int i = 0; i != numFolderNames; i++)
            {
                FrameHeaderScene scene = OwningResource.ConstructFrameAssetOfType<FrameHeaderScene>();
                scene.ReadFromFile(reader, isBigEndian);
            }
        }

        /// <summary>
        /// Write data to file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(isScene);
            writer.Write(numFolderNames);
            writer.Write(numGeometries);
            writer.Write(numMaterialResources);
            writer.Write(numBlendInfos);
            writer.Write(numSkeletons);
            writer.Write(numSkelHierachies);
            writer.Write(numObjects);

            if(isScene)
            {
                writer.Write(unk1);
                writer.Write(unk2);
                sceneName.WriteToFile(writer);

                for (int i = 0; i < (4 * 3); i++)
                {
                    writer.Write(unkData[i]);
                }
                writer.Write(unk3);
            }

            for (int i = 0; i != numFolderNames; i++)
            {
                sceneFolders[i].WriteToFile(writer);
            }

        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", isScene, numFolderNames, numGeometries, numMaterialResources, numBlendInfos, numSkeletons, numSkelHierachies, numObjects);
        }
    }
}
