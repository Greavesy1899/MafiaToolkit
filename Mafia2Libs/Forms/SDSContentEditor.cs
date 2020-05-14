using ResourceTypes.FrameResource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Linq;
using Utils.Types;

namespace Mafia2Tool
{
    public partial class SDSContentEditor : Form
    {
        SDSContentFile file;
        public SDSContentEditor()
        {
            InitializeComponent();
            Localise();
        }

        public SDSContentEditor(FileInfo info)
        {
            InitializeComponent();
            Localise();
            file = new SDSContentFile();
            file.ReadFromFile(info);
            PopulateTree();
            ShowDialog();
        }

        private void Localise()
        {


        }

        private void PopulateTree()
        {
            ResourceTreeView.Nodes.Clear();
            foreach(var resource in file.Resources)
            {
                TreeNode parent = SDSContentFile.BuildResourceTreeNode(resource.Key);
                parent.Nodes.AddRange(resource.Value.ToArray());
                ResourceTreeView.Nodes.Add(parent);
            }
        }

        private void AutoAddFilesButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will only automatically add any buffer pools or textures into the SDSContent.xml. Are you sure you want to do this?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                file.CreateFileFromFolder();
                PopulateTree();
            }
        }

        private void SaveButtonOnClick(object sender, EventArgs e)
        {
            file.WriteToFile();
        }
    }
}