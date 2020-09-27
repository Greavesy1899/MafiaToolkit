using System;
using System.Diagnostics;
using System.IO;
using Utils.StringHelpers;

namespace Mafia2Tool.MafiaLib.FileTypes.M3.XBin.Types
{
    public class StringTableItem
    {
        public uint Unk0 { get; set; } // I'm guessing this used to be a pointer to the stringID.
        public ulong Hash { get; set; } // Hash of the StringID which no longer exists. *cry ever tim*
        public uint ContentPtr { get; set; }
        public string Content { get; set; } // Taken from ContentPtr.

        public override string ToString()
        {
            return Content;
        }
    }

    public class StringTable
    {
        private StringTableItem[] Items;

        public StringTable()
        {
            Items = new StringTableItem[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            // XBin files store the count twice.
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            Debug.Assert(count0 == count1, "These numbers should be identical.");

            Items = new StringTableItem[count0];

            for (int i = 0; i < count0; i++)
            {
                StringTableItem Entry = new StringTableItem();
                Entry.Unk0 = reader.ReadUInt32();
                Entry.Hash = reader.ReadUInt64();
                Entry.ContentPtr = reader.ReadUInt32();
                Entry.Content = ReadStringFromBuffer(reader, Entry.ContentPtr);
                Items[i] = Entry;
            }

            DumpStringData();
        }

        private void DumpStringData()
        {
            using (StringWriter writer = new StringWriter())
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    string text = string.Format("{2} {0:X8} = {1}", Items[i].Hash, Items[i].Content, i);
                    writer.WriteLine(text);
                }
                File.WriteAllText("StringTable_EN.txt", writer.ToString());
            }
        }

        private string ReadStringFromBuffer(BinaryReader reader, uint offset)
        {
            long currentPosition = reader.BaseStream.Position;
            reader.BaseStream.Seek((currentPosition-4) + offset, SeekOrigin.Begin);
            var data = StringHelpers.ReadString(reader);
            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            return data;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Items.Length);
            writer.Write(Items.Length);

            // Idea is to write all entries and come back to replace Ptr(offset).
            long[] positions = new long[Items.Length];
            for(int i = 0; i < Items.Length; i++)
            {
                StringTableItem Entry = Items[i];
                writer.Write(Entry.Unk0);
                writer.Write(Entry.Hash);

                positions[i] = writer.BaseStream.Position;
                writer.Write(-1);
            }
            
            // Seems to be padding. Concerning..
            writer.Write(0);

            for(int i = 0; i < Items.Length; i++)
            {
                StringTableItem Entry = Items[i];

                // We get the position
                uint thisPosition = (uint)(writer.BaseStream.Position);
                StringHelpers.WriteString(writer, Entry.Content);
                long currentPosition = writer.BaseStream.Position;

                // Correct the offset and write to the file
                writer.BaseStream.Position = positions[i];
                var offset = (uint)(thisPosition - positions[i]);
                writer.Write(offset);
                writer.BaseStream.Position = currentPosition;
            }

            positions = new long[0];
        }
    }
}
