using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.City;
using Utils.Lang;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class CityAreaEditor : Form
    {
        private FileInfo cityAreasFile;
        private CityAreas areas;
        private int curIndex = -1;

        public CityAreaEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            cityAreasFile = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Editing City Areas.");
        }

        private void Localise()
        {
            Text = Language.GetString("$CITY_AREA_EDITOR_TITLE");
            fileToolButton.Text = Language.GetString("$FILE");
            SaveButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            ExitButton.Text = Language.GetString("$EXIT");
            toolButton.Text = Language.GetString("$TOOLS");
            AddAreaButton.Text = Language.GetString("$ADD_AREA");
            AreaGroupBox.Text = Language.GetString("$AREA_DATA");
            Area1Label.Text = Language.GetString("$AREA_1");
            Area2Label.Text = Language.GetString("$AREA_2");
            AreaNameLabel.Text = Language.GetString("$AREA_NAME");
            UnkByteLabel.Text = Language.GetString("$UNK_BYTE");
            SaveAreaButton.Text = Language.GetString("$SAVE_AREA");
            ReloadAreaButton.Text = Language.GetString("$RELOAD_AREA");
        }

        private void BuildData()
        {
            areas = new CityAreas(cityAreasFile.FullName);

            for (int i = 0; i != areas.AreaCollection.Count; i++)
            {
                listBox1.Items.Add(areas.AreaCollection[i].Name);
            }
        }

        private void AddAreaButton_Click(object sender, EventArgs e)
        {
            CityAreas.AreaData area = new CityAreas.AreaData();
            area.Create();
            areas.AreaCollection.Add(area);
            listBox1.Items.Add(area.Name);
            listBox1.SelectedIndex = areas.AreaCollection.Count - 1;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(cityAreasFile.FullName, FileMode.Create)))
            {
                areas.WriteToFile(writer);
            }
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            Clear();
            BuildData();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UpdateAreaData(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                curIndex = listBox1.SelectedIndex;
            }

            AreaNameBox.Text = areas.AreaCollection[curIndex].Name;
            Area1Box.Text = areas.AreaCollection[curIndex].IndexedString;
            Area2Box.Text = areas.AreaCollection[curIndex].IndexedString2;
            UnkByteBox.Checked = Convert.ToBoolean(areas.AreaCollection[curIndex].UnkByte);
        }

        private void SaveArea_Clicked(object sender, EventArgs e)
        {
            areas.AreaCollection[curIndex].Name = AreaNameBox.Text;
            areas.AreaCollection[curIndex].IndexedString = Area1Box.Text;
            areas.AreaCollection[curIndex].IndexedString2 = Area2Box.Text;
            areas.AreaCollection[curIndex].UnkByte = Convert.ToByte(UnkByteBox.Checked);
            listBox1.Items[curIndex] = AreaNameBox.Text;
        }

        private void ReloadArea_Click(object sender, EventArgs e)
        {
            UpdateAreaData(null, null);
        }

        private void DeleteArea_Click(object sender, EventArgs e)
        {
            //just a testing thing right now
            if(listBox1.SelectedIndex != 1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            else if (listBox1.SelectedItem != null)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void Clear()
        {
            listBox1.Items.Clear();
            AreaNameBox.Clear();
            Area1Box.Clear();
            Area2Box.Clear();
            UnkByteBox.Checked = false;
        }
    }
}