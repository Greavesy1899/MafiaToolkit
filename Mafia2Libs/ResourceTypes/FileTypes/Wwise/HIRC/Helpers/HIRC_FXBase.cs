using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FXBase
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject Parent { get; set; }
        public int FXType { get; set; }
        public uint FXID { get; set; }
        public byte[] FXParamBlock { get; set; }
        //public List<EQModule> eqModules { get; set; }
        //public float fxOutputLevel { get; set; }
        //public byte fxProcessLFE { get; set; }
        //public float fxPreDelay { get; set; }
        //public float fxFrontRearDelay { get; set; }
        //public float fxStereoWidth { get; set; }
        //public float fxInputCenterLevel { get; set; }
        //public float fxInputLFELevel { get; set; }
        //public float fxFrontLevel { get; set; }
        //public float fxRearLevel { get; set; }
        //public float fxCenterLevel { get; set; }
        //public float fxLFELevel { get; set; }
        //public float fxDryLevel { get; set; }
        //public float fxWetLevel { get; set; }
        //public uint fxAlgoType { get; set; } //0x00 = DOWNMIX
        //public float fxAttack { get; set; }
        //public float fxRelease { get; set; }
        //public float fxMin { get; set; }
        //public float fxMax { get; set; }
        //public float fxHold { get; set; }
        //public byte fxApplyDownStreamVolume { get; set; }
        //public uint fxGameParamID { get; set; }
        public List<MediaMap> FXMedia { get; set; }
        public List<RTPC> rtpc { get; set; }
        public List<RTPCInit> rtpcInit { get; set; }
        public FXBase(HIRCObject ParentObject, BinaryReader br)
        {
            Parent = ParentObject;
            FXID = br.ReadUInt32();
            uint FXBaseSize = br.ReadUInt32();
            FXParamBlock = br.ReadBytes((int)FXBaseSize);
            //if (FXBaseSize == 25)
            //{
            //    FXType = 1;
            //    fxAttack = br.ReadSingle();
            //    fxRelease = br.ReadSingle();
            //    fxMin = br.ReadSingle();
            //    fxMax = br.ReadSingle();
            //    fxHold = br.ReadSingle();
            //    fxApplyDownStreamVolume = br.ReadByte();
            //    fxGameParamID = br.ReadUInt32();
            //}
            //else if (FXBaseSize == 48)
            //{
            //    FXType = 2;
            //    fxPreDelay = br.ReadSingle();
            //    fxFrontRearDelay = br.ReadSingle();
            //    fxStereoWidth = br.ReadSingle();
            //    fxInputCenterLevel = br.ReadSingle();
            //    fxInputLFELevel = br.ReadSingle();
            //    fxFrontLevel = br.ReadSingle();
            //    fxRearLevel = br.ReadSingle();
            //    fxCenterLevel = br.ReadSingle();
            //    fxLFELevel = br.ReadSingle();
            //    fxDryLevel = br.ReadSingle();
            //    fxWetLevel = br.ReadSingle();
            //    fxAlgoType = br.ReadByte();
            //}
            //else if (FXBaseSize == 88)
            //{
            //    FXType = 3;
            //    FXParamBlock = br.ReadBytes(88);
            //}
            //else
            //{
            //    FXType = 4;
            //    eqModules = new List<EQModule>();
            //    uint eqCount = (FXBaseSize - 5) / 17;
            //
            //    for (int i = 0; i < eqCount; i++)
            //    {
            //        eqModules.Add(new EQModule(br));
            //    }
            //
            //    fxOutputLevel = br.ReadSingle();
            //    fxProcessLFE = br.ReadByte();
            //}

            FXMedia = new List<MediaMap>();
            uint FXMediaCount = br.ReadByte();

            for (int i = 0; i < FXMediaCount; i++)
            {
                FXMedia.Add(new MediaMap(br));
            }

            rtpc = new List<RTPC>();
            uint numRTPC = br.ReadUInt16();

            for (int i = 0; i < numRTPC; i++)
            {
                rtpc.Add(new RTPC(br));
            }

            rtpcInit = new List<RTPCInit>();
            uint numRTPCInit = br.ReadUInt16();

            for (int i = 0; i < numRTPCInit; i++)
            {
                rtpcInit.Add(new RTPCInit(br));
            }
        }

        public FXBase(HIRCObject ParentObject)
        {
            Parent = ParentObject;
            FXType = 0;
            FXID = 0;
            FXParamBlock = new byte[0];
            FXMedia = new List<MediaMap>();
            rtpc = new List<RTPC>();
            rtpcInit = new List<RTPCInit>();
    }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(FXID);
            bw.Write(FXParamBlock.Length);
            bw.Write(FXParamBlock);
            bw.Write((byte)FXMedia.Count);

            foreach (MediaMap map in FXMedia)
            {
                map.WriteToFile(bw);
            }

            bw.Write((short)rtpc.Count);

            foreach (RTPC value in rtpc)
            {
                value.WriteToFile(bw);
            }

            bw.Write((short)rtpcInit.Count);

            foreach (RTPCInit rtpc in rtpcInit)
            {
                rtpc.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int Length = 13 + FXParamBlock.Length + FXMedia.Count * 5 + rtpcInit.Count * 5;

            foreach (RTPC value in rtpc)
            {
                Length += value.GetLength();
            }

            return Length;
        }
    }
}
