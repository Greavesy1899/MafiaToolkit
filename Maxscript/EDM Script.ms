global edmLastFile = ""
global eddLastFile = ""
global edcLastFile = ""
global edmPath = ""
global aMeshes = #()

struct Part
(
	mesh
)	
struct EddStruct
(
	fileName,
	pos,
	rot
)

fn importFrame =
(
	eddFile = fopen eddLastFile "rb"
	files = #()
	files.count = readlong eddFile
	
	for i = 1 to files.count do
	(
		count = readbyte eddFile

		fa = ""
		char_array = #()
		char_array.count = count
		for i= 1 to count do
		(
			char_array[i] = readbyte eddFile
			fa += bit.intAsChar char_array[i]
		)
	
		pos = [0,0,0]
		pos.x = readfloat eddFile
		pos.y = readfloat eddFile
		pos.z = readfloat eddFile
		
		rot = [0,0,0] 

		rot.x = readfloat eddFile
		rot.y = readfloat eddFile
		rot.z = readfloat eddFile
		
		rot2 = eulerAngles rot.x rot.y rot.z		
		files[i] = EddStruct fileName: fa pos: pos rot:rot2
	)
	fclose eddFile
	
	for i = 1 to files.count do
	(
		edmLastFile = (edmPath +"\\" + files[i].fileName + ".edm")
		importMesh()
	)
	for i = 1 to files.count do
	(
		aMeshes[i].pos = files[i].pos
		
		matrix = aMeshes[i].transform
		preRotate matrix (eulertoquat(files[i].rot))
		aMeshes[i].transform = matrix
	)
	
)
fn importMesh =
(
	file = fopen edmLastFile "rb"
	
	root= Dummy();
	root.boxsize=[1,1,1]
	count = readbyte file
	sString = ""
	char_array = #()
	char_array.count = count
	for i= 1 to count do
	(
		char_array[i] = readbyte file
		sString += bit.intAsChar char_array[i]
	)

	root.name = sString

	aParts = #()
	aParts.count = readlong file

	for i = 1 to aParts.count do
	(
		count = readbyte file
		meshString = ""
		char_array = #()
		char_array.count = count
		for i= 1 to count do
		(
			char_array[i] = readbyte file
			meshString += bit.intAsChar char_array[i]
		)

		vertarray = #()
		vertarray.count = readlong file	
	
		for i = 1 to vertarray.count do
		(
			vert = [0,0,0]
			vert.x = readfloat file
			vert.y = readfloat file
			vert.z = readfloat file
			vertarray[i] = vert
			i+=2
		)
	
		tvertarray = #()
		tvertarray.count = readlong file
	
		for i = 1 to tvertarray.count do
		(
			tvert = [0,0,0]
			tvert.x = readfloat file
			tvert.y = readfloat file
			tvertarray[i] = tvert
			i+=1
		)
	
		facearray = #()
		facearray.count = readlong file	
	
		for i = 1 to facearray.count do
		(
			face = [0,0,0]
			face.x = readlong file
			face.y = readlong file
			face.z = readlong file
			facearray[i] = face
			i+=2
		)
	
		mesh1 = mesh vertices:vertarray tverts:tvertarray faces:facearray
		mesh1.name = meshString
		mesh1.parent = root
		
		if getNumTVerts mesh1 > 0 then
			buildTVFaces mesh1
		
		if tvertarray.count  != 0 then
			for i = 1 to mesh1.numfaces do(setTVFace mesh1 i (getFace mesh1 i))
			
		mat = standardMaterial()
		mat.diffusemap = bitmapTexture()
		mat.name = mesh1.name
		mat.Maps[2] = Bitmaptexture fileName:(edmPath + "\\textures\\" + mesh1.name)
	
		mesh1.material = mat
			
		aParts[i]=Part mesh: mesh1
	)
	append aMeshes root
	fclose file
)
fn importCollision =
(
	file = fopen edcLastFile "rb"
	
	colMesh = undefined

	count = readbyte file
	sString = ""
	char_array = #()
	char_array.count = count
	for i= 1 to count do
	(
		char_array[i] = readbyte file
		sString += bit.intAsChar char_array[i]
	)

	colName = sString
	print(colName)
	pos = [0,0,0]
	pos.x = readFloat file
	pos.y = readFloat file
	pos.z = readFloat file
	rot= [0,0,0]
	rot.x = readFloat file
	rot.y = readFloat file
	rot.z = readFloat file
	
	colType = readByte file
	
	if colType == 1 then
	(
		val1 = readfloat file
		val2 = readfloat file
		val3 = readfloat file
		colMesh = box length:(val2*2) width:(val1*2) height:(val3*2)
	)
	if colType == 2 then
	(
		val1 = readfloat file
		colMesh = sphere radius:val1
	)
	if colType == 3 then
	(
		val1 = readfloat file
		val2 = readfloat file
		colMesh = Capsule radius:(val1) height:(val2)
	)
	
	colMesh.name = colName
	
	fclose file
)

utility EDMImport "EDM Importer"
(
	group "Import"
	(
		button importer "Import Single Mesh"
		button fimporter "Import Entire Folder"
		button cimporter "Import Single Collision"
	)
	
	group "About"
	(
		label title "Mafia 2 Editables tool"
		label desc "Imports EDM/EDC/EDD files"
	)
	
	on importer pressed do
	(
		edmFile = getopenfilename caption:"Select File" filename: edmLastFile types:"edm (*.edm)"
		
		if edmFile != undefined then
		(
			edmLastFile = edmFile
			importMesh()
		)
	)
	on cimporter pressed do
	(
		edcFile = getopenfilename caption:"Select File" filename: edmLastFile types:"edc (*.edc)"
		
		if edcFile != undefined then
		(
			edcLastFile = edcFile
			importCollision()
		)
	)
	on fimporter pressed do
	(
		edmPath = getSavePath caption:"Select Folder" initialDir:"C:\\Users\\Connor\\source\\repos\\Mafia2Tools\\Mafia2Tools\\Mafia2Libs\\bin\\Debug\\exported"
		
		--don't know if break, returns, etc exist in this language. 
		if edmPath != undefined then
		(
			aMeshes = #()
			files = getFiles (edmpath+"\\*.edd")
			
			if files.count > 0 then
			(
				eddLastFile = files[1]
				importFrame()
			)
		)

	)
)
