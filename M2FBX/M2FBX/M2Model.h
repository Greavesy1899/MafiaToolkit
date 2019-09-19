#ifndef M2_EDM_HEADER
#define M2_EDM_HEADER
#include "Common.h"

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
typedef struct {
	Point3 position;
	Point3 normals;
	Point3 tangent;
	UVVert uv0;
	UVVert uv1;
	UVVert uv2;
	UVVert uv3;
} Vertex;

class SubMesh {
private:
	int startIndex;
	int numFaces;
	std::string matName;

public:
	void SetStartIndex(int& value);
	void SetNumFaces(int& value);
	void SetMatName(std::string name);
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
	uint numVertices;
	Vertex* vertices;
	uint numSubMeshes;
	SubMesh* submeshes;
	uint numIndices;
	std::vector<Int3> indices;
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
	void SetVertices(Vertex* vertices, unsigned int count);
	void SetSubMeshes(SubMesh* subMeshes, unsigned int count);
	void SetSubMeshCount(int count);
	void SetIndicesSize(int count);
	void SetIndices(std::vector<Int3> indices, unsigned int count);
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
	uint GetVertSize();
	Vertex* GetVertices();
	uint GetSubMeshCount();
	uint GetIndicesSize();
	SubMesh* GetSubMeshes() const;
	std::vector<Int3> GetIndices();
	
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
	void SetName(std::string name);
	void SetPartSize(char& count);
	void SetParts(ModelPart* parts);
	std::string GetName() const;
	char GetPartSize() const;
	ModelPart* GetParts() const;

	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE * stream);
};
#endif