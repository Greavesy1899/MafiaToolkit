#include <iostream>

#define WIN32
#include "PhysXCooker.h"
#include "PhysXModel.h"
#include "PhysXModelBundleHandler.h"

#include <conio.h>

extern "C" int  __declspec(dllexport) _stdcall RunCookTriangleFunction(const char* source, const char* dest);

extern int _stdcall RunCookTriangleFunction(const char* source, const char* dest)
{
    printf("Called RunCookTriangleCollision\n");

	PhysXModel* NewModel = PhysXModelBundleHandler::LoadModel(source);
	PhysXCooker* Cooker = new PhysXCooker();
	Cooker->Initialise(dest);
	Cooker->CookTriangleMeshFromModel(*NewModel);
	Cooker->Deinitialise();
	return 0;
}

int main(int argc, char** argv)
{
    int result = 0;

	if ((strcmp(argv[1], "-CookTriangleMesh") == 0) && (argc >= 4))
	{
        result = RunCookTriangleFunction(argv[2], argv[3]);
	}

    return result;
}