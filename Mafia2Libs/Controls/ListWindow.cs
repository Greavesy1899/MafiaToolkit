using ResourceTypes.FrameResource;
using System.Collections.Generic;
using System.Windows.Forms;
using Utils.Lang;

namespace Mafia2Tool
{
    public partial class ListWindow : Form
    {
        private bool searchMode = false;
        public object chosenObject = null;
        int type = 0;
        public string chosenObjectName = "";

        public ListWindow()
        {
            InitializeComponent();
        }

        public void PopulateForm(int parent)
        {
            labelInfo.Text = Language.GetString("$SELECT_PARENT") + '\n' + Language.GetString("$HOW_TO_SEARCH");
            type = parent;
            if (parent == 1)
            {
                foreach (KeyValuePair<int, FrameHeaderScene> entry in SceneData.FrameResource.FrameScenes)
                    listBox1.Items.Add(entry.Value);
            }
                foreach (KeyValuePair<int, object> entry in SceneData.FrameResource.FrameObjects)
                    listBox1.Items.Add(entry.Value);
        }

        private void SearchForms()
        {
            listBox1.Items.Clear();
            foreach (KeyValuePair<int, object> entry in SceneData.FrameResource.FrameObjects)
            {
                FrameObjectBase obj = entry.Value as FrameObjectBase;

                if (obj.Name.String.Contains(SearchBox.Text))
                    listBox1.Items.Add(entry.Value);
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
                chosenObject = listBox1.SelectedItem;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
