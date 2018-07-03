using System;
using System.IO.Compression;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Mafia2ToolUpdater
{
    public partial class updateForm : Form
    {
        public updateForm()
        {
            InitializeComponent();
            DownloadUpdate();
        }

        public void DownloadUpdate()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progressChanged);
            
            try
            {
                percentageLabel.Text = "0%";

                if (!Directory.Exists("update/"))
                    Directory.CreateDirectory("update/");

                webClient.DownloadFileAsync(new Uri("https://www.dropbox.com/s/fb7o8w5ttcaeug5/Debug.zip?dl=1"), "update/Update.zip");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void completed(object sender, AsyncCompletedEventArgs e)
        {
            ZipArchive archive = new ZipArchive(File.Open("update/Update.zip", FileMode.Open));
            archive.ExtractToDirectory(Application.StartupPath, true);
            archive = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Directory.Delete("update", true);
            MessageBox.Show("Completed! This program will close.", "Updater");
            Application.Exit();
        }

        private void progressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            percentageLabel.Text = e.ProgressPercentage.ToString() + "%";
        }
    }

    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name == "")
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }
    }
}
