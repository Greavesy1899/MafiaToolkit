using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class CarTuningItemTable : BaseTable
    {
        public class CarTuningItemTableItem
        {
            public uint ID { get; set; }
            public uint SlotId { get; set; }
            public string Description { get; set; }
            public ECarTuningItemFlags Flags { get; set; }

            public string TyreFront { get; set; }
            public string TyreRear { get; set; }

            public float EngineTorqueMinRot { get; set; }
            public float EngineTorque { get; set; }
            public float EngineTorqueMaxRot { get; set; }
            public float EnginePowerAndTorqueRotations { get; set; }

            public float EngineMaxRotations { get; set; }
            public float EngineBrakeTorque { get; set; }
            public float EngineInertia { get; set; }
            public float EngineEfficiency { get; set; }

            public float EngineTurboMinimalRotations { get; set; }
            public float EngineTurboOptimalRotations { get; set; }
            public float EngineTurboTurnOnTime { get; set; }
            public float EngineTurboTorqueIncrease { get; set; }

            public int Gearbox { get; set; }
            public float FinalGear { get; set; }
            public float ViscousClutch { get; set; }

            public float FrontSpringLength { get; set; }
            public float FrontSpringStiffness { get; set; }
            public float FrontDamperStiffness { get; set; }
            public float FrontSwayBar { get; set; }
            public float FrontTyrePressure { get; set; }

            public float RearSpringLength { get; set; }
            public float RearSpringStiffness { get; set; }
            public float RearDamperStiffness { get; set; }
            public float RearSwayBar { get; set; }
            public float RearTyrePressure { get; set; }

            public float BreakTorque { get; set; }
            public float BreakEfficiency { get; set; }
            public float BreakReaction { get; set; }

            public float FrontSpoilerCoeff { get; set; }
            public float RearSpoilerCoeff { get; set; }
            public float Aerodynamic { get; set; }

            public uint VehicleBodyMaterialID { get; set; }
            public uint VehicleWindowMaterialID { get; set; }

            public float VehicleMass { get; set; }
            public float EngineResistance { get; set; }
            public float VehicleBodyBoneStiffness { get; set; }
            public float CrashSpeedChange { get; set; }
            public float CarCrewCrashSpeedChange { get; set; }

            public CarTuningItemTableItem()
            {
                Description = "";
                TyreFront = "";
                TyreRear = "";
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", ID, Description);
            }
        }

        public CarTuningItemTableItem[] CarTuningItems { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            CarTuningItems = new CarTuningItemTableItem[count1];

            for (int i = 0; i < CarTuningItems.Length; i++)
            {
                CarTuningItemTableItem CarTuningItem = new CarTuningItemTableItem();
                CarTuningItem.ID = reader.ReadUInt32();
                CarTuningItem.SlotId = reader.ReadUInt32();
                CarTuningItem.Description = StringHelpers.ReadStringBuffer(reader, 32);
                CarTuningItem.Flags = (ECarTuningItemFlags)reader.ReadUInt32();

                CarTuningItem.TyreFront = StringHelpers.ReadStringBuffer(reader, 8);
                CarTuningItem.TyreRear = StringHelpers.ReadStringBuffer(reader, 8);

                CarTuningItem.EngineTorqueMinRot = reader.ReadSingle();
                CarTuningItem.EngineTorque = reader.ReadSingle();
                CarTuningItem.EngineTorqueMaxRot = reader.ReadSingle();
                CarTuningItem.EnginePowerAndTorqueRotations = reader.ReadSingle();

                CarTuningItem.EngineMaxRotations = reader.ReadSingle();
                CarTuningItem.EngineBrakeTorque = reader.ReadSingle();
                CarTuningItem.EngineInertia = reader.ReadSingle();
                CarTuningItem.EngineEfficiency = reader.ReadSingle();

                CarTuningItem.EngineTurboMinimalRotations = reader.ReadSingle();
                CarTuningItem.EngineTurboOptimalRotations = reader.ReadSingle();
                CarTuningItem.EngineTurboTurnOnTime = reader.ReadSingle();
                CarTuningItem.EngineTurboTorqueIncrease = reader.ReadSingle();

                CarTuningItem.Gearbox = reader.ReadInt32();
                CarTuningItem.FinalGear = reader.ReadSingle();
                CarTuningItem.ViscousClutch = reader.ReadSingle();

                CarTuningItem.FrontSpringLength = reader.ReadSingle();
                CarTuningItem.FrontSpringStiffness = reader.ReadSingle();
                CarTuningItem.FrontDamperStiffness = reader.ReadSingle();
                CarTuningItem.FrontSwayBar = reader.ReadSingle();
                CarTuningItem.FrontTyrePressure = reader.ReadSingle();

                CarTuningItem.RearSpringLength = reader.ReadSingle();
                CarTuningItem.RearSpringStiffness = reader.ReadSingle();
                CarTuningItem.RearDamperStiffness = reader.ReadSingle();
                CarTuningItem.RearSwayBar = reader.ReadSingle();
                CarTuningItem.RearTyrePressure = reader.ReadSingle();

                CarTuningItem.BreakTorque = reader.ReadSingle();
                CarTuningItem.BreakEfficiency = reader.ReadSingle();
                CarTuningItem.BreakReaction = reader.ReadSingle();

                CarTuningItem.FrontSpoilerCoeff = reader.ReadSingle();
                CarTuningItem.RearSpoilerCoeff = reader.ReadSingle();
                CarTuningItem.Aerodynamic = reader.ReadSingle();

                CarTuningItem.VehicleBodyMaterialID = reader.ReadUInt32();
                CarTuningItem.VehicleWindowMaterialID = reader.ReadUInt32();

                CarTuningItem.VehicleMass = reader.ReadSingle();
                CarTuningItem.EngineResistance = reader.ReadSingle();
                CarTuningItem.VehicleBodyBoneStiffness = reader.ReadSingle();
                CarTuningItem.CrashSpeedChange = reader.ReadSingle();
                CarTuningItem.CarCrewCrashSpeedChange = reader.ReadSingle();

                CarTuningItems[i] = CarTuningItem;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(CarTuningItems.Length);
            writer.Write(CarTuningItems.Length);

            for (int i = 0; i < CarTuningItems.Length; i++)
            {
                CarTuningItemTableItem CarTuningItem = CarTuningItems[i];
                writer.Write(CarTuningItem.ID);
                writer.Write(CarTuningItem.SlotId);
                StringHelpers.WriteStringBuffer(writer, 32, CarTuningItem.Description);
                writer.Write((uint)CarTuningItem.Flags);

                StringHelpers.WriteStringBuffer(writer, 8, CarTuningItem.TyreFront);
                StringHelpers.WriteStringBuffer(writer, 8, CarTuningItem.TyreRear);

                writer.Write(CarTuningItem.EngineTorqueMinRot);
                writer.Write(CarTuningItem.EngineTorque);
                writer.Write(CarTuningItem.EngineTorqueMaxRot);
                writer.Write(CarTuningItem.EnginePowerAndTorqueRotations);

                writer.Write(CarTuningItem.EngineMaxRotations);
                writer.Write(CarTuningItem.EngineBrakeTorque);
                writer.Write(CarTuningItem.EngineInertia);
                writer.Write(CarTuningItem.EngineEfficiency);

                writer.Write(CarTuningItem.EngineTurboMinimalRotations);
                writer.Write(CarTuningItem.EngineTurboOptimalRotations);
                writer.Write(CarTuningItem.EngineTurboTurnOnTime);
                writer.Write(CarTuningItem.EngineTurboTorqueIncrease);

                writer.Write(CarTuningItem.Gearbox);
                writer.Write(CarTuningItem.FinalGear);
                writer.Write(CarTuningItem.ViscousClutch);

                writer.Write(CarTuningItem.FrontSpringLength);
                writer.Write(CarTuningItem.FrontSpringStiffness);
                writer.Write(CarTuningItem.FrontDamperStiffness);
                writer.Write(CarTuningItem.FrontSwayBar);
                writer.Write(CarTuningItem.FrontTyrePressure);

                writer.Write(CarTuningItem.RearSpringLength);
                writer.Write(CarTuningItem.RearSpringStiffness);
                writer.Write(CarTuningItem.RearDamperStiffness);
                writer.Write(CarTuningItem.RearSwayBar);
                writer.Write(CarTuningItem.RearTyrePressure);

                writer.Write(CarTuningItem.BreakTorque);
                writer.Write(CarTuningItem.BreakEfficiency);
                writer.Write(CarTuningItem.BreakReaction);

                writer.Write(CarTuningItem.FrontSpoilerCoeff);
                writer.Write(CarTuningItem.RearSpoilerCoeff);
                writer.Write(CarTuningItem.Aerodynamic);

                writer.Write(CarTuningItem.VehicleBodyMaterialID);
                writer.Write(CarTuningItem.VehicleWindowMaterialID);

                writer.Write(CarTuningItem.VehicleMass);
                writer.Write(CarTuningItem.EngineResistance);
                writer.Write(CarTuningItem.VehicleBodyBoneStiffness);
                writer.Write(CarTuningItem.CrashSpeedChange);
                writer.Write(CarTuningItem.CarCrewCrashSpeedChange);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarTuningItemTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarTuningItemTable>(Root);
            this.CarTuningItems = TableInformation.CarTuningItems;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "CarTuningItem Table";

            foreach (var Item in CarTuningItems)
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
            CarTuningItems = new CarTuningItemTableItem[Root.Nodes.Count];

            for (int i = 0; i < CarTuningItems.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarTuningItemTableItem Entry = (CarTuningItemTableItem)ChildNode.Tag;
                CarTuningItems[i] = Entry;
            }
        }
    }
}
