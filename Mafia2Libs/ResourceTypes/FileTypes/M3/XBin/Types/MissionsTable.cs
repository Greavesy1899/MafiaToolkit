using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;
using Utils.Settings;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public class MissionItem
    {
        public ulong ID { get; set; }
        public ulong TextID { get; set; }
        public ulong DescriptionID { get; set; }
        public uint IconID { get; set; }
        public uint CityID { get; set; }
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public EMissionType Type { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public uint DescOffset1 { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public uint DescOffset2 { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public uint Unknown { get; set; } // 0

        public string MissionID { get; set; }
        public string CheckPointFile { get; set; }

        public MissionItem()
        {
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
        private MissionItem[] missions;
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
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint unknown = reader.ReadUInt32();
            missions = new MissionItem[count0];

            for (int i = 0; i < count1; i++)
            {
                MissionItem item = new MissionItem();
                item.ID = reader.ReadUInt64();
                item.TextID = reader.ReadUInt64();
                item.DescriptionID = reader.ReadUInt64();
                item.IconID = reader.ReadUInt32();
                item.CityID = reader.ReadUInt32();
                item.Type = (EMissionType)reader.ReadUInt32();

                item.DescOffset1 = reader.ReadUInt32();
                item.DescOffset2 = reader.ReadUInt32();
                item.Unknown = reader.ReadUInt32();

                missions[i] = item;
            }

            for (int i = 0; i < count1; i++)
            {
                var item = missions[i];
                item.MissionID = StringHelpers.ReadString(reader).TrimEnd('\0');
                item.CheckPointFile = StringHelpers.ReadString(reader).TrimEnd('\0');
                missions[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(missions.Length);
            writer.Write(missions.Length);
            writer.Write(0);

            int i = 0;
            int k = 0;
            long[] offsets = new long[missions.Length * 2];
            foreach (var slot in missions)
            {
                //offsets[i] = writer.BaseStream.Position;
                MissionItem Item = missions[i];
                writer.Write(Item.ID);
                writer.Write(Item.TextID);
                writer.Write(Item.DescriptionID);
                writer.Write(Item.IconID);
                writer.Write(Item.CityID);
                writer.Write((int)Item.Type);
                offsets[k++] = writer.BaseStream.Position;
                writer.Write(0xDEADBEEF); // placeholder
                offsets[k++] = writer.BaseStream.Position;
                writer.Write(0xDEADBEEF); // placeholder
                writer.Write(Item.Unknown);
                i++;
            }

            k = 0;
            for (int j = 0; j < missions.Length; j++)
            {
                MissionItem Item = missions[j];
                uint DescOffset1 = (uint)(writer.BaseStream.Position);
                StringHelpers.WriteString(writer, Item.MissionID);
                uint DescOffset2 = (uint)(writer.BaseStream.Position);
                StringHelpers.WriteString(writer, Item.CheckPointFile);

                long currentPosition = writer.BaseStream.Position;

                writer.BaseStream.Position = offsets[k];
                var offset1 = (uint)(DescOffset1 - offsets[k++]);
                writer.Write(offset1);

                writer.BaseStream.Position = offsets[k];
                var offset2 = (uint)(DescOffset2 - offsets[k++]);
                writer.Write(offset2);

                writer.BaseStream.Position = currentPosition;
            }

            offsets = new long[0];
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
            Root.Text = "MissionsTable";

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
