using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.Settings;

namespace ResourceTypes.M3.XBin
{
    public class CarInteriorColorsItem
    {
        public int CarID { get; set; }
        public byte R1 { get; set; }
        public byte G1 { get; set; }
        public byte B1 { get; set; }
        public byte R2 { get; set; }
        public byte G2 { get; set; }
        public byte B2 { get; set; }
        public byte R3 { get; set; }
        public byte G3 { get; set; }
        public byte B3 { get; set; }
        public byte R4 { get; set; }
        public byte G4 { get; set; }
        public byte B4 { get; set; }
        public byte R5 { get; set; }
        public byte G5 { get; set; }
        public byte B5 { get; set; }
        public byte Alpha { get; set; }
        public int Desc { get; set; }

        public override string ToString()
        {
            return string.Format("CarID = {0}", CarID);
        }
    }

    public class CarInteriorColorsTable : BaseTable
    {
        private uint unk0;
        private CarInteriorColorsItem[] colors;
        private GamesEnumerator gameVersion;

        public CarInteriorColorsItem[] InteriorColors {
            get { return colors; }
            set { colors = value; }
        }

        public CarInteriorColorsTable()
        {
            colors = new CarInteriorColorsItem[0];
            gameVersion = gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            colors = new CarInteriorColorsItem[count0];

            for (int i = 0; i < count1; i++)
            {
                CarInteriorColorsItem item = new CarInteriorColorsItem();
                item.CarID = reader.ReadInt32();
                item.R1 = reader.ReadByte();
                item.G1 = reader.ReadByte();
                item.B1 = reader.ReadByte();
                item.R2 = reader.ReadByte();
                item.G2 = reader.ReadByte();
                item.B2 = reader.ReadByte();
                item.R3 = reader.ReadByte();
                item.G3 = reader.ReadByte();
                item.B3 = reader.ReadByte();
                item.R4 = reader.ReadByte();
                item.G4 = reader.ReadByte();
                item.B4 = reader.ReadByte();
                item.R5 = reader.ReadByte();
                item.G5 = reader.ReadByte();
                item.B5 = reader.ReadByte();

                // M1: DE only.
                if (gameVersion == GamesEnumerator.MafiaI_DE)
                {
                    item.Alpha = reader.ReadByte();
                }

                item.Desc = reader.ReadInt32();

                colors[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(colors.Length);
            writer.Write(colors.Length);

            foreach(var color in colors)
            {
                writer.Write(color.CarID);
                writer.Write(color.R1);
                writer.Write(color.G1);
                writer.Write(color.B1);
                writer.Write(color.R2);
                writer.Write(color.G2);
                writer.Write(color.B2);
                writer.Write(color.R3);
                writer.Write(color.G3);
                writer.Write(color.B3);
                writer.Write(color.R4);
                writer.Write(color.G4);
                writer.Write(color.B4);
                writer.Write(color.R5);
                writer.Write(color.G5);
                writer.Write(color.B5);
                
                // Only in M1: DE
                if (gameVersion == GamesEnumerator.MafiaI_DE)
                {
                    writer.Write(color.Alpha);
                }

                writer.Write(color.Desc);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarInteriorColorsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarInteriorColorsTable>(Root);
            this.colors = TableInformation.colors;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "CarInteriorColorsTable";

            foreach(var Item in InteriorColors)
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
            InteriorColors = new CarInteriorColorsItem[Root.Nodes.Count];

            for (int i = 0; i < InteriorColors.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarInteriorColorsItem Entry = (CarInteriorColorsItem)ChildNode.Tag;
                InteriorColors[i] = Entry;
            }
        }
    }
}
