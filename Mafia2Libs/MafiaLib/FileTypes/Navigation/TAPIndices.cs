using System;
using System.Collections.Generic;
using System.IO;

namespace ResourceTypes.Navigation
{
    public class TAPIndices
    {
        public struct VAPSegment
        {
            public int Unk01;
            public int Unk02;
            public int Unk03;

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", Unk01, Unk02, Unk03);
            }

        }

        private const int magicTAP0 = 0x30504154;
        private const int magicUAP0 = 0x30504155;
        private const int magicVAP0 = 0x30504156;
        private VAPSegment[] mappingData;

        public TAPIndices(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            List<string> mappings = new List<string>();
            if (reader.ReadInt32() == magicTAP0)
            {
                var size = reader.ReadInt32();

                if (reader.ReadInt32() == magicUAP0)
                {
                    var unk0 = reader.ReadInt32();
                    var unk1 = reader.ReadInt32();

                    if (reader.ReadInt32() == magicVAP0)
                    {
                        var chunkSize = reader.ReadInt32();
                        var numMappings = reader.ReadInt32();
                        mappingData = new VAPSegment[numMappings];

                        for (int i = 0; i != numMappings; i++)
                        {
                            var data = new VAPSegment();
                            data.Unk01 = reader.ReadInt32();
                            data.Unk02 = reader.ReadInt32();
                            data.Unk03 = reader.ReadInt32();
                            mappings.Add(string.Format("{0} {1} {2} {3}", i, data.Unk01, data.Unk02, data.Unk03));
                            mappingData[i] = data;
                        }
                    }
                }
            }
            File.WriteAllLines("MappingData.txt", mappings);
        }
    }
}