using System.IO;
using System.Windows;

namespace FileTypes.XBin.StreamMap.Commands
{
    // This is *NOT* a command. This is a Toolkit Util for opening/saving.
    public static class Command_Factory
    {
        public static ICommand ReadCommand(BinaryReader reader, uint CommandId)
        {
            ICommand Command = null;

            // Read the CommanID and construct the new Command.
            switch(CommandId)
            {
                case 0xB64D9A5D:
                    Command = new Command_CloseTraffic();
                    break;
                case 0x22663242:
                    Command = new Command_LoadSDS();
                    break;
                case 0xA1C05A78:
                    Command = new Command_LoadVehicle();
                    break;
                case 0xEA186B6:
                    Command = new Command_Save();
                    break;
                case 0x72386E2B:
                    Command = new Command_SetPosDirPlayer();
                    break;
                case 0xB40BC168:
                    Command = new Command_Suspend();
                    break;
                case 0xD7C10363:
                    Command = new Command_OpenSlot();
                    break;
                case 0x31247C78:
                    Command = new Command_Barrier();
                    break;
                case 0xD4F4F264:
                    Command = new Command_OpenTraffic();
                    break;
                case 0x687DAD8B:
                    Command = new Command_PlayVideo();
                    break;
                case 0x665E90F2:
                    Command = new Command_WaitForMovie();
                    break;
                case 0x90ACE5D5:
                    Command = new Command_PlayCutscene();
                    break;
                case 0x3B3DD38A:
                    Command = new Command_UnlockVehicle();
                    break;
                case 0xA033FEEB:
                    Command = new Command_UnlockSDS();
                    break;
                case 0x1EFE290F:
                    Command = new Command_If();
                    break;
                case 0x20AE48F2:
                    Command = new Command_EndIf();
                    break;
                default:
                    MessageBox.Show("Detected new command!", "Toolkit");
                    break;
            }

            // Read the Command information.
            Command.ReadFromFile(reader);
            return Command;
        }
    }
}
