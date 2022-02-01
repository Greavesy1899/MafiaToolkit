#include "MT_Lod.h"

#include "Utilities/FileUtils.h"

void MT_MaterialInstance::Cleanup()
{
	Name = "";
	DiffuseTexture = "";
}

bool MT_MaterialInstance::HasMaterialFlag(const MT_MaterialInstanceFlags Flag) const
{
	return (MaterialFlags & Flag);
}

void MT_FaceGroup::Cleanup()
{
	if (MaterialInstance)
	{
		MaterialInstance->Cleanup();
		MaterialInstance = nullptr;
	}
}

void MT_Lod::AddVertexFlag(const VertexFlags Flag)
{
	bool bHasFlag = (VertexDeclaration & Flag);
	if (!bHasFlag)
	{
		uint Value = VertexDeclaration | Flag;
		VertexDeclaration = (VertexFlags)Value;
	}
}

bool MT_Lod::HasVertexFlag(const VertexFlags Flag) const
{
	return (VertexDeclaration & Flag);
}

void MT_Lod::Cleanup()
{
	// Empty FaceGroups
	for (auto& FaceGroup : FaceGroups)
	{
		FaceGroup.Cleanup();
	}

	// Cleanup other vectors
	FaceGroups.clear();
	Vertices.clear();
	Indices.clear();
}

void MT_Lod::ReadFromFile(FILE* InStream)
{
	FileUtils::Read(InStream, &VertexDeclaration);

	// Read the Vertices
	uint NumVertices = 0;
	FileUtils::Read(InStream, &NumVertices);
	Vertices.resize(NumVertices);

	for (uint i = 0; i < NumVertices; i++)
	{
		Vertex NewVertex = Vertex();
		if (HasVertexFlag(VertexFlags::Position))
		{
			FileUtils::Read(InStream, &NewVertex.position);
		}
		if (HasVertexFlag(VertexFlags::Normals))
		{
			FileUtils::Read(InStream, &NewVertex.normals);
		}
		if (HasVertexFlag(VertexFlags::Tangent))
		{
			FileUtils::Read(InStream, &NewVertex.tangent);
		}
		if (HasVertexFlag(VertexFlags::Skin))
		{
			FileUtils::Read(InStream, &NewVertex.boneIDs);
			FileUtils::Read(InStream, &NewVertex.boneWeights);
		}
		if (HasVertexFlag(VertexFlags::DamageGroup))
		{
			FileUtils::Read(InStream, &NewVertex.damageGroup);
		}
		if (HasVertexFlag(VertexFlags::Color))
		{
			FileUtils::Read(InStream, &NewVertex.color0);
		}
		if (HasVertexFlag(VertexFlags::Color1))
		{
			FileUtils::Read(InStream, &NewVertex.color1);
		}
		if (HasVertexFlag(VertexFlags::TexCoords0))
		{
			FileUtils::Read(InStream, &NewVertex.uv0);
		}
		if (HasVertexFlag(VertexFlags::TexCoords1))
		{
			FileUtils::Read(InStream, &NewVertex.uv1);
		}
		if (HasVertexFlag(VertexFlags::TexCoords2))
		{
			FileUtils::Read(InStream, &NewVertex.uv2);
		}
		if (HasVertexFlag(VertexFlags::ShadowTexture))
		{
			FileUtils::Read(InStream, &NewVertex.uv3);
		}

		Vertices[i] = NewVertex;
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

void MT_Lod::WriteToFile(FILE* OutStream) const
{
	FileUtils::Write(OutStream, VertexDeclaration);
	FileUtils::Write(OutStream, (uint)Vertices.size());

	for (int x = 0; x < Vertices.size(); x++)
	{
		const Vertex& VertexInfo = Vertices[x];
		if (HasVertexFlag(VertexFlags::Position))
		{
			FileUtils::Write(OutStream, VertexInfo.position);
		}
		if (HasVertexFlag(VertexFlags::Normals))
		{
			FileUtils::Write(OutStream, VertexInfo.normals);
		}
		if (HasVertexFlag(VertexFlags::Tangent))
		{
			FileUtils::Write(OutStream, VertexInfo.tangent);
		}
		if (HasVertexFlag(VertexFlags::Skin))
		{
			FileUtils::Write(OutStream, VertexInfo.boneIDs);
			FileUtils::Write(OutStream, VertexInfo.boneWeights);
		}
		if (HasVertexFlag(VertexFlags::DamageGroup))
		{
			FileUtils::Write(OutStream, VertexInfo.damageGroup);
		}
		if (HasVertexFlag(VertexFlags::Color))
		{
			FileUtils::Write(OutStream, VertexInfo.color0);
		}
		if (HasVertexFlag(VertexFlags::Color1))
		{
			FileUtils::Write(OutStream, VertexInfo.color1);
		}
		if (HasVertexFlag(VertexFlags::TexCoords0))
		{
			FileUtils::Write(OutStream, VertexInfo.uv0);
		}
		if (HasVertexFlag(VertexFlags::TexCoords1))
		{
			FileUtils::Write(OutStream, VertexInfo.uv1);
		}
		if (HasVertexFlag(VertexFlags::TexCoords2))
		{
			FileUtils::Write(OutStream, VertexInfo.uv2);
		}
		if (HasVertexFlag(VertexFlags::ShadowTexture))
		{
			FileUtils::Write(OutStream, VertexInfo.uv3);
		}
	}

	// Write FaceGroups
	FileUtils::Write(OutStream, (uint)FaceGroups.size());
	for (uint i = 0; i < FaceGroups.size(); i++)
	{
		// Write the FaceGroup
		const MT_FaceGroup& FaceGroupInfo = FaceGroups[i];
		FaceGroupInfo.WriteToFile(OutStream);
	}

	// Multiply by 3 because indices are stored as Int3's
	FileUtils::Write(OutStream, (uint)Indices.size() * 3);
	for (uint i = 0; i < (uint)Indices.size(); i++)
	{
		FileUtils::Write(OutStream, Indices[i]);
	}
}
