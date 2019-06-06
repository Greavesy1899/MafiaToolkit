#include "M2Model.h"
#include "Utilities.h"
#include <algorithm>

//===================================================
//		ModelPart
//===================================================
void ModelPart::SetHasPositions(bool b) {
	ModelPart::hasPosition = b;
}

void ModelPart::SetHasNormals(bool b) {
	ModelPart::hasNormals = b;
}

void ModelPart::SetHasTangents(bool b) {
	ModelPart::hasTangents = b;
}

void ModelPart::SetHasBlendData(bool b) {
	ModelPart::hasBlendData = b;
}

void ModelPart::SetHasFlag0x80(bool b) {
	ModelPart::hasFlag0x80 = b;
}

void ModelPart::SetHasUV0(bool b) {
	ModelPart::hasUV0 = b;
}

void ModelPart::SetHasUV1(bool b) {
	ModelPart::hasUV1 = b;
}

void ModelPart::SetHasUV2(bool b) {
	ModelPart::hasUV2 = b;
}

void ModelPart::SetHasUV7(bool b) {
	ModelPart::hasUV7 = b;
}

void ModelPart::SetHasFlag0x20000(bool b) {
	ModelPart::hasFlag0x20000 = b;
}

void ModelPart::SetHasFlag0x40000(bool b) {
	ModelPart::hasFlag0x40000 = b;
}

void ModelPart::SetHasDamage(bool b) {
	ModelPart::hasDamageGroup = b;
}

void ModelPart::SetVertSize(int count) {
	ModelPart::vertSize = count;
}

void ModelPart::SetVertices(std::vector<Point3> vertices, bool updateCount) {
	ModelPart::vertices = vertices;

	if(updateCount)
		ModelPart::vertSize = vertices.size();
}

void ModelPart::SetNormals(std::vector<Point3> normals) {
	ModelPart::normals = normals;
}

void ModelPart::SetTangents(std::vector<Point3> tangents) {
	ModelPart::tangents = tangents;
}

void ModelPart::SetUV0s(std::vector<UVVert> uvs) {
	ModelPart::uvs0 = uvs;
}

void ModelPart::SetUV1s(std::vector<UVVert> uvs) {
	ModelPart::uvs1 = uvs;
}

void ModelPart::SetUV2s(std::vector<UVVert> uvs) {
	ModelPart::uvs2 = uvs;
}

void ModelPart::SetUV7s(std::vector<UVVert> uvs) {
	ModelPart::uvs7 = uvs;
}

void ModelPart::SetSubMeshes(SubMesh* subMeshes)
{
	this->submeshes = subMeshes;
}

void ModelPart::SetSubMeshCount(int count) {
	ModelPart::subMeshCount = count;
}

void ModelPart::SetIndicesSize(int count) {
	ModelPart::indicesSize = count;
}

void ModelPart::SetIndices(std::vector<Int3> indices, bool updateCount) {
	ModelPart::indices = indices;

	if (updateCount)
		ModelPart::indicesSize = indices.size();
}

bool ModelPart::GetHasPositions() {
	return ModelPart::hasPosition;
}

bool ModelPart::GetHasNormals() {
	return ModelPart::hasNormals;
}

bool ModelPart::GetHasTangents() {
	return ModelPart::hasTangents;
}

bool ModelPart::GetHasBlendData() {
	return ModelPart::hasBlendData;
}

bool ModelPart::GetHasFlag0x80() {
	return ModelPart::hasFlag0x80;
}

bool ModelPart::GetHasUV0() {
	return ModelPart::hasUV0;
}

bool ModelPart::GetHasUV1() {
	return ModelPart::hasUV1;
}

bool ModelPart::GetHasUV2() {
	return ModelPart::hasUV2;
}

bool ModelPart::GetHasUV7() {
	return ModelPart::hasUV7;
}

bool ModelPart::GetHasFlag0x20000() {
	return ModelPart::hasFlag0x20000;
}

bool ModelPart::GetHasFlag0x40000() {
	return ModelPart::hasFlag0x40000;
}

bool ModelPart::GetHasDamage() {
	return ModelPart::hasDamageGroup;
}

int ModelPart::GetVertSize() {
	return ModelPart::vertSize;
}

std::vector<Point3> ModelPart::GetVertices() {
	return ModelPart::vertices;
}

std::vector<Point3> ModelPart::GetNormals() {
	return ModelPart::normals;
}

std::vector<Point3> ModelPart::GetTangents() {
	return ModelPart::tangents;
}

std::vector<UVVert> ModelPart::GetUV0s() {
	return ModelPart::uvs0;
}

std::vector<UVVert> ModelPart::GetUV1s() {
	return ModelPart::uvs1;
}

std::vector<UVVert> ModelPart::GetUV2s() {
	return ModelPart::uvs2;
}

std::vector<UVVert> ModelPart::GetUV7s() {
	return ModelPart::uvs7;
}

int ModelPart::GetSubMeshCount() {
	return ModelPart::subMeshCount;
}

int ModelPart::GetIndicesSize() {
	return ModelPart::indicesSize;
}

SubMesh* ModelPart::GetSubMeshes() const
{
	return this->submeshes;
}

std::vector<Int3> ModelPart::GetIndices() {
	return ModelPart::indices;
}

void ModelPart::ReadFromStream(FILE * stream) {
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

	if(hasNormals)
		normals = std::vector<Point3>(vertSize);

	if(hasTangents)
		tangents = std::vector<Point3>(vertSize);

	if(hasUV0)
		uvs0 = std::vector<UVVert>(vertSize);

	if(hasUV1)
		uvs1 = std::vector<UVVert>(vertSize);

	if (hasUV2)
		uvs2 = std::vector<UVVert>(vertSize);

	if (hasUV7)
		uvs7 = std::vector<UVVert>(vertSize);

	for (int i = 0; i != vertSize; i++) {
		if (hasPosition) {
			fread(&vertices[i], sizeof(Point3), 1, stream);
		}
		if (hasNormals) {
			fread(&normals[i], sizeof(Point3), 1, stream);
		}
		if (hasTangents) {
			fread(&tangents[i], sizeof(Point3), 1, stream);
		}
		if (hasUV0) {
			fread(&uvs0[i], sizeof(UVVert), 1, stream);
		}
		if (hasUV1) {
			fread(&uvs1[i], sizeof(UVVert), 1, stream);
		}
		if (hasUV2) {
			fread(&uvs2[i], sizeof(UVVert), 1, stream);
		}
		if (hasUV7) {
			fread(&uvs7[i], sizeof(UVVert), 1, stream);
		}
	}
	fread(&subMeshCount, sizeof(int), 1, stream);
	long pos = ftell(stream);
	this->submeshes = new SubMesh[subMeshCount];

	for (int i = 0; i != subMeshCount; i++) {
		SubMesh subMesh = SubMesh();
		std::string name = std::string();
		int startIndex, numFaces;

		name = ReadString(stream, name);
		subMesh.SetMatName(name);
		fread(&startIndex, sizeof(int), 1, stream);
		fread(&numFaces, sizeof(int), 1, stream);
		subMesh.SetStartIndex(startIndex);
		subMesh.SetNumFaces(numFaces);
		this->submeshes[i] = subMesh;
	}
	fread(&indicesSize, sizeof(int), 1, stream);
	for (int x = 0; x != indicesSize/3; x++) {
		Int3 tri;
		fread(&tri, sizeof(Int3), 1, stream);
		indices.push_back(tri);
	}
}

void ModelPart::WriteToStream(FILE * stream) {
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
			fwrite(&vertices[i], sizeof(Point3), 1, stream);
		}
		if (hasNormals) {
			fwrite(&normals[i], sizeof(Point3), 1, stream);
		}
		if (hasTangents) {
			fwrite(&tangents[i], sizeof(Point3), 1, stream);
		}
		if (hasUV0) {
			fwrite(&uvs0[i], sizeof(UVVert), 1, stream);
		}
		if (hasUV1) {
			fwrite(&uvs1[i], sizeof(UVVert), 1, stream);
		}
		if (hasUV2) {
			fwrite(&uvs2[i], sizeof(UVVert), 1, stream);
		}
		if (hasUV7) {
			fwrite(&uvs7[i], sizeof(UVVert), 1, stream);
		}
	}
	fwrite(&subMeshCount, sizeof(int), 1, stream);
	for (int i = 0; i != subMeshCount; i++) {
		SubMesh subMesh = this->submeshes[i];
		WriteString(stream, subMesh.GetMatName());
		int startIndex = subMesh.GetStartIndex();
		int numFaces = subMesh.GetNumFaces();
		fwrite(&startIndex, sizeof(int), 1, stream);
		fwrite(&numFaces, sizeof(int), 1, stream);
	}
	int indMult = indicesSize * 3;
	fwrite(&indMult, sizeof(int), 1, stream);
	for (int i = 0; i != indices.size(); i++) {
		fwrite(&indices[i], sizeof(Int3), 1, stream);
	}
}

ModelPart::ModelPart() {}
ModelPart::~ModelPart() 
{
	//delete[] this->submeshes;
}

//===================================================
//		ModelStructure
//===================================================
void ModelStructure::SetName(std::string name) {
	name.erase(std::remove(name.begin(), name.end(), '?'), name.end());
	ModelStructure::name = name;
}

void ModelStructure::SetPartSize(char& count) {
	ModelStructure::partSize = count;
}

void ModelStructure::SetParts(ModelPart* parts) {
	ModelStructure::parts = parts;
}

std::string ModelStructure::GetName() const {
	return name;
}

char ModelStructure::GetPartSize() const {
	return partSize;
}

ModelPart* ModelStructure::GetParts() const {
	return parts;
}

void ModelStructure::ReadFromStream(FILE * stream) {
	int header;
	fread(&header, sizeof(int), 1, stream); //header

	if (header != magic)
		exit(0);

	this->name = ReadString(stream, this->name);
	fread(&this->partSize, sizeof(char), 1, stream);
	this->parts = new ModelPart[this->partSize];

	for (int i = 0; i != this->partSize; i++)
	{
		ModelPart part = ModelPart();
		part.ReadFromStream(stream);
		this->parts[i] = part;
	}
		
	fclose(stream);
}

void ModelStructure::WriteToStream(FILE * stream) {
	fwrite(&magic, sizeof(int), 1, stream);
	WriteString(stream, this->name);
	fwrite(&this->partSize, sizeof(char), 1, stream);

	for (int x = 0; x != this->partSize; x++)
		this->parts[x].WriteToStream(stream);

	fclose(stream);
}

ModelStructure::ModelStructure() {}
ModelStructure::~ModelStructure()
{
	//delete[] this->parts;
}

void SubMesh::SetStartIndex(int& value)
{
	this->startIndex = value;
}

void SubMesh::SetNumFaces(int& value)
{
	this->numFaces = value;
}

void SubMesh::SetMatName(std::string name)
{
	this->matName = name;
}

int SubMesh::GetStartIndex() const
{
	return this->startIndex;
}

int SubMesh::GetNumFaces() const
{
	return this->numFaces;
}

std::string SubMesh::GetMatName() const
{
	return this->matName;
}
