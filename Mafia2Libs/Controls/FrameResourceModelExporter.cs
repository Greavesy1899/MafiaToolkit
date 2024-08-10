using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.Materials;
using ResourceTypes.ModelHelpers.ModelExporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Utils.Language;
using Utils.Models;
using Utils.Settings;

namespace Forms.EditorControls
{
    public partial class FrameResourceModelExporter : Form
    {
        // Generic Variables (fields)
        private MT_ObjectBundle CurrentBundle;
        private MT_ValidationTracker TrackerObject;
        private IImportHelper CurrentHelper;

        // Material Tab (Fields)
        private string[] ComboBox_LibraryEntries = null;
        private MT_MaterialHelper CurrentMatHelper = null;
        private Dictionary<ulong, bool> CachedMaterialsExistence = null;
        private Dictionary<ulong, MT_MaterialHelper> ModifiedMatHelpers = null;

        // Material Tab (Properties)
        public List<MaterialAddRequestParams> NewMaterials { get; private set; }

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
            // Material tab
            ComboBox_ChoosePreset.Items.Clear();
            ComboBox_ChooseLibrary.Items.Clear();

            ComboBox_ChoosePreset.DataSource = Enum.GetValues(typeof(MaterialPreset));

            Game CurrentGame = GameStorage.Instance.GetSelectedGame();
            string MaterialList = CurrentGame.Materials;
            string[] SplitList = MaterialList.Split(',');

            // Generate array for ComboBox_ChooseLibrary
            List<string> ComboEntries = new List<string>();
            for(int i = 0; i < SplitList.Length; i++)
            {
                // Should 'fingers crossed' be the last entry
                if(string.IsNullOrEmpty(SplitList[i]))
                {
                    continue;
                }

                ComboBox_ChooseLibrary.Items.Add(SplitList[i]);
                ComboEntries.Add(SplitList[i]);
            }

            // Make sure to cache for later
            ComboBox_LibraryEntries = ComboEntries.ToArray();

            // Material Defines
            ModifiedMatHelpers = new Dictionary<ulong, MT_MaterialHelper>();
            CachedMaterialsExistence = new Dictionary<ulong, bool>();
            NewMaterials = new List<MaterialAddRequestParams>();

            Localise();
        }

        private void Localise()
        {
            Button_Validate.Text = Language.GetString("$VALIDATE");
            Button_Continue.Text = Language.GetString("$CONTINUE");
            Button_StopImport.Text = Language.GetString("$STOP");
            TabPage_Model.Text = Language.GetString("$TAB_MODELS");
            TabPage_Material.Text = Language.GetString("$TAB_MATERIALS");
            TabPage_Validation.Text = Language.GetString("$TAB_VALIDATION");
            TabPage_ConvertLogs.Text = Language.GetString("$TAB_IMPORT_LOG");
            Label_ChooseLibrary.Text = Language.GetString("$LABEL_MT_CHOOSE_LIBRARY");
            Label_ChoosePreset.Text = Language.GetString("$LABEL_MT_CHOOSE_PRESET");
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

            GenerateMaterialList();

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

            if(CurrentMatHelper != null)
            {
                SaveMaterialChanges(CurrentMatHelper);
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
            if(CurrentHelper != null)
            {
                CurrentHelper.Store();
            }
        }

        //~ BEGIN MATERIAL MANAGEMENT CODE
        private void SaveMaterialChanges(MT_MaterialHelper MatHelper)
        {
            CurrentMatHelper.LibraryIndex = ComboBox_ChoosePreset.SelectedIndex;
            CurrentMatHelper.SetPreset((MaterialPreset)ComboBox_ChooseLibrary.SelectedIndex);

            // If not present and obviously attempted to be modified, 
            // push into the dictionary so we can get it later.
            if (!ModifiedMatHelpers.ContainsKey(CurrentMatHelper.Hash))
            {
                ModifiedMatHelpers.Add(CurrentMatHelper.Hash, CurrentMatHelper);
            }
            else
            {
                // Just to make sure C# doesn't do anything wacky like not keeping changes.
                ModifiedMatHelpers[CurrentMatHelper.Hash] = CurrentMatHelper;
            }
        }

        private bool DoesMaterialExistAlready(MT_MaterialInstance Instance)
        {
            // Use Cached version first
            ulong Hash = FNV64.Hash(Instance.Name);
            if (CachedMaterialsExistence.ContainsKey(Hash))
            {
                return true;
            }
            else
            {
                // Then try and look ourselves
                IMaterial FoundMaterial = MaterialsManager.LookupMaterialByName(Instance.Name);
                if (FoundMaterial != null)
                {
                    CachedMaterialsExistence.Add(Hash, true);
                    return true;
                }
            }

            return false;
        }

        private void GenerateMaterialList()
        {
            Dictionary<ulong, MaterialAddRequestParams> NewMaterialDictionary = new Dictionary<ulong, MaterialAddRequestParams>();

            // Setup a visitor to look through the entire bundle.
            // After it has completed we can then iterate through the whole dictioanry
            MT_MaterialCollectorVisitor MatCollectionVisitor = new MT_MaterialCollectorVisitor();
            CurrentBundle.Accept(MatCollectionVisitor);

            // Iterate through all collected materials.
            // Do the steps in this order:
            // 1. if exists, skip. Cannot and will not allow material changes.
            // 2. if modified and in dictioanry, favour that rather than the one in MT_MaterialInstance.
            // 3. fallback on the one in MT_MaterialInstance, nothing is wrong with this, just not edited.
            foreach (KeyValuePair<ulong, MT_MaterialInstance> MatPair in MatCollectionVisitor.Materials)
            {
                // 1. ....
                if(DoesMaterialExistAlready(MatPair.Value))
                {
                    continue;
                }

                // 2. ....
                if(ModifiedMatHelpers.ContainsKey(MatPair.Key))
                {
                    MT_MaterialHelper Helper = ModifiedMatHelpers[MatPair.Key];
                    MaterialAddRequestParams NewParams = new MaterialAddRequestParams(Helper.Material, ComboBox_LibraryEntries[Helper.LibraryIndex]);
                    NewMaterialDictionary.TryAdd(MatPair.Key, NewParams);

                    continue;
                }

                // 3....
                if (!NewMaterialDictionary.ContainsKey(MatPair.Key))
                {
                    // Worth mentioning here that it will use the default, and probably go into an undesirable MTL.
                    MT_MaterialHelper MatHelper = new MT_MaterialHelper(MatPair.Value);
                    MatHelper.Setup();

                    MaterialAddRequestParams NewParams = new MaterialAddRequestParams(MatHelper.Material, ComboBox_LibraryEntries[MatHelper.LibraryIndex]);
                    NewMaterialDictionary.TryAdd(MatPair.Key, NewParams);

                    continue;
                }
            }

            // Cache our new findings!
            NewMaterials = NewMaterialDictionary.Values.ToList();
        }

        private void TabControl_Editors_TabIndexChanged(object sender, EventArgs e)
        {
            if(TabControl_Editors.SelectedTab == TabPage_Material)
            {
                // Make sure to clear the list.
                ListView_Materials.Items.Clear();

                // Setup a visitor to look through the entire bundle.
                // After it has completed we can then iterate through the whole dictioanry
                MT_MaterialCollectorVisitor MatCollectionVisitor = new MT_MaterialCollectorVisitor();
                CurrentBundle.Accept(MatCollectionVisitor);

                // Iterate through all collected materials.
                // If we find a valid Material, then we can add it to the list as valid.
                // Otherwise it's added as missing.
                foreach(KeyValuePair<ulong, MT_MaterialInstance> MatPair in MatCollectionVisitor.Materials)
                {
                    MT_MaterialInstance MatInstance = MatPair.Value;
                    if(MatInstance.MaterialFlags.HasFlag(MT_MaterialInstanceFlags.IsCollision))
                    {
                        // Skip Collisions
                        continue;
                    }

                    // If material exists then skip - we shouldn't allow any edits
                    if(DoesMaterialExistAlready(MatPair.Value))
                    {
                        // skip, not allowed to edit
                        continue;
                    }

                    // Try to get existing Material Helper.
                    MT_MaterialHelper MatHelper = null;
                    if (!ModifiedMatHelpers.ContainsKey(MatPair.Key))
                    {
                        MatHelper = new MT_MaterialHelper(MatInstance);
                        MatHelper.Setup();
                    }
                    else
                    {
                        MatHelper = ModifiedMatHelpers[MatPair.Key];
                    }

                    // Make the new List Entry and push into list control
                    ListViewItem NewListItem = new ListViewItem();
                    NewListItem.Text = MatInstance.Name;
                    NewListItem.ImageIndex = 0;
                    NewListItem.Checked = true;
                    NewListItem.Name = MatInstance.Name;
                    NewListItem.Tag = MatHelper;

                    ListView_Materials.Items.Add(NewListItem);
                }
            }
        }

        private void ListView_Materials_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Store current values for the current helper
            if(CurrentMatHelper != null)
            {
                // Save changes and clear
                SaveMaterialChanges(CurrentMatHelper);
                CurrentMatHelper = null;
            }

            if (TabControl_Editors.SelectedTab != TabPage_Material)
            {
                // Skip if not on this control, we need to avoid any uneccessary changes for performance
                return;
            }

            if (ListView_Materials.SelectedItems.Count == 0)
            {
                // Skip if nothing is selected
                return;
            }

            // Then update the controls with the recently selected Item.
            ListViewItem Item = ListView_Materials.SelectedItems[0];
            if(Item.Tag != null)
            {
                MT_MaterialHelper MatHelper = Item.Tag as MT_MaterialHelper;
                ComboBox_ChoosePreset.SelectedIndex = MatHelper.LibraryIndex;
                ComboBox_ChooseLibrary.SelectedIndex = (int)MatHelper.Preset;
                PropertyGrid_Material.SelectedObject = MatHelper.Material;

                CurrentHelper = MatHelper;
            }
        }

        private void ComboBox_Preset_SelectionChangeCommitted(object sender, EventArgs e)
        {
            MaterialPreset NewPreset = (MaterialPreset)ComboBox_ChoosePreset.SelectedIndex;

            // Check is guarded, will handle if we need to update material.
            CurrentMatHelper.SetPreset(NewPreset);
            PropertyGrid_Material.SelectedObject = CurrentMatHelper.Material;
        }
    }
}
