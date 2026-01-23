using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using ResourceTypes.Speech;
using Utils.Helpers;
using Utils.Helpers.Reflection;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class SpeechEditor : Form
    {
        private FileInfo speechFile;
        private SpeechFile speechData;

        private bool bIsFileEdited = false;

        public SpeechEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            speechFile = file;
            speechData = new SpeechFile(file);
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
            Button_Edit.Text = Language.GetString("$EDIT");
            Button_SaveToXML.Text = Language.GetString("$SAVE_TO_XML");
            Button_LoadFromXML.Text = Language.GetString("$LOAD_FROM_XML");
        }

        private void BuildData()
        {
            TreeView_Speech.SelectedNode = null;
            Grid_Speech.SelectedObject = null;

            TreeView_Speech.Nodes.Clear();

            for (int i = 0; i != speechData.SpeechTypes.Length; i++)
            {
                SpeechFile.SpeechTypeInfo typeData = speechData.SpeechTypes[i];

                TreeNode node = new TreeNode(typeData.SpeechType.ToString());
                node.Tag = typeData;

                int num = 0;
                for (int x = 0; x != speechData.SpeechItems.Length; x++)
                {
                    num++;
                    SpeechFile.SpeechItemInfo itemData = speechData.SpeechItems[x];
                    TreeNode node1 = new TreeNode(itemData.ItemName.ToString());
                    node1.Tag = itemData;

                    if (typeData.Unk0 + num == itemData.Unk0)
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
            speechData = new SpeechFile(speechFile);

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

        private void OnSaveToXMLClicked(object sender, System.EventArgs e)
        {
            if (FileSaveDialog_SelectXML.ShowDialog() == DialogResult.OK)
            {
                XElement RootElement = ReflectionHelpers.ConvertPropertyToXML<SpeechFile>(speechData);
                RootElement.Save(FileSaveDialog_SelectXML.FileName);
            }
        }

        private void OnLoadFromXMLClicked(object sender, System.EventArgs e)
        {
            if (FileOpenDialog_SelectXML.ShowDialog() == DialogResult.OK)
            {
                XElement RootElement = XElement.Load(FileOpenDialog_SelectXML.FileName);
                speechData = ReflectionHelpers.ConvertToPropertyFromXML<SpeechFile>(RootElement);
                BuildData();
            }
        }

        private void Button_Save_Click(object sender, System.EventArgs e) => Save();
        private void Button_Reload_Click(object sender, System.EventArgs e) => Reload();
        private void Button_Exit_Click(object sender, System.EventArgs e) => Close();
    }
}