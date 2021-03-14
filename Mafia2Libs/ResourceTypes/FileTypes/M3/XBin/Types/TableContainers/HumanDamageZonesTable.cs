using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class HumanDamageZonesTable : BaseTable
    {
        public class StunGroup
        {
            // array data
            [PropertyIgnoreByReflector]
            public uint BodyListOffset { get; set; }
            [PropertyIgnoreByReflector]
            public uint BodyListOffsetLength0 { get; set; }
            [PropertyIgnoreByReflector]
            public uint BodyListOffsetLength1 { get; set; }

            // actual data
            public EBodyPartType[] BodyPartList { get; set; }
            [PropertyForceAsAttribute]
            public float DamageToStun { get; set; }
            [PropertyForceAsAttribute]
            public float DamageTimeWindow { get; set; }
        }

        public class HumanDamageZoneItem
        {
            // ordered id - probably needs to be ordered 
            // or game may crash
            [PropertyForceAsAttribute]
            public uint ItemID { get; set; }

            // stun group array
            [PropertyIgnoreByReflector]
            public uint StunGroupOffset { get; set; }
            [PropertyIgnoreByReflector]
            public uint StunGroupLength0 { get; set; }
            [PropertyIgnoreByReflector]
            public uint StunGroupLength1 { get; set; }

            // actual data
            public StunGroup[] StunGroups { get; set; }
            [PropertyForceAsAttribute]
            public string DamageZoneName { get; set; }
            public int Health { get; set; }
            public float DamageMultiplexBody { get; set; }
            public float ArmorBodyVal { get; set; }
            public float SneakDmgBodyMult { get; set; }
            public float SneakDmgLowerBodyMult { get; set; }
            public float DamageMultiplexHead { get; set; }
            public float ArmorHeadVal { get; set; }
            public float SneakDmgHeadMult { get; set; }
            public float DamageMultiplexHands { get; set; }
            public float ArmorHandsVal { get; set; }
            public float SneakDmgHandsMult { get; set; }
            public float DamageMultiplexLegs { get; set; }
            public float ArmorLegsVal { get; set; }
            public float SneakDmgLegsMult { get; set; }
            public float EachHitStunHealthLevelThreshold { get; set; }
            public float SingleHitStunHealthThreshold { get; set; }
            public XBinHashName WeaponImpactGroupName { get; set; }
        }

        private int unk0;
        public HumanDamageZoneItem[] DamageZones { get; set; }
        private int unk1;

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            unk1 = reader.ReadInt32();

            DamageZones = new HumanDamageZoneItem[count1];
            for (int i = 0; i < DamageZones.Length; i++)
            {
                HumanDamageZoneItem NewItem = new HumanDamageZoneItem();
                NewItem.ItemID = reader.ReadUInt32();
                NewItem.StunGroupOffset = reader.ReadUInt32();
                NewItem.StunGroupLength0 = reader.ReadUInt32();
                NewItem.StunGroupLength1 = reader.ReadUInt32();
                NewItem.DamageZoneName = StringHelpers.ReadStringBuffer(reader, 32).Trim('\0');
                NewItem.Health = reader.ReadInt32();
                NewItem.DamageMultiplexBody = reader.ReadSingle();
                NewItem.ArmorBodyVal = reader.ReadSingle();
                NewItem.SneakDmgBodyMult = reader.ReadSingle();
                NewItem.SneakDmgLowerBodyMult = reader.ReadSingle();
                NewItem.DamageMultiplexHead = reader.ReadSingle();
                NewItem.ArmorHeadVal = reader.ReadSingle();
                NewItem.SneakDmgHeadMult = reader.ReadSingle();
                NewItem.ArmorHandsVal = reader.ReadSingle();
                NewItem.SneakDmgHandsMult = reader.ReadSingle();
                NewItem.DamageMultiplexLegs = reader.ReadSingle();
                NewItem.ArmorLegsVal = reader.ReadSingle();
                NewItem.SneakDmgLegsMult = reader.ReadSingle();
                NewItem.DamageMultiplexHands = reader.ReadSingle();
                NewItem.EachHitStunHealthLevelThreshold = reader.ReadSingle();
                NewItem.SingleHitStunHealthThreshold = reader.ReadSingle();
                NewItem.WeaponImpactGroupName = XBinHashName.ConstructAndReadFromFile(reader);
                DamageZones[i] = NewItem;
            }

            for(int i = 0; i < DamageZones.Length; i++)
            {
                HumanDamageZoneItem DamageZone = DamageZones[i];
                DamageZone.StunGroups = new StunGroup[DamageZone.StunGroupLength0];

                for (int x = 0; x < DamageZone.StunGroupLength0; x++)
                {
                    StunGroup NewGroup = new StunGroup();
                    NewGroup.BodyListOffset = reader.ReadUInt32();
                    NewGroup.BodyListOffsetLength0 = reader.ReadUInt32();
                    NewGroup.BodyListOffsetLength1 = reader.ReadUInt32();
                    NewGroup.DamageToStun = reader.ReadSingle();
                    NewGroup.DamageTimeWindow = reader.ReadSingle();
                    DamageZone.StunGroups[x] = NewGroup;
                }

                for (int x = 0; x < DamageZone.StunGroupLength0; x++)
                {
                    StunGroup Group = DamageZone.StunGroups[x];
                    Group.BodyPartList = new EBodyPartType[Group.BodyListOffsetLength0];
                    for(int z = 0; z < Group.BodyPartList.Length; z++)
                    {
                        Group.BodyPartList[z] = (EBodyPartType)reader.ReadUInt32();
                    }
                }
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(DamageZones.Length);
            writer.Write(DamageZones.Length);

            for (int i = 0; i < DamageZones.Length; i++)
            {
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            HumanDamageZonesTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<HumanDamageZonesTable>(Root);
            this.DamageZones = TableInformation.DamageZones;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "HumanWeaponImpact Table";

            foreach (var Item in DamageZones)
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
            DamageZones = new HumanDamageZoneItem[DamageZones.Length];
            for (int i = 0; i < DamageZones.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                HumanDamageZoneItem Entry = (HumanDamageZoneItem)ChildNode.Tag;
                DamageZones[i] = Entry;
            }
        }
    }
}
