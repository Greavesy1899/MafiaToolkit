using System;
using System.IO;
using System.Windows.Forms;

namespace ResourceTypes.M3.XBin.GuiContainers
{
    public class GameGuiContainer : BaseTable
    {
        public uint GuiInputMapPtr { get; set; }
        public uint FlashInputMapPtr { get; set; }
        public uint GuiFontMapPtr { get; set; }
        public uint GuiSoundMapPtr { get; set; }
        public uint GuiLanguageMapPtr { get; set; }
        public GuiInputMapTable GuiInputMap { get; set; }
        public FlashInputMapTable FlashInputMap { get; set; }
        public GuiFontMapTable GuiFontMap { get; set; }
        public GuiSoundMapTable GuiSoundMap { get; set; }
        public GuiLanguageMapTable GuiLanguageMap { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            GuiInputMapPtr = reader.ReadUInt32();
            FlashInputMapPtr = reader.ReadUInt32();
            GuiFontMapPtr = reader.ReadUInt32();
            GuiSoundMapPtr = reader.ReadUInt32();
            GuiLanguageMapPtr = reader.ReadUInt32();

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
            throw new NotImplementedException();
        }

        public void ReadFromXML(string file)
        {
            throw new NotImplementedException();
        }

        public void WriteToXML(string file)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private void GotoTablePtr(BinaryReader reader)
        {

        }
    }
}
