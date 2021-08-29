using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class FXBase
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        public int fxType { get; set; }
        public uint fxId { get; set; }
        public byte[] fxParamBlock { get; set; }
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
        //public byte fxApplyDownstreamVolume { get; set; }
        //public uint fxGameParamID { get; set; }
        public List<MediaMap> fxMedia { get; set; }
        public List<RTPC> rtpc { get; set; }
        public List<RTPCInit> rtpcInit { get; set; }
        public List<StateProp> stateProps { get; set; }
        public List<StateChunk> stateGroups { get; set; }
        public List<FxProp> fxProps { get; set; }
        public FXBase(HIRCObject parentObject, BinaryReader br)
        {
            parent = parentObject;
            fxId = br.ReadUInt32();
            uint fxBaseSize = br.ReadUInt32();
            fxParamBlock = br.ReadBytes((int)fxBaseSize);
            //if (fxBaseSize == 25)
            //{
            //    fxType = 1;
            //    fxAttack = br.ReadSingle();
            //    fxRelease = br.ReadSingle();
            //    fxMin = br.ReadSingle();
            //    fxMax = br.ReadSingle();
            //    fxHold = br.ReadSingle();
            //    fxApplyDownstreamVolume = br.ReadByte();
            //    fxGameParamID = br.ReadUInt32();
            //}
            //else if (fxBaseSize == 48)
            //{
            //    fxType = 2;
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
            //else if (fxBaseSize == 88)
            //{
            //    fxType = 3;
            //    fxParamBlock = br.ReadBytes(88);
            //}
            //else
            //{
            //    fxType = 4;
            //    eqModules = new List<EQModule>();
            //    uint eqCount = (fxBaseSize - 5) / 17;
            //
            //    for (int i = 0; i < eqCount; i++)
            //    {
            //        eqModules.Add(new EQModule(br));
            //    }
            //
            //    fxOutputLevel = br.ReadSingle();
            //    fxProcessLFE = br.ReadByte();
            //}

            fxMedia = new List<MediaMap>();
            uint fxMediaCount = br.ReadByte();

            for (int i = 0; i < fxMediaCount; i++)
            {
                fxMedia.Add(new MediaMap(br));
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

        public FXBase(HIRCObject parentObject)
        {
            parent = parentObject;
            fxType = 0;
            fxId = 0;
            fxParamBlock = new byte[0];
            fxMedia = new List<MediaMap>();
            rtpc = new List<RTPC>();
            rtpcInit = new List<RTPCInit>();
            fxProps = new List<FxProp>();
            stateProps = new List<StateProp>();
            stateGroups = new List<StateChunk>();
    }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(fxId);
            bw.Write(fxParamBlock.Length);
            bw.Write(fxParamBlock);
            bw.Write((byte)fxMedia.Count);

            foreach (MediaMap map in fxMedia)
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
            int length = 13 + fxParamBlock.Length + fxMedia.Count * 5 + rtpcInit.Count * 5;

            foreach (RTPC value in rtpc)
            {
                length += value.GetLength();
            }

            return length;
        }
    }
}
