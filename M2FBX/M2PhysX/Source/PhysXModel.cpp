#include "PhysXModel.h"

PhysXModel::PhysXModel()
{

}

PhysXModel::~PhysXModel()
{
	Vertices.clear();
	Vertices.resize(0);
	Indices.clear();
	Indices.resize(0);
	MaterialIDs.clear();
	MaterialIDs.resize(0);
}
