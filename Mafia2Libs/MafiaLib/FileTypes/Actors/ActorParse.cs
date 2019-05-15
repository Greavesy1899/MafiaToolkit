using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Actors
{
    public class Actor
    {
        ActorDefinition[] definitions;
        ActorItem[] items;
        ActorExtraData unkSector;
        int unk16;

        public ActorDefinition[] Definitions {
            get { return definitions; }
        }
        public ActorItem[] Items {
            get { return items; }
        }
        public ActorExtraData UnkSector {
            get { return unkSector; }
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
                int pos = definitions[i].NamePos;
                definitions[i].name = pool.Substring(pos, pool.IndexOf('\0', pos) - pos);
            }

            unkSector = new ActorExtraData(reader);

            int itemCount = reader.ReadInt32();
            reader.BaseStream.Seek(itemCount * 4, SeekOrigin.Current);

            items = new ActorItem[itemCount];
            for (int i = 0; i != itemCount; i++)
            {
                items[i] = new ActorItem(reader);
            }

            unk16 = reader.ReadInt32();

            //if (unk16 != 0)
                //throw new Exception("UNK16 is not 0. Message Greavesy with this message and the name of the SDS you tried to read");
        }

        public struct ActorDefinition
        {
            ulong hash; //hash, this is the same as in the frame.
            short unk01; //always zero
            short namePos; //starting position for the name.
            int frameIndex; //links to FrameResource
            public string name;

            public ulong Hash {
                get { return hash; }
                set { hash = value; }
            }
            public int FrameIndex {
                get { return frameIndex; }
                set { frameIndex = value; }
            }
            public short NamePos {
                get { return namePos; }
                set { namePos = value; }
            }

            public ActorDefinition (BinaryReader reader)
            {
                hash = 0;
                unk01 = 0;
                namePos = 0;
                frameIndex = 0;
                name = "";
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                hash = reader.ReadUInt64();
                unk01 = reader.ReadInt16();
                namePos = reader.ReadInt16();
                frameIndex = reader.ReadInt32();
                name = "";
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(hash);
                writer.Write(unk01);
                writer.Write(namePos);
                writer.Write(frameIndex);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}", hash, name);
            }
        }

        public class ActorExtraData
        {
            int filesize; //size of sector in bits. After this integer (so filesize - 4)
            short const6; //always 6
            short const2; //always 2
            int const16; //always 16
            int filesize2;
            int unk12;
            
            int newpos;

            long pos_entity;
            int unk14;
            int unk13;

            List<temp_unk> temp_Unks = new List<temp_unk>();

            public List<temp_unk> TempUnks {
                get { return temp_Unks; }
                set { temp_Unks = value; }
            }

            public ActorExtraData(BinaryReader reader)
            {
                filesize = reader.ReadInt32(); 
                const6 = reader.ReadInt16();
                const2 = reader.ReadInt16();
                const16 = reader.ReadInt32();
                filesize2 = reader.ReadInt32();
                unk12 = reader.ReadInt32();
                unk13 = reader.ReadInt32();

                //if (const2 != 2)
                //    throw new Exception("const_6 is not 6");

                if (const6 != 6)
                    throw new Exception("const_2 is not 2");

                if (const16 != 16)
                    throw new Exception("const_16 is not 16");

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
            }

            public struct temp_unk
            {
                ActorTypes bufferType;
                IActorExtraDataInterface data;
                byte[] buffer;

                public ActorTypes BufferType {
                    get { return bufferType; }
                    set { bufferType = value; }
                }

                [TypeConverter(typeof(ExpandableObjectConverter))]
                public IActorExtraDataInterface Data {
                    get { return data; }
                    set { data = value; }
                }
                public byte[] Buffer {
                    get { return buffer; }
                    set { buffer = value; }
                }

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
                    else if(bufferType == ActorTypes.Radio && bufferLength == 1028)
                    {
                        data = new ActorRadio(reader);
                    }
                    else if (bufferType == ActorTypes.Airplane && bufferLength == 4)
                    {
                        data = new ActorAircraft(reader);
                    }
                    else if (bufferType == ActorTypes.SpikeStrip && bufferLength == 4)
                    {
                        data = new ActorSpikeStrip(reader);
                    }
                    else if(bufferType == ActorTypes.Door && bufferLength == 364)
                    {
                        data = new ActorDoor(reader);
                    }
                    else if (bufferType == ActorTypes.Wardrobe && bufferLength == 208)
                    {
                        data = new ActorWardrobe(reader);
                    }
                    else if(bufferType == ActorTypes.Sound && bufferLength == 592)
                    {
                        //long pos = reader.BaseStream.Position;
                        //data = new ActorSoundEntity(reader);
                        //reader.BaseStream.Position = pos;
                        buffer = reader.ReadBytes((int)bufferLength);
                    }
                    else
                    {
                        buffer = reader.ReadBytes((int)bufferLength);
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
            ushort unk3;
            ushort propID;

            public int Size {
                get { return size; }
            }
            public string EntityType {
                get { return entityType; }
                set { entityType = value; }
            }
            public string ItemType {
                get { return itemType; }
                set { itemType = value; }
            }
            public string UnkString {
                get { return unkString; }
                set { unkString = value; }
            }
            public string Unk2String {
                get { return unk2String; }
                set { unk2String = value; }
            }
            public string FrameName {
                get { return frameName; }
                set { frameName = value; }
            }
            public string FrameUnk {
                get { return frameUnk; }
                set { frameUnk = value; }
            }
            public int ActorType {
                get { return actortype; }
                set { actortype = value; }
            }
            public ulong Hash1 {
                get { return hash1; }
                set { hash1 = value; }
            }
            public ulong Hash2 {
                get { return hash2; }
                set { hash2 = value; }
            }
            public Vector3 Position {
                get { return position; }
                set { position = value; }
            }
            public Vector2 Rotation {
                get { return rotation; }
                set { rotation = value; }
            }
            public Vector2 Direction {
                get { return direction; }
                set { direction = value; }
            }
            public Vector3 Scale {
                get { return scale; }
                set { scale = value; }
            }
            public ushort Unk3 {
                get { return unk3; }
                set { unk3 = value; }
            }
            public ushort PropID {
                get { return propID; }
                set { propID = value; }
            }

            public ActorItem(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                size = reader.ReadInt32();
                itemType = readString(reader);
                entityType = readString(reader);
                unkString = readString(reader);
                unk2String = readString(reader);
                frameName = readString(reader);
                frameUnk = readString(reader);
                actortype = reader.ReadInt32();
                hash1 = reader.ReadUInt64();
                hash2 = reader.ReadUInt64();
                position = Vector3Extenders.ReadFromFile(reader);
                rotation = Vector2Extenders.ReadFromFile(reader);
                direction = Vector2Extenders.ReadFromFile(reader);
                scale = Vector3Extenders.ReadFromFile(reader);
                unk3 = reader.ReadUInt16();
                propID = reader.ReadUInt16();
                Console.WriteLine("{0} {1}", unkString, (ActorTypes)actortype);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                size = 0;

                long pos = writer.BaseStream.Position;
                writer.Write(0);

                //item type
                writeString(itemType, writer);
                size += (itemType.Length+2);

                //entity type
                writeString(entityType, writer);
                size += (entityType.Length + 2);

                //unk string
                writeString(unkString, writer);
                size += (unkString.Length + 2);

                //unk2 string
                writeString(unk2String, writer);
                size += (unk2String.Length + 2);

                //frame name
                writeString(frameName, writer);
                size += (frameName.Length + 2);

                //frame unk
                writeString(frameUnk, writer);
                size += (frameUnk.Length + 2);

                writer.Write(actortype);
                writer.Write(hash1);
                writer.Write(hash2);
                position.WriteToFile(writer);
                rotation.WriteToFile(writer);
                direction.WriteToFile(writer);
                scale.WriteToFile(writer);
                writer.Write(unk3);
                writer.Write(propID);
                size += 68;

                long pos2 = writer.BaseStream.Position;
                writer.BaseStream.Position = pos;
                writer.Write(size);
                writer.BaseStream.Position = pos2;
            }

            private string readString(BinaryReader reader)
            {
                byte length = reader.ReadByte();
                string text = new string(reader.ReadChars(length - 2));
                reader.ReadByte();
                return text;
            }

            private void writeString(string text, BinaryWriter writer)
            {
                writer.Write((byte)text.Length+2);
                writer.Write(text.ToCharArray());
                writer.Write((byte)0);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}, {3}", entityType, actortype, itemType, propID);
            }
        }
    }
}
