using Mafia2Tool;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using SharpDX;
using System;
using Mafia2Tool.Forms;
using Utils.Language;
using WeifenLuo.WinFormsUI.Docking;
using Utils.SharpDXExtensions;
using System.Collections.Generic;

namespace Forms.Docking
{
    public partial class DockPropertyGrid : DockContent
    {
        private bool isMaterialTabFocused;
        private bool hasLoadedMaterials;
        private object currentObject;
        private Dictionary<TextureEntry, MaterialStruct> currentMaterials;

        public bool IsEntryReady;

        public event EventHandler<EventArgs> OnObjectUpdated;

        public DockPropertyGrid()
        {
            InitializeComponent();
            Localise();
            currentObject = null;
            IsEntryReady = false;
            isMaterialTabFocused = false;
            hasLoadedMaterials = false;
            currentMaterials = new Dictionary<TextureEntry, MaterialStruct>();
        }

        private void Localise()
        {
            MainTabControl.TabPages[0].Text = Language.GetString("$PROPERTY_GRID");
            MainTabControl.TabPages[1].Text = Language.GetString("$EDIT_TRANSFORM");
            PositionXLabel.Text = Language.GetString("$POSITION_X");
            PositionYLabel.Text = Language.GetString("$POSITION_Y");
            PositionZLabel.Text = Language.GetString("$POSITION_Z");
            RotationXLabel.Text = Language.GetString("$ROTATION_X");
            RotationYLabel.Text = Language.GetString("$ROTATION_Y");
            RotationZLabel.Text = Language.GetString("$ROTATION_Z");
            ScaleXLabel.Text = Language.GetString("$SCALE_X");
            ScaleYLabel.Text = Language.GetString("$SCALE_Y");
            ScaleZLabel.Text = Language.GetString("$SCALE_Z");
        }

        public void SetObject(object obj)
        {
            currentObject = obj;
            SetTransformEdit();
            SetMaterialTab();
            SetPropertyGrid();
        }

        private void SetMaterialTab()
        {
            hasLoadedMaterials = false;
            LODComboBox.Items.Clear();
            if (FrameResource.IsFrameType(currentObject))
            {
                if (currentObject is FrameObjectSingleMesh)
                {
                    var entry = (currentObject as FrameObjectSingleMesh);
                    for (int i = 0; i != entry.Geometry.NumLods; i++)
                    {
                        LODComboBox.Items.Add("LOD #" + i);
                    }
                    LODComboBox.SelectedIndex = 0;
                    LoadMaterials();
                }
            }
        }

        private void LoadMaterials()
        {
            if (isMaterialTabFocused && !hasLoadedMaterials)
            {
                MatViewPanel.Controls.Clear();
                currentMaterials.Clear();
                if (FrameResource.IsFrameType(currentObject))
                {
                    if (currentObject is FrameObjectSingleMesh)
                    {
                        var entry = (currentObject as FrameObjectSingleMesh);
                        MaterialStruct[] materialAssignments = entry.Material.Materials[LODComboBox.SelectedIndex];
                        for (int x = 0; x != materialAssignments.Length; x++)
                        {
                            TextureEntry textEntry = new TextureEntry();

                            var mat = materialAssignments[x];
                            Material material = MaterialsManager.LookupMaterialByHash(mat.MaterialHash);

                            textEntry.WasClicked += MatViewerPanel_WasClicked;
                            textEntry.SetMaterial(material);

                            currentMaterials.Add(textEntry, mat);
                            MatViewPanel.Controls.Add(textEntry);
                        }
                    }
                }
                hasLoadedMaterials = true;
            }
        }

        private void SetTransformEdit()
        {
            IsEntryReady = false;
            if (FrameResource.IsFrameType(currentObject))
            {
                FrameObjectBase fObject = (currentObject as FrameObjectBase);
                Vector3 position;
                Quaternion rotation2;
                Vector3 scale;
                fObject.LocalTransform.Decompose(out scale, out rotation2, out position);

                CurrentEntry.Text = fObject.Name.ToString();
                PositionXNumeric.Value = Convert.ToDecimal(position.X);
                PositionYNumeric.Value = Convert.ToDecimal(position.Y);
                PositionZNumeric.Value = Convert.ToDecimal(position.Z);

                Vector3 rotation = rotation2.ToEuler();
                RotationXNumeric.Value = Convert.ToDecimal(rotation.X);
                RotationYNumeric.Value = Convert.ToDecimal(rotation.Y);
                RotationZNumeric.Value = Convert.ToDecimal(rotation.Z);
                ScaleXNumeric.Enabled = ScaleYNumeric.Enabled = ScaleZNumeric.Enabled = true;
                ScaleXNumeric.Value = Convert.ToDecimal(scale.X);
                ScaleYNumeric.Value = Convert.ToDecimal(scale.Y);
                ScaleZNumeric.Value = Convert.ToDecimal(scale.Z);
            }
            else if (currentObject is ResourceTypes.Collisions.Collision.Placement)
            {
                ResourceTypes.Collisions.Collision.Placement placement = (currentObject as ResourceTypes.Collisions.Collision.Placement);
                CurrentEntry.Text = placement.Hash.ToString();
                PositionXNumeric.Value = Convert.ToDecimal(placement.Position.X);
                PositionYNumeric.Value = Convert.ToDecimal(placement.Position.Y);
                PositionZNumeric.Value = Convert.ToDecimal(placement.Position.Z);
                Vector3 placementRotation = placement.RotationDegrees;
                RotationXNumeric.Value = Convert.ToDecimal(placementRotation.X);
                RotationYNumeric.Value = Convert.ToDecimal(placementRotation.Y);
                RotationZNumeric.Value = Convert.ToDecimal(placementRotation.Z);
                ScaleXNumeric.Value = ScaleYNumeric.Value = ScaleZNumeric.Value = 0.0M;
                ScaleXNumeric.Enabled = ScaleYNumeric.Enabled = ScaleZNumeric.Enabled = false;
            }
            IsEntryReady = true;
        }

        private void SetPropertyGrid()
        {
            PropertyGrid.SelectedObject = currentObject;
        }

        public void UpdateObject()
        {
            if (IsEntryReady && currentObject != null)
            {
                Vector3 position = new Vector3(Convert.ToSingle(PositionXNumeric.Value), Convert.ToSingle(PositionYNumeric.Value), Convert.ToSingle(PositionZNumeric.Value));
                Vector3 rotation = new Vector3(Convert.ToSingle(RotationXNumeric.Value), Convert.ToSingle(RotationYNumeric.Value), Convert.ToSingle(RotationZNumeric.Value));
                Vector3 scale = new Vector3(Convert.ToSingle(ScaleXNumeric.Value), Convert.ToSingle(ScaleYNumeric.Value), Convert.ToSingle(ScaleZNumeric.Value));

                if (FrameResource.IsFrameType(currentObject))
                {
                    FrameObjectBase fObject = (currentObject as FrameObjectBase);
                    fObject.LocalTransform = MatrixExtensions.SetMatrix(rotation, scale, position);
                }
                else if (currentObject is ResourceTypes.Collisions.Collision.Placement)
                {
                    ResourceTypes.Collisions.Collision.Placement placement = (currentObject as ResourceTypes.Collisions.Collision.Placement);
                    placement.Position = position;
                    placement.RotationDegrees = rotation;
                }
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            hasLoadedMaterials = false;
            LoadMaterials();
        }

        void MatViewerPanel_WasClicked(object sender, EventArgs e)
        {
            // Set IsSelected for all UCs in the FlowLayoutPanel to false. 
            MatBrowser browser = null;
            foreach (var c in MatViewPanel.Controls)
            {
                TextureEntry entry = (c as TextureEntry);
                if (entry != null)
                {
                    if (entry.IsSelected)
                    {
                        browser = new MatBrowser();
                        Material selectedMaterial = browser.GetSelectedMaterial();

                        if (selectedMaterial != null)
                        {
                            currentMaterials[entry].MaterialName = selectedMaterial.MaterialName;
                            currentMaterials[entry].MaterialHash = selectedMaterial.MaterialHash;
                            entry.SetMaterial(selectedMaterial);
                            OnObjectUpdated(sender, e);
                        }

                        browser.Dispose();
                        browser = null;
                    }

                    entry.IsSelected = false;
                }
            }
        }

        private void MainTabControl_OnTabIndexChanged(object sender, EventArgs e)
        {
            isMaterialTabFocused = (MainTabControl.SelectedIndex == 2);
            LoadMaterials();
        }

        private void ObjectHasUpdated(object sender, EventArgs e)
        {
            OnObjectUpdated(sender, e);
        }
    }
}
