using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin
{
    public class UserServiceMapping
    {
        public uint PlatformId { get; set; }
        public uint InternalId { get; set; }
        public uint Flags { get; set; }

        public override string ToString() => string.Format("[{0}] -> [{1}] (flags=0x{2:X})", PlatformId, InternalId, Flags);
    }

    public class UserServicePlatform
    {
        public XBinHashName Id { get; set; }
        public string NameFormat { get; set; }
        public UserServiceMapping[] Mappings { get; set; }

        public UserServicePlatform()
        {
            Id = new XBinHashName();
            NameFormat = "";
            Mappings = new UserServiceMapping[0];
        }

        public override string ToString() => string.IsNullOrEmpty(NameFormat) ? Id.ToString() : NameFormat;
    }

    public class UserServicesTable : BaseTable
    {
        private uint unk0;
        private uint unk1;
        private UserServicePlatform[] platforms;

        public UserServicePlatform[] Platforms
        {
            get { return platforms; }
            set { platforms = value; }
        }

        public UserServicesTable()
        {
            platforms = new UserServicePlatform[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            unk1 = reader.ReadUInt32();

            platforms = new UserServicePlatform[count0];

            for (int i = 0; i < count1; i++)
            {
                UserServicePlatform p = new UserServicePlatform();
                uint mappingsOffset = reader.ReadUInt32();
                uint mapCount0 = reader.ReadUInt32();
                uint mapCount1 = reader.ReadUInt32();
                p.NameFormat = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                p.Id = XBinHashName.ConstructAndReadFromFile(reader);
                p.Mappings = new UserServiceMapping[mapCount0];
                platforms[i] = p;
            }

            for (int i = 0; i < count1; i++)
            {
                UserServicePlatform p = platforms[i];
                for (int j = 0; j < p.Mappings.Length; j++)
                {
                    UserServiceMapping m = new UserServiceMapping();
                    m.PlatformId = reader.ReadUInt32();
                    m.InternalId = reader.ReadUInt32();
                    m.Flags = reader.ReadUInt32();
                    p.Mappings[j] = m;
                }
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(platforms.Length);
            writer.Write(platforms.Length);
            writer.Write(unk1);

            for (int i = 0; i < platforms.Length; i++)
            {
                UserServicePlatform p = platforms[i];
                writer.PushObjectPtr("UserServiceMappings_" + i);
                writer.Write(p.Mappings.Length);
                writer.Write(p.Mappings.Length);
                writer.PushStringPtr(p.NameFormat);
                p.Id.WriteToFile(writer);
            }

            for (int i = 0; i < platforms.Length; i++)
            {
                UserServicePlatform p = platforms[i];
                writer.FixUpObjectPtr("UserServiceMappings_" + i);
                foreach (UserServiceMapping m in p.Mappings)
                {
                    writer.Write(m.PlatformId);
                    writer.Write(m.InternalId);
                    writer.Write(m.Flags);
                }
            }

            writer.FixUpStringPtrs();
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            UserServicesTable Loaded = ReflectionHelpers.ConvertToPropertyFromXML<UserServicesTable>(Root);
            platforms = Loaded.platforms;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode("User Services Table");
            foreach (UserServicePlatform p in platforms)
            {
                TreeNode platformNode = new TreeNode(p.ToString());
                platformNode.Tag = p;
                foreach (UserServiceMapping m in p.Mappings)
                {
                    TreeNode mapNode = new TreeNode(m.ToString());
                    mapNode.Tag = m;
                    platformNode.Nodes.Add(mapNode);
                }
                Root.Nodes.Add(platformNode);
            }
            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            platforms = new UserServicePlatform[Root.Nodes.Count];
            for (int i = 0; i < platforms.Length; i++)
            {
                TreeNode platformNode = Root.Nodes[i];
                UserServicePlatform p = (UserServicePlatform)platformNode.Tag;
                p.Mappings = new UserServiceMapping[platformNode.Nodes.Count];
                for (int j = 0; j < p.Mappings.Length; j++)
                {
                    p.Mappings[j] = (UserServiceMapping)platformNode.Nodes[j].Tag;
                }
                platforms[i] = p;
            }
        }
    }
}
