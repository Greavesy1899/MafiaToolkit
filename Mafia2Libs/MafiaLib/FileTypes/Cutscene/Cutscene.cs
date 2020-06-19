using SharpDX;
using System;
using System.Diagnostics;
using System.IO;
using Utils.SharpDXExtensions;
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
        private int count; // number of cutscenes
        private Cutscene[] cutscenes;

        public CutsceneLoader(FileInfo file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            count = reader.ReadInt32();
            cutscenes = new Cutscene[count];
            for(int i = 0; i != cutscenes.Length; i++)
            {
                cutscenes[i] = new Cutscene(reader);
                return;
            }
        }

        public class Cutscene
        {
            private string cutsceneName; //begins with a length; short data type.
            //5 empty bytes;
            //This size of GCS Seems to be ignoring the cutscene count, and then checking for a bool; basically to check if SPD data exists.
            private int gcsSize; //size of GCS! data.
            private GCSData gcsData; //GCS! data.
            private SPDData spdData; //SPD! data.
            private GCRData[] gcrData; //GCR! data;

            public Cutscene(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                short length = reader.ReadInt16();
                cutsceneName = new string(reader.ReadChars(length));
                byte[] unkBytes1 = reader.ReadBytes(5);
                gcsSize = reader.ReadInt32();

                long start = reader.BaseStream.Position;
                gcsData = new GCSData();
                gcsData.ReadFromFile(reader);

                bool hasSPD = reader.ReadBoolean();
                if(hasSPD)
                {
                    spdData = new SPDData();
                    spdData.ReadFromFile(reader);
                    File.WriteAllBytes("CutsceneData/SPD_Data.bin", spdData.Data);
                }

                int gcrCount = reader.ReadInt32();
                gcrData = new GCRData[gcrCount];
                for(int i = 0; i < gcrCount; i++)
                {
                    GCRData data = new GCRData();
                    data.ReadFromFile(reader);
                    gcrData[i] = data;
                    File.WriteAllBytes("Cutscenedata/GCR_" + gcrData[i].Name + ".bin", gcrData[i].Data);
                }

                return;
            }

            public class GCSData
            {
                private string header; //usually equals !GCS.
                private int unk02; //cutscene flags? 
                private float unk03; //0?
                private short unk04; //25.0f;
                private short unk05; //sometimes 0xFF
                private int unk06; //100000.
                private int unk07; //size
                private byte[] unk08; //size from unk07; facefx data
                private int unk09; //possible size of entries;
                private ushort numEntities; //numEntities;
                public IAeEntity[] entities;
                public int unk10;
                public float unk11;
                public int unk12;
                public float unk13;
                public int unk14;

                public void ReadFromFile(BinaryReader reader)
                {
                    header = new string(reader.ReadChars(4));
                    unk02 = reader.ReadInt32();
                    unk03 = reader.ReadSingle();
                    unk04 = reader.ReadInt16();
                    unk05 = reader.ReadInt16();
                    unk06 = reader.ReadInt32();
                    unk07 = reader.ReadInt32();
                    unk08 = reader.ReadBytes(unk07-4);
                    unk09 = reader.ReadInt32();
                    numEntities = reader.ReadUInt16();
                    entities = new IAeEntity[numEntities];

                    for(int i = 0; i < numEntities; i++)
                    {
                        int header = reader.ReadInt32();
                        if (header == 126)
                        {
                            int size = reader.ReadInt32();
                            int type = reader.ReadInt32();
                            byte[] data = reader.ReadBytes(size - 12);
                            MemoryStream stream = new MemoryStream(data);
                            if(type == AeModel.Type)
                            {
                                AeModel model = new AeModel();
                                model.ReadFromFile(stream, false);
                                entities[i] = model;
                            }
                            else if (type == AeSpotLight.Type)
                            {
                                AeSpotLight spotLight = new AeSpotLight();
                                spotLight.ReadFromFile(stream, false);
                                entities[i] = spotLight;
                            }
                            else if (type == AeOmniLight.Type)
                            {
                                AeOmniLight omniLight = new AeOmniLight();
                                omniLight.ReadFromFile(stream, false);
                                entities[i] = omniLight;
                            }
                            else if (type == AeUnk4.Type)
                            {
                                AeUnk4 unk4 = new AeUnk4();
                                unk4.ReadFromFile(stream, false);
                                entities[i] = unk4;
                            }
                            else if (type == AeTargetCamera.Type)
                            {
                                AeTargetCamera targetCamera = new AeTargetCamera();
                                targetCamera.ReadFromFile(stream, false);
                                entities[i] = targetCamera;
                            }
                            else if(type == AeFrame.Type)
                            {
                                AeFrame frame = new AeFrame();
                                frame.ReadFromFile(stream, false);
                                entities[i] = frame;
                            }
                            else if (type == AeUnk10.Type)
                            {
                                AeUnk10 unk10 = new AeUnk10();
                                unk10.ReadFromFile(stream, false);
                                entities[i] = unk10;
                            }
                            else if (type == AeUnk12.Type)
                            {
                                AeUnk12 unk12 = new AeUnk12();
                                unk12.ReadFromFile(stream, false);
                                entities[i] = unk12;
                            }
                            else if (type == AeUnk13.Type)
                            {
                                AeUnk13 unk13 = new AeUnk13();
                                unk13.ReadFromFile(stream, false);
                                entities[i] = unk13;
                            }
                            else if (type == AeVehicle.Type)
                            {
                                AeVehicle vehicle = new AeVehicle();
                                vehicle.ReadFromFile(stream, false);
                                entities[i] = vehicle;
                            }
                            else if (type == AeUnk18.Type)
                            {
                                AeUnk18 unk18 = new AeUnk18();
                                unk18.ReadFromFile(stream, false);
                                entities[i] = unk18;
                            }
                            else if (type == AeUnk23.Type)
                            {
                                AeUnk23 unk23 = new AeUnk23();
                                unk23.ReadFromFile(stream, false);
                                entities[i] = unk23;
                            }
                            else if (type == AeEffects.Type)
                            {
                                AeEffects effects = new AeEffects();
                                effects.ReadFromFile(stream, false);
                                entities[i] = effects;
                            }
                            else if (type == AeSunLight.Type)
                            {
                                AeSunLight sunlight = new AeSunLight();
                                sunlight.ReadFromFile(stream, false);
                                entities[i] = sunlight;
                            }
                            else
                            {
                                Console.WriteLine("Unknown type");
                                stream.Position = stream.Length;
                                File.WriteAllBytes("CutSceneData/Entitiy" + type + "_" + i + ".bin", data);
                            }
                            File.WriteAllBytes("CutSceneData/Entitiy" + type + "_" + i + ".bin", data);
                            if (stream.Position != stream.Length)
                            {
                                Debug.Assert(stream.Position == stream.Length);
                            }
                            
                        }
                    }

                    for (int z = 0; z < numEntities; z++)
                    {
                        int dunno = reader.ReadInt32();
                        int size = reader.ReadInt32();
                        reader.BaseStream.Position -= 8;
                        File.WriteAllBytes("CutSceneData/" + entities[z] + "_" + z.ToString() + ".bin", reader.ReadBytes(size));
                    }

                    unk10 = reader.ReadInt32();
                    unk11 = reader.ReadSingle();
                    unk12 = reader.ReadInt32();
                    unk13 = reader.ReadSingle();
                    unk14 = reader.ReadInt32();
                }
            }

            public class SPDData
            {
                public byte[] Data;
                public void ReadFromFile(BinaryReader reader)
                {
                    int magic = reader.ReadInt32();
                    if(magic == 1000)
                    {
                        //the size includes the size and the magic, A.K.A: (magic and size == 8)
                        int size = reader.ReadInt32();
                        reader.BaseStream.Position -= 8;
                        //Data = reader.ReadBytes(size-8);
                        Data = reader.ReadBytes(size);
                    }
                }
            }

            public class GCRData
            {
                public string Name;
                public byte[] Data;
                public void ReadFromFile(BinaryReader reader)
                {
                    Name = StringHelpers.ReadString16(reader);
                    int unk0 = reader.ReadInt32();
                    int size = reader.ReadInt32();
                    reader.BaseStream.Position -= 8;
                    //Data = reader.ReadBytes(size-8);
                    Data = reader.ReadBytes(size);
                }
            }
        }
    }
}