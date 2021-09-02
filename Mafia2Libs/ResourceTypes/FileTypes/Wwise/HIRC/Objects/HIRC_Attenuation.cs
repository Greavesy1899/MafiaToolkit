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
        public int Type { get; set; }
        public uint ID { get; set; }
        public int IsConeEnabled { get; set; }
        public float ConeInsideDegrees { get; set; }
        public float ConeOutsideDegrees { get; set; }
        public float ConeOutsideVolume { get; set; }
        public float ConeLoPass { get; set; }
        public float ConeHiPass { get; set; }
        public byte CurveToUse0 { get; set; }
        public byte CurveToUse1 { get; set; }
        public byte CurveToUse2 { get; set; }
        public byte CurveToUse3 { get; set; }
        public byte CurveToUse4 { get; set; }
        public byte CurveToUse5 { get; set; }
        public byte CurveToUse6 { get; set; }
        public List<Curve> Curves { get; set; }
        public List<RTPC> rtpc { get; set; }
        [System.ComponentModel.Browsable(false)]
        public byte[] Data { get; set; }
        public Attenuation(BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            IsConeEnabled = br.ReadByte();

            if (IsConeEnabled == 1)
            {
                ConeInsideDegrees = br.ReadSingle();
                ConeOutsideDegrees = br.ReadSingle();
                ConeOutsideVolume = br.ReadSingle();
                ConeLoPass = br.ReadSingle();
                ConeHiPass = br.ReadSingle();
            }

            CurveToUse0 = br.ReadByte();
            CurveToUse1 = br.ReadByte();
            CurveToUse2 = br.ReadByte();
            CurveToUse3 = br.ReadByte();
            CurveToUse4 = br.ReadByte();
            CurveToUse5 = br.ReadByte();
            CurveToUse6 = br.ReadByte();
            Curves = new List<Curve>();
            uint numCurves = br.ReadByte();
            
            for (int i = 0; i < numCurves; i++)
            {
                Curves.Add(new Curve(br));
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
            Type = 0;
            ID = 0;
            IsConeEnabled = 0;
            ConeInsideDegrees = 0;
            ConeOutsideDegrees = 0;
            ConeOutsideVolume = 0;
            ConeLoPass = 0;
            ConeHiPass = 0;
            CurveToUse0 = 0;
            CurveToUse1 = 0;
            CurveToUse2 = 0;
            CurveToUse3 = 0;
            CurveToUse4 = 0;
            CurveToUse5 = 0;
            CurveToUse6 = 0;
            Curves = new List<Curve>();
            rtpc = new List<RTPC>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write((byte)IsConeEnabled);

            if (IsConeEnabled == 1)
            {
                bw.Write(ConeInsideDegrees);
                bw.Write(ConeOutsideDegrees);
                bw.Write(ConeOutsideVolume);
                bw.Write(ConeLoPass);
                bw.Write(ConeHiPass);
            }

            bw.Write(CurveToUse0);
            bw.Write(CurveToUse1);
            bw.Write(CurveToUse2);
            bw.Write(CurveToUse3);
            bw.Write(CurveToUse4);
            bw.Write(CurveToUse5);
            bw.Write(CurveToUse6);
            bw.Write((byte)Curves.Count);

            foreach (Curve Curve in Curves)
            {
                Curve.WriteToFile(bw);
            }

            bw.Write((short)rtpc.Count);

            foreach (RTPC value in rtpc)
            {
                value.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int Length = 15;

            if (IsConeEnabled == 1)
            {
                Length += 20;
            }

            foreach (Curve Curve in Curves)
            {
                Length += Curve.GetLength();
            }

            foreach (RTPC value in rtpc)
            {
                Length += value.GetLength();
            }

            return Length;
        }
    }
}
