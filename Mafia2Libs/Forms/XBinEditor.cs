using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.M3.XBin;
using Utils.Language;

namespace Mafia2Tool
{
    public partial class XBinEditor : Form
    {
        private FileInfo xbinfile;
        private XBin xbin;

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
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            TreeNode SelectedNode = TreeView_XBin.SelectedNode;

            if(SelectedNode != null)
            {
                SelectedNode.Remove();
            }
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            xbin.TableInformation.SetFromTreeNodes(TreeView_XBin.Nodes[0]);

            // Create backup, set our new xbins, and then save.
            File.Copy(xbinfile.FullName, xbinfile.FullName + "_old", true);
            xbin.WriteToFile(xbinfile);
        }

        private void OnPropertyValidChanged(object s, PropertyValueChangedEventArgs e)
        {
            Grid_XBin.Refresh();
        }
    }
}