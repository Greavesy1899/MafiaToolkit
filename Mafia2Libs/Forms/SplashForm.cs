using System;
using System.IO;
using System.Windows.Forms;
using Mafia2;
using ApexSDK;
using System.Threading;
using System.ComponentModel;
using Utils.Settings;
using Utils.Lang;
using Utils.Logging;

namespace Mafia2Tool
{
    public partial class SplashForm : Form
    {
        public bool done = false;
        public SplashForm()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = false;
        }

        public void InitToolkit()
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void BGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //setup logger.
            Log.DeleteOldLog();

            //do vital inits;
            CheckINIExists();
            ToolkitSettings.ReadINI();
            backgroundWorker1.ReportProgress(10, "Read INI Settings..");

            Language.ReadLanguageXML();

            //SystemClass.StartRenderForm("Model Viewer", 1024, 720, true, "Model", false, 0);
            MaterialData.Load();
        }

        private static void CheckINIExists()
        {
            if (File.Exists("Mafia2Tool.ini"))
                return;
            else
                new IniFile();
        }

        private void BGWorker_UpdateUI(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            listBox1.Items.Add(e.UserState);
        }

        private void BGWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Hide();
        }
    }
}