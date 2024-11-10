using Mafia2Tool;
using Mafia2Tool.Forms;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using System;
using System.Collections.Generic;
using System.Numerics;
using ResourceTypes.Translokator;
using Utils.Language;
using Utils.VorticeUtils;
using WeifenLuo.WinFormsUI.Docking;

namespace Forms.Docking
{
    public partial class DockPropertyGrid : DockContent
    {
        private bool isMaterialTabFocused;
        private bool hasLoadedMaterials;
        private object currentObject;
        private TextureEntry currentEntry;
        private Dictionary<TextureEntry, MaterialStruct> currentMaterials;

        public bool IsEntryReady;

        public event EventHandler<EventArgs> OnObjectUpdated;

        public DockPropertyGrid()
        {
            InitializeComponent();
            Localise();
            currentObject = null;
            currentEntry = null;
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
            currentEntry = null;
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
                            IMaterial material = MaterialsManager.LookupMaterialByHash(mat.MaterialHash);

                            textEntry.OnEntrySingularClick += MatViewPanel_TextureEntryOnSingularClick;
                            textEntry.OnEntryDoubleClick += MatViewPanel_TextureEntryOnDoubleClick;
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
                Vector3 position = Vector3.Zero;
                Quaternion rotation2 = Quaternion.Identity;
                Vector3 scale = Vector3.Zero;
                Matrix4x4.Decompose(fObject.LocalTransform, out scale, out rotation2, out position);

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
            else if (currentObject is Instance)
            {
                Instance instance = (currentObject as Instance);
                CurrentEntry.Text = instance.ID.ToString();
                PositionXNumeric.Value = Convert.ToDecimal(instance.Position.X);
                PositionYNumeric.Value = Convert.ToDecimal(instance.Position.Y);
                PositionZNumeric.Value = Convert.ToDecimal(instance.Position.Z);
                RotationXNumeric.Value = Convert.ToDecimal(instance.Rotation.X);//redo this with quat later?
                RotationYNumeric.Value = Convert.ToDecimal(instance.Rotation.Y);
                RotationZNumeric.Value = Convert.ToDecimal(instance.Rotation.Z);
                ScaleXNumeric.Value = ScaleYNumeric.Value = ScaleZNumeric.Value = Convert.ToDecimal(instance.Scale);
                ScaleYNumeric.Enabled = ScaleZNumeric.Enabled = false;
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
                    fObject.LocalTransform = MatrixUtils.SetMatrix(rotation, scale, position);
                }
                else if (currentObject is ResourceTypes.Collisions.Collision.Placement)
                {
                    ResourceTypes.Collisions.Collision.Placement placement = (currentObject as ResourceTypes.Collisions.Collision.Placement);
                    placement.Position = position;
                    placement.RotationDegrees = rotation;
                }
                else if (currentObject is Instance)
                {
                    Instance instance = (currentObject as Instance);
                    instance.Position = position;
                    instance.Rotation = rotation;
                    instance.Scale = scale.X;
                }
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            hasLoadedMaterials = false;
            LoadMaterials();
        }

        private void MatViewPanel_TextureEntryOnDoubleClick(object sender, EventArgs e)
        {
            // Get our entry
            TextureEntry Entry = (sender as TextureEntry);

            // Create our browser; once the user has finished with this menu they should? have a material.
            string MaterialName = "";
            IMaterial OurMaterial = Entry.GetMaterial();
            if(OurMaterial != null)
            {
                MaterialName = OurMaterial.GetMaterialName();
            }

            MaterialBrowser Browser = new MaterialBrowser(MaterialName);
            IMaterial SelectedMaterial = Browser.GetSelectedMaterial();

            // Set the new material data, notify the map editor that a change has been made.
            if (SelectedMaterial != null)
            {
                currentMaterials[Entry].MaterialName = SelectedMaterial.GetMaterialName();
                currentMaterials[Entry].MaterialHash = SelectedMaterial.GetMaterialHash();
                Entry.SetMaterial(SelectedMaterial);
                OnObjectUpdated(sender, e);
            }

            // Yeet the browser into the shadow realm.
            Browser.Dispose();
            Browser = null;
            Entry.IsSelected = false;
        }

        void MatViewPanel_TextureEntryOnSingularClick(object sender, EventArgs e)
        {
            // Set IsSelected for all UCs in the FlowLayoutPanel to false. 
            // Add the new selected one
            TextureEntry Entry = (sender as TextureEntry);

            // Remove the previous entry
            if (currentEntry != null)
            {
                currentEntry.IsSelected = false;
            }

            currentEntry = Entry;
        }

        private void MainTabControl_OnTabIndexChanged(object sender, EventArgs e)
        {
            isMaterialTabFocused = (MainTabControl.SelectedIndex == 2);

            if (currentObject != null)
            {
                LoadMaterials();
            }
        }

        private void ObjectHasUpdated(object sender, EventArgs e)
        {
            OnObjectUpdated(this, EventArgs.Empty);
        }
    }
}
