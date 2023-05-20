using System;
using System.Diagnostics;
using System.IO;
using Utils.StringHelpers;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Navigation
{
    [PropertyClassAllowReflection, PropertyClassCheckInherited]
    public class INavigationData
    { 
    }

    //For AIWorld
    //Type 1: AI Group
    //Type 2: AI World Part
    //Type 3: AI Nav Point
    //Type 4: AI Cover Part
    //Type 5: AI Anim Point
    //Type 6: AI User Area
    //Type 7: AI Path Object
    //Type 8: AI Action Point
    //Type 9: AI Action Point 2?
    //Type 10: AI Action Point 3?
    //Type 11: AI Hiding Place
    //Type 12: AI Action Point 4?
    [PropertyClassAllowReflection]
    public class NAVData
    {
        //unk01_flags could be types; AIWORLDS seem to have 1005, while OBJDATA is 3604410608.
        FileInfo file;

        int fileSize; //size - 4;

        public uint Flags { get; set; }
        [PropertyIgnoreByReflector]
        public string Filename { get; set; }
        public INavigationData Data { get; set; }

        public NAVData(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public NAVData(FileInfo info)
        {
            file = info;

            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }

            if(Debugger.IsAttached)
            {
                WriteToFile();
            }
        }

        public void WriteToFile(bool bIsTest = true)
        {
            string OutputName = file.FullName;
            if (bIsTest)
            {
                OutputName = OutputName.Insert(OutputName.Length - 4, "_test");
            }

            using (NavigationWriter writer = new NavigationWriter(File.Open(OutputName, FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            fileSize = reader.ReadInt32();
            Flags = reader.ReadUInt32();

            //file name seems to be earlier.
            if (Flags == 3604410608)
            {
                Filename = StringHelpers.ReadString32(reader);

                long start = reader.BaseStream.Position;
                string hpdString = new string(reader.ReadChars(11));
                reader.ReadByte();
                int hpdVersion = reader.ReadInt32();
                if (hpdString == "Kynogon HPD" && hpdVersion == 2)
                {
                    Data = new HPDData();
                    (Data as HPDData).ReadFromFile(reader);
                }
                else
                {
                    reader.BaseStream.Seek(start, SeekOrigin.Begin);
                    Data = new OBJData(reader);
                }
            }
            else if (Flags == 1005)
            {
                Data = new AIWorld(reader);
            }
            else
            {
                throw new Exception("Found unexpected type: " + Flags);
            }
        }

        public void WriteToFile(NavigationWriter writer)
        {
            writer.Write(-1); //file size
            writer.Write(Flags);

            if (Flags == 3604410608)
            {
                writer.Write(Filename.Length);
                StringHelpers.WriteString(writer, Filename, false);
                if (Data is OBJData)
                {
                    (Data as OBJData).WriteToFile(writer);
                }
                else if(Data is HPDData)
                {
                    WriteHPD(writer);

                    // Our HPD file here actually should subtract 12 of the total.
                    writer.BaseStream.Position = 0;
                    writer.Write((uint)(writer.BaseStream.Length - 12));
                    return;
                }
            }
            else if(Flags == 1005)
            {
                (Data as AIWorld).WriteToFile(writer);
            }

            writer.BaseStream.Position = 0;
            writer.Write((uint)(writer.BaseStream.Length - 4));
        }

        private void WriteHPD(BinaryWriter writer)
        {
            var hpd = (Data as HPDData);

            StringHelpers.WriteString(writer, "Kynogon HPD");
            writer.Write(2); // HPD Version is always 2.
            hpd.WriteToFile(writer);
        }
    }
}