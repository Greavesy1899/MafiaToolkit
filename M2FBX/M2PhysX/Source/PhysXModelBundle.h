#pragma once

#include "PhysXModel.h"

#include <vector>

class PhysXModel;
class PhysXStream;

/*
* A collection of models which require cooking by PhysX
*/
class PhysXModelBundle
{
public:

	void AddToCollection(PhysXModel* Model);

	void CookModelBundle(PhysXStream* OutStream);

	NxU32 GetNumModels() { return ModelCollection.size(); }

private:

	// Vector which contains the model collection.
	std::vector<PhysXModel*> ModelCollection;
};

