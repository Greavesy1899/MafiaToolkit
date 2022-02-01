#pragma once

#include "Source/DataTypes.h"

#include <vector>
#include <string>

struct JointMatrix
{
	Point3 Position;
	Quaternion Rotation;
	Point3 Scale;
};

enum MT_JointUsage
{
	LOD0 = 1,
	LOD1 = 2,
	LOD2 = 4,
};

class MT_Joint
{
public:

	// Getters
	const std::string& GetName() const { return Name; }
	const std::string& GetParentName() const { return ParentName; }
	MT_JointUsage GetUsage() const { return UsageFlags; }
	const JointMatrix& GetTransform() const { return Transform; }
	int GetParentJointIndex() const { return ParentJointIndex; }

	// Setters
	void SetName(const std::string& InName) { Name = InName; }
	void SetParentName(const std::string& InParentName) { ParentName = InParentName; }
	void SetUsage(const MT_JointUsage InUsageFlags) { UsageFlags = InUsageFlags; }
	void SetTransform(const JointMatrix& InTransform) { Transform = InTransform; }
	void SetParentJointIndex(const int InParentIndex) { ParentJointIndex = InParentIndex; }

	// IO
	bool ReadFromFile(FILE* InStream);
	void WriteToFile(FILE* OutStream) const;

private:

	std::string Name;
	MT_JointUsage UsageFlags;
	JointMatrix Transform;
	int ParentJointIndex;

	// NOT SERIALIZED
	std::string ParentName;

};

class MT_Skeleton
{
public:

	// Getters
	std::vector<MT_Joint> GetJoints() const { return Joints; }

	// Setters
	void SetJoints(std::vector<MT_Joint>& InJoints) { Joints = InJoints; }

	// IO
	bool ReadFromFile(FILE* InStream);
	void WriteToFile(FILE* OutStream) const;

private:

	std::vector<MT_Joint> Joints;
};
