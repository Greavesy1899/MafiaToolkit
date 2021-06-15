using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml;

namespace Utils.Settings
{
    public enum GamesEnumerator
    {
        None = 0,
        MafiaII = 1,
        MafiaII_DE = 2,
        MafiaIII = 3,
        MafiaI_DE = 4
    }
    public class Game
    {
        private string logo;
        private string name;
        private string description;
        private string directoryKey;
        private string materialKey;
        private GamesEnumerator gameType;

        public string Logo {
            get { return logo; }
        }
        public string Name {
            get { return name; }
        }
        public string Description {
            get { return description; }
        }
        public string Directory {
            get { return ToolkitSettings.ReadDirectoryKey(directoryKey); }
            set { ToolkitSettings.WriteDirectoryKey(directoryKey, value); }
        }
        public string Materials {
            get { return ToolkitSettings.ReadKey(materialKey, "Materials", ""); }
            set { ToolkitSettings.WriteKey(materialKey, "Materials", value); }
        }
        public GamesEnumerator GameType {
            get { return gameType; }
        }

        public Game(string name, string description, string logo, string directoryKey, string materialKey, GamesEnumerator type)
        {
            this.name = name;
            this.description = description;
            this.logo = logo;
            this.directoryKey = directoryKey;
            this.materialKey = materialKey;
            this.gameType = type;
        }
    }

    public sealed class GameStorage
    {
        private List<Game> games = new List<Game>();
        private Game selectedGame;

        public List<Game> Games {
            get { return games; }
        }

        public void InitStorage()
        {
            games.Clear();

            // TODO: Maybe we should ask to create a new one if this file does not exist?
            string ExePath = Assembly.GetExecutingAssembly().Location;
            FileInfo Exe = new FileInfo(ExePath);
            string GamesFile = Path.Combine(Exe.Directory.FullName, "games.xml");
            if(!File.Exists(GamesFile))
            {
                MessageBox.Show("Could not start due to missing file: games.xml. Please setup this file.", "Toolkit", MessageBoxButton.OK);
                return;
            }

            XmlDocument document = new XmlDocument();
            document.Load("games.xml");
            var nav = document.CreateNavigator();
            var nodes = nav.Select("/Games/Game");
            while (nodes.MoveNext() == true)
            {
                if (nodes.Current.MoveToFirstChild())
                {
                    var name = nodes.Current.Value;
                    nodes.Current.MoveToNext();
                    var description = nodes.Current.Value;
                    nodes.Current.MoveToNext();
                    var logoPath = nodes.Current.Value;
                    nodes.Current.MoveToNext();
                    var directoryKey = nodes.Current.Value;
                    nodes.Current.MoveToNext();
                    var materialKey = nodes.Current.Value;
                    nodes.Current.MoveToNext();
                    var type = nodes.Current.Value;

                    GamesEnumerator gameType = GamesEnumerator.None;
                    Enum.TryParse(type, out gameType);
                    Game newGame = new Game(name, description, logoPath, directoryKey, materialKey, gameType);
                    games.Add(newGame);
                }
            }
        }

        public void SetSelectedGame(Game game)
        {
            selectedGame = game;
        }

        public void SetSelectedGameByIndex(int index)
        {
            selectedGame = games[index];
        }

        public Game GetSelectedGame()
        {
            return selectedGame;
        }

        public static string GetExecutableName(GamesEnumerator type)
        {
            if (type == GamesEnumerator.MafiaII)
            {
                return "mafia2.exe";
            } 
            else if(type == GamesEnumerator.MafiaII_DE)
            {
                return "mafia ii definitive edition.exe";
            }
            else if (type == GamesEnumerator.MafiaIII)
            {
                return "mafia3definitiveedition.exe";
            }
            else if (type == GamesEnumerator.MafiaI_DE)
            {
                return "mafiadefinitiveedition.exe";
            }
            else
            {
                return "";
            }
        }

        public static GameStorage Instance {
            get {
                return Nested.instance;
            }
        }

        class Nested
        {
            static Nested()
            {
            }

            internal static readonly GameStorage instance = new GameStorage();
        }
    }
}
