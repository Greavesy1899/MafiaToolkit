bl_info = {
    "name":"EDM Importer",
    "category" : "Object"
}

import bpy
import struct

class binary:
    
    def readByte(file):
        (value,) = struct.unpack('b', file.read(1))
        return value

    def readShort(file):
        (value,) = struct.unpack('h', file.read(2))
        return value
    
    def readInt(file):
        (value,) = struct.unpack('i', file.read(4))
        return value

    def readFloat(file):
        (value,) = struct.unpack('f', file.read(4))
        return value

    def readString(file, numChars):
        string = fidin.read(numChars)
        return string


class ImportEDM(bpy.types.Operator):
    """EDM Importer"""
    bl_idname = "edm.importer"
    bl_label = "EDM Importer"
    bl_options = {'REGISTER', 'UNDO'}

    def execute(self, context):

        scene = context.scene

        me = bpy.data.meshes.new("NewMeshMesh")
        ob = bpy.data.objects.new("NewMesh", me)
        scene.objects.link(ob)

        return {'FINISHED'}

def menu_func_import(self, context):
    self.layout.operator(ImportEDM.bl_idname, text="EDM (.edm)")
    
def register():
    bpy.utils.register_class(ImportEDM)
    bpy.types.INFO_MT_file_import.append(menu_func_import)

def unregister():
    bpy.utils.unregister_class(ImportEDM)
    bpy.types.INFO_MT_file_import.remove(menu_func_import)

if __name__ == "__main__":
    register()
