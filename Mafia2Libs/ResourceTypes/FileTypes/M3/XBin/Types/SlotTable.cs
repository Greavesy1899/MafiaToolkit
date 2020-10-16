using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public class SlotItem
    {
        public int TypeID { get; set; }
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ESlotType SlotType { get; set; } //ESlotType
        [Browsable(false), PropertyIgnoreByReflector]
        public int BaseNameOffset { get; set; }
        public string BaseName { get; set; }
        public uint RAM_Windows { get; set; }
        public uint VRAM_Windows { get; set; }
        public uint RAM_Xbox360 { get; set; }
        public uint VRAM_Xbox360 { get; set; }
        public uint RAM_PS3_DEVKIT { get; set; }
        public uint VRAM_PS3_DEVKIT { get; set; }
        public uint RAM_PS3_TESTKIT { get; set; }
        public uint VRAM_PS3_TESTKIT { get; set; }

        public SlotItem()
        {
            BaseName = "";
        }

        public override string ToString()
        {
            return string.Format("{0}", BaseName);
        }
    }

    public class SlotTable : BaseTable
    {
        private SlotItem[] slots;

        public SlotItem[] Slots {
            get { return slots; }
            set { slots = value; }
        }

        public SlotTable()
        {
            slots = new SlotItem[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            slots = new SlotItem[count0];

            for (int i = 0; i < count1; i++)
            {
                SlotItem item = new SlotItem();
                item.TypeID = reader.ReadInt32();
                item.SlotType = (ESlotType)reader.ReadInt32();
                item.BaseNameOffset = reader.ReadInt32();
                item.RAM_Windows = reader.ReadUInt32();
                item.VRAM_Windows = reader.ReadUInt32();
                item.RAM_Xbox360 = reader.ReadUInt32();
                item.VRAM_Xbox360 = reader.ReadUInt32();
                item.RAM_PS3_DEVKIT = reader.ReadUInt32();
                item.VRAM_PS3_DEVKIT = reader.ReadUInt32();
                item.RAM_PS3_TESTKIT = reader.ReadUInt32();
                item.VRAM_PS3_TESTKIT = reader.ReadUInt32();

                slots[i] = item;
            }

            for (int i = 0; i < count1; i++)
            {
                var item = slots[i];
                item.BaseName = StringHelpers.ReadString(reader).TrimEnd('\0');
                slots[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(slots.Length);
            writer.Write(slots.Length);

            int i = 0;
            long[] offsets = new long[slots.Length];
            foreach (var slot in slots)
            {
                SlotItem Item = slots[i];
                writer.Write(Item.TypeID);
                writer.Write((int)slot.SlotType);
                offsets[i] = writer.BaseStream.Position;
                writer.Write(0xDEADBEEF); // temporary
                writer.Write(Item.RAM_Windows);
                writer.Write(Item.VRAM_Windows);
                writer.Write(Item.RAM_Xbox360);
                writer.Write(Item.VRAM_Xbox360);
                writer.Write(Item.RAM_PS3_DEVKIT);
                writer.Write(Item.VRAM_PS3_DEVKIT);
                writer.Write(Item.RAM_PS3_TESTKIT);
                writer.Write(Item.VRAM_PS3_TESTKIT);
                i++;
            }

            for (int j = 0; j < slots.Length; j++)
            {
                SlotItem Item = slots[j];
                uint thisPosition = (uint)(writer.BaseStream.Position);
                StringHelpers.WriteString(writer, Item.BaseName);

                long currentPosition = writer.BaseStream.Position;
                writer.BaseStream.Position = offsets[j];
                var offset = (uint)(thisPosition - offsets[j]);
                writer.Write(offset);
                writer.BaseStream.Position = currentPosition;
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            SlotTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<SlotTable>(Root);
            this.slots = TableInformation.slots;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "SlotTable";

            foreach(var Item in Slots)
            {
                TreeNode ChildNode = new TreeNode();
                ChildNode.Tag = Item;
                ChildNode.Text = Item.ToString();
                Root.Nodes.Add(ChildNode);
            }

            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            Slots = new SlotItem[Root.Nodes.Count];

            for (int i = 0; i < Slots.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                SlotItem Entry = (SlotItem)ChildNode.Tag;
                Slots[i] = Entry;
            }
        }
    }
}
