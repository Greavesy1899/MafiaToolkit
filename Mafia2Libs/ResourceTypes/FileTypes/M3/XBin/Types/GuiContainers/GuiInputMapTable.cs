using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.GuiContainers
{
    public class GuiInputMapTable : BaseTable
    {
        public class GuiInputMapItem
        {
            public int ID { get; set; }
            public EInputControlFlags ControlMode { get; set; }
            public EKeyModifiers KeyModifiers { get; set; }
            public EInputDeviceType DeviceType { get; set; }
            public EInputControlUnified Control { get; set; }
            public EControllerType ControlPriority { get; set; }
            public EMenuAction MenuAction { get; set; }

            public GuiInputMapItem()
            {
                ID = 0;
            }

            public override string ToString()
            {
                return string.Format("ID = {0}", ID);
            }
        }

        public GuiInputMapItem[] GuiInputMap { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            GuiInputMap = new GuiInputMapItem[count1];

            for (int i = 0; i < GuiInputMap.Length; i++)
            {
                GuiInputMapItem Item = new GuiInputMapItem();
                Item.ID = i;
                Item.ControlMode = (EInputControlFlags)reader.ReadUInt32();
                Item.KeyModifiers = (EKeyModifiers)reader.ReadUInt32();
                Item.DeviceType = (EInputDeviceType)reader.ReadUInt32();
                Item.Control = (EInputControlUnified)reader.ReadUInt32();
                Item.ControlPriority = (EControllerType)reader.ReadUInt32();
                Item.MenuAction = (EMenuAction)reader.ReadUInt32();
                GuiInputMap[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(GuiInputMap.Length);
            writer.Write(GuiInputMap.Length);

            for (int i = 0; i < GuiInputMap.Length; i++)
            {
                GuiInputMapItem Item = GuiInputMap[i];
                writer.Write((uint)Item.ControlMode);
                writer.Write((uint)Item.KeyModifiers);
                writer.Write((uint)Item.DeviceType);
                writer.Write((uint)Item.Control);
                writer.Write((uint)Item.ControlPriority);
                writer.Write((uint)Item.MenuAction);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GuiInputMapTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<GuiInputMapTable>(Root);
            this.GuiInputMap = TableInformation.GuiInputMap;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "GuiInputMap Table";

            foreach (var Item in GuiInputMap)
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
            GuiInputMap = new GuiInputMapItem[Root.Nodes.Count];

            for (int i = 0; i < GuiInputMap.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                GuiInputMapItem Entry = (GuiInputMapItem)ChildNode.Tag;
                GuiInputMap[i] = Entry;
            }
        }
    }
}
