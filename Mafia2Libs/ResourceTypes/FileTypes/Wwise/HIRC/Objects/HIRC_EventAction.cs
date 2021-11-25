using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows;
using ResourceTypes.Wwise.Helpers;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Objects
{
    public class EventAction
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject Parent { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public int ActionType { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public string ActionTypeName { get; set; }
        public uint ObjectID { get; set; }
        public byte PropSectionInit { get; set; } //bit0 = "bIsBus"
        public List<Prop> Props { get; set; }
        public List<RangedModifier> RangedModifiers { get; set; }
        public byte BitVector { get; set; } //bit0 = "eFadeCurve"
        public uint BnkFileHashname { get; set; }
        public uint StateGroupID { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public string StateGroupName { get; set; }
        public uint TargetStateID { get; set; } //529726532 = "end"
        [System.ComponentModel.ReadOnly(true)]
        public string TargetStateName { get; set; }
        public uint SwitchGroupID { get; set; }
        public uint SwitchStateID { get; set; }
        public byte ValueMeaning { get; set; } //0x02 = Offset
        public float RandomizerModifierBase { get; set; }
        public float RandomizerModifierMin { get; set; }
        public float RandomizerModifierMax { get; set; }
        public byte PauseActionSpecificParams { get; set; } //bit0 = "bIncludePendingResume", bit1 = "bApplyToStateTransitions", bit2 = "bApplyToDynamicSequence"
        public byte StopActionSpecificParams { get; set; } //bit1 = "bApplyToStateTransitions", bit2 = "bApplyToDynamicSequence"
        public byte ResumeActionSpecificParams { get; set; } //bit0 = "bIsMasterResume", bit1 = "bApplyToStateTransitions", bit2 = "bApplyToDynamicSequence"
        public float RangedParameterBase { get; set; }
        public float RangedParameterMin { get; set; }
        public float RangedParameterMax { get; set; }
        public byte BypassTransition { get; set; }
        public int SeekRelativeToDuration { get; set; }
        public byte SnapToNearestMarker { get; set; }
        public int Bypass { get; set; }
        public byte TargetMask { get; set; }
        public List<ExceptionItem> Exceptions { get; set; }

        [System.ComponentModel.Browsable(false)]
        private Dictionary<int, string> ActionTypeNames = new Dictionary<int, string>();

        public EventAction(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Parent = ParentObject;
            Type = iType;
            ActionTypeNames.Add(258, "Stop_E");
            ActionTypeNames.Add(259, "Stop_E_O");
            ActionTypeNames.Add(514, "Pause_E");
            ActionTypeNames.Add(515, "Pause_E_O");
            ActionTypeNames.Add(520, "Pause_AE");
            ActionTypeNames.Add(770, "Resume_E");
            ActionTypeNames.Add(771, "Resume_E_O");
            ActionTypeNames.Add(772, "Resume_ALL");
            ActionTypeNames.Add(1027, "Play");
            ActionTypeNames.Add(1538, "Mute_M");
            ActionTypeNames.Add(1539, "Mute_O");
            ActionTypeNames.Add(1794, "Unmute_M");
            ActionTypeNames.Add(1795, "Unmute_O");
            ActionTypeNames.Add(1796, "Unmute_ALL");
            ActionTypeNames.Add(2050, "SetPitch_M");
            ActionTypeNames.Add(2051, "SetPitch_O");
            ActionTypeNames.Add(2307, "ResetPitch_O");
            ActionTypeNames.Add(2562, "SetVolume_M");
            ActionTypeNames.Add(2563, "SetVolume_O");
            ActionTypeNames.Add(2818, "ResetVolume_M");
            ActionTypeNames.Add(2819, "ResetVolume_O");
            ActionTypeNames.Add(3074, "SetBusVolume_M");
            ActionTypeNames.Add(3330, "ResetBusVolume_M");
            ActionTypeNames.Add(3332, "ResetBusVolume_ALL");
            ActionTypeNames.Add(3586, "SetLPF_M");
            ActionTypeNames.Add(3587, "SetLPF_O");
            ActionTypeNames.Add(3842, "ResetLPF_M");
            ActionTypeNames.Add(3843, "ResetLPF_O");
            ActionTypeNames.Add(3845, "ResetLPF_ALL_O");
            ActionTypeNames.Add(4612, "SetState");
            ActionTypeNames.Add(4866, "SetGameParameter");
            ActionTypeNames.Add(4867, "SetGameParameter_O");
            ActionTypeNames.Add(5122, "ResetGameParameter");
            ActionTypeNames.Add(5123, "ResetGameParameter_O");
            ActionTypeNames.Add(6401, "SetSwitch");
            ActionTypeNames.Add(6658, "BypassFX_M");
            ActionTypeNames.Add(7426, "Trigger_E");
            ActionTypeNames.Add(7427, "Trigger_E_O");
            ActionTypeNames.Add(7683, "Seek_E_O");
            ActionTypeNames.Add(7685, "Seek_ALL_O");
            ActionTypeNames.Add(8195, "SetHPF_O");

            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            ActionType = br.ReadUInt16();

            if (ActionTypeNames.ContainsKey(ActionType))
            {
                ActionTypeName = ActionTypeNames[ActionType];
            }
            else
            {
                ActionTypeName = "Unknown";
            }

            ObjectID = br.ReadUInt32();
            PropSectionInit = br.ReadByte();
            Props = new List<Prop>();
            int PropsCount = br.ReadByte();

            for (int i = 0; i < PropsCount; i++)
            {
                byte ID = br.ReadByte();
                Props.Add(new Prop(ID));
            }

            foreach (Prop prop in Props)
            {
                prop.Value = br.ReadUInt32();
            }

            RangedModifiers = new List<RangedModifier>();
            int RangedModifiersCount = br.ReadByte();

            for (int i = 0; i < RangedModifiersCount; i++)
            {
                byte ID = br.ReadByte();
                uint min = br.ReadUInt32();
                uint max = br.ReadUInt32();
                RangedModifiers.Add(new RangedModifier(ID, min, max));
            }

            //Data changes depending on action Type

            uint ExceptionListSize;
            Exceptions = new List<ExceptionItem>();

            switch (ActionType)
            {
                case 7426: //Trigger_E
                case 7427: //Trigger_E_O
                case 7939: //Release_O
                    break;

                case 1027: //Play
                    BitVector = br.ReadByte();
                    BnkFileHashname = br.ReadUInt32();
                    break;

                case 514: //Pause_E
                case 515: //Pause_E_O
                case 520: //Pause_AE
                    BitVector = br.ReadByte();
                    PauseActionSpecificParams = br.ReadByte();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        Exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 770: //Resume_E
                case 771: //Resume_E_O
                case 772: //Resume_ALL
                    BitVector = br.ReadByte();
                    ResumeActionSpecificParams = br.ReadByte();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        Exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 6401: //SetSwitch
                    SwitchGroupID = br.ReadUInt32();
                    SwitchStateID = br.ReadUInt32();
                    break;

                case 4612: //SetState
                    StateGroupID = br.ReadUInt32();
                    TargetStateID = br.ReadUInt32();
                    break;

                case 7682: //Seek_E
                case 7683: //Seek_E_O
                case 7685: //Seek_ALL_O
                    SeekRelativeToDuration = br.ReadByte();
                    RandomizerModifierBase = br.ReadSingle(); //"fSeekValue"
                    RandomizerModifierMin = br.ReadSingle(); //"fSeekValueMin"
                    RandomizerModifierMax = br.ReadSingle(); //"fSeekValueMax"
                    SnapToNearestMarker = br.ReadByte();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        Exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 2050: //SetPitch_M
                case 2051: //SetPitch_O
                case 2306: //Unknown
                case 2307: //ResetPitch_O
                case 2562: //SetVolume_M
                case 2563: //SetVolume_O
                case 2818: //ResetVolume_M
                case 2819: //ResetVolume_O
                case 3074: //SetBusVolume_M
                case 3330: //ResetBusVolume_M
                case 3332: //ResetBusVolume_ALL
                case 3586: //SetLPF_M
                case 3587: //SetLPF_O
                case 3842: //ResetLPF_M
                case 3843: //ResetLPF_O
                case 3845: //ResetLPF_ALL_O
                case 8195: //SetHPF_O
                    BitVector = br.ReadByte();
                    ValueMeaning = br.ReadByte();
                    RandomizerModifierBase = br.ReadSingle();
                    RandomizerModifierMin = br.ReadSingle();
                    RandomizerModifierMax = br.ReadSingle();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        Exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 6658: //BypassFX_M
                    Bypass = br.ReadByte();
                    TargetMask = br.ReadByte();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        Exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 8451: //PlayEvent
                    break;

                case 4866: //SetGameParameter
                case 4867: //SetGameParameter_O
                case 5122: //ResetGameParameter
                case 5123: //ResetGameParameter_O
                    BitVector = br.ReadByte();
                    BypassTransition = br.ReadByte();
                    ValueMeaning = br.ReadByte();
                    RangedParameterBase = br.ReadSingle();
                    RangedParameterMin = br.ReadSingle();
                    RangedParameterMax = br.ReadSingle();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        Exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 258: //Stop_E
                case 259: //Stop_E_O
                case 261: //Stop_ALL_O
                case 1538: //Mute_M
                case 1539: //Mute_O
                case 1794: //Unmute_M
                case 1795: //Unmute_O
                case 1796: //Unmute_ALL
                case 8706: //ResetPlaylist_E
                    BitVector = br.ReadByte();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        Exceptions.Add(new ExceptionItem(br));
                    }

                    break;
            }
        }

        public EventAction()
        {
            Type = 0;
            ID = 0;
            ActionType = 0;
            ActionTypeName = "";
            ObjectID = 0;
            PropSectionInit = 0;
            Props = new List<Prop>();
            RangedModifiers = new List<RangedModifier>();
            BitVector = 0;
            BnkFileHashname = 0;
            StateGroupID = 0;
            TargetStateID = 0;
            SwitchGroupID = 0;
            SwitchStateID = 0;
            ValueMeaning = 0;
            RandomizerModifierBase = 0;
            RandomizerModifierMin = 0;
            RandomizerModifierMax = 0;
            PauseActionSpecificParams = 0;
            StopActionSpecificParams = 0;
            ResumeActionSpecificParams = 0;
            RangedParameterBase = 0;
            RangedParameterMin = 0;
            RangedParameterMax = 0;
            BypassTransition = 0;
            SeekRelativeToDuration = 0;
            SnapToNearestMarker = 0;
            Bypass = 0;
            TargetMask = 0;
            Exceptions = new List<ExceptionItem>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write((short)ActionType);
            bw.Write(ObjectID);
            bw.Write(PropSectionInit);
            bw.Write((byte)Props.Count);

            foreach (Prop prop in Props)
            {
                bw.Write((byte)prop.ID);
            }

            foreach (Prop prop in Props)
            {
                bw.Write(prop.Value);
            }

            bw.Write((byte)RangedModifiers.Count);

            foreach (RangedModifier mod in RangedModifiers)
            {
                bw.Write(mod.ID);
                bw.Write(mod.Min);
                bw.Write(mod.Max);
            }

            switch (ActionType)
            {
                case 7426:  //Trigger_E
                case 7427:  //Trigger_E_O
                case 7939: //Release_O
                    break;

                case 1027: //Play
                    bw.Write(BitVector);
                    bw.Write(BnkFileHashname);
                    break;

                case 514: //Pause_E
                case 515: //Pause_E_O
                case 520: //Pause_AE
                    bw.Write(BitVector);
                    bw.Write(PauseActionSpecificParams);
                    bw.Write(Exceptions.Count);

                    foreach (ExceptionItem except in Exceptions)
                    {
                        bw.Write(except.ID);
                        bw.Write((byte)except.IsBus);
                    }

                    break;

                case 770: //Resume_E
                case 771: //Resume_E_O
                case 772: //Resume_ALL
                    bw.Write(BitVector);
                    bw.Write(ResumeActionSpecificParams);
                    bw.Write(Exceptions.Count);

                    foreach (ExceptionItem except in Exceptions)
                    {
                        bw.Write(except.ID);
                        bw.Write((byte)except.IsBus);
                    }

                    break;

                case 6401: //SetSwitch
                    bw.Write(SwitchGroupID);
                    bw.Write(SwitchStateID);
                    break;

                case 4612: //SetState
                    bw.Write(StateGroupID);
                    bw.Write(TargetStateID);
                    break;

                case 7682: //Seek_E
                case 7683: //Seek_E_O
                case 7685: //Seek_ALL_O
                    bw.Write((byte)SeekRelativeToDuration);
                    bw.Write(RandomizerModifierBase);
                    bw.Write(RandomizerModifierMin);
                    bw.Write(RandomizerModifierMax);
                    bw.Write(SnapToNearestMarker);
                    bw.Write(Exceptions.Count);

                    foreach (ExceptionItem except in Exceptions)
                    {
                        bw.Write(except.ID);
                        bw.Write((byte)except.IsBus);
                    }

                    break;

                case 2050: //SetPitch_M
                case 2051: //SetPitch_O
                case 2306: //Unknown
                case 2307: //ResetPitch_O
                case 2562: //SetVolume_M
                case 2563: //SetVolume_O
                case 2818: //ResetVolume_M
                case 2819: //ResetVolume_O
                case 3074: //SetBusVolume_M
                case 3330: //ResetBusVolume_M
                case 3332: //ResetBusVolume_ALL
                case 3586: //SetLPF_M
                case 3587: //SetLPF_O
                case 3842: //ResetLPF_M
                case 3843: //ResetLPF_O
                case 3845: //ResetLPF_ALL_O
                case 8195: //SetHPF_O
                    bw.Write(BitVector);
                    bw.Write(ValueMeaning);
                    bw.Write(RandomizerModifierBase);
                    bw.Write(RandomizerModifierMin);
                    bw.Write(RandomizerModifierMax);
                    bw.Write(Exceptions.Count);

                    foreach (ExceptionItem except in Exceptions)
                    {
                        bw.Write(except.ID);
                        bw.Write((byte)except.IsBus);
                    }

                    break;

                case 6658: //BypassFX_M
                    bw.Write((byte)Bypass);
                    bw.Write(TargetMask);
                    bw.Write(Exceptions.Count);

                    foreach (ExceptionItem except in Exceptions)
                    {
                        bw.Write(except.ID);
                        bw.Write((byte)except.IsBus);
                    }

                    break;

                case 4866: //SetGameParameter
                case 4867: //SetGameParameter_O
                case 5122: //ResetGameParameter
                case 5123: //ResetGameParameter_O
                    bw.Write(BitVector);
                    bw.Write(BypassTransition);
                    bw.Write(ValueMeaning);
                    bw.Write(RangedParameterBase);
                    bw.Write(RangedParameterMin);
                    bw.Write(RangedParameterMax);
                    bw.Write(Exceptions.Count);

                    foreach (ExceptionItem except in Exceptions)
                    {
                        bw.Write(except.ID);
                        bw.Write((byte)except.IsBus);
                    }

                    break;

                case 258: //Stop_E
                case 259: //Stop_E_O
                case 261: //Stop_ALL_O
                case 1538: //Mute_M
                case 1539: //Mute_O
                case 1794: //Unmute_M
                case 1795: //Unmute_O
                case 1796: //Unmute_ALL
                case 8706: //ResetPlaylist_E
                    bw.Write(BitVector);
                    bw.Write(Exceptions.Count);

                    foreach (ExceptionItem except in Exceptions)
                    {
                        bw.Write(except.ID);
                        bw.Write((byte)except.IsBus);
                    }

                    break;

                case 8451: //PlayEvent
                    break;
            }
        }

        public int GetLength()
        {
            int Length = 13 + Props.Count * 5 + RangedModifiers.Count * 9;

            switch (ActionType)
            {
                case 7426:  //Trigger_E
                case 7427:  //Trigger_E_O
                case 7939: //Release_O
                    break;

                case 1027: //Play
                    Length += 5;
                    break;

                case 514: //Pause_E
                case 515: //Pause_E_O
                case 520: //Pause_AE
                    Length += 6 + Exceptions.Count * 5;
                    break;

                case 770: //Resume_E
                case 771: //Resume_E_O
                case 772: //Resume_ALL
                    Length += 6 + Exceptions.Count * 5;
                    break;

                case 6401: //SetSwitch
                    Length += 8;
                    break;

                case 4612: //SetState
                    Length += 8;
                    break;

                case 7682: //Seek_E
                case 7683: //Seek_E_O
                case 7685: //Seek_ALL_O
                    Length += 18 + Exceptions.Count * 5;
                    break;

                case 2050: //SetPitch_M
                case 2051: //SetPitch_O
                case 2306: //Unknown
                case 2307: //ResetPitch_O
                case 2562: //SetVolume_M
                case 2563: //SetVolume_O
                case 2818: //ResetVolume_M
                case 2819: //ResetVolume_O
                case 3074: //SetBusVolume_M
                case 3330: //ResetBusVolume_M
                case 3332: //ResetBusVolume_ALL
                case 3586: //SetLPF_M
                case 3587: //SetLPF_O
                case 3842: //ResetLPF_M
                case 3843: //ResetLPF_O
                case 3845: //ResetLPF_ALL_O
                case 8195: //SetHPF_O
                    Length += 18 + Exceptions.Count * 5;
                    break;

                case 6658: //BypassFX_M
                    Length += 6 + Exceptions.Count * 5;
                    break;

                case 8451: //PlayEvent
                    break;

                case 4866: //SetGameParameter
                case 4867: //SetGameParameter_O
                case 5122: //ResetGameParameter
                case 5123: //ResetGameParameter_O
                    Length += 19 + Exceptions.Count * 5;
                    break;

                case 258: //Stop_E
                case 259: //Stop_E_O
                case 261: //Stop_ALL_O
                    Length += 5 + Exceptions.Count * 5;
                    break;

                case 1538: //Mute_M
                case 1539: //Mute_O
                case 1794: //Unmute_M
                case 1795: //Unmute_O
                case 1796: //Unmute_ALL
                case 8706: //ResetPlaylist_E
                    Length += 5 + Exceptions.Count * 5;
                    break;

                default:
                    break;
            }

            return Length;
        }
    }
}
