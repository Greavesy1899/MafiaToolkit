using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.Settings;

namespace ResourceTypes.M3.XBin
{
    public class MissionItem
    {
        public XBinHashName ID { get; set; }
        public XBinHashName TextID { get; set; }
        public XBinHashName DescriptionID { get; set; }
        public uint IconID { get; set; }
        public uint CityID { get; set; }
        public EMissionType Type { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public uint DescOffset1 { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public uint DescOffset2 { get; set; }
        public string MissionID { get; set; }
        [Description("Only used in Mafia: Definitive Edition")]
        public string CheckPointFile { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public uint Unknown { get; set; }

        public MissionItem()
        {
            ID = new XBinHashName();
            TextID = new XBinHashName();
            DescriptionID = new XBinHashName();
            MissionID = "";
            CheckPointFile = "";
        }

        public override string ToString()
        {
            return string.Format("{0}", MissionID);
        }
    }

    public class MissionsTable : BaseTable
    {
        private uint unk0;
        private MissionItem[] missions;
        private uint unk1;

        private GamesEnumerator gameVersion;

        public MissionItem[] Missions {
            get { return missions; }
            set { missions = value; }
        }

        public MissionsTable()
        {
            missions = new MissionItem[0];
            gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            unk1 = reader.ReadUInt32();

            missions = new MissionItem[count0];

            for (int i = 0; i < count1; i++)
            {
                MissionItem item = new MissionItem();
                item.ID.ReadFromFile(reader);
                item.TextID.ReadFromFile(reader);
                item.DescriptionID.ReadFromFile(reader);
                item.IconID = reader.ReadUInt32();
                item.CityID = reader.ReadUInt32();
                item.Type = (EMissionType)reader.ReadUInt32();
                item.MissionID = XBinCoreUtils.ReadStringPtrWithOffset(reader);

                // Only read checkpoint file if we are running M1: DE...
                if (gameVersion == GamesEnumerator.MafiaI_DE)
                {
                    item.CheckPointFile = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                    item.Unknown = reader.ReadUInt32();
                }

                missions[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(missions.Length);
            writer.Write(missions.Length);
            writer.Write(unk1);

            foreach (var slot in missions)
            {
                MissionItem Item = slot;
                Item.ID.WriteToFile(writer);
                Item.TextID.WriteToFile(writer);
                Item.DescriptionID.WriteToFile(writer);
                writer.Write(Item.IconID);
                writer.Write(Item.CityID);
                writer.Write((int)Item.Type);
                writer.PushStringPtr(Item.MissionID);

                if (gameVersion == GamesEnumerator.MafiaI_DE)
                {
                    writer.PushStringPtr(Item.CheckPointFile);
                    writer.Write(Item.Unknown);
                }
            }

            writer.Write(0); // padding?
            writer.FixUpStringPtrs();
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            MissionsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<MissionsTable>(Root);
            this.missions = TableInformation.missions;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "Missions Table";

            foreach(var Item in Missions)
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
            Missions = new MissionItem[Root.Nodes.Count];

            for (int i = 0; i < Missions.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                MissionItem Entry = (MissionItem)ChildNode.Tag;
                Missions[i] = Entry;
            }
        }
    }
}
