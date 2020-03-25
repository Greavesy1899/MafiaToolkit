using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace ResourceTypes.Actors
{
    public class ActorExtraData
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

        public ActorExtraData()
        {

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
                if (bufferType == ActorTypes.C_Pinup && bufferLength == 4)
                {
                    data = new ActorPinup(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.C_ActorDetector && bufferLength == 20)
                {
                    data = new ActorActorDetector(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.C_ScriptEntity && bufferLength == 100)
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
                else if (bufferType == ActorTypes.C_Door && bufferLength == 364)
                {
                    data = new ActorDoor(stream, isBigEndian);
                    parsed = true;
                }
                else if(bufferType == ActorTypes.FrameWrapper && bufferLength == 4)
                {
                    data = new ActorFrameWrapper(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.Wardrobe && bufferLength == 208)
                {
                    data = new ActorWardrobe(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.C_TrafficTrain && bufferLength == 180)
                {
                    data = new ActorTrafficTrain(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.C_TrafficHuman && bufferLength == 160)
                {
                    data = new ActorTrafficHuman(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.C_TrafficCar && bufferLength == 220)
                {
                    data = new ActorTrafficCar(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.C_StaticParticle && bufferLength == 16)
                {
                    data = new ActorStaticParticle(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.StaticEntity && bufferLength == 4)
                {
                    data = new ActorStaticEntity(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.LightEntity && bufferLength == 2316)
                {
                    data = new ActorLight(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.C_Item && bufferLength == 152)
                {
                    data = new ActorItem(stream, isBigEndian);
                    parsed = true;
                }
                else if (bufferType == ActorTypes.C_Sound && bufferLength == 592)
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
