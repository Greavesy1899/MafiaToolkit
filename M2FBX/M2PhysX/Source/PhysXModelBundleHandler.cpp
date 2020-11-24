#include "PhysXModelBundleHandler.h"

// Disable fread warnings.
#pragma warning( disable : 6387 )

#include <cstdio>

template<class T>
void Read(FILE* InFile, T* Object, NxU32 Count = 1)
{
	fread(Object, Count, sizeof(T), InFile);
}

PhysXModelBundle* PhysXModelBundleHandler::LoadBundle(const char* BundleFile)
{
	return nullptr;
}

bool PhysXModelBundleHandler::SaveBundle(const PhysXModelBundle& BundleObject, const char* NameOfFile)
{
	return false;
}

PhysXModel* PhysXModelBundleHandler::LoadModel(const char* ModelFile)
{
	FILE* InFile = nullptr;
	fopen_s(&InFile, ModelFile, "rb");

	PhysXModel* NewModel = new PhysXModel();

	// Read vertices
	NxU32 NumVertices = 0;
	Read(InFile, &NumVertices);

	std::vector<NxVec3> Vertices;
	Vertices.resize(NumVertices);
	Read(InFile, Vertices.data(), Vertices.size());
	NewModel->SetVertices(Vertices);

	// Read Indices
	NxU32 NumIndices = 0;
	Read(InFile, &NumIndices);

	std::vector<NxU32> Indices;
	Indices.resize(NumIndices);
	Read(InFile, Indices.data(), Indices.size());
	NewModel->SetIndices(Indices);

	// Read MaterialIDs
	NxU32 NumMaterialIDs = 0;
	Read(InFile, &NumMaterialIDs);

	std::vector<NxU16> MaterialIDs;
	MaterialIDs.resize(NumMaterialIDs);
	Read(InFile, MaterialIDs.data(), MaterialIDs.size());
	NewModel->SetMaterialIDs(MaterialIDs);
	fclose(InFile);

	return NewModel;
}

bool PhysXModelBundleHandler::SaveModel(const PhysXModel& ModelObject, const char* NameOfFile)
{
	return false;
}
