using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise
{
    public class BNK
    {
        public byte[] BKHDTypeString = { (byte)'B', (byte)'K', (byte)'H', (byte)'D' };
        public byte[] DidXTypeString = { (byte)'D', (byte)'I', (byte)'D', (byte)'X' };
        public byte[] DATATypeString = { (byte)'D', (byte)'A', (byte)'T', (byte)'A' };
        public byte[] HIRCTypeString = { (byte)'H', (byte)'I', (byte)'R', (byte)'C' };
        public BKHD Header { get; set; }
        public DidX Wems;
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
                    uint TypeString = br.ReadUInt32();

                    switch (TypeString)
                    {
                        case 1480870212:
                            ReadDidX(br);
                            ExtraData.Add(TypeString, new byte[0]);
                            break;

                        case 1129466184:
                            ReadHIRC(br);
                            ExtraData.Add(TypeString, new byte[0]);
                            break;

                        default:
                            int sectionLength = br.ReadInt32();
                            byte[] Data = br.ReadBytes(sectionLength);
                            ExtraData.Add(TypeString, Data);
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

        public void ReadDidX(BinaryReader br)
        {
            uint headerLen = br.ReadUInt32();
            Wems = new DidX(headerLen);
            uint wemCount = headerLen / 12;
            for (int i = 0; i < wemCount; i++)
            {
                uint DidXID = br.ReadUInt32();
                uint DidXOffset = br.ReadUInt32();
                uint DidXLength = br.ReadUInt32();
                Wems.Data.Add(new DIDXChunk(DidXID, DidXOffset, DidXLength));
            }
            Wems.Offset = br.BaseStream.Position;

            uint DataStartString = br.ReadUInt32();

            if (DataStartString != 1096040772)
            {
                throw new Exception("Failed to read DATA!");
            }

            uint DataLength = br.ReadUInt32();
            long initPos = br.BaseStream.Position;

            uint ii = 0;
            foreach (DIDXChunk chunk in Wems.Data)
            {
                ii++;
                br.BaseStream.Seek(initPos + chunk.ChunkOffset, SeekOrigin.Begin);
                byte[] file = br.ReadBytes((int)chunk.ChunkLength);
                string name = "Imported_Wem_" + ii;

                Wem newWem = new Wem(name, chunk.ChunkID, file, 0, chunk.ChunkOffset);
                WemList.Add(newWem);
            }
        }

        public void WriteDidX(BinaryWriter bw)
        {
            WemList = new List<Wem>(WemList.OrderBy(i => i.ID));

            bw.Write(DidXTypeString);
            bw.Write(WemList.Count * 12);

            int DataLength = 0;

            int Offset = (int)bw.BaseStream.Position + (WemList.Count * 12);

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

                    wem.Offset = (uint)(prevWem.Offset + prevWem.File.Length);
                }

                bw.Write((uint)wem.ID);
                bw.Write(wem.Offset);
                bw.Write(wem.File.Length);
                long workingOffset = bw.BaseStream.Position;
                bw.Seek((int)wem.Offset + Offset + 8, SeekOrigin.Begin);
                bw.Write(wem.File);
                bw.Seek((int)workingOffset, SeekOrigin.Begin);
                DataLength += wem.File.Length;
            }

            bw.Write(DATATypeString);
            bw.Write(DataLength);

            Wem lastWem = WemList[WemList.Count - 1];
            bw.Seek((int)lastWem.Offset + (int)lastWem.File.Length + Offset + 8, SeekOrigin.Begin);
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
                    if (!Objects.Settings.ContainsKey((int)tempObj.SettingsObject.ID))
                    {
                        Objects.Settings.Add((int)tempObj.SettingsObject.ID, new List<int>());
                    }

                    Objects.Settings[(int)tempObj.SettingsObject.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.SoundSFXObject != null)
                {
                    if (!Objects.SoundSFX.ContainsKey(tempObj.SoundSFXObject.SourceID))
                    {
                        Objects.SoundSFX.Add(tempObj.SoundSFXObject.SourceID, new List<int>());
                    }

                    if (!Objects.SoundSFX[tempObj.SoundSFXObject.SourceID].Contains(Objects.Data.IndexOf(tempObj)))
                    {
                        Objects.SoundSFX[tempObj.SoundSFXObject.SourceID].Add(Objects.Data.IndexOf(tempObj));
                    }
                }
                else if (tempObj.EventAction != null)
                {
                    if (!Objects.EventAction.ContainsKey((int)tempObj.EventAction.ObjectID))
                    {
                        Objects.EventAction.Add((int)tempObj.EventAction.ObjectID, new List<int>());
                    }

                    Objects.EventAction[(int)tempObj.EventAction.ObjectID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.Event != null)
                {
                    foreach (int ID in tempObj.Event.ActionIDs)
                    {
                        if (!Objects.Event.ContainsKey(ID))
                        {
                            Objects.Event.Add(ID, new List<int>());
                        }

                        Objects.Event[ID].Add(Objects.Data.IndexOf(tempObj));
                    }
                }
                else if (tempObj.RandomContainer != null)
                {
                    if (!Objects.RandomContainer.ContainsKey((int)tempObj.RandomContainer.ID))
                    {
                        Objects.RandomContainer.Add((int)tempObj.RandomContainer.ID, new List<int>());
                    }

                    Objects.RandomContainer[(int)tempObj.RandomContainer.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.SwitchContainer != null)
                {
                    if (!Objects.SwitchContainer.ContainsKey((int)tempObj.SwitchContainer.ID))
                    {
                        Objects.SwitchContainer.Add((int)tempObj.SwitchContainer.ID, new List<int>());
                    }

                    Objects.SwitchContainer[(int)tempObj.SwitchContainer.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.ActorMixer != null)
                {
                    if (!Objects.ActorMixer.ContainsKey((int)tempObj.ActorMixer.ID))
                    {
                        Objects.ActorMixer.Add((int)tempObj.ActorMixer.ID, new List<int>());
                    }

                    Objects.ActorMixer[(int)tempObj.ActorMixer.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.AudioBus != null)
                {
                    if (!Objects.AudioBus.ContainsKey((int)tempObj.AudioBus.ID))
                    {
                        Objects.AudioBus.Add((int)tempObj.AudioBus.ID, new List<int>());
                    }

                    Objects.AudioBus[(int)tempObj.AudioBus.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.BlendContainer != null)
                {
                    if (!Objects.BlendContainer.ContainsKey((int)tempObj.BlendContainer.ID))
                    {
                        Objects.BlendContainer.Add((int)tempObj.BlendContainer.ID, new List<int>());
                    }

                    Objects.BlendContainer[(int)tempObj.BlendContainer.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.MusicSegment != null)
                {
                    foreach (uint child in tempObj.MusicSegment.ChildIDs)
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
                    foreach (TrackSource src in tempObj.MusicTrack.TrackSources)
                    {
                        if (!Objects.MusicTrack.ContainsKey((int)src.SourceID))
                        {
                            Objects.MusicTrack.Add((int)src.SourceID, new List<int>());
                        }

                        Objects.MusicTrack[(int)src.SourceID].Add(Objects.Data.IndexOf(tempObj));
                    }
                }
                else if (tempObj.MusicSwitchContainer != null)
                {
                    if (!Objects.MusicSwitchContainer.ContainsKey((int)tempObj.MusicSwitchContainer.ID))
                    {
                        Objects.MusicSwitchContainer.Add((int)tempObj.MusicSwitchContainer.ID, new List<int>());
                    }

                    Objects.MusicSwitchContainer[(int)tempObj.MusicSwitchContainer.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.MusicSequence != null)
                {
                    if (!Objects.MusicSequence.ContainsKey((int)tempObj.MusicSequence.ID))
                    {
                        Objects.MusicSequence.Add((int)tempObj.MusicSequence.ID, new List<int>());
                    }

                    Objects.MusicSequence[(int)tempObj.MusicSequence.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.Attenuation != null)
                {
                    if (!Objects.Attenuation.ContainsKey((int)tempObj.Attenuation.ID))
                    {
                        Objects.Attenuation.Add((int)tempObj.Attenuation.ID, new List<int>());
                    }

                    Objects.Attenuation[(int)tempObj.Attenuation.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.FeedbackNode != null)
                {
                    if (!Objects.FeedbackNode.ContainsKey((int)tempObj.FeedbackNode.ID))
                    {
                        Objects.FeedbackNode.Add((int)tempObj.FeedbackNode.ID, new List<int>());
                    }

                    Objects.FeedbackNode[(int)tempObj.FeedbackNode.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.FxShareSet != null)
                {
                    if (!Objects.FxShareSet.ContainsKey((int)tempObj.FxShareSet.ID))
                    {
                        Objects.FxShareSet.Add((int)tempObj.FxShareSet.ID, new List<int>());
                    }

                    Objects.FxShareSet[(int)tempObj.FxShareSet.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.FxCustom != null)
                {
                    if (!Objects.FxCustom.ContainsKey((int)tempObj.FxCustom.ID))
                    {
                        Objects.FxCustom.Add((int)tempObj.FxCustom.ID, new List<int>());
                    }

                    Objects.FxCustom[(int)tempObj.FxCustom.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.AuxiliaryBus != null)
                {
                    if (!Objects.AuxiliaryBus.ContainsKey((int)tempObj.AuxiliaryBus.ID))
                    {
                        Objects.AuxiliaryBus.Add((int)tempObj.AuxiliaryBus.ID, new List<int>());
                    }

                    Objects.AuxiliaryBus[(int)tempObj.AuxiliaryBus.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.LFO != null)
                {
                    if (!Objects.LFO.ContainsKey((int)tempObj.LFO.ID))
                    {
                        Objects.LFO.Add((int)tempObj.LFO.ID, new List<int>());
                    }

                    Objects.LFO[(int)tempObj.LFO.ID].Add(Objects.Data.IndexOf(tempObj));
                }
                else if (tempObj.Envelope != null)
                {
                    if (!Objects.Envelope.ContainsKey((int)tempObj.Envelope.ID))
                    {
                        Objects.Envelope.Add((int)tempObj.Envelope.ID, new List<int>());
                    }

                    Objects.Envelope[(int)tempObj.Envelope.ID].Add(Objects.Data.IndexOf(tempObj));
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
                else
                {
                    bw.Write(obj.Data);
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

            foreach (uint DataPart in Header.Data)
            {
                bw.Write(DataPart);
            }

            foreach (var Data in ExtraData)
            {
                if (Data.Key == 1129466184)
                {
                    WriteHIRC(bw);
                }
                else if (Data.Key == 1480870212)
                {
                    WriteDidX(bw);
                }
                else
                {
                    bw.Write(Data.Key);
                    bw.Write(Data.Value.Length);
                    bw.Write(Data.Value);
                }
            }
        }

        public uint GetLength()
        {
            uint Length = 28 + (uint)(Header.Data.Count * 4);

            if (ExtraData.ContainsKey(1480870212))
            {
                Length += (uint)WemList.Count * 12 + 8;

                foreach (Wem wem in WemList)
                {
                    Length += (uint)wem.File.Length;
                }
            }

            if (ExtraData.ContainsKey(1129466184))
            {
                Length += (uint)Objects.GetLength();
            }

            foreach (var Data in ExtraData)
            {
                Length += 8 + (uint)Data.Value.Length;
            }

            return Length;
        }
    }
}
