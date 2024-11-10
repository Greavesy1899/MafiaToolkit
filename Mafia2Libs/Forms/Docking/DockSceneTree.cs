using ResourceTypes.FrameResource;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using ResourceTypes.Translokator;
using Utils.Language;
using Utils.VorticeUtils;
using WeifenLuo.WinFormsUI.Docking;
using Object = ResourceTypes.Translokator.Object;

namespace Forms.Docking
{
    public partial class DockSceneTree : DockContent
    {
        public TreeNode SelectedNode {
            get { return TreeView_Explorer.SelectedNode; }
            set { TreeView_Explorer.SelectedNode = value; }
        }

        public TreeNode DraggedNode;
        public TreeNode targetNode;

        // Cache the last searched string so we can check if it has changed before we search again.
        private string LastSearchedString = String.Empty;
        
        private MouseButtons dragButton;
        
        public event EventHandler<TreeViewDragEventArgs> TreeViewNodeDropped;

        public DockSceneTree()
        {
            InitializeComponent();

            Localize();
        }

        public void Localize()
        {
            Text = Language.GetString("$SCENE_OUTLINER_FORMNAME");
            TabPage_Explorer.Text = Language.GetString("$SCENE_OUTLINER_EXPLORER");
            TabPage_Searcher.Text = Language.GetString("$SCENE_OUTLINER_SEARCHER");
            string xmlText = Language.GetString("$TOOLTIP");
            string tooltipText = xmlText.Replace("|", Environment.NewLine);
            if (!tooltipText.Equals("$TOOLTIP", StringComparison.InvariantCulture))
            {
                this.tooltipText.Text = tooltipText;
            }
        }

        /* Abstract Functions for the outliner */
        public void SetEventHandler(string eventType, TreeViewEventHandler handler)
        {
            if(eventType.Equals("AfterSelect"))
            {
                TreeView_Explorer.AfterSelect += handler;
            }
            else if (eventType.Equals("AfterCheck"))
            {
                TreeView_Explorer.AfterCheck += handler;
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
                TreeView_Explorer.AfterCheck -= handler;
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
                TreeView_Explorer.KeyUp += handler;
            }
            else if (eventType.Equals("KeyDown"))
            {
                TreeView_Explorer.KeyDown += handler;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        
        private void TreeView_Explorer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
            {
                dragButton = e.Button;
            }
            
            TreeNode clickedNode = TreeView_Explorer.GetNodeAt(e.X, e.Y);
            if (dragButton == MouseButtons.Middle && clickedNode != null)
            {
                TreeView_Explorer.SelectedNode = clickedNode;
                DoDragDrop(clickedNode, DragDropEffects.Move);
            }
        }
        
        private void TreeView_Explorer_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
        
        private void TreeView_Explorer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }
        
        private void TreeView_Explorer_DragDrop(object sender, DragEventArgs e)
        {
            Point pt = TreeView_Explorer.PointToClient(new Point(e.X, e.Y));
            targetNode = TreeView_Explorer.GetNodeAt(TreeView_Explorer.PointToClient(new System.Drawing.Point(e.X, e.Y)));
            DraggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            TreeView_Explorer.SelectedNode = DraggedNode;

            if (targetNode != null && DraggedNode != null && DraggedNode != targetNode)
            {
                TreeViewNodeDropped?.Invoke(this, new TreeViewDragEventArgs(DraggedNode, targetNode, dragButton));
            }
            if (previousNode != null)
            {
                previousNode.BackColor = TreeView_Explorer.BackColor;
                previousNode.ForeColor = TreeView_Explorer.ForeColor;
                previousNode = null;
            }
        }
        
        private TreeNode previousNode;

        private void TreeView_Explorer_DragOver(object sender, DragEventArgs e)
        {
            Point pt = TreeView_Explorer.PointToClient(new Point(e.X, e.Y));
            
            TreeNode currentNode = TreeView_Explorer.GetNodeAt(pt);
            
            if (currentNode != null && currentNode != previousNode)
            {
                if (previousNode != null)
                {
                    previousNode.BackColor = TreeView_Explorer.BackColor;
                    previousNode.ForeColor = TreeView_Explorer.ForeColor;
                }

                if ((dragButton & MouseButtons.Left) != 0)
                {
                    currentNode.BackColor = Color.LightBlue; 
                    currentNode.ForeColor = Color.Black;
                }

                if ((dragButton & MouseButtons.Right) != 0)
                {
                    currentNode.BackColor = Color.Orange; 
                    currentNode.ForeColor = Color.Black;
                }
                
                if ((dragButton & MouseButtons.Middle) != 0)
                {
                    currentNode.BackColor = Color.LimeGreen; 
                    currentNode.ForeColor = Color.Black;
                }
                
                previousNode = currentNode;
            }
            
            e.Effect = DragDropEffects.Move;
        }

        private void TreeView_Explorer_DragLeave(object sender, EventArgs e)
        {
            if (previousNode != null)
            {
                previousNode.BackColor = TreeView_Explorer.BackColor;
                previousNode.ForeColor = TreeView_Explorer.ForeColor;
                previousNode = null;
            }
        }

        public TreeNode[] Find(string key, bool searchAllChildren)
        {
            return TreeView_Explorer.Nodes.Find(key, searchAllChildren);
        }

        public void RemoveNode(TreeNode node)
        {
            TreeView_Explorer.Nodes.Remove(node);
        }

        public void AddToTree(TreeNode node, TreeNode parentNode = null)
        {
            node.Checked = true;
            ApplyImageIndex(node);
            RecurseChildren(node);

            if (parentNode != null)
            {
                parentNode.Nodes.Add(node);
            }
            else
            {
                TreeView_Explorer.Nodes.Add(node);
            }
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
                AttemptedFoundNodes = TreeView_Explorer.Nodes.Find(TreeNodeKey, bSearchChildren);
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
                node.SelectedImageIndex = node.ImageIndex = 8;
            else if ((node.Tag is string) && ((node.Tag as string) == "Folder"))
                node.SelectedImageIndex = node.ImageIndex = 8;
            else if (node.Tag.GetType() == typeof(ObjectGroup))
                node.SelectedImageIndex = node.ImageIndex = 8;
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
            Export3DButton.Visible = false;
            EntryMenuStrip.Items[4].Visible = false;
            FrameActions.DropDownItems[3].Visible = false;

            if (TreeView_Explorer.SelectedNode != null && TreeView_Explorer.SelectedNode.Tag != null)
            {

                EntryMenuStrip.Items[1].Visible = true;
                EntryMenuStrip.Items[2].Visible = true;

                object data = TreeView_Explorer.SelectedNode.Tag;
                if (FrameResource.IsFrameType(data) || data.GetType() == typeof(ResourceTypes.Collisions.Collision.Placement) || data.GetType() == typeof(Rendering.Graphics.RenderJunction) ||
                    data.GetType() == typeof(ResourceTypes.Actors.ActorEntry) || data.GetType() == typeof(Rendering.Graphics.RenderNav))
                {
                    EntryMenuStrip.Items[0].Visible = true;
                }
                if ((TreeView_Explorer.SelectedNode.Tag.GetType() == typeof(ResourceTypes.Collisions.Collision.CollisionModel)) 
                    || (TreeView_Explorer.SelectedNode.Tag.GetType() == typeof(ResourceTypes.FrameResource.FrameHeaderScene)))
                {
                    Export3DButton.Visible = true;
                }
                if(TreeView_Explorer.SelectedNode.Text == "Collision Data")
                {
                    // TODO: Get a better check than the string of a tree node.
                    // It doesn't prove to be very stable.
                    Export3DButton.Visible = true;
                }
                if (FrameResource.IsFrameType(TreeView_Explorer.SelectedNode.Tag))
                {
                    EntryMenuStrip.Items[3].Visible = true;
                    EntryMenuStrip.Items[4].Visible = true;

                    if (TreeView_Explorer.SelectedNode.Tag is FrameObjectFrame)
                    {
                        Export3DButton.Visible = true;
                        LinkToActorButton.Visible = true;
                    }
                }

                if (TreeView_Explorer.SelectedNode.Tag is Instance || TreeView_Explorer.SelectedNode.Tag is Object)
                {
                    EntryMenuStrip.Items[0].Visible = true;
                }
            }
        }

        public Vector3 JumpToHelper()
        {
            object data = TreeView_Explorer.SelectedNode.Tag;

            if (FrameResource.IsFrameType(data))
            {
                return (data as FrameObjectBase).WorldTransform.Translation;
            }
            else if (data.GetType() == typeof(ResourceTypes.Collisions.Collision.Placement))
            {
                return (data as ResourceTypes.Collisions.Collision.Placement).Position;
            }
            else if (data.GetType() == typeof(Rendering.Graphics.RenderNav))
            {
                return (data as Rendering.Graphics.RenderNav).Transform.Translation;
            }
            else if (data.GetType() == typeof(ResourceTypes.Actors.ActorEntry))
            {
                return (data as ResourceTypes.Actors.ActorEntry).Position;
            }
            else if (data is Object objectgroup)
            {
                List<TreeNode> CurrentNodeMatches = new List<TreeNode>();//intented for object to jump to refframe
                SearchNodes(objectgroup.Name.String, TreeView_Explorer.Nodes[0], ref CurrentNodeMatches);
                if (CurrentNodeMatches.Count!=0)//this should look into frames first,if there is none, it doesn't have frame ref
                {
                    if (CurrentNodeMatches[0].Tag is FrameObjectBase refframe)
                    {
                        return refframe.WorldTransform.Translation; 
                    }
                }
            }
            else if (data is Instance instance)//jump to instance coords
            {
                return MatrixUtils.SetMatrix(instance.Quaternion, new Vector3(instance.Scale,instance.Scale,instance.Scale), instance.Position).Translation;
            }

            return Vector3.Zero;
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            Point localPosition = TreeView_Explorer.PointToClient(Cursor.Position);

            TreeViewHitTestInfo hitTestInfo = TreeView_Explorer.HitTest(localPosition);
            if (hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
            {
                return;
            }
        }

        private void InternalSearchExplorer()
        {
            TreeView_Searcher.Nodes.Clear();

            // Avoid searching if LastSearchedString and the search box is equal
            if(LastSearchedString != string.Empty && LastSearchedString.Equals(TextBox_Search.Text))
            {
                return;
            }

            // Search for the node. We expect nodes to be added and that the text box includes text.
            if (TreeView_Explorer.Nodes.Count > 0 && TextBox_Search.TextLength > 0)
            {
                List<TreeNode> CurrentNodeMatches = new List<TreeNode>();
                List<TreeNode> FoundNodes = SearchNodes(TextBox_Search.Text, TreeView_Explorer.Nodes[0], ref CurrentNodeMatches);
                foreach (TreeNode Tree in FoundNodes)
                {
                    TreeNode CloneNode = (TreeNode)Tree.Clone();
                    CloneNode.Nodes.Clear();
                    TreeView_Searcher.Nodes.Add(CloneNode);
                }

                LastSearchedString = TextBox_Search.Text;
            }
        }

        private List<TreeNode> SearchNodes(string SearchText, TreeNode StartNode, ref List<TreeNode> CurrentNodeMatches)
        {
            while (StartNode != null)
            {
                if (StartNode.Text.ToLower().Contains(SearchText.ToLower()))
                {
                    CurrentNodeMatches.Add(StartNode);
                }
                if (StartNode.Nodes.Count != 0)
                {
                    SearchNodes(SearchText, StartNode.Nodes[0], ref CurrentNodeMatches);
                }
                StartNode = StartNode.NextNode;
            };

            return CurrentNodeMatches;
        }

        private void InternalGotoExplorerNode()
        {
            if(TreeView_Searcher.SelectedNode == null)
            {
                // We don't have a selected node to goto.
                return;
            }

            // C# will throw an error if name is null.
            string NodeName = TreeView_Searcher.SelectedNode.Name;
            if (string.IsNullOrEmpty(NodeName))
            {
                string ErrorMessage = string.Format("Selected Node: [{0}] had an invalid name, cannot search in explorer.", TreeView_Searcher.SelectedNode.Text);
                MessageBox.Show(ErrorMessage, "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            // TODO: Is there a way to improve this, in a way we don't have to call the find function again?
            TreeNode[] SearchResult = TreeView_Explorer.Nodes.Find(TreeView_Searcher.SelectedNode.Name, true);
            if (SearchResult.Length > 0)
            {
                TreeView_Explorer.SelectedNode = SearchResult[0];
                Tab_Explorer.SelectedTab = TabPage_Explorer;
            }
        }

        private void TreeView_Searcher_OnDoubleClick(object sender, EventArgs e)
        {
            InternalGotoExplorerNode();
        }

        private void Button_Search_OnClick(object sender, EventArgs e)
        {
            InternalSearchExplorer();
        }

        private void TextBox_Search_OnKeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                InternalSearchExplorer();
            }
        }

        private void TreeView_Searcher_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InternalGotoExplorerNode();
            }
        }
    }
}
public class TreeViewDragEventArgs : EventArgs
{
    public TreeNode DraggedNode { get; } 
    public TreeNode TargetNode { get; } 
    public MouseButtons DragButton { get; }

    public TreeViewDragEventArgs(TreeNode draggedNode, TreeNode targetNode, MouseButtons dragButton)
    {
        DraggedNode = draggedNode;
        TargetNode = targetNode;
        DragButton = dragButton;
    }
}
