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
	ModelPart::numVertices = count;
}

void ModelPart::SetVertices(Vertex* vertices, unsigned int count) {
	ModelPart::vertices = vertices;
	ModelPart::numVertices = count;
}

void ModelPart::SetSubMeshes(SubMesh* subMeshes, unsigned int count) {
	this->submeshes = subMeshes;
	this->numSubMeshes = count;
}

void ModelPart::SetSubMeshCount(int count) {
	this->numSubMeshes = count;
}

void ModelPart::SetIndicesSize(int count) {
	this->numIndices = count;
}

void ModelPart::SetIndices(std::vector<Int3> indices, unsigned int count) {
	this->indices = indices;
	this->numIndices = count;
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

uint ModelPart::GetVertSize() {
	return this->numVertices;
}

Vertex* ModelPart::GetVertices() {
	return this->vertices;
}

uint ModelPart::GetSubMeshCount() {
	return this->numSubMeshes;
}

uint ModelPart::GetIndicesSize() {
	return this->numIndices;
}

SubMesh* ModelPart::GetSubMeshes() const
{
	return this->submeshes;
}

std::vector<Int3> ModelPart::GetIndices() {
	return this->indices;
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
	fread(&numVertices, sizeof(int), 1, stream);
	vertices = new Vertex[numVertices];
	indices = std::vector<Int3>();

	for (uint i = 0; i < numVertices; i++) {
		Vertex vertex;
		if (hasPosition) {
			fread(&vertex.position, sizeof(Point3), 1, stream);
		}
		if (hasNormals) {
			fread(&vertex.normals, sizeof(Point3), 1, stream);
		}
		if (hasTangents) {
			fread(&vertex.tangent, sizeof(Point3), 1, stream);
		}
		if (hasUV0) {
			fread(&vertex.uv0, sizeof(UVVert), 1, stream);
		}
		if (hasUV1) {
			fread(&vertex.uv1, sizeof(UVVert), 1, stream);
		}
		if (hasUV2) {
			fread(&vertex.uv2, sizeof(UVVert), 1, stream);
		}
		if (hasUV7) {
			fread(&vertex.uv3, sizeof(UVVert), 1, stream);
		}
		vertices[i] = vertex;
	}
	fread(&numSubMeshes, sizeof(int), 1, stream);
	this->submeshes = new SubMesh[numSubMeshes];

	for (uint i = 0; i < this->numSubMeshes; i++) {
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
	fread(&numIndices, sizeof(int), 1, stream);
	for (int x = 0; x != numIndices/3; x++) {
		Int3 tri;
		fread(&tri, sizeof(Int3), 1, stream);
		this->indices.push_back(tri);
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
	fwrite(&numVertices, sizeof(int), 1, stream);

	for (uint i = 0; i < numVertices; i++) {
		if (hasPosition) {
			fwrite(&this->vertices[i].position, sizeof(Point3), 1, stream);
		}
		if (hasNormals) {
			fwrite(&this->vertices[i].normals, sizeof(Point3), 1, stream);
		}
		if (hasTangents) {
			fwrite(&this->vertices[i].tangent, sizeof(Point3), 1, stream);
		}
		if (hasUV0) {
			fwrite(&this->vertices[i].uv0, sizeof(UVVert), 1, stream);
		}
		if (hasUV1) {
			fwrite(&this->vertices[i].uv1, sizeof(UVVert), 1, stream);
		}
		if (hasUV2) {
			fwrite(&this->vertices[i].uv2, sizeof(UVVert), 1, stream);
		}
		if (hasUV7) {
			fwrite(&this->vertices[i].uv3, sizeof(UVVert), 1, stream);
		}
	}
	fwrite(&numSubMeshes, sizeof(int), 1, stream);
	for (uint i = 0; i < numSubMeshes; i++) {
		SubMesh subMesh = this->submeshes[i];
		WriteString(stream, subMesh.GetMatName());
		int startIndex = subMesh.GetStartIndex();
		int numFaces = subMesh.GetNumFaces();
		fwrite(&startIndex, sizeof(int), 1, stream);
		fwrite(&numFaces, sizeof(int), 1, stream);
	}
	int indMult = numIndices * 3;
	fwrite(&indMult, sizeof(int), 1, stream);
	for (uint i = 0; i < numIndices; i++) {
		fwrite(&indices[i], sizeof(Int3), 1, stream);
	}
}

ModelPart::ModelPart() {}
ModelPart::~ModelPart() 
{
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
