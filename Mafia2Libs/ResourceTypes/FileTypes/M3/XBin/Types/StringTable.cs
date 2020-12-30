using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public class LocalisableString
    {
        [PropertyIgnoreByReflector]
        public uint Unk0 { get; set; } // I'm guessing this used to be a pointer to the stringID.
        [PropertyForceAsAttribute]
        public XBinHashName StringID { get; set; } // Hash of the StringID which no longer exists. *cry ever tim*
        public string Content { get; set; } // Text

        public LocalisableString()
        {
            Unk0 = 0;
            StringID = new XBinHashName();
            Content = "";
        }

        public override string ToString()
        {
            return Content;
        }
    }

    public class StringTable : BaseTable
    {
        private uint unk0;
        public LocalisableString[] Items { get; set; }

        public StringTable()
        {
            Items = new LocalisableString[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            // XBin files store the count twice.
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            Debug.Assert(count0 == count1, "These numbers should be identical.");

            Items = new LocalisableString[count0];

            for (int i = 0; i < count0; i++)
            {
                LocalisableString Entry = new LocalisableString();
                Entry.Unk0 = reader.ReadUInt32();
                Entry.StringID.ReadFromFile(reader);
                Entry.Content = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                Items[i] = Entry;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            // Order StringTable entries.
            Items = Items.OrderBy(e => e.StringID.Hash).ToArray();

            writer.Write(unk0);
            writer.Write(Items.Length);
            writer.Write(Items.Length);

            // Idea is to write all entries and come back to replace Ptr(offset).
            long[] positions = new long[Items.Length];
            for(int i = 0; i < Items.Count(); i++)
            {
                LocalisableString Entry = Items[i];
                writer.Write(Entry.Unk0);
                Entry.StringID.WriteToFile(writer);

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

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            StringTable Entry = ReflectionHelpers.ConvertToPropertyFromXML<StringTable>(Root);
            Items = Entry.Items;
        }

        public void WriteToXML(string file)
        {
            XElement Elements = ReflectionHelpers.ConvertPropertyToXML(this);
            Elements.Save(file);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "StringTable Contents";

            foreach(var Entry in Items)
            {
                TreeNode ChildNode = new TreeNode();
                ChildNode.Tag = Entry;
                ChildNode.Text = Entry.Content;

                Root.Nodes.Add(ChildNode);
            }

            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            Items = new LocalisableString[Root.Nodes.Count];

            for(int i = 0; i < Items.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                LocalisableString Entry = (LocalisableString)ChildNode.Tag;
                Items[i] = Entry;
            }
        }
    }
}
