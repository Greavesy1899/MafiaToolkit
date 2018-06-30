using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    public class Actor
    {
        ActorDefinition[] definitions;
        ActorItem[] items;
        UnkSector1 unkSector;
        int unk16;

        public ActorItem[] Items {
            get { return items; }
        }

        public Actor(string file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int poolLength = reader.ReadInt32();
            string pool = new string(reader.ReadChars(poolLength));

            int hashesLength = reader.ReadInt32();

            definitions = new ActorDefinition[hashesLength];
            
            for (int i = 0; i != hashesLength; i++)
            {
                definitions[i] = new ActorDefinition(reader);
                int pos = definitions[i].namePos;
                definitions[i].name = pool.Substring(pos, pool.IndexOf('\0', pos) - pos);
            }

            unkSector = new UnkSector1(reader);

            items = new ActorItem[unkSector.ItemCount];

            for (int i = 0; i != unkSector.ItemCount; i++)
            {
                items[i] = new ActorItem(reader);
            }

            unk16 = reader.ReadInt32();

            if (unk16 != 0)
                throw new Exception("UNK16 is not 0. Message Greavesy with this message and the name of the SDS you tried to read");
        }

        public struct ActorDefinition
        {
            ulong hash; //hash, this is the same as in the frame.
            short unk01; //always zero
            public short namePos; //starting position for the name.
            int frameIndex; //links to FrameResource

            public string name;

            public ActorDefinition (BinaryReader reader)
            {
                hash = reader.ReadUInt64();
                unk01 = reader.ReadInt16();
                namePos = reader.ReadInt16();
                frameIndex = reader.ReadInt32();
                name = "";

                if (unk01 != 0)
                    throw new Exception("Not ZERO!");
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}", hash, name);
            }
        }

        public class UnkSector1
        {
            int filesize; //size of sector in bits. After this integer (so filesize - 4)
            short const_6; //always 6
            short const_2; //always 2
            int const_16; //always 16
            int filesize2;
            int unk12;
            
            int newpos;

            long pos_entity;
            int unk14;
            int unk13;

            int itemCount;

            public int ItemCount {
                get { return itemCount; }
                set { itemCount = value; }
            }
            

            List<temp_unk> temp_Unks = new List<temp_unk>();

            public UnkSector1(BinaryReader reader)
            {
                filesize = reader.ReadInt32(); 
                const_6 = reader.ReadInt16();
                const_2 = reader.ReadInt16();
                const_16 = reader.ReadInt32();
                filesize2 = reader.ReadInt32();
                unk12 = reader.ReadInt32();
                unk13 = reader.ReadInt32();

                if (const_2 != 2)
                    throw new Exception("const_2 is not 2");

                pos_entity = reader.BaseStream.Length - (filesize - filesize2);
                long tempPosition = reader.BaseStream.Position;
                unk14 = reader.ReadInt32();
                reader.BaseStream.Seek(tempPosition, SeekOrigin.Begin);
                newpos = (unk14 / 4 - 2)*4;
                reader.BaseStream.Seek(newpos, SeekOrigin.Current);

                while(reader.BaseStream.Position < pos_entity)
                {
                    temp_Unks.Add(new temp_unk(reader, temp_Unks.Count));
                }

                itemCount = reader.ReadInt32();
                reader.BaseStream.Seek(itemCount * 4, SeekOrigin.Current);
            }

            public struct temp_unk
            {
                ActorTypes bufferType;
                object data;
                byte[] buffer;

                public temp_unk(BinaryReader reader, int num)
                {
                    buffer = null;
                    data = null;

                    bufferType = (ActorTypes)reader.ReadInt32();
                    uint bufferLength = reader.ReadUInt32();

                    if (bufferType == ActorTypes.Pinup && bufferLength == 4)
                    {
                        data = new ActorPinup(reader);
                    }
                    else if (bufferType == ActorTypes.ScriptEntity && bufferLength == 100)
                    {
                        data = new ActorScriptEntity(reader);
                    }
                    else
                    {
                        buffer = reader.ReadBytes((int)bufferLength);
                        
                        string folder = "actors_unks/" + bufferType + "/";
                        string filename = folder + num + ".dat";

                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);

                        using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                        {
                            writer.Write(buffer);
                        }
                    }
                }

                public override string ToString()
                {
                    if(buffer != null)
                        return string.Format("{0}, {1}", bufferType, buffer.Length);
                    else
                        return string.Format("{0}", bufferType);
                }
            }

        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ActorItem
        {
            int size; //item size in bytes;
            string itemType;
            string entityType;
            string unkString;
            string unk2String;
            string frameName;
            string frameUnk;
            int actortype;
            ulong hash1;
            ulong hash2;
            Vector3 position;
            Vector2 rotation;
            Vector2 direction;
            Vector3 scale;
            bool unkBool;
            ushort propID;

            public string ItemType {
                get { return itemType; }
                set { itemType = value; }
            }
            public ulong Hash1 {
                get { return hash1; }
                set { hash1 = value; }
            }
            public Vector3 Position {
                get { return position; }
                set { position = value; }
            }
            public Vector2 Rotation {
                get { return rotation; }
                set { rotation = value; }
            }
            public ActorItem(BinaryReader reader)
            {
                size = reader.ReadInt32();

                itemType = readString(itemType, reader);
                entityType = readString(entityType, reader);
                unkString = readString(unkString, reader);
                unk2String = readString(unk2String, reader);
                frameName = readString(frameName, reader);
                frameUnk = readString(frameUnk, reader);
                actortype = reader.ReadInt32();
                hash1 = reader.ReadUInt64();
                hash2 = reader.ReadUInt64();
                position = new Vector3(reader);
                rotation = new Vector2(reader);
                direction = new Vector2(reader);
                scale = new Vector3(reader);
                unkBool = reader.ReadBoolean();
                propID = reader.ReadUInt16();
                reader.ReadByte();
            }

            private string readString(string text, BinaryReader reader)
            {
                byte length = reader.ReadByte();
                text = new string(reader.ReadChars(length - 2));
                reader.ReadByte();
                return text;
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", entityType, actortype, itemType);
            }
        }
    }
}
