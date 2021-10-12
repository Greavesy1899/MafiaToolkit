using System;
using System.Text;
using System.Windows;
using WinForms = System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Toolkit.Controls;
using Lang = Utils.Language;
using Utils.Settings;

namespace Toolkit.Xaml
{
    /// <summary>
    /// Interaction logic for GameSelector.xaml
    /// </summary>
    public partial class GameSelector : Window
    {
        private GamesViewModel ViewModel;
        public GameSelector()
        {
            InitializeComponent();
            Localise();
            ViewModel = new GamesViewModel();
            Listbox_Games.DataContext = ViewModel;
            Listbox_Games.ItemsSource = ViewModel.Controls;
            CheckBox_SelectAsDefault.IsChecked = ToolkitSettings.SkipGameSelector;
            UpdateListbox();
        }

        public void Localise()
        {
            CheckBox_SelectAsDefault.Content = Lang.Language.GetString("$SELECT_AS_DEFAULT");

            StringBuilder builder = new StringBuilder("Toolkit v");
            builder.Append(ToolkitSettings.Version);
            Label_ToolkitVersion.Content = builder.ToString();
        }

        public void UpdateListbox()
        {
            var games = GameStorage.Instance.Games;
            ViewModel.Controls.Clear();

            for (int i = 0; i < games.Count; i++)
            {
                ControlGameEntry entry = new ControlGameEntry();
                entry.InitialiseEntry(games[i]);
                ViewModel.Controls.Add(entry);
            }
        }

        private void ListBox_Games_MouseDoubleClick(object sender, EventArgs e)
        {
            ControlGameEntry entry = Listbox_Games.SelectedItem as ControlGameEntry;
            var game = entry.GetGame();
            GameStorage.Instance.SetSelectedGame(game);
            
            if(CheckBox_SelectAsDefault.IsChecked.Value)
            {
                var selectedIndex = Convert.ToInt32(Listbox_Games.SelectedIndex);
                SaveDefaultGame(selectedIndex);
            }
            Close();
            OpenGameExplorer();
        }

        private void Button_SelectFolder_OnClick(object sender, EventArgs e)
        {
            Button Button_SelectFolder = sender as Button;
            WinForms.FolderBrowserDialog FolderDialog_MafiaFolder = new WinForms.FolderBrowserDialog();
            if (FolderDialog_MafiaFolder.ShowDialog() == WinForms.DialogResult.OK)
            {
                var path = FolderDialog_MafiaFolder.SelectedPath;

                ControlGameEntry entry = Button_SelectFolder.DataContext as ControlGameEntry;
                entry.SetFolderPath(path, true);
                UpdateListbox();
            }
        }

        private void SaveDefaultGame(int index)
        {
            ToolkitSettings.DefaultGame = index;
            ToolkitSettings.WriteKey("DefaultGame", "Misc", ToolkitSettings.DefaultGame.ToString());
        }

        private static void OpenGameExplorer()
        {
            GameExplorer explorer = new GameExplorer();
            explorer.ShowDialog();
            explorer.Dispose();
        }
    }

    public class GamesViewModel
    {
        public ObservableCollection<ControlGameEntry> Controls;

        public GamesViewModel(List<ControlGameEntry> ctrl)
        {
            Controls = new ObservableCollection<ControlGameEntry>(ctrl);
        }

        public GamesViewModel()
        {
            Controls = new ObservableCollection<ControlGameEntry>();
        }

    }
}
