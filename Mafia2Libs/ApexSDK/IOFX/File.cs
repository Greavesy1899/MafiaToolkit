using System;
using System.Collections.Generic;
using System.IO;
using Utils.StringHelpers;

namespace ApexSDK
{
    public class IOFxFile
    {
        //docs.nvidia.com/gameworks/content/gameworkslibrary/physx/apexsdk/_static/build_iofx/classnvidia_1_1apex_1_1RandomScaleModifier.html

        private FileInfo file;
        private int unk01;
        ApexRenderMesh[] renderMeshes;
        string renderMaterials;
        List<IModifier> spawnModifiers = new List<IModifier>();
        List<IModifier> continuousModifiers = new List<IModifier>();

        public IOFxFile(FileInfo file)
        {
            this.file = file;
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }            
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Console.WriteLine(string.Format("Checking: {0}", file.Name));

            //check version.
            if (reader.ReadInt32() != 41)
                return;

            //check header.
            if (StringHelpers.ReadString32(reader) != "IOFX")
                return;

            //check unk1.
            //if (reader.ReadInt32() != 13)
            //    return;
            unk01 = reader.ReadInt32();

            //check unk2.
            if (reader.ReadInt32() != 4096)
                return;

            //Read ApexRenderMeshes.
            int numRenderMeshes = reader.ReadInt32();
            renderMeshes = new ApexRenderMesh[numRenderMeshes];

            for(int i = 0; i != renderMeshes.Length; i++)
            {
                renderMeshes[i] = new ApexRenderMesh();
                renderMeshes[i].ReadFromFile(reader);
            }

            //Read ApexMaterials.
            StringHelpers.ReadString32(reader); //ApexMaterials
            renderMaterials = StringHelpers.ReadString32(reader);

            //Read Spawn Modifiers
            int spawnModifiersCount = reader.ReadInt32(); //first count of modifiers.
            for(int i = 0; i != spawnModifiersCount; i++)
                spawnModifiers.Add(DetermineModifier(reader));

            //Read Continous Modifiers
            int continousModifiersCount = reader.ReadInt32();
            for (int i = 0; i != continousModifiersCount; i++)
                continuousModifiers.Add(DetermineModifier(reader));
        }

        private IModifier DetermineModifier(BinaryReader reader)
        {
            IModifier modifier = null;

            string type = StringHelpers.ReadString32(reader);
            switch (type)
            {
                case "RandomScaleModifierParams":
                    modifier = new RandomScaleModifier(reader);
                    break;
                case "RandomRotationModifierParams":
                    modifier = new RandomRotationModifier(reader);
                    break;
                case "RandomSubtextureModifierParams":
                    modifier = new RandomSubtextureModifier(reader);
                    break;
                case "SimpleScaleModifierParams":
                    modifier = new SimpleScaleModifier(reader);
                    break;
                case "RotationModifierParams":
                    modifier = new RotationModifier(reader);
                    break;
                case "ColorVsLifeModifierParams":
                    modifier = new ColorVsLifeModifier(reader);
                    break;
                case "SubtextureVsLifeModifierParams":
                    modifier = new SubtextureVsLifeModifier(reader);
                    break;
                case "ScaleVsLifeModifierParams":
                    modifier = new ScaleVsLifeModifier(reader);
                    break;
                case "ColorVsDensityModifierParams":
                    modifier = new ColorVsDensityModifier(reader);
                    break;
                case "ScaleAlongVelocityModifierParams":
                    modifier = new ScaleAlongVelocityModifier(reader);
                    break;
                default:
                    Console.WriteLine("Did not do modifier: {0}", type);
                    break;
            }

            return modifier;
        }

        public struct ApexRenderMesh
        {
            string name;
            int weight;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public int Weight {
                get { return weight; }
                set { weight = value; }
            }

            public void ReadFromFile(BinaryReader reader)
            {
                StringHelpers.ReadString32(reader); //ApexRenderMesh
                name = StringHelpers.ReadString32(reader);
                weight = reader.ReadInt32();
            }
        }
    }
}
