#include "MT_ObjectUtils.h"

#include <fbxsdk.h>

namespace MT_ObjectUtils_Consts
{
	const char* ConstMesh = "MESH";
	const char* ConstRigged = "RIGD";
	const char* ConstItemDesc = "ITEM";
	const char* ConstActor = "ACTR";
	const char* ConstDummy = "DUMY";
	const char* ConstJoint = "JOIT";
	const char* ConstNull = "NULL";
}

void MT_ObjectUtils::RemoveMetaTagFromName(std::string& ObjectName)
{
	size_t Offset = ObjectName.find('[');
	if (Offset != std::string::npos)
	{
		size_t EndOffset = Offset + 5;
		if (ObjectName[EndOffset] == ']')
		{
			ObjectName.erase(ObjectName.begin() + Offset, ObjectName.end());
		}
	}

	// TODO: Assert or Log here
}

MT_ObjectType MT_ObjectUtils::GetTypeFromString(const FbxString ObjectName)
{
	if (ObjectName.Find(MT_ObjectUtils_Consts::ConstMesh) != std::string::npos)
	{
		return MT_ObjectType::StaticMesh;
	}
	else if (ObjectName.Find(MT_ObjectUtils_Consts::ConstRigged) != std::string::npos)
	{
		return MT_ObjectType::RiggedMesh;
	}
	else if (ObjectName.Find(MT_ObjectUtils_Consts::ConstItemDesc) != std::string::npos)
	{
		return MT_ObjectType::ItemDesc;
	}
	else if (ObjectName.Find(MT_ObjectUtils_Consts::ConstDummy) != std::string::npos)
	{
		return MT_ObjectType::Dummy;
	}
	else if (ObjectName.Find(MT_ObjectUtils_Consts::ConstActor) != std::string::npos)
	{
		return MT_ObjectType::Actor;
	}
	else if (ObjectName.Find(MT_ObjectUtils_Consts::ConstJoint) != std::string::npos)
	{
		return MT_ObjectType::Joint;
	}
	else if (ObjectName.Find(MT_ObjectUtils_Consts::ConstNull) != std::string::npos)
	{
		return MT_ObjectType::Null;
	}
	else
	{
		return MT_ObjectType::Null;
	}
}

void MT_ObjectUtils::GetTypeAsStringClosed(const MT_ObjectType ObjectType, std::string& TypeEnclosed)
{
	TypeEnclosed += "[";
	TypeEnclosed += GetTypeAsString(ObjectType);
	TypeEnclosed += "]";
}

const char* MT_ObjectUtils::GetTypeAsString(const MT_ObjectType ObjectType)
{
	switch (ObjectType)
	{
	case StaticMesh:
	{
		return MT_ObjectUtils_Consts::ConstMesh;
	}
	case RiggedMesh:
	{
		return MT_ObjectUtils_Consts::ConstRigged;
	}
	case ItemDesc:
	{
		return MT_ObjectUtils_Consts::ConstItemDesc;
	}
	case Actor:
	{
		return MT_ObjectUtils_Consts::ConstActor;
	}
	case Dummy:
	{
		return MT_ObjectUtils_Consts::ConstDummy;
	}
	case Joint:
	{
		return MT_ObjectUtils_Consts::ConstJoint;
	}
	}

	return MT_ObjectUtils_Consts::ConstNull;
}
