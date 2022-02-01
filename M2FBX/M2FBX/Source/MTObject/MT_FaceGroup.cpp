#include "MT_FaceGroup.h"

#include <cassert>

#include "Source/Utilities/FileUtils.h"


MT_FaceGroup::MT_FaceGroup()
{
	MaterialInstance = new MT_MaterialInstance();
	MaterialInstance->SetName("NewMaterial");
}

void MT_FaceGroup::ReadFromFile(FILE* InStream)
{
	FileUtils::Read(InStream, &StartIndex);
	FileUtils::Read(InStream, &NumFaces);

	// Read material instance data
	MaterialInstance = new MT_MaterialInstance();
	MaterialInstance->ReadFromFile(InStream);
}

void MT_FaceGroup::WriteToFile(FILE* OutStream) const
{
	assert(MaterialInstance);

	FileUtils::Write(OutStream, StartIndex);
	FileUtils::Write(OutStream, NumFaces);

	// Write material instance data
	MaterialInstance->WriteToFile(OutStream);
}
