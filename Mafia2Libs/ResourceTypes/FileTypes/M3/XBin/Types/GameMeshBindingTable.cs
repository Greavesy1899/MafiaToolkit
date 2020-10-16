using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin
{
    public class GameMeshBindingItem
    {
        public ulong NameHash { get; set; }
        public ulong SingleMeshIndex { get; set; }
        public ulong HavokIndex { get; set; }
        public override string ToString()
        {
            return string.Format("SingleMeshIndex = {0} HavokIndex = {1}", SingleMeshIndex, HavokIndex);
        }
    }

    public class GameMeshBindingTable : BaseTable
    {
        private GameMeshBindingItem[] bindings;

        public GameMeshBindingItem[] MeshBindings {
            get { return bindings; }
            set { bindings = value; }
        }

        public GameMeshBindingTable()
        {
            bindings = new GameMeshBindingItem[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint unknown = reader.ReadUInt32();
            bindings = new GameMeshBindingItem[count0];

            for (int i = 0; i < count1; i++)
            {
                GameMeshBindingItem item = new GameMeshBindingItem();
                item.NameHash = reader.ReadUInt64();
                item.SingleMeshIndex = reader.ReadUInt64();
                item.HavokIndex = reader.ReadUInt64();

                bindings[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(bindings.Length);
            writer.Write(bindings.Length);
            writer.Write(0);

            foreach(var bind in bindings)
            {
                writer.Write(bind.NameHash);
                writer.Write(bind.SingleMeshIndex);
                writer.Write(bind.HavokIndex);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GameMeshBindingTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<GameMeshBindingTable>(Root);
            this.bindings = TableInformation.bindings;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "GameMeshBindingTable";

            foreach(var Item in MeshBindings)
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
            MeshBindings = new GameMeshBindingItem[Root.Nodes.Count];

            for (int i = 0; i < MeshBindings.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                GameMeshBindingItem Entry = (GameMeshBindingItem)ChildNode.Tag;
                MeshBindings[i] = Entry;
            }
        }
    }
}
