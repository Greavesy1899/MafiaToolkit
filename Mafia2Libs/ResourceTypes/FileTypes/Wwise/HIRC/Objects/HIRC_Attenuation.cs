using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise.Objects
{
    public class Attenuation
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public int isConeEnabled { get; set; }
        public float coneInsideDegrees { get; set; }
        public float coneOutsideDegrees { get; set; }
        public float coneOutsideVolume { get; set; }
        public float coneLoPass { get; set; }
        public float coneHiPass { get; set; }
        public byte curveToUse0 { get; set; }
        public byte curveToUse1 { get; set; }
        public byte curveToUse2 { get; set; }
        public byte curveToUse3 { get; set; }
        public byte curveToUse4 { get; set; }
        public byte curveToUse5 { get; set; }
        public byte curveToUse6 { get; set; }
        public List<Curve> curves { get; set; }
        public List<RTPC> rtpc { get; set; }
        [System.ComponentModel.Browsable(false)]
        public byte[] data { get; set; }
        public Attenuation(BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            isConeEnabled = br.ReadByte();

            if (isConeEnabled == 1)
            {
                coneInsideDegrees = br.ReadSingle();
                coneOutsideDegrees = br.ReadSingle();
                coneOutsideVolume = br.ReadSingle();
                coneLoPass = br.ReadSingle();
                coneHiPass = br.ReadSingle();
            }

            curveToUse0 = br.ReadByte();
            curveToUse1 = br.ReadByte();
            curveToUse2 = br.ReadByte();
            curveToUse3 = br.ReadByte();
            curveToUse4 = br.ReadByte();
            curveToUse5 = br.ReadByte();
            curveToUse6 = br.ReadByte();
            curves = new List<Curve>();
            uint numCurves = br.ReadByte();
            
            for (int i = 0; i < numCurves; i++)
            {
                curves.Add(new Curve(br));
            }

            rtpc = new List<RTPC>();
            uint numRTPC = br.ReadUInt16();
            
            for (int i = 0; i < numRTPC; i++)
            {
                rtpc.Add(new RTPC(br));
            }
        }

        public Attenuation()
        {
            type = 0;
            id = 0;
            isConeEnabled = 0;
            coneInsideDegrees = 0;
            coneOutsideDegrees = 0;
            coneOutsideVolume = 0;
            coneLoPass = 0;
            coneHiPass = 0;
            curveToUse0 = 0;
            curveToUse1 = 0;
            curveToUse2 = 0;
            curveToUse3 = 0;
            curveToUse4 = 0;
            curveToUse5 = 0;
            curveToUse6 = 0;
            curves = new List<Curve>();
            rtpc = new List<RTPC>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write((byte)isConeEnabled);

            if (isConeEnabled == 1)
            {
                bw.Write(coneInsideDegrees);
                bw.Write(coneOutsideDegrees);
                bw.Write(coneOutsideVolume);
                bw.Write(coneLoPass);
                bw.Write(coneHiPass);
            }

            bw.Write(curveToUse0);
            bw.Write(curveToUse1);
            bw.Write(curveToUse2);
            bw.Write(curveToUse3);
            bw.Write(curveToUse4);
            bw.Write(curveToUse5);
            bw.Write(curveToUse6);
            bw.Write((byte)curves.Count);

            foreach (Curve curve in curves)
            {
                curve.WriteToFile(bw);
            }

            bw.Write((short)rtpc.Count);

            foreach (RTPC value in rtpc)
            {
                value.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int length = 15;

            if (isConeEnabled == 1)
            {
                length += 20;
            }

            foreach (Curve curve in curves)
            {
                length += curve.GetLength();
            }

            foreach (RTPC value in rtpc)
            {
                length += value.GetLength();
            }

            return length;
        }
    }
}
