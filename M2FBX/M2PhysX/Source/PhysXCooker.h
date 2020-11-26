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

	void Initialise();
	void CookTriangleMeshFromModel(PhysXModel& Model, PhysXStream* OutStream);
	void Deinitialise();

private:

	void* CookTriangleMesh(const NxTriangleMeshDesc& MeshDesc, NxU32& CookedSize, PhysXStream* OutStream);

	NxCookingInterface* CookingLib = nullptr;
	NxPhysicsSDK* PhysicsSDK = nullptr;

};
