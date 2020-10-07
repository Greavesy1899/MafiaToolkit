using ResourceTypes.FileTypes.M3.XBin;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public class LocalisableString
    {
        public uint Unk0 { get; set; } // I'm guessing this used to be a pointer to the stringID.
        public ulong Hash { get; set; } // Hash of the StringID which no longer exists. *cry ever tim*
        public string Content { get; set; } // Text

        public override string ToString()
        {
            return Content;
        }
    }

    public class StringTable : BaseTable
    {
        private readonly ulong XBinMagic = 0x5E42EF29E8A3E1D3;
        public LocalisableString[] Items { get; set; }

        public StringTable()
        {
            Items = new LocalisableString[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            // XBin files store the count twice.
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            Debug.Assert(count0 == count1, "These numbers should be identical.");

            Items = new LocalisableString[count0];

            for (int i = 0; i < count0; i++)
            {
                LocalisableString Entry = new LocalisableString();
                Entry.Unk0 = reader.ReadUInt32();
                Entry.Hash = reader.ReadUInt64();
                Entry.Content = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                Items[i] = Entry;
            }

            DumpStringData();
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(Items.Length);
            writer.Write(Items.Length);

            // Idea is to write all entries and come back to replace Ptr(offset).
            long[] positions = new long[Items.Length];
            for(int i = 0; i < Items.Length; i++)
            {
                LocalisableString Entry = Items[i];
                writer.Write(Entry.Unk0);
                writer.Write(Entry.Hash);

                positions[i] = writer.BaseStream.Position;
                writer.Write(-1);
            }
            
            // Seems to be padding. Concerning..
            writer.Write(0);

            for(int i = 0; i < Items.Length; i++)
            {
                LocalisableString Entry = Items[i];

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

        public void ReadFromXML()
        {
            XElement Root = XElement.Load("StringTable_EN.xml");

            foreach (XElement Element in Root.Elements())
            {
                StringTable Entry = ReflectionHelpers.ConvertToPropertyFromXML<StringTable>(Element);
            }
        }

        public void WriteToXML()
        {
            XElement Elements = ReflectionHelpers.ConvertPropertyToXML(Items);
            Elements.Save("StringTable.xml");
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
    }
}
