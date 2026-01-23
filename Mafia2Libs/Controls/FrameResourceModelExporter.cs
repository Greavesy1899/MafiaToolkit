using ResourceTypes.ModelHelpers.ModelExporter;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Utils.Language;
using Utils.Models;

namespace Forms.EditorControls
{
    public partial class FrameResourceModelExporter : Form
    {
        // Generic Variables (fields)
        private MT_ObjectBundle CurrentBundle;
        private MT_ValidationTracker TrackerObject;
        private IImportHelper CurrentHelper;

        public FrameResourceModelExporter(ModelWrapper Wrapper)
        {
            InitializeComponent();
            InitializeControls();

            MT_Object Model = Wrapper.ModelObject;
            TreeView_Objects.Nodes.Add(ConvertObjectToNode(Model));

            // Create a bundle to make it easier to validate
            CurrentBundle = new MT_ObjectBundle();
            CurrentBundle.Objects = new MT_Object[1];
            CurrentBundle.Objects[0] = Model;

            InitiateValidation();
        }

        public FrameResourceModelExporter(MT_ObjectBundle ObjectBundle)
        {
            InitializeComponent();
            InitializeControls();

            CurrentBundle = ObjectBundle;

            InitiateValidation();

            TreeView_Objects.Nodes.Add(ConvertBundleToNode(ObjectBundle));
        }

        private void InitializeControls()
        {
            Localise();
        }

        private void Localise()
        {
            Button_Validate.Text = Language.GetString("$VALIDATE");
            Button_Continue.Text = Language.GetString("$CONTINUE");
            Button_StopImport.Text = Language.GetString("$STOP");
            ImportAOBox.Text = Language.GetString("$IMPORT_AO");
            TabPage_Model.Text = Language.GetString("$TAB_MODEL");
            TabPage_Validation.Text = Language.GetString("$TAB_VALIDATION");
            TabPage_ConvertLogs.Text = Language.GetString("$TAB_IMPORT_LOG");
            Text = Language.GetString("$TITLE_EXPORT_MODEL_BUNDLE");
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

            if (Object.ObjectFlags.HasFlag(MT_ObjectFlags.HasSkinning))
            {
                if (Object.Skeleton != null)
                {
                    TreeNode SkeletonNode = new TreeNode("Skeleton");
                    SkeletonNode.Tag = Object.Skeleton;
                    ValidateObject(SkeletonNode);
                    Root.Nodes.Add(SkeletonNode);
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

            // Create a Helper based on the object we have selected.
            IImportHelper NewHelper = null;
            if (e.Node.Tag is MT_Lod)
            {
                NewHelper = new MT_LodHelper((MT_Lod)e.Node.Tag);
            }
            else if (e.Node.Tag is MT_Object)
            {
                NewHelper = new MT_ObjectHelper((MT_Object)e.Node.Tag);
            }
            else if (e.Node.Tag is MT_Collision)
            {
                NewHelper = new MT_CollisionHelper((MT_Collision)e.Node.Tag);
            }
            else if (e.Node.Tag is MT_Skeleton)
            {
                NewHelper = new MT_SkeletonHelper((MT_Skeleton)e.Node.Tag);
            }

            // Then set it up from the object we pass into the helper,
            // and continue by caching it and assigning onto the property grid.
            // This is guarded as some items (like the bundle node) does not have a helper.
            if (NewHelper != null)
            {
                NewHelper.Setup();
                CurrentHelper = NewHelper;
                PropertyGrid_Model.SelectedObject = NewHelper;
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

            Label_DebugMessage.Text = string.Format("[MESSAGE COUNT: {0}]", TrackerObject.GetMessageCount());
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

            PropertyGrid_Model.SelectedObject = null;
            TreeView_Objects.Nodes.Clear();

            TreeView_Objects.Nodes.Add(ConvertBundleToNode(CurrentBundle));
        }

        private void PropertyGrid_Model_OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Make sure we are selecting an object
            TreeNode Selected = TreeView_Objects.SelectedNode;
            if (Selected == null)
            {
                return;
            }

            // We want to update the object.
            // This will essentially move all properties from the helper
            // directly into the object. This is used to avoid bloat on the 
            // property grid.
            if (CurrentHelper != null)
            {
                CurrentHelper.Store();
            }
        }

        private void HelperContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HelperContextMenu.Items.Clear();

            if (TreeView_Objects.SelectedNode == null)
            {
                // nothing selected
                e.Cancel = true;
                return;
            }

            if (CurrentHelper == null)
            {
                // no valid helper
                e.Cancel = true;
                return;
            }

            // Let the helper add their own context items
            string[] HelperItems = CurrentHelper.GetContextItems();
            foreach(string Item in HelperItems)
            {
                HelperContextMenu.Items.Add(Item);
            }

            // cancel the context strip menu if we don't even have items
            if (HelperContextMenu.Items.Count == 0)
            {
                e.Cancel = true;
            }
        }

        private void HelperContextMenu_OnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(CurrentHelper == null)
            {
                return;
            }

            // The Current helper might want to chime in
            CurrentHelper.OnContextItemSelected(e.ClickedItem.Text);
        }
    }
}
