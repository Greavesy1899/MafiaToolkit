using System;
using System.Text;
using System.Windows.Forms;
using Toolkit.Controls;
using Utils.Language;
using Utils.Settings;

namespace Toolkit.Forms
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
                //entry.Tag = i;
                entry.InitialiseEntry(games[i]);
                //FlowPanel_GamesList.Controls.Add(entry);
                entry.GetStartButton().Click += StartToolkit_OnClicked;
            }

            var size = this.Size;
            size.Width = (FlowPanel_GamesList.Size.Width + 48);
            size.Height = (FlowPanel_GamesList.Size.Height + 24);
            this.Size = size;
        }

        private void StartToolkit_OnClicked(object sender, EventArgs e)
        {
            //Button button = (sender as Button);
            //ControlGameEntry entry = (button.Parent as ControlGameEntry);
            //var game = entry.GetGame();
            //GameStorage.Instance.SetSelectedGame(game);
            //
            //if(CheckBox_SelectAsDefault.Checked)
            //{
            //    var selectedIndex = Convert.ToInt32(entry.Tag);
            //    SaveDefaultGame(selectedIndex);
            //}
            //
            //DialogResult = DialogResult.OK;
            //Close();
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

        private void FlowPanel_GamesList_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
