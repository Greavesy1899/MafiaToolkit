#pragma once
#include "Common.h"

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
	VertexFlags flags;
	uint numVertices;
	Vertex* vertices;
	std::vector<SubMesh> submeshes;
	uint numIndices;
	std::vector<Int3> indices;
public:
	ModelPart();
	~ModelPart();
	void SetVertexFlag(VertexFlags flag);
	void SetVertSize(int count);
	void SetVertices(Vertex* vertices, unsigned int count);
	void SetSubMeshes(const std::vector<SubMesh>& subMeshes);
	void SetIndicesSize(int count);
	void SetIndices(std::vector<Int3> indices, unsigned int count);
	bool HasVertexFlag(VertexFlags flag);
	uint GetVertSize();
	Vertex* GetVertices();
	uint GetSubMeshCount();
	uint GetIndicesSize();
	std::vector<SubMesh> GetSubMeshes() const;
	std::vector<Int3>& GetIndices();
	
	void ReadFromStream(FILE* stream);
	void ReadFromStream2(FILE* stream);
	void WriteToStream(FILE * stream);
};

class ModelStructure {
private:
	const int magicVersion1 = 22295117;
	const int magicVersion2 = 39072333;
	std::string name;
	bool isSkinned;
	std::vector<std::string> jointNames;
	std::vector<Joint> joints;
	char partSize;
	ModelPart* parts;
public:
	ModelStructure();
	~ModelStructure();
	void SetName(std::string name);
	void SetPartSize(char& count);
	void SetParts(ModelPart* parts);
	void SetJointNames(const std::vector<std::string>& names);
	void SetJoints(const std::vector<Joint>& boneIDs);
	void SetIsSkinned(bool skinned);
	std::string GetName() const;
	char GetPartSize() const;
	ModelPart* GetParts() const;
	std::vector<std::string>& GetJointNames();
	std::vector<Joint>& GetJoints();
	bool GetIsSkinned();

	void ReadFromStream(FILE* stream);
	void WriteToStream(FILE * stream);
};