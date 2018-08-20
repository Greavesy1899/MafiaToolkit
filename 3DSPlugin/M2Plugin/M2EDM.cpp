#include "M2EDM.h"
#include "M2Helpers.h"

//===================================================
//		EDMPART
//===================================================
void EDMPart::SetHasPositions(bool b) {
	EDMPart::hasPosition = b;
}

void EDMPart::SetHasNormals(bool b) {
	EDMPart::hasNormals = b;
}

void EDMPart::SetHasTangents(bool b) {
	EDMPart::hasTangents = b;
}

void EDMPart::SetHasBlendData(bool b) {
	EDMPart::hasBlendData = b;
}

void EDMPart::SetHasFlag0x80(bool b) {
	EDMPart::hasFlag0x80 = b;
}

void EDMPart::SetHasUV0(bool b) {
	EDMPart::hasUV0 = b;
}

void EDMPart::SetHasUV1(bool b) {
	EDMPart::hasUV1 = b;
}

void EDMPart::SetHasUV2(bool b) {
	EDMPart::hasUV2 = b;
}

void EDMPart::SetHasUV7(bool b) {
	EDMPart::hasUV7 = b;
}

void EDMPart::SetHasFlag0x20000(bool b) {
	EDMPart::hasFlag0x20000 = b;
}

void EDMPart::SetHasFlag0x40000(bool b) {
	EDMPart::hasFlag0x40000 = b;
}

void EDMPart::SetHasDamage(bool b) {
	EDMPart::hasDamageGroup = b;
}

void EDMPart::SetVertSize(int count) {
	EDMPart::vertSize = count;
}

void EDMPart::SetVertices(std::vector<Point3> vertices) {
	EDMPart::vertices = vertices;
}

void EDMPart::SetNormals(std::vector<Point3> normals) {
	EDMPart::normals = normals;
}

void EDMPart::SetTangents(std::vector<Point3> tangents) {
	EDMPart::tangents = tangents;
}

void EDMPart::SetUVs(std::vector<UVVert> uvs) {
	EDMPart::uvs = uvs;
}

void EDMPart::SetSubMeshCount(int count) {
	EDMPart::subMeshCount = count;
}

void EDMPart::SetMatNames(std::vector<std::wstring> names) {
	EDMPart::matNames = names;
}

void EDMPart::SetIndicesSize(int count) {
	EDMPart::indicesSize = count;
}

void EDMPart::SetIndices(std::vector<Int3> indices) {
	EDMPart::indices = indices;
}

void EDMPart::SetMatIDs(std::vector<byte> ids) {
	EDMPart::matIDs = ids;
}

void EDMPart::SetMesh(Mesh mesh) {
	EDMPart::mesh = mesh;
}

bool EDMPart::GetHasPositions() {
	return EDMPart::hasPosition;
}

bool EDMPart::GetHasNormals() {
	return EDMPart::hasNormals;
}

bool EDMPart::GetHasTangents() {
	return EDMPart::hasTangents;
}

bool EDMPart::GetHasBlendData() {
	return EDMPart::hasBlendData;
}

bool EDMPart::GetHasFlag0x80() {
	return EDMPart::hasFlag0x80;
}

bool EDMPart::GetHasUV0() {
	return EDMPart::hasUV0;
}

bool EDMPart::GetHasUV1() {
	return EDMPart::hasUV1;
}

bool EDMPart::GetHasUV2() {
	return EDMPart::hasUV2;
}

bool EDMPart::GetHasUV7() {
	return EDMPart::hasUV7;
}

bool EDMPart::GetHasFlag0x20000() {
	return EDMPart::hasFlag0x20000;
}

bool EDMPart::GetHasFlag0x40000() {
	return EDMPart::hasFlag0x40000;
}

bool EDMPart::GetHasDamage() {
	return EDMPart::hasDamageGroup;
}

int EDMPart::GetVertSize() {
	return EDMPart::vertSize;
}

std::vector<Point3> EDMPart::GetVertices() {
	return EDMPart::vertices;
}

std::vector<Point3> EDMPart::GetNormals() {
	return EDMPart::normals;
}

std::vector<Point3> EDMPart::GetTangents() {
	return EDMPart::tangents;
}

std::vector<UVVert> EDMPart::GetUVs() {
	return EDMPart::uvs;
}

int EDMPart::GetSubMeshCount() {
	return EDMPart::subMeshCount;
}

int EDMPart::GetIndicesSize() {
	return EDMPart::indicesSize;
}

std::vector<Int3> EDMPart::GetIndices() {
	return EDMPart::indices;
}

std::vector<std::wstring> EDMPart::GetMatNames() {
	return EDMPart::matNames;
}

std::vector<byte> EDMPart::GetMatIDs() {
	return EDMPart::matIDs;
}

Mesh EDMPart::GetMesh() {
	return EDMPart::mesh;
}

void EDMPart::ReadFromStream(FILE * stream) {
	fread(&hasPosition, sizeof(bool), 1, stream);
	fread(&hasNormals, sizeof(bool), 1, stream);
	fread(&hasTangents, sizeof(bool), 1, stream);
	fread(&hasBlendData, sizeof(bool), 1, stream);
	fread(&hasFlag0x80, sizeof(bool), 1, stream);
	fread(&hasUV0, sizeof(bool), 1, stream);
	fread(&hasUV1, sizeof(bool), 1, stream);
	fread(&hasUV2, sizeof(bool), 1, stream);
	fread(&hasUV7, sizeof(bool), 1, stream);
	fread(&hasFlag0x20000, sizeof(bool), 1, stream);
	fread(&hasFlag0x40000, sizeof(bool), 1, stream);
	fread(&hasDamageGroup, sizeof(bool), 1, stream);
	fread(&vertSize, sizeof(int), 1, stream);
	vertices = std::vector<Point3>(vertSize);
	normals = std::vector<Point3>(vertSize);
	tangents = std::vector<Point3>(vertSize);
	uvs = std::vector<Point3>(vertSize);
	for (int i = 0; i != vertSize; i++) {
		if (hasPosition) {
			fread(&vertices[i].x, sizeof(float), 1, stream);
			fread(&vertices[i].y, sizeof(float), 1, stream);
			fread(&vertices[i].z, sizeof(float), 1, stream);
		}
		if (hasNormals) {
			fread(&normals[i].x, sizeof(float), 1, stream);
			fread(&normals[i].y, sizeof(float), 1, stream);
			fread(&normals[i].z, sizeof(float), 1, stream);
		}
		if (hasTangents) {
			fread(&tangents[i].x, sizeof(float), 1, stream);
			fread(&tangents[i].y, sizeof(float), 1, stream);
			fread(&tangents[i].z, sizeof(float), 1, stream);
		}
		if (hasUV0) {
			fread(&uvs[i].x, sizeof(float), 1, stream);
			fread(&uvs[i].y, sizeof(float), 1, stream);
		}
	}
	fread(&subMeshCount, sizeof(int), 1, stream);
	matNames = std::vector<std::wstring>(subMeshCount);
	for (int i = 0; i != subMeshCount; i++) {
		std::wstring edmName = std::wstring();
		edmName = ReadString(stream, edmName);
		matNames[i] = edmName;
	}
	fread(&indicesSize, sizeof(int), 1, stream);
	indices = std::vector<Int3>(indicesSize);
	matIDs = std::vector<byte>(indicesSize);
	for (int x = 0; x != indicesSize; x++) {
		Int3 tri;
		byte matID;
		fread(&tri.i1, sizeof(short), 1, stream);
		fread(&tri.i2, sizeof(short), 1, stream);
		fread(&tri.i3, sizeof(short), 1, stream);
		fread(&matIDs[x], sizeof(byte), 1, stream);
		indices[x] = tri;
	}

	mesh = Mesh();
	mesh.setNumFaces(indicesSize);
	mesh.setNumVerts(vertSize);
	for (int i = 0; i != mesh.numVerts; i++) {
		mesh.setVert(i, vertices[i]);
	}

	for (int i = 0; i != mesh.numFaces; i++) {
		mesh.faces[i].setVerts(indices[i].i1, indices[i].i2, indices[i].i3);
		mesh.faces[i].setMatID(matIDs[i]);
		mesh.faces[i].setEdgeVisFlags(1, 1, 1);
	}

	if (hasNormals) {
		for (int i = 0; i != mesh.numVerts; i++) {
			mesh.setNormal(i, normals[i]);
		}
		//mesh.SpecifyNormals();
		//MeshNormalSpec *normalSpec = mesh.GetSpecifiedNormals();
		//normalSpec->ClearNormals();
		//normalSpec->SetNumNormals(mesh.numVerts);
		//for (int i = 0; i != mesh.numVerts; i++) {
		//	normalSpec->Normal(i) = normals[i];
		//	normalSpec->SetNormalExplicit(i, true);
		//}
		////I think this piece of code more or less breaks the normals.
		//normalSpec->SetNumFaces(indicesSize);
		//for (int i = 0; i != mesh.numFaces; i++) {
		//	normalSpec->Face(i).SpecifyAll();
		//	normalSpec->Face(i).SetNormalID(0, indices[i].i1);
		//	normalSpec->Face(i).SetNormalID(1, indices[i].i2);
		//	normalSpec->Face(i).SetNormalID(2, indices[i].i3);
		//}
	}

	if (hasUV0)
	{
		mesh.setNumMaps(2);
		mesh.setMapSupport(1, true);
		MeshMap &map = mesh.Map(1);
		map.setNumVerts(vertSize);

		for (int i = 0; i != map.getNumVerts(); i++) {
			map.tv[i].x = uvs[i].x;
			map.tv[i].y = uvs[i].y;
			map.tv[i].z = 0.0f;
		}
		map.setNumFaces(indicesSize);

		for (int i = 0; i != mesh.numFaces; i++) {
			map.tf[i].setTVerts(indices[i].i1, indices[i].i2, indices[i].i3);
		}
	}
	mesh.InvalidateGeomCache();
	mesh.InvalidateTopologyCache();
}

void EDMPart::WriteToStream(FILE * stream) {
	fwrite(&hasPosition, sizeof(bool), 1, stream);
	fwrite(&hasNormals, sizeof(bool), 1, stream);
	fwrite(&hasTangents, sizeof(bool), 1, stream);
	fwrite(&hasBlendData, sizeof(bool), 1, stream);
	fwrite(&hasFlag0x80, sizeof(bool), 1, stream);
	fwrite(&hasUV0, sizeof(bool), 1, stream);
	fwrite(&hasUV1, sizeof(bool), 1, stream);
	fwrite(&hasUV2, sizeof(bool), 1, stream);
	fwrite(&hasUV7, sizeof(bool), 1, stream);
	fwrite(&hasFlag0x20000, sizeof(bool), 1, stream);
	fwrite(&hasFlag0x40000, sizeof(bool), 1, stream);
	fwrite(&hasDamageGroup, sizeof(bool), 1, stream);
	fwrite(&vertSize, sizeof(int), 1, stream);

	for (int i = 0; i != vertSize; i++) {
		if (hasPosition) {
			fwrite(&vertices[i].x, sizeof(float), 1, stream);
			fwrite(&vertices[i].y, sizeof(float), 1, stream);
			fwrite(&vertices[i].z, sizeof(float), 1, stream);
		}
		if (hasNormals) {
			fwrite(&normals[i].x, sizeof(float), 1, stream);
			fwrite(&normals[i].y, sizeof(float), 1, stream);
			fwrite(&normals[i].z, sizeof(float), 1, stream);
		}
		if (hasTangents) {
			fwrite(&tangents[i].x, sizeof(float), 1, stream);
			fwrite(&tangents[i].y, sizeof(float), 1, stream);
			fwrite(&tangents[i].z, sizeof(float), 1, stream);
		}
		if (hasUV0) {
			fwrite(&uvs[i].x, sizeof(float), 1, stream);
			fwrite(&uvs[i].y, sizeof(float), 1, stream);
		}
	}
	fwrite(&subMeshCount, sizeof(int), 1, stream);

	for (int i = 0; i != subMeshCount; i++)
		WriteString(stream, matNames[i]);

	fwrite(&indicesSize, sizeof(int), 1, stream);
	for (int i = 0; i != indices.size(); i++) {
		fwrite(&indices[i].i1, sizeof(short), 1, stream);
		fwrite(&indices[i].i2, sizeof(short), 1, stream);
		fwrite(&indices[i].i3, sizeof(short), 1, stream);
		fwrite(&matIDs[i], sizeof(byte), 1, stream);
	}
}

EDMPart::EDMPart() {}
EDMPart::~EDMPart() {}

//===================================================
//		EDMSTRUCTURE
//===================================================
void EDMStructure::SetName(std::wstring name) {
	EDMStructure::name = name;
}

void EDMStructure::SetPartSize(byte count) {
	EDMStructure::partSize = count;
}

void EDMStructure::SetParts(std::vector<EDMPart> parts) {
	EDMStructure::parts = parts;
}

std::wstring EDMStructure::GetName() {
	return name;
}

byte EDMStructure::GetPartSize() {
	return partSize;
}

std::vector<EDMPart> EDMStructure::GetParts() {
	return parts;
}

void EDMStructure::ReadFromStream(FILE * stream) {
	int header;
	fread(&header, sizeof(int), 1, stream); //header
	std::wstring edmName = std::wstring();
	edmName = ReadString(stream, edmName);
	name = edmName;
	fread(&partSize, 1, 1, stream);
	parts = std::vector<EDMPart>(partSize);
	
	for (int i = 0; i != parts.size(); i++)
		parts[i].ReadFromStream(stream);
	
	fclose(stream);
}

void EDMStructure::WriteToStream(FILE * stream) {
	int header = 542388813;
	fwrite(&header, sizeof(int), 1, stream);
	WriteString(stream, name);
	fwrite(&partSize, sizeof(byte), 1, stream);

	for (int x = 0; x != parts.size(); x++)
		parts[x].WriteToStream(stream);

	fclose(stream);
	
}

EDMStructure::EDMStructure() {}
EDMStructure::~EDMStructure() {}
