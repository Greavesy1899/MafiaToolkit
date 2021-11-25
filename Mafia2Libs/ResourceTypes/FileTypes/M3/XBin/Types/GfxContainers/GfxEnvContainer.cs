using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.GfxContainers
{
    public class GfxEnvContainer : BaseTable
    {
        public DecalGroupPatternTable DecalGroupPattern { get; set; }
        public DecalPatternTable DecalPattern { get; set; }
        public GfxGlassBreakTypeTable GfxGlassBreakType { get; set; }
        public GfxGlassMatTemplateTable GfxGlassMatTemplate { get; set; }
        public MultiDecalPatternTable MultiDecalPattern { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint DecalGroupPatternPtr = reader.ReadUInt32();
            uint DecalPatternPtr = reader.ReadUInt32();
            uint GfxGlassBreakTypePtr = reader.ReadUInt32();
            uint GfxGlassMatTemplatePtr = reader.ReadUInt32();
            uint MultiDecalPatternPtr = reader.ReadUInt32();

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
            writer.PushObjectPtr("DecalGroupPatternPtr");
            writer.PushObjectPtr("DecalPatternPtr");
            writer.PushObjectPtr("GfxGlassBreakTypePtr");
            writer.PushObjectPtr("GfxGlassMatTemplatePtr");
            writer.PushObjectPtr("MultiDecalPatternPtr");

            // Write DecalGroupPattern table
            writer.FixUpObjectPtr("DecalGroupPatternPtr");
            writer.Write(0xC);
            DecalGroupPattern.WriteToFile(writer);

            // Write DecalPattern table
            writer.FixUpObjectPtr("DecalPatternPtr");
            writer.Write(0x10);
            DecalPattern.WriteToFile(writer);

            // Write GfxGlassBreakTypePtr table
            writer.FixUpObjectPtr("GfxGlassBreakTypePtr");
            writer.Write(0xC);
            GfxGlassBreakType.WriteToFile(writer);

            // Write GfxGlassMatTemplate table
            writer.FixUpObjectPtr("GfxGlassMatTemplatePtr");
            writer.Write(0xC);
            GfxGlassMatTemplate.WriteToFile(writer);

            // Write MultiDecalPattern table
            writer.FixUpObjectPtr("MultiDecalPatternPtr");
            writer.Write(0xC);
            MultiDecalPattern.WriteToFile(writer);
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            DecalGroupPattern = ReflectionHelpers.ConvertToPropertyFromXML<DecalGroupPatternTable>(Root.Element("DecalGroupPatternTable"));
            DecalPattern = ReflectionHelpers.ConvertToPropertyFromXML<DecalPatternTable>(Root.Element("DecalPatternTable"));
            GfxGlassBreakType = ReflectionHelpers.ConvertToPropertyFromXML<GfxGlassBreakTypeTable>(Root.Element("GfxGlassBreakTypeTable"));
            GfxGlassMatTemplate = ReflectionHelpers.ConvertToPropertyFromXML<GfxGlassMatTemplateTable>(Root.Element("GfxGlassMatTemplateTable"));
            MultiDecalPattern = ReflectionHelpers.ConvertToPropertyFromXML<MultiDecalPatternTable>(Root.Element("MultiDecalPatternTable"));
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = new XElement("GfxEnvContainer");
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(DecalGroupPattern));
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(DecalPattern));
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(GfxGlassBreakType));
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(GfxGlassMatTemplate));
            RootElement.Add(ReflectionHelpers.ConvertPropertyToXML(MultiDecalPattern));
            RootElement.Save(file, SaveOptions.None);
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
            DecalGroupPattern.SetFromTreeNodes(Root.Nodes[0]);
            DecalPattern.SetFromTreeNodes(Root.Nodes[1]);
            GfxGlassBreakType.SetFromTreeNodes(Root.Nodes[2]);
            GfxGlassMatTemplate.SetFromTreeNodes(Root.Nodes[3]);
            MultiDecalPattern.SetFromTreeNodes(Root.Nodes[4]);
        }
    }
}
