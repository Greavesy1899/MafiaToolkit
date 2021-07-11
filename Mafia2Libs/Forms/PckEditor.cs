using System.Diagnostics;
using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.Wwise;
using Utils.Language;
using Utils.Settings;
using Forms.EditorControls;

namespace Mafia2Tool
{
    public partial class PckEditor : Form
    {
        private FileInfo PckFile;
        private PCK pck;
        private bool bIsFileEdited = false;
        public PckEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            PckFile = file;
            BuildData();
            Show();
        }

        private void Localise()
        {
            Text = Language.GetString("$PCK_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Edit.Text = Language.GetString("$EDIT");
        }

        private void BuildData()
        {
            pck = new PCK(PckFile.FullName);
            
            foreach (Wem wem in pck.WemList)
            {
                TreeNode node = new TreeNode(wem.Id.ToString());
                node.Text = wem.Name;
                node.Tag = wem;
                TreeView_Pck.Nodes.Add(node);
            }
        }

        private void Save()
        {
            File.Copy(PckFile.FullName, PckFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(PckFile.FullName, FileMode.Open)))
            {
                pck.WriteToFile(writer);
            }

            Text = Language.GetString("$PCK_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            TreeView_Pck.SelectedNode = null;
            TreeView_Pck.Nodes.Clear();
            Grid_Pck.SelectedObject = null;

            BuildData();

            Text = Language.GetString("$PCK_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void PckEditor_OnNodeSelect(object sender, TreeViewEventArgs e)
        {
            Grid_Pck.SelectedObject = e.Node.Tag;
        }

        private void Button_Save_Click(object sender, System.EventArgs e) => Save();
        private void Button_Reload_Click(object sender, System.EventArgs e) => Reload();
        private void Button_Exit_Click(object sender, System.EventArgs e) => Close();
    }
}
