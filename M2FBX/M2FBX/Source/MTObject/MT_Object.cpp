#include "MT_Object.h"

#include "MT_Collision.h"
#include "MT_Skeleton.h"
#include "Utilities/FileUtils.h"

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
	for (auto& LodObject : LodObjects)
	{
		LodObject.Cleanup();
	}

	LodObjects.clear();

	// Empty Children
	for (auto& ChildObject : Children)
	{
		ChildObject.Cleanup();
	}

	Children.clear();

	// Cleanup Collision
	if (CollisionObject)
	{
		CollisionObject->Cleanup();
		CollisionObject = nullptr;
	}

	// Cleanup Skeleton
	if (SkeletonObject)
	{
		// TODO: Maybe do cleanup?
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
			MT_Lod LodObject = {};
			LodObject.ReadFromFile(InStream);
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
			MT_Object ChildObject = {};
			ChildObject.ReadFromFile(InStream);
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
			const MT_Lod& LodInfo = LodObjects[i];
			LodInfo.WriteToFile(OutStream);
		}
	}

	if (HasObjectFlag(MT_ObjectFlags::HasChildren))
	{
		FileUtils::Write(OutStream, (uint)Children.size());
		for (size_t i = 0; i < Children.size(); i++)
		{
			const MT_Object& ChildObject = Children[i];
			ChildObject.WriteToFile(OutStream);
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

void MT_ObjectBundle::Cleanup()
{
	for (auto& Object : Objects)
	{
		Object.Cleanup();
	}
}
