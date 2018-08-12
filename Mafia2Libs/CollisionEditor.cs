using System.IO;
using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool
{
    public partial class CollisionEditor : Form
    {
        public CollisionEditor(FileInfo file)
        {
            InitializeComponent();
            CheckCollision(file);
            ShowDialog();
            DiscordPrefs.Update("Using the Collision editor.");
        }

        public void CheckCollision(FileInfo file)
        {
            SceneData.Collisions = new Collision(file.FullName);
            LoadInCollision();
        }

        public void LoadInCollision()
        {
            for (int i = 0; i != SceneData.Collisions.NXSData.Count; i++)
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

            for (int i = 0; i != SceneData.Collisions.Placements.Count; i++)
            {
                Collision.Placement data = SceneData.Collisions.Placements[i];

                for (int x = 0; x != treeView1.Nodes.Count; x++)
                {
                    if (data.Hash.ToString() == treeView1.Nodes[x].Name)
                    {
                        TreeNode node = new TreeNode(treeView1.Nodes[x].Nodes.Count + 1.ToString());
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

        private void buttonLoadMesh_Click(object sender, System.EventArgs e)
        {
            if (openM2T.ShowDialog() == DialogResult.OK)
            {
                Model colModel = new Model();

                using (BinaryReader reader = new BinaryReader(File.Open(openM2T.FileName, FileMode.Open)))
                    colModel.ReadFromM2T(reader);

                Collision.NXSStruct nxsData = new Collision.NXSStruct();
                nxsData.Hash = 5214193213415322;
                nxsData.Data.BuildBasicCollision(colModel.Lods[0].Vertices, colModel.Lods[0].Parts[0].Indices);
                nxsData.Sections = new Collision.Section[1];
                nxsData.Sections[0] = new Collision.Section();
                nxsData.Sections[0].Unk2 = 13;
                nxsData.Sections[0].NumEdges = nxsData.Data.Triangles.Length * 3;
                nxsData.Sections[0].EdgeData = new byte[nxsData.Sections[0].NumEdges];
                nxsData.Data.sections = nxsData.Sections;

                Collision.Placement placement = new Collision.Placement();
                placement.Hash = 5214193213415322;
                placement.Unk5 = 128;
                placement.Unk4 = -1;
                placement.Position = new Vector3(-1567.367f, -269.247f, -20.333f);
                placement.Rotation = new Vector3(0);


                SceneData.Collisions.NXSData.Add(nxsData);
                SceneData.Collisions.Placements.Add(placement);
                treeView1.Nodes.Clear();
                LoadInCollision();
            }
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(SceneData.Collisions.name)))
            {
                SceneData.Collisions.WriteToFile(writer);
            }
        }
    }
}
