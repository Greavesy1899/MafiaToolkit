using ResourceTypes.Actors;
using ResourceTypes.FrameResource;
using System.Collections.Generic;
using System.Windows.Forms;
using Utils.Language;

namespace Mafia2Tool
{
    public partial class ListWindow : Form
    {
        private bool frameMode = false;
        private bool searchMode = false;
        public object chosenObject = null;
        private const string ROOT_STRING = "root (-1)";
        ParentInfo.ParentType type = 0;

        public ListWindow()
        {
            InitializeComponent();
        }

        public void PopulateForm(ParentInfo.ParentType ParentType)
        {
            labelInfo.Text = Language.GetString("$SELECT_PARENT") + '\n' + Language.GetString("$HOW_TO_SEARCH");
            type = ParentType;
            frameMode = true;
            listBox1.Items.Add(ROOT_STRING);

            if (ParentType == ParentInfo.ParentType.ParentIndex2)
            {
                foreach (KeyValuePair<int, FrameHeaderScene> entry in SceneData.FrameResource.FrameScenes)
                {
                    listBox1.Items.Add(entry.Value);
                }
            }

            foreach (KeyValuePair<int, object> entry in SceneData.FrameResource.FrameObjects)
            {
                listBox1.Items.Add(entry.Value);
            }
        }

        public void PopulateForm(List<ActorEntry> items)
        {
            labelInfo.Text = Language.GetString("$SELECT_ITEM");
            foreach(var item in items)
            {
                listBox1.Items.Add(item);
            }
        }

        private void SearchForms()
        {
            if (frameMode)
            {
                listBox1.Items.Clear();
                foreach (KeyValuePair<int, object> entry in SceneData.FrameResource.FrameObjects)
                {
                    FrameObjectBase obj = entry.Value as FrameObjectBase;

                    if (obj.Name.String.Contains(SearchBox.Text))
                    {
                        listBox1.Items.Add(entry.Value);
                    }
                }
            }
        }

        private void SearchOnClick(object sender, System.EventArgs e)
        {
            if (!searchMode)
            {
                searchMode = true;
                SearchBox.Clear();
            }
        }

        private void SearchOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (searchMode && e.KeyChar == 13)
            {
                SearchForms();
                searchMode = false;
            }
        }

        private void OnItemSelect(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                chosenObject = (listBox1.SelectedItem.ToString() == ROOT_STRING) ? null : listBox1.SelectedItem;       
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
