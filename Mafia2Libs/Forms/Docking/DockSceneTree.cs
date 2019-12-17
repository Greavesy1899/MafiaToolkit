using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using ResourceTypes.FrameResource;
using SharpDX;
using Utils.Types;
using Mafia2Tool;
using System;

namespace Forms.Docking
{
    public partial class DockSceneTree : DockContent
    {
        public DockSceneTree()
        {
            InitializeComponent();
        }

        public void AddToTree(TreeNode newNode, TreeNode parentNode = null)
        {
            newNode.Checked = true;
            ApplyImageIndex(newNode);
            RecurseChildren(newNode);

            if (parentNode != null)
                parentNode.Nodes.Add(newNode);
            else
                treeView1.Nodes.Add(newNode);
        }

        private void RecurseChildren(TreeNode node)
        {
            foreach (TreeNode child in node.Nodes)
            {
                child.Checked = true;
                ApplyImageIndex(child);
                RecurseChildren(child);
            }
        }

        private void ApplyImageIndex(TreeNode node)
        {
            if (node.Tag == null)
            {
                node.ImageIndex = 7;
                return;
            }

            if (node.Tag.GetType() == typeof(FrameObjectJoint))
                node.SelectedImageIndex = node.ImageIndex = 7;
            else if (node.Tag.GetType() == typeof(FrameObjectSingleMesh))
                node.SelectedImageIndex = node.ImageIndex = 6;
            else if (node.Tag.GetType() == typeof(FrameObjectFrame))
                node.SelectedImageIndex = node.ImageIndex = 0;
            else if (node.Tag.GetType() == typeof(FrameObjectLight))
                node.SelectedImageIndex = node.ImageIndex = 5;
            else if (node.Tag.GetType() == typeof(FrameObjectCamera))
                node.SelectedImageIndex = node.ImageIndex = 2;
            else if (node.Tag.GetType() == typeof(FrameObjectComponent_U005))
                node.SelectedImageIndex = node.ImageIndex = 7;
            else if (node.Tag.GetType() == typeof(FrameObjectSector))
                node.SelectedImageIndex = node.ImageIndex = 7;
            else if (node.Tag.GetType() == typeof(FrameObjectDummy))
                node.SelectedImageIndex = node.ImageIndex = 10;
            else if (node.Tag.GetType() == typeof(FrameObjectDeflector))
                node.SelectedImageIndex = node.ImageIndex = 7;
            else if (node.Tag.GetType() == typeof(FrameObjectArea))
                node.SelectedImageIndex = node.ImageIndex = 1;
            else if (node.Tag.GetType() == typeof(FrameObjectTarget))
                node.SelectedImageIndex = node.ImageIndex = 7;
            else if (node.Tag.GetType() == typeof(FrameObjectModel))
                node.SelectedImageIndex = node.ImageIndex = 9;
            else if (node.Tag.GetType() == typeof(FrameObjectCollision))
                node.SelectedImageIndex = node.ImageIndex = 3;
            else if (node.Tag.GetType() == typeof(ResourceTypes.Collisions.Collision.Placement))
                node.SelectedImageIndex = node.ImageIndex = 4;
            else if (node.Tag.GetType() == typeof(FrameHeaderScene))
                node.SelectedImageIndex = node.ImageIndex = 8;
            else if ((node.Tag is string) && ((node.Tag as string) == "Folder"))
                node.SelectedImageKey = node.ImageKey = "SceneObject.png";
            else
                node.SelectedImageIndex = node.ImageIndex = 7;
        }

        private void OpenEntryContext(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EntryMenuStrip.Items[0].Visible = false;
            EntryMenuStrip.Items[1].Visible = false;
            EntryMenuStrip.Items[2].Visible = false;
            EntryMenuStrip.Items[3].Visible = false;
            EntryMenuStrip.Items[4].Visible = false;

            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {

                EntryMenuStrip.Items[1].Visible = true;
                EntryMenuStrip.Items[2].Visible = true;

                if (FrameResource.IsFrameType(treeView1.SelectedNode.Tag) ||
                    treeView1.SelectedNode.Tag.GetType() == typeof(ResourceTypes.Collisions.Collision.Placement) ||
                    treeView1.SelectedNode.Tag.GetType() == typeof(Rendering.Graphics.RenderJunction))
                {
                    EntryMenuStrip.Items[0].Visible = true;
                }
                if ((treeView1.SelectedNode.Tag.GetType() == typeof(FrameObjectSingleMesh) || 
                    treeView1.SelectedNode.Tag.GetType() == typeof(FrameObjectModel) ||                   
                    treeView1.SelectedNode.Tag.GetType() == typeof(ResourceTypes.Collisions.Collision.CollisionModel)))
                {
                    EntryMenuStrip.Items[3].Visible = true;
                }

                if (FrameResource.IsFrameType(treeView1.SelectedNode.Tag))
                {
                    EntryMenuStrip.Items[4].Visible = true;
                }
            }
        }

        public Vector3 JumpToHelper()
        {
            object data = treeView1.SelectedNode.Tag;

            if (FrameResource.IsFrameType(data))
            {
                bool fin = false;
                TransformMatrix matrix = (data as FrameObjectBase).Matrix;
                TreeNode curNode = treeView1.SelectedNode;
                while (!fin)
                {
                    if (curNode.Parent == null)
                    {
                        fin = true;
                    }
                    else
                    {
                        FrameObjectBase parent = (curNode.Parent.Tag as FrameObjectBase);
                        curNode = curNode.Parent;
                        if (parent != null)
                            matrix += parent.Matrix;
                    }

                }
                return matrix.Position;
            }

            if(data.GetType() == typeof(ResourceTypes.Collisions.Collision.Placement))
                return (data as ResourceTypes.Collisions.Collision.Placement).Position;

            if(data.GetType() == typeof(Rendering.Graphics.RenderJunction))
                return (data as Rendering.Graphics.RenderJunction).Data.Position;

            return new Vector3(0, 0, 0);
        }

        private void OnDoubleClick(object sender, System.EventArgs e)
        {
            var localPosition = treeView1.PointToClient(Cursor.Position);
            var hitTestInfo = treeView1.HitTest(localPosition);
            if (hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
                return;
        }

        private void UpdateParent1Pressed(object sender, System.EventArgs e)
        {
            ModifyParents(0);
        }

        private void UpdateParent2Pressed(object sender, System.EventArgs e)
        {
            ModifyParents(1);
        }

        private void ModifyParents(int parent)
        {
            FrameObjectBase obj = treeView1.SelectedNode.Tag as FrameObjectBase;
            ListWindow window = new ListWindow();
            window.PopulateForm(parent);

            if (window.ShowDialog() == DialogResult.OK)
            {
                int refID = (window.chosenObject as FrameEntry).RefID;
                obj.IsOnFrameTable = true;

                if (parent == 0)
                {
                    obj.ParentIndex1.Name = window.chosenObject.ToString();
                    obj.ParentIndex1.Index = SceneData.FrameResource.GetIndexOfObject(refID);
                    obj.ParentIndex1.RefID = refID;
                    obj.SubRef(FrameEntryRefTypes.Parent1);
                    obj.AddRef(FrameEntryRefTypes.Parent1, refID);
                }
                else if(parent == 1)
                {
                    obj.ParentIndex2.Name = window.chosenObject.ToString();
                    obj.ParentIndex2.Index = SceneData.FrameResource.GetIndexOfObject(refID) + SceneData.FrameResource.FrameScenes.Count;
                    obj.ParentIndex2.RefID = refID;
                    obj.SubRef(FrameEntryRefTypes.Parent2);
                    obj.AddRef(FrameEntryRefTypes.Parent2, refID);
                }
                  
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                TreeNode newNode = new TreeNode();
                newNode.Tag = obj;
                newNode.Name = obj.RefID.ToString();
                newNode.Text = obj.ToString();

                if (obj.ParentIndex1.Index != -1)
                {
                    TreeNode[] nodes = treeView1.Nodes.Find(obj.ParentIndex1.RefID.ToString(), true);

                    if (nodes.Length > 0)
                        AddToTree(newNode, nodes[0]);
                }
                else if (obj.ParentIndex2.Index != -1)
                {
                    TreeNode[] nodes = treeView1.Nodes.Find(obj.ParentIndex2.RefID.ToString(), true);

                    if (nodes.Length > 0)
                        AddToTree(newNode, nodes[0]);
                }
                else
                {
                }
            }
        }
    }
}
