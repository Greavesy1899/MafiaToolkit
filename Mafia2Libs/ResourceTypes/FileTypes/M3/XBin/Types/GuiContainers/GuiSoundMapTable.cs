using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.GuiContainers
{
    public class GuiSoundMapTable : BaseTable
    {
        public class GuiSoundMapItem
        {
            public int ID { get; set; }
            public ESoundEvent SoundEvent { get; set; }
            public uint WwiseEvent { get; set; }

            public GuiSoundMapItem()
            {
                ID = 0;
            }

            public override string ToString()
            {
                return string.Format("ID = {0}", ID);
            }
        }

        public GuiSoundMapItem[] GuiSoundMap { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            GuiSoundMap = new GuiSoundMapItem[count1];

            for (int i = 0; i < GuiSoundMap.Length; i++)
            {
                GuiSoundMapItem Item = new GuiSoundMapItem();
                Item.ID = i;
                Item.SoundEvent = (ESoundEvent)reader.ReadUInt32();
                Item.WwiseEvent = reader.ReadUInt32();
                GuiSoundMap[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(GuiSoundMap.Length);
            writer.Write(GuiSoundMap.Length);

            for (int i = 0; i < GuiSoundMap.Length; i++)
            {
                GuiSoundMapItem Item = GuiSoundMap[i];
                writer.Write((uint)Item.SoundEvent);
                writer.Write(Item.WwiseEvent);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GuiSoundMapTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<GuiSoundMapTable>(Root);
            this.GuiSoundMap = TableInformation.GuiSoundMap;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "GuiSoundMap Table";

            foreach (var Item in GuiSoundMap)
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
            GuiSoundMap = new GuiSoundMapItem[Root.Nodes.Count];

            for (int i = 0; i < GuiSoundMap.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                GuiSoundMapItem Entry = (GuiSoundMapItem)ChildNode.Tag;
                GuiSoundMap[i] = Entry;
            }
        }
    }
}
