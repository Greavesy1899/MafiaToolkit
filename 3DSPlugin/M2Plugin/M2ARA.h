#ifndef M2_ARA_HEADER
#define M2_ARA_HEADER
#include <iparamb2.h>
#include <vector>

class ARAPart {
private:
	std::vector<Point3> vertices;
	std::wstring name;
	Point3 position;
public:
	ARAPart();
	~ARAPart();
	void SetName(std::wstring name);
	void SetVertices(std::vector<Point3> vertices);
	void SetPosition(Point3 position);
	std::wstring GetName();
	std::vector<Point3> GetVertices();
	Point3 GetPosition();

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
