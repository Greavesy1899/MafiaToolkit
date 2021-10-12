using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Utils.Settings;
using Utils.Language;
using System.IO;

namespace MafiaToolkit.Controls
{
    public partial class ControlGameEntry : UserControl
    {
        private Game game;

        public ControlGameEntry()
        {
            InitializeComponent();
            LocaliseEntry();
        }

        public void LocaliseEntry()
        {
            Label_FolderPath.Text = Language.GetString("$FOLDER_PATH_LABEL");
            Button_SelectFolder.Text = Language.GetString("$SELECT_FOLDER");
            Button_Start.Text = Language.GetString("$START_TOOLKIT");
        }

        public void InitialiseEntry(Game game)
        {
            this.game = game;
            Label_GameName.Text = game.Name;
            Label_GameDescription.Text = game.Description;
            Label_GameType.Text = game.GameType.ToString();

            if(File.Exists(game.Logo))
            {
                Picture_GameIcon.Image = Image.FromFile(game.Logo);
                Label_MissingImage.Visible = false;
            }

            TextBox_FolderPath.Text = game.Directory;
            ValidatePath();
        }

        public Game GetGame()
        {
            return game;
        }

        public Button GetStartButton()
        {
            return Button_Start;
        }

        private void ValidatePath(bool debug = false)
        {
            var executable = GameStorage.GetExecutableName(game.GameType);
            var path = game.Directory;
            var isValid = false;

            if (!string.IsNullOrEmpty(path))
            {
                DirectoryInfo directory = new DirectoryInfo(game.Directory);
                FileInfo file = new FileInfo(game.Directory + "\\" + executable);
                isValid = file.Exists;
            }

            if(isValid)
            {
                //TODO add tick here..
                Button_Start.Enabled = true;
                Picture_Status.Image = Image.FromFile("Resources/tick.png");
            }
            else
            {
                Button_Start.Enabled = false;
                Picture_Status.Image = Image.FromFile("Resources/cross.png");

                if (debug)
                {
                    MessageBox.Show(string.Format("Failed to find correct executable: {0}!", executable), "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }      
        }

        private void SetFolderPath(string path, bool debug = false)
        {
            game.Directory = path;
            TextBox_FolderPath.Text = path;
            ValidatePath(debug);
        }

        private void Button_SelectFolder_OnClick(object sender, EventArgs e)
        {
            if (FolderDialog_MafiaFolder.ShowDialog() == DialogResult.OK)
            {
                var path = FolderDialog_MafiaFolder.SelectedPath;
                SetFolderPath(path, true);
            }
        }

        private void TextBox_FolderPath_OnTextChanged(object sender, EventArgs e)
        {
            var path = TextBox_FolderPath.Text;
            SetFolderPath(path, false);
        }
    }
}
