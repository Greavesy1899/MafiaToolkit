using Core.IO;
using Mafia2Tool.Forms;
using Mafia2Tool.MCP;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toolkit.Forms;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ToolkitAssemblyLoadContext.SetupLoadContext();
            ToolkitExceptionHandler.Initialise();

            // Start MCP server in background for AI assistant integration
            McpServerHost.Start();

            if (args.Length > 0)
            {
                CheckINIExists();
                ToolkitSettings.ReadINI();
                ProcessCommandArguments(args);
                McpServerHost.Stop();
                return;
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Load INI
            CheckINIExists();
            ToolkitSettings.ReadINI();

            // Register BIN file editors with the factory
            // This decouples Core.IO from Forms - see BinEditorRegistration.cs
            Mafia2Tool.Forms.BinEditorRegistration.Initialize();
            CheckIfNewUpdate();

            GameStorage.Instance.InitStorage();
            Language.ReadLanguageXML();
            CheckLatestRelease();

            if (ToolkitSettings.SkipGameSelector)
            {
                GameStorage.Instance.SetSelectedGameByIndex(ToolkitSettings.DefaultGame);
                OpenGameExplorer();
                McpServerHost.Stop();
                return;
            }

            GameSelector selector = new GameSelector();
            if (selector.ShowDialog() == DialogResult.OK)
            {
                selector.Dispose();
                OpenGameExplorer();
            }

            McpServerHost.Stop();
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
            try
            {
                Octokit.GitHubClient client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("ToolkitUpdater", "1"));
                GetLatest(client).Wait();
            }
            catch (Exception)
            {
                MessageBox.Show(Language.GetString("$FAILED_UPDATE_CHECK"), "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static async Task GetLatest(Octokit.GitHubClient client)
        {
            //NOTE: Getting the very latest release causes an exception, so we need to use GetAll().
            var releases = await client.Repository.Release.GetAll("Greavesy1899", "MafiaToolkit");
            var release = releases[0];
            var version = release.TagName.Replace("v", "");
            version = version.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            float.TryParse(version, out float value);
            if (ToolkitSettings.Version < value)
            {
                string message = string.Format("{0}\n\n{1}\n{2}", Language.GetString("$UPDATE_MESSAGE1"), Language.GetString("$UPDATE_MESSAGE2"), Language.GetString("$UPDATE_MESSAGE3"));
                var result = MessageBox.Show(message, "Toolkit update", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    ProcessStartInfo StartInfo = new ProcessStartInfo();
                    StartInfo.UseShellExecute = true;
                    StartInfo.FileName = "https://github.com/Greavesy1899/MafiaToolkit/releases";

                    Process.Start(StartInfo);
                }
            }
        }

        private static void CheckIfNewUpdate()
        {
            if (ToolkitSettings.CurrentVersion != ToolkitSettings.Version)
            {
                string UpdateMessage = string.Format("Welcome to update {0}! \nPress 'Ok' to visit the changelist on the wiki. \nPress 'Cancel' to continue.", ToolkitSettings.Version);
                if (MessageBox.Show(UpdateMessage, "Toolkit", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    ProcessStartInfo StartInfo = new ProcessStartInfo();
                    StartInfo.UseShellExecute = true;
                    StartInfo.FileName = "https://github.com/Greavesy1899/MafiaToolkit/wiki/Toolkit-Changelist";

                    Process.Start(StartInfo);
                }

                // Write new version
                ToolkitSettings.CurrentVersion = ToolkitSettings.Version;
                ToolkitSettings.WriteKey("CurrentVersion", "Update", ToolkitSettings.CurrentVersion.ToString());
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

    public static class ToolkitAssemblyLoadContext
    {
        private static bool bAppliedCallback = false;

        public static void SetupLoadContext()
        {
            if (!bAppliedCallback)
            {
                AssemblyLoadContext.Default.Resolving += Default_Resolving;
                bAppliedCallback = true;
            }
        }

        private static System.Reflection.Assembly Default_Resolving(AssemblyLoadContext ALC, System.Reflection.AssemblyName AssemblyName)
        {
            string probeSetting = AppContext.GetData("SubdirectoriesToProbe") as string;
            if (string.IsNullOrEmpty(probeSetting))
            {
                return null;
            }

            foreach (string subdirectory in probeSetting.Split(';'))
            {
                string pathMaybe = Path.Combine(AppContext.BaseDirectory, subdirectory, $"{AssemblyName.Name}.dll");
                if (File.Exists(pathMaybe))
                {
                    return ALC.LoadFromAssemblyPath(pathMaybe);
                }
            }

            return null;
        }
    }

    public static class ToolkitExceptionHandler
    {
        public static void Initialise()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                ToolkitExceptionHandler.ShowExceptionForm((Exception)e.ExceptionObject);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                ToolkitExceptionHandler.ShowExceptionForm(e.Exception);
            }
        }

        private static void ShowExceptionForm(Exception InException)
        {
            ExceptionForm Form = new ExceptionForm();
            Form.ShowException(InException);

            DialogResult Result = Form.ShowDialog();
            if (Result == DialogResult.No)
            {
                Application.Exit();
            }
        }
    }
}
