using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.Diagnostics;
using Utils;
using ResourceTypes.Wwise.Helpers;
using ResourceTypes.Wwise.Objects;

namespace ResourceTypes.Wwise
{
    public class BNK
    {
        public byte[] BKHDTypeString = { (byte)'B', (byte)'K', (byte)'H', (byte)'D' };
        public byte[] DIDXTypeString = { (byte)'D', (byte)'I', (byte)'D', (byte)'X' };
        public byte[] DATATypeString = { (byte)'D', (byte)'A', (byte)'T', (byte)'A' };
        public byte[] HIRCTypeString = { (byte)'H', (byte)'I', (byte)'R', (byte)'C' };
        public BKHD Header { get; set; }
        public DIDX Wems;
        public HIRC Objects;
        public List<Wem> WemList = new List<Wem>();
        public Dictionary<uint, byte[]> ExtraData { get; set; }

        public BNK(string fileName)
        {
            using (BinaryReader br = new BinaryReader(new FileStream(fileName, FileMode.Open), Encoding.ASCII))
            {
                int Magic = br.ReadInt32();
                Header = new BKHD(br);
                int BKHDDataCount = (Header.Length / 4) - 5;

                for (int i = 0; i < BKHDDataCount; i++)
                {
                    Header.Data.Add(br.ReadInt32());
                }

                Header.Offset = br.BaseStream.Position;
                ExtraData = new Dictionary<uint, byte[]>();

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    uint typeString = br.ReadUInt32();

                    switch (typeString)
                    {
                        case 1480870212:
                            ReadDIDX(br);
                            ExtraData.Add(typeString, new byte[0]);
                            break;

                        case 1129466184:
                            ReadHIRC(br);
                            ExtraData.Add(typeString, new byte[0]);
                            break;

                        default:
                            int sectionLength = br.ReadInt32();
                            byte[] data = br.ReadBytes(sectionLength);
                            ExtraData.Add(typeString, data);
                            break;
                    }
                }

                if (WemList != null && Objects != null)
                {
                    foreach (Wem wem in WemList)
                    {
                        if (Objects.SoundSFX.ContainsKey((uint)wem.ID))
                        {
                            wem.AssignedHirc.SoundSFX = Objects.SoundSFX[(uint)wem.ID];
                        }

                        if (Objects.MusicTrack.ContainsKey((int)wem.ID))
                        {
                            wem.AssignedHirc.MusicTrack = Objects.MusicTrack[(int)wem.ID];
                        }
                    }
                }
            }
        }

        public BNK()
        {
            Header = new BKHD();
            ExtraData = new Dictionary<uint, byte[]>();
        }

        public void ReadDIDX(BinaryReader br)
        {
            uint headerLen = br.ReadUInt32();
            Wems = new DIDX(headerLen);
            uint wemCount = headerLen / 12;
            for (int i = 0; i < wemCount; i++)
            {
                uint DIDXId = br.ReadUInt32();
                uint DIDXOffset = br.ReadUInt32();
                uint DIDXLength = br.ReadUInt32();
                Wems.data.Add(new DIDXChunk(DIDXId, DIDXOffset, DIDXLength));
            }
            Wems.offset = br.BaseStream.Position;

            uint DataStartString = br.ReadUInt32();

            if (DataStartString != 1096040772)
            {
                throw new Exception("Failed to read DATA!");
            }

            uint DataLength = br.ReadUInt32();
            long initPos = br.BaseStream.Position;

            uint ii = 0;
            foreach (DIDXChunk chunk in Wems.data)
            {
                ii++;
                br.BaseStream.Seek(initPos + chunk.chunkOffset, SeekOrigin.Begin);
                byte[] file = br.ReadBytes((int)chunk.chunkLength);
                string name = "Imported_Wem_" + ii;

                Wem newWem = new Wem(name, chunk.chunkId, file, 0, chunk.chunkOffset);
                WemList.Add(newWem);
            }
        }

        public void WriteDIDX(BinaryWriter bw)
        {
            WemList = new List<Wem>(WemList.OrderBy(i => i.ID));

            bw.Write(DIDXTypeString);
            bw.Write(WemList.Count * 12);

            int dataLength = 0;

            int offset = (int)bw.BaseStream.Position + (WemList.Count * 12);

            for (int i = 0; i < WemList.Count; i++)
            {
                Wem wem = WemList[i];

                if (i == 0)
                {
                    wem.Offset = 0;
                }
                else
                {
                    Wem prevWem = WemList[i - 1];

                    wem.Offset = (uint)(prevWem.Offset + prevWem.file.Length);
                }

                bw.Write((uint)wem.ID);
                bw.Write(wem.Offset);
                bw.Write(wem.file.Length);
                long workingOffset = bw.BaseStream.Position;
                bw.Seek((int)wem.Offset + offset + 8, SeekOrigin.Begin);
                bw.Write(wem.file);
                bw.Seek((int)workingOffset, SeekOrigin.Begin);
                dataLength += wem.file.Length;
            }

            bw.Write(DATATypeString);
            bw.Write(dataLength);

            Wem lastWem = WemList[WemList.Count - 1];
            bw.Seek((int)lastWem.Offset + (int)lastWem.file.Length + offset + 8, SeekOrigin.Begin);
        }

        public void ReadHIRC(BinaryReader br)
        {
            int HIRCLength = br.ReadInt32();
            int HIRCPos = (int)br.BaseStream.Position;
            uint HIRCCount = br.ReadUInt32();
            Objects = new HIRC(HIRCLength);

            for (int i = 0; i < HIRCCount; i++)
            {
                HIRCObject tempObj = new HIRCObject(this, br);
                Objects.Data.Add(tempObj);

                if (tempObj.SettingsObject != null)
                {
                    if (!Objects.Settings.ContainsKey((int)tempObj.SettingsObject.id))
                    {
                        Objects.Settings.Add((int)tempObj.SettingsObject.id, new List<int>());
                    }

                    Objects.Settings[(int)tempObj.SettingsObject.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.SoundSFXObject != null)
                {
                    if (!Objects.SoundSFX.ContainsKey(tempObj.SoundSFXObject.sourceId))
                    {
                        Objects.SoundSFX.Add(tempObj.SoundSFXObject.sourceId, new List<int>());
                    }

                    if (!Objects.SoundSFX[tempObj.SoundSFXObject.sourceId].Contains(Objects.Data.IndexOf(tempObj)))
                    {
                        Objects.SoundSFX[tempObj.SoundSFXObject.sourceId].Add(Objects.Data.IndexOf(tempObj));
                    }
                }
                else if (tempObj.EventAction != null)
                {
                    if (!Objects.EventAction.ContainsKey((int)tempObj.EventAction.objectID))
                    {
                        Objects.EventAction.Add((int)tempObj.EventAction.objectID, new List<int>());
                    }

                    Objects.EventAction[(int)tempObj.EventAction.objectID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.Event != null)
                {
                    foreach (int id in tempObj.Event.actionIDs)
                    {
                        if (!Objects.Event.ContainsKey(id))
                        {
                            Objects.Event.Add(id, new List<int>());
                        }

                        Objects.Event[id].Add(Objects.Data.IndexOf(tempObj));
                    }
                }
                else if (tempObj.RandomContainer != null)
                {
                    if (!Objects.RandomContainer.ContainsKey((int)tempObj.RandomContainer.id))
                    {
                        Objects.RandomContainer.Add((int)tempObj.RandomContainer.id, new List<int>());
                    }

                    Objects.RandomContainer[(int)tempObj.RandomContainer.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.SwitchContainer != null)
                {
                    if (!Objects.SwitchContainer.ContainsKey((int)tempObj.SwitchContainer.id))
                    {
                        Objects.SwitchContainer.Add((int)tempObj.SwitchContainer.id, new List<int>());
                    }

                    Objects.SwitchContainer[(int)tempObj.SwitchContainer.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.ActorMixer != null)
                {
                    if (!Objects.ActorMixer.ContainsKey((int)tempObj.ActorMixer.id))
                    {
                        Objects.ActorMixer.Add((int)tempObj.ActorMixer.id, new List<int>());
                    }

                    Objects.ActorMixer[(int)tempObj.ActorMixer.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.AudioBus != null)
                {
                    if (!Objects.AudioBus.ContainsKey((int)tempObj.AudioBus.id))
                    {
                        Objects.AudioBus.Add((int)tempObj.AudioBus.id, new List<int>());
                    }

                    Objects.AudioBus[(int)tempObj.AudioBus.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.BlendContainer != null)
                {
                    if (!Objects.BlendContainer.ContainsKey((int)tempObj.BlendContainer.id))
                    {
                        Objects.BlendContainer.Add((int)tempObj.BlendContainer.id, new List<int>());
                    }

                    Objects.BlendContainer[(int)tempObj.BlendContainer.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.MusicSegment != null)
                {
                    foreach (uint child in tempObj.MusicSegment.childIDs)
                    {
                        if (!Objects.MusicSegment.ContainsKey((int)child))
                        {
                            Objects.MusicSegment.Add((int)child, new List<int>());
                        }

                        Objects.MusicSegment[(int)child].Add(Objects.Data.IndexOf(tempObj));
                    }
                }
                else if (tempObj.MusicTrack != null)
                {
                    foreach (TrackSource src in tempObj.MusicTrack.trackSources)
                    {
                        if (!Objects.MusicTrack.ContainsKey((int)src.sourceId))
                        {
                            Objects.MusicTrack.Add((int)src.sourceId, new List<int>());
                        }

                        Objects.MusicTrack[(int)src.sourceId].Add(Objects.Data.IndexOf(tempObj));
                    }
                }
                else if (tempObj.MusicSwitchContainer != null)
                {
                    if (!Objects.MusicSwitchContainer.ContainsKey((int)tempObj.MusicSwitchContainer.id))
                    {
                        Objects.MusicSwitchContainer.Add((int)tempObj.MusicSwitchContainer.id, new List<int>());
                    }

                    Objects.MusicSwitchContainer[(int)tempObj.MusicSwitchContainer.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.MusicSequence != null)
                {
                    if (!Objects.MusicSequence.ContainsKey((int)tempObj.MusicSequence.id))
                    {
                        Objects.MusicSequence.Add((int)tempObj.MusicSequence.id, new List<int>());
                    }

                    Objects.MusicSequence[(int)tempObj.MusicSequence.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.Attenuation != null)
                {
                    if (!Objects.Attenuation.ContainsKey((int)tempObj.Attenuation.id))
                    {
                        Objects.Attenuation.Add((int)tempObj.Attenuation.id, new List<int>());
                    }

                    Objects.Attenuation[(int)tempObj.Attenuation.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.FeedbackNode != null)
                {
                    if (!Objects.FeedbackNode.ContainsKey((int)tempObj.FeedbackNode.id))
                    {
                        Objects.FeedbackNode.Add((int)tempObj.FeedbackNode.id, new List<int>());
                    }

                    Objects.FeedbackNode[(int)tempObj.FeedbackNode.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.FxShareSet != null)
                {
                    if (!Objects.FxShareSet.ContainsKey((int)tempObj.FxShareSet.id))
                    {
                        Objects.FxShareSet.Add((int)tempObj.FxShareSet.id, new List<int>());
                    }

                    Objects.FxShareSet[(int)tempObj.FxShareSet.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.FxCustom != null)
                {
                    if (!Objects.FxCustom.ContainsKey((int)tempObj.FxCustom.id))
                    {
                        Objects.FxCustom.Add((int)tempObj.FxCustom.id, new List<int>());
                    }

                    Objects.FxCustom[(int)tempObj.FxCustom.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.AuxiliaryBus != null)
                {
                    if (!Objects.AuxiliaryBus.ContainsKey((int)tempObj.AuxiliaryBus.id))
                    {
                        Objects.AuxiliaryBus.Add((int)tempObj.AuxiliaryBus.id, new List<int>());
                    }

                    Objects.AuxiliaryBus[(int)tempObj.AuxiliaryBus.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.LFO != null)
                {
                    if (!Objects.LFO.ContainsKey((int)tempObj.LFO.id))
                    {
                        Objects.LFO.Add((int)tempObj.LFO.id, new List<int>());
                    }

                    Objects.LFO[(int)tempObj.LFO.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.Envelope != null)
                {
                    if (!Objects.Envelope.ContainsKey((int)tempObj.Envelope.id))
                    {
                        Objects.Envelope.Add((int)tempObj.Envelope.id, new List<int>());
                    }

                    Objects.Envelope[(int)tempObj.Envelope.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.State != null)
                {
                    if (!Objects.State.ContainsKey(tempObj.State.id))
                    {
                        Objects.State.Add(tempObj.State.id, new List<int>());
                    }

                    Objects.State[tempObj.State.id].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.AudioDevice != null)
                {
                    if (!Objects.AudioDevice.ContainsKey(tempObj.AudioDevice.id))
                    {
                        Objects.AudioDevice.Add(tempObj.AudioDevice.id, new List<int>());
                    }

                    Objects.AudioDevice[tempObj.AudioDevice.id].Add(Objects.Data.IndexOf(tempObj));
                }
            }
        }

        public void WriteHIRC(BinaryWriter bw)
        {
            long initPos = bw.BaseStream.Position + 12;
            bw.Write(HIRCTypeString);
            bw.Write(Objects.GetLength());
            bw.Write(Objects.Data.Count);
            bw.Seek((int)initPos, SeekOrigin.Begin);
            foreach (HIRCObject obj in Objects.Data)
            {
                int objLength = obj.GetLength();
                    long tempPos = bw.BaseStream.Position;
                if (obj.SettingsObject != null)
                {
                    obj.SettingsObject.WriteToFile(bw);
                }
                else if (obj.SoundSFXObject != null)
                {
                    obj.SoundSFXObject.WriteToFile(bw);
                }
                else if (obj.EventAction != null)
                {
                    obj.EventAction.WriteToFile(bw);
                }
                else if (obj.Event != null)
                {
                    obj.Event.WriteToFile(bw);
                }
                else if (obj.RandomContainer != null)
                {
                    obj.RandomContainer.WriteToFile(bw);
                }
                else if (obj.SwitchContainer != null)
                {
                    obj.SwitchContainer.WriteToFile(bw);
                }
                else if (obj.ActorMixer != null)
                {
                    obj.ActorMixer.WriteToFile(bw);
                }
                else if (obj.AudioBus != null)
                {
                    obj.AudioBus.WriteToFile(bw);
                }
                else if (obj.BlendContainer != null)
                {
                    obj.BlendContainer.WriteToFile(bw);
                }
                else if (obj.MusicSegment != null)
                {
                    obj.MusicSegment.WriteToFile(bw);
                }
                else if (obj.MusicTrack != null)
                {
                    obj.MusicTrack.WriteToFile(bw);
                }
                else if (obj.MusicSwitchContainer != null)
                {
                    obj.MusicSwitchContainer.WriteToFile(bw);
                }
                else if (obj.MusicSequence != null)
                {
                    obj.MusicSequence.WriteToFile(bw);
                }
                else if (obj.Attenuation != null)
                {
                    obj.Attenuation.WriteToFile(bw);
                }
                else if (obj.FeedbackNode != null)
                {
                    obj.FeedbackNode.WriteToFile(bw);
                }
                else if (obj.FxShareSet != null)
                {
                    obj.FxShareSet.WriteToFile(bw);
                }
                else if (obj.FxCustom != null)
                {
                    obj.FxCustom.WriteToFile(bw);
                }
                else if (obj.AuxiliaryBus != null)
                {
                    obj.AuxiliaryBus.WriteToFile(bw);
                }
                else if (obj.LFO != null)
                {
                    obj.LFO.WriteToFile(bw);
                }
                else if (obj.Envelope != null)
                {
                    obj.Envelope.WriteToFile(bw);
                }
                else if (obj.State != null)
                {
                    obj.State.WriteToFile(bw);
                }
                else if (obj.AudioDevice != null)
                {
                    obj.AudioDevice.WriteToFile(bw);
                }
                else
                {
                    bw.Write(obj.data);
                }
            }
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(BKHDTypeString);
            bw.Write(20 + Header.Data.Count * 4);
            bw.Write(Header.Version);
            bw.Write(Header.ID);
            bw.Write(Header.LangID);
            bw.Write(Header.Feedback);
            bw.Write(Header.ProjectID);

            foreach (uint dataPart in Header.Data)
            {
                bw.Write(dataPart);
            }

            foreach (var data in ExtraData)
            {
                if (data.Key == 1129466184)
                {
                    WriteHIRC(bw);
                }
                else if (data.Key == 1480870212)
                {
                    WriteDIDX(bw);
                }
                else
                {
                    bw.Write(data.Key);
                    bw.Write(data.Value.Length);
                    bw.Write(data.Value);
                }
            }
        }

        public uint GetLength()
        {
            uint length = 28 + (uint)(Header.Data.Count * 4);

            if (ExtraData.ContainsKey(1480870212))
            {
                length += (uint)WemList.Count * 12 + 8;

                foreach (Wem wem in WemList)
                {
                    length += (uint)wem.file.Length;
                }
            }

            if (ExtraData.ContainsKey(1129466184))
            {
                length += (uint)Objects.GetLength();
            }

            foreach (var data in ExtraData)
            {
                length += 8 + (uint)data.Value.Length;
            }

            return length;
        }
    }
}
