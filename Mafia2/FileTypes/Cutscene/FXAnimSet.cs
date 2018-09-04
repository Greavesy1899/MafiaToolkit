using System.IO;

namespace Mafia2
{
    public class FXAnimSet
    {
        int numAnimSets; //num of first set of data.
        FxAnim[] animSets;

        public FXAnimSet(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public FXAnimSet(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                Speech speech = new Speech(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            numAnimSets = reader.ReadInt32();
            animSets = new FxAnim[numAnimSets];

            for(int i = 0; i != animSets.Length; i++)
                animSets[i] = new FxAnim(reader);
        }

        public class FxAnim
        {
            int size;
            byte[] data;

            public FxAnim(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                size = reader.ReadInt32();
                data = reader.ReadBytes(size);
            }
        }
    }
}
