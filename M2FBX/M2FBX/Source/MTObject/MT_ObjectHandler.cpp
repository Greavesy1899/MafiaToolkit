#include "MT_ObjectHandler.h"

#include "MT_Animation.h"
#include "MT_Object.h"
#include "Utilities/FileUtils.h"

#include <cstdio>

MT_ObjectBundle* MT_ObjectHandler::ReadBundleFromFile(const std::string& FileName)
{
	FILE* InStream = nullptr;
	fopen_s(&InStream, FileName.data(), "rb");

	if (!InStream)
	{
		// Failed
		return nullptr;
	}

	// Try Validate Header
	uint Magic = 0;
	fread(&Magic, sizeof(uint), 1, InStream);
	if (Magic != 4346957)
	{
		// TODO: Probably send to MT_Bundle to validate?
		// Invalid
		return nullptr;
	}

	MT_ObjectBundle* BundleObject = new MT_ObjectBundle();

	uint NumObjects = 0;
	FileUtils::Read(InStream, &NumObjects);
	BundleObject->Objects.resize(NumObjects);
	for (uint i = 0; i < NumObjects; i++)
	{
		MT_Object* NewObject = new MT_Object();
		bool bIsValid = ReadObjectFromFile(InStream, NewObject);
		if (!bIsValid)
		{
			// Failed
			return nullptr;
		}

		BundleObject->Objects[i] = NewObject;
	}

	int32_t Offet = ftell(InStream);

	uint HasAnimation = 0;
	FileUtils::Read(InStream, &HasAnimation);
	if (HasAnimation)
	{
		BundleObject->Animation = new MT_Animation();
		BundleObject->Animation->ReadFromFile(InStream);
	}

	fclose(InStream);

	return BundleObject;
}

void MT_ObjectHandler::WriteBundleToFile(const std::string& FileName, const MT_ObjectBundle& Bundle)
{
	FILE* OutStream = nullptr;
	fopen_s(&OutStream, FileName.data(), "wb");

	if (!OutStream)
	{
		// Failed
		return;
	}

	// Write magic
	FileUtils::Write(OutStream, (uint)4346957);
	FileUtils::Write(OutStream, (uint)Bundle.Objects.size());

	// Write Objects
	for (const MT_Object* Object : Bundle.Objects)
	{
		WriteObjectToFile(OutStream, *Object);
	}

	fclose(OutStream);
}

MT_Object* MT_ObjectHandler::ReadObjectFromFile(const std::string& FileName)
{
	FILE* InStream = nullptr;
	fopen_s(&InStream, FileName.data(), "rb");

	if (!InStream)
	{
		// Failed
		return nullptr;
	}

	// Begin reading the file
	MT_Object* NewObject = new MT_Object();
	bool bIsValid = ReadObjectFromFile(InStream, NewObject);
	if (!bIsValid)
	{
	// Failed
		return nullptr;
	}

	fclose(InStream);

	return NewObject;
}

void MT_ObjectHandler::WriteObjectToFile(const std::string& FileName, const MT_Object& Object)
{
	FILE* OutStream = nullptr;
	fopen_s(&OutStream, FileName.data(), "wb");

	if (!OutStream)
	{
		// Failed
		return;
	}

	WriteObjectToFile(OutStream, Object);
	fclose(OutStream);
}

bool MT_ObjectHandler::ReadObjectFromFile(FILE* InStream, MT_Object* NewObject)
{
	NewObject->ReadFromFile(InStream);

	return true;
}

void MT_ObjectHandler::WriteObjectToFile(FILE* OutStream, const MT_Object& Object)
{
	Object.WriteToFile(OutStream);
}
