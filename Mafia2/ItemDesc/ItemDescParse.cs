using System;
using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class ItemDesc
    {
        public ulong frameRef; //links into FrameResources. Only checked collisions.
        public byte unk_byte;
        public byte unk_byte2; //possible type; 7 makes file size increase.
        public ulong unk_hash2;
        public TransformMatrix unk_matrix;

        public override string ToString()
        {
            return string.Format("{0}, {1}", frameRef, unk_hash2);
        }
    }
    public class ItemDescParse
    {
        List<ItemDesc> itemDescList = new List<ItemDesc>();

        public ItemDescParse()
        {
            Parse();
        }

        public void Parse()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

            FileInfo[] files = dirInfo.GetFiles();

            List<string> ItemDesc = new List<string>();

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains("ItemDesc_"))
                    ParseItemDesc(file.Name);
            }
        }

        private void ParseItemDesc(string name)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open)))
            {
                ItemDesc item = new ItemDesc();

                item.frameRef = reader.ReadUInt64();
                item.unk_byte = reader.ReadByte();
                item.unk_byte2 = reader.ReadByte();
                item.unk_hash2 = reader.ReadUInt64();
                item.unk_matrix = new TransformMatrix(reader);
                itemDescList.Add(item);
            }
        }
    }
}
