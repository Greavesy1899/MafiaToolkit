using System;
using System.IO;
using System.Windows;
using Utils.Settings;
using Utils.Language;
using System.Threading.Tasks;
using System.Diagnostics;
using MafiaToolkit.Forms;
using Core.IO;
using System.Text;
using System.Runtime.Loader;

namespace MafiaToolkit
{
    public partial class App : Application
    {
        public App()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            ToolkitAssemblyLoadContext.SetupLoadContext();
            ToolkitExceptionHandler.Initialise();

            // Load INI
            CheckINIExists();
            ToolkitSettings.ReadINI();
            CheckIfNewUpdate();

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
            if (selector.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selector.Dispose();
                OpenGameExplorer();
            }

            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
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
                catch (Exception)
                {
                    MessageBox.Show(Language.GetString("$FAILED_UPDATE_CHECK"), "Toolkit", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
                var result = MessageBox.Show(message, "Toolkit update", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    Process.Start("https://github.com/Greavesy1899/MafiaToolkit/releases");
                }
            }
        }

        private static void CheckIfNewUpdate()
        {
            if (ToolkitSettings.CurrentVersion != ToolkitSettings.Version)
            {
                string UpdateMessage = string.Format("Welcome to update {0}! \nPress 'Ok' to visit the changelist on the wiki. \nPress 'Cancel' to continue.", ToolkitSettings.Version);
                if (MessageBox.Show(UpdateMessage, "Toolkit", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    ProcessStartInfo StartInfo = new ProcessStartInfo();
                    StartInfo.UseShellExecute = true;
                    StartInfo.FileName = "https://github.com/Greavesy1899/MafiaToolkit/wiki/Toolkit-Changelist";

                    Process.Start(StartInfo);
                }

                ToolkitSettings.CurrentVersion = ToolkitSettings.Version;
            }
        }

        private static void CheckINIExists()
        {
            string PathToIni = Path.Combine(System.Windows.Forms.Application.ExecutablePath, "MafiaToolkit.ini");
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
            System.Windows.Forms.Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
        }
    }
}
