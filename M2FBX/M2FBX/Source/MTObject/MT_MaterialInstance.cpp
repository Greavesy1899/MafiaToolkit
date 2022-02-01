#include "MT_MaterialInstance.h"

#include "Source/Utilities/FileUtils.h"

void MT_MaterialInstance::ReadFromFile(FILE* InStream)
{
	FileUtils::Read(InStream, &MaterialFlags);
	FileUtils::ReadString(InStream, &Name);

	if (HasMaterialFlag(HasDiffuse))
	{
		FileUtils::ReadString(InStream, &DiffuseTexture);
	}
}

void MT_MaterialInstance::WriteToFile(FILE* OutStream) const
{
	FileUtils::Write(OutStream, MaterialFlags);
	FileUtils::WriteString(OutStream, Name);

	if (HasMaterialFlag(HasDiffuse))
	{
		FileUtils::WriteString(OutStream, DiffuseTexture);
	}
}
