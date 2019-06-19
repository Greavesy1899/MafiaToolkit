#include "CPhysXCooking.h"
#include "CTriangleMesh.h"
#include "CStream.h"

#define SUPPORT_CONVEX_PARTS

int CookMesh(const char* source, const char* dest)
{
	NxCookingInterface* gCooking = NxGetCookingLib(NX_PHYSICS_SDK_VERSION);
	if (!gCooking)
	{
		WriteLine("Failed to load PhysX cooking library!");
		return -10;
	}
	gCooking->NxInitCooking();
	WriteLine("Init cooking library");
	NxCookingParams params = gCooking->NxGetCookingParams();
	params.targetPlatform = PLATFORM_PC;
	gCooking->NxSetCookingParams(params);
	WriteLine("Set Cooking Params");
	CStream in(source, true);
	CStream out(dest, false);
	WriteLine("Set IO streams");
	NxPhysicsSDK* sdk = NxCreatePhysicsSDK(NX_PHYSICS_SDK_VERSION);
	NxTriangleMesh* mesh2 = sdk->createTriangleMesh(in);
	NxTriangleMeshDesc desc;
	mesh2->saveToDesc(desc);
	gCooking->NxCookTriangleMesh(desc, out);
	gCooking->NxCloseCooking();
	WriteLine("Completed cooking!");
	return 0;
}
