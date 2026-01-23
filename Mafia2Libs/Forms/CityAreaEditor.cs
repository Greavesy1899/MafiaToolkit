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

        private bool bIsFileEdited = false;

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
            DeleteArea.Text = Language.GetString("$DELETE_AREA");
        }

        private void BuildData()
        {
            areas = new CityAreas(cityAreasFile.FullName);

            for (int i = 0; i != areas.AreaCollection.Count; i++)
            {
                ListBox_Areas.Items.Add(areas.AreaCollection[i]);
            }
        }

        private void AddArea()
        {
            CityAreas.AreaData area = new CityAreas.AreaData();
            area.Create();
            areas.AreaCollection.Add(area);
            ListBox_Areas.Items.Add(area);
            ListBox_Areas.SelectedItem = area;

            Text = Language.GetString("$CITY_AREA_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
    }

        private void Save()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(cityAreasFile.FullName, FileMode.Create)))
            {
                areas.WriteToFile(writer);
            }

            Text = Language.GetString("$CITY_AREA_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            Clear();
            BuildData();

            PropertyGrid_Area.SelectedObject = null;
            ListBox_Areas.SelectedItem = null;

            Text = Language.GetString("$CITY_AREA_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Delete()
        {
            if (ListBox_Areas.SelectedItem != null)
            {
                areas.AreaCollection.Remove((CityAreas.AreaData)ListBox_Areas.SelectedItem);
                ListBox_Areas.Items.Remove(ListBox_Areas.SelectedItem);

                Text = Language.GetString("$CITY_AREA_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void Clear()
        {
            ListBox_Areas.Items.Clear();
        }

        private void UpdateAreaData(object sender, EventArgs e)
        {
            if (ListBox_Areas.SelectedIndex != -1)
            {
                PropertyGrid_Area.SelectedObject = ListBox_Areas.SelectedItem;
            }
        }

        private void PropertyGrid_Area_ValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var area = (ListBox_Areas.SelectedItem as CityAreas.AreaData);
            var index = ListBox_Areas.SelectedIndex;
            ListBox_Areas.Items.RemoveAt(index);
            ListBox_Areas.Items.Insert(index, area);
            ListBox_Areas.SelectedItem = area;

            Text = Language.GetString("$CITY_AREA_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
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

        private void CityAreaEditor_Closing(object sender, FormClosingEventArgs e)
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

        private void AddAreaButton_Click(object sender, EventArgs e) => AddArea();
        private void SaveButton_Click(object sender, EventArgs e) => Save();
        private void ReloadButton_Click(object sender, EventArgs e) => Reload();
        private void ExitButton_Click(object sender, EventArgs e) => Close();
        private void DeleteArea_Click(object sender, EventArgs e) => Delete();
    }
}