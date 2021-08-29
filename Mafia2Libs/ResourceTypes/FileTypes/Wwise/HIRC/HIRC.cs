using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using ResourceTypes.Wwise.Objects;
using System.Xml.Linq;

namespace ResourceTypes.Wwise
{
    public class HIRCObject
    {
        public BNK bnk;
        public AudioDevice AudioDevice { get; set; }
        public State State { get; set; }
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
        public byte[] data { get; set; }
        public long lastPos { get; set; }
        public HIRCObject(BNK mainBnk, BinaryReader br)
        {
            bnk = mainBnk;
            int type = br.ReadByte();
            lastPos = br.BaseStream.Position;
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
                    data = br.ReadBytes(length + 5);
                    MessageBox.Show("Detected Unknown HIRC Object!", "Detected unkown HIRC Object type at: " + (lastPos - 1).ToString("X"));
                    break;
            }
        }

        public HIRCObject(BNK bnkObject)
        {
            bnk = bnkObject;
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
            else if (State != null)
            {
                length += State.GetLength();
            }
            else if (AudioDevice != null)
            {
                length += AudioDevice.GetLength();
            }
            else
            {
                if (data != null)
                {
                    length += data.Length;
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
        public Dictionary<uint,List<int>> AudioDevice = new Dictionary<uint, List<int>>();
        public Dictionary<int,List<int>> ActorMixer = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> Attenuation = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> AudioBus = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> AuxiliaryBus = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> BlendContainer = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> DialogueEvent = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> Envelope = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> Event = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> EventAction = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> FeedbackNode = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> FxCustom = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> FxShareSet = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> LFO = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> MotionBus = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> MusicSegment = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> MusicSequence = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> MusicSwitchContainer = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> MusicTrack = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> RandomContainer = new Dictionary<int, List<int>>();
        public Dictionary<int,List<int>> Settings = new Dictionary<int, List<int>>();
        public Dictionary<uint,List<int>> SoundSFX = new Dictionary<uint, List<int>>();
        public Dictionary<uint,List<int>> State = new Dictionary<uint, List<int>>();
        public Dictionary<int,List<int>> SwitchContainer = new Dictionary<int, List<int>>();
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
    }
}
