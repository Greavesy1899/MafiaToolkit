#pragma once

#include "MT_Object.h"

#include <string>

class MT_ObjectUtils
{
public:

	static void RemoveMetaTagFromName(std::string& ObjectName);
	static MT_ObjectType GetTypeFromString(const FbxString ObjectName);
	static void GetTypeAsStringClosed(const MT_ObjectType ObjectType, std::string& TypeEnclosed);
	static const char* GetTypeAsString(const MT_ObjectType ObjectType);
};
