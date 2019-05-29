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
                spawnModifiers.Add(ReadModifier(reader));

            //Read Continous Modifiers
            int continousModifiersCount = reader.ReadInt32();
            for (int i = 0; i != continousModifiersCount; i++)
                continuousModifiers.Add(ReadModifier(reader));
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(41);
            StringHelpers.WriteString32(writer, "IOFX");
            writer.Write(unk01);
            writer.Write(4096);
            writer.Write(renderMeshes.Length);

            for (int i = 0; i != renderMeshes.Length; i++)
            {
                renderMeshes[i] = new ApexRenderMesh();
                renderMeshes[i].WriteToFile(writer);
            }

            //Read ApexMaterials.
            StringHelpers.WriteString32(writer, "ApexMaterials"); //ApexMaterials
            StringHelpers.WriteString32(writer, renderMaterials);

            //Read Spawn Modifiers
            writer.Write(spawnModifiers.Count);
            for (int i = 0; i != spawnModifiers.Count; i++)
                WriteModifier(writer, spawnModifiers[i]);

            //Read Continous Modifiers
            writer.Write(continuousModifiers.Count);
            for (int i = 0; i != continuousModifiers.Count; i++)
                WriteModifier(writer, continuousModifiers[i]);
        }

        private void WriteModifier(BinaryWriter writer, IModifier modifier)
        {
            string type = "";
            switch(modifier.Type)
            {
                case ModifierType.RandomScale:
                    type = "RandomScaleModifierParams";
                    break;
                case ModifierType.RandomRotation:
                    type = "RandomRotationModifierParams";
                    break;
                case ModifierType.RandomSubtexture:
                    type = "RandomSubtextureModifierParams";
                    break;
                case ModifierType.SimpleScale:
                    type = "SimpleScaleModifierParams";
                    break;
                case ModifierType.Rotation:
                    type = "RotationModifierParams";
                    break;
                case ModifierType.ColorVsLife:
                    type = "ColorVsLifeModifierParams";
                    break;
                case ModifierType.SubtextureVsLife:
                    type = "SubtextureVsLifeModifierParams";
                    break;
                case ModifierType.ScaleVsLife:
                    type = "ScaleVsLifeModifierParams";
                    break;
                case ModifierType.ColorVsDensity:
                    type = "ColorVsDensityModifierParams";
                    break;
                case ModifierType.ScaleAlongVelocity:
                    type = "ScaleAlongVelocityModifierParams";
                    break;
                default:
                    Console.WriteLine("Did not do modifier: {0}", type);
                    break;
            }
            StringHelpers.WriteString32(writer, type);
            modifier.WriteToFile(writer);
        }

        private IModifier ReadModifier(BinaryReader reader)
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

            public void WriteToFile(BinaryWriter writer)
            {
                StringHelpers.WriteString32(writer, "ApexRenderMesh");
                StringHelpers.WriteString32(writer, name);
                writer.Write(weight);
            }
        }
    }
}
