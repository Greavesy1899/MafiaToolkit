using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin
{
    public class GenericSpeechSituationsItem
    {
        public ulong SituationID { get; set; }
        public uint Cooldown { get; set; }
        public uint PerActorCooldownMin { get; set; }
        public uint PerActorCooldownMax { get; set; }
        public uint PerTeamCooldownMin { get; set; }
        public uint PerTeamCooldownMax { get; set; }
        public uint IsForeground { get; set; } //bool
        [ReadOnly(true)]
        public ulong FNV64InitHash { get; set; }

        public override string ToString()
        {
            return string.Format("Situation = {0}", SituationID);
        }
    }
	
    public class GenericSpeechSituationsTable : BaseTable
    {
        private uint unk0;
        private GenericSpeechSituationsItem[] speechsituations;
        private uint unk1;

        public GenericSpeechSituationsItem[] SpeechSituations {
            get { return speechsituations; }
            set { speechsituations = value; }
        }

        public GenericSpeechSituationsTable()
        {
            speechsituations = new GenericSpeechSituationsItem[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            speechsituations = new GenericSpeechSituationsItem[count0];

            for (int i = 0; i < count1; i++)
            {
                GenericSpeechSituationsItem item = new GenericSpeechSituationsItem();
                item.SituationID = reader.ReadUInt64();
                item.Cooldown = reader.ReadUInt32();
                item.PerActorCooldownMin = reader.ReadUInt32();
                item.PerActorCooldownMax = reader.ReadUInt32();
                item.PerTeamCooldownMin = reader.ReadUInt32();
                item.PerTeamCooldownMax = reader.ReadUInt32();
                item.IsForeground = reader.ReadUInt32();
                item.FNV64InitHash = reader.ReadUInt64();

                speechsituations[i] = item;
            }

            unk1 = reader.ReadUInt32();
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(speechsituations.Length);
            writer.Write(speechsituations.Length);

            foreach (var speechsituation in speechsituations)
            {
                writer.Write(speechsituation.SituationID);
                writer.Write(speechsituation.Cooldown);
                writer.Write(speechsituation.PerActorCooldownMin);
                writer.Write(speechsituation.PerActorCooldownMax);
                writer.Write(speechsituation.PerTeamCooldownMin);
                writer.Write(speechsituation.PerTeamCooldownMax);
                writer.Write(speechsituation.IsForeground);
                writer.Write(speechsituation.FNV64InitHash);
            }

            writer.Write(unk1);
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GenericSpeechSituationsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<GenericSpeechSituationsTable>(Root);
            this.speechsituations = TableInformation.speechsituations;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "GenericSpeechSituationsTable";

            foreach(var Item in SpeechSituations)
            {
                TreeNode ChildNode = new TreeNode();
                ChildNode.Tag = Item;
                ChildNode.Text = Item.ToString();
                Root.Nodes.Add(ChildNode);
            }

            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            SpeechSituations = new GenericSpeechSituationsItem[Root.Nodes.Count];

            for (int i = 0; i < SpeechSituations.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                GenericSpeechSituationsItem Entry = (GenericSpeechSituationsItem)ChildNode.Tag;
                SpeechSituations[i] = Entry;
            }
        }
    }
}
