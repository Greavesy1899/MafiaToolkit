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
            if (!info.Name.Contains("SDSContent") && info.Extension != "xml")
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
                    case "Video":
                        treeView1.Nodes[0].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Script":
                        treeView1.Nodes[1].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "ItemDesc":
                        treeView1.Nodes[2].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Collisions":
                        treeView1.Nodes[3].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "FxAnimSet":
                        treeView1.Nodes[4].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "FxActor":
                        treeView1.Nodes[5].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Tables":
                        treeView1.Nodes[6].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "XML":
                        treeView1.Nodes[7].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Texture":
                        treeView1.Nodes[8].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Mipmap":
                        treeView1.Nodes[9].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "MemFile":
                        treeView1.Nodes[10].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "FrameResource":
                        treeView1.Nodes[11].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "ExtraData":
                        treeView1.Nodes[12].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "IndexBufferPool":
                        treeView1.Nodes[13].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "VertexBufferPool":
                        treeView1.Nodes[14].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Animated Texture":
                        treeView1.Nodes[15].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "AnimSet":
                        treeView1.Nodes[16].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Effects":
                        treeView1.Nodes[17].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Translokator":
                        treeView1.Nodes[18].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Cutscene":
                        treeView1.Nodes[19].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "SoundTable":
                        treeView1.Nodes[20].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Sound":
                        treeView1.Nodes[21].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "AudioSectors":
                        treeView1.Nodes[22].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Animation2":
                        treeView1.Nodes[23].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "AnimalTrafficPaths":
                        treeView1.Nodes[24].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "FrameNameTable":
                        treeView1.Nodes[25].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "EntityDataStorage":
                        treeView1.Nodes[26].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Actors":
                        treeView1.Nodes[27].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "Speech":
                        treeView1.Nodes[28].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "PREFAB":
                        treeView1.Nodes[29].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "NAV_AIWORLD_DATA":
                        treeView1.Nodes[30].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "NAV_OBJ_DATA":
                        treeView1.Nodes[31].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    case "NAV_HPD_DATA":
                        treeView1.Nodes[32].Nodes.Add(BuildTreeNode(file, info.Directory));
                        break;
                    default:
                        Console.WriteLine("Failed to add " + file);
                        break;
                }
            }
        }

        private void autoAddFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
