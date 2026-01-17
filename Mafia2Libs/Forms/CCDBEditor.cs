using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.CCDB;
using Utils.Language;

namespace Mafia2Tool
{
    public partial class CCDBEditor : Form
    {
        private FileInfo ccdbFile;
        private CCDBFile ccdb;

        private bool bIsFileEdited = false;

        public CCDBEditor(FileInfo file)
        {
            InitializeComponent();

            ccdb = new CCDBFile();
            ccdbFile = file;

            Localise();
            ReadFromFile();
            Initialise();
            ShowDialog();
        }

        private void Localise()
        {
            Text = Language.GetString("$CCDB_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
        }

        private void ReadFromFile()
        {
            try
            {
                ccdb.ReadFromFile(ccdbFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading CCDB file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Initialise()
        {
            TreeView_CCDB.Nodes.Clear();
            Grid_CCDB.SelectedObject = null;
            TreeView_CCDB.Nodes.Add(ccdb.GetAsTreeNodes());
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            Grid_CCDB.SelectedObject = e.Node.Tag;
        }

        private void Save()
        {
            try
            {
                // Create backup
                File.Copy(ccdbFile.FullName, ccdbFile.FullName + "_old", true);
                ccdb.WriteToFile(ccdbFile);

                Text = Language.GetString("$CCDB_EDITOR_TITLE");
                bIsFileEdited = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving CCDB file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button_Save_Click(object sender, EventArgs e) => Save();

        private void Button_Reload_Click(object sender, EventArgs e)
        {
            TreeView_CCDB.SelectedNode = null;
            Grid_CCDB.SelectedObject = null;

            Text = Language.GetString("$CCDB_EDITOR_TITLE");
            bIsFileEdited = false;

            ReadFromFile();
            Initialise();
        }

        private void Button_Exit_Click(object sender, EventArgs e) => Close();

        private void OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Grid_CCDB.Refresh();

            Text = Language.GetString("$CCDB_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void CCDBEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", System.Windows.MessageBoxButton.YesNoCancel);

                if (SaveChanges == System.Windows.MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (SaveChanges == System.Windows.MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Button_ExpandAll_Click(object sender, EventArgs e)
        {
            TreeView_CCDB.ExpandAll();
        }

        private void Button_CollapseAll_Click(object sender, EventArgs e)
        {
            TreeView_CCDB.CollapseAll();
        }
    }
}
