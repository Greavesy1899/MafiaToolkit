using System.IO;

namespace Mafia2
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
        FrameHeaderScene[] sceneFolders;

        //unknown
        public float unk1 = 0f;
        public float unk2 = 0f;
        public bool unk3;
        public float[] unkData = new float[4 * 3];

        public bool IsScene {
            get { return isScene; }
            set { isScene = value; }
        }
        public int NumFolderNames {
            get { return numFolderNames; }
            set { numFolderNames = value; }
        }
        public int NumGeometries {
            get { return numGeometries; }
            set { numGeometries = value; }
        }
        public int NumMaterialResources {
            get { return numMaterialResources; }
            set { numMaterialResources = value; }
        }
        public int NumObjects {
            get { return numObjects; }
            set { numObjects = value; }
        }
        public int NumBlendInfos {
            get { return numBlendInfos; }
            set { numBlendInfos = value; }
        }
        public int NumSkeletons {
            get { return numSkeletons; }
            set { numSkeletons = value; }
        }
        public int NumSkelHierachies {
            get { return numSkelHierachies; }
            set { numSkelHierachies = value; }
        }
        public FrameHeaderScene[] SceneFolders {
            get { return sceneFolders; }
            set { sceneFolders = value; }
        }

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

            sceneFolders = new FrameHeaderScene[numFolderNames];
            for (int i = 0; i != numFolderNames; i++)
            {
                sceneFolders[i] = new FrameHeaderScene();
                sceneFolders[i].ReadFromFile(reader);
            }
        }

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
