#include "MT_Collision.h"

#include "Utilities/FileUtils.h"

void MT_Collision::Cleanup()
{
	// Cleanup other vectors
	FaceGroups.clear();
	Vertices.clear();
	Indices.clear();
}

void MT_Collision::ReadFromFile(FILE* InStream)
{
	// Read the Vertices
	uint NumVertices = 0;
	FileUtils::Read(InStream, &NumVertices);
	Vertices.resize(NumVertices);
	for (size_t i = 0; i < Vertices.size(); i++)
	{
		FileUtils::Read(InStream, &Vertices[i]);
	}

	// Read the FaceGroups
	uint NumFaceGroups = 0;
	FileUtils::Read(InStream, &NumFaceGroups);
	FaceGroups.resize(NumFaceGroups);
	for (uint i = 0; i < NumFaceGroups; i++)
	{
		// Read FaceGroup
		MT_FaceGroup FaceGroup = MT_FaceGroup();
		FaceGroup.ReadFromFile(InStream);

		FaceGroups[i] = FaceGroup;
	}

	// Read the Indices
	uint NumIndices = 0;
	FileUtils::Read(InStream, &NumIndices);
	for (uint i = 0; i < NumIndices / 3; i++)
	{
		Int3 Triangle = {};
		FileUtils::Read(InStream, &Triangle);
		Indices.push_back(Triangle);
	}
}

void MT_Collision::WriteToFile(FILE* OutStream) const
{
	// Write Vertices
	FileUtils::Write(OutStream, (uint)Vertices.size());
	for (size_t i = 0; i < Vertices.size(); i++)
	{
		FileUtils::Write(OutStream, Vertices[i]);
	}

	// Write FaceGroups
	FileUtils::Write(OutStream, (uint)FaceGroups.size());
	for (size_t i = 0; i < FaceGroups.size(); i++)
	{
		// Write the FaceGroup
		const MT_FaceGroup& FaceGroupInfo = FaceGroups[i];
		FaceGroupInfo.WriteToFile(OutStream);
	}

	// Multiply by 3 because indices are stored as Int3's
	FileUtils::Write(OutStream, (uint)Indices.size() * 3);
	for (size_t i = 0; i < (uint)Indices.size(); i++)
	{
		FileUtils::Write(OutStream, Indices[i]);
	}
}
