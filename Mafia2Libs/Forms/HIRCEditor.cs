using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.Wwise;
using Utils.Language;
using Utils.Settings;
using Forms.EditorControls;
using System.Collections.Generic;

namespace Mafia2Tool
{
    public partial class HIRCEditor : Form
    {
        private HIRC Hirc;
        private Wem Wem;

        public HIRCEditor(HIRC Input, Wem InputWem)
        {
            InitializeComponent();
            Localise();
            Hirc = Input;
            Wem = InputWem;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Wwise BNK editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$HIRC_EDITOR_TITLE");
            ContextExport.Text = Language.GetString("$EXPORT_WEM");
        }

        private void BuildData()
        {
            Wem.AssignedHirc.EventAction = new List<int>();
            Wem.AssignedHirc.Event = new List<int>();
            Wem.AssignedHirc.RandomContainer = new List<int>();
            Wem.AssignedHirc.SwitchContainer = new List<int>();
            Wem.AssignedHirc.ActorMixer = new List<int>();
            Wem.AssignedHirc.AudioBus = new List<int>();
            Wem.AssignedHirc.BlendContainer = new List<int>();
            Wem.AssignedHirc.MusicSegment = new List<int>();
            Wem.AssignedHirc.MusicSwitchContainer = new List<int>();
            Wem.AssignedHirc.MusicSequence = new List<int>();
            Wem.AssignedHirc.Attenuation = new List<int>();
            Wem.AssignedHirc.FeedbackNode = new List<int>();
            Wem.AssignedHirc.FxShareSet = new List<int>();
            Wem.AssignedHirc.FxCustom = new List<int>();
            Wem.AssignedHirc.AuxiliaryBus = new List<int>();
            Wem.AssignedHirc.LFO = new List<int>();
            Wem.AssignedHirc.Envelope = new List<int>();

            Hirc.AssignHirc(Wem.AssignedHirc);

            AddNode(Wem.AssignedHirc.ActorMixer, "Actor Mixer");
            AddNode(Wem.AssignedHirc.Attenuation, "Attenuation");
            AddNode(Wem.AssignedHirc.BlendContainer, "Blend Container");
            AddNode(Wem.AssignedHirc.Envelope, "Envelope");
            AddNode(Wem.AssignedHirc.Event, "Event");
            AddNode(Wem.AssignedHirc.EventAction, "Event Action");
            AddNode(Wem.AssignedHirc.FeedbackNode, "Feedback Node");
            AddNode(Wem.AssignedHirc.FxCustom, "FxCustom");
            AddNode(Wem.AssignedHirc.FxShareSet, "FxShareSet");
            AddNode(Wem.AssignedHirc.LFO, "LFO");
            AddNode(Wem.AssignedHirc.MusicSegment, "Music Segment");
            AddNode(Wem.AssignedHirc.MusicSequence, "Music Sequence");
            AddNode(Wem.AssignedHirc.MusicSwitchContainer, "Music Switch Container");
            AddNode(Wem.AssignedHirc.MusicTrack, "Music Track");
            AddNode(Wem.AssignedHirc.RandomContainer, "Random Container");
            AddNode(Wem.AssignedHirc.Settings, "Settings");
            AddNode(Wem.AssignedHirc.SoundSFX, "Sound SFX");
            AddNode(Wem.AssignedHirc.SwitchContainer, "Switch Container");
        }

        private void AddNode(List<int> ids, string name)
        {
            if (ids != null)
            {
                TreeNode node = new TreeNode();
                node.Text = name;

                foreach (int id in ids)
                {
                    TreeNode tempItem = Hirc.CreateNode(name, id);
                    node.Nodes.Add(tempItem);
                }

                if (node.Nodes.Count != 0)
                {
                    TreeView_HIRC.Nodes.Add(node);
                }
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            HircGrid.SelectedObject = e.Node.Tag;
        }

        private void WemGrid_OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name")
            {
                TreeView_HIRC.SelectedNode.Text = e.ChangedItem.Value.ToString();
            }

            HircGrid.Refresh();
        }

        private void HIRCEditor_Load(object sender, System.EventArgs e)
        {

        }
    }
}