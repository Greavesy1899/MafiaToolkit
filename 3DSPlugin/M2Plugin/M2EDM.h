#ifndef M2_EDM_HEADER
#define M2_EDM_HEADER
#include <iparamb2.h>
#include <vector>

typedef struct {
	int i1;
	int i2;
	int i3;
} Int3;

class EDMPart {
private:
	std::wstring name;
	int vertSize;
	std::vector<Point3> vertices;
	std::vector<Point3> normals;
	int uvSize;
	std::vector<UVVert> uvs;
	int indicesSize;
	std::vector<Int3> indices;
public:
	EDMPart();
	~EDMPart();
	void SetName(std::wstring name);
	void SetVertSize(int count);
	void SetVertices(std::vector<Point3> vertices);
	void SetNormals(std::vector<Point3> normals);
	void SetUVSize(int count);
	void SetUVs(std::vector<UVVert> uvs);
	void SetIndicesSize(int count);
	void SetIndices(std::vector<Int3> indices);
	std::wstring GetName();
	int GetVertSize();
	std::vector<Point3> GetVertices();
	std::vector<Point3> GetNormals();
	int GetUVSize();
	std::vector<UVVert> GetUVs();
	int GetIndicesSize();
	std::vector<Int3> GetIndices();

	void ReadFromStream(FILE* stream);
};

class EDMStructure {
private:
	std::wstring name;
	int partSize;
	std::vector<EDMPart> parts;
public:
	EDMStructure();
	~EDMStructure();
	void SetName(std::wstring name);
	void SetPartSize(int count);
	void SetParts(std::vector<EDMPart> parts);
	std::wstring GetName();
	int GetPartSize();
	std::vector<EDMPart> GetParts();

	void ReadFromStream(FILE* stream);
};
#endif