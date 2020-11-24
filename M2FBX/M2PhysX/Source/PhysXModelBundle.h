#pragma once

#include "PhysXModel.h"

#include <vector>

class PhysXModel;

/*
* A collection of models which require cooking by PhysX
*/
class PhysXModelBundle
{
public:

private:

	// Vector which contains the model collection.
	std::vector<PhysXModel*> ModelCollection;
};

