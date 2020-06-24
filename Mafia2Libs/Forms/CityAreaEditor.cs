using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.City;
using Utils.Language;
using Utils.Settings;

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
        }

        private void BuildData()
        {
            areas = new CityAreas(cityAreasFile.FullName);

            for (int i = 0; i != areas.AreaCollection.Count; i++)
            {
                ListBox_Areas.Items.Add(areas.AreaCollection[i]);
            }
        }

        private void AddAreaButton_Click(object sender, EventArgs e)
        {
            CityAreas.AreaData area = new CityAreas.AreaData();
            area.Create();
            areas.AreaCollection.Add(area);
            ListBox_Areas.Items.Add(area);
            ListBox_Areas.SelectedItem = area;
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
            if (ListBox_Areas.SelectedIndex != -1)
            {
                PropertyGrid_Area.SelectedObject = ListBox_Areas.SelectedItem;
            }
        }

        private void DeleteArea_Click(object sender, EventArgs e)
        {
            if(ListBox_Areas.SelectedItem != null)
            {
                areas.AreaCollection.Remove((CityAreas.AreaData)ListBox_Areas.SelectedItem);
                ListBox_Areas.Items.Remove(ListBox_Areas.SelectedItem);
            }
        }

        private void Clear()
        {
            ListBox_Areas.Items.Clear();
        }

        private void PropertyGrid_Area_ValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var area = (ListBox_Areas.SelectedItem as CityAreas.AreaData);
            var index = ListBox_Areas.SelectedIndex;
            ListBox_Areas.Items.RemoveAt(index);
            ListBox_Areas.Items.Insert(index, area);
            ListBox_Areas.SelectedItem = area;
        }

        private void Button_Search_OnClick(object sender, EventArgs e)
        {
            var text = TextBox_Search.Text;
            var textUpper = text.ToUpper();
            var textLower = text.ToLower();
            var bIsSearching = !string.IsNullOrEmpty(text);
            ListBox_Areas.Items.Clear();

            foreach(var area in areas.AreaCollection)
            {
                if (bIsSearching)
                {
                    if (area.Name.Contains(text) || area.Name.Contains(textLower) || area.Name.Contains(textUpper))
                    {
                        ListBox_Areas.Items.Add(area);
                    }
                }
                else
                {
                    ListBox_Areas.Items.Add(area);
                }
            }
        }
    }
}