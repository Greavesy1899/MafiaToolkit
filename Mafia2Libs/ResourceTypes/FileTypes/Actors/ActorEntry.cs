using Gibbed.Illusion.FileFormats.Hashing;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Utils.StringHelpers;
using Utils.VorticeUtils;

namespace ResourceTypes.Actors
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ActorEntry
    {
        int size; //item size in bytes;
        string actorTypeName; //actor type (string)
        string entityName; //entity name
        string unkString; //Name1
        string unk2String; //SceneSectorName
        string definitionName; //actor name
        string frameName; //frame name
        int actortypeID;
        ulong entityHash; //definition hash
        ulong frameNameHash; //frame name hash
        Vector3 position;
        Quaternion rotation;
        Vector3 scale;
        bool ActivateOnInit;
        short dataID;
        ActorExtraData data;

        public int Size
        {
            get { return size; }
        }

        public string EntityName
        {
            get { return entityName; }
            set
            {
                entityName = value;
                entityHash = FNV64.Hash(value);
            }
        }

        public string ActorTypeName
        {
            get { return actorTypeName; }
            set { actorTypeName = value; }
        }

        public string Name1
        {
            get { return unkString; }
            set { unkString = value; }
        }

        public string SceneSectorName
        {
            get { return unk2String; }
            set { unk2String = value; }
        }

        public string DefinitionName
        {
            get { return definitionName; }
            set { definitionName = value; }
        }

        public string FrameName
        {
            get { return frameName; }
            set
            {
                frameName = value;
                frameNameHash = FNV64.Hash(value);
            }
        }

        public int ActorTypeID
        {
            get { return actortypeID; }
            set { actortypeID = value; }
        }

        public ulong EntityHash
        {
            get { return entityHash; }
            set { entityHash = value; }
        }

        public ulong FrameNameHash
        {
            get { return frameNameHash; }
            set { frameNameHash = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Quaternion Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public bool bActivateOnInit
        {
            get { return ActivateOnInit; }
            set { ActivateOnInit = value; }
        }

        public short DataID
        {
            get { return dataID; }
            set { dataID = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ActorExtraData Data
        {
            get { return data; }
            set { data = value; }
        }


        public ActorEntry()
        {
            rotation = Quaternion.Identity;
            scale = Vector3.One;
            actorTypeName = "";
            entityName = "";
            unkString = "";
            unk2String = "";
            frameName = "";
            definitionName = "";
        }

        public ActorEntry(BinaryReader reader, bool compressed)
        {
            if (compressed)
            {
                ReadFromFile(reader);
            }
            else
            {
                ReadFromUncompressedFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            size = reader.ReadInt32();
            actorTypeName = readString(reader);
            entityName = readString(reader);
            unkString = readString(reader);
            unk2String = readString(reader);
            definitionName = readString(reader);
            frameName = readString(reader);
            actortypeID = reader.ReadInt32();
            entityHash = reader.ReadUInt64();
            frameNameHash = reader.ReadUInt64();
            position = Vector3Utils.ReadFromFile(reader);
            rotation = QuaternionExtensions.ReadFromFile(reader);
            scale = Vector3Utils.ReadFromFile(reader);
            ActivateOnInit = Convert.ToBoolean(reader.ReadUInt16());
            dataID = reader.ReadInt16();
        }

        public void ReadFromUncompressedFile(BinaryReader reader)
        {
            long initPos = reader.BaseStream.Position;
            size = reader.ReadInt32();
            byte[] actorBytes = reader.ReadBytes((int)size);

            using (MemoryStream memoryStream = new MemoryStream(actorBytes)) //poor way of reading and parsing strings
            {
                string[] parts = new string[6]; // 6 should be maximum number of whole strings
                int partIndex = 0;

                while (partIndex < parts.Length)
                {
                    // reading array until it hits zero byte for splitting
                    List<byte> byteList = new List<byte>();
                    int b;
                    while ((b = memoryStream.ReadByte()) != 0 && b != -1)
                    {
                        byteList.Add((byte)b);
                    }

                    if (byteList.Count == 0 && b == -1)
                    {
                        // end of stream
                        break;
                    }

                    parts[partIndex] = Encoding.UTF8.GetString(byteList.ToArray());
                    partIndex++;
                }

                actorTypeName = parts.Length > 0 ? parts[0] : "";
                entityName = parts.Length > 1 ? parts[1] : "";
                unkString = parts.Length > 2 ? parts[2] : "";
                unk2String = parts.Length > 3 ? parts[3] : "";
                definitionName = parts.Length > 4 ? parts[4] : "";
                frameName = parts.Length > 5 ? parts[5] : "";
                // sets reader position to the end of string array

                reader.BaseStream.Seek(initPos + memoryStream.Position + 4, SeekOrigin.Begin); //+4 because reading size
            }

            position = Vector3Utils.ReadFromFile(reader);
            rotation = QuaternionExtensions.ReadFromFile(reader);
            scale = Vector3Utils.ReadFromFile(reader);
            ActivateOnInit = Convert.ToBoolean(reader.ReadUInt16());
            dataID = reader.ReadInt16();

            if (actorTypeName.Equals("FrameWrapper")) //didn't encounter any other type in uncompressed yet
            {
                actortypeID = 55;
            }
            //entityHash = reader.ReadUInt64();
            //isnt in uncompressed?

            //frameNameHash = reader.ReadUInt64();
            //TODO load matching framehash as in definition?

            reader.BaseStream.Seek(initPos + size, SeekOrigin.Begin);//moving onto next item or the end of stream
        }

        public int CalculateSize()
        {
            size = 4;
            size += (actorTypeName.Length + 2);
            size += (entityName.Length + 2);
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
            writeString(entityName, writer);
            writeString(unkString, writer);
            writeString(unk2String, writer);
            writeString(definitionName, writer);
            writeString(frameName, writer);
            writer.Write(actortypeID);
            writer.Write(entityHash);
            writer.Write(frameNameHash);
            position.WriteToFile(writer);
            rotation.WriteToFile(writer);
            scale.WriteToFile(writer);
            writer.Write(Convert.ToUInt16(ActivateOnInit));
            writer.Write(dataID);
            long pos2 = writer.BaseStream.Position;
            writer.BaseStream.Position = pos;
            writer.Write(size);
            writer.BaseStream.Position = pos2;
        }

        private string readString(BinaryReader reader)
        {
            byte length = reader.ReadByte();
            string text = StringHelpers.ReadStringBuffer(reader, length - 2);
            reader.ReadByte();
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
            return string.Format("{0}, {1}, {2}, {3}", entityName, actortypeID, actorTypeName, dataID);
        }
    }
}