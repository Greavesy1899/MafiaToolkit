#pragma once

#include "Common.h"

#include "MTObject/MT_FaceGroup.h"

#include <map>
#include <vector>

class MT_Collision;
class MT_Object;
class MT_ObjectBundle;
class MT_Lod;
class MT_Joint;
class MT_Skeleton;
class LogSystem;

class MT_Wrangler
{
public:

	MT_Wrangler() = default;
	MT_Wrangler(const char* InName, const char* InDest);
	~MT_Wrangler();

	bool ConstructMTBFromFbx();
	bool SaveBundleToFile();

private:

	const FbxNodeAttribute::EType GetNodeType(FbxNode* Node) const;

	MT_Object* ConstructBaseObject(FbxNode* Node);
	MT_Object* ConstructMesh(FbxNode* Node);
	MT_Collision* ConstructCollision(FbxNode* Node);
	MT_Lod* ConstructFromLod(FbxNode* Lod);
	MT_Skeleton* ConstructSkeleton(FbxNode* Node);
	MT_Joint* ConstructJoint(FbxNode* Node);

	bool SetupFbxManager();
	bool SetupImporter();

	// Construct the mesh data from the FbxNode (expected to be an FbxMesh).
	void ConstructIndicesAndFaceGroupsFromNode(FbxNode* TargetNode, std::vector<Vertex>* Vertices, std::vector<Int3>* Indices, std::vector<MT_FaceGroup>* FaceGroups);

	// Construct a Vertex from the passed FbxMesh. Based on the layers Reference/Mapping mode, we will either use the ControlPointIndex OR the PolygonIndex.
	// Make sure both are correct setup otherwise you may see some wonky mesh outputs.
	void ConstructVertexFromMesh(FbxMesh* InMesh, uint32_t ControlPointIndex, uint32_t PolygonIndex, uint32_t PolygonVertexIndex, Vertex& OutVertex);

	// Get the UVElement by index. Based on how we order each UV in the format.
	FbxGeometryElementUV* GetUVElementByIndex(FbxMesh* Mesh, uint ElementType) const;

	// Update the LodObject vertex declaration based on the passed FbxMesh.
	void UpdateVertexDeclaration(FbxMesh* InMesh, MT_Lod& LodObject) const;

	const char* MTOName;
	const char* FbxName;

	struct SkeletonData {
		std::vector<MT_Joint> Joints;
		std::map<std::string, int> JointLookupTable;
	} SkeletonState;

	MT_ObjectBundle* LoadedBundle = nullptr;

	// Fbx related
	FbxManager* SdkManager = nullptr;
	FbxScene* Scene = nullptr;

	// Logging
	LogSystem* Logger = nullptr;
};

