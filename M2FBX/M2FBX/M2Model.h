#ifndef M2_EDM_HEADER
#define M2_EDM_HEADER
#include <vector>

typedef struct {
	short i1;
	short i2;
	short i3;
} Int3;
typedef struct {
	float x;
	float y;
	float z;
} Point3;
typedef struct {
	float x;
	float y;
} UVVert;

class ModelPart {
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
	int subMeshCount;
	std::vector<std::wstring> matNames;
	int indicesSize;
	std::vector<Int3> indices;
	std::vector<char> matIDs;
public:
	ModelPart();
	~ModelPart();
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
	void SetSubMeshCount(int count);
	void SetIndicesSize(int count);
	void SetMatNames(std::vector<std::wstring> matNames);
	void SetIndices(std::vector<Int3> indices);
	void SetMatIDs(std::vector<char> matIDs);
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
	int GetSubMeshCount();
	int GetIndicesSize();
	std::vector<std::wstring> GetMatNames();
	std::vector<Int3> GetIndices();
	std::vector<char> GetMatIDs();

	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE * stream);
};

class ModelStructure {
private:
	std::wstring name;
	char partSize;
	std::vector<ModelPart> parts;
public:
	ModelStructure();
	~ModelStructure();
	void SetName(std::wstring name);
	void SetPartSize(char count);
	void SetParts(std::vector<ModelPart> parts);
	std::wstring GetName();
	char GetPartSize();
	std::vector<ModelPart> GetParts();

	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE * stream);
};
#endif