using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class ParticleKeysTable : BaseTable
    {
        public class ParticleKeysItem
        {
            public int EffectID { get; set; }
            public int EmitterID { get; set; }
            public int GenerationID { get; set; }
            public EEffectAffectorFlags AffectFlag { get; set; }
            public float Affector { get; set; }
            public int BR { get; set; }
            public float BirthRate { get; set; }
            public int BL { get; set; }
            public float BirthLife { get; set; }
            public int SP { get; set; }
            public float Speed { get; set; }
            public int SC { get; set; }
            public float Scale { get; set; }
            public int CO { get; set; }
            public float ColorOpacity { get; set; }
            public int RS { get; set; }
            public float RotationSpin { get; set; }
            public override string ToString()
            {
                return string.Format("{0} {1} {2}", EffectID, EmitterID, GenerationID);
            }
        }

        public ParticleKeysItem[] Items { get; set; }

        private uint unk0;

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();

            Items = new ParticleKeysItem[count1];
            for (int i = 0; i < Items.Length; i++)
            {
                ParticleKeysItem Item = new ParticleKeysItem();
                Item.EffectID = reader.ReadInt32();
                Item.EmitterID = reader.ReadInt32();
                Item.GenerationID = reader.ReadInt32();
                Item.AffectFlag = (EEffectAffectorFlags)reader.ReadInt32();
                Item.Affector = reader.ReadSingle();
                Item.BR = reader.ReadInt32();
                Item.BirthRate = reader.ReadSingle();
                Item.BL = reader.ReadInt32();
                Item.BirthLife = reader.ReadSingle();
                Item.SP = reader.ReadInt32();
                Item.Speed = reader.ReadSingle();
                Item.SC = reader.ReadInt32();
                Item.Scale = reader.ReadSingle();
                Item.CO = reader.ReadInt32();
                Item.ColorOpacity = reader.ReadSingle();
                Item.RS = reader.ReadInt32();
                Item.RotationSpin = reader.ReadSingle();
                Items[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(Items.Length);
            writer.Write(Items.Length);

            foreach (var Item in Items)
            {
                writer.Write(Item.EffectID);
                writer.Write(Item.EmitterID);
                writer.Write(Item.GenerationID);
                writer.Write((int)Item.AffectFlag);
                writer.Write(Item.Affector);
                writer.Write(Item.BR);
                writer.Write(Item.BirthRate);
                writer.Write(Item.BL);
                writer.Write(Item.BirthLife);
                writer.Write(Item.SP);
                writer.Write(Item.Speed);
                writer.Write(Item.SC);
                writer.Write(Item.Scale);
                writer.Write(Item.CO);
                writer.Write(Item.ColorOpacity);
                writer.Write(Item.RS);
                writer.Write(Item.RotationSpin);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            ParticleKeysTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<ParticleKeysTable>(Root);
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
            Root.Text = "ParticleKeys Table";

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
            Items = new ParticleKeysItem[Items.Length];
            for (int i = 0; i < Items.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                ParticleKeysItem Entry = (ParticleKeysItem)ChildNode.Tag;
                Items[i] = Entry;
            }
        }
    }
}
