using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Utils.Extensions;
using Utils.Logging;

namespace ResourceTypes.Actors
{
    public class ActorExtraData
    {
        ActorTypes bufferType;
        IActorExtraDataInterface data;
        byte[] buffer;

        public ActorTypes BufferType
        {
            get { return bufferType; }
            set { bufferType = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IActorExtraDataInterface Data
        {
            get { return data; }
            set { data = value; }
        }

        public ActorExtraData()
        {
        }

        public ActorExtraData(BinaryReader reader, bool compressed)
        {
            buffer = null;
            data = null;
            bufferType = 0;
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
            buffer = null;
            data = null;

            bufferType = (ActorTypes)reader.ReadInt32();

            uint bufferLength = reader.ReadUInt32();
            buffer = reader.ReadBytes((int)bufferLength);
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                bool isBigEndian = false; //we'll change this once console parsing is complete.
                data = ActorFactory.LoadExtraData(bufferType, stream, isBigEndian);
                ToolkitAssert.Ensure(bufferLength == stream.Length,
                    string.Format("We did not reach the end of this stream! BufferType {0}", bufferType));
            }

            buffer = (data == null ? buffer : null);
        }

        public void ReadFromUncompressedFile(BinaryReader reader)
        {
            buffer = null;
            data = null;

            bufferType = (ActorTypes)reader.ReadInt32(); //string will determine id, also this doesnt determine buffertype?

            uint bufferLength = 15; //todo fix the code for uncompressed if broken anywhere, is always 15?
            buffer = reader.ReadBytes(15);
            MemoryStream memoryStream = new MemoryStream(buffer);
            string check = memoryStream.ReadStringBuffer(15);
            if (check.Equals("FrameWrapper")) //didn't encounter any other type in uncompressed yet
            {
                bufferType = (ActorTypes)55;
            }

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                bool isBigEndian = false; //we'll change this once console parsing is complete.
                data = ActorFactory.LoadExtraData(bufferType, stream, isBigEndian);
                ToolkitAssert.Ensure(bufferLength == stream.Length,
                    string.Format("We did not reach the end of this stream! BufferType {0}", bufferType));
            }

            buffer = (data == null ? buffer : null);
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
                    ToolkitAssert.Ensure(data.GetSize() == stream.Length,
                        string.Format("We did not reach the end of this stream! BufferType {0}", bufferType));
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