using System.ComponentModel;
using System.IO;
using System.Windows;
using Utils.Extensions;
using ResourceTypes.Actors;

namespace ResourceTypes.EntityDataStorage
{
    public class EntityDataStorageLoader
    {
        [ReadOnly(true)]
        public ActorEDSTypes EntityType { get; set; }
        [ReadOnly(true)]
        public int TableSize { get; set; }
        public ulong Hash { get; set; }
        public ulong[] TableHashes { get; set; }
        [Browsable(false)]
        public IActorExtraDataInterface[] Tables { get; set; }

        public EntityDataStorageLoader()
        {
        }

        public void ReadFromFile(string fileName, bool isBigEndian)
        {
            using (var fileStream = new MemoryStream(File.ReadAllBytes(fileName)))
            {
                EntityType = (ActorEDSTypes)fileStream.ReadInt32(isBigEndian);

                if(EntityType == ActorEDSTypes.C_Train)
                {
                    MessageBox.Show(string.Format("Detected unsupported entity. The EntityStorageData will not load."), "Toolkit", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Hash = fileStream.ReadUInt64(isBigEndian);
                TableSize = fileStream.ReadInt32(isBigEndian);

                uint numTables = fileStream.ReadUInt32(isBigEndian);
                TableHashes = new ulong[numTables];
                Tables = new IActorExtraDataInterface[numTables];

                for (int i = 0; i < numTables; i++)
                {
                    TableHashes[i] = fileStream.ReadUInt64(isBigEndian);
                }

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

                for(int i = 0; i < Tables.Length; i++)
                {
                    fileStream.Write(TableHashes[i], isBigEndian);
                }

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
    }
}