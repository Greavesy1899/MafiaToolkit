using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool
{
    public partial class CollisionEditor : Form
    {
        private FileInfo collisionFile;

        public CollisionEditor(FileInfo file)
        {
            InitializeComponent();
            collisionFile = file;
            CheckCollision();
            ShowDialog();
            ToolkitSettings.UpdateRichPresence("Using the Collision editor.");
        }

        /// <summary>
        /// make sure collisions can load; and then proceed.
        /// </summary>
        public void CheckCollision()
        {
            SceneData.Collisions = new Collision(collisionFile.FullName);
            LoadInCollision();
        }

        /// <summary>
        /// Insert all collisions into the editor.
        /// </summary>
        public void LoadInCollision()
        {
            treeView1.Nodes.Clear();
            for (int i = 0; i != SceneData.Collisions.NXSData.Count; i++)
            {
                Collision.NXSStruct nxsData = SceneData.Collisions.NXSData.ElementAt(i).Value;

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
                {
                    model.Lods[0].Parts[0].Indices[x] = new Short3(nxsData.Data.Triangles[x]);
                }


                model.ExportCollisionToM2T(node.Name);
                treeView1.Nodes.Add(node);
            }

            CustomEDD frame = new CustomEDD();
            for (int i = 0; i != SceneData.Collisions.Placements.Count; i++)
            {
                Collision.Placement data = SceneData.Collisions.Placements[i];
                CustomEDD.Entry entry = new CustomEDD.Entry();

                entry.LodCount = 1;
                entry.LODNames = new string[1];
                entry.LODNames[0] = data.Hash.ToString();
                entry.Position = data.Position;
                entry.Rotation = new Matrix33();

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
                frame.Entries.Add(entry);
            }
            frame.EntryCount = frame.Entries.Count;
            using (BinaryWriter writer = new BinaryWriter(File.Create("collisions/frame.edd")))
            {
                frame.WriteToFile(writer);
            }
        }

        /// <summary>
        /// Update property grid with latest selected  item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickNode(object sender, System.EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            FrameResourceGrid.SelectedObject = treeView1.SelectedNode.Tag;
        }


        /// <summary>
        /// Insert new mesh into the collision data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCollisionModel(object sender, System.EventArgs e)
        {
            //make sure an actual model has been selected.
            if (openM2T.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Failed to select an M2T model.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Model colModel = new Model();

            if (openM2T.FileName.ToLower().EndsWith(".m2t"))
                colModel.ReadFromM2T(new BinaryReader(File.Open(openM2T.FileName, FileMode.Open)));
            else if (openM2T.FileName.ToLower().EndsWith(".fbx"))
                colModel.ReadFromFbx(openM2T.FileName);

            Collision.NXSStruct nxsData = new Collision.NXSStruct();
            nxsData.Hash = (ulong)(Functions.RandomGenerator.Next() + Functions.RandomGenerator.Next());
            nxsData.Data.BuildBasicCollision(colModel.Lods[0]);
            nxsData.Sections = new Collision.Section[1];
            nxsData.Sections[0] = new Collision.Section();
            nxsData.Sections[0].Unk2 = 13;
            nxsData.Sections[0].NumEdges = nxsData.Data.Triangles.Length * 3;
            nxsData.Sections[0].EdgeData = new byte[nxsData.Sections[0].NumEdges];
            nxsData.Data.sections = nxsData.Sections;

            Collision.Placement placement = new Collision.Placement();
            placement.Hash = nxsData.Hash;
            placement.Unk5 = 128;
            placement.Unk4 = -1;
            placement.Position = new Vector3(-1567.367f, -269.247f, -20.333f);
            placement.Rotation = new Vector3(0);


            SceneData.Collisions.NXSData.Add(nxsData.Hash, nxsData);
            SceneData.Collisions.Placements.Add(placement);
            treeView1.Nodes.Clear();
            LoadInCollision();
        }

        /// <summary>
        /// Close and clean data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClose(object sender, FormClosingEventArgs e)
        {
            SceneData.CleanData();
        }

        /// <summary>
        /// Save collision file to system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Create(SceneData.Collisions.name+"2")))
            {
                SceneData.Collisions.WriteToFile(writer);
            }
        }

        /// <summary>
        /// Reload collision file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reloadToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            CheckCollision();
        }


        /// <summary>
        /// Exit editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Delete collision and placements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteCollision(object sender, System.EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            Collision.NXSStruct col = treeView1.SelectedNode.Tag as Collision.NXSStruct;
            SceneData.Collisions.NXSData.Remove(col.Hash);

            for(int i = 0; i != SceneData.Collisions.Placements.Count; i++)
            {
                Collision.Placement placement = SceneData.Collisions.Placements[i];
                if(placement.Hash == col.Hash)
                {
                    SceneData.Collisions.Placements.Remove(placement);

                    TreeNode[] nodes = treeView1.Nodes.Find(placement.Hash.ToString(), true);

                    if (nodes.Length != 0)
                    {
                        for(int x = 0; x != nodes.Length; x++)
                        {
                            nodes[x].Remove();
                        }
                    }
                    i--;
                }
            }
        }

        /// <summary>
        /// Sort our context. TODO: NEED TO REDO. DOESN'T WORK VERY WELL..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOpening(object sender, CancelEventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                e.Cancel = true;
                return;
            }

            CollisionContext.Items[0].Visible = false;
            CollisionContext.Items[1].Visible = false;

            if (treeView1.SelectedNode.Tag.GetType() == typeof(Collision.NXSStruct))
            {
                CollisionContext.Items[0].Visible = true;
            }
            else if (treeView1.SelectedNode.Tag.GetType() == typeof(Collision.Placement))
            {
                CollisionContext.Items[1].Visible = true;
            }
        }
    }
}