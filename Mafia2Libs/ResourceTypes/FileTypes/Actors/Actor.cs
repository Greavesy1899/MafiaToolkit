using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utils.StringHelpers;

namespace ResourceTypes.Actors
{
    public class Actor
    {
        List<ActorDefinition> definitions;
        List<ActorEntry> items;
        string pool;
        //temp_unk start
        int filesize; //size of sector in bits. After this integer (so filesize - 4)
        short const6; //always 6
        short const2; //2 or 0
        byte[] unk02; //only full when const 2 == 0;
        int const16; //always 16
        int size;
        int unk12;
        int unk14;
        int unk13;
        List<ActorExtraData> extraData;
        string fileName;

        public List<ActorDefinition> Definitions {
            get { return definitions; }
        }
        public List<ActorEntry> Items {
            get { return items; }
        }
        public List<ActorExtraData> ExtraData {
            get { return extraData; }
            set { extraData = value; }
        }

        public Actor(string file)
        {
            fileName = file;
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        private string BuildDefinitions()
        {
            string bufferPool = "";
            bufferPool += "<scene>\0";
            Dictionary<string, int> names = new Dictionary<string, int>();
            for(int i = 0; i < definitions.Count; i++)
            {
                int startPos = 0;

                if(!string.IsNullOrEmpty(definitions[i].Name))
                {
                    if(!names.ContainsKey(definitions[i].Name))
                    {
                        startPos = bufferPool.Length;
                        bufferPool += definitions[i].Name;
                        bufferPool += '\0';
                        names.Add(definitions[i].Name, startPos);
                        definitions[i].NamePos = (ushort)startPos;
                    }
                    else
                    {
                        names.TryGetValue(definitions[i].Name, out startPos);
                        definitions[i].NamePos = (ushort)startPos;
                    }
                }
            }
            return bufferPool;
        }

        private void Sanitize()
        {
            var ordered = definitions.OrderBy(d => d.FrameNameHash);
            definitions = ordered.ToList();

            Dictionary<ushort, ushort> reorganisedKeys = new Dictionary<ushort, ushort>();
            extraData.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                if (!reorganisedKeys.ContainsKey(items[i].DataID))
                {
                    extraData.Add(items[i].Data);
                    reorganisedKeys.Add(items[i].DataID, (ushort)(extraData.Count - 1));
                    items[i].DataID = reorganisedKeys[items[i].DataID];
                }
                else
                {
                    items[i].DataID = reorganisedKeys[items[i].DataID];
                }
            }
            reorganisedKeys.Clear();
        }

        public ActorEntry CreateActorEntry(ActorTypes type, string name)
        {
            ActorExtraData extraData = new ActorExtraData();
            extraData.BufferType = type;
            extraData.Data = ActorFactory.CreateExtraData(type);

            ActorEntry entry = ActorFactory.CreateActorItem(type, name);
            entry.DataID = (ushort)(ExtraData.Count);
            entry.Data = extraData;

            ExtraData.Add(extraData);
            Items.Add(entry);
            return entry;
        }

        public ActorDefinition CreateActorDefinition(ActorEntry entry)
        {
            ActorDefinition definition = new ActorDefinition();
            definition.Name = entry.DefinitionName;
            definition.FrameNameHash = entry.FrameNameHash;
            Definitions.Add(definition);
            return definition;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int poolLength = reader.ReadInt32();
            pool = new string(reader.ReadChars(poolLength));

            int hashesLength = reader.ReadInt32();

            definitions = new List<ActorDefinition>();

            for (int i = 0; i != hashesLength; i++)
            {
                ActorDefinition definition = new ActorDefinition(reader);
                int pos = definition.NamePos;
                definition.Name = pool.Substring(pos, pool.IndexOf('\0', pos) - pos);
                definitions.Add(definition);
            }

            // Offset is required for reading cutscene names later.
            long actorDataOffset = reader.BaseStream.Position + 4;

            filesize = reader.ReadInt32();
            const6 = reader.ReadInt16();
            const2 = reader.ReadInt16();
            const16 = reader.ReadInt32();
            size = reader.ReadInt32(); //size of sector end.
            unk12 = reader.ReadInt32();
            unk13 = reader.ReadInt32();

            //if (const2 != 2)
            //    throw new Exception("const_6 is not 6");

            //if (const6 != 6)
            //    throw new Exception("const_2 is not 2");

            //if (const16 != 16)
            //    throw new Exception("const_16 is not 16");

            unk14 = reader.ReadInt32();

            if (const2 == 2)
            {
                int newpos = (unk14 / 4 - 2) * 4;
                if (unk14 - 8 != newpos)
                {
                    throw new FormatException("unk14-8 != newpos");
                }

                int count = (unk14 - 8) / sizeof(int);
                reader.BaseStream.Seek(unk14 - 12, SeekOrigin.Current);
                extraData = new List<ActorExtraData>();
                for (int i = 0; i < count; i++)
                {
                    extraData.Add(new ActorExtraData(reader));
                }
            }
            else
            {
                unk02 = reader.ReadBytes(size - unk14);
            }

            int itemCount = reader.ReadInt32();
            reader.BaseStream.Seek(itemCount * 4, SeekOrigin.Current);

            items = new List<ActorEntry>();
            for (int i = 0; i != itemCount; i++)
            {
                ActorEntry item = new ActorEntry(reader);
                item.Data = ExtraData[item.DataID];
                items.Add(item);
            }

            // Read how many cutscenes and check if we actually need to do anything.
            int numCutscenes = reader.ReadInt32();
            if (numCutscenes > 0)
            {
                long endPosition = 0;
                for (int i = 0; i < numCutscenes; i++)
                {
                    // Get the offset, then save the position so we can return.      
                    uint offset = reader.ReadUInt32();
                    long currentPosition = reader.BaseStream.Position;

                    // Seek to the offset and read cutscene name
                    reader.BaseStream.Seek(actorDataOffset + offset, SeekOrigin.Begin);
                    string cutsceneName = StringHelpers.ReadString(reader);
                    ushort cutscene_unk01 = reader.ReadUInt16();

                    // End position so we can make sure we have reached the end of file.
                    endPosition = reader.BaseStream.Position;

                    // Return to our offset.
                    reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
                }

                // Seek back to our end point and assert if we have not reached the end of file.
                reader.BaseStream.Position = endPosition;
            }

            Debug.Assert(reader.BaseStream.Position == reader.BaseStream.Length, "This is not the end of the file. Message Greavesy with this message and the name of the SDS you tried to read.");
        }

        public void WriteToFile()
        {
            Sanitize();
            pool = BuildDefinitions();
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Open)))
            {
                WriteToFile(writer);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            Dictionary<int, int> sanitizedIDs = new Dictionary<int, int>();

            // Cutscenes to save at the end of the file.
            List<ActorEntry> cutsceneEntries = new List<ActorEntry>();

            Sanitize();
            pool = BuildDefinitions();

            writer.Write(pool.Length);
            StringHelpers.WriteString(writer, pool, false);
            writer.Write(definitions.Count);
            for (int i = 0; i < definitions.Count; i++)
            {
                definitions[i].WriteToFile(writer);
            }

            long instancePos = writer.BaseStream.Position;
            writer.Write(0);
            writer.Write(const6);
            writer.Write(const2);
            writer.Write(const16);
            writer.Write(int.MinValue); //size
            writer.Write(int.MinValue); //unk12

            int instanceOffset = ((extraData.Count * sizeof(int)) + 8);
            writer.Write(0);

            //could do it so we seek to offset and save each one, but that would decrease performance. 
            for (int i = 0; i < extraData.Count; i++)
            {
                writer.Write(instanceOffset);
                instanceOffset += (extraData[i].Data != null ? extraData[i].Data.GetSize() : extraData[i].GetDataInBytes().Length) + 8;
            }

            for(int i = 0; i < extraData.Count; i++)
            {
                extraData[i].WriteToFile(writer);
            }

            int itemOffset = instanceOffset + (items.Count * sizeof(int)) + 16;
            long itemPos = writer.BaseStream.Position;
            writer.Write(items.Count);
            for(int i = 0; i < items.Count; i++)
            {
                writer.Write(itemOffset);
                itemOffset += items[i].CalculateSize();
            }

            for (int i = 0; i < items.Count; i++)
            {
                items[i].WriteToFile(writer);

                // We need to store cutscenes so we can save them for later.
                ActorTypes actorType = (ActorTypes)items[i].ActorTypeID;
                if (actorType == ActorTypes.C_Cutscene)
                {
                    cutsceneEntries.Add(items[i]);
                }
            }

            // Now we try and save the cutscene data which is stored at the bottom of the file.
            long cutsceneEntryOffset = writer.BaseStream.Position;
            long[] cutsceneOffsets = new long[cutsceneEntries.Count];
            writer.Write(cutsceneEntries.Count);

            // TODO: Maybe consider doing one loop rather than two.
            for (int i = 0; i < cutsceneEntries.Count; i++)
            {
                ActorEntry cutscene = cutsceneEntries[i];

                // Save our 'TEMP' offset; this stored the ptr to the string.
                cutsceneOffsets[i] = writer.BaseStream.Position;
                writer.Write(-1);
            }

            for (int i = 0; i < cutsceneEntries.Count; i++)
            {
                ActorEntry cutscene = cutsceneEntries[i];

                // Now we save the cutscene entity name and save the new offset;
                uint nameOffset = (uint)writer.BaseStream.Position;
                StringHelpers.WriteString(writer, cutscene.EntityName);
                writer.Write((ushort)0);

                long completeOffset = writer.BaseStream.Position;

                // Update our new offset and then return to our original offset;
                writer.BaseStream.Seek(cutsceneOffsets[i], SeekOrigin.Begin);
                uint newOffset = (uint)(nameOffset - (instancePos + 4));
                writer.Write(newOffset);

                writer.BaseStream.Seek(completeOffset, SeekOrigin.Begin);
            }

            //for that unknown value.
            long endPos = cutsceneEntryOffset - instancePos-4;
            long instanceLength = writer.BaseStream.Position - instancePos-4;
            long unk = writer.BaseStream.Position - itemPos;
            long size = instanceLength - unk;

            writer.BaseStream.Seek(instancePos, SeekOrigin.Begin);
            writer.Write((int)(instanceLength));
            writer.Write(const6);
            writer.Write(const2);
            writer.Write(const16);
            writer.Write((int)size); //size
            writer.Write((int)(endPos)); //unk12
        }
    }
}
