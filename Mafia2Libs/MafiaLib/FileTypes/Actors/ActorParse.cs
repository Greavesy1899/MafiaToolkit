using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.Actors
{
    public class Actor
    {
        ActorDefinition[] definitions;
        ActorItem[] items;
        int unk16;
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
        ActorExtraData[] extraData;
        string fileName;

        public ActorDefinition[] Definitions {
            get { return definitions; }
        }
        public ActorItem[] Items {
            get { return items; }
        }
        public ActorExtraData[] ExtraData {
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
            string pool = "";
            pool += "<scene>\0";
            Dictionary<string, int> names = new Dictionary<string, int>();
            for(int i = 0; i < items.Length; i++)
            {
                int startPos = 0;

                if(!string.IsNullOrEmpty(items[i].DefinitionName))
                {
                    if(!names.ContainsKey(items[i].DefinitionName))
                    {
                        startPos = pool.Length;
                        pool += items[i].DefinitionName;
                        pool += '\0';
                        names.Add(items[i].DefinitionName, startPos);
                    }
                }
                else
                {
                    names.TryGetValue(items[i].DefinitionName, out startPos);
                }

                for (int y = 0; y < definitions.Length; y++)
                {
                    if (definitions[y].Hash == items[i].FrameNameHash)
                    {
                        definitions[y].NamePos = (short)startPos;
                    }
                }
            }
            return pool;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int poolLength = reader.ReadInt32();
            pool = new string(reader.ReadChars(poolLength));

            int hashesLength = reader.ReadInt32();

            definitions = new ActorDefinition[hashesLength];

            for (int i = 0; i != hashesLength; i++)
            {
                definitions[i] = new ActorDefinition(reader);
                int pos = definitions[i].NamePos;
                definitions[i].name = pool.Substring(pos, pool.IndexOf('\0', pos) - pos);
            }

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
                    throw new FormatException("unk14-8 != newpos");

                int count = (unk14 - 8) / sizeof(int);
                reader.BaseStream.Seek(unk14 - 12, SeekOrigin.Current);
                extraData = new ActorExtraData[count];
                for (int i = 0; i < count; i++)
                    extraData[i] = new ActorExtraData(reader);
            }
            else
            {
                unk02 = reader.ReadBytes(size - unk14);
            }

            int itemCount = reader.ReadInt32();
            reader.BaseStream.Seek(itemCount * 4, SeekOrigin.Current);

            items = new ActorItem[itemCount];
            for (int i = 0; i != itemCount; i++)
            {
                items[i] = new ActorItem(reader);
                items[i].Data = ExtraData[items[i].DataID];
            }

            unk16 = reader.ReadInt32();

            //if (unk16 != 0)
            //    throw new Exception("UNK16 is not 0. Message Greavesy with this message and the name of the SDS you tried to read");
        }

        public void WriteToFile()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Open)))
            {
                WriteToFile(writer);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(pool.Length);
            StringHelpers.WriteString(writer, pool, false);
            writer.Write(definitions.Length);
            for (int i = 0; i < definitions.Length; i++)
                definitions[i].WriteToFile(writer);

            long instancePos = writer.BaseStream.Position;
            writer.Write(0);
            writer.Write(const6);
            writer.Write(const2);
            writer.Write(const16);
            writer.Write(int.MinValue); //size
            writer.Write(int.MinValue); //unk12

            int instanceOffset = ((extraData.Length * sizeof(int)) + 8);
            writer.Write(0);

            //could do it so we seek to offset and save each one, but that would decrease performance. 
            for (int i = 0; i < extraData.Length; i++)
            {
                writer.Write(instanceOffset);
                instanceOffset += (extraData[i].Data != null ? extraData[i].Data.GetSize() : extraData[i].GetDataInBytes().Length) + 8;
            }
            for (int i = 0; i < extraData.Length; i++)
                extraData[i].WriteToFile(writer);

            int itemOffset = instanceOffset + (items.Length * sizeof(int)) + 16;
            long itemPos = writer.BaseStream.Position;
            writer.Write(items.Length);
            for(int i = 0; i < items.Length; i++)
            {
                writer.Write(itemOffset);
                itemOffset += items[i].CalculateSize();
            }

            for (int i = 0; i < items.Length; i++)
                items[i].WriteToFile(writer);

            //for that unknown value.
            writer.Write(0);
            long endPos = writer.BaseStream.Position - instancePos-8;
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
            //writer.Write(0); //unk13
            
            //unk16 = reader.ReadInt32();

            //if (unk16 != 0)
            //    throw new Exception("UNK16 is not 0. Message Greavesy with this message and the name of the SDS you tried to read");
        }

        public class ActorDefinition
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

            public ActorDefinition(BinaryReader reader)
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

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ActorItem
        {
            int size; //item size in bytes;
            string actorTypeName; //actor type (string)
            string entityType; //entity name
            string unkString;
            string unk2String;
            string definitionName; //actor name
            string frameName; //frame name
            int actortypeID;
            ulong defintionHash; //definition hash
            ulong frameNameHash; //frame name hash
            Vector3 position;
            Vector4 quat;
            Vector3 euler;
            Vector3 scale;
            ushort unk3;
            ushort dataID;
            ActorExtraData data;

            public int Size {
                get { return size; }
            }
            public string EntityType {
                get { return entityType; }
                set { entityType = value; }
            }
            public string ActorTypeName {
                get { return actorTypeName; }
                set { actorTypeName = value; }
            }
            public string UnkString {
                get { return unkString; }
                set { unkString = value; }
            }
            public string Unk2String {
                get { return unk2String; }
                set { unk2String = value; }
            }
            public string DefinitionName {
                get { return definitionName; }
                set { definitionName = value; }
            }
            public string FrameName {
                get { return frameName; }
                set { frameName = value; }
            }
            public int ActorTypeID {
                get { return actortypeID; }
                set { actortypeID = value; }
            }
            public ulong DefinitionHash {
                get { return defintionHash; }
                set { defintionHash = value; }
            }
            public ulong FrameNameHash {
                get { return frameNameHash; }
                set { frameNameHash = value; }
            }
            public Vector3 Position {
                get { return position; }
                set { position = value; }
            }
            public Vector4 Quaternion {
                get { return quat; }
                set { quat = value; }
            }
            public Vector3 Rotation {
                get { return euler; }
                set { euler = value; }
            }
            public Vector3 Scale {
                get { return scale; }
                set { scale = value; }
            }
            public ushort Unk3 {
                get { return unk3; }
                set { unk3 = value; }
            }
            public ushort DataID {
                get { return dataID; }
                set { dataID = value; }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public ActorExtraData Data {
                get { return data; }
                set { data = value; }
            }

            public ActorItem(BinaryReader reader)
            {
                ReadFromFile(reader);
                ConvertQuaternion();
            }

            private void ConvertQuaternion()
            {
                euler = new Vector3();
                var qw = quat.W;
                var qx = quat.X;
                var qy = quat.Y;
                var qz = quat.Z;
                var eX = Math.Atan2(-2 * ((qy * qz) - (qw * qx)), (qw * qw) - (qx * qx) - (qy * qy) + (qz * qz));
                var eY = Math.Asin(2 * ((qx * qz) + (qw * qy)));
                var eZ = Math.Atan2(-2 * ((qx * qy) - (qw * qz)), (qw * qw) + (qx * qx) - (qy * qy) - (qz * qz));
                euler.Z = (float)Math.Round(eZ * 180 / Math.PI);
                euler.Y = (float)Math.Round(eY * 180 / Math.PI);
                euler.X = (float)Math.Round(eX * 180 / Math.PI);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                size = reader.ReadInt32();
                actorTypeName = readString(reader);
                entityType = readString(reader);
                unkString = readString(reader);
                unk2String = readString(reader);
                definitionName = readString(reader);
                frameName = readString(reader);
                actortypeID = reader.ReadInt32();
                defintionHash = reader.ReadUInt64();
                frameNameHash = reader.ReadUInt64();
                position = Vector3Extenders.ReadFromFile(reader);
                quat = Vector4Extenders.ReadFromFile(reader);
                scale = Vector3Extenders.ReadFromFile(reader);
                unk3 = reader.ReadUInt16();
                dataID = reader.ReadUInt16();
            }

            public int CalculateSize()
            {
                size = 4;
                size += (actorTypeName.Length + 2);
                size += (entityType.Length + 2);
                size += (unkString.Length + 2);
                size += (unk2String.Length + 2);
                size += (definitionName.Length + 2);
                size += (frameName.Length + 2);
                size += 64;
                return size;
            }

            public void WriteToFile(BinaryWriter writer)
            {
                long pos = writer.BaseStream.Position;
                writer.Write(0);
                writeString(actorTypeName, writer);
                writeString(entityType, writer);
                writeString(unkString, writer);
                writeString(unk2String, writer);
                writeString(definitionName, writer);
                writeString(frameName, writer);
                writer.Write(actortypeID);
                writer.Write(defintionHash);
                writer.Write(frameNameHash);
                position.WriteToFile(writer);
                quat.WriteToFile(writer);
                scale.WriteToFile(writer);
                writer.Write(unk3);
                writer.Write(dataID);
                long pos2 = writer.BaseStream.Position;
                writer.BaseStream.Position = pos;
                writer.Write(size);
                writer.BaseStream.Position = pos2;
            }

            private string readString(BinaryReader reader)
            {
                byte length = reader.ReadByte();
                string text = "";
                //if(length > 30)
                //{
                //    reader.BaseStream.Position--;
                //    text = StringHelpers.ReadString(reader);
                //}
                //else if(length > 0)
                //{
                    text = new string(reader.ReadChars(length - 2));
                    reader.ReadByte();
                //}
                return text;
            }

            private void writeString(string text, BinaryWriter writer)
            {
                writer.Write((byte)(text.Length + 2));
                writer.Write(text.ToCharArray());
                writer.Write((byte)0);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}, {3}", entityType, actortypeID, actorTypeName, dataID);
            }
        }
    }

    public struct ActorExtraData
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

        public ActorExtraData(BinaryReader reader)
        {
            buffer = null;
            data = null;
            bufferType = 0;
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            buffer = null;
            data = null;

            bufferType = (ActorTypes)reader.ReadInt32();

            uint bufferLength = reader.ReadUInt32();
            buffer = reader.ReadBytes((int)bufferLength);
            bool parsed = false;
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                bool isBigEndian = false; //we'll change this once console parsing is complete.
                if (bufferType == ActorTypes.Pinup && bufferLength == 4)
                {
                    data = new ActorPinup(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.ScriptEntity && bufferLength == 100)
                {
                    data = new ActorScriptEntity(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.Radio && bufferLength == 1028)
                {
                    data = new ActorRadio(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.Airplane && bufferLength == 4)
                {
                    data = new ActorAircraft(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.SpikeStrip && bufferLength == 4)
                {
                    data = new ActorSpikeStrip(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.Door && bufferLength == 364)
                {
                    data = new ActorDoor(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.Wardrobe && bufferLength == 208)
                {
                    data = new ActorWardrobe(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.TrafficTrain && bufferLength == 180)
                {
                    data = new ActorTrafficTrain(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.TrafficHuman && bufferLength == 160)
                {
                    data = new ActorTrafficHuman(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.TrafficCar && bufferLength == 220)
                {
                    data = new ActorTrafficCar(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.LightEntity && bufferLength == 2316)
                {
                    data = new ActorLight(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.Item && bufferLength == 152)
                {
                    data = new ActorItem(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.Sound && bufferLength == 592)
                {
                    data = new ActorSoundEntity(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.CleanEntity && bufferLength == 20)
                {
                    data = new ActorCleanEntity(stream, isBigEndian);
                    parsed = true;
                }

                Debug.Assert(bufferLength == stream.Length);
            }

            if (parsed)
                buffer = null;
        }
        public void WriteToFile(BinaryWriter writer)
        {
            bool isBigEndian = false;
            writer.Write((int)bufferType);

            if (buffer != null)
            {
                writer.Write(buffer.Length);
                writer.Write(buffer);
            }
            else
            {
                writer.Write(data.GetSize());

                using (MemoryStream stream = new MemoryStream())
                {
                    data.WriteToFile(stream, isBigEndian);
                    Debug.Assert(data.GetSize() == stream.Length);
                    stream.WriteTo(writer.BaseStream);
                }
            }
        }
        public byte[] GetDataInBytes()
        {
            if (data == null)
            {
                return buffer;
            }
            else
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    data.WriteToFile(stream, false);
                    return stream.ToArray();
                }
            }
        }

        public override string ToString()
        {
            if (buffer != null)
                return string.Format("{0}, {1}", bufferType, buffer.Length);
            else
                return string.Format("{0}", bufferType);
        }
    }
}
