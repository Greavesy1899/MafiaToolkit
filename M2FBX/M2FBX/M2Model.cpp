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

void ModelPart::SetSubMeshCount(int count) {
	ModelPart::subMeshCount = count;
}

void ModelPart::SetMatNames(std::vector<std::string> names, bool updateCount) {
	ModelPart::matNames = names;

	if (updateCount)
		ModelPart::subMeshCount = names.size();
}

void ModelPart::SetIndicesSize(int count) {
	ModelPart::indicesSize = count;
}

void ModelPart::SetIndices(std::vector<Int3> indices, bool updateCount) {
	ModelPart::indices = indices;

	if (updateCount)
		ModelPart::indicesSize = indices.size();
}

void ModelPart::SetMatIDs(std::vector<short> ids) {
	ModelPart::matIDs = ids;
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

std::vector<Int3> ModelPart::GetIndices() {
	return ModelPart::indices;
}

std::vector<std::string> ModelPart::GetMatNames() {
	return ModelPart::matNames;
}

std::vector<short> ModelPart::GetMatIDs() {
	return ModelPart::matIDs;
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
			fread(&uvs0[i].x, sizeof(float), 1, stream);
			fread(&uvs0[i].y, sizeof(float), 1, stream);
		}
		if (hasUV1) {
			fread(&uvs1[i].x, sizeof(float), 1, stream);
			fread(&uvs1[i].y, sizeof(float), 1, stream);
		}
		if (hasUV2) {
			fread(&uvs2[i].x, sizeof(float), 1, stream);
			fread(&uvs2[i].y, sizeof(float), 1, stream);
		}
		if (hasUV7) {
			fread(&uvs7[i].x, sizeof(float), 1, stream);
			fread(&uvs7[i].y, sizeof(float), 1, stream);
		}
	}
	fread(&subMeshCount, sizeof(int), 1, stream);
	long pos = ftell(stream);
	matNames = std::vector<std::string>(subMeshCount);
	for (int i = 0; i != subMeshCount; i++) {
		std::string edmName = std::string();
		edmName = ReadString(stream, edmName);
		matNames[i] = edmName;
	}
	fread(&indicesSize, sizeof(int), 1, stream);
	indices = std::vector<Int3>(indicesSize);
	matIDs = std::vector<short>(indicesSize);
	for (int x = 0; x != indicesSize; x++) {
		Int3 tri;
		char matID = 0;
		fread(&tri.i1, sizeof(unsigned short), 1, stream);
		fread(&tri.i2, sizeof(unsigned short), 1, stream);
		fread(&tri.i3, sizeof(unsigned short), 1, stream);
		fread(&matIDs[x], sizeof(short), 1, stream);
		indices[x] = tri;
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
			fwrite(&uvs0[i].x, sizeof(float), 1, stream);
			fwrite(&uvs0[i].y, sizeof(float), 1, stream);
		}
		if (hasUV1) {
			fwrite(&uvs1[i].x, sizeof(float), 1, stream);
			fwrite(&uvs1[i].y, sizeof(float), 1, stream);
		}
		if (hasUV2) {
			fwrite(&uvs2[i].x, sizeof(float), 1, stream);
			fwrite(&uvs2[i].y, sizeof(float), 1, stream);
		}
		if (hasUV7) {
			fwrite(&uvs7[i].x, sizeof(float), 1, stream);
			fwrite(&uvs7[i].y, sizeof(float), 1, stream);
		}
	}
	fwrite(&subMeshCount, sizeof(int), 1, stream);

	for (int i = 0; i != subMeshCount; i++)
		WriteString(stream, matNames[i]);

	fwrite(&indicesSize, sizeof(int), 1, stream);
	for (int i = 0; i != indices.size(); i++) {
		fwrite(&indices[i].i1, sizeof(unsigned short), 1, stream);
		fwrite(&indices[i].i2, sizeof(unsigned short), 1, stream);
		fwrite(&indices[i].i3, sizeof(unsigned short), 1, stream);
		fwrite(&matIDs[i], sizeof(short), 1, stream);
	}
}

ModelPart::ModelPart() {}
ModelPart::~ModelPart() {}

//===================================================
//		ModelStructure
//===================================================
void ModelStructure::SetName(std::string name) {
	name.erase(std::remove(name.begin(), name.end(), '?'), name.end());
	ModelStructure::name = name;
}

void ModelStructure::SetPartSize(char count) {
	ModelStructure::partSize = count;
}

void ModelStructure::SetParts(std::vector<ModelPart> parts, bool updateCount) {
	ModelStructure::parts = parts;

	if (updateCount)
		ModelStructure::partSize = parts.size();
}

std::string ModelStructure::GetName() {
	return name;
}

char ModelStructure::GetPartSize() {
	return partSize;
}

std::vector<ModelPart> ModelStructure::GetParts() {
	return parts;
}

void ModelStructure::ReadFromStream(FILE * stream) {
	int header;
	fread(&header, sizeof(int), 1, stream); //header

	if (header != magic)
		exit(0);

	std::string edmName = std::string();
	edmName = ReadString(stream, edmName);
	name = edmName;
	fread(&partSize, 1, 1, stream);
	parts = std::vector<ModelPart>(partSize);

	for (int i = 0; i != parts.size(); i++)
		parts[i].ReadFromStream(stream);

	fclose(stream);
}

void ModelStructure::WriteToStream(FILE * stream) {
	fwrite(&magic, sizeof(int), 1, stream);
	WriteString(stream, name);
	fwrite(&partSize, sizeof(char), 1, stream);

	for (int x = 0; x != parts.size(); x++)
		parts[x].WriteToStream(stream);

	fclose(stream);
}

ModelStructure::ModelStructure() {}
ModelStructure::~ModelStructure()
{
}

//===================================================
//		FrameEntry
//===================================================
FrameEntry::FrameEntry() 
{ 
	lodCount = 0; 
	matrix.m00 = 1.0f;
	matrix.m11 = 1.0f;
	matrix.m22 = 1.0f;
}
FrameEntry::~FrameEntry() {}

void FrameEntry::SetLodCount(int count)
{
	this->lodCount = count;
}

void FrameEntry::SetMatrix(Matrix3 matrix)
{
	this->matrix = matrix;
}

void FrameEntry::SetLodNames(std::vector<std::string> lodNames)
{
	this->lodNames = lodNames;
	this->lodCount = lodNames.size();
}

int FrameEntry::GetLodCount()
{
	return this->lodCount;
}

Matrix3 FrameEntry::GetMatrix()
{
	return this->matrix;
}

std::vector<std::string> FrameEntry::GetLodNames()
{
	return this->lodNames;
}

void FrameEntry::SetPosition(Point3 pos)
{
	this->position = pos;
}

Point3 FrameEntry::GetPosition()
{
	return this->position;
}

void FrameEntry::ReadFromStream(FILE * stream)
{
	fread(&lodCount, sizeof(int), 1, stream);

	fread(&position.x, sizeof(float), 1, stream);
	fread(&position.y, sizeof(float), 1, stream);
	fread(&position.z, sizeof(float), 1, stream);

	fread(&matrix.m00, sizeof(float), 1, stream);
	fread(&matrix.m01, sizeof(float), 1, stream);
	fread(&matrix.m02, sizeof(float), 1, stream);
	fread(&matrix.m10, sizeof(float), 1, stream);
	fread(&matrix.m11, sizeof(float), 1, stream);
	fread(&matrix.m12, sizeof(float), 1, stream);
	fread(&matrix.m20, sizeof(float), 1, stream);
	fread(&matrix.m21, sizeof(float), 1, stream);
	fread(&matrix.m22, sizeof(float), 1, stream);

	lodNames = std::vector<std::string>(lodCount);
	for (int c = 0; c != lodNames.size(); c++) {
		lodNames[c] = std::string();
		lodNames[c] = ReadString(stream, lodNames[c]);
		lodNames[c] += ".m2t";
	}
}

void FrameEntry::WriteToStream(FILE * stream)
{
	fwrite(&lodCount, sizeof(int), 1, stream);

	fwrite(&position.x, sizeof(float), 1, stream);
	fwrite(&position.y, sizeof(float), 1, stream);
	fwrite(&position.z, sizeof(float), 1, stream);

	fwrite(&matrix.m00, sizeof(float), 1, stream);
	fwrite(&matrix.m01, sizeof(float), 1, stream);
	fwrite(&matrix.m02, sizeof(float), 1, stream);
	fwrite(&matrix.m10, sizeof(float), 1, stream);
	fwrite(&matrix.m11, sizeof(float), 1, stream);
	fwrite(&matrix.m12, sizeof(float), 1, stream);
	fwrite(&matrix.m20, sizeof(float), 1, stream);
	fwrite(&matrix.m21, sizeof(float), 1, stream);
	fwrite(&matrix.m22, sizeof(float), 1, stream);

	for (int c = 0; c != lodNames.size(); c++) {
		WriteString(stream, lodNames[c]);
	}
}

FrameClass::~FrameClass()
{
}

int FrameClass::GetNumEntries()
{
	return this->entryCount;
}

void FrameClass::SetNumEntries(int num)
{
	this->entryCount = num;
}

std::vector<FrameEntry> FrameClass::GetEntries()
{
	return this->entries;
}

void FrameClass::SetEntries(std::vector<FrameEntry> entries)
{
	this->entries = entries;
	this->entryCount = entries.size();
}

void FrameClass::ReadFromStream(FILE * stream)
{
	int tempHeader;
	fread(&tempHeader, sizeof(int), 1, stream);

	if (tempHeader != magic)
		return;

	fread(&entryCount, sizeof(int), 1, stream);
	entries = std::vector<FrameEntry>(entryCount);
	for (int i = 0; i != entryCount; i++)
	{
		FrameEntry entry = FrameEntry();
		entry.ReadFromStream(stream);
		entries.push_back(entry);
	}
}

void FrameClass::WriteToStream(FILE * stream)
{
	fwrite(&magic, sizeof(int), 1, stream);

	fwrite(&entryCount, sizeof(int), 1, stream);
	for (int i = 0; i != entryCount; i++)
	{
		entries.at(i).WriteToStream(stream);
	}
}

FrameClass::FrameClass()
{

}
