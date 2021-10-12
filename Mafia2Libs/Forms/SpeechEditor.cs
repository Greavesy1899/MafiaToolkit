using System.IO;
using System.Windows.Forms;
using ResourceTypes.Speech;
using Utils.Helpers;
using Utils.Language;
using Utils.Settings;

namespace MafiaToolkit
{
    public partial class SpeechEditor : Form
    {
        private FileInfo speechFile;
        private SpeechLoader speechData;

        private bool bIsFileEdited = false;

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
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
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

        private void Save()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(speechFile.FullName, FileMode.Create)))
            {
                speechData.WriteToFile(writer);
            }

            Text = Language.GetString("$SPEECH_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            TreeView_Speech.SelectedNode = null;
            Grid_Speech.SelectedObject = null;

            TreeView_Speech.Nodes.Clear();
            BuildData();

            Text = Language.GetString("$SPEECH_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            Grid_Speech.SelectedObject = e.Node.Tag;
        }

        private void Grid_Speech_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name")
                TreeView_Speech.SelectedNode.Text = e.ChangedItem.Value.ToString();

            Text = Language.GetString("$SPEECH_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void SpeechEditor_Closing(object sender, FormClosingEventArgs e)
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

        private void Button_Save_Click(object sender, System.EventArgs e) => Save();
        private void Button_Reload_Click(object sender, System.EventArgs e) => Reload();
        private void Button_Exit_Click(object sender, System.EventArgs e) => Close();
    }
}