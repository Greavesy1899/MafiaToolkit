using System;
using System.IO;
using System.Windows.Forms;

namespace ResourceTypes.M3.XBin.GfxContainers
{
    public class GfxEnvContainer : BaseTable
    {
        public uint DecalGroupPatternPtr { get; set; }
        public uint DecalPatternPtr { get; set; }
        public uint GfxGlassBreakTypePtr { get; set; }
        public uint GfxGlassMatTemplatePtr { get; set; }
        public uint MultiDecalPatternPtr { get; set; }
        public DecalGroupPatternTable DecalGroupPattern { get; set; }
        public DecalPatternTable DecalPattern { get; set; }
        public GfxGlassBreakTypeTable GfxGlassBreakType { get; set; }
        public GfxGlassMatTemplateTable GfxGlassMatTemplate { get; set; }
        public MultiDecalPatternTable MultiDecalPattern { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            DecalGroupPatternPtr = reader.ReadUInt32();
            DecalPatternPtr = reader.ReadUInt32();
            GfxGlassBreakTypePtr = reader.ReadUInt32();
            GfxGlassMatTemplatePtr = reader.ReadUInt32();
            MultiDecalPatternPtr = reader.ReadUInt32();

            uint DecalGroupPatternsValue = reader.ReadUInt32();
            DecalGroupPattern = new DecalGroupPatternTable();
            DecalGroupPattern.ReadFromFile(reader);

            uint DecalPatternValue = reader.ReadUInt32();
            DecalPattern = new DecalPatternTable();
            DecalPattern.ReadFromFile(reader);

            uint GfxGlassBreakTypeValue = reader.ReadUInt32();
            GfxGlassBreakType = new GfxGlassBreakTypeTable();
            GfxGlassBreakType.ReadFromFile(reader);

            uint GfxGlassMatTemplateValue = reader.ReadUInt32();
            GfxGlassMatTemplate = new GfxGlassMatTemplateTable();
            GfxGlassMatTemplate.ReadFromFile(reader);

            uint MultiDecalPatternValue = reader.ReadUInt32();
            MultiDecalPattern = new MultiDecalPatternTable();
            MultiDecalPattern.ReadFromFile(reader);
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
            Root.Text = "GfxEnv Container";
            Root.Nodes.Add(DecalGroupPattern.GetAsTreeNodes());
            Root.Nodes.Add(DecalPattern.GetAsTreeNodes());
            Root.Nodes.Add(GfxGlassBreakType.GetAsTreeNodes());
            Root.Nodes.Add(GfxGlassMatTemplate.GetAsTreeNodes());
            Root.Nodes.Add(MultiDecalPattern.GetAsTreeNodes());

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
