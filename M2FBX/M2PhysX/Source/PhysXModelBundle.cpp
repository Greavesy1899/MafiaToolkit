#include "PhysXModelBundle.h"

#include "PhysXCooker.h"
#include "PhysXStream.h"

void PhysXModelBundle::AddToCollection(PhysXModel* Model)
{
	ModelCollection.push_back(Model);
}

void PhysXModelBundle::CookModelBundle(PhysXStream* OutStream)
{
	for (int i = 0; i < ModelCollection.size(); i++)
	{
		PhysXCooker* Cooker = new PhysXCooker();
		Cooker->Initialise();
		Cooker->CookTriangleMeshFromModel(*ModelCollection[i], OutStream);		
		Cooker->Deinitialise();
		delete Cooker;
	}
}
