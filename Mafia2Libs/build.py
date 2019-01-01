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
remove_dll("ICSharpCode.SharpZipLib.dll")
remove_dll("Mafia2Libs.dll")
remove_dll("Gibbed.IO.dll");
remove_dll("Octokit.dll");

#remove files
remove_file("ToolkitUpdater.pdb")
remove_file("Octokit.pdb")
remove_file("Octokit.xml")
remove_file("Mafia2Libs.pdb")
remove_file("Mafia2Toolkit.pdb")
remove_file("Gibbed.IO.pdb");
