#include "MT_Object.h"

#include "MT_Collision.h"
#include "MT_Skeleton.h"
#include "Utilities/FileUtils.h"

MT_Object::~MT_Object()
{
	for (MT_Lod* CurLod : LodObjects)
	{
		CurLod->Cleanup();

		delete CurLod;
		CurLod = nullptr;
	}

	LodObjects.clear();

	for (MT_Object* CurObj : Children)
	{
		CurObj->Cleanup();

		delete CurObj;
		CurObj = nullptr;
	}

	Children.clear();
}

bool MT_Object::HasObjectFlag(const MT_ObjectFlags FlagToCheck) const
{
	return (ObjectFlags & FlagToCheck);
}

void MT_Object::AddObjectFlag(const MT_ObjectFlags FlagToAdd)
{
	int Temp = ObjectFlags;
	Temp |= FlagToAdd;
	ObjectFlags = (MT_ObjectFlags)Temp;
}

void MT_Object::Cleanup()
{
	ObjectName = "";

	// Empty LodObjects
	for (MT_Lod* LodObject : LodObjects)
	{
		LodObject->Cleanup();
		delete LodObject;
	}

	LodObjects.clear();

	// Empty Children
	for (MT_Object* ChildObject : Children)
	{
		ChildObject->Cleanup();
		delete ChildObject;
	}

	Children.clear();

	// Cleanup Collision
	if (CollisionObject)
	{
		CollisionObject->Cleanup();
		delete CollisionObject;
		CollisionObject = nullptr;
	}

	// Cleanup Skeleton
	if (SkeletonObject)
	{
		// TODO: Maybe do cleanup?
		delete SkeletonObject;
		SkeletonObject = nullptr;
	}
}

bool MT_Object::ReadFromFile(FILE* InStream)
{
	// Try Validate Header
	uint Magic = 0;
	FileUtils::Read(InStream, &Magic);
	if (!ValidateHeader(Magic))
	{
		return false;
	}

	// Read Name and Flags
	FileUtils::ReadString(InStream, &ObjectName);
	FileUtils::Read(InStream, &ObjectFlags);
	FileUtils::Read(InStream, &ObjectType);

	FileUtils::Read(InStream, &Transform);

	// Read LODs
	if (HasObjectFlag(MT_ObjectFlags::HasLODs))
	{
		// Read LODS Size
		uint NumLODs = 0;
		FileUtils::Read(InStream, &NumLODs);
		LodObjects.resize(NumLODs);

		for (uint i = 0; i < NumLODs; i++)
		{
			// Begin to read LOD
			MT_Lod* LodObject = new MT_Lod();
			LodObject->ReadFromFile(InStream);
			LodObjects[i] = LodObject;
		}
	}
	
	// Read Children
	if (HasObjectFlag(MT_ObjectFlags::HasChildren))
	{
		// Read Children Count
		uint NumChildren = 0;
		FileUtils::Read(InStream, &NumChildren);
		Children.resize(NumChildren);

		for (uint i = 0; i < NumChildren; i++)
		{
			MT_Object* ChildObject = new MT_Object();
			ChildObject->ReadFromFile(InStream);
			Children[i] = ChildObject;
		}
	}

	// Read Collision
	if (HasObjectFlag(MT_ObjectFlags::HasCollisions))
	{
		CollisionObject = new MT_Collision();
		CollisionObject->ReadFromFile(InStream);
	}

	// Read Skeleton
	if (HasObjectFlag(MT_ObjectFlags::HasSkinning))
	{
		SkeletonObject = new MT_Skeleton();
		SkeletonObject->ReadFromFile(InStream);
	}

	return true;
}

void MT_Object::WriteToFile(FILE* OutStream) const
{
	// Write magic
	FileUtils::Write(OutStream, (uint)55530573);

	// Begin to Write
	FileUtils::WriteString(OutStream, ObjectName);
	FileUtils::Write(OutStream, ObjectFlags);
	FileUtils::Write(OutStream, ObjectType);

	FileUtils::Write(OutStream, Transform);

	// Write LODs
	if (HasObjectFlag(MT_ObjectFlags::HasLODs))
	{
		FileUtils::Write(OutStream, (uint)LodObjects.size());

		for (int i = 0; i < LodObjects.size(); i++)
		{
			const MT_Lod* LodInfo = LodObjects[i];
			LodInfo->WriteToFile(OutStream);
		}
	}

	if (HasObjectFlag(MT_ObjectFlags::HasChildren))
	{
		FileUtils::Write(OutStream, (uint)Children.size());
		for (size_t i = 0; i < Children.size(); i++)
		{
			const MT_Object* ChildObject = Children[i];
			ChildObject->WriteToFile(OutStream);
		}
	}

	// Write Collisions
	if (HasObjectFlag(MT_ObjectFlags::HasCollisions))
	{
		CollisionObject->WriteToFile(OutStream);
	}

	// Write Skeleton
	if (HasObjectFlag(MT_ObjectFlags::HasSkinning))
	{
		SkeletonObject->WriteToFile(OutStream);
	}
}

bool MT_Object::ValidateHeader(const int Magic) const
{
	// MTO, version 3
	if (Magic == 55530573)
	{
		return true;
	}

	return false;
}

MT_ObjectBundle::~MT_ObjectBundle()
{
	Cleanup();
}

void MT_ObjectBundle::Cleanup()
{
	for (MT_Object* Object : Objects)
	{
		Object->Cleanup();
		delete Object;
	}

	Objects.clear();

	if (Animation)
	{
		delete Animation;
		Animation = nullptr;
	}
}
