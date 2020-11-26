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
	FILE* InFile = nullptr;
	fopen_s(&InFile, BundleFile, "rb");

	PhysXModelBundle* ModelBundle = new PhysXModelBundle();
	
	// Read number of models.
	NxU32 NumModels = 0;
	Read(InFile, &NumModels);

	for (NxU32 i = 0; i < NumModels; i++)
	{
		PhysXModel* NewModel = InternalLoadModel(InFile);
		ModelBundle->AddToCollection(NewModel);
	}

	fclose(InFile);
	return ModelBundle;
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
	NewModel = InternalLoadModel(InFile);
	return NewModel;
}

bool PhysXModelBundleHandler::SaveModel(const PhysXModel& ModelObject, const char* NameOfFile)
{
	return false;
}

PhysXModel* PhysXModelBundleHandler::InternalLoadModel(FILE* InStream)
{
	PhysXModel* NewModel = new PhysXModel();

	// Read vertices
	NxU32 NumVertices = 0;
	Read(InStream, &NumVertices);

	std::vector<NxVec3> Vertices;
	Vertices.resize(NumVertices);
	Read(InStream, Vertices.data(), Vertices.size());
	NewModel->SetVertices(Vertices);

	// Read Indices
	NxU32 NumIndices = 0;
	Read(InStream, &NumIndices);

	std::vector<NxU32> Indices;
	Indices.resize(NumIndices);
	Read(InStream, Indices.data(), Indices.size());
	NewModel->SetIndices(Indices);

	// Read MaterialIDs
	NxU32 NumMaterialIDs = 0;
	Read(InStream, &NumMaterialIDs);

	std::vector<NxU16> MaterialIDs;
	MaterialIDs.resize(NumMaterialIDs);
	Read(InStream, MaterialIDs.data(), MaterialIDs.size());
	NewModel->SetMaterialIDs(MaterialIDs);

	return NewModel;
}
