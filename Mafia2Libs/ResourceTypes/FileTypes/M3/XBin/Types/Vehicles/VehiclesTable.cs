using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using ResourceTypes.M3.XBin.Vehicles;
using Utils.Helpers.Reflection;
using Utils.Settings;

namespace ResourceTypes.M3.XBin
{
    public class VehicleTable : BaseTable
    {
        private uint unk0;
        private IVehicleTableItem[] vehicles;
        private GamesEnumerator gameVersion;

        public IVehicleTableItem[] Vehicles {
            get { return vehicles; }
            set { vehicles = value; }
        }

        public VehicleTable()
        {
            vehicles = new IVehicleTableItem[0];
            gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            vehicles = new IVehicleTableItem[count0];

            for (int i = 0; i < count1; i++)
            {
                IVehicleTableItem Item = null;
                if (gameVersion == GamesEnumerator.MafiaIII)
                {
                    Item = new VehicleTableItem_M3();
                }
                else if(gameVersion == GamesEnumerator.MafiaI_DE)
                {
                    Item = new VehicleTableItem_M1();
                }
                else
                {
                    MessageBox.Show("Toolkit", "Should have got the correct VehicleTableItem type.");
                }

                Item.ReadEntry(reader);

                vehicles[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(vehicles.Length);
            writer.Write(vehicles.Length);

            foreach(var vehicle in vehicles)
            {
                vehicle.WriteEntry(writer);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            VehicleTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<VehicleTable>(Root);
            this.vehicles = TableInformation.vehicles;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "Vehicles Table";

            foreach(var Item in Vehicles)
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
            Vehicles = new IVehicleTableItem[Root.Nodes.Count];

            for (int i = 0; i < Vehicles.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                IVehicleTableItem Entry = (IVehicleTableItem)ChildNode.Tag;
                Vehicles[i] = Entry;
            }
        }
    }
}
