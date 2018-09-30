using Mafia2;
using System.Collections.Generic;
using System.IO;

namespace ApexSDK
{
    public class IOFXFile
    {
        //https://docs.nvidia.com/gameworks/content/gameworkslibrary/physx/apexsdk/_static/build_iofx/classnvidia_1_1apex_1_1RandomScaleModifier.html

        ApexRenderMesh[] renderMeshes;
        string renderMaterials;
        List<IModifier> spawnModifiers;
        List<IModifier> continuousModifiers;

        public void ReadFromFile(BinaryReader reader)
        {
            //check version.
            if (reader.ReadInt32() != 41)
                return;

            //check header.
            if (Functions.ReadString32(reader) != "IOFX")
                return;

            //check unk1.
            if (reader.ReadInt32() != 13)
                return;

            //check unk2.
            if (reader.ReadInt32() != 16)
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
            Functions.ReadString32(reader); //ApexMaterials
            renderMaterials = Functions.ReadString32(reader);


        }

        public struct ApexRenderMesh
        {
            string name;
            int unk1;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public int Unk1 {
                get { return unk1; }
                set { unk1 = value; }
            }

            public void ReadFromFile(BinaryReader reader)
            {
                Functions.ReadString32(reader); //ApexRenderMesh
                name = Functions.ReadString32(reader);
                unk1 = reader.ReadInt32();
            }
        }
    }
}
