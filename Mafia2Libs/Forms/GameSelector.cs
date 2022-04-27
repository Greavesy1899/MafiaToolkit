using System;
using System.Text;
using System.Windows.Forms;
using Mafia2Tool.Controls;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool.Forms
{
    public partial class GameSelector : Form
    {
        public GameSelector()
        {
            InitializeComponent();
            LocaliseForm();
            Init();
        }

        public void LocaliseForm()
        {
            CheckBox_SelectAsDefault.Text = Language.GetString("$SELECT_AS_DEFAULT");

            StringBuilder builder = new StringBuilder("Toolkit v");
            builder.Append(ToolkitSettings.Version);
            Label_ToolkitVersion.Text = builder.ToString();
        }

        public void Init()
        {
            CheckBox_SelectAsDefault.Checked = ToolkitSettings.SkipGameSelector;
            var games = GameStorage.Instance.Games;

            for(int i = 0; i < games.Count; i++)
            {
                ControlGameEntry entry = new ControlGameEntry();
                entry.Tag = i;
                entry.InitialiseEntry(games[i]);
                FlowPanel_GamesList.Controls.Add(entry);
                entry.GetStartButton().Click += StartToolkit_OnClicked;
            }

            var size = this.Size;
            size.Width = (FlowPanel_GamesList.Size.Width + 48);
            size.Height = (FlowPanel_GamesList.Size.Height + 24);
            this.Size = size;
        }

        private void StartToolkit_OnClicked(object sender, EventArgs e)
        {
            Button button = (sender as Button);
            ControlGameEntry entry = (button.Parent as ControlGameEntry);
            var game = entry.GetGame();
            GameStorage.Instance.SetSelectedGame(game);

            if(CheckBox_SelectAsDefault.Checked)
            {
                var selectedIndex = Convert.ToInt32(entry.Tag);
                SaveDefaultGame(selectedIndex);
            }

            DialogResult = DialogResult.OK;

            string[] MaterialFileNames = { "default.mtl", "default50.mtl", "default60.mtl" };
            switch (GameStorage.Instance.GetSelectedGame().GameType)
            {
                case GamesEnumerator.MafiaII:
                    string Mafia2MaterialsPath = System.IO.Directory.GetParent(ToolkitSettings.ReadDirectoryKey("M2_Directory")).ToString() + "\\edit\\materials\\";
                    string Mafia2MaterialsValue =
                        Mafia2MaterialsPath + MaterialFileNames[0] + "," +
                        Mafia2MaterialsPath + MaterialFileNames[1] + "," +
                        Mafia2MaterialsPath + MaterialFileNames[2] + ",";
                    GameStorage.Instance.GetSelectedGame().Materials = Mafia2MaterialsValue;
                    break;
                case GamesEnumerator.MafiaII_DE:
                    string Mafia2DeMaterialsPath = System.IO.Directory.GetParent(ToolkitSettings.ReadDirectoryKey("M2DE_Directory")).ToString() + "\\edit\\materials\\";
                    string Mafia2DeMaterialsValue =
                        Mafia2DeMaterialsPath + MaterialFileNames[0] + "," +
                        Mafia2DeMaterialsPath + MaterialFileNames[1] + "," +
                        Mafia2DeMaterialsPath + MaterialFileNames[2] + ",";
                    GameStorage.Instance.GetSelectedGame().Materials = Mafia2DeMaterialsValue;
                    break;
                case GamesEnumerator.MafiaI_DE:
                    string Mafia1DeMaterialsPath = ToolkitSettings.ReadDirectoryKey("M1DE_Directory") + "\\edit\\materials\\";
                    string Mafia1DeMaterialsValue =
                    Mafia1DeMaterialsPath + MaterialFileNames[0] + ",";
                    GameStorage.Instance.GetSelectedGame().Materials = Mafia1DeMaterialsValue;
                    break;
                case GamesEnumerator.MafiaIII:
                    string Mafia3MaterialsPath = ToolkitSettings.ReadDirectoryKey("M3_Directory") + "\\edit\\materials\\";
                    string Mafia3MaterialsValue =
                    Mafia3MaterialsPath + MaterialFileNames[0] + ",";
                    GameStorage.Instance.GetSelectedGame().Materials = Mafia3MaterialsValue;
                    break;
                default:
                    break;
            }

            Close();
        }

        private void CheckBox_SelectAsDefault_OnChecked(object sender, EventArgs e)
        {
            ToolkitSettings.SkipGameSelector = CheckBox_SelectAsDefault.Checked;
            ToolkitSettings.WriteKey("SkipGameSelector", "Misc", ToolkitSettings.SkipGameSelector.ToString());
        }

        private void SaveDefaultGame(int index)
        {
            ToolkitSettings.DefaultGame = index;
            ToolkitSettings.WriteKey("DefaultGame", "Misc", ToolkitSettings.DefaultGame.ToString());
        }
    }
}
