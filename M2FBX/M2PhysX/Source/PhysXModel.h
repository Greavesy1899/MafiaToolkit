#pragma once

#include <vector>

#include <NxSimpleTypes.h>
#include <NxVec3.h>

class NxVec3;


/*
* A Model used by PhysXModelBundle.
*/
class PhysXModel
{
public:

	PhysXModel();
	~PhysXModel();

	// Settings
	void SetVertices(std::vector<NxVec3>& InVertices) { Vertices = InVertices; }
	void SetIndices(std::vector<NxU32>& InIndices) { Indices = InIndices; }
	void SetMaterialIDs(std::vector<NxU16>& InMaterialIDs) { MaterialIDs = InMaterialIDs; }

	// Accessors
	std::vector<NxVec3>& GetVertices() { return Vertices; }
	std::vector<NxU32>& GetIndices() { return Indices; }
	std::vector<NxU16>& GetMaterialIDs() { return MaterialIDs; }
	NxU32 GetNumTriangles() { return Indices.size() / 3; }
	NxU32 GetNumVertices() { return Vertices.size(); }

private:

	std::vector<NxVec3> Vertices;
	std::vector<NxU32> Indices;
	std::vector<NxU16> MaterialIDs;
};

