using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.GfxContainers
{
    public class DecalPatternTable : BaseTable
    {
        public class DecalPatternItem
        {
            public long ID { get; set; }
            public float UV_min_x { get; set; }
            public float UV_min_y { get; set; }
            public float UV_max_x { get; set; }
            public float UV_max_y { get; set; }
            public uint MaterialGUID_Part0 { get; set; }
            public uint MaterialGUID_Part1 { get; set; }
            public float MinRadius { get; set; }
            public float MaxRadius { get; set; }
            public EDecalFlags Flags { get; set; }
            public float Impact { get; set; }
            public int TexCols { get; set; }
            public int TexRows { get; set; }
            public int TexStart { get; set; }
            public int TexEnd { get; set; } 
            public uint Group { get; set; }
            public int MultiDecal { get; set; }
            public float BlendTime { get; set; }
            public int FootStep { get; set; }
            public string Notes { get; set; }

            public DecalPatternItem()
            {
                ID = 0;
            }

            public override string ToString()
            {
                return string.Format("ID = {0}", ID);
            }
        }

        public DecalPatternItem[] DecalPattern { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            uint unknown = reader.ReadUInt32();
            DecalPattern = new DecalPatternItem[count1];

            for (int i = 0; i < DecalPattern.Length; i++)
            {
                DecalPatternItem Item = new DecalPatternItem();
                Item.ID = reader.ReadInt64();
                Item.UV_min_x = reader.ReadSingle();
                Item.UV_min_y = reader.ReadSingle();
                Item.UV_max_x = reader.ReadSingle();
                Item.UV_max_y = reader.ReadSingle();
                Item.MaterialGUID_Part0 = reader.ReadUInt32();
                Item.MaterialGUID_Part1 = reader.ReadUInt32();
                Item.MinRadius = reader.ReadSingle();
                Item.MaxRadius = reader.ReadSingle();
                Item.Flags = (EDecalFlags)reader.ReadUInt32();
                Item.Impact = reader.ReadSingle();
                Item.TexCols = reader.ReadInt32();
                Item.TexRows = reader.ReadInt32();
                Item.TexStart = reader.ReadInt32();
                Item.TexEnd = reader.ReadInt32();
                Item.Group = reader.ReadUInt32();
                Item.MultiDecal = reader.ReadInt32();
                Item.BlendTime = reader.ReadSingle();
                Item.FootStep = reader.ReadInt32();
                Item.Notes = StringHelpers.ReadStringBuffer(reader, 32);
                DecalPattern[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(DecalPattern.Length);
            writer.Write(DecalPattern.Length);
            writer.Write(0);

            for (int i = 0; i < DecalPattern.Length; i++)
            {
                DecalPatternItem Item = DecalPattern[i];
                writer.Write(Item.ID);
                writer.Write(Item.UV_min_x);
                writer.Write(Item.UV_min_y);
                writer.Write(Item.UV_max_x);
                writer.Write(Item.UV_max_y);
                writer.Write(Item.MaterialGUID_Part0);
                writer.Write(Item.MaterialGUID_Part1);
                writer.Write(Item.MinRadius);
                writer.Write(Item.MaxRadius);
                writer.Write((uint)Item.Flags);
                writer.Write(Item.Impact);
                writer.Write(Item.TexCols);
                writer.Write(Item.TexRows);
                writer.Write(Item.TexStart);
                writer.Write(Item.TexEnd);
                writer.Write(Item.Group);
                writer.Write(Item.MultiDecal);
                writer.Write(Item.BlendTime);
                writer.Write(Item.FootStep);
                StringHelpers.WriteStringBuffer(writer, 32, Item.Notes);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            DecalPatternTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<DecalPatternTable>(Root);
            this.DecalPattern = TableInformation.DecalPattern;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "DecalPattern Table";

            foreach (var Item in DecalPattern)
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
            DecalPattern = new DecalPatternItem[Root.Nodes.Count];

            for (int i = 0; i < DecalPattern.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                DecalPatternItem Entry = (DecalPatternItem)ChildNode.Tag;
                DecalPattern[i] = Entry;
            }
        }
    }
}
