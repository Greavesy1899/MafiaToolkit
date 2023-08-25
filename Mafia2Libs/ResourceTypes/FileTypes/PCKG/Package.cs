using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTypes.PCKG
{
    public class Package
    {
        private string ExtractedFolder;
        private uint Magic = 0x474B4350;
        private int Version = 2;
        public List<PackageEntry> Entries { get; set; } = new List<PackageEntry>();

        public Package()
        {

        }

        public Package(string extractedFolder)
        {
            ExtractedFolder = extractedFolder;

            if (!Directory.Exists(ExtractedFolder))
            {
                Directory.CreateDirectory(ExtractedFolder);
            }
        }

        public Package(string fileName, string extractedFolder)
        {
            ExtractedFolder = extractedFolder;

            if (!Directory.Exists(ExtractedFolder))
            {
                Directory.CreateDirectory(ExtractedFolder);
            }

            Unpack(fileName);
        }

        public bool Unpack(string fileName)
        {
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    return Unpack(stream);
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Unpack(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                return Unpack(br);
            }
        }

        public bool Unpack(BinaryReader br)
        {
            if (br.ReadUInt32() != Magic)
            {
                return false;
            }

            if (br.ReadInt32() != Version)
            {
                return false;
            }

            int Count = br.ReadInt32();
            br.ReadInt32(); //Const -1

            using (MemoryStream stream = new MemoryStream(br.ReadBytes(Count * 32)))
            {
                using (BinaryReader mbr = new BinaryReader(stream))
                {
                    for (int i = 0; i < Count; i++)
                    {
                        var Entry = new PackageEntry(mbr);
                        br.BaseStream.Position = Entry.Offset;
                        File.WriteAllBytes(Path.Combine(ExtractedFolder, Entry.Name), br.ReadBytes(Entry.CompressedSize));
                    }
                }
            }

            return true;
        }

        public bool Pack(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                return Pack(stream);
            }
        }

        public bool Pack(Stream stream)
        {
            using (BinaryWriter bw = new BinaryWriter(stream))
            {
                return Pack(bw);
            }
        }

        public bool Pack(BinaryWriter bw)
        {
            foreach (string file in Directory.GetFiles(ExtractedFolder, "*.dds"))
            {
                PackageEntry entry = new PackageEntry();

                string sID = Path.GetFileNameWithoutExtension(file);
                ulong ID = 0;

                if (!ulong.TryParse(sID, System.Globalization.NumberStyles.HexNumber, null, out ID))
                {
                    return false;
                }

                entry.ID = ID;

                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        br.BaseStream.Position = 12;
                        entry.Height = (ushort)br.ReadUInt32();
                        entry.Width = (ushort)br.ReadUInt32();
                        br.BaseStream.Position = 0;
                        entry.Data = br.ReadBytes((int)br.BaseStream.Length);
                    }
                }

                entry.UncompressedSize = entry.Data.Length;
                entry.CompressedSize = entry.Data.Length;

                Entries.Add(entry);
            }

            Entries = Entries.OrderBy(entry => entry.ID).ToList();

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter mbw = new BinaryWriter(stream))
                {
                    mbw.Write(Magic);
                    mbw.Write(Version);
                    mbw.Write(Entries.Count);
                    mbw.Write(-1);

                    for (int i = 0; i < Entries.Count; i++)
                    {
                        var entry = Entries[i];

                        if (i > 0)
                        {
                            var prevEntry = Entries[i - 1];
                            entry.Offset = prevEntry.Offset + prevEntry.CompressedSize;
                        }
                        else
                        {
                            entry.Offset += Entries.Count * 32;
                        }

                        entry.Write(mbw);
                        bw.BaseStream.Position = entry.Offset;
                        bw.Write(entry.Data);
                    }
                }

                bw.BaseStream.Position = 0;
                bw.Write(stream.ToArray());
            }

            return true;
        }
    }
}
