using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool
{
    public partial class CollisionEditor : Form
    {
        public CollisionEditor()
        {
            InitializeComponent();
            CheckCollision();
        }

        public void CheckCollision()
        {
            if (SceneData.Collisions == null)
            {
                MessageBox.Show("No collisions have been loaded.", "Error!");
                Close();
            }
            else
            {
                LoadInCollision();
                ShowDialog();
            }
        }

        public void LoadInCollision()
        {
            for(int i = 0; i != SceneData.Collisions.NXSData.Length; i++)
            {
                Collision.NXSStruct nxsData = SceneData.Collisions.NXSData[i];

                TreeNode node = new TreeNode(nxsData.Hash.ToString());
                node.Tag = nxsData;
                node.Name = nxsData.Hash.ToString();

                treeView1.Nodes.Add(node);
            }

            for(int i = 0; i != SceneData.Collisions.Placements.Length; i++)
            {
                Collision.Placement data = SceneData.Collisions.Placements[i];

                for(int x = 0; x != treeView1.Nodes.Count; x++)
                {
                    if(data.Hash.ToString() == treeView1.Nodes[x].Name)
                    {
                        TreeNode node = new TreeNode(treeView1.Nodes[x].Nodes.Count+1.ToString());
                        node.Tag = data;
                        node.Name = treeView1.Nodes[x].Nodes.Count + 1.ToString();

                        treeView1.Nodes[x].Nodes.Add(node);
                    }
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FrameResourceGrid.SelectedObject = treeView1.SelectedNode.Tag;
        }
    }
}
