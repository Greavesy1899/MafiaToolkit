using Gibbed.Squish;
using Mafia2Tool;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using SharpDX;
using System;
using Mafia2Tool.Forms;
using System.Drawing;
using System.IO;
using Utils.Lang;
using WeifenLuo.WinFormsUI.Docking;

namespace Forms.Docking
{
    public partial class DockPropertyGrid : DockContent
    {
        private object currentObject;
        public bool IsEntryReady;

        public DockPropertyGrid()
        {
            InitializeComponent();
            Localise();
            currentObject = null;
            IsEntryReady = false;
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
            LODComboBox.Items.Clear();
            if (FrameResource.IsFrameType(currentObject))
            {
                if (currentObject is FrameObjectSingleMesh)
                {
                    var entry = (currentObject as FrameObjectSingleMesh);
                    for (int i = 0; i != entry.Geometry.NumLods; i++)
                        LODComboBox.Items.Add("LOD #" + i);
                    LODComboBox.SelectedIndex = 0;
                }
            }
        }

        private void SetTransformEdit()
        {
            IsEntryReady = false;
            if (FrameResource.IsFrameType(currentObject))
            {
                FrameObjectBase fObject = (currentObject as FrameObjectBase);
                CurrentEntry.Text = fObject.Name.String;
                PositionXNumeric.Value = Convert.ToDecimal(fObject.Matrix.Position.X);
                PositionYNumeric.Value = Convert.ToDecimal(fObject.Matrix.Position.Y);
                PositionZNumeric.Value = Convert.ToDecimal(fObject.Matrix.Position.Z);
                RotationXNumeric.Value = Convert.ToDecimal(fObject.Matrix.Rotation.X);
                RotationYNumeric.Value = Convert.ToDecimal(fObject.Matrix.Rotation.Y);
                RotationZNumeric.Value = Convert.ToDecimal(fObject.Matrix.Rotation.Z);
                ScaleXNumeric.Enabled = ScaleYNumeric.Enabled = ScaleZNumeric.Enabled = true;
                ScaleXNumeric.Value = Convert.ToDecimal(fObject.Matrix.Scale.X);
                ScaleYNumeric.Value = Convert.ToDecimal(fObject.Matrix.Scale.Y);
                ScaleZNumeric.Value = Convert.ToDecimal(fObject.Matrix.Scale.Z);
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
                if (FrameResource.IsFrameType(currentObject))
                {
                    FrameObjectBase fObject = (currentObject as FrameObjectBase);
                    fObject.Matrix.Position = new Vector3(Convert.ToSingle(PositionXNumeric.Value), Convert.ToSingle(PositionYNumeric.Value), Convert.ToSingle(PositionZNumeric.Value));
                    fObject.Matrix.SetRotationMatrix(new Vector3(Convert.ToSingle(RotationXNumeric.Value), Convert.ToSingle(RotationYNumeric.Value), Convert.ToSingle(RotationZNumeric.Value)));
                    fObject.Matrix.Scale = new Vector3(Convert.ToSingle(ScaleXNumeric.Value), Convert.ToSingle(ScaleYNumeric.Value), Convert.ToSingle(ScaleZNumeric.Value));
                }
                else if (currentObject is ResourceTypes.Collisions.Collision.Placement)
                {
                    ResourceTypes.Collisions.Collision.Placement placement = (currentObject as ResourceTypes.Collisions.Collision.Placement);
                    placement.Position = new Vector3(Convert.ToSingle(PositionXNumeric.Value), Convert.ToSingle(PositionYNumeric.Value), Convert.ToSingle(PositionZNumeric.Value));
                    placement.RotationDegrees = new Vector3(Convert.ToSingle(RotationXNumeric.Value), Convert.ToSingle(RotationYNumeric.Value), Convert.ToSingle(RotationZNumeric.Value));
                }
            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        private Image LoadDDSSquish(string name)
        {
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            DdsFile dds = new DdsFile();

            name = File.Exists(name) == false ? "Resources/texture.dds" : name;

            var bLoaded = false;
            using (var stream = File.Open(name, FileMode.Open))
            {
                try
                {
                    dds.Load(stream);
                    bLoaded = true;
                }
                catch(Exception ex)
                {
                    Utils.Logging.Log.WriteLine("Failed to load DDS: " + name, Utils.Logging.LoggingTypes.WARNING);
                }
            }

            Image thumbnail = null;
            if (bLoaded)
                thumbnail = dds.Image().GetThumbnailImage(128, 120, myCallback, IntPtr.Zero);
            else
                thumbnail = LoadDDSSquish("Resources/texture.dds");

            dds = null;
            return thumbnail;
        }

        private Image GetThumbnail(MaterialStruct material)
        {
            Material mat = MaterialsManager.LookupMaterialByHash(material.MaterialHash);
            Image thumbnail = null;
            if (mat != null)
            {
                if(mat.Samplers.ContainsKey("S000"))
                {
                    thumbnail = LoadDDSSquish(Path.Combine(SceneData.ScenePath, mat.Samplers["S000"].File));
                }
                else
                {
                    thumbnail = LoadDDSSquish("Resources/texture.dds");
                }
            }
            else
            {
                thumbnail = LoadDDSSquish("Resources/MissingMaterial.dds");
            }
            return thumbnail;
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            MatViewPanel.Controls.Clear();
            if (FrameResource.IsFrameType(currentObject))
            {
                if (currentObject is FrameObjectSingleMesh)
                {
                    var entry = (currentObject as FrameObjectSingleMesh);
                    for (int i = 0; i != entry.Material.NumLods; i++)
                    {
                        for (int x = 0; x != entry.Material.Materials[i].Length; x++)
                        {
                            var mat = entry.Material.Materials[i][x];
                            TextureEntry textEntry = new TextureEntry();
                            textEntry.WasClicked += MatViewerPanel_WasClicked;
                            textEntry.SetMaterialName(mat.MaterialName);
                            textEntry.SetMaterialTexture(GetThumbnail(mat));
                            MatViewPanel.Controls.Add(textEntry);
                        }
                    }
                }
            }
        }

        void MatViewerPanel_WasClicked(object sender, EventArgs e)
        {
            // Set IsSelected for all UCs in the FlowLayoutPanel to false. 
            MatBrowser browser = null;
            foreach (var c in MatViewPanel.Controls)
            {
                if (c is TextureEntry)
                {
                    if(((TextureEntry)c).IsSelected)
                        browser = new MatBrowser();

                    ((TextureEntry)c).IsSelected = false;
                }
            }
        }
    }
}
