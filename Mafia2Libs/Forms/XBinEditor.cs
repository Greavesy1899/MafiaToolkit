using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.M3.XBin;
using Utils.Helpers;
using Utils.Language;

namespace Mafia2Tool
{
    public partial class XBinEditor : Form
    {
        private FileInfo xbinfile;
        private XBin xbin;

        private bool bIsFileEdited = false;

        public XBinEditor(FileInfo file)
        {
            InitializeComponent();

            xbin = new XBin();
            xbinfile = file;

            Localise();
            ReadFromFile();
            Initialise();
            ShowDialog();
        }

        private void Localise()
        {
            Text = Language.GetString("$XBIN_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Tools.Text = Language.GetString("$TOOLS");
            Button_Import.Text = Language.GetString("$IMPORT_XBIN");
            Button_Export.Text = Language.GetString("$EXPORT_XBIN");
        }

        private void ReadFromFile()
        {
            // Read contents of XBin
            using (BinaryReader reader = new BinaryReader(File.Open(xbinfile.FullName, FileMode.Open)))
            {
                xbin.ReadFromFile(reader);
            }
        }

        private void Initialise()
        {
            // Push it onto the TreeView
            TreeView_XBin.Nodes.Clear();
            Grid_XBin.SelectedObject = null;
            TreeView_XBin.Nodes.Add(xbin.TableInformation.GetAsTreeNodes());
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            Grid_XBin.SelectedObject = e.Node.Tag;
        }

        private void Save()
        {
            xbin.TableInformation.SetFromTreeNodes(TreeView_XBin.Nodes[0]);

            // Create backup, set our new xbins, and then save.
            File.Copy(xbinfile.FullName, xbinfile.FullName + "_old", true);
            xbin.WriteToFile(xbinfile);

            Text = Language.GetString("$XBIN_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Button_Export_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var XMLFileName = Path.Combine(xbinfile.DirectoryName, Path.GetFileNameWithoutExtension(xbinfile.FullName));
            XMLFileName += ".xml";
            xbin.TableInformation.WriteToXML(XMLFileName);
            Cursor.Current = Cursors.Default;
        }

        private void Button_Import_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            // Lets make sure the actual XML document exists first.
            var XMLFileName = Path.Combine(xbinfile.DirectoryName, Path.GetFileNameWithoutExtension(xbinfile.FullName));
            XMLFileName += ".xml";

            if (!File.Exists(XMLFileName))
            {
                MessageBox.Show("To import an XML file for this XBin, please export first.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            xbin.TableInformation.ReadFromXML(XMLFileName);
            Initialise();
            Cursor.Current = Cursors.Default;

            Text = Language.GetString("$XBIN_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            TreeNode SelectedNode = TreeView_XBin.SelectedNode;

            if(SelectedNode != null)
            {
                SelectedNode.Remove();

                Text = Language.GetString("$XBIN_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void Button_Save_Click(object sender, EventArgs e) => Save();

        private void Button_Reload_Click(object sender, EventArgs e)
        {
            TreeView_XBin.SelectedNode = null;
            Grid_XBin.SelectedObject = null;

            Text = Language.GetString("$XBIN_EDITOR_TITLE");
            bIsFileEdited = false;

            ReadFromFile();
            Initialise();
        }

        private void Button_Exit_Click(object sender, EventArgs e) => Close();

        private void OnPropertyValidChanged(object s, PropertyValueChangedEventArgs e)
        {
            Grid_XBin.Refresh();

            Text = Language.GetString("$XBIN_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void XbinEditor_Closing(object sender, FormClosingEventArgs e)
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
    }
}