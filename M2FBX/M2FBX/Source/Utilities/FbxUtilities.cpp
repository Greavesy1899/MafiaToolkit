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

void Fbx_Utilities::ConvertToOppositeSerialization(const char* Source, const char* Dest)
{
	FbxManager* SdkManager = nullptr;

	Fbx_Utilities::InitializeSdkObjects(SdkManager);
	if (SdkManager)
	{
		FbxImporter* Importer = FbxImporter::Create(SdkManager, "");
		FBX_ASSERT(Importer);

		// Attempt to initialise the scene
		if (!Importer->Initialize(Source, -1, SdkManager->GetIOSettings()))
		{
			return;
		}

		// move imported data into scene
		FbxScene* Scene = FbxScene::Create(SdkManager, "Scene");
		if (!Scene)
		{
			return;
		}

		// Attempt to import the scene
		if (!Importer->Import(Scene))
		{
			return;
		}

		const int ExportFormat = (Importer->GetFileFormat() == 1 ? 0 : 1);

		// destroy importer
		Importer->Destroy();
		Importer = nullptr;

		FbxExporter* Exporter = FbxExporter::Create(SdkManager, "");

		// Set the export states. By default, the export states are always set to 
		// true except for the option eEXPORT_TEXTURE_AS_EMBEDDED. The code below 
		// shows how to change these states.
		IOS_REF.SetBoolProp(EXP_FBX_MATERIAL, true);
		IOS_REF.SetBoolProp(EXP_FBX_TEXTURE, true);
		IOS_REF.SetBoolProp(EXP_FBX_EMBEDDED, false);
		IOS_REF.SetBoolProp(EXP_FBX_ANIMATION, true);
		IOS_REF.SetBoolProp(EXP_FBX_GLOBAL_SETTINGS, true);

		// attempt to initialise
		if (Exporter->Initialize(Dest, 1, SdkManager->GetIOSettings()))
		{
			// attempt to export
			Exporter->Export(Scene);

			// destroy
			Exporter->Destroy();
			Exporter = nullptr;
		}

		Fbx_Utilities::DestroySdkObjects(SdkManager, true);
	}
}

bool Fbx_Utilities::FindInString(const FbxString& Text, const FbxString& StringToFind)
{
	int Result = Text.Find(StringToFind);
	bool bIsFound = Result != std::string::npos;
	return bIsFound;
}

static const uint64_t Initial = 0xCBF29CE484222325;
uint64_t Fbx_Utilities::FNV64_Hash(const std::string& Text)
{
	uint64_t Hash = Initial;

	for (size_t i = 0; i < Text.size(); i++)
	{
		Hash *= 0x00000100000001B3;
		Hash ^= Text[i];
	}

	return Hash;
}
