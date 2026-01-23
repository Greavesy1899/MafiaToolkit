using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace ResourceTypes.FrameResource
{
    public class BaseResource
    {
        protected readonly int SerialVersion;
        protected readonly string TypeName;
        protected int EntryVersion;
        protected string FileName;

        public BaseResource(int version, string name)
        {
            TypeName = name;
            SerialVersion = version;
        }

        public BaseResource()
        {
        }

        public virtual int GetEntryVersion()
        {
            return EntryVersion;
        }

        public virtual string GetEntryVersionString()
        {
            return EntryVersion.ToString();
        }

        public virtual void SetEntryVersion(int version)
        {
            EntryVersion = version;
        }

        public virtual int GetSerializationVersion()
        {
            return SerialVersion;
        }

        public virtual string GetSerializationVersionString()
        {
            return SerialVersion.ToString();
        }

        public virtual string GetTypeName()
        {
            return TypeName;
        }

        public virtual string GetFileName()
        {
            return FileName;
        }

        public virtual void SetFileName(string file)
        {
            if(file.Contains('\\'))
            {
                file = file.Replace('\\', '/');
            }
            
            FileName = file;
        }

        public virtual void ReadResourceEntry(XPathNodeIterator iterator)
        {
            iterator.Current.MoveToNext();
            FileName = iterator.Current.Value;
            iterator.Current.MoveToNext();

            // Use TryParse to handle potential overflow from hash values in malformed XML
            if (int.TryParse(iterator.Current.Value, out int versionValue))
            {
                EntryVersion = versionValue;
            }
        }

        public virtual void WriteResourceEntry(XmlWriter writer)
        {
            writer.WriteElementString("File", GetFileName());
            writer.WriteElementString("Version", GetEntryVersionString());
        }
    }
    public class TableResource : BaseResource
    {
        private string[] tables;

        public string[] Tables {
            get { return tables; }
            set { tables = value; }
        }
        public TableResource()
        {

        }

        public TableResource(int version, string name) : base(version, name)
        {
        }

        public override void ReadResourceEntry(XPathNodeIterator iterator)
        {
            iterator.Current.MoveToNext();
            int.TryParse(iterator.Current.Value, out int numTables);
            Tables = new string[numTables];
            for (int i = 0; i < numTables; i++)
            {
                iterator.Current.MoveToNext();
                Tables[i] = iterator.Current.Value;
            }
            iterator.Current.MoveToNext();
            int.TryParse(iterator.Current.Value, out int versionValue);
            EntryVersion = versionValue;
        }

        public override void WriteResourceEntry(XmlWriter writer)
        {
            writer.WriteElementString("NumTables", Tables.Length.ToString());
            for(int i = 0; i < Tables.Length; i++)
            {
                writer.WriteElementString("Table", Tables[i]);
            }
            writer.WriteElementString("Version", GetEntryVersionString());
        }
    }
    public class ScriptResource : BaseResource
    {
        private string[] scripts = null;

        public string[] Scripts {
            get { return scripts; }
            set { scripts = value; }
        }

        public ScriptResource()
        {

        }

        public ScriptResource(int version, string name) : base(version, name)
        {

        }

        public override void ReadResourceEntry(XPathNodeIterator iterator)
        {
            iterator.Current.MoveToNext();
            FileName = iterator.Current.Value;
            iterator.Current.MoveToNext();
            int.TryParse(iterator.Current.Value, out int numScripts);
            Scripts = new string[numScripts];
            for(int i = 0; i < numScripts; i++)
            {
                iterator.Current.MoveToNext();
                Scripts[i] = iterator.Current.Value;
            }
            iterator.Current.MoveToNext();
            int.TryParse(iterator.Current.Value, out int versionValue);
            EntryVersion = versionValue;
        }

        public override void WriteResourceEntry(XmlWriter writer)
        {
            writer.WriteElementString("File", GetFileName());
            writer.WriteElementString("ScriptNum", Scripts.Length.ToString());
            for(int i = 0; i < Scripts.Length; i++)
            {
                writer.WriteElementString("Name", Scripts[i]);
            }
            writer.WriteElementString("Version", GetEntryVersionString());
        }
    }
    public class XMLResource : BaseResource
    {
        private string xmlTag;
        private int unk1;
        private int unk3;
        private int failedToDecompile;

        public string XMLTag {
            get { return xmlTag; }
            set { xmlTag = value; }
        }
        public int Unk1 {
            get { return unk1; }
            set { unk1 = value; }
        }
        public int Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }
        public int FailedToDecompile {
            get { return failedToDecompile; }
            set { failedToDecompile = value; }
        }

        public XMLResource()
        {

        }

        public XMLResource(int version, string name) : base(version, name)
        {

        }

        public override void ReadResourceEntry(XPathNodeIterator iterator)
        {
            iterator.Current.MoveToNext();
            FileName = iterator.Current.Value;
            iterator.Current.MoveToNext();
            XMLTag = iterator.Current.Value;
            iterator.Current.MoveToNext();
            int.TryParse(iterator.Current.Value, out int unk1Value);
            Unk1 = unk1Value;
            iterator.Current.MoveToNext();
            int.TryParse(iterator.Current.Value, out int unk3Value);
            Unk3 = unk3Value;
            iterator.Current.MoveToNext();
            int.TryParse(iterator.Current.Value, out int failedValue);
            FailedToDecompile = failedValue;
            iterator.Current.MoveToNext();
            int.TryParse(iterator.Current.Value, out int versionValue);
            EntryVersion = versionValue;
        }

        public override void WriteResourceEntry(XmlWriter writer)
        {
            writer.WriteElementString("File", GetFileName());
            writer.WriteElementString("XMLTag", XMLTag);
            writer.WriteElementString("Unk1", Unk1.ToString());
            writer.WriteElementString("Unk3", Unk3.ToString());
            writer.WriteElementString("FailedToDecompile", FailedToDecompile.ToString());
            writer.WriteElementString("Version", GetEntryVersionString());
        }
    }
    public class MemFileResource : BaseResource
    {
        public MemFileResource()
        {
        }

        public MemFileResource(int version, string name) : base(version, name)
        {
        }

        public int Unk2_V4 { get; set; }

        public override void ReadResourceEntry(XPathNodeIterator iterator)
        {
            iterator.Current.MoveToNext();
            FileName = iterator.Current.Value;

            // Sanity check for older SDSContent.XMLs which may not have this.
            iterator.Current.MoveToNext();
            if (iterator.Current.Name.Equals("Unk2_V4"))
            {
                int.TryParse(iterator.Current.Value, out int unk2Value);
                Unk2_V4 = unk2Value;
                iterator.Current.MoveToNext();
            }

            // Whichever outcomes happens, we will be at the version value by now.
            int.TryParse(iterator.Current.Value, out int versionValue);
            EntryVersion = versionValue;
        }

        public override void WriteResourceEntry(XmlWriter writer)
        {
            writer.WriteElementString("File", GetFileName());
            writer.WriteElementString("Unk2_V4", Unk2_V4.ToString());
            writer.WriteElementString("Version", GetEntryVersionString());
        }
    }

    public class TextureResource : BaseResource
    {
        private int hasMIP;

        public int HasMIP {
            get { return hasMIP; }
            set { hasMIP = value; }
        }

        public TextureResource()
        {

        }

        public TextureResource(int version, string name) : base(version, name)
        {

        }

        public override void ReadResourceEntry(XPathNodeIterator iterator)
        {
            iterator.Current.MoveToNext();
            FileName = iterator.Current.Value;
            iterator.Current.MoveToNext();

            // Handle both old format (File, HasMIP, Version) and potentially
            // new format where HasMIP might be missing or have a different name
            if (iterator.Current.Name.Equals("HasMIP"))
            {
                if (int.TryParse(iterator.Current.Value, out int mipValue))
                {
                    HasMIP = mipValue;
                }
                iterator.Current.MoveToNext();
            }

            // Read version - handle potential overflow from hash values
            if (int.TryParse(iterator.Current.Value, out int versionValue))
            {
                EntryVersion = versionValue;
            }
        }

        public override void WriteResourceEntry(XmlWriter writer)
        {
            writer.WriteElementString("File", GetFileName());
            writer.WriteElementString("HasMIP", HasMIP.ToString());
            writer.WriteElementString("Version", GetEntryVersionString());
        }
    }
}
