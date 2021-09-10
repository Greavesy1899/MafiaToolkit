using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.Settings;

namespace ResourceTypes.M3.XBin
{
    public class SubtitleTableItem
    {
        public XBinAkHashName SubtitleID { get; set; }
        public XBinAkHashName SoundID { get; set; }
        public XBinHashName FacialAnimName { get; set; }
        public XBinHashName LongStringID { get; set; }
        public XBinHashName ShortStringID { get; set; }
        public uint SoundPreset { get; set; }
        public XBinAkHashName VoicePresetOverride { get; set; }
        public uint SubtitlePriorityOverride { get; set; }
        public uint Unk0 { get; set; }
        public XBinHashName SubtitleCharacter { get; set; }

        public override string ToString()
        {
            return LongStringID.ToString();
        }
    }

    public class SubtitleTable : BaseTable
    {
        private uint unk0;
        private SubtitleTableItem[] SubtitleItems;
        private GamesEnumerator gameVersion;

        public SubtitleTableItem[] Subtitles
        {
            get { return SubtitleItems; }
            set { SubtitleItems = value; }
        }

        public SubtitleTable() : base()
        {
            gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            SubtitleItems = new SubtitleTableItem[count0];

            uint another_unknown = reader.ReadUInt32();

            for (int i = 0; i < count1; i++)
            {
                SubtitleTableItem NewItem = new SubtitleTableItem();
                NewItem.SubtitleID = XBinAkHashName.ConstructAndReadFromFile(reader);
                NewItem.SoundID = XBinAkHashName.ConstructAndReadFromFile(reader);
                NewItem.FacialAnimName = XBinHashName.ConstructAndReadFromFile(reader);
                NewItem.LongStringID = XBinHashName.ConstructAndReadFromFile(reader);
                NewItem.ShortStringID = XBinHashName.ConstructAndReadFromFile(reader);
                NewItem.SoundPreset = reader.ReadUInt32();
                NewItem.VoicePresetOverride = XBinAkHashName.ConstructAndReadFromFile(reader);
                NewItem.SubtitlePriorityOverride = reader.ReadUInt32();
                NewItem.Unk0 = reader.ReadUInt32();
                NewItem.SubtitleCharacter = XBinHashName.ConstructAndReadFromFile(reader);
                SubtitleItems[i] = NewItem;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(SubtitleItems.Length);
            writer.Write(SubtitleItems.Length);
            writer.Write(0);

            foreach (SubtitleTableItem Item in SubtitleItems)
            {
                writer.Write(Item.SubtitleID.Hash);
                writer.Write(Item.SoundID.Hash);
                writer.Write(Item.FacialAnimName.Hash);
                writer.Write(Item.LongStringID.Hash);
                writer.Write(Item.ShortStringID.Hash);
                writer.Write(Item.SoundPreset);
                writer.Write(Item.VoicePresetOverride.Hash);
                writer.Write(Item.SubtitlePriorityOverride);
                writer.Write(Item.Unk0);
                writer.Write(Item.SubtitleCharacter.Hash);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            SubtitleTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<SubtitleTable>(Root);
            this.SubtitleItems = TableInformation.SubtitleItems;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "Subtitles Table";

            foreach (var Item in SubtitleItems)
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
            Subtitles = new SubtitleTableItem[Root.Nodes.Count];

            for (int i = 0; i < Subtitles.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                SubtitleTableItem Entry = (SubtitleTableItem)ChildNode.Tag;
                Subtitles[i] = Entry;
            }
        }
    }
}
