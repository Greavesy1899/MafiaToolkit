bl_info = {
    "name":"EDM Importer",
    "category" : "Object",
    "description": "Import EDM files into blender"}
        
import bpy
import struct
from bpy.props import StringProperty
from bpy_extras.io_utils import ImportHelper

class ImportEDM(bpy.types.Operator, ImportHelper):
    """EDM Importer"""
    bl_idname = "import_object.edm"
    bl_label = "Import EDM"
    bl_options = {'REGISTER', 'UNDO'}
    
    filename_ext = ".edm"
    filter_glob = StringProperty(default="*.edm", options={'HIDDEN'})

    def execute(self, context):
        self.report({'INFO'}, "Begin importing edm..")
        return loadEDM(self, context)

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
        
#BINARY HELPERS
#=============================
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
    string = file.read(numChars)
    return string.decode("utf-8")

#EDM OBJECT
#=================================
class edmObject(object):
    def __init__( self ) :
        self.name = ""
        self.partCount = 0
        self.parts = []
        return
    
    def readfile(self, file):
        numChars = readByte(file)
        self.name = readString(file, numChars)
        self.partCount = readInt(file)
        for i in range( self.partCount ) :
            part = edmPart()
            part.readfile(file)
            self.parts.append(part)

#EDM PART
#=================================
class edmPart(object):
    def __init__( self ) :
        self.name = ""
        self.vertCount = 0
        self.verts = []
        self.uvCount = 0;
        self.uvs = []
        self.facesCount = 0;
        self.faces = []
        return

    def readfile(self, file):
        numChars = readByte(file)
        self.name = readString(file, numChars)
        
        self.vertCount = readInt(file)
        for i in range(self.vertCount):
            vert = (readFloat(file), readFloat(file), readFloat(file))
            self.verts.append(vert)

        self.uvCount = readInt(file)
        for i in range(self.uvCount):
            uv = (readFloat(file), readFloat(file))
            self.uvs.append(uv)

        self.facesCount = readInt(file);
        for i in range(self.facesCount):
            face = (readInt(file)-1, readInt(file)-1, readInt(file)-1)
            self.faces.append(face)
            
#BEGIN LOADING AND PARSING
#==============================
def loadEDM(operator, context):
    scene = context.scene
    filepath = operator.properties.filepath
    
    path = "Importing: " + filepath
    operator.report({'INFO'}, path)
    parseEDM(filepath);
    
    return {'FINISHED'}

def parseEDM(filepath):
    scene = bpy.context.scene
    file = open(filepath, 'rb')
    edmMesh = edmObject()
    edmMesh.readfile(file)

    for i in range(edmMesh.partCount):
        me = bpy.data.meshes.new(edmMesh.parts[i].name)
        ob = bpy.data.objects.new(edmMesh.parts[i].name + "_mesh", me)
        scene.objects.link(ob)
        me.from_pydata(edmMesh.parts[i].verts, [], edmMesh.parts[i].faces)
        me.update(calc_edges=True)
    
    file.close()
