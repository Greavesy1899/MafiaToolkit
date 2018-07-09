# Mafia 2: Toolkit

This toolkit is a group of programs a plugin for 3DS max and the library, which allows modders to build their own programs. 


This includes the following;
- Library to build your own programs
- Programs to read the "Material Libraries" and other file types which are included in the "SDS" archives.
- 3DS plugin to make life easier when exporting and modifying models.

This library has the ability to parse the following file types.
- FrameNameTable.bin
- FrameResource.bin
- Collisions_0.bin
- ItemDesc.bin
- defaultXX.mtl
- VertexBufferPool.bin
- IndexBufferPool.bin
- (There might be more, but these are the most important)

It also packages functions to export 3D data to obj/mtl filetypes, or a custom file type, "EDM" and "EDD" built for this tool and allows easy importing and exporting in 3DS Max. This file type is actually really nice; you can export the model from the program, modify and then inject it into the buffers of Mafia 2. 

EDD = Used to view singular meshes. Contains mesh data.

EDM = Used to build the entire scene. Contains name of mesh, position and rotation.

ARA = AREA editing. Very experimental currently.

(For the structure of both of these files, please visit the Wiki.)

# Discord link:

To keep upto date on the progress of this toolkit, I recommend joining this discord by using the link [here](http://bit.ly/2L4z8vj "Discord invite")
