#ifndef M2_ARA_HEADER
#define M2_ARA_HEADER
#include <iparamb2.h>
#include <vector>
#include "quat.h"

class ARAPart {
private:
	std::vector<Point3> vertices;
	std::wstring name;
	Point3 position;
	Matrix3 matrix;
public:
	ARAPart();
	~ARAPart();
	void SetName(std::wstring name);
	void SetVertices(std::vector<Point3> vertices);
	void SetPosition(Point3 position);
	void SetMatrix(Matrix3 matrix);
	std::wstring GetName();
	std::vector<Point3> GetVertices();
	Point3 GetPosition();
	Matrix3 GetMatrix();

	void ReadFromStream(FILE* stream);
};

class ARAStructure {
private:
	int partSize;
	std::vector<ARAPart> parts;
public:
	ARAStructure();
	~ARAStructure();
	void SetPartSize(int size);
	void SetParts(std::vector<ARAPart> parts);
	int GetPartSize();
	std::vector<ARAPart> GetParts();

	void ReadFromStream(FILE* stream);
};
#endif
