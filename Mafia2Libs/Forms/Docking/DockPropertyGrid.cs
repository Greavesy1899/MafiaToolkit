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
            }
        }
    }
}
