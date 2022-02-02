#pragma once

#include "Source/Common.h"

#ifdef IOS_REF
#undef  IOS_REF
#define IOS_REF (*(SdkManager->GetIOSettings()))
#endif

class Fbx_Utilities
{
public:

	static void InitializeSdkObjects(FbxManager*& pManager);
	static void DestroySdkObjects(FbxManager* pManager, bool pExitStatus);

	static bool FindInString(const FbxString& Text, const FbxString& StringToFind);
	
	// TODO: Find a new home for this. Perhaps generalised utlity header?

	// Convert string to FNV64 hash.
	static uint64_t FNV64_Hash(const std::string& Text);
};
