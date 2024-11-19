using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using ResourceTypes.Actors;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.EntityDataStorage
{
    public static class EDSHashes
    {
        private static Dictionary<ulong, string> Hashes = new Dictionary<ulong, string>()
        {
            { 15612227391441882417, "plr" },
            { 10074354998178438286, "trfc" },
            { 17374058875421798942, "hunt" },
            { 12442775612479247440, "pt01" },
            { 12442775612479247443, "pt02" },
            { 12442775612479247442, "pt03" },
        };

        public static string GetHashName(ulong Hash)
        {
            return Hashes.ContainsKey(Hash) ? Hashes[Hash] : Hash.ToString();
        }
    }

    public class EntityDataStorageLoader
    {
        [ReadOnly(true)]
        public ActorEDSTypes EntityType { get; set; }
        [ReadOnly(true)]
        public int TableSize { get; set; }
        public ulong Hash { get; set; }
        [PropertyIgnoreByReflector]
        public string Name { get => EDSHashes.GetHashName(Hash); }
        public ulong[] TableHashes { get; set; }
        [Browsable(false)]
        public IActorExtraDataInterface[] Tables { get; set; }

        public EntityDataStorageLoader()
        {
            Tables = new IActorExtraDataInterface[0];
        }

        public void ReadFromFile(string fileName, bool isBigEndian)
        {
            using (var fileStream = new MemoryStream(File.ReadAllBytes(fileName)))
            {
                EntityType = (ActorEDSTypes)fileStream.ReadInt32(isBigEndian);

                Hash = fileStream.ReadUInt64(isBigEndian);
                TableSize = fileStream.ReadInt32(isBigEndian);

                uint numTables = fileStream.ReadUInt32(isBigEndian);
                TableHashes = new ulong[numTables];
                Tables = new IActorExtraDataInterface[numTables];

                // Iterate and read table hashes
                for (int i = 0; i < numTables; i++)
                {
                    TableHashes[i] = fileStream.ReadUInt64(isBigEndian);
                }

                // Iterate and Read all tables
                for (int i = 0; i < numTables; i++)
                {
                    using (MemoryStream stream = new MemoryStream(fileStream.ReadBytes(TableSize)))
                    {
                        var item = ActorFactory.LoadEntityDataStorage(EntityType, stream, isBigEndian);
                        Tables[i] = item;
                    }
                }
            }
        }

        public void WriteToFile(string fileName, bool isBigEndian)
        {
            using (var fileStream = new MemoryStream())
            {
                fileStream.Write((int)EntityType, isBigEndian);
                fileStream.Write(Hash, isBigEndian);
                fileStream.Write(TableSize, isBigEndian);
                fileStream.Write(Tables.Length, isBigEndian);

                // Write file hashes
                for(int i = 0; i < Tables.Length; i++)
                {
                    fileStream.Write(TableHashes[i], isBigEndian);
                }

                // Iterate through each table and write their data into bespoke streams, 
                // then dump into main stream.
                for (int i = 0; i < Tables.Length; i++)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        Tables[i].WriteToFile(stream, isBigEndian);
                        fileStream.Write(stream.ToArray());
                    }
                }

                File.WriteAllBytes(fileName, fileStream.ToArray());
            }
        }

        public void ConvertToXML(string Filename)
        {
            XElement Root = ReflectionHelpers.ConvertPropertyToXML(this);
            Root.Save(Filename);
        }

        public void ConvertFromXML(string Filename)
        {
            XElement LoadedDoc = XElement.Load(Filename);
            EntityDataStorageLoader FileContents = ReflectionHelpers.ConvertToPropertyFromXML<EntityDataStorageLoader>(LoadedDoc);

            // Copy data taken from loaded XML
            EntityType = FileContents.EntityType;
            TableSize = FileContents.TableSize;
            Hash = FileContents.Hash;
            TableHashes = FileContents.TableHashes;
            Tables = FileContents.Tables;
        }
    }
}