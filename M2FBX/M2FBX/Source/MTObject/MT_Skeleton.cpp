#include "MT_Skeleton.h"

#include "Utilities/FileUtils.h"

bool MT_Joint::ReadFromFile(FILE* InStream)
{
	FileUtils::ReadString(InStream, &Name);
	FileUtils::Read(InStream, &UsageFlags);
	FileUtils::Read(InStream, &Transform);
	FileUtils::Read(InStream, &ParentJointIndex);

	return true;
}

void MT_Joint::WriteToFile(FILE* OutStream) const
{
	FileUtils::WriteString(OutStream, Name);
	FileUtils::Write(OutStream, UsageFlags);
	FileUtils::Write(OutStream, Transform);
	FileUtils::Write(OutStream, ParentJointIndex);
}

bool MT_Skeleton::ReadFromFile(FILE* InStream)
{
	uint NumJoints = 0;
	FileUtils::Read(InStream, &NumJoints);
	Joints.resize(NumJoints);
	for (uint i = 0; i < NumJoints; i++)
	{
		// TODO: Do validity
		MT_Joint NewJoint = {};
		NewJoint.ReadFromFile(InStream);
		Joints[i] = NewJoint;
	}

	return true;
}

void MT_Skeleton::WriteToFile(FILE* OutStream) const
{
	FileUtils::Write(OutStream, (uint)Joints.size());
	for (const MT_Joint& JointObject : Joints)
	{
		JointObject.WriteToFile(OutStream);
	}
}
