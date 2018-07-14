#ifndef M2_EDM_HEADER
#define M2_EDM_HEADER
#include <iparamb2.h>
#include <vector>
#include <mesh.h>
#include "MeshNormalSpec.h"
#include <stdmat.h>

typedef struct {
	short i1;
	short i2;
	short i3;
} Int3;

class EDMPart {
private:
	std::wstring name;
	bool hasNormals;
	bool hasTangents;
	bool hasUVs;
	int vertSize;
	std::vector<Point3> vertices;
	std::vector<Point3> normals;
	std::vector<Point3> tangents;
	int uvSize;
	std::vector<UVVert> uvs;
	int indicesSize;
	std::vector<Int3> indices;
	Mesh mesh;
public:
	EDMPart();
	~EDMPart();
	void SetName(std::wstring name);
	void SetHasNormals(bool b);
	void SetHasTangents(bool b);
	void SetHasUVS(bool b);
	void SetVertSize(int count);
	void SetVertices(std::vector<Point3> vertices);
	void SetNormals(std::vector<Point3> normals);
	void SetTangents(std::vector<Point3> tangents);
	void SetUVSize(int count);
	void SetUVs(std::vector<UVVert> uvs);
	void SetIndicesSize(int count);
	void SetIndices(std::vector<Int3> indices);
	void SetMesh(Mesh mesh);
	std::wstring GetName();
	bool GetHasNormals();
	bool GetHasTangents();
	bool GetHasUVs();
	int GetVertSize();
	std::vector<Point3> GetVertices();
	std::vector<Point3> GetNormals();
	std::vector<Point3> GetTangents();
	int GetUVSize();
	std::vector<UVVert> GetUVs();
	int GetIndicesSize();
	std::vector<Int3> GetIndices();
	Mesh GetMesh();

	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE * stream);
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
	void WriteToStream(FILE * stream);
};
#endif