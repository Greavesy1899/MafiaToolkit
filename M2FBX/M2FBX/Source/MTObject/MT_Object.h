#pragma once

#include "MT_Lod.h"

#include <string>
#include <vector>

class MT_Collision;
class MT_ObjectHandler;
class MT_Skeleton;
struct TransformStruct;

enum MT_ObjectFlags : uint
{
	HasLODs = 1,
	HasSkinning = 2,
	HasCollisions = 4,
	HasChildren = 8,
};

enum MT_ObjectType : uint
{
	Null = 0,
	StaticMesh,
	RiggedMesh,
	Joint,
	Actor,
	ItemDesc,
	Dummy,
};

struct TransformStruct
{
	Point3 Position;
	Point3 Rotation;
	Point3 Scale;
};

class MT_Object
{

	friend MT_ObjectHandler;

public:

	bool HasObjectFlag(const MT_ObjectFlags FlagToCheck) const;
	void AddObjectFlag(const MT_ObjectFlags FlagToAdd);
	void Cleanup();

	// Accessors
	const std::string& GetName() const { return ObjectName; }
	const MT_ObjectFlags GetFlags() const { return ObjectFlags; }
	const MT_ObjectType GetType() const { return ObjectType; }
	const std::vector<MT_Object> GetChildren() const { return Children; }
	const std::vector<MT_Lod> GetLods() const { return LodObjects; }
	const TransformStruct& GetTransform() const { return Transform; }
	const MT_Collision* GetCollision() const { return CollisionObject; }
	const MT_Skeleton* GetSkeleton() const { return SkeletonObject; }

	// Setters
	void SetName(std::string& InName) { ObjectName = InName; }
	void SetObjectFlags(MT_ObjectFlags InFlags) { ObjectFlags = InFlags; }
	void SetType(MT_ObjectType InType) { ObjectType = InType; }
	void SetTransform(TransformStruct& InTransform) { Transform = InTransform; }

	// TODO: Move all these to cpp file
	void SetLods(std::vector<MT_Lod> InLods)
	{
		LodObjects = InLods;
		if (InLods.size() > 0)
		{
			AddObjectFlag(MT_ObjectFlags::HasLODs);
		}
	}
	void SetCollisions(MT_Collision* InCollision) 
	{ 
		CollisionObject = InCollision; 
		if (InCollision)
		{
			AddObjectFlag(MT_ObjectFlags::HasCollisions);
		}
	}
	void SetSkeleton(MT_Skeleton* InSkeleton)
	{
		SkeletonObject = InSkeleton;
		if (InSkeleton)
		{
			AddObjectFlag(MT_ObjectFlags::HasSkinning);
		}
	}
	void SetChildren(std::vector<MT_Object> InChildren)
	{
		Children = InChildren;
		if (InChildren.size() > 0)
		{
			AddObjectFlag(MT_ObjectFlags::HasChildren);
		}
	}

	// IO
	bool ReadFromFile(FILE* InStream);
	void WriteToFile(FILE* OutStream) const;

private:

	bool ValidateHeader(const int Magic) const;

	std::string ObjectName = "";
	MT_ObjectFlags ObjectFlags;
	MT_ObjectType ObjectType;
	TransformStruct Transform;

	std::vector<MT_Lod> LodObjects;
	std::vector<MT_Object> Children;
	MT_Collision* CollisionObject = nullptr;
	MT_Skeleton* SkeletonObject = nullptr;
};

class MT_ObjectBundle
{

	friend MT_ObjectHandler;

public:

	void Cleanup();

	// Accessor
	const std::vector<MT_Object>& GetObjects() const { return Objects; }

	// Setter
	void SetObjects(std::vector<MT_Object> InObjects) { Objects = InObjects; }

private:

	std::vector<MT_Object> Objects;
};
