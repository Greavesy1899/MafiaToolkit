using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.GuiContainers
{
    public class GameGuiContainer : BaseTable
    {
        public GuiInputMapTable GuiInputMap { get; set; }
        public FlashInputMapTable FlashInputMap { get; set; }
        public GuiFontMapTable GuiFontMap { get; set; }
        public GuiSoundMapTable GuiSoundMap { get; set; }
        public GuiLanguageMapTable GuiLanguageMap { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint GuiInputMapPtr = reader.ReadUInt32();
            uint FlashInputMapPtr = reader.ReadUInt32();
            uint GuiFontMapPtr = reader.ReadUInt32();
            uint GuiSoundMapPtr = reader.ReadUInt32();
            uint GuiLanguageMapPtr = reader.ReadUInt32();

            uint GuiInputMapValue = reader.ReadUInt32();
            GuiInputMap = new GuiInputMapTable();
            GuiInputMap.ReadFromFile(reader);

            uint FlashInputMapValue = reader.ReadUInt32();
            FlashInputMap = new FlashInputMapTable();
            FlashInputMap.ReadFromFile(reader);

            uint GuiFontMapValue = reader.ReadUInt32();
            GuiFontMap = new GuiFontMapTable();
            GuiFontMap.ReadFromFile(reader);

            uint GuiSoundMapValue = reader.ReadUInt32();
            GuiSoundMap = new GuiSoundMapTable();
            GuiSoundMap.ReadFromFile(reader);

            uint GuiLanguageMapValue = reader.ReadUInt32();
            GuiLanguageMap = new GuiLanguageMapTable();
            GuiLanguageMap.ReadFromFile(reader);
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.PushObjectPtr("GuiInputMapPtr");
            writer.PushObjectPtr("FlashInputMapPtr");
            writer.PushObjectPtr("GuiFontMapPtr");
            writer.PushObjectPtr("GuiSoundMapPtr");
            writer.PushObjectPtr("GuiLanguageMapPtr");

            // Write GuiInputMap table
            writer.FixUpObjectPtr("GuiInputMapPtr");
            writer.Write(0xC);
            GuiInputMap.WriteToFile(writer);

            // Write FlashInputMap table
            writer.FixUpObjectPtr("FlashInputMapPtr");
            writer.Write(0xC);
            FlashInputMap.WriteToFile(writer);

            // Write GuiFontMap table
            writer.FixUpObjectPtr("GuiFontMapPtr");
            writer.Write(0xC);
            GuiFontMap.WriteToFile(writer);

            // Write GuiSoundMap table
            writer.FixUpObjectPtr("GuiSoundMapPtr");
            writer.Write(0xC);
            GuiSoundMap.WriteToFile(writer);

            // Write GuiLanguageMap table
            writer.FixUpObjectPtr("GuiLanguageMapPtr");
            writer.Write(0xC);
            GuiLanguageMap.WriteToFile(writer);
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GuiInputMap = ReflectionHelpers.ConvertToPropertyFromXML<GuiInputMapTable>(Root.Element("GuiInputMapTable"));
            FlashInputMap = ReflectionHelpers.ConvertToPropertyFromXML<FlashInputMapTable>(Root.Element("FlashInputMapTable"));
            GuiFontMap = ReflectionHelpers.ConvertToPropertyFromXML<GuiFontMapTable>(Root.Element("GuiFontMapTable"));
            GuiSoundMap = ReflectionHelpers.ConvertToPropertyFromXML<GuiSoundMapTable>(Root.Element("GuiSoundMapTable"));
            GuiLanguageMap = ReflectionHelpers.ConvertToPropertyFromXML<GuiLanguageMapTable>(Root.Element("GuiLanguageMapTable"));
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = new XElement("GfxEnvContainer");
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(GuiInputMap));
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(FlashInputMap));
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(GuiFontMap));
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(GuiSoundMap));
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(GuiLanguageMap));
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "GameGui Container";
            Root.Nodes.Add(GuiInputMap.GetAsTreeNodes());
            Root.Nodes.Add(FlashInputMap.GetAsTreeNodes());
            Root.Nodes.Add(GuiFontMap.GetAsTreeNodes());
            Root.Nodes.Add(GuiSoundMap.GetAsTreeNodes());
            Root.Nodes.Add(GuiLanguageMap.GetAsTreeNodes());

            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            GuiInputMap.SetFromTreeNodes(Root.Nodes[0]);
            FlashInputMap.SetFromTreeNodes(Root.Nodes[1]);
            GuiFontMap.SetFromTreeNodes(Root.Nodes[2]);
            GuiSoundMap.SetFromTreeNodes(Root.Nodes[3]);
            GuiLanguageMap.SetFromTreeNodes(Root.Nodes[4]);
        }
    }
}
