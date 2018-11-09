#ifndef M2_EDM_HEADER
#define M2_EDM_HEADER
#include <iparamb2.h>
#include <vector>
#include <mesh.h>
#include "MeshNormalSpec.h"
#include <stdmat.h>

typedef struct {
	unsigned short i1;
	unsigned short i2;
	unsigned short i3;
} Int3;

class EDMPart {
private:
	bool hasPosition;
	bool hasNormals;
	bool hasTangents;
	bool hasBlendData;
	bool hasFlag0x80;
	bool hasUV0;
	bool hasUV1;
	bool hasUV2;
	bool hasUV7;
	bool hasFlag0x20000;
	bool hasFlag0x40000;
	bool hasDamageGroup;
	int vertSize;
	std::vector<Point3> vertices;
	std::vector<Point3> normals;
	std::vector<Point3> tangents;
	std::vector<UVVert> uvs;
	std::vector<UVVert> uvs1;
	std::vector<UVVert> uvs2;
	std::vector<UVVert> uvs7;
	int subMeshCount;
	std::vector<std::wstring> matNames;
	int indicesSize;
	std::vector<Int3> indices;
	std::vector<short> matIDs;
	Mesh mesh;
public:
	EDMPart();
	~EDMPart();
	void SetHasPositions(bool b);
	void SetHasNormals(bool b);
	void SetHasTangents(bool b);
	void SetHasBlendData(bool b);
	void SetHasFlag0x80(bool b);
	void SetHasUV0(bool b);
	void SetHasUV1(bool b);
	void SetHasUV2(bool b);
	void SetHasUV7(bool b);
	void SetHasFlag0x20000(bool b);
	void SetHasFlag0x40000(bool b);
	void SetHasDamage(bool b);
	void SetVertSize(int count);
	void SetVertices(std::vector<Point3> vertices);
	void SetNormals(std::vector<Point3> normals);
	void SetTangents(std::vector<Point3> tangents);
	void SetUVs(std::vector<UVVert> uvs);
	void SetUV1s(std::vector<UVVert> uvs);
	void SetUV2s(std::vector<UVVert> uvs);
	void SetUV7s(std::vector<UVVert> uvs);
	void SetSubMeshCount(int count);
	void SetIndicesSize(int count);
	void SetMatNames(std::vector<std::wstring> matNames);
	void SetIndices(std::vector<Int3> indices);
	void SetMatIDs(std::vector<short> matIDs);
	void SetMesh(Mesh mesh);
	bool GetHasPositions();
	bool GetHasNormals();;
	bool GetHasTangents();
	bool GetHasBlendData();
	bool GetHasFlag0x80();
	bool GetHasUV0();
	bool GetHasUV1();
	bool GetHasUV2();
	bool GetHasUV7();
	bool GetHasFlag0x20000();
	bool GetHasFlag0x40000();
	bool GetHasDamage();
	int GetVertSize();
	std::vector<Point3> GetVertices();
	std::vector<Point3> GetNormals();
	std::vector<Point3> GetTangents();
	std::vector<UVVert> GetUVs();
	std::vector<UVVert> GetUV1s();
	std::vector<UVVert> GetUV2s();
	std::vector<UVVert> GetUV7s();
	int GetSubMeshCount();
	int GetIndicesSize();
	std::vector<std::wstring> GetMatNames();
	std::vector<Int3> GetIndices();
	std::vector<short> GetMatIDs();
	Mesh GetMesh();

	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE * stream);
};

class EDMStructure {
private:
	std::wstring name;
	byte partSize;
	std::vector<EDMPart> parts;
public:
	EDMStructure();
	~EDMStructure();
	void SetName(std::wstring name);
	void SetPartSize(byte count);
	void SetParts(std::vector<EDMPart> parts);
	std::wstring GetName();
	byte GetPartSize();
	std::vector<EDMPart> GetParts();

	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE * stream);
};
#endif