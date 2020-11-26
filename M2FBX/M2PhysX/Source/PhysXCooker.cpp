#include "PhysXCooker.h"

#include "PhysXModel.h"

#include <NxStream.h>
#include <conio.h>

void PhysXCooker::Initialise()
{
	CookingLib = NxGetCookingLib(NX_PHYSICS_SDK_VERSION);
	if (!CookingLib)
	{
		// Failed to init
		printf("Didn't get init cooking \n");
		return;
	}

	PhysicsSDK = NxCreatePhysicsSDK(NX_PHYSICS_SDK_VERSION);
	if (!PhysicsSDK)
	{
		// Failed to get PhysicsSDK
		printf("Didn't get Physics SDK \n");
		return;
	}
}

void PhysXCooker::CookTriangleMeshFromModel(PhysXModel& Model, PhysXStream* OutStream)
{
	const bool bIsInited = CookingLib->NxInitCooking();
	if (!bIsInited)
	{
		// Failed to init
		printf("Didn't get init cooking \n");
		return;
	}

	NxCookingParams CookingParams = CookingLib->NxGetCookingParams();
	CookingParams.targetPlatform = PLATFORM_PC;
	CookingLib->NxSetCookingParams(CookingParams);

	NxPhysicsSDK* PhysicsSDK = NxCreatePhysicsSDK(NX_PHYSICS_SDK_VERSION);
	if (!PhysicsSDK)
	{
		// Failed to get PhysicsSDK
		printf("Didn't get Physics SDK \n");
		return;
	}

	NxVec3* Vertices = Model.GetVertices().data();
	NxU32* Indices = Model.GetIndices().data();

	// Just some dumping of info
	printf("Number of Vertices: %i\n", Model.GetNumVertices());
	printf("Number of Triangles: %i\n", Model.GetNumTriangles());

	printf("Success, going to cook now.\n");
	NxTriangleMeshDesc MeshDesc = {};
	MeshDesc.materialIndexStride = sizeof(NxU16);
	MeshDesc.materialIndices = Model.GetMaterialIDs().data();
	MeshDesc.triangleStrideBytes = 3 * sizeof(NxU32);
	MeshDesc.triangles = Model.GetIndices().data();
	MeshDesc.numTriangles = Model.GetNumTriangles();
	MeshDesc.pointStrideBytes = sizeof(NxVec3);
	MeshDesc.points = Model.GetVertices().data();
	MeshDesc.numVertices = Model.GetNumVertices();
	printf("Attempting to cook.\n");

	// Move cooked information to PhysXModel.
	NxU32 CookedSize = 0;
	void* CookedInfo = CookTriangleMesh(MeshDesc, CookedSize, OutStream);
	Model.SetCookedInfo(CookedInfo, CookedSize);
}

void PhysXCooker::Deinitialise()
{
	CookingLib->NxCloseCooking();
	PhysicsSDK->release();
	CookingLib = nullptr;
	PhysicsSDK = nullptr;
}

void* PhysXCooker::CookTriangleMesh(const NxTriangleMeshDesc& MeshDesc, NxU32& CookedSize, PhysXStream* OutStream)
{
	if (!CookingLib->NxCookTriangleMesh(MeshDesc, *OutStream))
	{
		printf("Failed to cook TriangleMesh \n");
		return nullptr;
	}

	printf("Cooked and saved to stream.\n");

	return nullptr;
}
