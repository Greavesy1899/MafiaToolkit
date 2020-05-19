using ResourceTypes.FrameResource;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Linq;
using System;

namespace Utils.Types
{
    public class SDSContentFile
    {
        DirectoryInfo parent;
        Dictionary<string, List<TreeNode>> resources;
        public Dictionary<string, BaseResource> typeList = new Dictionary<string, BaseResource>();
        static Dictionary<string, string> typeExtension = new Dictionary<string, string>();
        static readonly List<string> sortList = new List<string>() {"IndexBufferPool", "VertexBufferPool", "Texture", "FrameResource", "Effects", "FrameNameTable",
               "Actors", "EntityDataStorage",  "PREFAB", "Animation2",  "AnimalTrafficPaths", "Table", "NAV_OBJ_DATA", "NAV_AIWORLD_DATA", "NAV_HPD_DATA",
                "AudioSectors", "MemFile", "Collisions", "ItemDesc", "FxActor", "FxAnimSet", "Script", "Sound", "Speech", "Cutscene", "SoundTable", "XML", "Translokator", "Mipmap" };
        
        public Dictionary<string, List<TreeNode>> Resources {
            get { return resources; }
        }

        public SDSContentFile()
        {
            PopulateTypeList();
            resources = new Dictionary<string, List<TreeNode>>();
        }

        private void PopulateTypeList()
        {
            typeList = new Dictionary<string, BaseResource>();
            typeList.Add("IndexBufferPool", new BaseResource(2, "IndexBufferPool"));
            typeList.Add("VertexBufferPool", new BaseResource(2, "VertexBufferPool"));
            typeList.Add("Texture", new TextureResource(2, "Texture"));
            typeList.Add("FrameResource", new BaseResource(28, "FrameResource"));
            typeList.Add("Effects", new BaseResource(2, "Effects"));
            typeList.Add("FrameNameTable", new BaseResource(3, "FrameNameTable"));
            typeList.Add("Actors", new BaseResource(4, "Actors"));
            typeList.Add("EntityDataStorage", new BaseResource(2, "EntityDataStorage"));
            typeList.Add("PREFAB", new BaseResource(0, "PREFAB"));
            typeList.Add("Animation2", new BaseResource(1, "Animation2"));
            typeList.Add("Tables", new TableResource(1, "Tables"));
            typeList.Add("NAV_OBJ_DATA", new BaseResource(0, "NAV_OBJ_DATA"));
            typeList.Add("NAV_AIWORLD_DATA", new BaseResource(1, "NAV_AIWORLD_DATA"));
            typeList.Add("NAV_HPD_DATA", new BaseResource(1, "NAV_HPD_DATA"));
            typeList.Add("AnimalTrafficPaths", new BaseResource(0, "AnimalTrafficPaths"));
            typeList.Add("AudioSectors", new BaseResource(6, "AudioSectors"));
            typeList.Add("MemFile", new BaseResource(2, "MemFile"));
            typeList.Add("Collisions", new BaseResource(2, "Collisions"));
            typeList.Add("Sound", new BaseResource(5, "Sound"));
            typeList.Add("Cutscene", new BaseResource(3, "Cutscene"));
            typeList.Add("ItemDesc", new BaseResource(3, "ItemDesc"));
            typeList.Add("Script", new ScriptResource(2, "Script"));
            typeList.Add("SoundTable", new BaseResource(2, "SoundTable"));
            typeList.Add("XML", new XMLResource(3, "XML"));
            typeList.Add("Translokator", new BaseResource(1, "Translokator"));
            typeList.Add("Mipmap", new BaseResource(2, "Mipmap"));

            typeExtension = new Dictionary<string, string>();
            typeExtension.Add("ibp", "IndexBufferPool");
            typeExtension.Add("vbp", "VertexBufferPool");
            typeExtension.Add("fr", "FrameResource");
            typeExtension.Add("eff", "Effects");
            typeExtension.Add("fnt", "FrameNameTable");
            typeExtension.Add("act", "Actors");
            typeExtension.Add("prf", "PREFAB");
            typeExtension.Add("an2", "Animation2");
            typeExtension.Add("nov", "NAV_OBJ_DATA");
            typeExtension.Add("nav", "NAV_AIWORLD_DATA");
            typeExtension.Add("nhv", "NAV_HPD_DATA");
            typeExtension.Add("atp", "AnimalTrafficPaths");
            typeExtension.Add("fsb", "Sound");
            typeExtension.Add("cut", "Cutscene");
            typeExtension.Add("col", "Collisions");
            typeExtension.Add("ids", "ItemDesc");
            typeExtension.Add("stbl", "SoundTable");
            typeExtension.Add("tra", "Translokator");
        }

        public static TreeNode BuildResourceTreeNode(string name, object data = null)
        {
            TreeNode node = new TreeNode();
            node.Name = name + "Node";
            node.Text = name;
            node.Tag = data;
            return node;
        }

        public bool HasResource(string typeName)
        {
            return resources.ContainsKey(typeName);
        }

        public string[] GetResourceFiles(string typeName, bool addParentDirectory)
        {
            if(HasResource(typeName))
            {
                var list = resources[typeName];
                string[] paths = new string[list.Count];
                for(int i = 0; i < list.Count; i++)
                {
                    var tag = (list[i].Tag as BaseResource);
                    paths[i] = (addParentDirectory ? parent.FullName + "/" + tag.GetFileName() : tag.GetFileName());
                }
                return paths;
            }
            return new string[0];
        }

        public void ReadFromFile(FileInfo info)
        {
            if (!info.Name.Contains("SDSContent") && info.Extension != "xml")
            {
                return;
            }

            parent = info.Directory;
            XmlDocument document = new XmlDocument();
            document.Load(info.FullName);

            XPathNavigator nav = document.CreateNavigator();
            var nodes = nav.Select("/SDSResource/ResourceEntry");
            while (nodes.MoveNext() == true)
            {
                nodes.Current.MoveToFirstChild();
                string resourceType = nodes.Current.Value;
                BaseResource resource = null;

                if (!resources.ContainsKey(resourceType))
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
                        MessageBox.Show("Did not load type: " + resourceType, "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }

                TreeNode node = BuildResourceTreeNode(resource.GetFileName(), resource);
                resources[resourceType].Add(node);
            }
        }

        public void WriteToFile()
        {
            Sort();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.OmitXmlDeclaration = true;

            File.Copy(Path.Combine(parent.FullName, "SDSContent.xml"), Path.Combine(parent.FullName, "SDSContent_old.xml"), true);
            XmlWriter writer = XmlWriter.Create(Path.Combine(parent.FullName, "SDSContent.xml"), settings);
            writer.WriteStartElement("SDSResource");

            foreach (var pair in resources)
            {
                var list = pair.Value;

                foreach (var entry in list)
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

        private void Sort()
        {
            resources = resources.OrderBy(d => sortList.IndexOf(d.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        private void AddResource(string typeName, TreeNode node)
        {
            if(resources.ContainsKey(typeName))
            {
                resources[typeName].Add(node);
            }
            else
            {
                resources.Add(typeName, new List<TreeNode>());
                resources[typeName].Add(node);
            }
        }

        private void CreateBaseResource(string typeName, FileInfo info)
        {
            string fromRoot = info.FullName.Remove(0, parent.FullName.Length+1);
            var typeResource = typeList[typeName];
            var version = typeResource.GetSerializationVersion();
            BaseResource resource = new BaseResource();
            resource.SetFileName(fromRoot);
            resource.SetEntryVersion(version);
            AddResource(typeName, BuildResourceTreeNode(fromRoot, resource));
        }

        private void CreateTableResource(List<string> tables)
        {
            var typeResource = typeList["Tables"];
            TableResource resource = new TableResource();
            resource.Tables = tables.ToArray();
            resource.SetEntryVersion(typeResource.GetSerializationVersion());
            AddResource("Tables", BuildResourceTreeNode("", resource));
        }

        private void CreateTextureResource(string name, bool mipped)
        {
            var typeResource = typeList["Texture"];
            TextureResource resource = new TextureResource();
            resource.HasMIP = Convert.ToInt32(mipped);
            resource.SetEntryVersion(typeResource.GetSerializationVersion());
            resource.SetFileName(name);
            AddResource("Texture", BuildResourceTreeNode(name, resource));
        }

        private void ScanFolder(DirectoryInfo directory, ref List<string> tables, ref List<string> textures, ref List<string> mips)
        {
            foreach(var info in directory.GetFiles())
            {
                var extension = info.Extension.Replace(".", string.Empty);
                if(typeExtension.ContainsKey(extension))
                {
                    CreateBaseResource(typeExtension[extension], info);
                }
                else if(info.Extension.Contains("tbl"))
                {
                    tables.Add(info.Name);
                }
                else if(info.Extension.Contains("dds") && !info.Name.Contains("MIP_"))
                {
                    textures.Add(info.Name);
                }
                else if (info.Extension.Contains("dds") && info.Name.Contains("MIP_"))
                {
                    mips.Add(info.Name);
                }
            }

            foreach (var folder in directory.GetDirectories())
            {
                ScanFolder(folder, ref tables, ref textures, ref mips);
            }

            
        }

        private List<TreeNode> ProtectResourceType(string typeName)
        {
            if(resources.ContainsKey(typeName))
            {
                return resources[typeName];
            }
            return null;
        }

        private void ReapplyResourceType(string typeName, List<TreeNode> nodes)
        {
            if(nodes != null)
            {
                if (resources.ContainsKey(typeName))
                {
                    resources[typeName].AddRange(nodes);
                }
                else
                {
                    resources.Add(typeName, nodes);
                }
            }
        }

        private Dictionary<string, bool> BuildHasMipsDict(List<string> textures, List<string> mips)
        {
            var dict = new Dictionary<string, bool>();
            foreach(var tex in textures)
            {
                dict.Add(tex, false);
            }

            foreach(var mip in mips)
            {
                string name = mip.Substring(4, mip.Length - 4);
                if(dict.ContainsKey(name))
                {
                    dict[name] = true;
                }
            }
            return dict;
        }

        public void CreateFileFromFolder()
        {
            //we have to do these after initial resource population
            List<string> tables = new List<string>();
            List<string> textures = new List<string>();
            List<string> mips = new List<string>();

            //we keep resources which require human knowledge
            var scripts = ProtectResourceType("Script");
            var xmls = ProtectResourceType("XML");
            var memfile = ProtectResourceType("MemFile");
            var audioSectors = ProtectResourceType("AudioSectors");
            var entityDataStorage = ProtectResourceType("EntityDataStorage");

            //clear and scan
            resources.Clear();
            ScanFolder(parent, ref tables, ref textures, ref mips);

            //get new dict of HasMips
            if (textures.Count > 0)
            {
                var dict = BuildHasMipsDict(textures, mips);
                foreach (var item in dict)
                {
                    CreateTextureResource(item.Key, item.Value);

                    if (item.Value)
                    {
                        string name = "MIP_" + item.Key;
                        var info = new FileInfo(Path.Combine(parent.FullName, name));
                        CreateBaseResource("Mipmap", info);
                    }
                }
            }
            if(tables.Count > 0)
            {
                CreateTableResource(tables);
            }
            
            ReapplyResourceType("Script", scripts);
            ReapplyResourceType("XML", xmls);
            ReapplyResourceType("MemFile", memfile);
            ReapplyResourceType("AudioSectors", audioSectors);
            ReapplyResourceType("EntityDataStorage", entityDataStorage);
            Sort();
        }
    }
}
