// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using Forms.OptionControls;
using System.Windows.Forms;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
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
            ClearOptionControls();
            splitContainer1.Panel2.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.AutoSize = true;
        }

        private void ClearOptionControls()
        {
            // Disposing a control also removes it from its parent collection.
            while (splitContainer1.Panel2.Controls.Count > 0)
            {
                splitContainer1.Panel2.Controls[0].Dispose();
            }
        }

        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ClearOptionControls();

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
