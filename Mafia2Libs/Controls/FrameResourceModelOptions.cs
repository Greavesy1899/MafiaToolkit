using ResourceTypes.ModelHelpers.ModelExporter;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Utils.Models;

namespace Forms.EditorControls
{
    public partial class FrameResourceModelOptions : Form
    {
        private MT_ObjectBundle CurrentBundle;
        private MT_ValidationTracker TrackerObject;

        private IImportHelper CurrentHelper;

        public FrameResourceModelOptions(ModelWrapper Wrapper)
        {
            InitializeComponent();

            MT_Object Model = Wrapper.ModelObject;
            TreeView_Objects.Nodes.Add(ConvertObjectToNode(Model));

            // Create a bundle to make it easier to validate
            CurrentBundle = new MT_ObjectBundle();
            CurrentBundle.Objects = new MT_Object[1];
            CurrentBundle.Objects[0] = Model;

            InitiateValidation();
        }

        public FrameResourceModelOptions(MT_ObjectBundle ObjectBundle)
        {
            InitializeComponent();

            CurrentBundle = ObjectBundle;

            InitiateValidation();

            TreeView_Objects.Nodes.Add(ConvertBundleToNode(ObjectBundle));
        }

        private TreeNode ConvertObjectToNode(MT_Object Object)
        {
            TreeNode Root = new TreeNode(Object.ObjectName);
            Root.Tag = Object;
            ValidateObject(Root);

            if (Object.ObjectFlags.HasFlag(MT_ObjectFlags.HasLODs))
            {
                for (int i = 0; i < Object.Lods.Length; i++)
                {
                    TreeNode LodNode = new TreeNode("LOD" + i);
                    LodNode.Tag = Object.Lods[i];
                    ValidateObject(LodNode);
                    Root.Nodes.Add(LodNode);
                }
            }

            if (Object.ObjectFlags.HasFlag(MT_ObjectFlags.HasCollisions))
            {
                TreeNode SCollisionNode = new TreeNode("Static Collision");
                SCollisionNode.Tag = Object.Collision;
                ValidateObject(SCollisionNode);
                Root.Nodes.Add(SCollisionNode);
            }

            if (Object.ObjectFlags.HasFlag(MT_ObjectFlags.HasChildren))
            {
                foreach (MT_Object Child in Object.Children)
                {
                    Root.Nodes.Add(ConvertObjectToNode(Child));
                }
            }

            return Root;
        }

        private TreeNode ConvertBundleToNode(MT_ObjectBundle Bundle)
        {
            TreeNode Root = new TreeNode("Bundle");
            Root.Tag = Bundle;
            ValidateObject(Root);

            foreach (MT_Object ObjectInfo in Bundle.Objects)
            {
                Root.Nodes.Add(ConvertObjectToNode(ObjectInfo));
            }

            return Root;
        }

        private void TreeView_OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex;

            if (e.Node.Tag is MT_Lod)
            {
                MT_LodHelper LodHelper = new MT_LodHelper((MT_Lod)e.Node.Tag);
                LodHelper.Setup();
                CurrentHelper = LodHelper;
                PropertyGrid_Test.SelectedObject = LodHelper;

            }
            else if (e.Node.Tag is MT_Object)
            {
                MT_ObjectHelper ObjectHelper = new MT_ObjectHelper((MT_Object)e.Node.Tag);
                ObjectHelper.Setup();
                CurrentHelper = ObjectHelper;
                PropertyGrid_Test.SelectedObject = ObjectHelper;
            }
            else if (e.Node.Tag is MT_Collision)
            {
                MT_CollisionHelper ColHelper = new MT_CollisionHelper((MT_Collision)e.Node.Tag);
                ColHelper.Setup();
                CurrentHelper = ColHelper;
                PropertyGrid_Test.SelectedObject = ColHelper;
            }

            // Get validation messages for object and add to tab
            ListBox_Validation.Items.Clear();
            List<string> Messages = TrackerObject.GetObjectMessages((IValidator)e.Node.Tag);
            foreach (string Message in Messages)
            {
                ListBox_Validation.Items.Add(Message);
            }
        }

        private void TreeView_OnBeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode Selected = TreeView_Objects.SelectedNode;

            if (Selected == null)
            {
                return;
            }

            if (Selected.Tag is MT_Lod)
            {
                MT_LodHelper LodHelper = (CurrentHelper as MT_LodHelper);
                LodHelper.Store();

                string Message = string.Format("{0} - {1}", DateTime.Now.ToLongTimeString(), "Updated Vertex Flags for LOD");
                //Label_MessageText.Text = Message;
            }
            else if (Selected.Tag is MT_Object)
            {
                MT_ObjectHelper ObjectHelper = (CurrentHelper as MT_ObjectHelper);
                ObjectHelper.Store();

                string Message = string.Format("{0} - Updated Object: {1}", DateTime.Now.ToLongTimeString(), ObjectHelper.ObjectName);
                //Label_MessageText.Text = Message;
            }
            else if (Selected.Tag is MT_Collision)
            {
                MT_CollisionHelper CollisionHelper = (CurrentHelper as MT_CollisionHelper);
                CollisionHelper.Store();

                string Message = string.Format("{0} - {1}", DateTime.Now.ToLongTimeString(), "Updated COL.");
                //Label_MessageText.Text = Message;
            }

            // clear validation box
            ListBox_Validation.Items.Clear();

            // clear helper
            CurrentHelper = null;
        }

        private void ValidateObject(TreeNode CurrentNode)
        {
            IValidator ValidationObject = (CurrentNode.Tag as IValidator);
            if (ValidationObject != null)
            {
                bool bIsValid = TrackerObject.IsObjectValid((IValidator)CurrentNode.Tag);
                CurrentNode.ImageIndex = (bIsValid ? 1 : 0);
            }
        }

        private void InitiateValidation()
        {
            TrackerObject = new MT_ValidationTracker();
            CurrentBundle.ValidateObject(TrackerObject);

            Label_DebugMessage.Text = string.Format("MESSAGE DEBUG: {0}", TrackerObject.GetMessageCount());
        }

        private void Button_Continue_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            CleanupHelper();
            Close();
        }

        private void Button_StopImport_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            CleanupHelper();
            Close();
        }

        private void CleanupHelper()
        {
            if (CurrentHelper != null)
            {
                CurrentHelper.Store();
            }
        }

        private void Button_Validate_Click(object sender, EventArgs e)
        {
            InitiateValidation();

            PropertyGrid_Test.SelectedObject = null;
            TreeView_Objects.Nodes.Clear();

            TreeView_Objects.Nodes.Add(ConvertBundleToNode(CurrentBundle));
        }
    }
}
