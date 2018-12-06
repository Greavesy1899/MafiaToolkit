using System.IO;
using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool
{
    public partial class CityAreaEditor : Form
    {
        private FileInfo cityAreasFile;
        private CityAreas areas;

        public CityAreaEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            cityAreasFile = file;
            BuildData();
            ShowDialog();
            ToolkitSettings.UpdateRichPresence("Editing City Areas.");
        }

        private void Localise()
        {
            Text = Language.GetString("$CITY_AREA_EDITOR_TITLE");
            fileToolButton.Text = Language.GetString("$FILE");
            saveToolStripMenuItem.Text = Language.GetString("$SAVE");
            reloadToolStripMenuItem.Text = Language.GetString("$RELOAD");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
        }

        private void BuildData()
        {
            areas = new CityAreas(cityAreasFile.FullName);

            for(int i = 0; i != areas.AreaCollection.Length; i++)
            {
                TreeNode node = new TreeNode(areas.AreaCollection[i].Name);
                node.Name = areas.AreaCollection[i].Name.ToString();
                node.Tag = areas.AreaCollection[i];
                treeView1.Nodes.Add(node);
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            FrameResourceGrid.SelectedObject = e.Node.Tag;
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void reloadToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(cityAreasFile.FullName, FileMode.Open)))
                areas.ReadFromFile(reader);
        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(cityAreasFile.FullName, FileMode.Create)))
                areas.WriteToFile(writer);
        }
    }
}