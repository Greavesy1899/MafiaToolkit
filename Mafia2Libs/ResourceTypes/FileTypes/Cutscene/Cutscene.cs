using ResourceTypes.Cutscene.AnimEntities;
using System;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.Logging;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene
{
    //ue::game::cutscenes::C_AnimEntityManager::LoadEntityCreateData
    //__cstring:01E80A6E aPp41_motionblu db 'pp41_MotionBlurScale',0
    //__cstring:01E80A83 aU025_motionblu db 'U025_MotionBlurStartAndBack',0
    //__cstring:01E80A9F aPp00_blendamou db 'pp00_BlendAmount',0
    //__cstring:01E80AB0 aPp20_rblurnobl db 'pp20_RBlurNoblurRadius',0
    //__cstring:01E80AC7 aPp21_rblurblen db 'pp21_RBlurBlendAmount',0
    //__cstring:01E80ADD aPp38_doubleblu db 'pp38_DoubleBlurDistance',0
    //__cstring:01E80AF5 aPp17_edgetrash db 'pp17_EdgeTrashold',0
    //__cstring:01E80B07 aPp18_edgepower db 'pp18_EdgePower',0
    //__cstring:01E80B16 aPp19_edgewidth db 'pp19_EdgeWidth',0
    //__cstring:01E80B25 aPp09_noisegrai db 'pp09_NoiseGrainThickness',0
    //__cstring:01E80B3E aPp10_noisegrai db 'pp10_NoiseGrainAmount',0
    //__cstring:01E80B54 aPp01_intensity db 'pp01_Intensity',0
    //__cstring:01E80B63 aPp02_cutoff    db 'pp02_Cutoff',0
    //__cstring:01E80B6F aU014_bloomcont db 'U014_BloomContrast',0
    //__cstring:01E80B82 aU015_bloombrig db 'U015_BloomBrightness',0
    //__cstring:01E80B97 aU016_bloomsatu db 'U016_BloomSaturation',0
    //__cstring:01E80BAC aU017_bloomhue  db 'U017_BloomHue',0
    //__cstring:01E80BBA aU018_bloomoffs db 'U018_BloomOffset',0
    //__cstring:01E80BCB aU005_accomodat db 'U005_AccomodationSpeed',0
    //__cstring:01E80BE2 aU006_accomodat db 'U006_AccomodationTarget',0
    //__cstring:01E80BFA aU008_coveragai db 'U008_CoverAgainstSun',0
    //__cstring:01E80C0F aU007_boostinda db 'U007_BoostInDarkness',0
    //__cstring:01E80C24 aPp05_focusdist db 'pp05_FocusDistance',0
    //__cstring:01E80C37 aPp06_dofnear   db 'pp06_DofNear',0
    //__cstring:01E80C44 aPp07_doffar    db 'pp07_DofFar',0
    //__cstring:01E80C50 aPp08_maxblur   db 'pp08_MaxBlur',0
    //__cstring:01E80C5D aPp13_maxblurne db 'pp13_MaxBlurNear',0
    //__cstring:01E80C6E aPp03_basecolor db 'pp03_BaseColor',0
    //__cstring:01E80C7D aPp04_scenecolo db 'pp04_SceneColor',0
    //__cstring:01E80C8D aU001_contrast  db 'U001_Contrast',0
    //__cstring:01E80C9B aU002_brightnes db 'U002_Brightness',0
    //__cstring:01E80CAB aU003_saturatio db 'U003_Saturation',0
    //__cstring:01E80CBB aU004_hue       db 'U004_Hue',0
    //__cstring:01E80CC4 aU019_offset    db 'U019_Offset',0
    //__cstring:01E80CD0 aU020_precontra db 'U020_PreContrast',0
    //__cstring:01E80CE1 aU021_prebright db 'U021_PreBrightness',0
    //__cstring:01E80CF4 aU022_presatura db 'U022_PreSaturation',0
    //__cstring:01E80D07 aU023_prehue db    'U023_PreHue',0
    //__cstring:01E80D13 aU024_preoffset db 'U024_PreOffset',0
    //__cstring:01E80D22 aU009_maxflash db 'U009_MaxFlash',0
    //__cstring:01E80D30 aU010_fadeintim db 'U010_FadeInTime',0
    //__cstring:01E80D40 aU011_blindtime db 'U011_BlindTime',0
    //__cstring:01E80D4F aU012_blindfade db 'U012_BlindFadeOutTime',0
    //__cstring:01E80D65 aU013_monofadeo db 'U013_MonoFadeOutTime',0

    public class CutsceneLoader
    {
        private Cutscene[] cutscenes;

        public Cutscene[] Cutscenes {
            get { return cutscenes; }
            set { cutscenes = value; }
        }

        public GCRData[] VehicleContent { get; set; } = new GCRData[0];

        public CutsceneLoader(FileInfo file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int numCutscenes = reader.ReadInt32();
            cutscenes = new Cutscene[numCutscenes];
            for(int i = 0; i < cutscenes.Length; i++)
            {
                cutscenes[i] = new Cutscene(reader);
            }

            int numGCR = reader.ReadInt32();

            VehicleContent = new GCRData[numGCR];

            for (int i = 0; i < numGCR; i++)
            {
                VehicleContent[i] = new();
                VehicleContent[i].ReadFromFile(reader);
            }
        }

        public void WriteToFile(string Filename)
        {
            File.Copy(Filename, Filename + "_old", true);
            using (BinaryWriter Writer = new BinaryWriter(File.Open(Filename, FileMode.Create)))
            {
                InternalWriteToFile(Writer);
            }
        }

        private void InternalWriteToFile(BinaryWriter Writer)
        {
            Writer.Write(Cutscenes.Length);
            foreach(var Scene in Cutscenes)
            {
                Scene.WriteToFile(Writer);
            }

            Writer.Write(VehicleContent.Length); // Num GCR

            foreach (GCRData VehicleData in VehicleContent)
            {
                VehicleData.WriteToFile(Writer);
            }
        }

        public class Cutscene
        {
            [Browsable(false)]
            private int unk05; // Padding?
            [Browsable(false)]
            private byte unk06; // Compressed?

            public string CutsceneName { get; set; }
            [Browsable(false)]
            public GCSData AssetContent { get; set; }
            [Browsable(false)]
            public SPDData SoundContent { get; set; }

            public Cutscene(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                CutsceneName = reader.ReadString16();

                unk05 = reader.ReadInt32();
                unk06 = reader.ReadByte();

                using (BinaryReader gcsReader = new(new MemoryStream(reader.ReadBytes(reader.ReadInt32() - 8))))
                {
                    AssetContent = new GCSData();
                    AssetContent.ReadFromFile(gcsReader, CutsceneName);
                }

                bool hasSPD = reader.ReadBoolean();
                if(hasSPD)
                {
                    reader.ReadInt32(); //1000
                    using (BinaryReader spdReader = new(new MemoryStream(reader.ReadBytes(reader.ReadInt32() - 8))))
                    {
                        SoundContent = new SPDData();
                        SoundContent.ReadFromFile(spdReader, CutsceneName);
                    }
                }
            }

            public void WriteToFile(BinaryWriter Writer)
            {
                Writer.WriteString16(CutsceneName);
                Writer.Write(unk05);
                Writer.Write(unk06);

                using (MemoryStream ms = new())
                {
                    using (BinaryWriter bw = new(ms))
                    {
                        AssetContent.WriteToFile(bw);
                    }

                    byte[] gcsData = ms.ToArray();
                    Writer.Write(gcsData.Length + 8);
                    Writer.Write(gcsData);
                }

                Writer.Write(SoundContent != null);

                if (SoundContent != null)
                {
                    Writer.Write(1000);

                    using (MemoryStream ms = new())
                    {
                        using (BinaryWriter bw = new(ms))
                        {
                            SoundContent.WriteToFile(bw);
                        }

                        byte[] spdData = ms.ToArray();
                        Writer.Write(spdData.Length + 8);
                        Writer.Write(spdData);
                    }
                }
            }

            public class GCSData
            {
                private string header; //usually equals !GCS.
                public int Type { get; set; } //cutscene flags? 
                public float FPS { get; set; } //0?
                public short unk04 { get; set; } //25.0f;
                public short unk05 { get; set; } //sometimes 0xFF
                public int unk06 { get; set; } //100000.
                public FaceFX FaceFX { get; set; }
                [Browsable(false)]
                public AnimEntityWrapper[] entities { get; set; } = new AnimEntityWrapper[0];
                public int unk10 { get; set; }
                public float unk11 { get; set; }
                public float unk12 { get; set; }
                public float FrameCount { get; set; }
                public int unk14 { get; set; }

                private string CutsceneName;

                public void ReadFromFile(BinaryReader reader, string name)
                {
                    CutsceneName = name;

                    header = new string(reader.ReadChars(4));
                    Type = reader.ReadInt32();
                    FPS = reader.ReadSingle();

                    if (Type != 101)
                    {
                        unk04 = reader.ReadInt16();
                        unk05 = reader.ReadInt16();
                    }
                    
                    unk06 = reader.ReadInt32();
                    FaceFX = new FaceFX(reader);

                    ToolkitAssert.Ensure(reader.ReadInt32() == 100, "Missed the Entity Block Magic!");

                    using (BinaryReader br = new(new MemoryStream(reader.ReadBytes(reader.ReadInt32() - 8))))
                    {
                        int numEntities = br.ReadUInt16();
                        entities = new AnimEntityWrapper[numEntities];

                        for (int i = 0; i < numEntities; i++)
                        {
                            int Header = br.ReadInt32();
                            ToolkitAssert.Ensure(Header == 126, "We've missed the entity definition Magic!"); // Or 0x7F

                            int Size = br.ReadInt32();
                            AnimEntityTypes AnimEntityType = (AnimEntityTypes)br.ReadInt32();

                            byte[] DefintionData = br.ReadBytes(Size - 12);

                            using (MemoryStream Reader = new MemoryStream(DefintionData))
                            {
                                AnimEntityWrapper EntityWrapper = CutsceneEntityFactory.ReadAnimEntityWrapperFromFile(AnimEntityType, Reader);

                                EntityWrapper.CutsceneName = CutsceneName;

                                //string format = string.Format("CutsceneInfo/{2}/Entity_{0}_{1}.bin", AnimEntityType, i, CutsceneName);
                                //File.WriteAllBytes(format, DefintionData);

                                entities[i] = EntityWrapper;
                            }
                        }

                        for (int z = 0; z < numEntities; z++)
                        {
                            // Export this first
                            int dataType = br.ReadInt32();
                            int size = br.ReadInt32();
                            br.BaseStream.Position -= 8;
                            byte[] dataBytes = br.ReadBytes(size);

                            //string folderPath = "%userprofile%\\Desktop\\CutsceneInfo\\" + CutsceneName;
                            //string path = Environment.ExpandEnvironmentVariables(folderPath);
                            //
                            //if (!Directory.Exists(path))
                            //{
                            //    Directory.CreateDirectory(path);
                            //}
                            //
                            //string format = string.Format("\\{0}_{1}.bin", z.ToString("0000"), entities[z].GetType().Name);
                            //path = path + format;
                            //File.WriteAllBytes(path, dataBytes);

                            // And then This
                            using (MemoryStream stream = new MemoryStream(dataBytes))
                            {
                                entities[z].AnimEntityData.CutsceneName = CutsceneName;
                                entities[z].AnimEntityData.ReadFromFile(stream, false);
                                ToolkitAssert.Ensure(stream.Position == stream.Length, "When reading the AnimEntity Data, we did not reach the end of the stream!");
                            }
                        }
                    }

                    unk10 = reader.ReadInt32();
                    unk11 = reader.ReadSingle();
                    unk12 = reader.ReadSingle();
                    FrameCount = reader.ReadSingle();
                    unk14 = reader.ReadInt32();
                }

                public void WriteToFile(BinaryWriter writer)
                {
                    writer.Write(header.ToCharArray());
                    writer.Write(Type);
                    writer.Write(FPS);

                    if (Type != 101)
                    {
                        writer.Write(unk04);
                        writer.Write(unk05);
                    }

                    writer.Write(unk06);
                    FaceFX.Write(writer);

                    writer.Write(100); //Entity block magic

                    using (MemoryStream ms = new())
                    {
                        using (BinaryWriter bw = new(ms))
                        {
                            bw.Write((short)entities.Length);

                            foreach (var Entity in entities)
                            {
                                bw.Write(126); //Header for types is 126.

                                byte[] entityData;

                                using (MemoryStream stream = new MemoryStream())
                                {
                                    // Write Entity to the Stream
                                    CutsceneEntityFactory.WriteAnimEntityToFile(stream, Entity);
                                    entityData = stream.ToArray();
                                }

                                bw.Write(entityData.Length + 12);
                                bw.Write((int)Entity.GetEntityType());
                                bw.Write(entityData);


                            }

                            foreach (var Entity in entities)
                            {
                                using (MemoryStream EntityStream = new MemoryStream())
                                {
                                    bool isBigEndian = false;
                                    Entity.AnimEntityData.WriteToFile(EntityStream, isBigEndian);

                                    byte[] animEntityData = EntityStream.ToArray();

                                    bw.Write(Entity.AnimEntityData.DataType);
                                    bw.Write(animEntityData.Length + 8);
                                    bw.Write(animEntityData);
                                }
                            }
                        }

                        byte[] data = ms.ToArray();
                        writer.Write(data.Length + 8);
                        writer.Write(data);
                    }

                    writer.Write(unk10);
                    writer.Write(unk11);
                    writer.Write(unk12);
                    writer.Write(FrameCount);
                    writer.Write(unk14);
                }
            }

            public class SPDData
            {
                public int Unk01 { get; set; }
                public float Unk02 { get; set; } // For GCS this is FPS i think.
                [Browsable(false)]
                public AnimEntityWrapper[] EntityDefinitions { get; set; }
                private string CutsceneName; // For debugging

                public void ReadFromFile(BinaryReader reader, string name)
                {
                    CutsceneName = name;
                    int fourcc = reader.ReadInt32();
                    if (fourcc == 0x21445053)
                    {
                        Unk01 = reader.ReadInt32();
                        Unk02 = reader.ReadSingle();
                        int NumEntities = reader.ReadInt32();
                        EntityDefinitions = new AnimEntityWrapper[NumEntities];

                        for (int i = 0; i < NumEntities; i++)
                        {
                            int Header = reader.ReadInt32();
                            ToolkitAssert.Ensure(Header == 126, "We've missed the entity definition Magic!"); // Or 0x7F

                            int Size = reader.ReadInt32();
                            AnimEntityTypes AnimEntityType = (AnimEntityTypes)reader.ReadInt32();

                            byte[] DefintionData = reader.ReadBytes(Size - 12);
                            using (MemoryStream Reader = new MemoryStream(DefintionData))
                            {
                                AnimEntityWrapper Entity = CutsceneEntityFactory.ReadAnimEntityWrapperFromFile(AnimEntityType, Reader);
                                EntityDefinitions[i] = Entity;
                            }
                        }

                        for (int z = 0; z < NumEntities; z++)
                        {
                            // Export this first
                            int dataType = reader.ReadInt32();
                            int entitySize = reader.ReadInt32();
                            reader.BaseStream.Position -= 8;
                            byte[] dataBytes = reader.ReadBytes(entitySize);

                            // And then This
                            using (MemoryStream stream = new MemoryStream(dataBytes))
                            {
                                EntityDefinitions[z].AnimEntityData.CutsceneName = CutsceneName;
                                EntityDefinitions[z].AnimEntityData.ReadFromFile(stream, false);
                            }
                        }
                    }
                }

                public void WriteToFile(BinaryWriter Writer)
                {
                    Writer.Write(0x21445053);
                    Writer.Write(Unk01);
                    Writer.Write(Unk02);
                    Writer.Write(EntityDefinitions.Length);

                    foreach (var Entity in EntityDefinitions)
                    {
                        byte[] AnimEntityData;

                        using (MemoryStream EntityStream = new MemoryStream())
                        {
                            CutsceneEntityFactory.WriteAnimEntityToFile(EntityStream, Entity);
                            
                            AnimEntityData = EntityStream.ToArray();
                        }

                        Writer.Write(126); //Header for types is 126.
                        Writer.Write(AnimEntityData.Length + 12);
                        Writer.Write((int)Entity.GetEntityType());
                        Writer.Write(AnimEntityData);
                    }

                    foreach (var Entity in EntityDefinitions)
                    {
                        using (MemoryStream EntityStream = new MemoryStream())
                        {
                            bool isBigEndian = false;

                            Entity.AnimEntityData.WriteToFile(EntityStream, isBigEndian);

                            byte[] animEntityData = EntityStream.ToArray();

                            Writer.Write(Entity.AnimEntityData.DataType);
                            Writer.Write(animEntityData.Length + 8);
                            Writer.Write(animEntityData);
                        }
                    }
                }
            }
        }

        public class GCRData
        {
            public string Name { get; set; }
            public byte[] Data { get; set; }

            public void ReadFromFile(BinaryReader reader)
            {
                Name = reader.ReadString16();
                reader.ReadInt32(); //Const 2569
                int size = reader.ReadInt32();
                Data = reader.ReadBytes(size - 8);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.WriteString16(Name);
                writer.Write(2569);
                writer.Write(Data.Length + 8); // writing size here
                writer.Write(Data);
            }
        }
    }
}