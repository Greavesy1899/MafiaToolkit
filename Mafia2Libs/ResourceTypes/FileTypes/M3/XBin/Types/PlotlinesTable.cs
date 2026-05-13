using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin
{
    public class PlotlineActivity
    {
        public string ScriptPath { get; set; }

        public PlotlineActivity()
        {
            ScriptPath = "";
        }

        public override string ToString() => ScriptPath;
    }

    public class PlotlineItem
    {
        public XBinHashName Id { get; set; }
        public string Name { get; set; }
        public PlotlineActivity[] Activities { get; set; }

        public PlotlineItem()
        {
            Id = new XBinHashName();
            Name = "";
            Activities = new PlotlineActivity[0];
        }

        public override string ToString() => string.IsNullOrEmpty(Name) ? Id.ToString() : Name;
    }

    public class PlotlinesTable : BaseTable
    {
        private uint unk0;
        private uint unk1;
        private PlotlineItem[] plotlines;

        public PlotlineItem[] Plotlines
        {
            get { return plotlines; }
            set { plotlines = value; }
        }

        public PlotlinesTable()
        {
            plotlines = new PlotlineItem[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            unk1 = reader.ReadUInt32();

            plotlines = new PlotlineItem[count0];

            for (int i = 0; i < count1; i++)
            {
                PlotlineItem item = new PlotlineItem();
                reader.ReadUInt32();                                // ActivitiesOffset (sub-table)
                reader.ReadUInt32();                                // reserved high-32
                item.Id = XBinHashName.ConstructAndReadFromFile(reader);
                item.Name = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                reader.ReadUInt32();                                // reserved high-32
                plotlines[i] = item;
            }

            for (int i = 0; i < count1; i++)
            {
                uint actItemsOffset = reader.ReadUInt32();
                uint actCount0 = reader.ReadUInt32();
                uint actCount1 = reader.ReadUInt32();
                plotlines[i].Activities = new PlotlineActivity[actCount0];
                for (int j = 0; j < actCount1; j++)
                {
                    PlotlineActivity act = new PlotlineActivity();
                    act.ScriptPath = XBinCoreUtils.ReadStringPtrWithOffset(reader);
                    plotlines[i].Activities[j] = act;
                }
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(plotlines.Length);
            writer.Write(plotlines.Length);
            writer.Write(unk1);

            for (int i = 0; i < plotlines.Length; i++)
            {
                PlotlineItem item = plotlines[i];
                writer.PushObjectPtr("PlotlineActivities_" + i);
                writer.Write(0u);
                item.Id.WriteToFile(writer);
                writer.PushStringPtr(item.Name);
                writer.Write(0u);
            }

            for (int i = 0; i < plotlines.Length; i++)
            {
                PlotlineItem item = plotlines[i];
                writer.FixUpObjectPtr("PlotlineActivities_" + i);
                writer.PushObjectPtr("PlotlineActivityItems_" + i);
                writer.Write(item.Activities.Length);
                writer.Write(item.Activities.Length);
                writer.FixUpObjectPtr("PlotlineActivityItems_" + i);
                foreach (PlotlineActivity act in item.Activities)
                {
                    writer.PushStringPtr(act.ScriptPath);
                }
            }

            writer.FixUpStringPtrs();
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            PlotlinesTable Loaded = ReflectionHelpers.ConvertToPropertyFromXML<PlotlinesTable>(Root);
            plotlines = Loaded.plotlines;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode("Plotlines Table");
            foreach (PlotlineItem item in plotlines)
            {
                TreeNode child = new TreeNode(item.ToString());
                child.Tag = item;
                Root.Nodes.Add(child);
            }
            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            plotlines = new PlotlineItem[Root.Nodes.Count];
            for (int i = 0; i < plotlines.Length; i++)
            {
                plotlines[i] = (PlotlineItem)Root.Nodes[i].Tag;
            }
        }
    }
}
