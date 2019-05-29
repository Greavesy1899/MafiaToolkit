using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        Hash sceneName;
        List<FrameHeaderScene> sceneFolders;

        float unk1;
        float unk2;
        bool unk3;
        float[] unkData = new float[4 * 3];

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
        public Hash SceneName {
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

        public FrameHeader()
        {
            sceneFolders = new List<FrameHeaderScene>();
        }

        /// <summary>
        /// Read data from file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            isScene = reader.ReadBoolean();
            numFolderNames = reader.ReadInt32();
            numGeometries = reader.ReadInt32();
            numMaterialResources = reader.ReadInt32();
            numBlendInfos = reader.ReadInt32();
            numSkeletons = reader.ReadInt32();
            numSkelHierachies = reader.ReadInt32();
            numObjects = reader.ReadInt32();

            if (isScene)
            {
                unk1 = reader.ReadSingle();
                unk2 = reader.ReadSingle();
                sceneName = new Hash(reader);

                for (int i = 0; i < (4 * 3); i++)
                {
                    unkData[i] = reader.ReadSingle();
                }
                unk3 = reader.ReadBoolean();
            }
        
            for (int i = 0; i != numFolderNames; i++)
            {
                FrameHeaderScene scene = new FrameHeaderScene();
                scene.ReadFromFile(reader);
                sceneFolders.Add(scene);
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
