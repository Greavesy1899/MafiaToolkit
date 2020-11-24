#include "PhysXCooker.h"

#include "PhysXModel.h"

#include <NxStream.h>
#include <conio.h>

void PhysXCooker::Initialise(const char* InDestinationFile)
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

	DestinationName = InDestinationFile;
}

void PhysXCooker::CookTriangleMeshFromModel(PhysXModel& Model)
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
	CookTriangleMesh(MeshDesc);
}

void PhysXCooker::Deinitialise()
{
	CookingLib->NxCloseCooking();
	PhysicsSDK->release();
	CookingLib = nullptr;
	PhysicsSDK = nullptr;
}

void* PhysXCooker::CookTriangleMesh(const NxTriangleMeshDesc& MeshDesc)
{
	InStream.OpenStream(DestinationName, "wb");
	if (!CookingLib->NxCookTriangleMesh(MeshDesc, InStream))
	{
		printf("Failed to cook TriangleMesh \n");
		return nullptr;
	}

	InStream.CloseStream();

	printf("Cooked and saved to stream: %s\n", DestinationName);

	// Broken
	/*
	NxU32 Size = 0;
	char* Buffer = InStream.GetContentsAsBuffer(Size);
	std::vector<char> CookedData(Buffer, Buffer+Size);*/
	//InStream.CloseStream();

	return nullptr;
}
