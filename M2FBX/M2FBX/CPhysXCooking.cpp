#include "CPhysXCooking.h"
#include "CStream.h"
#include <conio.h>

#define SUPPORT_CONVEX_PARTS

int CookTriangle(const char* source, const char* dest)
{
	NxCookingInterface* gCooking = NxGetCookingLib(NX_PHYSICS_SDK_VERSION);
	if (!gCooking)
	{
		WriteLine("Failed to load PhysX cooking library!");
		_getch();
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
	if (!sdk)
	{
		WriteLine("Failed to load PhysicsSDK: %i", NX_PHYSICS_SDK_VERSION);
		_getch();
		return -10;
	}

	WriteLine("Got a valid SDK");

	NxTriangleMesh* mesh2 = sdk->createTriangleMesh(in);
	NxTriangleMeshDesc desc;
	mesh2->saveToDesc(desc);
	
	WriteLine("Got cooked mesh");

	gCooking->NxCookTriangleMesh(desc, out);

	WriteLine("Writing to cook.bin");

	gCooking->NxCloseCooking();
	WriteLine("Completed cooking!");
	_getch();
	return 0;
}

int CookConvex(const char* source, const char* dest)
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

	FILE* stream;
	fopen_s(&stream, source, "rb");

	int numVertices = 0;
	int numTriangles = 0;
	NxPoint* vertices = nullptr;
	NxU32* triangles = nullptr;

	if (stream)
	{
		fread(&numVertices, sizeof(int), 1, stream);
		vertices = new NxPoint[numVertices];

		for (int i = 0; i < numVertices; i++)
		{
			fread(&vertices[i], sizeof(NxPoint), 1, stream);
			printf("%f, %f, %f\n", vertices[i].x, vertices[i].y, vertices[i].z);
		}

		fread(&numTriangles, sizeof(int), 1, stream);
		triangles = new NxU32[numTriangles];

		for (int i = 0; i < numTriangles; i++)
		{
			fread(&triangles[i], sizeof(NxU32), 1, stream);
			printf("%i\n", triangles[i]);
		}
	}

	printf("Yay! \n");
	fclose(stream);

	params.targetPlatform = PLATFORM_PC;
	gCooking->NxSetCookingParams(params);
	WriteLine("Set Cooking Params");
	CStream out(dest, false);
	WriteLine("Set IO streams");
	NxPhysicsSDK* sdk = NxCreatePhysicsSDK(NX_PHYSICS_SDK_VERSION);
	NxConvexMeshDesc desc;
	desc.pointStrideBytes = sizeof(NxPoint);
	desc.triangleStrideBytes = sizeof(NxTriangle32);
	desc.points = vertices;
	desc.numVertices = numVertices;
	desc.triangles = triangles;
	desc.numTriangles = numTriangles;
	desc.flags = NxConvexFlags::NX_CF_COMPUTE_CONVEX;
	gCooking->NxCookConvexMesh(desc, out);
	gCooking->NxCloseCooking();
	WriteLine("Completed cooking!");
	return 0;
}
