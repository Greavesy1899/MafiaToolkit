import os
import zipfile

def move_dll(dllname):
    print("Moving " + dllname + " to libs..")
    exists = os.path.isfile(dllname)
    exists_in_libs = os.path.isfile("libs/"+dllname)
    if exists and not exists_in_libs:
        os.rename(dllname, "libs/"+dllname)
        print("Moved " + dllname + ".")   
    elif exists and exists_in_libs:
        os.remove("libs/"+dllname)
        os.rename(dllname, "libs/"+dllname)
        print("Deleted old libs and moved new libs.")            
    else:
        print("Did not find " + dllname + " did not move to libs.")
    return;

def move_from_libs(dllname):
    print("Moving " + dllname + " from libs..")
    exists = os.path.isfile("libs/"+dllname)
    exists_in_root = os.path.isfile(dllname)
    if exists and not exists_in_root:
        os.rename("libs/"+dllname, dllname)
        print("Moved " + dllname + ".")   
    elif exists and exists_in_root:
        os.remove(dllname)
        os.rename("libs/"+dllname, dllname)      
    else:
        print("Did not find " + dllname + " did not move to libs.")
    return;

def remove_file(filename):
    print("Removing " + filename + ".")
    exists = os.path.isfile(filename)
    if exists:
        os.remove(filename)
        print("Deleted " + filename + ".")   
    else:
        print("Did not find " + filename + ".")
    return;

print("Building Toolkit for release..")

# remove libs
move_dll("discord-rpc.dll")
move_dll("M2FBX.dll")
move_dll("M2PhysX.dll")
#move_dll("OodleSharp.dll")
#move_dll("Gibbed.IO.dll")
#move_dll("SharpGen.Runtime.COM.dll")
#move_dll("SharpGen.Runtime.dll")
#move_dll("Vortice.D3DCompiler.dll")
#move_dll("Vortice.Direct3D11.dll")
#move_dll("Vortice.XInput.dll")
#move_dll("Vortice.Mathematics.dll")
#move_dll("Vortice.DXGI.dll")
#move_dll("Vortice.DirectX.dll")
#move_dll("WeifenLuo.WinFormsUI.Docking.dll")
#move_dll("WeifenLuo.WinFormsUI.Docking.ThemeVS2015.dll")
#move_dll("WinFormsUI.dll")
#move_dll("UnluacNET.dll");
#move_dll("BitStreams.dll");
#move_dll("Gibbed.Squish.dll");
#move_dll("squish_32.dll");
#move_dll("squish_64.dll");
#move_dll("Octokit.dll");
#move_dll("zlibnet.dll");
#move_dll("OodleSharp.dll");
#move_dll("oo2core_8_win64.dll");

# remove files
remove_file("MafiaToolkit.pdb")
remove_file("OodleSharp.pdb")

move_from_libs("Gibbed.IO.dll")
move_from_libs("Gibbed.Squish.dll")
move_from_libs("squish_32.dll")
move_from_libs("squish_64.dll")
move_from_libs("zlibnet.dll")
move_from_libs("Octokit.dll")
move_from_libs("OodleSharp.dll")
move_from_libs("UnluacNET.dll")