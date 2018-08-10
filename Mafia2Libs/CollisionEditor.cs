using System.IO;
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
            for (int i = 0; i != SceneData.Collisions.NXSData.Length; i++)
            {
                Collision.NXSStruct nxsData = SceneData.Collisions.NXSData[i];

                TreeNode node = new TreeNode(nxsData.Hash.ToString());
                node.Tag = nxsData;
                node.Name = nxsData.Hash.ToString();


                Model model = new Model();
                model.Lods = new Lod[1];
                model.Lods[0] = new Lod();
                model.Lods[0].Parts = new ModelPart[1];
                model.Lods[0].Parts[0] = new ModelPart();
                ;
                model.Lods[0].Vertices = new Vertex[nxsData.Data.Vertices.Length];
                model.Lods[0].Parts[0].Indices = new Short3[nxsData.Data.Triangles.Length];

                for (int x = 0; x != model.Lods[0].Vertices.Length; x++)
                {
                    model.Lods[0].Vertices[x] = new Vertex();
                    model.Lods[0].Vertices[x].Position = nxsData.Data.Vertices[x];
                }


                for (int x = 0; x != model.Lods[0].Parts[0].Indices.Length; x++)
                    model.Lods[0].Parts[0].Indices[x] = new Short3(nxsData.Data.Triangles[x]);


                model.ExportCollisionToM2T(node.Name);
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
