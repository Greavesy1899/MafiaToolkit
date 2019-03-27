using System;
using System.IO;
using Mafia2;
using SharpDX;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.FaceFX
{
    public class FXAnimSetLoader
    {
        int numAnimSets; //num of first set of data.
        FxAnim[] animSets;

        public FXAnimSetLoader(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public FXAnimSetLoader(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
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
            //byte[] data;
            int animCount; //number of animations.
            string fxAnimName;
            string pedName;
            string[] animNames;
            int unkCount;
            long[] unkNums;
            int unkCount2;
            UnkStruct[] unkData;
            int unkCount3;
            int[] unkNums2;
            Vector3 unkVector1;

            public FxAnim(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                size = reader.ReadInt32();
                //data = reader.ReadBytes(size);

                if (reader.ReadInt32() != 1162035526) //FACE
                    throw new System.Exception("Unexpected value");

                if (reader.ReadInt64() != 1730) //64bits after face.
                    throw new System.Exception("Unexpected value");

                if (StringHelpers.ReadString32(reader) != "IllusionSoftworks\0")
                    throw new System.Exception("Unexpected value");

                if (StringHelpers.ReadString32(reader) != "Mafia 2\0")
                    throw new System.Exception("Unexpected value");

                reader.ReadBytes(172); //seem to be exactly the same..

                //get to the real stuff.
                animCount = reader.ReadInt32();
                fxAnimName = StringHelpers.ReadString32(reader);
                pedName = StringHelpers.ReadString32(reader);

                //get them anim names.
                animNames = new string[animCount-2];
                for (int i = 0; i != animCount-2; i++)
                    animNames[i] = StringHelpers.ReadString32(reader);

                reader.ReadInt64();
                reader.ReadInt64();
                reader.ReadInt64();

                unkCount = reader.ReadInt32();
                unkNums = new long[unkCount];
                for (int i = 0; i != unkCount; i++)
                    unkNums[i] = reader.ReadInt64();

                unkCount2 = reader.ReadInt32();
                unkData = new UnkStruct[unkCount2];
                for (int i = 0; i != unkCount2; i++)
                    unkData[i].ReadFromFile(reader);

                unkCount3 = reader.ReadInt32();
                unkNums2 = new int[unkCount3];
                for (int i = 0; i != unkCount3; i++)
                    unkNums2[i] = reader.ReadInt32();

                unkVector1 = Vector3Extenders.ReadFromFile(reader);
                reader.ReadInt64();
                if (reader.ReadInt32() == -1)
                    Console.WriteLine("Reached 0xFFFF");
            }

            public struct UnkStruct
            {
                private Vector3 vector;
                private int unk1;

                public void ReadFromFile(BinaryReader reader)
                {
                    vector = Vector3Extenders.ReadFromFile(reader);
                    unk1 = reader.ReadInt32();
                }
            }

            public override string ToString()
            {
                return fxAnimName;
            }
        }
    }
}
