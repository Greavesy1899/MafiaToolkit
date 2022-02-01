#include "FbxUtilities.h"

void Fbx_Utilities::InitializeSdkObjects(FbxManager*& pManager)
{
	//The first thing to do is to create the FBX Manager which is the object allocator for almost all the classes in the SDK
	pManager = FbxManager::Create();
	if (!pManager)
	{
		FBXSDK_printf("Error: Unable to create FBX Manager!\n");
		exit(1);
	}
	else
	{
		FBXSDK_printf("Autodesk FBX SDK version %s\n", pManager->GetVersion());
	}

	//Create an IOSettings object. This object holds all import/export settings.
	FbxIOSettings* ios = FbxIOSettings::Create(pManager, IOSROOT);
	pManager->SetIOSettings(ios);
}
void Fbx_Utilities::DestroySdkObjects(FbxManager* pManager, bool pExitStatus)
{
#if _DEBUG
	//Delete the FBX Manager. All the objects that have been allocated using the FBX Manager and that haven't been explicitly destroyed are also automatically destroyed.
	if (pManager)
	{
		pManager->Destroy();
		pManager = nullptr;
	}
	if (pExitStatus)
	{
		FBXSDK_printf("Program Success!\n");
	}
#endif _DEBUG
}

bool Fbx_Utilities::FindInString(const FbxString& Text, const FbxString& StringToFind)
{
	int Result = Text.Find(StringToFind);
	bool bIsFound = Result != std::string::npos;
	return bIsFound;
}
