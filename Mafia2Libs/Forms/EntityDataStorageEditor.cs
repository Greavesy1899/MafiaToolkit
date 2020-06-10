using System.IO;
using System.Windows.Forms;
using ResourceTypes.EntityDataStorage;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class EntityDataStorageEditor : Form
    {
        private FileInfo edsFile;
        private EntityDataStorageLoader tables;

        public EntityDataStorageEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            edsFile = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the EDS editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$ACTOR_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
        }

        private void BuildData()
        {
            tables = new EntityDataStorageLoader();
            tables.ReadFromFile(edsFile.FullName, false);

            TreeNode entityNode = new TreeNode("Entity");
            entityNode.Tag = tables;
            TreeView_Tables.Nodes.Add(entityNode);

            if (tables.Tables != null)
            {
                for (int i = 0; i < tables.Tables.Length; i++)
                {
                    TreeNode node = new TreeNode("Table_" + i);
                    node.Tag = tables.Tables[i];
                    entityNode.Nodes.Add(node);
                }
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_Item.SelectedObject = e.Node.Tag;
        }

        private void Button_Save_OnClick(object sender, System.EventArgs e)
        {
            File.Copy(edsFile.FullName, edsFile.FullName + "_old", true);
            tables.WriteToFile(edsFile.FullName, false);
        }

        private void Button_Reload_OnClick(object sender, System.EventArgs e)
        {
            TreeView_Tables.Nodes.Clear();
            BuildData();
        }

        private void Button_Exit_OnClick(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}