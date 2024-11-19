using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.Wwise.Objects;
using MessageBox = System.Windows.MessageBox;

namespace ResourceTypes.Wwise
{
    public class hircError
    {
        public static bool errorOccured = false;
    }
    public class HIRCObject
    {
        public BNK Bnk;
        public Settings SettingsObject { get; set; }
        public SoundSFX SoundSFXObject { get; set; }
        public EventAction EventAction { get; set; }
        public Event Event { get; set; }
        public RandomContainer RandomContainer { get; set; }
        public SwitchContainer SwitchContainer { get; set; }
        public ActorMixer ActorMixer { get; set; }
        public AudioBus AudioBus { get; set; }
        public BlendContainer BlendContainer { get; set; }
        public MusicSegment MusicSegment { get; set; }
        public MusicTrack MusicTrack { get; set; }
        public MusicSwitchContainer MusicSwitchContainer { get; set; }
        public MusicSequence MusicSequence { get; set; }
        public Attenuation Attenuation { get; set; }
        public FeedbackNode FeedbackNode { get; set; }
        public FxShareSet FxShareSet { get; set; }
        public FxCustom FxCustom { get; set; }
        public AuxiliaryBus AuxiliaryBus { get; set; }
        public LFO LFO { get; set; }
        public Envelope Envelope { get; set; }
        public FeedbackBus FeedbackBus { get; set; }
        public byte[] Data { get; set; }
        public long LastPos { get; set; }
        public HIRCObject(BNK mainBnk, BinaryReader br)
        {
            Bnk = mainBnk;
            if (!hircError.errorOccured)
            {
                int type = br.ReadByte();
                LastPos = br.BaseStream.Position;
                switch (type)
                {
                    case 1:
                        SettingsObject = new Settings(br, type);
                        break;
                    case 2:
                        SoundSFXObject = new SoundSFX(this, br, type);
                        break;
                    case 3:
                        EventAction = new EventAction(this, br, type);
                        break;
                    case 4:
                        Event = new Event(this, br, type);
                        break;
                    case 5:
                        RandomContainer = new RandomContainer(this, br, type);
                        break;
                    case 6:
                        SwitchContainer = new SwitchContainer(this, br, type);
                        break;
                    case 7:
                        ActorMixer = new ActorMixer(this, br, type);
                        break;
                    case 8:
                        AudioBus = new AudioBus(this, br, type);
                        break;
                    case 9:
                        BlendContainer = new BlendContainer(this, br, type);
                        break;
                    case 10:
                        MusicSegment = new MusicSegment(this, br, type);
                        break;
                    case 11:
                        MusicTrack = new MusicTrack(this, br, type);
                        break;
                    case 12:
                        MusicSwitchContainer = new MusicSwitchContainer(this, br, type);
                        break;
                    case 13:
                        MusicSequence = new MusicSequence(this, br, type);
                        break;
                    case 14:
                        Attenuation = new Attenuation(br, type);
                        break;
                    case 16:
                        FeedbackBus = new FeedbackBus(this, br, type);
                        break;
                    case 17:
                        FeedbackNode = new FeedbackNode(this, br, type);
                        break;
                    case 18:
                        FxShareSet = new FxShareSet(this, br, type);
                        break;
                    case 19:
                        FxCustom = new FxCustom(this, br, type);
                        break;
                    case 20:
                        AuxiliaryBus = new AuxiliaryBus(this, br, type);
                        break;
                    case 21:
                        LFO = new LFO(br, type);
                        break;
                    case 22:
                        Envelope = new Envelope(br, type);
                        break;
                    default:
                        int length = br.ReadInt32();
                        br.BaseStream.Position -= 5;
                        Data = br.ReadBytes(length + 5);
                        MessageBox.Show("Detected unkown HIRC Object type at: " + (LastPos - 1).ToString("X"), "Toolkit");
                        break;
                }
            }
        }

        public HIRCObject(BNK bnkObject)
        {
            Bnk = bnkObject;
        }

        public int GetLength()
        {
            int length = 5;
            if (SettingsObject != null)
            {
                length += SettingsObject.GetLength();
            }
            else if (SoundSFXObject != null)
            {
                length += SoundSFXObject.GetLength();
            }
            else if (EventAction != null)
            {
                length += EventAction.GetLength();
            }
            else if (Event != null)
            {
                length += Event.GetLength();
            }
            else if (RandomContainer != null)
            {
                length += RandomContainer.GetLength();
            }
            else if (SwitchContainer != null)
            {
                length += SwitchContainer.GetLength();
            }
            else if (ActorMixer != null)
            {
                length += ActorMixer.GetLength();
            }
            else if (AudioBus != null)
            {
                length += AudioBus.GetLength();
            }
            else if (BlendContainer != null)
            {
                length += BlendContainer.GetLength();
            }
            else if (MusicSegment != null)
            {
                length += MusicSegment.GetLength();
            }
            else if (MusicTrack != null)
            {
                length += MusicTrack.GetLength();
            }
            else if (MusicSwitchContainer != null)
            {
                length += MusicSwitchContainer.GetLength();
            }
            else if (MusicSequence != null)
            {
                length += MusicSequence.GetLength();
            }
            else if (Attenuation != null)
            {
                length += Attenuation.GetLength();
            }
            else if (FeedbackNode != null)
            {
                length += FeedbackNode.GetLength();
            }
            else if (FxShareSet != null)
            {
                length += FxShareSet.GetLength();
            }
            else if (FxCustom != null)
            {
                length += FxCustom.GetLength();
            }
            else if (AuxiliaryBus != null)
            {
                length += AuxiliaryBus.GetLength();
            }
            else if (LFO != null)
            {
                length += LFO.GetLength();
            }
            else if (Envelope != null)
            {
                length += Envelope.GetLength();
            }
            else if (FeedbackBus != null)
            {
                length += FeedbackBus.GetLength();
            }
            else
            {
                if (Data != null)
                {
                    length += Data.Length;
                }
                else
                {
                    length = -1;
                }
            }

            return length;
        }
    }

    public class HIRC
    {
        public Dictionary<uint, List<int>> AudioDevice = new Dictionary<uint, List<int>>();
        public Dictionary<int, List<int>> ActorMixer = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> Attenuation = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> AudioBus = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> AuxiliaryBus = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> BlendContainer = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> DialogueEvent = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> Envelope = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> Event = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> EventAction = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> FeedbackNode = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> FxCustom = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> FxShareSet = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> LFO = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> MotionBus = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> MusicSegment = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> MusicSequence = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> MusicSwitchContainer = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> MusicTrack = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> RandomContainer = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> Settings = new Dictionary<int, List<int>>();
        public Dictionary<uint, List<int>> SoundSFX = new Dictionary<uint, List<int>>();
        public Dictionary<uint, List<int>> State = new Dictionary<uint, List<int>>();
        public Dictionary<int, List<int>> SwitchContainer = new Dictionary<int, List<int>>();
        public int Length { get; set; }
        public List<HIRCObject> Data { get; set; }
        public byte[] bytes { get; set; }
        public HIRC(int iLength)
        {
            Length = iLength;
            Data = new List<HIRCObject>();
        }

        public HIRC()
        {
            Length = 0;
            Data = new List<HIRCObject>();
        }

        public int GetLength()
        {
            int hircLength = 4;

            foreach (HIRCObject obj in Data)
            {
                int objLength = obj.GetLength();
                if (objLength != -1)
                {
                    hircLength += objLength;
                }
            }

            return hircLength;
        }

        public void AssignHirc(WemHirc AssignedHirc)
        {
            foreach (int ID in AssignedHirc.MusicTrack)
            {
                int index = ID;

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                if (MusicSegment.ContainsKey((int)Data[index].MusicTrack.ID))
                {
                    foreach (int Type10 in MusicSegment[(int)Data[index].MusicTrack.ID])
                    {
                        AssignedHirc.MusicSegment.Add((int)Type10);
                    }
                }
            }

            List<int> CheckedIDs = new List<int>();
            foreach (int ID in AssignedHirc.MusicSegment)
            {
                int index = ID;

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].MusicSegment.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                directParent = (int)Data[index].MusicSegment.NodeBase.AttenuationID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                if (EventAction.ContainsKey((int)Data[index].MusicSegment.ID))
                {
                    AssignedHirc.EventAction.AddRange(EventAction[(int)Data[index].MusicSegment.ID]);
                }
            }

            CheckedIDs = new List<int>();
            foreach (int ID in AssignedHirc.MusicSequence)
            {
                int index = ID;

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].MusicSequence.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                directParent = (int)Data[index].MusicSequence.NodeBase.AttenuationID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                if (EventAction.ContainsKey((int)Data[index].MusicSequence.ID))
                {
                    AssignedHirc.EventAction.AddRange(EventAction[(int)Data[index].MusicSequence.ID]);
                }
            }

            CheckedIDs = new List<int>();
            for (int i = 0; i < AssignedHirc.MusicSwitchContainer.Count; i++)
            {
                int index = AssignedHirc.MusicSwitchContainer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].MusicSwitchContainer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }
            }

            for (int i = 0; i < AssignedHirc.MusicSwitchContainer.Count; i++)
            {
                int index = AssignedHirc.MusicSwitchContainer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].MusicSwitchContainer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                directParent = (int)Data[index].MusicSwitchContainer.NodeBase.AttenuationID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                if (EventAction.ContainsKey((int)Data[index].MusicSwitchContainer.ID))
                {
                    AssignedHirc.EventAction.AddRange(EventAction[(int)Data[index].MusicSwitchContainer.ID]);
                }
            }

            CheckedIDs = new List<int>();
            foreach (int ID in AssignedHirc.SoundSFX)
            {
                int index = ID;

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].SoundSFXObject.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                directParent = (int)Data[index].SoundSFXObject.NodeBase.AttenuationID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                if (EventAction.ContainsKey((int)Data[index].SoundSFXObject.ID))
                {
                    AssignedHirc.EventAction.AddRange(EventAction[(int)Data[index].SoundSFXObject.ID]);
                }
            }

            CheckedIDs = new List<int>();
            for (int i = 0; i < AssignedHirc.RandomContainer.Count; i++)
            {
                int index = AssignedHirc.RandomContainer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].RandomContainer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }
            }

            for (int i = 0; i < AssignedHirc.RandomContainer.Count; i++)
            {
                int index = AssignedHirc.RandomContainer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].RandomContainer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                directParent = (int)Data[index].RandomContainer.NodeBase.AttenuationID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                if (EventAction.ContainsKey((int)Data[index].RandomContainer.ID))
                {
                    AssignedHirc.EventAction.AddRange(EventAction[(int)Data[index].RandomContainer.ID]);
                }
            }

            CheckedIDs = new List<int>();
            for (int i = 0; i < AssignedHirc.BlendContainer.Count; i++)
            {
                int index = AssignedHirc.BlendContainer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].BlendContainer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }
            }

            for (int i = 0; i < AssignedHirc.BlendContainer.Count; i++)
            {
                int index = AssignedHirc.BlendContainer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].BlendContainer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                directParent = (int)Data[index].BlendContainer.NodeBase.AttenuationID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                if (EventAction.ContainsKey((int)Data[index].BlendContainer.ID))
                {
                    AssignedHirc.EventAction.AddRange(EventAction[(int)Data[index].BlendContainer.ID]);
                }
            }

            CheckedIDs = new List<int>();
            for (int i = 0; i < AssignedHirc.SwitchContainer.Count; i++)
            {
                int index = AssignedHirc.SwitchContainer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].SwitchContainer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }
            }

            for (int i = 0; i < AssignedHirc.SwitchContainer.Count; i++)
            {
                int index = AssignedHirc.SwitchContainer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].SwitchContainer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                directParent = (int)Data[index].SwitchContainer.NodeBase.AttenuationID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                if (EventAction.ContainsKey((int)Data[index].SwitchContainer.ID))
                {
                    AssignedHirc.EventAction.AddRange(EventAction[(int)Data[index].SwitchContainer.ID]);
                }
            }

            CheckedIDs = new List<int>();
            for (int i = 0; i < AssignedHirc.ActorMixer.Count; i++)
            {
                int index = AssignedHirc.ActorMixer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].ActorMixer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }
            }

            for (int i = 0; i < AssignedHirc.ActorMixer.Count; i++)
            {
                int index = AssignedHirc.ActorMixer[i];

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                int directParent = (int)Data[index].ActorMixer.NodeBase.DirectParentID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                directParent = (int)Data[index].ActorMixer.NodeBase.AttenuationID;
                if (!CheckedIDs.Contains(directParent))
                {
                    CheckHirc(AssignedHirc, directParent);
                    CheckedIDs.Add(directParent);
                }

                if (EventAction.ContainsKey((int)Data[index].ActorMixer.ID))
                {
                    AssignedHirc.EventAction.AddRange(EventAction[(int)Data[index].ActorMixer.ID]);
                }
            }

            foreach (int ID in AssignedHirc.EventAction)
            {
                int index = ID;

                if (index < 0 || index > Data.Count)
                {
                    continue;
                }

                if (Event.ContainsKey((int)Data[index].EventAction.ID))
                {
                    AssignedHirc.Event.AddRange(Event[(int)Data[index].EventAction.ID]);
                }
            }
        }

        private void CheckHirc(WemHirc AssignedHirc, int Key)
        {
            if (Settings.ContainsKey(Key))
            {
                AssignedHirc.Settings.AddRange(Settings[Key]);
            }
            else if (Event.ContainsKey(Key))
            {
                AssignedHirc.Event.AddRange(Event[Key]);
            }
            else if (RandomContainer.ContainsKey(Key))
            {
                AssignedHirc.RandomContainer.AddRange(RandomContainer[Key]);
            }
            else if (SwitchContainer.ContainsKey(Key))
            {
                AssignedHirc.SwitchContainer.AddRange(SwitchContainer[Key]);
            }
            else if (ActorMixer.ContainsKey(Key))
            {
                AssignedHirc.ActorMixer.AddRange(ActorMixer[Key]);
            }
            else if (AudioBus.ContainsKey(Key))
            {
                AssignedHirc.AudioBus.AddRange(AudioBus[Key]);
            }
            else if (BlendContainer.ContainsKey(Key))
            {
                AssignedHirc.BlendContainer.AddRange(BlendContainer[Key]);
            }
            else if (MusicSequence.ContainsKey(Key))
            {
                AssignedHirc.MusicSequence.AddRange(MusicSequence[Key]);
            }
            else if (MusicSwitchContainer.ContainsKey(Key))
            {
                AssignedHirc.MusicSwitchContainer.AddRange(MusicSwitchContainer[Key]);
            }
            else if (Attenuation.ContainsKey(Key))
            {
                AssignedHirc.Attenuation.AddRange(Attenuation[Key]);
            }
            else if (FeedbackNode.ContainsKey(Key))
            {
                AssignedHirc.FeedbackNode.AddRange(FeedbackNode[Key]);
            }
            else if (FxShareSet.ContainsKey(Key))
            {
                AssignedHirc.FxShareSet.AddRange(FxShareSet[Key]);
            }
            else if (FxCustom.ContainsKey(Key))
            {
                AssignedHirc.FxCustom.AddRange(FxCustom[Key]);
            }
            else if (AuxiliaryBus.ContainsKey(Key))
            {
                AssignedHirc.AuxiliaryBus.AddRange(AuxiliaryBus[Key]);
            }
            else if (LFO.ContainsKey(Key))
            {
                AssignedHirc.LFO.AddRange(LFO[Key]);
            }
            else if (Envelope.ContainsKey(Key))
            {
                AssignedHirc.Envelope.AddRange(Envelope[Key]);
            }
        }

        public TreeNode CreateNode(string name, int ID)
        {
            TreeNode tempItem = new TreeNode();

            if (Data.Count > ID && ID > -1 && Data[ID] != null)
            {
                switch (name)
                {
                    case "Actor Mixer":
                        if (Data[ID].ActorMixer != null)
                        {
                            tempItem.Text = Data[ID].ActorMixer.ID.ToString();
                            tempItem.Tag = Data[ID].ActorMixer;
                        }
                        break;

                    case "Attenuation":
                        if (Data[ID].Attenuation != null)
                        {
                            tempItem.Text = Data[ID].Attenuation.ID.ToString();
                            tempItem.Tag = Data[ID].Attenuation;
                        }
                        break;

                    case "Blend Container":
                        if (Data[ID].BlendContainer != null)
                        {
                            tempItem.Text = Data[ID].BlendContainer.ID.ToString();
                            tempItem.Tag = Data[ID].BlendContainer;
                        }
                        break;

                    case "Envelope":
                        if (Data[ID].Envelope != null)
                        {
                            tempItem.Text = Data[ID].Envelope.ID.ToString();
                            tempItem.Tag = Data[ID].Envelope;
                        }
                        break;

                    case "Event":
                        if (Data[ID].Event != null)
                        {
                            tempItem.Text = Data[ID].Event.ID.ToString();
                            tempItem.Tag = Data[ID].Event;
                        }
                        break;

                    case "Event Action":
                        if (Data[ID].EventAction != null)
                        {
                            tempItem.Text = Data[ID].EventAction.ID.ToString();
                            tempItem.Tag = Data[ID].EventAction;
                        }
                        break;

                    case "Feedback Node":
                        if (Data[ID].FeedbackNode != null)
                        {
                            tempItem.Text = Data[ID].FeedbackNode.ID.ToString();
                            tempItem.Tag = Data[ID].FeedbackNode;
                        }
                        break;

                    case "FxCustom":
                        if (Data[ID].FxCustom != null)
                        {
                            tempItem.Text = Data[ID].FxCustom.ID.ToString();
                            tempItem.Tag = Data[ID].FxCustom;
                        }
                        break;

                    case "FxShareSet":
                        if (Data[ID].FxShareSet != null)
                        {
                            tempItem.Text = Data[ID].FxShareSet.ID.ToString();
                            tempItem.Tag = Data[ID].FxShareSet;
                        }
                        break;

                    case "LFO":
                        if (Data[ID].LFO != null)
                        {
                            tempItem.Text = Data[ID].LFO.ID.ToString();
                            tempItem.Tag = Data[ID].LFO;
                        }
                        break;

                    case "Music Segment":
                        if (Data[ID].MusicSegment != null)
                        {
                            tempItem.Text = Data[ID].MusicSegment.ID.ToString();
                            tempItem.Tag = Data[ID].MusicSegment;
                        }
                        break;

                    case "Music Sequence":
                        if (Data[ID].MusicSequence != null)
                        {
                            tempItem.Text = Data[ID].MusicSequence.ID.ToString();
                            tempItem.Tag = Data[ID].MusicSequence;
                        }
                        break;

                    case "Music Switch Container":
                        if (Data[ID].MusicSwitchContainer != null)
                        {
                            tempItem.Text = Data[ID].MusicSwitchContainer.ID.ToString();
                            tempItem.Tag = Data[ID].MusicSwitchContainer;
                        }
                        break;

                    case "Music Track":
                        if (Data[ID].MusicTrack != null)
                        {
                            tempItem.Text = Data[ID].MusicTrack.ID.ToString();
                            tempItem.Tag = Data[ID].MusicTrack;
                        }
                        break;

                    case "Random Container":
                        if (Data[ID].RandomContainer != null)
                        {
                            tempItem.Text = Data[ID].RandomContainer.ID.ToString();
                            tempItem.Tag = Data[ID].RandomContainer;
                        }
                        break;

                    case "Settings":
                        if (Data[ID].SettingsObject != null)
                        {
                            tempItem.Text = Data[ID].SettingsObject.ID.ToString();
                            tempItem.Tag = Data[ID].SettingsObject;
                        }
                        break;

                    case "Sound SFX":
                        if (Data[ID].SoundSFXObject != null)
                        {
                            tempItem.Text = Data[ID].SoundSFXObject.ID.ToString();
                            tempItem.Tag = Data[ID].SoundSFXObject;
                        }
                        break;

                    case "Switch Container":
                        if (Data[ID].SwitchContainer != null)
                        {
                            tempItem.Text = Data[ID].SwitchContainer.ID.ToString();
                            tempItem.Tag = Data[ID].SwitchContainer;
                        }
                        break;
                }
            }

            return tempItem;
        }
    }
}
