using ResourceTypes.FrameResource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Linq;

namespace Mafia2Tool
{
    public partial class SDSContentEditor : Form
    {
        DirectoryInfo parent;
        Dictionary<string, List<TreeNode>> resources;
        Dictionary<string, BaseResource> types;
        readonly List<string> SortList;
        public SDSContentEditor()
        {
            InitializeComponent();
            Localise();
        }

        public SDSContentEditor(FileInfo info)
        {
            InitializeComponent();
            resources = new Dictionary<string, List<TreeNode>>();
            parent = info.Directory;
            SortList = new List<string>();
            SortList.AddRange(new string[] {"IndexBufferPool", "VertexBufferPool", "Texture", "FrameResource", "Effects", "FrameNameTable",
               "Actors", "EntityDataStorage",  "PREFAB", "Animation2",  "AnimalTrafficPaths", "Table", "NAV_OBJ_DATA", "NAV_AIWORLD_DATA", "NAV_HPD_DATA",
                "AudioSectors", "MemFile", "Collisions", "ItemDesc", "FxActor", "Script", "Sound", "Speech", "Cutscene", "SoundTable", "XML", "Translokator", "Mipmap" });

            InitTypes();
            Localise();
            LoadSDSContent(info);
            ShowDialog();
        }

        private void InitTypes()
        {
            types = new Dictionary<string, BaseResource>();
            types.Add("IndexBufferPool", new BaseResource(2, "IndexBufferPool"));
            types.Add("VertexBufferPool", new BaseResource(2, "VertexBufferPool"));
            types.Add("Texture", new TextureResource(2, "Texture"));
            types.Add("FrameResource", new BaseResource(28, "FrameResource"));
            types.Add("Effects", new BaseResource(2, "Effects"));
            types.Add("FrameNameTable", new BaseResource(3, "FrameNameTable"));
            types.Add("Actors", new BaseResource(4, "Actors"));
            types.Add("EntityDataStorage", new BaseResource(2, "EntityDataStorage"));
            types.Add("PREFAB", new BaseResource(0, "PREFAB"));
            types.Add("Animation2", new BaseResource(1, "Animation2"));
            types.Add("Tables", new TableResource(1, "Tables"));
            types.Add("NAV_OBJ_DATA", new BaseResource(0, "NAV_OBJ_DATA"));
            types.Add("NAV_AIWORLD_DATA", new BaseResource(1, "NAV_AIWORLD_DATA"));
            types.Add("NAV_HPD_DATA", new BaseResource(1, "NAV_HPD_DATA"));
            types.Add("AnimalTrafficPaths", new BaseResource(0, "AnimalTrafficPaths"));
            types.Add("AudioSectors", new BaseResource(6, "AudioSectors"));
            types.Add("MemFile", new BaseResource(2, "MemFile"));
            types.Add("Collisions", new BaseResource(2, "Collisions"));
            types.Add("Sound", new BaseResource(5, "Sound"));
            types.Add("Cutscene", new BaseResource(3, "Cutscene"));
            types.Add("ItemDesc", new BaseResource(3, "ItemDesc"));
            types.Add("Script", new ScriptResource(2, "Script"));
            types.Add("SoundTable", new BaseResource(2, "SoundTable"));
            types.Add("XML", new XMLResource(3, "XML"));
            types.Add("Translokator", new BaseResource(1, "Translokator"));
            types.Add("Mipmap", new BaseResource(2, "Mipmap"));
        }

        private void Localise()
        {


        }

        private TreeNode BuildTreeNode(string name, object data = null)
        {
            TreeNode node = new TreeNode();
            node.Name = name + "Node";
            node.Text = name;
            node.Tag = data;
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
                BaseResource resource = null;

                if(!resources.ContainsKey(resourceType))
                {
                    resources.Add(resourceType, new List<TreeNode>());
                }

                switch (resourceType)
                {
                    case "FrameResource":
                    case "Effects":
                    case "PREFAB":
                    case "ItemDesc":
                    case "FrameNameTable":
                    case "Actors":
                    case "NAV_AIWORLD_DATA":
                    case "NAV_OBJ_DATA":
                    case "NAV_HPD_DATA":
                    case "Cutscene":
                    case "FxActor":
                    case "FxAnimSet":
                    case "Translokator":
                    case "Speech":
                    case "SoundTable":
                    case "AnimalTrafficPaths":
                    case "AudioSectors":
                    case "Animated Texture":
                    case "Collisions":
                    case "IndexBufferPool":
                    case "VertexBufferPool":
                    case "EntityDataStorage":
                    case "Animation2":
                    case "Mipmap":
                    case "Sound":
                    case "MemFile":
                        resource = new BaseResource();
                        resource.ReadResourceEntry(nodes);
                        break;
                    case "Texture":
                        resource = new TextureResource();
                        resource.ReadResourceEntry(nodes);
                        break;
                    case "XML":
                        resource = new XMLResource();
                        resource.ReadResourceEntry(nodes);
                        break;
                    case "Script":
                        resource = new ScriptResource();
                        resource.ReadResourceEntry(nodes);
                        break;
                    case "Table":
                        resource = new TableResource();
                        resource.ReadResourceEntry(nodes);
                        break;
                    default:
                        MessageBox.Show("Did not pack type: " + resourceType, "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }

                TreeNode node = BuildTreeNode(resource.GetFileName(), resource);
                resources[resourceType].Add(node);
            }
        }

        private void WriteNewSDSContent()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.OmitXmlDeclaration = true;

            XmlWriter writer = XmlWriter.Create(parent.FullName + "/SDSContent_Copy.xml", settings);
            writer.WriteStartElement("SDSResource");

            foreach(var pair in resources)
            {
                var list = pair.Value;

                foreach(var entry in list)
                {
                    writer.WriteStartElement("ResourceEntry");
                    writer.WriteElementString("Type", pair.Key);
                    (entry.Tag as BaseResource).WriteResourceEntry(writer);
                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement();
            writer.Close();
            writer.Dispose();
        }

        private void AutoAddFilesButton_Click(object sender, EventArgs e)
        {
            List<string> newBuffers = new List<string>();
            foreach(FileInfo info in parent.GetFiles())
            {
                if (info.Extension.Contains("vbp"))
                    newBuffers.Add(info.Name);

                if (info.Extension.Contains("ibp"))
                    newBuffers.Add(info.Name);

                if (info.Extension.Contains("dds"))
                    newBuffers.Add(info.Name);
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.OmitXmlDeclaration = true;

            XmlWriter resourceXML = XmlWriter.Create(parent.FullName + "/SDSContent_BUFFERS.xml", settings);
            resourceXML.WriteStartElement("SDSResource");
            for (int i = 0; i != newBuffers.Count; i++)
            {
                resourceXML.WriteStartElement("ResourceEntry");
                

                if (newBuffers[i].Contains(".ibp"))
                {
                    resourceXML.WriteElementString("Type", "IndexBufferPool");
                    resourceXML.WriteElementString("File", newBuffers[i]);
                    resourceXML.WriteElementString ("Version", "2");
                }
                else if(newBuffers[i].Contains(".vbp"))
                {
                    resourceXML.WriteElementString("Type", "VertexBufferPool");
                    resourceXML.WriteElementString("File", newBuffers[i]);
                    resourceXML.WriteElementString("Version", "2");
                }
                else if(newBuffers[i].Contains(".dds"))
                {
                    resourceXML.WriteElementString("Type", "Texture");
                    resourceXML.WriteElementString("File", newBuffers[i]);
                    resourceXML.WriteElementString("HasMIP", "0");
                    resourceXML.WriteElementString("Version", "2");
                }
                resourceXML.WriteEndElement();
            }
            resourceXML.WriteEndElement();
            resourceXML.Flush();
            resourceXML.Dispose();
        }

        private void SaveButtonOnClick(object sender, EventArgs e)
        {
            resources.OrderBy(d => SortList.IndexOf(d.Key)).ToList();
            WriteNewSDSContent();
        }
    }
}