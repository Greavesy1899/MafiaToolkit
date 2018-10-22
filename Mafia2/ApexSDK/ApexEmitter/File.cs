using Mafia2;
using System;
using System.Collections.Generic;
using System.IO;

namespace ApexSDK
{
    public class EmitterFile
    {
        //docs.nvidia.com/gameworks/content/gameworkslibrary/physx/apexsdk/_static/build_iofx/classnvidia_1_1apex_1_1RandomScaleModifier.html

        private FileInfo file;
        private int Signature = 41;
        private int unk1;
        private float[] unkVectors;
        private string iofxHeader;
        private string iofxName;
        private string nxFluidAssetHeader;
        private string nxFluidAssetName;
        private string emitterTypeName;
        private object emitter;

        public EmitterFile(FileInfo file)
        {
            this.file = file;
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != Signature)
                return;

            if (Functions.ReadString32(reader) != "ApexEmitterAsset")
                return;

            unk1 = reader.ReadInt32();
            unkVectors = new float[20];

            for (int i = 0; i != 20; i++)
                unkVectors[i] = reader.ReadSingle();

            iofxHeader = Functions.ReadString32(reader);
            iofxName = Functions.ReadString32(reader);
            nxFluidAssetHeader = Functions.ReadString32(reader);
            nxFluidAssetName = Functions.ReadString32(reader);
            emitterTypeName = Functions.ReadString32(reader);

            switch(emitterTypeName)
            {
                case "EmitterGeomExplicitParams":
                    emitter = new EmitterGeomExplicit(reader);
                    break;
            }
        }

        private struct NxRange
        {
            private float min;
            private float max;

            public NxRange(BinaryReader reader)
            {
                min = reader.ReadSingle();
                max = reader.ReadSingle();
            }
        }
    }
}
