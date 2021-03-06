using System;
using System.IO;
using System.Windows.Forms;
using Utils.Settings;
using Utils.Language;
using System.Threading.Tasks;
using System.Diagnostics;
using Mafia2Tool.Forms;
using Core.IO;

namespace Mafia2Tool
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                CheckINIExists();
                ToolkitSettings.ReadINI();
                ProcessCommandArguments(args);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CheckINIExists();
            ToolkitSettings.ReadINI();
            GameStorage.Instance.InitStorage();
            Language.ReadLanguageXML();
            CheckLatestRelease();

            if (ToolkitSettings.SkipGameSelector)
            {
                GameStorage.Instance.SetSelectedGameByIndex(ToolkitSettings.DefaultGame);
                OpenGameExplorer();
                return;
            }

            GameSelector selector = new GameSelector();
            if(selector.ShowDialog() == DialogResult.OK)
            {
                selector.Dispose();
                OpenGameExplorer();
            }
        }

        private static void ProcessCommandArguments(string[] Args)
        {
            Cursor.Current = Cursors.WaitCursor;
            if(Args[0].Equals("-gt"))
            {
                GamesEnumerator GameType = (GamesEnumerator)Enum.Parse(typeof(GamesEnumerator), Args[1]);

                if(Args[2].Equals("-SDSPack"))
                {
                    string SDSPath = Args[3];
                    string ExportPath = Args[4];

                    FileInfo SDSFileInfo = new FileInfo(SDSPath);
                    FileSDS SDSFile = new FileSDS(SDSFileInfo);
                    SDSFile.SaveSDSWithCustomFolder(GameType, ExportPath);
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private static void OpenGameExplorer()
        {
            GameExplorer explorer = new GameExplorer();
            explorer.ShowDialog();
            explorer.Dispose();
        }

        private static void CheckLatestRelease()
        {
            if (ToolkitSettings.CheckForUpdates)
            {
                try
                {
                    Octokit.GitHubClient client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("ToolkitUpdater", "1"));
                    GetLatest(client).Wait();
                }
                catch(Exception)
                {
                    MessageBox.Show(Language.GetString("$FAILED_UPDATE_CHECK"), "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private static async Task GetLatest(Octokit.GitHubClient client)
        {
            //NOTE: Getting the very latest release causes an exception, so we need to use GetAll().
            var releases = await client.Repository.Release.GetAll("Greavesy1899", "Mafia2Toolkit");
            var release = releases[0];
            var version = release.TagName.Replace("v", "");
            version = version.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            float value = 0.0f;
            float.TryParse(version, out value);
            if (ToolkitSettings.Version < value)
            {
                string message = string.Format("{0}\n\n{1}\n{2}", Language.GetString("$UPDATE_MESSAGE1"), Language.GetString("$UPDATE_MESSAGE2"), Language.GetString("$UPDATE_MESSAGE3"));
                var result = MessageBox.Show(message, "Toolkit", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    Process.Start("https://github.com/Greavesy1899/Mafia2Toolkit/releases");
                }
            }
        }

        private static void CheckINIExists()
        {
            string PathToIni = Path.Combine(Application.ExecutablePath, "MafiaToolkit.ini");
            if (!File.Exists(PathToIni))
            {
                new IniFile();
            }
        }
    }
}
