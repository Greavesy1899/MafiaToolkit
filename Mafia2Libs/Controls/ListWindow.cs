using ResourceTypes.FrameResource;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class ListWindow : Form
    {
        private bool searchMode = false;
        public object chosenObject = null;
        public int chosenObjectIndex = -1;

        public ListWindow()
        {
            InitializeComponent();
        }

        //sort this shit out.
        public void PopulateForm(int parent)
        {
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
            chosenObject = listBox1.SelectedItem;
            chosenObjectIndex = listBox1.SelectedIndex;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
