using ResourceTypes.Collisions;
using SharpDX;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utils.Lang;
using Utils.Models;
using Utils.Settings;
using Collision = ResourceTypes.Collisions.Collision;

namespace Mafia2Tool
{
    public partial class CollisionEditor : Form
    {
        private FileInfo collisionFile;

        public CollisionEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            collisionFile = file;
            CheckCollision();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Collision editor.");
        }

        private void Localise()
        {
            ContextDelete.Text = Language.GetString("$COLLISION_DELETE");
            deletePlacementToolStripMenuItem.Text = Language.GetString("$PLACEMENT_DELETE");
            openM2T.FileName = Language.GetString("$SELECT_MODEL_FILE");
            fileToolButton.Text = Language.GetString("$FILE");
            saveToolStripMenuItem.Text = Language.GetString("$SAVE");
            reloadToolStripMenuItem.Text = Language.GetString("$RELOAD");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
            toolsButton.Text = Language.GetString("$TOOLS");
            addButton.Text = Language.GetString("$COLLISION_ADD");
            addNewPlacementToolStripMenuItem.Text = Language.GetString("$PLACEMENT_ADD");
            Text = Language.GetString("$COLLISION_EDITOR_TITLE");
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
            for (int i = 0; i != SceneData.Collisions.Models.Count; i++)
            {
                Collision.CollisionModel collisionModel = SceneData.Collisions.Models.ElementAt(i).Value;

                TreeNode node = new TreeNode(collisionModel.Hash.ToString());
                node.Tag = collisionModel;
                node.Name = collisionModel.Hash.ToString();
                //M2TStructure structure = new M2TStructure();
                //structure.BuildCollision(nxsData, node.Name);
                //structure.ExportCollisionToM2T(node.Name);
                //structure.ExportToFbx("Collisions" + "//", true);
                treeView1.Nodes.Add(node);
            }

            //CustomEDD frame = new CustomEDD();
            for (int i = 0; i != SceneData.Collisions.Placements.Count; i++)
            {
                Collision.Placement data = SceneData.Collisions.Placements[i];
                //CustomEDD.Entry entry = new CustomEDD.Entry();

                //entry.LodCount = 1;
                //entry.LODNames = new string[1];
                //entry.LODNames[0] = data.Hash.ToString();
                //entry.Position = data.Position;
                //entry.Rotation = new Matrix33();

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
                //frame.Entries.Add(entry);
            }
            //frame.EntryCount = frame.Entries.Count;
            //using (BinaryWriter writer = new BinaryWriter(File.Create("collisions/frame.edd")))
            //{
            //    frame.WriteToFile(writer);
            //}
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

            M2TStructure m2tColModel = new M2TStructure();

            if (openM2T.FileName.ToLower().EndsWith(".m2t"))
                m2tColModel.ReadFromM2T(new BinaryReader(File.Open(openM2T.FileName, FileMode.Open)));
            else if (openM2T.FileName.ToLower().EndsWith(".fbx"))
                m2tColModel.ReadFromFbx(openM2T.FileName);

            //crash happened/
            if (m2tColModel.Lods[0] == null)
                return;

            Collision.CollisionModel collisionModel = new CollisionModelBuilder().BuildFromM2TStructure(m2tColModel);

            Collision.Placement placement = new Collision.Placement();
            placement.Hash = collisionModel.Hash;
            placement.Unk5 = 128;
            placement.Unk4 = -1;
            placement.Position = new Vector3(0, 0, 0);
            placement.Rotation = new Vector3(0);

            SceneData.Collisions.Models.Add(collisionModel.Hash, collisionModel);
            SceneData.Collisions.Placements.Add(placement);
            treeView1.Nodes.Clear();
            LoadInCollision();
        }

        private void CollisionMaterialTest(object sender, System.EventArgs e)
        {
            if (treeView1.SelectedNode.Tag == null)
                return;

            if (treeView1.SelectedNode.Tag.GetType() != typeof(Collision.CollisionModel))
                return;

            Collision.CollisionModel data = (Collision.CollisionModel)treeView1.SelectedNode.Tag;
            CollisionMaterials[] mats = data.Mesh.MaterialIndices.Select(mi => (CollisionMaterials)mi).ToArray();
            List<CollisionMaterials> typeList = new List<CollisionMaterials>();
            List<int> values = new List<int>();

            for (int x = 0; x != mats.Length; x++)
            {
                if (!typeList.Contains(mats[x]))
                {
                    typeList.Add(mats[x]);
                    values.Add(3);
                }
                else
                {
                    values[typeList.IndexOf(mats[x])] += 3;
                }
            }
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
            SceneData.Collisions.WriteToFile();
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

            Collision.CollisionModel col = treeView1.SelectedNode.Tag as Collision.CollisionModel;
            SceneData.Collisions.Models.Remove(col.Hash);

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

            if (treeView1.SelectedNode.Tag.GetType() == typeof(Collision.CollisionModel))
            {
                CollisionContext.Items[0].Visible = true;
            }
            else if (treeView1.SelectedNode.Tag.GetType() == typeof(Collision.Placement))
            {
                CollisionContext.Items[1].Visible = true;
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            //treeView1.SelectedNode = e.Node;
            FrameResourceGrid.SelectedObject = e.Node.Tag;
        }
    }
}