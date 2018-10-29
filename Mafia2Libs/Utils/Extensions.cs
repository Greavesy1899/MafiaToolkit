using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Extensions.TreeViewCollection
{
    //Taken from stackoverflow, not my code https://stackoverflow.com/questions/2273577/how-to-go-from-treenode-fullpath-data-and-get-the-actual-treenode;
    public static class TreeNodeCollectionUtils
    {
        public static TreeNode FindTreeNodeByFullPath(this TreeNodeCollection collection, string fullPath, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            var foundNode = collection.Cast<TreeNode>().FirstOrDefault(tn => string.Equals(tn.FullPath, fullPath, comparison));
            if (null == foundNode)
            {
                foreach (var childNode in collection.Cast<TreeNode>())
                {
                    var foundChildNode = FindTreeNodeByFullPath(childNode.Nodes, fullPath, comparison);
                    if (null != foundChildNode)
                    {
                        return foundChildNode;
                    }
                }
            }

            return foundNode;
        }
    }

    public static class FileInfoUtils
    {
        public static string CalculateFileSize(this FileInfo file)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = file.Length;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}