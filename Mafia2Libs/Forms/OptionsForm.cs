using Mafia2Tool.OptionControls;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class OptionsForm : Form
    {
        private IniFile ini = new IniFile();

        public OptionsForm()
        {
            InitializeComponent();
            SwapOptionControls(new GeneralOptions());
        }

        private void SwapOptionControls(UserControl control)
        {
            splitContainer1.Panel2.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.AutoSize = true;
        }

        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            splitContainer1.Panel2.Controls.Clear();
            
            switch(e.Node.Index)
            {
                case 0:
                    SwapOptionControls(new GeneralOptions());
                    break;
                case 1:
                    SwapOptionControls(new SDSOptions());
                    break;
                case 2:
                    SwapOptionControls(new ModelOptions());
                    break;
                case 3:
                    SwapOptionControls(new MTLOptions());
                    break;
            }
        }
    }
}
