using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace ToolkitUpdater
{
    class Program
    {
        static Release release;
        static void Main(string[] args)
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue("ToolkitUpdater", "1"));

            release = client.Repository.Release.GetLatest("Greavesy1899", "Mafia2Toolkit").Result;
            Console.WriteLine("The latest release is tagged at {0} and is named {1}", release.TagName, release.Name);
        }

        static async Task GetLatest(GitHubClient client)
        {
            var release = await client.Repository.Release.GetLatest("Greavesy1899", "Mafia2Toolkit");
            Console.WriteLine("The latest release is tagged at {0} and is named {1}", release.TagName, release.Name);
        }
    }
}
