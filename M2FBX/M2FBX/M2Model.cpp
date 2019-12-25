#include "M2Model.h"
#include "Utilities.h"
#include <algorithm>

//===================================================
//		ModelPart
//===================================================
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

bool ModelPart::HasVertexFlag(VertexFlags flag)
{
	return (this->flags & flag);
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

//Version 1. Hacky but support is needed for older versions.
void ModelPart::ReadFromStream(FILE * stream) {
	bool hasPosition, hasNormals, hasTangents, hasBlendData, hasFlag0x80,
		hasUV0, hasUV1, hasUV2, hasUV7, hasFlag0x20000, hasFlag0x40000,
		hasDamageGroup;
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

void ModelPart::ReadFromStream2(FILE* stream)
{
	fread(&this->flags, sizeof(int), 1, stream);
	fread(&numVertices, sizeof(int), 1, stream);
	vertices = new Vertex[numVertices];
	indices = std::vector<Int3>();

	for (uint i = 0; i < numVertices; i++) {
		Vertex vertex;
		if (HasVertexFlag(VertexFlags::Position)) {
			fread(&vertex.position, sizeof(Point3), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::Normals)) {
			fread(&vertex.normals, sizeof(Point3), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::Tangent)) {
			fread(&vertex.tangent, sizeof(Point3), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::Skin)) {
			fread(&vertex.boneIDs, sizeof(byte), 4, stream);
			fread(&vertex.boneWeights, sizeof(float), 4, stream);
		}
		if (HasVertexFlag(VertexFlags::TexCoords0)) {
			fread(&vertex.uv0, sizeof(UVVert), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::TexCoords1)) {
			fread(&vertex.uv1, sizeof(UVVert), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::TexCoords2)) {
			fread(&vertex.uv2, sizeof(UVVert), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::ShadowTexture)) {
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
	for (int x = 0; x != numIndices / 3; x++) {
		Int3 tri;
		fread(&tri, sizeof(Int3), 1, stream);
		this->indices.push_back(tri);
	}
}

void ModelPart::WriteToStream(FILE * stream) {
	fwrite(&this->flags, sizeof(int), 1, stream);
	fwrite(&numVertices, sizeof(int), 1, stream);

	for (uint i = 0; i < numVertices; i++) {
		if (HasVertexFlag(VertexFlags::Position)) {
			fwrite(&this->vertices[i].position, sizeof(Point3), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::Normals)) {
			fwrite(&this->vertices[i].normals, sizeof(Point3), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::Tangent)) {
			fwrite(&this->vertices[i].tangent, sizeof(Point3), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::Skin)) {
			fwrite(&this->vertices[i].boneIDs, sizeof(int), 1, stream);
			fwrite(&this->vertices[i].boneWeights, sizeof(float), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::TexCoords0)) {
			fwrite(&this->vertices[i].uv0, sizeof(UVVert), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::TexCoords1)) {
			fwrite(&this->vertices[i].uv1, sizeof(UVVert), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::TexCoords2)) {
			fwrite(&this->vertices[i].uv2, sizeof(UVVert), 1, stream);
		}
		if (HasVertexFlag(VertexFlags::ShadowTexture)) {
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

void ModelPart::SetVertexFlag(VertexFlags flag)
{
	this->flags = (VertexFlags)(this->flags | flag);
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

void ModelStructure::SetBoneNames(std::vector<std::string>& names)
{
	this->boneNames = names;
}

void ModelStructure::SetBoneIDs(std::vector<byte>& boneIDs)
{
	this->boneIDs = boneIDs;
}

void ModelStructure::SetIsSkinned(bool skinned)
{
	this->isSkinned = skinned;
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

std::vector<std::string>& ModelStructure::GetBoneNames()
{
	return boneNames;
}

std::vector<byte>& ModelStructure::GetBoneIDs()
{
	return boneIDs;
}

bool ModelStructure::GetIsSkinned()
{
	return this->isSkinned;
}

void ModelStructure::ReadFromStream(FILE * stream) {
	int header, version;
	fread(&header, sizeof(int), 1, stream); //header

	if (header == magicVersion1)
	{
		version = 1;
	}
	else if (header == magicVersion2)
	{
		version = 2;
	}
	else
	{
		exit(0);
	}

	this->name = ReadString(stream, this->name);
	if (version == 2)
	{
		fread(&this->isSkinned, sizeof(byte), 1, stream);

		if (isSkinned)
		{
			byte numBones;
			fread(&numBones, sizeof(byte), 1, stream);
			std::vector<std::string> names = std::vector<std::string>();
			std::vector<byte> parents = std::vector<byte>();
			for (int i = 0; i < numBones; i++)
			{
				byte id;
				std::string boneName;
				boneName = ReadString(stream, boneName);
				fread(&id, sizeof(byte), 1, stream);
				names.push_back(boneName);
				parents.push_back(id);
			}
			SetBoneIDs(parents);
			SetBoneNames(names);
		}
	}

	fread(&this->partSize, sizeof(char), 1, stream);
	this->parts = new ModelPart[this->partSize];

	for (int i = 0; i < this->partSize; i++)
	{
		ModelPart part = ModelPart();

		if (version == 2)
		{
			part.ReadFromStream2(stream);
		}
		else
		{
			part.ReadFromStream(stream);
		}
		this->parts[i] = part;
	}
		
	fclose(stream);
}

void ModelStructure::WriteToStream(FILE* stream) {
	fwrite(&magicVersion2, sizeof(int), 1, stream);
	fwrite(&this->isSkinned, sizeof(byte), 1, stream);

	if (isSkinned)
	{
		int numBones;
		fwrite(&numBones, sizeof(byte), 1, stream);
		std::vector<std::string> names = GetBoneNames();
		std::vector<byte> parents = GetBoneIDs();
		for (int i = 0; i < numBones; i++)
		{
			WriteString(stream, names[i]);
			fwrite(&parents[i], sizeof(byte), 1, stream);
		}
	}
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
