using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin
{
    public class CharacterCinematicsItem
    {
        public ulong StringId { get; set; }
        public ulong CharacterId { get; set; }

        public override string ToString()
        {
            return string.Format("StringId = {0}", StringId);
        }
    }
	
    public class CharacterCinematicsTable : BaseTable
    {
        private CharacterCinematicsItem[] charactercinematics;

        public CharacterCinematicsItem[] CharacterCinematics {
            get { return charactercinematics; }
            set { charactercinematics = value; }
        }

        public CharacterCinematicsTable()
        {
            charactercinematics = new CharacterCinematicsItem[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint unknown = reader.ReadUInt32();
            charactercinematics = new CharacterCinematicsItem[count0];

            for (int i = 0; i < count1; i++)
            {
                CharacterCinematicsItem item = new CharacterCinematicsItem();
                item.StringId = reader.ReadUInt64();
                item.CharacterId = reader.ReadUInt64();

                charactercinematics[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(charactercinematics.Length);
            writer.Write(charactercinematics.Length);
            writer.Write(0);

            foreach (var charactercinematic in charactercinematics)
            {
                writer.Write(charactercinematic.StringId);
                writer.Write(charactercinematic.CharacterId);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CharacterCinematicsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CharacterCinematicsTable>(Root);
            this.charactercinematics = TableInformation.charactercinematics;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "CharacterCinematicsTable";

            foreach(var Item in CharacterCinematics)
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
            CharacterCinematics = new CharacterCinematicsItem[Root.Nodes.Count];

            for (int i = 0; i < CharacterCinematics.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CharacterCinematicsItem Entry = (CharacterCinematicsItem)ChildNode.Tag;
                CharacterCinematics[i] = Entry;
            }
        }
    }
}
