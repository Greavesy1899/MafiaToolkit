
# Mafia: Toolkit

[![Build status](https://ci.appveyor.com/api/projects/status/62dtija7vekn7htn/branch/master?svg=true)](https://ci.appveyor.com/project/Greavesy1899/mafia2toolkit)

This Toolkit is a group of programs which aims to achieve better mod support for the 'Mafia' series, developed by 2K Czech and Hangar 13. Mafia 1 (Classic) is not supported. However, the remake is.

## Note:

All features of the Toolkit are always a WIP, and are constantly being improved to improve usability and modability. The list below includes all the current features, which may or may not be complete. 

## Features For All Games:

**Game Explorer:**

Not the best feature in this Toolkit, its primary reason it exists is because it makes it easier to load up different editors and manage SDS files. This feature needs serious improvements to usability however.

**SDS Packing/Repacking:**

This feature uses Gibbed's SDS code from his [repository](https://github.com/gibbed/Gibbed.Illusion), with multiple fixes and improvements. XML files and the games tables are automatically decompiled, with the option to decompile the LUA files. There is an option to unpack SDS files in the format like "SDS Tools GUI", for the people who would like to open the contents in ZModeler3. For repacking, the modder can either choose compressed or uncompressed in the toolkits options. Double clicking any SDS in the game explorer will unpack, and to repack right click the SDS you unpacked, and click "pack". SDS's from Mafia III and Mafia: DE are also supported. However, with Mafia: DE, the Toolkit requires the end-user (you) to copy and paste the 'Oodle' dll from the game folder into the Toolkit's library folder to function. The Toolkit *does not* redistribute this DLL.

**Material Editor:**

This editor can be opened by double-clicking on any file with the ".mtl" extension. They are usually named "default", with some suffix. While this is an old editor, it still has many features available for the end-user. It has the ability to add/remove materials, add parameters or samplers to materials, and even merge to .MTL files giving the option to add new materials and overwrite existing ones. The merging system has been improved to allow the end-user to select which materials should be merged or added to the loaded .MTL, and tries to be very verbose in what is being merged. This editor supports all three versions of the MTL format. 

## Mafia II:

**Map Editor:**

The map editor is the main feature of the Toolkit for Mafia II. The main objective is to allow modders to use this to edit portions of the map. Currently, it has the ability to edit map geometry and their collisions. To load the Map Editor, you can double click on any "FrameResource.fr". Sadly there is a lack of tutorials avaiable for this editor, but some are on the way shortly. I am fully aware that it's difficult from a usability standpoint, but I am trying to improve this. In regards to geometry, the user can change the materials the geometry uses. The geometry can also be modified externally, by being exported, editing in a 3D modelling package, and then reimported. This also supports collisions.

**M2FBX:**

This library is used alongside the map editor to allow modders to export/import Mafia II models, and the ability to "cook" the collision - improving ingame performance and stability. The library is under a heavy rework to allow the editing of skinned objects, and a general refactor of the code.

**Translocator Editor:**

A temporary editor whilst the map editor gets its much needed improvements. But gives the basic usability to add new objects and instances, and also delete them. You can load this editor by double clicking the "Translokator_X.tra" in the city_crash folder.

**Cityareas.bin/CityShops.bin Editor:**

Both separate editors but achieve the same thing - add new city parts and shops into the map. To load up the "cityareas.bin" editor, double click it in the game explorer. Double clicking the "cityshops.bin" file  will open the editor for cityshops.

**Table Editor:**

A basic table editor which allows modders to manipulate data and add new rows. 

## Mafia III & Mafia I: DE:

**XBin Editor:**

An extremely prototype editor, capable of loading & saving certain XBin files. This editor also has the ability to export these XBins to a friendly XML format, which can also be re-imported. This does not support all types of XBin's yet, but support is coming. 

## Building:
Only Windows is supported. Use Visual Studio 2019 to build the GitHub repository. 
For M2FBX, you'll need to have the Fbx SDK and PhysX 2.8.X to compile the solution.

## Known Bugs/Issues
You can check the current issues [here](https://github.com/Greavesy1899/Mafia2Toolkit/issues "Issues").

## Discord link:
To keep upto date on the progress of this toolkit, I recommend joining this discord by using the link [here](http://bit.ly/2L4z8vj "Discord invite")
