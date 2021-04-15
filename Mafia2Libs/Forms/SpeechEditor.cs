using System.IO;
using System.Windows.Forms;
using ResourceTypes.Speech;
using Utils.Helpers;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class SpeechEditor : Form
    {
        private FileInfo speechFile;
        private SpeechLoader speechData;

        public SpeechEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            speechFile = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Speech editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$SPEECH_EDITOR_TITLE");
            fileToolButton.Text = Language.GetString("$FILE");
            saveToolStripMenuItem.Text = Language.GetString("$SAVE");
            reloadToolStripMenuItem.Text = Language.GetString("$RELOAD");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
        }

        private void BuildData()
        {
            speechData = new SpeechLoader(speechFile);

            for (int i = 0; i != speechData.SpeechTypes.Length; i++)
            {
                SpeechLoader.SpeechTypeData typeData = speechData.SpeechTypes[i];

                TreeNode node = new TreeNode(typeData.SpeechType.ToString());
                node.Tag = typeData;

                int num = 0;
                for (int x = 0; x != speechData.SpeechItems.Length; x++)
                {
                    num++;
                    SpeechLoader.SpeechItemData itemData = speechData.SpeechItems[x];
                    TreeNode node1 = new TreeNode(typeData.SpeechType.ToString());
                    node1.Tag = itemData;

                    if (typeData.Unk0+num == itemData.Unk0)
                    {
                        node.Nodes.Add(node1);
                    }
                    else
                    {
                        num = 0;
                    }
                }
                TreeView_Speech.Nodes.Add(node);
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

        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }
    }
}