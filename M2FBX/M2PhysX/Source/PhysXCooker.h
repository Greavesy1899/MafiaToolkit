#pragma once

#include "PhysXModel.h"
#include "PhysXStream.h"

#include <NxPhysics.h>
#include <NxCooking.h>

class NxCookingInterface;
class NxPhysicsSDK;
class NxTriangleMesh;
class NxTriangleMeshDesc;

/*
* Utils class to cook the loaded PhysXModels (Or ModelBundles)
*/
class PhysXCooker
{
public:

	void Initialise(const char* DestinationFile);
	void CookTriangleMeshFromModel(PhysXModel& Model);
	void Deinitialise();

private:

	void* CookTriangleMesh(const NxTriangleMeshDesc& MeshDesc);

	NxCookingInterface* CookingLib = nullptr;
	NxPhysicsSDK* PhysicsSDK = nullptr;

	const char* DestinationName;

	PhysXStream InStream;

};
