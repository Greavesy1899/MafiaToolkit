using Rendering.Graphics;
using Utils.Language;
using WeifenLuo.WinFormsUI.Docking;

namespace Forms.Docking
{
    public partial class DockViewProperties : DockContent
    {
        public bool[] VisibleProperties;

        public DockViewProperties()
        {
            InitializeComponent();
        }

        public void SetPickInfo(PickOutParams InPickParams)
        {
            // Reformat string
            TextBox_PickWSLocation.Text = string.Format("{0} {1} {2}",
                InPickParams.WorldPosition.X,
                InPickParams.WorldPosition.Y,
                InPickParams.WorldPosition.Z);

        }

        private void OnResize(object sender, System.EventArgs e)
        {
        }
    }
}
