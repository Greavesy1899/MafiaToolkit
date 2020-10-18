using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class CarSkidmarksTable : BaseTable
    {
        public class CarSkidmarksItem
        {
            public uint ID { get; set; }
            public string MaterialName { get; set; }
            public int SkidId { get; set; }
            public float SkidAlpha { get; set; }
            public int RideId { get; set; }
            public float RideAlpha { get; set; }
            public float TerrainDeep { get; set; }
            public float FadeTime { get; set; }

            public CarSkidmarksItem()
            {
                MaterialName = "";
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", ID, MaterialName);
            }
        }

        public CarSkidmarksItem[] CarSkidmarks { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            CarSkidmarks = new CarSkidmarksItem[count1];

            for (int i = 0; i < CarSkidmarks.Length; i++)
            {
                CarSkidmarksItem Item = new CarSkidmarksItem();
                Item.ID = reader.ReadUInt32();
                Item.MaterialName = StringHelpers.ReadStringBuffer(reader, 32);
                Item.SkidId = reader.ReadInt32();
                Item.SkidAlpha = reader.ReadSingle();
                Item.RideId = reader.ReadInt32();
                Item.RideAlpha = reader.ReadSingle();
                Item.TerrainDeep = reader.ReadSingle();
                Item.FadeTime = reader.ReadSingle();
                CarSkidmarks[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(CarSkidmarks.Length);
            writer.Write(CarSkidmarks.Length);

            for (int i = 0; i < CarSkidmarks.Length; i++)
            {
                CarSkidmarksItem Item = CarSkidmarks[i];
                writer.Write(Item.ID);
                StringHelpers.WriteStringBuffer(writer, 32, Item.MaterialName);
                writer.Write(Item.SkidId);
                writer.Write(Item.SkidAlpha);
                writer.Write(Item.RideId);
                writer.Write(Item.RideAlpha);
                writer.Write(Item.TerrainDeep);
                writer.Write(Item.FadeTime);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarSkidmarksTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarSkidmarksTable>(Root);
            this.CarSkidmarks = TableInformation.CarSkidmarks;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "CarSkidmarks Table";

            foreach (var Item in CarSkidmarks)
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
            CarSkidmarks = new CarSkidmarksItem[Root.Nodes.Count];

            for (int i = 0; i < CarSkidmarks.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarSkidmarksItem Entry = (CarSkidmarksItem)ChildNode.Tag;
                CarSkidmarks[i] = Entry;
            }
        }
    }
}
