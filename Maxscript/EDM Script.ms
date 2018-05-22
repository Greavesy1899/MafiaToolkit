struct Part
(
	name,
	vertCount,
	faceCount,
	verts,
	faces
)	

file = fopen "C:/Users/Connor/source/repos/Mafia2Tools/Mafia2Tools/Mafia2Libs/bin/Debug/exported/hlavni_lod0.edm" "rb"
--------------------------------------------
--	CREATE ROOT
--------------------------------------------

root= Dummy();
root.boxsize=[10,10,10]
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

--------------------------------------------
--	CREATE PARTS
--------------------------------------------
aParts = #()
aParts.count = readlong file

--------------------------------------------
--	LOAD ALL VERTICES
--------------------------------------------
for i = 1 to aParts.count do
(
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
	
	buildTVFaces mesh1
	for i = 1 to mesh1.numfaces do(setTVFace mesh1 i (getFace mesh1 i))
	
)
fclose file
-----------------------------------------
-- OBSOLETE CODE
-----------------------------------------

-- aParts = #()
-- aParts.count = readlong file

-- for i = 1 to aParts.count do
-- (
-- 	count = readbyte file
-- 	sString = ""
-- 	char_array = #()
-- 	char_array.count = count
-- 	for i= 1 to count do
-- 	(
-- 		char_array[i] = readbyte file
-- 		sString += bit.intAsChar char_array[i]
-- 	)	
-- 	
-- 	vCount = readlong file
-- 	vert_array = #()
-- 	vert_array.count = vCount
-- 	
-- 	for i= 1 to vCount  do
-- 	(
-- 		vert = [0, 0, 0]
-- 		vert.x = readfloat file
-- 		vert.y = readfloat file
-- 		vert.z = readfloat file
-- 		vert_array[i] = vert
-- 		i+=2
-- 	)
-- 	
-- 	face_array = #()
-- 	face_array.count = vCount
-- 	
-- 	for i= 1 to vCount  do
-- 	(
-- 		face = [0, 0, 0]
-- 		face.x = readlong file
-- 		face.y = readlong file
-- 		face.z = readlong file
-- 		face_array[i] = face
-- 		i+=2
-- 	)
-- 	
-- 	aParts[i]=Part name:sString vertCount: vCount faceCount: vCount verts: vert_array faces: face_array
-- )

