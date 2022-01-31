#pragma once

#include "Common.h"

#include "MTObject/MT_FaceGroup.h"

#include <vector>
#include <map>

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

	void ConstructIndicesAndFaceGroupsFromNode(FbxNode* TargetNode, std::vector<Int3>* Indices, std::vector<MT_FaceGroup>* FaceGroups);
	FbxGeometryElementUV* GetUVElementByIndex(FbxMesh* Mesh, uint ElementType) const;

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

