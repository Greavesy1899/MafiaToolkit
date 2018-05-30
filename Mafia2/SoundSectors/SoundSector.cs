using System.IO;

namespace Mafia2
{
    public class SoundSector
    {
        ulong[] unk_01_array;
        short[] unk_02_array;

        public SoundSector(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            unk_01_array = new ulong[length];

            for(int i = 0; i != unk_01_array.Length; i++)
            {
                unk_01_array[i] = reader.ReadUInt64();
            }

            int unk01 = reader.ReadInt32();
            byte unk02 = reader.ReadByte();
            short count2 = reader.ReadInt16();

            unk_02_array = new short[count2];

            for (int i = 0; i != unk_02_array.Length; i++)
            {
                unk_02_array[i] = reader.ReadInt16();
            }

            long unk03 = reader.ReadInt64();

            string word = reader.ReadString();

            reader.ReadBytes(2);

            long unk04 = reader.ReadInt64();
        }
    }
}
