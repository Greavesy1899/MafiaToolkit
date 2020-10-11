using ResourceTypes.FileTypes.M3.XBin;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.Settings;

namespace ResourceTypes.M3.XBin
{
    public class CarTuningModificatorsItem
    {
        public int ID { get; set; }
        public int CarId { get; set; }
        public int ItemId { get; set; }
        public int MemberId { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return string.Format("CarID = {0} ItemId = {1}", CarId, ItemId);
        }
    }

    public class CarTuningModificatorsTable : BaseTable
    {
        private CarTuningModificatorsItem[] modificators;
        private GamesEnumerator gameVersion;

        public CarTuningModificatorsItem[] TuningModificators {
            get { return modificators; }
            set { modificators = value; }
        }

        public CarTuningModificatorsTable()
        {
            modificators = new CarTuningModificatorsItem[0];
            gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            modificators = new CarTuningModificatorsItem[count0];

            for (int i = 0; i < count1; i++)
            {
                CarTuningModificatorsItem item = new CarTuningModificatorsItem();
                item.ID = reader.ReadInt32();
                item.CarId = reader.ReadInt32();
                item.ItemId = reader.ReadInt32();
                item.MemberId = reader.ReadInt32();
                item.Value = reader.ReadInt32();

                modificators[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(modificators.Length);
            writer.Write(modificators.Length);

            foreach(var modificator in modificators)
            {
                writer.Write(modificator.ID);
                writer.Write(modificator.CarId);
                writer.Write(modificator.ItemId);
                writer.Write(modificator.MemberId);
                writer.Write(modificator.Value);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarTuningModificatorsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarTuningModificatorsTable>(Root);
            this.modificators = TableInformation.modificators;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "CarTuningModificatorsTable";

            foreach(var Item in TuningModificators)
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
            TuningModificators = new CarTuningModificatorsItem[Root.Nodes.Count];

            for (int i = 0; i < TuningModificators.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarTuningModificatorsItem Entry = (CarTuningModificatorsItem)ChildNode.Tag;
                TuningModificators[i] = Entry;
            }
        }
    }
}
