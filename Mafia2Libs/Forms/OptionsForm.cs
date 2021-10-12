using Forms.OptionControls;
using System.Windows.Forms;
using Utils.Language;
using Utils.Settings;

namespace Toolkit
{
    public partial class OptionsForm : Form
    {
        private IniFile ini = new IniFile();

        public OptionsForm()
        {
            InitializeComponent();
            Localise();
            SwapOptionControls(new GeneralOptions());
        }

        private void Localise()
        {
            foreach(TreeNode node in treeView1.Nodes)
            {
                node.Name = Language.GetString(node.Name);
                node.Text = Language.GetString(node.Text);
            }
            Text = Language.GetString("$OPTIONS");
        }

        private void SwapOptionControls(UserControl control)
        {
            splitContainer1.Panel2.Controls.Clear();
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
                case 4:
                    SwapOptionControls(new RenderOptions());
                    break;
            }
        }
    }
}
