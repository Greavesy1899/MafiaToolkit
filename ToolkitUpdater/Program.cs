using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;
using Octokit;

namespace ToolkitUpdater
{
    class Program
    {
        static Release release;
        static string toolkitFolder;

        static void Main(string[] args)
        {
            
            if(args.Length >= 1)
                toolkitFolder = args[0];

            if (toolkitFolder == null)
            {
                if (!File.Exists("Mafia2Toolkit.exe"))
                    return;

                Console.WriteLine("Found Mafia2Toolkit.exe");
            }

            GitHubClient client = new GitHubClient(new ProductHeaderValue("ToolkitUpdater", "1"));
            GetLatest(client).Wait();

            //Download the latest release.
            if (DownloadRelease())
                Console.WriteLine("Succesfully downloaded the latest release.");

            Update();
        }

        /// <summary>
        /// Get the latest release from the github repo.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        static async Task GetLatest(GitHubClient client)
        {
            try
            {
                var releases = await client.Repository.Release.GetAll("Greavesy1899", "Mafia2Toolkit");
                release = releases[0];
                Console.WriteLine("Found the latest release:");
                Console.WriteLine("Name: {0}", release.Name);
                Console.WriteLine("Tag: {0}", release.TagName);
                Console.WriteLine("Data: {0}", release.CreatedAt);
            }
            catch
            {
                Console.WriteLine("Unable to fetch the latest releases.");
            }
        }

        /// <summary>
        /// Download the latest release and store in updaters folder.
        /// </summary>
        /// <returns></returns>
        static bool DownloadRelease()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(release.Assets[0].BrowserDownloadUrl, "latest.zip");

                    if (!Directory.Exists("release"))
                        Directory.CreateDirectory("release");

                    ZipFile.ExtractToDirectory("latest.zip", "release");
                    Console.WriteLine("Succesfully extracted the package.");
                    return true;
                }
               
            }
            catch
            {
                Console.WriteLine("Failed to download the latest release.");
                return false;
            }
        }

        static void Update()
        {
            DirectoryInfo releaseInfo = new DirectoryInfo("release");
            DirectoryInfo toolkitInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

            UpdateFiles(releaseInfo, toolkitInfo);

            foreach (DirectoryInfo directory in releaseInfo.GetDirectories())
            {
                UpdateFiles(directory, toolkitInfo);
            }
        }

        static void UpdateFiles(DirectoryInfo fromDirectory, DirectoryInfo toDirectory)
        {
            string rootName = Directory.GetCurrentDirectory();

            foreach (FileInfo file in fromDirectory.GetFiles())
            {
                string filepath = Path.Combine(toDirectory.FullName, file.Name);

                if (File.Exists(Path.Combine(toDirectory.FullName, file.Name)))
                    File.Delete(Path.Combine(toDirectory.FullName, file.Name));

                if (rootName == toDirectory.FullName)
                    File.Move(file.FullName, filepath);
                else
                    File.Move(file.FullName, Path.Combine(filepath, fromDirectory.Name));

                Console.WriteLine("Succesfully Updated {0}", file.Name);
            }
        }
    }
}
