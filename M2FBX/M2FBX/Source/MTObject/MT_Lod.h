#pragma once

#include "DataTypes.h"
#include "MT_FaceGroup.h"

#include <string>

typedef unsigned int uint;
typedef unsigned short ushort;

class MT_ObjectHandler;

class MT_Lod
{
	friend MT_ObjectHandler;

public:

	// Utils
	void ResetVertexFlags() { VertexDeclaration = (VertexFlags)0; }
	void AddVertexFlag(const VertexFlags Flag);
	bool HasVertexFlag(const VertexFlags Flag) const;
	void Cleanup();

	// Accessors
	const std::vector<Vertex>& GetVertices() const { return Vertices; }
	const std::vector<Int3>& GetIndices() const { return Indices; }
	const std::vector<MT_FaceGroup>& GetFaceGroups() const { return FaceGroups; }

	// Setters
	void SetVertexFlags(const VertexFlags& InFlags) { VertexDeclaration = InFlags; }
	void SetVertices(std::vector<Vertex>& InVertices) { Vertices = InVertices; }
	void SetIndices(std::vector<Int3>& InIndices) { Indices = InIndices; }
	void SetFaceGroups(std::vector<MT_FaceGroup> InFaceGroups) { FaceGroups = InFaceGroups; }

	// IO
	void ReadFromFile(FILE* InStream);
	void WriteToFile(FILE* OutStream) const;

private:

	VertexFlags VertexDeclaration;
	std::vector<Vertex> Vertices;
	std::vector<Int3> Indices;
	std::vector<MT_FaceGroup> FaceGroups;
};
