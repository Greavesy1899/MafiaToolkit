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
            //check for MafiaIIToolkit.exe
            if (args.Length >= 1)
                toolkitFolder = args[0];
            else
                Console.WriteLine("WARNING: Did not find folder in arguments[0]. Checking parent directory.");
            if (toolkitFolder == null)
            {
                if (!File.Exists("Mafia2Toolkit.exe"))
                    return;

                Console.WriteLine("Found Mafia2Toolkit.exe!");
            }

            //checks complete.. now to get the actual download package...
            Console.WriteLine("Fetching Latest Package...");

            GitHubClient client = new GitHubClient(new ProductHeaderValue("ToolkitUpdater", "1"));
            GetLatest(client).Wait();

            //Download the latest release.
            if (DownloadRelease())
                Console.WriteLine("Succesfully downloaded the latest release.");

            Update();

            File.Delete("latest.zip");
            Directory.Delete("release");

            //finish up..
            Console.WriteLine("");
            Console.WriteLine("Completed update. Press any key to close this window.");
            Console.ReadKey();
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

                if (release.TagName.Contains("v1"))
                    throw new Exception("Found an update but cannot download it. Please manually go to the github page to update. Thanks!");

                Console.WriteLine("");
                Console.WriteLine("Found the latest release:");
                Console.WriteLine("Name: {0}", release.Name);
                Console.WriteLine("Tag: {0}", release.TagName);
                Console.WriteLine("Data: {0}", release.CreatedAt);
                Console.WriteLine("");
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

            foreach (FileInfo file in releaseInfo.GetFiles())
            {
                if (file.Name == "ToolkitUpdater.exe") //skip updater, if needed i'll just do version checking later.
                    continue;

                if (File.Exists(Path.Combine(toolkitInfo.FullName, file.Name)))
                    File.Delete(Path.Combine(toolkitInfo.FullName, file.Name));

                File.Move(file.FullName, Path.Combine(toolkitInfo.FullName, file.Name));
                Console.WriteLine("Succesfully moved file: {0}", file.Name);
            }

            foreach (DirectoryInfo directory in releaseInfo.GetDirectories())
            {
                if (Directory.Exists(Path.Combine(toolkitInfo.FullName, directory.Name + @"\")))
                    Directory.Delete(Path.Combine(toolkitInfo.FullName, directory.Name + @"\"), true);

                Directory.Move(directory.FullName, Path.Combine(toolkitInfo.FullName, directory.Name + @"\"));
                Console.WriteLine("Succesfully moved folder: {0}", directory.Name);
            }
        }
    }
}
