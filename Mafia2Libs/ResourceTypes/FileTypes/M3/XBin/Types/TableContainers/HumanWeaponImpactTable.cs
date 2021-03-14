using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class HumanWeaponImpactTable : BaseTable
    {
        public class ParticleNamesItem
        {
            public XBinHashName InitialShot { get; set; }
            public XBinHashName Headshot { get; set; }
            public XBinHashName GrazingShot { get; set; }
            public XBinHashName KillShot { get; set; }
            public float SplashDiameter { get; set; }
            public float SplashHardness { get; set; }
            public float SplashStrength { get; set; }
            public float ShotDiameter { get; set; }
            public float ShotHardness { get; set; }
            public float ShotStrength { get; set; }
        }

        private int unk0;
        public ParticleNamesItem[] ImpactGroups { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            ImpactGroups = new ParticleNamesItem[count1];
            for(int i = 0; i < ImpactGroups.Length; i++)
            {
                ParticleNamesItem NewGroup = new ParticleNamesItem();
                NewGroup.InitialShot = XBinHashName.ConstructAndReadFromFile(reader);
                NewGroup.Headshot = XBinHashName.ConstructAndReadFromFile(reader);
                NewGroup.GrazingShot = XBinHashName.ConstructAndReadFromFile(reader);
                NewGroup.KillShot = XBinHashName.ConstructAndReadFromFile(reader);
                NewGroup.SplashDiameter = reader.ReadSingle();
                NewGroup.SplashHardness = reader.ReadSingle();
                NewGroup.SplashStrength = reader.ReadSingle();
                NewGroup.ShotDiameter = reader.ReadSingle();
                NewGroup.ShotHardness = reader.ReadSingle();
                NewGroup.ShotStrength = reader.ReadSingle();
                ImpactGroups[i] = NewGroup;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(ImpactGroups.Length);
            writer.Write(ImpactGroups.Length);

            for(int i = 0; i < ImpactGroups.Length; i++)
            {
                ParticleNamesItem NewGroup = ImpactGroups[i];
                NewGroup.InitialShot.WriteToFile(writer);
                NewGroup.Headshot.WriteToFile(writer);
                NewGroup.GrazingShot.WriteToFile(writer);
                NewGroup.KillShot.WriteToFile(writer);
                writer.Write(NewGroup.SplashDiameter);
                writer.Write(NewGroup.SplashHardness);
                writer.Write(NewGroup.SplashStrength);
                writer.Write(NewGroup.ShotDiameter);
                writer.Write(NewGroup.ShotHardness);
                writer.Write(NewGroup.ShotStrength);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            HumanWeaponImpactTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<HumanWeaponImpactTable>(Root);
            this.ImpactGroups = TableInformation.ImpactGroups;
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

            foreach (var Item in ImpactGroups)
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
            ImpactGroups = new ParticleNamesItem[ImpactGroups.Length];
            for (int i = 0; i < ImpactGroups.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                ParticleNamesItem Entry = (ParticleNamesItem)ChildNode.Tag;
                ImpactGroups[i] = Entry;
            }
        }
    }
}
