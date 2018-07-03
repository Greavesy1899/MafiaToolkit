#include "M2ARA.h"
#include "M2Helpers.h"

//===================================================
//		ARAPART
//===================================================
void ARAPart::SetName(std::wstring name) {
	ARAPart::name = name;
}

void ARAPart::SetVertices(std::vector<Point3> vertices) {
	ARAPart::vertices = vertices;
}

void ARAPart::SetPosition(Point3 position) {
	ARAPart::position = position;
}

std::wstring ARAPart::GetName() {
	return ARAPart::name;
}

std::vector<Point3> ARAPart::GetVertices() {
	return ARAPart::vertices;
}

Point3 ARAPart::GetPosition() {
	return ARAPart::position;
}

void ARAPart::ReadFromStream(FILE * stream) {
	vertices = std::vector<Point3>(8);
	for (int i = 0; i != 8; i++) {
		fread(&vertices[i].x, sizeof(float), 1, stream);
		fread(&vertices[i].y, sizeof(float), 1, stream);
		fread(&vertices[i].z, sizeof(float), 1, stream);
	}
	std::wstring partName = std::wstring();
	partName = ReadString(stream, partName);
	name = partName;
	position = Point3();
	fread(&position.x, sizeof(float), 1, stream);
	fread(&position.y, sizeof(float), 1, stream);
	fread(&position.z, sizeof(float), 1, stream);
}

ARAPart::ARAPart() {}
ARAPart::~ARAPart() {}

//===================================================
//		ARASTRUCTURE
//===================================================
void ARAStructure::SetPartSize(int count) {
	ARAStructure::partSize = count;
}

void ARAStructure::SetParts(std::vector<ARAPart> parts) {
	ARAStructure::parts = parts;
}

int ARAStructure::GetPartSize() {
	return partSize;
}

std::vector<ARAPart> ARAStructure::GetParts() {
	return parts;
}

void ARAStructure::ReadFromStream(FILE * stream) {
	fread(&partSize, sizeof(int), 1, stream);
	parts = std::vector<ARAPart>(partSize);
	for (int i = 0; i != partSize; i++) {
		parts[i].ReadFromStream(stream);
	}
}

ARAStructure::ARAStructure() {}
ARAStructure::~ARAStructure() {}
