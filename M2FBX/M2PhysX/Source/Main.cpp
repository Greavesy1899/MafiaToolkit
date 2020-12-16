#include <iostream>

#define WIN32
#include "PhysXCooker.h"
#include "PhysXModel.h"
#include "PhysXModelBundle.h"
#include "PhysXModelBundleHandler.h"

#include <conio.h>

extern "C" int  __declspec(dllexport) _stdcall RunCookTriangle(const char* source, const char* dest);
extern "C" int __declspec(dllexport) _stdcall RunMultiCookTriangle(const char* source, const char* dest);

extern int _stdcall RunMultiCookTriangle(const char* source, const char* dest)
{
	printf("Called RunMultiCookTriangleCollision\n");
	PhysXModelBundle* NewBundle = PhysXModelBundleHandler::LoadBundle(source);

	PhysXStream* OutStream = new PhysXStream();
	OutStream->OpenStream(dest, "wb");
	OutStream->storeDword(NewBundle->GetNumModels());
	NewBundle->CookModelBundle(OutStream);
	OutStream->CloseStream();

	delete OutStream;
	delete NewBundle;
	OutStream = nullptr;
	NewBundle = nullptr;
	return 0;
}

extern int _stdcall RunCookTriangle(const char* source, const char* dest)
{
    printf("Called RunCookTriangleCollision\n");

	PhysXModel* NewModel = PhysXModelBundleHandler::LoadModel(source);
	PhysXStream* OutStream = new PhysXStream();
	OutStream->OpenStream(dest, "wb");
	PhysXCooker* Cooker = new PhysXCooker();
	Cooker->Initialise();
	Cooker->CookTriangleMeshFromModel(*NewModel, OutStream);
	Cooker->Deinitialise();
	OutStream->CloseStream();
	
	delete NewModel;
	delete Cooker;
	delete OutStream;

	return 0;
}

int main(int argc, char** argv)
{
    int result = 0;

	if ((strcmp(argv[1], "-CookTriangleMesh") == 0) && (argc >= 4))
	{
        result = RunCookTriangle(argv[2], argv[3]);
	}
	else if ((strcmp(argv[1], "-MultiCookTriangleMesh") == 0) && (argc >= 4))
	{
		result = RunMultiCookTriangle(argv[2], argv[3]);
	}

    return result;
}