using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.GuiContainers
{
    public class FlashInputMapTable : BaseTable
    {
        public class FlashInputMapItem
        {
            public int ID { get; set; }
            public EKeyModifiers KeyModifiers { get; set; }
            public EMenuAction MenuAction { get; set; }
            public EFlashControlType FlashControlType { get; set; }
            public EInputControlUnified Control { get; set; }
            public EAxisMode AxisMode { get; set; }
            public EFlashDeviceIndex DeviceIndex { get; set; }

            public FlashInputMapItem()
            {
                ID = 0;
            }

            public override string ToString()
            {
                return string.Format("ID = {0}", ID);
            }
        }

        public FlashInputMapItem[] FlashInputMap { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            FlashInputMap = new FlashInputMapItem[count1];

            for (int i = 0; i < FlashInputMap.Length; i++)
            {
                FlashInputMapItem Item = new FlashInputMapItem();
                Item.ID = i;
                Item.KeyModifiers = (EKeyModifiers)reader.ReadUInt32();
                Item.MenuAction = (EMenuAction)reader.ReadUInt32();
                Item.FlashControlType = (EFlashControlType)reader.ReadUInt32();
                Item.Control = (EInputControlUnified)reader.ReadUInt32();
                Item.AxisMode = (EAxisMode)reader.ReadUInt32();
                Item.DeviceIndex = (EFlashDeviceIndex)reader.ReadUInt32();
                FlashInputMap[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(FlashInputMap.Length);
            writer.Write(FlashInputMap.Length);

            for (int i = 0; i < FlashInputMap.Length; i++)
            {
                FlashInputMapItem Item = FlashInputMap[i];
                writer.Write((uint)Item.KeyModifiers);
                writer.Write((uint)Item.MenuAction);
                writer.Write((uint)Item.FlashControlType);
                writer.Write((uint)Item.Control);
                writer.Write((uint)Item.AxisMode);
                writer.Write((uint)Item.DeviceIndex);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            FlashInputMapTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<FlashInputMapTable>(Root);
            this.FlashInputMap = TableInformation.FlashInputMap;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "FlashInputMap Table";

            foreach (var Item in FlashInputMap)
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
            FlashInputMap = new FlashInputMapItem[Root.Nodes.Count];

            for (int i = 0; i < FlashInputMap.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                FlashInputMapItem Entry = (FlashInputMapItem)ChildNode.Tag;
                FlashInputMap[i] = Entry;
            }
        }
    }
}
