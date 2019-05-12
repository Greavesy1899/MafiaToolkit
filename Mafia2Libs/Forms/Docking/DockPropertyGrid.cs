using ResourceTypes.FrameResource;
using SharpDX;
using System;
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
            currentObject = null;
            IsEntryReady = false;
        }

        public void SetObject(object obj)
        {
            currentObject = obj;
            SetTransformEdit();
            SetPropertyGrid();
        }

        private void SetTransformEdit()
        {
            IsEntryReady = false;
            if (currentObject is FrameObjectBase)
            {
                FrameObjectBase fObject = (currentObject as FrameObjectBase);
                CurrentEntry.Text = fObject.Name.String;
                PositionXNumeric.Value = Convert.ToDecimal(fObject.Matrix.Position.X);
                PositionYNumeric.Value = Convert.ToDecimal(fObject.Matrix.Position.Y);
                PositionZNumeric.Value = Convert.ToDecimal(fObject.Matrix.Position.Z);
                RotationXNumeric.Value = Convert.ToDecimal(fObject.Matrix.Rotation.EulerRotation.X);
                RotationYNumeric.Value = Convert.ToDecimal(fObject.Matrix.Rotation.EulerRotation.Y);
                RotationZNumeric.Value = Convert.ToDecimal(fObject.Matrix.Rotation.EulerRotation.Z);
            }
            else if(currentObject is ResourceTypes.Collisions.Collision.Placement)
            {
                ResourceTypes.Collisions.Collision.Placement placement = (currentObject as ResourceTypes.Collisions.Collision.Placement);
                CurrentEntry.Text = placement.Hash.ToString();
                PositionXNumeric.Value = Convert.ToDecimal(placement.Position.X);
                PositionYNumeric.Value = Convert.ToDecimal(placement.Position.Y);
                PositionZNumeric.Value = Convert.ToDecimal(placement.Position.Z);
                RotationXNumeric.Value = Convert.ToDecimal(placement.Rotation.X);
                RotationYNumeric.Value = Convert.ToDecimal(placement.Rotation.Y);
                RotationZNumeric.Value = Convert.ToDecimal(placement.Rotation.Z);
            }
            IsEntryReady = true;
        }

        private void SetPropertyGrid()
        {
            PropertyGrid.SelectedObject = currentObject;
        }

        public void UpdateObject()
        {
            if(IsEntryReady && currentObject != null)
            {
                if (currentObject is FrameObjectBase)
                {
                    FrameObjectBase fObject = (currentObject as FrameObjectBase);
                    fObject.Matrix.Position = new Vector3(Convert.ToSingle(PositionXNumeric.Value), Convert.ToSingle(PositionYNumeric.Value), Convert.ToSingle(PositionZNumeric.Value));
                    fObject.Matrix.Rotation.EulerRotation = new Vector3(Convert.ToSingle(RotationXNumeric.Value), Convert.ToSingle(RotationYNumeric.Value), Convert.ToSingle(RotationZNumeric.Value));
                    fObject.Matrix.Rotation.UpdateMatrixFromEuler();
                }
                else if(currentObject is ResourceTypes.Collisions.Collision.Placement)
                {
                    ResourceTypes.Collisions.Collision.Placement placement = (currentObject as ResourceTypes.Collisions.Collision.Placement);
                    placement.Position = new Vector3(Convert.ToSingle(PositionXNumeric.Value), Convert.ToSingle(PositionYNumeric.Value), Convert.ToSingle(PositionZNumeric.Value));
                    placement.Rotation = new Vector3(Convert.ToSingle(RotationXNumeric.Value), Convert.ToSingle(RotationYNumeric.Value), Convert.ToSingle(RotationZNumeric.Value));
                }
            }
        }
    }
}
