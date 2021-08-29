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
        public HIRCObject parent { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public int actionType { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public string actionTypeName { get; set; }
        public uint objectID { get; set; }
        public byte propSectionInit { get; set; } //bit0 = "bIsBus"
        public List<Prop> props { get; set; }
        public List<RangedModifier> rangedModifiers { get; set; }
        public byte BitVector { get; set; } //bit0 = "eFadeCurve"
        public uint bnkFileHashname { get; set; }
        public uint stateGroupID { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public string stateGroupName { get; set; }
        public uint targetStateID { get; set; } //529726532 = "end"
        [System.ComponentModel.ReadOnly(true)]
        public string targetStateName { get; set; }
        public uint switchGroupID { get; set; }
        public uint switchStateID { get; set; }
        public byte valueMeaning { get; set; } //0x02 = Offset
        public float randomizerModifierBase { get; set; }
        public float randomizerModifierMin { get; set; }
        public float randomizerModifierMax { get; set; }
        public byte pauseActionSpecificParams { get; set; } //bit0 = "bIncludePendingResume", bit1 = "bApplyToStateTransitions", bit2 = "bApplyToDynamicSequence"
        public byte stopActionSpecificParams { get; set; } //bit1 = "bApplyToStateTransitions", bit2 = "bApplyToDynamicSequence"
        public byte resumeActionSpecificParams { get; set; } //bit0 = "bIsMasterResume", bit1 = "bApplyToStateTransitions", bit2 = "bApplyToDynamicSequence"
        public float rangedParameterBase { get; set; }
        public float rangedParameterMin { get; set; }
        public float rangedParameterMax { get; set; }
        public byte bypassTransition { get; set; }
        public int seekRelativeToDuration { get; set; }
        public byte snapToNearestMarker { get; set; }
        public int bypass { get; set; }
        public byte targetMask { get; set; }
        public List<ExceptionItem> exceptions { get; set; }

        [System.ComponentModel.Browsable(false)]
        private Dictionary<int, string> actionTypeNames = new Dictionary<int, string>();

        public EventAction(HIRCObject parentObject, BinaryReader br, int iType)
        {
            parent = parentObject;
            type = iType;
            actionTypeNames.Add(258, "Stop_E");
            actionTypeNames.Add(259, "Stop_E_O");
            actionTypeNames.Add(514, "Pause_E");
            actionTypeNames.Add(515, "Pause_E_O");
            actionTypeNames.Add(520, "Pause_AE");
            actionTypeNames.Add(770, "Resume_E");
            actionTypeNames.Add(771, "Resume_E_O");
            actionTypeNames.Add(772, "Resume_ALL");
            actionTypeNames.Add(1027, "Play");
            actionTypeNames.Add(1538, "Mute_M");
            actionTypeNames.Add(1539, "Mute_O");
            actionTypeNames.Add(1794, "Unmute_M");
            actionTypeNames.Add(1795, "Unmute_O");
            actionTypeNames.Add(1796, "Unmute_ALL");
            actionTypeNames.Add(2050, "SetPitch_M");
            actionTypeNames.Add(2051, "SetPitch_O");
            actionTypeNames.Add(2307, "ResetPitch_O");
            actionTypeNames.Add(2562, "SetVolume_M");
            actionTypeNames.Add(2563, "SetVolume_O");
            actionTypeNames.Add(2818, "ResetVolume_M");
            actionTypeNames.Add(2819, "ResetVolume_O");
            actionTypeNames.Add(3074, "SetBusVolume_M");
            actionTypeNames.Add(3330, "ResetBusVolume_M");
            actionTypeNames.Add(3332, "ResetBusVolume_ALL");
            actionTypeNames.Add(3586, "SetLPF_M");
            actionTypeNames.Add(3587, "SetLPF_O");
            actionTypeNames.Add(3842, "ResetLPF_M");
            actionTypeNames.Add(3843, "ResetLPF_O");
            actionTypeNames.Add(3845, "ResetLPF_ALL_O");
            actionTypeNames.Add(4612, "SetState");
            actionTypeNames.Add(4866, "SetGameParameter");
            actionTypeNames.Add(4867, "SetGameParameter_O");
            actionTypeNames.Add(5122, "ResetGameParameter");
            actionTypeNames.Add(5123, "ResetGameParameter_O");
            actionTypeNames.Add(6401, "SetSwitch");
            actionTypeNames.Add(6658, "BypassFX_M");
            actionTypeNames.Add(7426, "Trigger_E");
            actionTypeNames.Add(7427, "Trigger_E_O");
            actionTypeNames.Add(7683, "Seek_E_O");
            actionTypeNames.Add(7685, "Seek_ALL_O");
            actionTypeNames.Add(8195, "SetHPF_O");

            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            actionType = br.ReadUInt16();

            if (actionTypeNames.ContainsKey(actionType))
            {
                actionTypeName = actionTypeNames[actionType];
            }
            else
            {
                actionTypeName = "Unknown";
            }

            objectID = br.ReadUInt32();
            propSectionInit = br.ReadByte();
            props = new List<Prop>();
            int propsCount = br.ReadByte();

            for (int i = 0; i < propsCount; i++)
            {
                byte id = br.ReadByte();
                props.Add(new Prop(id));
            }

            foreach (Prop prop in props)
            {
                prop.value = br.ReadUInt32();
            }

            rangedModifiers = new List<RangedModifier>();
            int rangedModifiersCount = br.ReadByte();

            for (int i = 0; i < rangedModifiersCount; i++)
            {
                byte id = br.ReadByte();
                uint min = br.ReadUInt32();
                uint max = br.ReadUInt32();
                rangedModifiers.Add(new RangedModifier(id, min, max));
            }

            //Data changes depending on action type

            uint ExceptionListSize;
            exceptions = new List<ExceptionItem>();

            switch (actionType)
            {
                case 7426: //Trigger_E
                case 7427: //Trigger_E_O
                case 7939: //Release_O
                    break;

                case 1027: //Play
                    BitVector = br.ReadByte();
                    bnkFileHashname = br.ReadUInt32();
                    break;

                case 514: //Pause_E
                case 515: //Pause_E_O
                case 520: //Pause_AE
                    BitVector = br.ReadByte();
                    pauseActionSpecificParams = br.ReadByte();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 770: //Resume_E
                case 771: //Resume_E_O
                case 772: //Resume_ALL
                    BitVector = br.ReadByte();
                    resumeActionSpecificParams = br.ReadByte();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 6401: //SetSwitch
                    switchGroupID = br.ReadUInt32();
                    switchStateID = br.ReadUInt32();
                    break;

                case 4612: //SetState
                    stateGroupID = br.ReadUInt32();
                    targetStateID = br.ReadUInt32();
                    break;

                case 7682: //Seek_E
                case 7683: //Seek_E_O
                case 7685: //Seek_ALL_O
                    seekRelativeToDuration = br.ReadByte();
                    randomizerModifierBase = br.ReadSingle(); //"fSeekValue"
                    randomizerModifierMin = br.ReadSingle(); //"fSeekValueMin"
                    randomizerModifierMax = br.ReadSingle(); //"fSeekValueMax"
                    snapToNearestMarker = br.ReadByte();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        exceptions.Add(new ExceptionItem(br));
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
                    valueMeaning = br.ReadByte();
                    randomizerModifierBase = br.ReadSingle();
                    randomizerModifierMin = br.ReadSingle();
                    randomizerModifierMax = br.ReadSingle();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 6658: //BypassFX_M
                    bypass = br.ReadByte();
                    targetMask = br.ReadByte();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        exceptions.Add(new ExceptionItem(br));
                    }

                    break;

                case 8451: //PlayEvent
                    break;

                case 4866: //SetGameParameter
                case 4867: //SetGameParameter_O
                case 5122: //ResetGameParameter
                case 5123: //ResetGameParameter_O
                    BitVector = br.ReadByte();
                    bypassTransition = br.ReadByte();
                    valueMeaning = br.ReadByte();
                    rangedParameterBase = br.ReadSingle();
                    rangedParameterMin = br.ReadSingle();
                    rangedParameterMax = br.ReadSingle();
                    ExceptionListSize = br.ReadUInt32();

                    for (int i = 0; i < ExceptionListSize; i++)
                    {
                        exceptions.Add(new ExceptionItem(br));
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
                        exceptions.Add(new ExceptionItem(br));
                    }

                    break;
            }
        }

        public EventAction()
        {
            type = 0;
            id = 0;
            actionType = 0;
            actionTypeName = "";
            objectID = 0;
            propSectionInit = 0;
            props = new List<Prop>();
            rangedModifiers = new List<RangedModifier>();
            BitVector = 0;
            bnkFileHashname = 0;
            stateGroupID = 0;
            targetStateID = 0;
            switchGroupID = 0;
            switchStateID = 0;
            valueMeaning = 0;
            randomizerModifierBase = 0;
            randomizerModifierMin = 0;
            randomizerModifierMax = 0;
            pauseActionSpecificParams = 0;
            stopActionSpecificParams = 0;
            resumeActionSpecificParams = 0;
            rangedParameterBase = 0;
            rangedParameterMin = 0;
            rangedParameterMax = 0;
            bypassTransition = 0;
            seekRelativeToDuration = 0;
            snapToNearestMarker = 0;
            bypass = 0;
            targetMask = 0;
            exceptions = new List<ExceptionItem>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write((short)actionType);
            bw.Write(objectID);
            bw.Write(propSectionInit);
            bw.Write((byte)props.Count);

            foreach (Prop prop in props)
            {
                bw.Write((byte)prop.id);
            }

            foreach (Prop prop in props)
            {
                bw.Write(prop.value);
            }

            bw.Write((byte)rangedModifiers.Count);

            foreach (RangedModifier mod in rangedModifiers)
            {
                bw.Write(mod.id);
                bw.Write(mod.min);
                bw.Write(mod.max);
            }

            switch (actionType)
            {
                case 7426:  //Trigger_E
                case 7427:  //Trigger_E_O
                case 7939: //Release_O
                    break;

                case 1027: //Play
                    bw.Write(BitVector);
                    bw.Write(bnkFileHashname);
                    break;

                case 514: //Pause_E
                case 515: //Pause_E_O
                case 520: //Pause_AE
                    bw.Write(BitVector);
                    bw.Write(pauseActionSpecificParams);
                    bw.Write(exceptions.Count);

                    foreach (ExceptionItem except in exceptions)
                    {
                        bw.Write(except.id);
                        bw.Write((byte)except.isBus);
                    }

                    break;

                case 770: //Resume_E
                case 771: //Resume_E_O
                case 772: //Resume_ALL
                    bw.Write(BitVector);
                    bw.Write(resumeActionSpecificParams);
                    bw.Write(exceptions.Count);

                    foreach (ExceptionItem except in exceptions)
                    {
                        bw.Write(except.id);
                        bw.Write((byte)except.isBus);
                    }

                    break;

                case 6401: //SetSwitch
                    bw.Write(switchGroupID);
                    bw.Write(switchStateID);
                    break;

                case 4612: //SetState
                    bw.Write(stateGroupID);
                    bw.Write(targetStateID);
                    break;

                case 7682: //Seek_E
                case 7683: //Seek_E_O
                case 7685: //Seek_ALL_O
                    bw.Write((byte)seekRelativeToDuration);
                    bw.Write(randomizerModifierBase);
                    bw.Write(randomizerModifierMin);
                    bw.Write(randomizerModifierMax);
                    bw.Write(snapToNearestMarker);
                    bw.Write(exceptions.Count);

                    foreach (ExceptionItem except in exceptions)
                    {
                        bw.Write(except.id);
                        bw.Write((byte)except.isBus);
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
                    bw.Write(valueMeaning);
                    bw.Write(randomizerModifierBase);
                    bw.Write(randomizerModifierMin);
                    bw.Write(randomizerModifierMax);
                    bw.Write(exceptions.Count);

                    foreach (ExceptionItem except in exceptions)
                    {
                        bw.Write(except.id);
                        bw.Write((byte)except.isBus);
                    }

                    break;

                case 6658: //BypassFX_M
                    bw.Write((byte)bypass);
                    bw.Write(targetMask);
                    bw.Write(exceptions.Count);

                    foreach (ExceptionItem except in exceptions)
                    {
                        bw.Write(except.id);
                        bw.Write((byte)except.isBus);
                    }

                    break;

                case 4866: //SetGameParameter
                case 4867: //SetGameParameter_O
                case 5122: //ResetGameParameter
                case 5123: //ResetGameParameter_O
                    bw.Write(BitVector);
                    bw.Write(bypassTransition);
                    bw.Write(valueMeaning);
                    bw.Write(rangedParameterBase);
                    bw.Write(rangedParameterMin);
                    bw.Write(rangedParameterMax);
                    bw.Write(exceptions.Count);

                    foreach (ExceptionItem except in exceptions)
                    {
                        bw.Write(except.id);
                        bw.Write((byte)except.isBus);
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
                    bw.Write(exceptions.Count);

                    foreach (ExceptionItem except in exceptions)
                    {
                        bw.Write(except.id);
                        bw.Write((byte)except.isBus);
                    }

                    break;

                case 8451: //PlayEvent
                    break;
            }
        }

        public int GetLength()
        {
            int length = 13 + props.Count * 5 + rangedModifiers.Count * 9;

            switch (actionType)
            {
                case 7426:  //Trigger_E
                case 7427:  //Trigger_E_O
                case 7939: //Release_O
                    break;

                case 1027: //Play
                    length += 5;
                    break;

                case 514: //Pause_E
                case 515: //Pause_E_O
                case 520: //Pause_AE
                    length += 6 + exceptions.Count * 5;
                    break;

                case 770: //Resume_E
                case 771: //Resume_E_O
                case 772: //Resume_ALL
                    length += 6 + exceptions.Count * 5;
                    break;

                case 6401: //SetSwitch
                    length += 8;
                    break;

                case 4612: //SetState
                    length += 8;
                    break;

                case 7682: //Seek_E
                case 7683: //Seek_E_O
                case 7685: //Seek_ALL_O
                    length += 18 + exceptions.Count * 5;
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
                    length += 18 + exceptions.Count * 5;
                    break;

                case 6658: //BypassFX_M
                    length += 6 + exceptions.Count * 5;
                    break;

                case 8451: //PlayEvent
                    break;

                case 4866: //SetGameParameter
                case 4867: //SetGameParameter_O
                case 5122: //ResetGameParameter
                case 5123: //ResetGameParameter_O
                    length += 19 + exceptions.Count * 5;
                    break;

                case 258: //Stop_E
                case 259: //Stop_E_O
                case 261: //Stop_ALL_O
                    length += 5 + exceptions.Count * 5;
                    break;

                case 1538: //Mute_M
                case 1539: //Mute_O
                case 1794: //Unmute_M
                case 1795: //Unmute_O
                case 1796: //Unmute_ALL
                case 8706: //ResetPlaylist_E
                    length += 5 + exceptions.Count * 5;
                    break;

                default:
                    break;
            }

            return length;
        }
    }
}
