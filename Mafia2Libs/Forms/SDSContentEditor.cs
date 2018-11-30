using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace Mafia2Tool
{
    public partial class SDSContentEditor : Form
    {
        public SDSContentEditor()
        {
            InitializeComponent();
            Localise();
        }

        public SDSContentEditor(FileInfo info)
        {
            InitializeComponent();
            Localise();
            LoadSDSContent(info);
            ShowDialog();
        }

        private void Localise()
        {


        }

        private TreeNode BuildTreeNode(string name, DirectoryInfo info)
        {
            TreeNode node = new TreeNode();
            node.Name = name + "Node";
            node.Text = name;
            node.Tag = new FileInfo(Path.Combine(info.FullName, name));
            return node;
        }

        private void LoadSDSContent(FileInfo info)
        {
            if (info.Name != "SDSContent" && info.Extension != "xml")
                return;

            XmlDocument document = new XmlDocument();
            document.Load(info.FullName);

            XPathNavigator nav = document.CreateNavigator();
            var nodes = nav.Select("/SDSResource/ResourceEntry");
            while (nodes.MoveNext() == true)
            {
                nodes.Current.MoveToFirstChild();
                string resourceType = nodes.Current.Value;
                nodes.Current.MoveToNext();
                string file = nodes.Current.Value;

                switch (resourceType)
                {
                    case "IndexBufferPool":
                        treeView1.Nodes[13].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                }
            }
        }

        private void autoAddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
