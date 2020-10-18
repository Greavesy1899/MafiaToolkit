using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.GfxContainers
{
    public class GfxGlassBreakTypeTable : BaseTable
    {
        public class GfxGlassBreakTypeItem
        {
            public uint ID { get; set; }
            public string TypeName { get; set; }
            public string MixedTex { get; set; }
            public string SpiderTex { get; set; }
            public uint MatGuid_Part0 { get; set; }
            public uint MatGuid_Part1 { get; set; }
            public int Defence { get; set; }
            public int OptimalScale { get; set; }
            public int WorstScale { get; set; }
            public int DynamicBreakPower { get; set; }
            public int DynamicCrackPower { get; set; }
            public int FragmentDisappearLimit { get; set; }
            public int SpiderRnd { get; set; }
            public int SpidersLimitMin { get; set; }
            public int SpidersLimitMax { get; set; }
            public float SpiderSize { get; set; }
            public int FragmentConnectionLimit { get; set; }
            public int CracksLimitMin { get; set; }
            public int CracksLimitMax { get; set; }
            public int CracksLimitPerHit { get; set; }
            public int CracksDamagePerPiece { get; set; }
            public int CrackCreateRnd { get; set; }
            public int SndSpider { get; set; }
            public int SndSpiderCategory { get; set; }
            public int SndDestruct { get; set; }
            public int SndDestructCategory { get; set; }
            public int SndLargeDestruct { get; set; }
            public int SndLargeDestructCategory { get; set; }
            public int PtcSpider { get; set; }
            public int PtcFragment { get; set; }
            public int PtcMultiGlass { get; set; }
            public bool CanDropShards { get; set; }
            public bool GenHumanHole { get; set; }
            public bool Unknown1 { get; set; }
            public bool Unknown2 { get; set; }
            public float ManHoleHeight { get; set; }
            public float ManHoleWidth { get; set; }
            public float DmgForDestruction { get; set; }

            public GfxGlassBreakTypeItem()
            {
                ID = 0;
            }

            public override string ToString()
            {
                return string.Format("ID = {0}", ID);
            }
        }

        public GfxGlassBreakTypeItem[] GfxGlassBreakType { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            GfxGlassBreakType = new GfxGlassBreakTypeItem[count1];

            for (int i = 0; i < GfxGlassBreakType.Length; i++)
            {
                GfxGlassBreakTypeItem Item = new GfxGlassBreakTypeItem();
                Item.ID = reader.ReadUInt32();
                Item.TypeName = StringHelpers.ReadStringBuffer(reader, 32);
                Item.MixedTex = StringHelpers.ReadStringBuffer(reader, 32);
                Item.SpiderTex = StringHelpers.ReadStringBuffer(reader, 32);
                Item.MatGuid_Part0 = reader.ReadUInt32();
                Item.MatGuid_Part1 = reader.ReadUInt32();
                Item.Defence = reader.ReadInt32();
                Item.OptimalScale = reader.ReadInt32();
                Item.WorstScale = reader.ReadInt32();
                Item.DynamicBreakPower = reader.ReadInt32();
                Item.DynamicCrackPower = reader.ReadInt32();
                Item.FragmentDisappearLimit = reader.ReadInt32();
                Item.SpiderRnd = reader.ReadInt32();
                Item.SpidersLimitMin = reader.ReadInt32();
                Item.SpidersLimitMax = reader.ReadInt32();
                Item.SpiderSize = reader.ReadSingle();
                Item.FragmentConnectionLimit = reader.ReadInt32();
                Item.CracksLimitMin = reader.ReadInt32();
                Item.CracksLimitMax = reader.ReadInt32();
                Item.CracksLimitPerHit = reader.ReadInt32();
                Item.CracksDamagePerPiece = reader.ReadInt32();
                Item.CrackCreateRnd = reader.ReadInt32();
                Item.SndSpider = reader.ReadInt32();
                Item.SndSpiderCategory = reader.ReadInt32();
                Item.SndDestruct = reader.ReadInt32();
                Item.SndDestructCategory = reader.ReadInt32();
                Item.SndLargeDestruct = reader.ReadInt32();
                Item.SndLargeDestructCategory = reader.ReadInt32();
                Item.PtcSpider = reader.ReadInt32();
                Item.PtcFragment = reader.ReadInt32();
                Item.PtcMultiGlass = reader.ReadInt32();
                Item.CanDropShards = reader.ReadBoolean();
                Item.GenHumanHole = reader.ReadBoolean();
                Item.Unknown1 = reader.ReadBoolean();
                Item.Unknown2 = reader.ReadBoolean();
                Item.ManHoleHeight = reader.ReadSingle();
                Item.ManHoleWidth = reader.ReadSingle();
                Item.DmgForDestruction = reader.ReadSingle();
                GfxGlassBreakType[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(GfxGlassBreakType.Length);
            writer.Write(GfxGlassBreakType.Length);

            for (int i = 0; i < GfxGlassBreakType.Length; i++)
            {
                GfxGlassBreakTypeItem Item = GfxGlassBreakType[i];
                writer.Write(Item.ID);
                StringHelpers.WriteStringBuffer(writer, 32, Item.TypeName);
                StringHelpers.WriteStringBuffer(writer, 32, Item.MixedTex);
                StringHelpers.WriteStringBuffer(writer, 32, Item.SpiderTex);
                writer.Write(Item.MatGuid_Part0);
                writer.Write(Item.MatGuid_Part1);
                writer.Write(Item.Defence);
                writer.Write(Item.OptimalScale);
                writer.Write(Item.WorstScale);
                writer.Write(Item.DynamicBreakPower);
                writer.Write(Item.DynamicCrackPower);
                writer.Write(Item.FragmentDisappearLimit);
                writer.Write(Item.SpiderRnd);
                writer.Write(Item.SpidersLimitMin);
                writer.Write(Item.SpidersLimitMax);
                writer.Write(Item.SpiderSize);
                writer.Write(Item.FragmentConnectionLimit);
                writer.Write(Item.CracksLimitMin);
                writer.Write(Item.CracksLimitMax);
                writer.Write(Item.CracksLimitPerHit);
                writer.Write(Item.CracksDamagePerPiece);
                writer.Write(Item.CrackCreateRnd);
                writer.Write(Item.SndSpider);
                writer.Write(Item.SndSpiderCategory);
                writer.Write(Item.SndDestruct);
                writer.Write(Item.SndDestructCategory);
                writer.Write(Item.SndLargeDestruct);
                writer.Write(Item.SndLargeDestructCategory);
                writer.Write(Item.PtcSpider);
                writer.Write(Item.PtcFragment);
                writer.Write(Item.PtcMultiGlass);
                writer.Write(Item.CanDropShards);
                writer.Write(Item.GenHumanHole);
                writer.Write(Item.Unknown1);
                writer.Write(Item.Unknown2);
                writer.Write(Item.ManHoleHeight);
                writer.Write(Item.ManHoleWidth);
                writer.Write(Item.DmgForDestruction);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GfxGlassBreakTypeTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<GfxGlassBreakTypeTable>(Root);
            this.GfxGlassBreakType = TableInformation.GfxGlassBreakType;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "GfxGlassBreakType Table";

            foreach (var Item in GfxGlassBreakType)
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
            GfxGlassBreakType = new GfxGlassBreakTypeItem[Root.Nodes.Count];

            for (int i = 0; i < GfxGlassBreakType.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                GfxGlassBreakTypeItem Entry = (GfxGlassBreakTypeItem)ChildNode.Tag;
                GfxGlassBreakType[i] = Entry;
            }
        }
    }
}
