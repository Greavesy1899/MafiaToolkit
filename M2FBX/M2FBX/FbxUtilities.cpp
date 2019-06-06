#include "FbxUtilities.h"

void InitializeSdkObjects(FbxManager*& pManager)
{
	//The first thing to do is to create the FBX Manager which is the object allocator for almost all the classes in the SDK
	pManager = FbxManager::Create();
	if (!pManager)
	{
		WriteLine("Error: Unable to create FBX Manager!");
		exit(1);
	}
	else WriteLine("Autodesk FBX SDK version %s", pManager->GetVersion());

	//Create an IOSettings object. This object holds all import/export settings.
	FbxIOSettings* ios = FbxIOSettings::Create(pManager, IOSROOT);
	pManager->SetIOSettings(ios);
}
void DestroySdkObjects(FbxManager* pManager, bool pExitStatus)
{
#if _DEBUG
	//Delete the FBX Manager. All the objects that have been allocated using the FBX Manager and that haven't been explicitly destroyed are also automatically destroyed.
	if (pManager) pManager->Destroy();
	if (pExitStatus) WriteLine("Program Success!");
#endif _DEBUG
}