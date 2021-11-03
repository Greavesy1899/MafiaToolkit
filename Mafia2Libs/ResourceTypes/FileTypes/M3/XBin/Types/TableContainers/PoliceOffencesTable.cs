using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class PoliceOffencesTables : BaseTable
    {
        public class PoliceOffencesItem
        {
            public uint ID { get; set; }
            [Description("Each type use only once!!!")]
            public EOffenceType OffenceType { get; set; }
            [Description("Is watched in this city")]
            public bool bIsActive { get; set; } // Stored as 4 bytes
            [Description("Points to police bar in second when seen only by human with OffenceReactionRatio set to 0 in civil archetype")]
            public float PointsByNone { get; set; }
            [Description("Points to police bar in second when seen only by human with OffenceReactionRatio set to 1 in civil archetype")]
            public float PointsByWeak { get; set; }
            [Description("Points to police bar in second when seen only by human with OffenceReactionRatio set to 2 in civil archetype")]
            public float PointsByIndifferent { get; set; }
            [Description("Points to police bar in second when seen only by human with OffenceReactionRatio set to 3 in civil archetype")]
            public float PointsByIntense { get; set; }
            [Description("Points to police bar in second when seen by discipline")]
            public float PointsByWeakPolice { get; set; }
            [Description("Points to police bar in second when seen by discipline")]
            public float PointsByIndifferentPolice { get; set; }
            [Description("Points to police bar in second when seen by discipline")]
            public float PointsByIntensePolice { get; set; }
            [Description("Points to police bar in second when seen by shopkeeper")]
            public float PointsByWeakShopkeeper { get; set; }
            [Description("Points to police bar in second when seen by shopkeeper")]
            public float PointsByIndifferentShopkeeper { get; set; }
            [Description("Points to police bar in second when seen by shopkeeper")]
            public float PointsByIntenseShopkeeper { get; set; }
            [Description("This offence will be ignored - not adding any points - from this escalation level on")]
            public int IgnoredFromEscalationLevel { get; set; }
            [Description("Use should be commented in DbgText, for debug purposes")]
            public float DbgSettings { get; set; }
            [Description("Commentary, debug description")]
            public string DbgText { get; set; }

            public PoliceOffencesItem() 
            {
                DbgText = String.Empty;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", ID, OffenceType);
            }
        }

        private uint unk0;
        public string City { get; set; } // length is 32
        public PoliceOffencesItem[] Items { get; set; }

        public PoliceOffencesTables() : base()
        {
            City = string.Empty;
            Items = new PoliceOffencesItem[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            City = StringHelpers.ReadStringBuffer(reader, 32);

            Items = new PoliceOffencesItem[count1];

            for (int i = 0; i < Items.Length; i++)
            {
                PoliceOffencesItem Item = new PoliceOffencesItem();
                Item.ID = reader.ReadUInt32();
                Item.OffenceType = (EOffenceType)reader.ReadUInt32();
                Item.bIsActive = Convert.ToBoolean(reader.ReadInt32());
                Item.PointsByNone = reader.ReadSingle();
                Item.PointsByWeak = reader.ReadSingle();
                Item.PointsByIndifferent = reader.ReadSingle();
                Item.PointsByIntense = reader.ReadSingle();
                Item.PointsByWeakPolice = reader.ReadSingle();
                Item.PointsByIndifferentPolice = reader.ReadSingle();
                Item.PointsByIntensePolice = reader.ReadSingle();
                Item.PointsByWeakShopkeeper = reader.ReadSingle();
                Item.PointsByIndifferentShopkeeper = reader.ReadSingle();
                Item.PointsByIntenseShopkeeper = reader.ReadSingle();
                Item.IgnoredFromEscalationLevel = reader.ReadInt32();
                Item.DbgSettings = reader.ReadSingle();
                Item.DbgText = StringHelpers.ReadStringBuffer(reader, 32);
                Items[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(Items.Length);
            writer.Write(Items.Length);
            StringHelpers.WriteStringBuffer(writer, 32, City);

            for (int i = 0; i < Items.Length; i++)
            {
                PoliceOffencesItem Item = Items[i];
                writer.Write(Item.ID);
                writer.Write((int)Item.OffenceType);
                writer.Write(Convert.ToInt32(Item.bIsActive));
                writer.Write(Item.PointsByNone);
                writer.Write(Item.PointsByWeak);
                writer.Write(Item.PointsByIndifferent);
                writer.Write(Item.PointsByIntense);
                writer.Write(Item.PointsByWeakPolice);
                writer.Write(Item.PointsByIndifferentPolice);
                writer.Write(Item.PointsByIntensePolice);
                writer.Write(Item.PointsByWeakShopkeeper);
                writer.Write(Item.PointsByIndifferentShopkeeper);
                writer.Write(Item.PointsByIntenseShopkeeper);
                writer.Write(Item.IgnoredFromEscalationLevel);
                writer.Write(Item.DbgSettings);
                StringHelpers.WriteStringBuffer(writer, 32, Item.DbgText);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            PoliceOffencesTables TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<PoliceOffencesTables>(Root);
            this.Items = TableInformation.Items;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "PoliceOffences Table";

            foreach (var Item in Items)
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
            Items = new PoliceOffencesItem[Root.Nodes.Count];

            for (int i = 0; i < Items.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                PoliceOffencesItem Entry = (PoliceOffencesItem)ChildNode.Tag;
                Items[i] = Entry;
            }
        }
    }
}
