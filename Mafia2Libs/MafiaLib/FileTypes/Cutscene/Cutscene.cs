using System;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene
{
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
            }
        }

        public class Cutscene
        {
            private string cutsceneName; //begins with a length; short data type.
            //5 empty bytes;
            //This size of GCS Seems to be ignoring the cutscene count, and then checking for a bool; basically to check if SPD data exists.
            private int gcsSize; //size of GCS! data.
            private GCSData gcsData; //GCS! data.
            private int unk1;
            private SPDData spdData; //SPD! data.

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
                reader.BaseStream.Seek(start, SeekOrigin.Begin);
                byte[] unkBytes2 = reader.ReadBytes(gcsSize - 4);
                int spdSize = reader.ReadInt32();
                if (spdSize == 0)
                {
                    if(reader.BaseStream.Position == reader.BaseStream.Length)
                        Console.WriteLine("Succesfully hit eof");
                    return;
                }
                start = reader.BaseStream.Position;
                spdData = new SPDData();
                spdData.ReadFromFile(reader);
                reader.BaseStream.Seek(start, SeekOrigin.Begin);
                byte[] unkBytes3 = reader.ReadBytes(spdSize);
            }

            public class GCSData
            {
                private string header; //usually equals !GCS.
                private int unk02; //cutscene flags? 
                private float unk03; //0?
                private short unk04; //25.0f;
                private short unk05; //sometimes 0xFF
                private int unk06; //100000.
                private int faceSize; //size of FaceFX data and the int after.
                private int numStrings; //in fmv0108, i found 2 strings; this block could be for faceFX.
                private string[] strings; //size is numstrings.
                private int faceUnk0; //100?

                //size of next set of data.
                private int size2;
                private short unkCount1; //camera count - 1?
                private int unkCount10; //126
                private int unkCount11; //14
                private int unkCount12; //18
                private short unkCount13; //0;
                private int unkCount14; //126;
                private int unkCount2; //entity count - 1?
                private int unkCount21; //0x5;
                private int unkCount22; //0x2;
                private short unkCount23; //0;

                public void ReadFromFile(BinaryReader reader)
                {
                    header = new string(reader.ReadChars(4));
                    unk02 = reader.ReadInt32();
                    unk03 = reader.ReadSingle();
                    unk04 = reader.ReadInt16();
                    unk05 = reader.ReadInt16();
                    unk06 = reader.ReadInt32();
                    faceSize = reader.ReadInt32();
                    numStrings = reader.ReadInt32();
                    strings = new string[numStrings];
                    for (int i = 0; i != numStrings; i++)
                    {
                        short len = reader.ReadInt16();
                        strings[i] = new string(reader.ReadChars(len));
                    }
                    faceUnk0 = reader.ReadInt32();
                    size2 = reader.ReadInt32();
                    unkCount1 = reader.ReadInt16(); //camera count - 1?
                    unkCount10 = reader.ReadInt32(); //126
                    unkCount11 = reader.ReadInt32(); //14
                    unkCount12 = reader.ReadInt32(); //18
                    unkCount13 = reader.ReadInt16(); //0;
                    unkCount14 = reader.ReadInt32(); //126;
                    unkCount2 = reader.ReadInt32(); //entity count - 1?
                    unkCount21 = reader.ReadInt32(); //0x5;
                    unkCount22 = reader.ReadInt32(); //0x2;
                    int unkZero = reader.ReadInt16();
                    for (int i = 0; i != unkCount1; i++)
                    {
                        ulong hash = reader.ReadUInt64();
                        ulong hash2 = reader.ReadUInt64();
                        GCSCamera camera = new GCSCamera();
                        camera.Name = StringHelpers.ReadString16(reader);
                        camera.Unk0 = reader.ReadInt32();
                        camera.Unk1 = reader.ReadInt32();
                        camera.UnkType = reader.ReadInt32();
                        camera.Padding = reader.ReadBytes(9);
                        camera.Matrix = new Utils.Types.TransformMatrix();
                        camera.Matrix.ReadFromFile(reader);
                        camera.Unk2 = reader.ReadSingle();
                        camera.Unk3 = reader.ReadSingle();
                        camera.Unk4 = reader.ReadSingle();
                    }
                }
            }

            public class SPDData
            {
                private string header;
                private int unk01; //i think this is the same as unk02 in GCSData.
                private int unk02; //possible empty? or unk01 & unk02 are just a long.
                private UnkStruct1[] unkIntData; //size at the start as an int, then three ints following. (possible struct with 3 ints)

                public void ReadFromFile(BinaryReader reader)
                {
                    header = new string(reader.ReadChars(4)); //header
                    unk01 = reader.ReadInt32();
                    unk02 = reader.ReadInt32();

                    int sizeOfInts = reader.ReadInt32();
                    unkIntData = new UnkStruct1[sizeOfInts];

                    for (int i = 0; i != unkIntData.Length; i++)
                    {
                        unkIntData[i] = new UnkStruct1(reader);
                    }
                }

                private struct UnkStruct1
                {
                    private int unk01;
                    private int unk02;
                    private int unk03;
                    private short unk04;

                    public UnkStruct1(BinaryReader reader)
                    {
                        unk01 = reader.ReadInt32();
                        unk02 = reader.ReadInt32();
                        unk03 = reader.ReadInt32();
                        unk04 = reader.ReadInt16();
                    }
                }
            }
        }
    }
}