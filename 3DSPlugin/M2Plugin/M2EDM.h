#pragma once
#include <iparamb2.h>
#include <vector>

typedef struct {
	int i1;
	int i2;
	int i3;
} Int3;

class EDMPart {
private:
	const wchar_t* name;
	int vertSize;
	std::vector<Point3> vertices;
	int uvSize;
	std::vector<UVVert> uvs;
	int indicesSize;
	std::vector<Int3> indices;
public:
	EDMPart();
	~EDMPart();
	void SetName(const wchar_t* name);
	void SetVertSize(int count);
	void SetVertices(std::vector<Point3> vertices);
	void SetUVSize(int count);
	void SetUVs(std::vector<UVVert> uvs);
	void SetIndicesSize(int count);
	void SetIndices(std::vector<Int3> indices);
	const wchar_t* GetName();
	int GetVertSize();
	std::vector<Point3> GetVertices();
	int GetUVSize();
	std::vector<UVVert> GetUVs();
	int GetIndicesSize();
	std::vector<Int3> GetIndices();

	void ReadFromStream(FILE* stream);
};

class EDMStructure {
private:
	const wchar_t* name;
	int partSize;
	std::vector<EDMPart> parts;
public:
	EDMStructure();
	~EDMStructure();
	void SetName(const wchar_t* name);
	void SetPartSize(int count);
	void SetParts(std::vector<EDMPart> parts);
	const wchar_t* GetName();
	int GetPartSize();
	std::vector<EDMPart> GetParts();

	void ReadFromStream(FILE* stream);
};
