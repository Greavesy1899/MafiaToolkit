using System.IO;

namespace Mafia2 {
    public class FrameHeader {

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
        float num1 = 0f;
        float num2 = 0f;
        float num3 = 0;

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

        public void ReadFromFile(BinaryReader reader) {
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
                reader.ReadSingle();
                reader.ReadSingle();
                sceneName = new Hash(reader);

                for (int i = 0; i < 4; i++)
                {
                    num1 = reader.ReadInt32();
                    num2 = reader.ReadInt32();
                    num3 = reader.ReadInt32();
                }
                reader.ReadBoolean();
            }

            sceneFolders = new FrameHeaderScene[numFolderNames];
            for (int i = 0; i != numFolderNames; i++) {
                sceneFolders[i] = new FrameHeaderScene();
                sceneFolders[i].ReadFromFile(reader);
            }
        }

        public override string ToString() {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", isScene, numFolderNames, numGeometries, numMaterialResources, numBlendInfos, numSkeletons, numSkelHierachies, numObjects);
        }
    }
}
