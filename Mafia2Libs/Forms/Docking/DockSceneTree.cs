using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using ResourceTypes.FrameResource;
using SharpDX;
using System;

namespace Forms.Docking
{
    public partial class DockSceneTree : DockContent
    {
        public TreeNode SelectedNode {
            get { return treeView1.SelectedNode; }
            set { treeView1.SelectedNode = value; }
        }

        public DockSceneTree()
        {
            InitializeComponent();
        }

        /* Abstract Functions for the outliner */
        public void SetEventHandler(string eventType, TreeViewEventHandler handler)
        {
            if(eventType.Equals("AfterSelect"))
            {
                treeView1.AfterSelect += handler;
            }
            else if (eventType.Equals("AfterCheck"))
            {
                treeView1.AfterCheck += handler;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void RemoveEventHandler(string eventType, TreeViewEventHandler handler)
        {
            if (eventType.Equals("AfterCheck"))
            {
                treeView1.AfterCheck -= handler;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void SetKeyHandler(string eventType, KeyEventHandler handler)
        {
            if (eventType.Equals("KeyUp"))
            {
                treeView1.KeyUp += handler;
            }
            else if (eventType.Equals("KeyDown"))
            {
                treeView1.KeyDown += handler;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public TreeNode[] Find(string key, bool searchAllChildren)
        {
            return treeView1.Nodes.Find(key, searchAllChildren);
        }

        public void RemoveNode(TreeNode node)
        {
            treeView1.Nodes.Remove(node);
        }

        public void AddToTree(TreeNode node, TreeNode parentNode = null)
        {
            node.Checked = true;
            ApplyImageIndex(node);
            RecurseChildren(node);

            if (parentNode != null)
                parentNode.Nodes.Add(node);
            else
                treeView1.Nodes.Add(node);
        }

        public TreeNode GetTreeNode(string TreeNodeKey, TreeNode ParentNode = null, bool bSearchChildren = false)
        {
            // Search for the node
            TreeNode[] AttemptedFoundNodes = null;
            if(ParentNode != null)
            {
                AttemptedFoundNodes = ParentNode.Nodes.Find(TreeNodeKey, bSearchChildren);
            }
            else
            {
                AttemptedFoundNodes = treeView1.Nodes.Find(TreeNodeKey, bSearchChildren);
            }

            // If we have found nodes, then get the first one
            if(AttemptedFoundNodes.Length > 0)
            {
                return AttemptedFoundNodes[0];
            }

            // We have failed, return null.
            return null;
        }

        /* Helper functions */
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
            else if (node.Tag.GetType() == typeof(FrameHeader))
                node.SelectedImageKey = node.ImageKey = "SceneObject.png";
            else if ((node.Tag is string) && ((node.Tag as string) == "Folder"))
                node.SelectedImageKey = node.ImageKey = "SceneObject.png";
            else
                node.SelectedImageIndex = node.ImageIndex = 7;
        }

        /* Context function */
        private void OpenEntryContext(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO: Clean this messy system.
            EntryMenuStrip.Items[0].Visible = false;
            EntryMenuStrip.Items[1].Visible = false;
            EntryMenuStrip.Items[2].Visible = false;
            EntryMenuStrip.Items[3].Visible = false;
            EntryMenuStrip.Items[4].Visible = false;
            FrameActions.DropDownItems[3].Visible = false;

            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null)
            {

                EntryMenuStrip.Items[1].Visible = true;
                EntryMenuStrip.Items[2].Visible = true;

                object data = treeView1.SelectedNode.Tag;
                if (FrameResource.IsFrameType(data) || data.GetType() == typeof(ResourceTypes.Collisions.Collision.Placement) || data.GetType() == typeof(Rendering.Graphics.RenderJunction) ||
                    data.GetType() == typeof(ResourceTypes.Actors.ActorEntry) || data.GetType() == typeof(ResourceTypes.Navigation.OBJData.VertexStruct) || data.GetType().IsSubclassOf(typeof(ResourceTypes.Navigation.IType)))
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

                    if(treeView1.SelectedNode.Tag is FrameObjectFrame)
                    {
                        FrameActions.DropDownItems[3].Visible = true;
                    }
                }
            }
        }

        public Vector3 JumpToHelper()
        {
            TreeNode SelectedNode = treeView1.SelectedNode;
            object data = treeView1.SelectedNode.Tag;

            if (FrameResource.IsFrameType(data))
            {
                return (data as FrameObjectBase).WorldTransform.TranslationVector;
            }

            if(data.GetType() == typeof(ResourceTypes.Collisions.Collision.Placement))
                return (data as ResourceTypes.Collisions.Collision.Placement).Position;

            if(data.GetType() == typeof(Rendering.Graphics.RenderJunction))
                return (data as Rendering.Graphics.RenderJunction).Data.Position;

            if(SelectedNode.Name.Equals("NAV_INDEXED_NODE"))
            {
                TreeNode ParentNode = SelectedNode.Parent;
                if(ParentNode.Name.Equals("NAV_OBJ_DATA"))
                {
                    ResourceTypes.Navigation.OBJData.VertexStruct Vertex = (ResourceTypes.Navigation.OBJData.VertexStruct)data;
                    return Vertex.Position;
                }
            }
            else if(data.GetType().IsSubclassOf(typeof(ResourceTypes.Navigation.IType)))
            {
                ResourceTypes.Navigation.IType AIWorld = (ResourceTypes.Navigation.IType)data;
                return AIWorld.GetPosition();
            }

           // if (data.GetType() == typeof(Rendering.Graphics.RenderNav))
          //      return (data as Rendering.Graphics.RenderNav).NavigationBox.Transform.TranslationVector;

            if (data.GetType() == typeof(ResourceTypes.Actors.ActorEntry))
                return (data as ResourceTypes.Actors.ActorEntry).Position;

            return new Vector3(0, 0, 0);
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            var localPosition = treeView1.PointToClient(Cursor.Position);
            var hitTestInfo = treeView1.HitTest(localPosition);
            if (hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
                return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Button_Filter_Click(object sender, EventArgs e)
        {
        }
    }
}
