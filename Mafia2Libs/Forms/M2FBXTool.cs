using System;
using Utils;
using System.Windows.Forms;
using Utils.Language;
using System.IO;

namespace MafiaToolkit.Forms
{
    public partial class M2FBXTool : Form
    {

        public M2FBXTool()
        {
            InitializeComponent();
            Localise();
            ShowDialog();
        }

        private void Localise()
        {
            M2FBXLabel.Text = Language.GetString("$M2FBX_INSTRUCTIONS");
            ImportLabel.Text = Language.GetString("$EXPORT");
            ExportLabel.Text = Language.GetString("$IMPORT");
            ImportButton.Text = ExportButton.Text = Language.GetString("$SELECT_MESH");
            ConvertButton.Text = Language.GetString("$CONVERT");
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            MeshBrowser.FileName = "";
            if (MeshBrowser.ShowDialog() == DialogResult.OK)
            {
                ImportBox.Text = MeshBrowser.FileName.ToLower();
            }
            else return;
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            MeshBrowser.FileName = "";
            if (MeshBrowser.ShowDialog() == DialogResult.OK)
            {
                ExportBox.Text = MeshBrowser.FileName.ToLower();
            }
            else return;
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(ImportBox.Text))
            {
                if (ImportBox.Text.Contains(".m2t"))
                {
                    FBXHelper.ConvertM2T(ImportBox.Text, ExportBox.Text);
                }
                else if (ImportBox.Text.Contains(".fbx"))
                {
                    FBXHelper.ConvertFBX(ImportBox.Text, ExportBox.Text);
                }
            }
            else
            {
                MessageBox.Show("Import file does not exist!", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
