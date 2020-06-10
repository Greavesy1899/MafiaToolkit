using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using ResourceTypes.FrameResource;
using SharpDX;
using System;
using Utils.Extensions;

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
            else if (node.Tag.GetType() == typeof(FrameHeader))
                node.SelectedImageKey = node.ImageKey = "SceneObject.png";
            else if ((node.Tag is string) && ((node.Tag as string) == "Folder"))
                node.SelectedImageKey = node.ImageKey = "SceneObject.png";
            else
                node.SelectedImageIndex = node.ImageIndex = 7;
        }

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
                    data.GetType() == typeof(ResourceTypes.Actors.ActorEntry) || data.GetType() == typeof(Rendering.Graphics.RenderNav))
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
            object data = treeView1.SelectedNode.Tag;

            if (FrameResource.IsFrameType(data))
            {
                return (data as FrameObjectBase).WorldTransform.TranslationVector;
            }

            if(data.GetType() == typeof(ResourceTypes.Collisions.Collision.Placement))
                return (data as ResourceTypes.Collisions.Collision.Placement).Position;

            if(data.GetType() == typeof(Rendering.Graphics.RenderJunction))
                return (data as Rendering.Graphics.RenderJunction).Data.Position;

            if (data.GetType() == typeof(Rendering.Graphics.RenderNav))
                return (data as Rendering.Graphics.RenderNav).BoundingBox.Transform.TranslationVector;

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
