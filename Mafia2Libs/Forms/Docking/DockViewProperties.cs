using System;
using System.Numerics;
using Rendering.Graphics;
using WeifenLuo.WinFormsUI.Docking;

namespace Forms.Docking
{
    public partial class DockViewProperties : DockContent
    {
        private Vector3 Offset = Vector3.Zero;

        public DockViewProperties()
        {
            InitializeComponent();

            TextBox_PickWSLocation.Text = String.Format("0.0 0.0 0.0");
            TextBox_WithOffset.Text = String.Format("0.0 0.0 0.0");
        }

        public void SetPickInfo(PickOutParams InPickParams)
        {
            // Reformat string
            TextBox_PickWSLocation.Text = string.Format("{0} {1} {2}",
                InPickParams.WorldPosition.X,
                InPickParams.WorldPosition.Y,
                InPickParams.WorldPosition.Z);

            // Create Offset vector
            Vector3 WithOffset = InPickParams.WorldPosition + Offset;
            TextBox_WithOffset.Text = string.Format("{0} {1} {2}",
                WithOffset.X,
                WithOffset.Y,
                WithOffset.Z);

        }

        private void OnResize(object sender, EventArgs e)
        {
        }

        private void Numeric_OnValueChanged(object sender, EventArgs e)
        {
            if(sender == Numeric_PosX)
            {
                Offset.X = Convert.ToSingle(Numeric_PosX.Value);
            }
            else if(sender == Numeric_PosY)
            {
                Offset.Y = Convert.ToSingle(Numeric_PosY.Value);
            }
            else if(sender == Numeric_PosZ)
            {
                Offset.Z = Convert.ToSingle(Numeric_PosZ.Value);
            }
        }
    }
}
