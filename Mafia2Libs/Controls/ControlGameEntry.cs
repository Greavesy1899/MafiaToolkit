using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Utils.Settings;
using Utils.Language;
using System.IO;

namespace Toolkit.Controls
{
    public class ControlGameEntry
    {
        public Game game { get; set; }

        public ControlGameEntry() { }

        public void InitialiseEntry(Game game)
        {
            this.game = game;
            ValidatePath();
        }

        public Game GetGame()
        {
            return game;
        }

        public Button GetStartButton()
        {
            return null;
        }

        public bool ValidatePath(bool debug = false)
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

            if(!isValid && debug)
            {
                MessageBox.Show(string.Format("Failed to find correct executable: {0}!", executable), "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isValid;    
        }

        public void SetFolderPath(string path, bool debug = false)
        {
            game.Directory = path;
            ValidatePath(debug);
        }
    }
}
