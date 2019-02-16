using Mafia2;
using System;
using System.Windows.Forms;

namespace Mafia2Tool.OptionControls
{
    public partial class RenderOptions : UserControl
    {
        public RenderOptions()
        {
            InitializeComponent();
            Localise();
            LoadSettings();
        }

        private void Localise()
        {
            RenderGroup.Text = Language.GetString("$RENDER_OPTIONS");
            ScreenFarLabel.Text = Language.GetString("$RENDER_SCREENFAR");
            ScreenNearLabel.Text = Language.GetString("$RENDER_SCREENEAR");
        }

        /// <summary>
        /// Read Settings from INI and populate controls.
        /// </summary>
        private void LoadSettings()
        {
            ScreenFarUpDown.Value = Math.Min(Convert.ToInt16(ToolkitSettings.ScreenDepth), ScreenFarUpDown.Maximum);
            ScreenNearUpDown.Value = Math.Min(Convert.ToInt16(ToolkitSettings.ScreenNear), ScreenNearUpDown.Maximum);
        }

        private void ScreenDepth_Changed(object sender, EventArgs e)
        {
            ToolkitSettings.ScreenDepth = Convert.ToSingle(ScreenFarUpDown.Value);
            ToolkitSettings.WriteKey("ScreenDepth", "ModelViewer", ToolkitSettings.ScreenDepth.ToString());
        }

        private void ScreenNear_Changed(object sender, EventArgs e)
        {
            ToolkitSettings.ScreenNear = Convert.ToSingle(ScreenNearUpDown.Value);
            ToolkitSettings.WriteKey("ScreenNear", "ModelViewer", ToolkitSettings.ScreenNear.ToString());
        }
    }
}
