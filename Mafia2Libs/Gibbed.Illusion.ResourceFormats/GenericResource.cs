using System.IO;
using Gibbed.IO;
using Gibbed.Illusion.FileFormats;
using System.Collections.Generic;
using System.Windows;
using System;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class GenericResource : IResourceType
    {
        readonly Dictionary<ulong, string> TypeExtensionMagic = new Dictionary<ulong, string>()
        {
            { 0x15B770C22,  ".vi.compiled" },
            { 0xA53038C9,  ".flownode" },
            { 0xC3A9C338,  ".dlgsel" },
            { 0x164D0E75C,  ".ires.compiled" },
            { 0xA757FB5364D0E75C,  ".ires.[nomesh].compiled" },
            { 0x222FDF7264D0E75C,  ".ires.[lod0].compiled" },
            { 0x222FDF7364D0E75C,  ".ires.[lod1].compiled" },
            { 0x222FDF7064D0E75C,  ".ires.[lod2].compiled" },
            { 0x1B4347D18,   ".entity.compiled" },
            { 0x4DE17E9B,  ".gbin" },
            { 0x45F07C8B,  ".gxml" },
            { 0x172D9EA8F, ".scene.compiled" },
            { 0x16AD0740B, ".collision.compiled" },
            { 0xA757FB5372D9EA8F,  ".scene.[nomesh].compiled" },
            { 0x222FDF7272D9EA8F,  ".scene.[lod0].compiled" },
            { 0x222FDF7372D9EA8F,  ".scene.[lod1].compiled" },
            { 0x222FDF7072D9EA8F,  ".scene.[lod2].compiled" },
            { 0x1E5CA8123,  ".streaming.compiled" },
            { 0x16024B481,  ".bpdb.compiled" },
            { 0x57572DAC256CA1DB,  ".trb.[global].compiled" },
            { 0x1256CA1DB, ".trb.compiled" },
            { 0xA27F694D, ".iprofai" },
            { 0x4A336D64, ".iproftime" },
            { 0x222FDF72B2F3E413, ".lodbaked.[lod0].compiled" },
            { 0x222FDF73B2F3E413, ".lodbaked.[lod1].compiled" },
            { 0x222FDF70B2F3E413, ".lodbaked.[lod2].compiled" },
            { 0xF3BB3621, ".ccdb" },
            { 0x28930A39, ".egr" },
            { 0x1D4AD8D9E, ".cegr.compiled" },
            { 0x428F61D4, ".fmv.compiled" }, // NOTE: Type is a guess, used in cine_ in M1: DE.
            { 0x118E35C27, ".animprojectreflected.compiled" },
            { 0x73CB32C9, ".effects" }
            //{ 0x45F07C8B, ".scene.gxml"  },
        };

        readonly Dictionary<string, ulong> TypeExtensionString = new Dictionary<string, ulong>()
        {
            { ".vi.compiled", 0x15B770C22 },
            { ".flownode", 0xA53038C9 },
            { ".dlgsel", 0xC3A9C338 },
            { ".ires.compiled", 0x164D0E75C },
            { ".ires.[nomesh].compiled", 0xA757FB5364D0E75C },
            { ".ires.[lod0].compiled", 0x222FDF7264D0E75C },
            { ".ires.[lod1].compiled", 0x222FDF7364D0E75C },
            { ".ires.[lod2].compiled", 0x222FDF7064D0E75C },
            { ".entity.compiled", 0x1B4347D18 },
            { ".gbin", 0x4DE17E9B },
            { ".gxml", 0x45F07C8B },
            { ".scene.compiled", 0x172D9EA8F },
            { ".collision.compiled", 0x16AD0740B },
            { ".scene.[nomesh].compiled", 0xA757FB5372D9EA8F },
            { ".scene.[lod0].compiled", 0x222FDF7272D9EA8F },
            { ".scene.[lod1].compiled", 0x222FDF7372D9EA8F },
            { ".scene.[lod2].compiled", 0x222FDF7072D9EA8F },
            { ".streaming.compiled", 0x1E5CA8123 },
            { ".bpdb.compiled", 0x16024B481 },
            { ".trb.[global].compiled", 0x57572DAC256CA1DB },
            { ".trb.compiled", 0x1256CA1DB },
            { ".iprofai", 0xA27F694D },
            { ".iproftime", 0x4A336D64 },
            { ".fmv", 0x428F61D4 },
            { ".lodbaked.[lod0].compiled", 0x222FDF72B2F3E413 },
            { ".lodbaked.[lod1].compiled", 0x222FDF73B2F3E413 },
            { ".lodbaked.[lod2].compiled", 0x222FDF70B2F3E413 },
            { ".ccdb", 0xF3BB3621 },
            { ".egr", 0x28930A39 },
            { ".cegr.compiled", 0x1D4AD8D9E },
            { ".fmv.compiled", 0x428F61D4 }, // NOTE: Type is a guess, used in cine_ in M1: DE.
            { ".animprojectreflected.compiled",  0x118E35C27},
            { ".effects", 0x73CB32C9 }
            //{ ".scene.gxml", 0x45F07C8B  }
        };


        public ulong GenericType;
        public ushort Unk0;
        public string DebugName;
        public byte[] Data;

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            GenericType = DetermineMagic(DebugName);

            string TempName = string.IsNullOrEmpty(DebugName) ? "" : DebugName;

            output.WriteValueU64(GenericType);
            output.WriteValueU16(Unk0);
            output.WriteStringU16(TempName, endian);
            output.WriteBytes(Data);
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            GenericType = input.ReadValueU64();
            Unk0 = input.ReadValueU16();
            DebugName = input.ReadStringU16(endian);

            string Message = string.Format("{0} {1}", DebugName, GenericType);
            Console.WriteLine(Message);

            // We do not have any size so we do (FILE_LENGTH - CURRENT_POS);
            Data = input.ReadBytes((int)(input.Length - input.Position));
        }

        public ulong DetermineMagic(string name)
        {
            string extension = GetFullExtensionUtil(name);
            Console.WriteLine(extension);
            ulong magic = 0;

            bool bHasFound = TypeExtensionString.ContainsKey(extension);

            if(!bHasFound)
            {
                bHasFound = RecursiveExtensionCheck(ref extension);
            }

            if(bHasFound)
            {
                magic = TypeExtensionString[extension];
            }
            else
            {
                MessageBox.Show("Detected an unknown extension!!! SDS will NOT work!", "Toolkit");
            }

            return magic;
        }

        public string DetermineName(string name)
        {
            bool bGotDebugName = false;

            // Make sure we use the debug name.
            if (!string.IsNullOrEmpty(DebugName))
            {
                name = DebugName;
                bGotDebugName = true;
            }

            // Our database tool has figured out this file name.
            // Return.
            // TODO: Consider an easier approach for this, maybe have a flag?
            if (!name.Contains("File_") && !bGotDebugName)
            {
                string extension = GetFullExtensionUtil(name);
                if(!TypeExtensionString.ContainsKey(extension))
                {
                    MessageBox.Show("Detected missing extension from DB. Please contract Greavesy with SDS name.", "Toolkit");
                }
                return name;
            }

            if (!bGotDebugName)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(name);

                if(TypeExtensionMagic.ContainsKey(GenericType))
                {
                    string extension = TypeExtensionMagic[GenericType];
                    withoutExtension += extension;
                }
                else 
                {
                    withoutExtension += ".genr";
                    MessageBox.Show("Detected an unknown GENR type. Please contract Greavesy with SDS name.", "Toolkit");
                }

                name = withoutExtension;
            }

            return name;
        }

        private bool RecursiveExtensionCheck(ref string Extension)
        {
            while(Extension.LastIndexOf('.') != 0)
            {
                string RemovedDot = Extension.Remove(0, 1);
                Extension = GetFullExtensionUtil(RemovedDot);

                bool bHasFound = TypeExtensionString.ContainsKey(Extension);
                if(bHasFound)
                {
                    return true;
                }
            }

            return false;
        }

        private string GetFullExtensionUtil(string FileName)
        {
            int extensionStart = FileName.IndexOf(".");
            return FileName.Substring(extensionStart);
        }
    }
}
