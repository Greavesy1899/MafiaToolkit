using System.Collections.Generic;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.CGame
{
    public class Type5Chunk : IGameChunk
    {
        public Type5Data[] Strings { get; set; } = new Type5Data[0];
        public Type5Chunk()
        {

        }

        public Type5Chunk(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            List<Type5Data> datas = new();

            using (MemoryStream ms = new(br.ReadBytes(br.ReadInt32())))
            {
                while (ms.Position != ms.Length)
                {
                    datas.Add(new(ms));
                }
            }

            Strings = datas.ToArray();
        }

        public void Write(BinaryWriter bw)
        {
            using (MemoryStream ms = new())
            {
                foreach (var slot in Strings)
                {
                    slot.Write(ms);
                }

                bw.Write((int)ms.Length);
                bw.Write(ms.ToArray());
            }
        }

        public int GetChunkType()
        {
            return 5 | 0xE000;
        }
    }

    public class Type5Data
    {
        public string Value { get; set; } = "";
        public Type5Data()
        {

        }

        public Type5Data(MemoryStream ms)
        {
            Read(ms);
        }

        public void Read(MemoryStream ms)
        {
            Value = ms.ReadString();
        }

        public void Write(MemoryStream ms)
        {
            ms.WriteString(Value);
        }
    }
}
