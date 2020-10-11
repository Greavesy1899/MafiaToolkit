using ResourceTypes.FileTypes.M3.XBin;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.Settings;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public class CarGearBoxesItem
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int Automatic { get; set; }
        public int GearCount { get; set; }
        public int GearReverseCount { get; set; }
        public float GearRatio0 { get; set; }
        public float RotationsGearUp0 { get; set; }
        public float RotationsGearDown0 { get; set; }
        public float GearRatio1 { get; set; }
        public float RotationsGearUp1 { get; set; }
        public float RotationsGearDown1 { get; set; }
        public float GearRatio2 { get; set; }
        public float RotationsGearUp2 { get; set; }
        public float RotationsGearDown2 { get; set; }
        public float GearRatio3 { get; set; }
        public float RotationsGearUp3 { get; set; }
        public float RotationsGearDown3 { get; set; }
        public float GearRatio4 { get; set; }
        public float RotationsGearUp4 { get; set; }
        public float RotationsGearDown4 { get; set; }
        public float GearRatio5 { get; set; }
        public float RotationsGearUp5 { get; set; }
        public float RotationsGearDown5 { get; set; }
        public float GearRatio6 { get; set; }
        public float RotationsGearUp6 { get; set; }
        public float RotationsGearDown6 { get; set; }
        public float MinClutchGlobal { get; set; }
        public float MinClutchAngleCoeffGlobal { get; set; }
        public float ShiftDelayMin { get; set; }
        public float ShiftDelayMax { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", ID, Description);
        }
    }

    public class CarGearBoxesTable : BaseTable
    {
        private CarGearBoxesItem[] gearboxes;
        private GamesEnumerator gameVersion;

        public CarGearBoxesItem[] Gearboxes {
            get { return gearboxes; }
            set { gearboxes = value; }
        }

        public CarGearBoxesTable()
        {
            gearboxes = new CarGearBoxesItem[0];
            gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            gearboxes = new CarGearBoxesItem[count0];

            for (int i = 0; i < count1; i++)
            {
                CarGearBoxesItem item = new CarGearBoxesItem();
                item.ID = reader.ReadInt32();
                item.Description = StringHelpers.ReadStringBuffer(reader, 32).TrimEnd('\0');
                item.Automatic = reader.ReadInt32(); //bool
                item.GearCount = reader.ReadInt32();
                item.GearReverseCount = reader.ReadInt32();
                item.GearRatio0 = reader.ReadSingle();
                item.RotationsGearUp0 = reader.ReadSingle();
                item.RotationsGearDown0 = reader.ReadSingle();
                item.GearRatio1 = reader.ReadSingle();
                item.RotationsGearUp1 = reader.ReadSingle();
                item.RotationsGearDown1 = reader.ReadSingle();
                item.GearRatio2 = reader.ReadSingle();
                item.RotationsGearUp2 = reader.ReadSingle();
                item.RotationsGearDown2 = reader.ReadSingle();
                item.GearRatio3 = reader.ReadSingle();
                item.RotationsGearUp3 = reader.ReadSingle();
                item.RotationsGearDown3 = reader.ReadSingle();
                item.GearRatio4 = reader.ReadSingle();
                item.RotationsGearUp4 = reader.ReadSingle();
                item.RotationsGearDown4 = reader.ReadSingle();
                item.GearRatio5 = reader.ReadSingle();
                item.RotationsGearUp5 = reader.ReadSingle();
                item.RotationsGearDown5 = reader.ReadSingle();
                item.GearRatio6 = reader.ReadSingle();
                item.RotationsGearUp6 = reader.ReadSingle();
                item.RotationsGearDown6 = reader.ReadSingle();
                item.MinClutchGlobal = reader.ReadSingle();
                item.MinClutchAngleCoeffGlobal = reader.ReadSingle();
                item.ShiftDelayMin = reader.ReadSingle();
                item.ShiftDelayMax = reader.ReadSingle();

                gearboxes[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(gearboxes.Length);
            writer.Write(gearboxes.Length);

            foreach(var gearbox in gearboxes)
            {
                writer.Write(gearbox.ID);
                StringHelpers.WriteStringBuffer(writer, 32, gearbox.Description);
                writer.Write(gearbox.Automatic);
                writer.Write(gearbox.GearCount);
                writer.Write(gearbox.GearReverseCount);
                writer.Write(gearbox.GearRatio0);
                writer.Write(gearbox.RotationsGearUp0);
                writer.Write(gearbox.RotationsGearDown0);
                writer.Write(gearbox.GearRatio1);
                writer.Write(gearbox.RotationsGearUp1);
                writer.Write(gearbox.RotationsGearDown1);
                writer.Write(gearbox.GearRatio2);
                writer.Write(gearbox.RotationsGearUp2);
                writer.Write(gearbox.RotationsGearDown2);
                writer.Write(gearbox.GearRatio3);
                writer.Write(gearbox.RotationsGearUp3);
                writer.Write(gearbox.RotationsGearDown3);
                writer.Write(gearbox.GearRatio4);
                writer.Write(gearbox.RotationsGearUp4);
                writer.Write(gearbox.RotationsGearDown4);
                writer.Write(gearbox.GearRatio5);
                writer.Write(gearbox.RotationsGearUp5);
                writer.Write(gearbox.RotationsGearDown5);
                writer.Write(gearbox.GearRatio6);
                writer.Write(gearbox.RotationsGearUp6);
                writer.Write(gearbox.RotationsGearDown6);
                writer.Write(gearbox.MinClutchGlobal);
                writer.Write(gearbox.MinClutchAngleCoeffGlobal);
                writer.Write(gearbox.ShiftDelayMin);
                writer.Write(gearbox.ShiftDelayMax);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarGearBoxesTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarGearBoxesTable>(Root);
            this.gearboxes = TableInformation.gearboxes;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "CarGearBoxesTable";

            foreach(var Item in Gearboxes)
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
            Gearboxes = new CarGearBoxesItem[Root.Nodes.Count];

            for (int i = 0; i < Gearboxes.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarGearBoxesItem Entry = (CarGearBoxesItem)ChildNode.Tag;
                Gearboxes[i] = Entry;
            }
        }
    }
}
