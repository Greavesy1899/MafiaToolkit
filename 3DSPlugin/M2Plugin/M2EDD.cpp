#include "M2EDD.h"
#include "M2Helpers.h"

//===================================================
//		EDDEntry
//===================================================
void EDDEntry::SetLodCount(int count) {
	EDDEntry::lodCount = count;
}

void EDDEntry::SetMatrix(Matrix3 matrix) {
	EDDEntry::matrix = matrix;
}

void EDDEntry::SetLodNames(std::vector<std::wstring> lodNames) {
	EDDEntry::lodNames = lodNames;
}

int EDDEntry::GetLodCount() {
	return EDDEntry::lodCount;
}
Matrix3 EDDEntry::GetMatrix() {
	return EDDEntry::matrix;
}
std::vector<std::wstring> EDDEntry::GetLodNames() {
	return EDDEntry::lodNames;
}

void EDDEntry::ReadFromStream(FILE * stream) {
	fread(&lodCount, sizeof(int), 1, stream);

	Point3 position = Point3();
	fread(&position.x, sizeof(float), 1, stream);
	fread(&position.y, sizeof(float), 1, stream);
	fread(&position.z, sizeof(float), 1, stream);

	Point3 rotation[3];
	for (int c = 0; c != 3; c++) {
		fread(&rotation[c].x, sizeof(float), 1, stream);
		fread(&rotation[c].y, sizeof(float), 1, stream);
		fread(&rotation[c].z, sizeof(float), 1, stream);
	}

	matrix = Matrix3();
	matrix.Identity;
	matrix.SetRow(0, rotation[0]);
	matrix.SetRow(1, rotation[1]);
	matrix.SetRow(2, rotation[2]);
	matrix.SetRow(3, position);

	lodNames = std::vector<std::wstring>(lodCount);
	for (int c = 0; c != lodNames.size(); c++) {
		lodNames[c] = std::wstring();
		lodNames[c] = ReadString(stream, lodNames[c]);
		lodNames[c] += _T(".edm");
	}
}

EDDEntry::EDDEntry() {}
EDDEntry::~EDDEntry() {}