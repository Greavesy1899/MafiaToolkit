#ifndef M2_EDM_HEADER
#define M2_EDM_HEADER
#include <fbxsdk.h>
#include <vector>
#include <string>

typedef struct {
	unsigned short i1;
	unsigned short i2;
	unsigned short i3;
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
typedef struct {
	float m00;
	float m01;
	float m02;
	float m10;
	float m11;
	float m12;
	float m20;
	float m21;
	float m22;
} Matrix3;

class SubMesh {
private:
	int startIndex;
	int numFaces;
	std::string matName;

public:
	void SetStartIndex(int& value);
	void SetNumFaces(int& value);
	void SetMatName(std::string& name);
	int GetStartIndex() const;
	int GetNumFaces() const;
	std::string GetMatName() const;
};
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
	std::vector<UVVert> uvs0;
	std::vector<UVVert> uvs1;
	std::vector<UVVert> uvs2;
	std::vector<UVVert> uvs7;
	int subMeshCount;
	SubMesh* submeshes;
	int indicesSize;
	std::vector<Int3> indices;
	std::vector<short> matIDs;
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
	void SetVertices(std::vector<Point3> vertices, bool updateCount = false);
	void SetNormals(std::vector<Point3> normals);
	void SetTangents(std::vector<Point3> tangents);
	void SetUV0s(std::vector<UVVert> uvs);
	void SetUV1s(std::vector<UVVert> uvs);
	void SetUV2s(std::vector<UVVert> uvs);
	void SetUV7s(std::vector<UVVert> uvs);
	void SetSubMeshes(SubMesh* subMeshes);
	void SetSubMeshCount(int count);
	void SetIndicesSize(int count);
	void SetIndices(std::vector<Int3> indices, bool updateCount = false);
	void SetMatIDs(std::vector<short> matIDs);
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
	std::vector<UVVert> GetUV0s();
	std::vector<UVVert> GetUV1s();
	std::vector<UVVert> GetUV2s();
	std::vector<UVVert> GetUV7s();
	int GetSubMeshCount();
	int GetIndicesSize();
	SubMesh* GetSubMeshes() const;
	std::vector<Int3> GetIndices();
	std::vector<short> GetMatIDs();

	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE * stream);
};

class ModelStructure {
private:
	const int magic = 22295117;
	std::string name;
	char partSize;
	ModelPart* parts;
public:
	ModelStructure();
	~ModelStructure();
	void SetName(std::string& name);
	void SetPartSize(char& count);
	void SetParts(ModelPart* parts);
	std::string GetName() const;
	char GetPartSize() const;
	ModelPart* GetParts() const;

	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE * stream);
};

class FrameEntry {
private:
	int lodCount;
	std::vector<std::string> lodNames;
	Matrix3 matrix;
	Point3 position;
public:
	FrameEntry();
	~FrameEntry();
	void SetLodCount(int count);
	void SetMatrix(Matrix3 matrix);
	void SetLodNames(std::vector<std::string> lodNames);
	int GetLodCount();
	Matrix3 GetMatrix();
	std::vector<std::string> GetLodNames();
	void SetPosition(Point3 pos);
	Point3 GetPosition();
	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE* stream);
};

class FrameClass {
private:
	const int magic = 808535109;
	int entryCount;
	std::vector<FrameEntry> entries;
public:
	FrameClass();
	~FrameClass();
	int GetNumEntries();
	void SetNumEntries(int num);
	std::vector<FrameEntry> GetEntries();
	void SetEntries(std::vector<FrameEntry> entries);
	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE* stream);
};
#endif