﻿using System;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Navigation
{
    public class IType
    {
        public virtual void Read(BinaryReader Reader) { }
        public virtual void Write(BinaryWriter Writer) { }
        public virtual void DebugWrite(StreamWriter Writer) { }
    }

    public class AIWorld
    {
        public int Unk03 { get; set; }
        public int Unk05 { get; set; } // same as Unk3

        public string PartName { get; set; }
        public string KynogonString { get; set; }
        public IType[] AIPoints { get; set; }
        public string OriginStream { get; set; }

        // Always the same
        private int Unk02; // 1005
        private short Unk04; //might always == 2 (NB: This could actually "Type2")

        private string Unk01; // Usually empty?
        private string ConstAIWorldPart; // always AIWORLDPART.
        private byte Unk06;
        private uint Unk8; // NB: Do we have to assert if this isn't the usual value?

        public AIWorld(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Unk02 = reader.ReadInt32();
            Unk03 = reader.ReadInt32(); 
            Unk04 = reader.ReadInt16(); // NB: This could actually "Type2".
            PartName = StringHelpers.ReadString16(reader);
            Unk05 = reader.ReadInt32();
            KynogonString = StringHelpers.ReadString16(reader);
            Unk01 = StringHelpers.ReadString16(reader);
            ConstAIWorldPart = StringHelpers.ReadString16(reader);

            // TODO: This could just be a bool such as - 'if not empty'
            Unk06 = reader.ReadByte();
            if (Unk06 != 1)
            {
                throw new Exception("unk06 was not 1");
            }

            // Read AIPoints
            int unkCount = reader.ReadInt32();
            AIPoints = new IType[unkCount];
            for (int i = 0; i < unkCount; i++)
            {
                ushort TypeID = reader.ReadUInt16();
                AIPoints[i] = AIWorld_Factory.ConstructByTypeID(TypeID);
                AIPoints[i].Read(reader);
            }

            // Read footer data
            OriginStream = StringHelpers.ReadString(reader);
            uint originFileSize = reader.ReadUInt32();
            Unk8 = reader.ReadUInt32();

            DebugWriteToFile();
        }

        public void WriteToFile(BinaryWriter Writer)
        {
            Writer.Write(Unk02);
            Writer.Write(Unk03);
            Writer.Write(Unk04);
            StringHelpers.WriteString16(Writer, PartName);
            Writer.Write(Unk05);
            StringHelpers.WriteString16(Writer, KynogonString); 
            StringHelpers.WriteString16(Writer, Unk01);
            StringHelpers.WriteString16(Writer, ConstAIWorldPart);
            Writer.Write(Unk06);

            // Write AI Points
            Writer.Write(AIPoints.Length);
            foreach(IType AIPoint in AIPoints)
            {
                ushort TypeID = AIWorld_Factory.GetIDByType(AIPoint);
                Writer.Write(TypeID);
                AIPoint.Write(Writer);
            }

            // Write footer data
            StringHelpers.WriteString(Writer, OriginStream);
            Writer.Write(OriginStream.Length + 1);
            Writer.Write(Unk8);
        }

        public void DebugWriteToFile()
        {
            using(StreamWriter Writer = new StreamWriter(PartName + ".txt"))
            {
                Writer.WriteLine("Unk01: {0}", Unk01);
                Writer.WriteLine("Unk02: {0}", Unk02);
                Writer.WriteLine("Unk03: {0}", Unk03);
                Writer.WriteLine("Unk04: {0}", Unk04);
                Writer.WriteLine("PartName: {0}", PartName);
                Writer.WriteLine("Unk05: {0}", Unk05);
                Writer.WriteLine("KynogonString: {0}", KynogonString);
                Writer.WriteLine("ConstAIWorldPart: {0}", ConstAIWorldPart);
                Writer.WriteLine("Unk06: {0}", Unk06);

                foreach (IType AIPoint in AIPoints)
                {
                    AIPoint.DebugWrite(Writer);
                    Writer.WriteLine("");
                }

                Writer.WriteLine("OriginStream: {0}", OriginStream);
                Writer.WriteLine("Unk8: {0}", Unk8);
            }
        }
    }
}
