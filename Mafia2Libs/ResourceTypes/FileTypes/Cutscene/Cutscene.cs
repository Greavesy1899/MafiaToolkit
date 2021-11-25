using ResourceTypes.Cutscene.AnimEntities;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Utils.Extensions;
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
        }

        public class Cutscene
        {
            private byte[] unk05; // Padding?
            private int gcsSize; // Will need to be replaced

            public string CutsceneName { get; set; }
            public GCSData AssetContent { get; set; }
            public SPDData SoundContent { get; set; }
            public GCRData[] VehicleContent { get; set; }

            public Cutscene(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                short length = reader.ReadInt16();
                CutsceneName = new string(reader.ReadChars(length));

                if(!Directory.Exists("CutsceneInfo/"+ CutsceneName))
                {
                    Directory.CreateDirectory("CutsceneInfo/" + CutsceneName);
                }

                unk05 = reader.ReadBytes(5);
                gcsSize = reader.ReadInt32();

                long start = reader.BaseStream.Position;
                AssetContent = new GCSData();
                AssetContent.ReadFromFile(reader, CutsceneName);

                bool hasSPD = reader.ReadBoolean();
                if(hasSPD)
                {
                    SoundContent = new SPDData();
                    SoundContent.ReadFromFile(reader, CutsceneName);
                }

                // TODO: Figure out the actual way of detecting this.
                // Get vehicle data.. if it is an absurd value we know its a GCR.
                int numGCR = reader.ReadInt32();
                if(numGCR > 0xFF)
                {
                    reader.BaseStream.Position -= 4;
                    return;
                }

                VehicleContent = new GCRData[numGCR];

                for (int i = 0; i < numGCR; i++)
                {
                    VehicleContent[i] = new GCRData();
                    VehicleContent[i].ReadFromFile(reader);
                }
                return;
            }

            public void WriteToFile(BinaryWriter Writer)
            {
                StringHelpers.WriteString16(Writer, CutsceneName);
                Writer.Write(unk05);
                Writer.Write(gcsSize);
                AssetContent.WriteToFile(Writer);

                if(SoundContent != null)
                {
                    Writer.Write(true); // Has SPD
                    SoundContent.WriteToFile(Writer);
                }

                // TODO: Not sure if this is fully valid or not..
                if (VehicleContent != null && VehicleContent.Length > 0)
                {
                    Writer.Write(VehicleContent.Length); // Num GCR

                    foreach(GCRData VehicleData in VehicleContent)
                    {
                        VehicleData.WriteToFile(Writer);
                    }
                }
                else
                {
                    Writer.Write(0);
                }
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
                public AnimEntityWrapper[] entities;
                public int unk10;
                public float unk11;
                public int unk12;
                public float unk13;
                public int unk14;

                private string CutsceneName;

                public void ReadFromFile(BinaryReader reader, string name)
                {
                    CutsceneName = name;

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
                    entities = new AnimEntityWrapper[numEntities];

                    for(int i = 0; i < numEntities; i++)
                    {
                        int Header = reader.ReadInt32();
                        Debug.Assert(Header == 126, "We've missed the entity definition Magic!"); // Or 0x7F

                        int Size = reader.ReadInt32();
                        int RawAnimEntityType = reader.ReadInt32();
                        AnimEntityTypes AnimEntityType = (AnimEntityTypes)RawAnimEntityType;

                        byte[] DefintionData = reader.ReadBytes(Size - 12);
                        using (MemoryStream Reader = new MemoryStream(DefintionData))
                        {
                            AnimEntityWrapper EntityWrapper = CutsceneEntityFactory.ReadAnimEntityWrapperFromFile(AnimEntityType, Reader);

                            string format = string.Format("CutsceneInfo/{2}/Entity_{0}_{1}.bin", AnimEntityType, i, CutsceneName);
                            File.WriteAllBytes(format, DefintionData);

                            entities[i] = EntityWrapper;
                        }
                    }

                    for (int z = 0; z < numEntities; z++)
                    {
                        // Export this first
                        int dataType = reader.ReadInt32();
                        int size = reader.ReadInt32();
                        reader.BaseStream.Position -= 8;
                        byte[] dataBytes = reader.ReadBytes(size);

                        string format = string.Format("CutsceneInfo/{2}/{0}_{1}.bin", entities[z], z, CutsceneName);
                        File.WriteAllBytes(format, dataBytes);

                        // And then This
                        using(MemoryStream stream = new MemoryStream(dataBytes))
                        {
                            entities[z].AnimEntityData.ReadFromFile(stream, false);
                            Debug.Assert(stream.Position == stream.Length, "When reading the AnimEntity Data, we did not reach the end of the stream!");
                        }
                    }

                    unk10 = reader.ReadInt32();
                    unk11 = reader.ReadSingle();
                    unk12 = reader.ReadInt32();
                    unk13 = reader.ReadSingle();
                    unk14 = reader.ReadInt32();
                }

                public void WriteToFile(BinaryWriter writer)
                {
                    writer.Write(header.ToCharArray());
                    writer.Write(unk02);
                    writer.Write(unk03);
                    writer.Write(unk04);
                    writer.Write(unk05);
                    writer.Write(unk06);
                    writer.Write(unk07);
                    writer.Write(unk08);
                    writer.Write(unk09);
                    writer.Write(numEntities);

                    foreach(var Entity in entities)
                    {
                        writer.Write(126); //Header for types is 126.

                        long sizePosition = writer.BaseStream.Position;
                        writer.Write(-1);
                        writer.Write((int)Entity.GetEntityType());

                        using(MemoryStream stream = new MemoryStream())
                        {
                            // Write Entity to the Stream
                            CutsceneEntityFactory.WriteAnimEntityToFile(stream, Entity);
                            writer.Write(stream.ToArray());

                            // Update Size.
                            long currentPosition = writer.BaseStream.Position;
                            writer.BaseStream.Seek(sizePosition, SeekOrigin.Begin);
                            writer.Write((uint)stream.Length + 12);
                            writer.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
                        }
                    }

                    foreach (var Entity in entities)
                    {
                        using (MemoryStream EntityStream = new MemoryStream())
                        {
                            bool isBigEndian = false;
                            Entity.AnimEntityData.WriteToFile(EntityStream, isBigEndian);

                            EntityStream.Seek(4, SeekOrigin.Begin);
                            EntityStream.Write((uint)EntityStream.Length, isBigEndian);
                            EntityStream.Seek(0, SeekOrigin.End);

                            writer.Write(EntityStream.ToArray());
                        }
                    }

                    writer.Write(unk10);
                    writer.Write(unk11);
                    writer.Write(unk12);
                    writer.Write(unk13);
                    writer.Write(unk14);
                }
            }

            public class SPDData
            {
                public int Unk01 { get; set; }
                public float Unk02 { get; set; } // For GCS this is FPS i think.
                public AnimEntityWrapper[] EntityDefinitions { get; set; }

                private uint Size; // Will be removed when we have proper saving.
                private string CutsceneName; // For debugging

                public void ReadFromFile(BinaryReader reader, string name)
                {
                    CutsceneName = name;

                    int magic = reader.ReadInt32();
                    if(magic == 1000)
                    {
                        //the size includes the size and the magic, A.K.A: (magic and size == 8)
                        Size = reader.ReadUInt32();

                        int fourcc = reader.ReadInt32();
                        if(fourcc == 0x21445053)
                        {
                            Unk01 = reader.ReadInt32();
                            Unk02 = reader.ReadSingle();
                            int NumEntities = reader.ReadInt32();
                            EntityDefinitions = new AnimEntityWrapper[NumEntities];

                            for (int i = 0; i < NumEntities; i++)
                            {
                                int Header = reader.ReadInt32();
                                Debug.Assert(Header == 126, "We've missed the entity definition Magic!"); // Or 0x7F

                                int Size = reader.ReadInt32();
                                int RawAnimEntityType = reader.ReadInt32();
                                AnimEntityTypes AnimEntityType = (AnimEntityTypes)RawAnimEntityType;

                                byte[] DefintionData = reader.ReadBytes(Size - 12);
                                using (MemoryStream Reader = new MemoryStream(DefintionData))
                                {
                                    AnimEntityWrapper Entity = CutsceneEntityFactory.ReadAnimEntityWrapperFromFile(AnimEntityType, Reader);

                                    if (Debugger.IsAttached)
                                    {
                                        // Debugging: If the AnimEntity is null and a debugger is attached, we should save it to the disc.
                                        string format = string.Format("CutsceneInfo/{0}/Entity_SPD_{1}_{2}.bin", CutsceneName, AnimEntityType, i);
                                        File.WriteAllBytes(format, DefintionData);
                                    }

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

                                if (Debugger.IsAttached)
                                {
                                    string format = string.Format("CutsceneInfo/{2}/{0}_SPD_{1}.bin", EntityDefinitions[z], z, CutsceneName);
                                    File.WriteAllBytes(format, dataBytes);
                                }

                                // And then This
                                using (MemoryStream stream = new MemoryStream(dataBytes))
                                {
                                    EntityDefinitions[z].AnimEntityData.ReadFromFile(stream, false);
                                }
                            }
                        }
                    }
                }

                public void WriteToFile(BinaryWriter Writer)
                {
                    Writer.Write(1000); // Magic
                    long SPDSizePosition = Writer.BaseStream.Position;
                    Writer.Write(Size);
                    Writer.Write(0x21445053);
                    Writer.Write(Unk01);
                    Writer.Write(Unk02);
                    Writer.Write(EntityDefinitions.Length);

                    foreach (var Entity in EntityDefinitions)
                    {
                        Writer.Write(126); //Header for types is 126.

                        long sizePosition = Writer.BaseStream.Position;
                        Writer.Write(-1);
                        Writer.Write((int)Entity.GetEntityType());

                        using (MemoryStream EntityStream = new MemoryStream())
                        {
                            CutsceneEntityFactory.WriteAnimEntityToFile(EntityStream, Entity);
                            Writer.Write(EntityStream.ToArray());

                            long currentPosition = Writer.BaseStream.Position;
                            Writer.BaseStream.Seek(sizePosition, SeekOrigin.Begin);
                            Writer.Write((uint)EntityStream.Length + 12);
                            Writer.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
                        }
                    }

                    foreach (var Entity in EntityDefinitions)
                    {
                        using (MemoryStream EntityStream = new MemoryStream())
                        {
                            bool isBigEndian = false;
                            Entity.AnimEntityData.WriteToFile(EntityStream, isBigEndian);

                            EntityStream.Seek(4, SeekOrigin.Begin);
                            EntityStream.Write((uint)EntityStream.Length, isBigEndian);
                            EntityStream.Seek(0, SeekOrigin.End);

                            Writer.Write(EntityStream.ToArray());
                        }
                    }

                    Writer.Seek((int)SPDSizePosition, SeekOrigin.Begin);
                    uint SPDSize = (uint)(Writer.BaseStream.Length - SPDSizePosition) + 4;
                    Writer.Write(SPDSize);
                    Writer.Seek(0, SeekOrigin.End);
                }
            }

            public class GCRData
            {
                public string Name { get; set; }
                public int Unk0 { get; set; }
                public byte[] Data { get; set; }

                public void ReadFromFile(BinaryReader reader)
                {
                    Name = StringHelpers.ReadString16(reader);
                    Unk0 = reader.ReadInt32();
                    int size = reader.ReadInt32();
                    reader.BaseStream.Position -= 8;
                    //Data = reader.ReadBytes(size-8);
                    Data = reader.ReadBytes(size);

                    if (Debugger.IsAttached)
                    {
                        File.WriteAllBytes("CutsceneInfo/" + Name + ".gcr", Data);
                    }
                }

                public void WriteToFile(BinaryWriter writer)
                {
                    StringHelpers.WriteString16(writer, Name);
                    writer.Write(Unk0);
                    writer.Write(Data.Length + 8); // writing size here
                    writer.Write(Data);
                }
            }
        }
    }
}