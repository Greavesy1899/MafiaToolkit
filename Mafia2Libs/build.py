import os

def remove_dll(dllname):
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

#remove libs
remove_dll("Gibbed.IO.dll")
remove_dll("Fbx.dll")
remove_dll("ICSharpCode.SharpZipLib.dll")
remove_dll("Mafia2Libs.dll")
remove_dll("SharpDX.D3DCompiler.dll")
remove_dll("SharpDX.Desktop.dll")
remove_dll("SharpDX.Direct2D1.dll")
remove_dll("SharpDX.Direct3D11.dll")
remove_dll("SharpDX.DirectInput.dll")
remove_dll("SharpDX.dll")
remove_dll("SharpDX.DXGI.dll")
remove_dll("SharpDX.Mathematics.dll")

#remove files
remove_file("Mafia2Libs.pdb")
remove_file("SharpDX.D3DCompiler.pdb")
remove_file("SharpDX.D3DCompiler.xml")
remove_file("SharpDX.Desktop.pdb")
remove_file("SharpDX.Desktop.xml")
remove_file("SharpDX.Direct2D1.pdb")
remove_file("SharpDX.Direct2D1.xml")
remove_file("SharpDX.Direct3D11.pdb")
remove_file("SharpDX.Direct3D11.xml")
remove_file("SharpDX.DirectInput.xml")
remove_file("SharpDX.DXGI.pdb")
remove_file("SharpDX.DXGI.xml")
remove_file("SharpDX.Mathematics.pdb")
remove_file("SharpDX.Mathematics.xml")
remove_file("SharpDX.pdb")
