#include <fbxsdk.h>
#include "FbxWrangler.h"
#include "M2TWrangler.h"
#include "CPhysXCooking.h"
#include "M2Model.h"
#include <conio.h>

void PrintHelp()
{
	WriteLine("Possible Commands:");
	WriteLine(""); //space
	WriteLine("To convert M2T to FBX:");
	WriteLine("M2FBX.exe -ConvertToFBX \"model_source\" \"model_destination\"");
	WriteLine(""); //space
	WriteLine("To convert FBX to M2T:");
	WriteLine("M2FBX.exe -ConvertToM2T \"model_source\" \"model_destination\"");
	WriteLine(""); //space
	WriteLine("To Cook collision data:");
	WriteLine("M2FBX.exe -CookCollisions \"uncook_source\" \"cook_source\"");
}

void PrintError(int code)
{
	switch (code)
	{
	case 0:
		WriteLine("ERROR: This program requires 4 arguments to run.");
		break;
	case 1:
		WriteLine("ERROR: No command inputted.. Press any key to continue.");
		break;
	default:
		WriteLine("ERROR: Unknown Error Code.");
	}
	PrintHelp();
	_getch();
}

extern "C" int  __declspec(dllexport) _stdcall RunConvertFBX(const char* source, const char* dest);
extern "C" int  __declspec(dllexport) _stdcall RunConvertM2T(const char* source, const char* dest);
extern "C" int  __declspec(dllexport) _stdcall RunCookCollision(const char* source, const char* dest);

extern int _stdcall RunConvertFBX(const char* source, const char* dest)
{
	return ConvertFBX(source, dest);
}
extern int _stdcall RunConvertM2T(const char* source, const char* dest)
{
	return ConvertM2T(source, dest);
}
extern int _stdcall RunCookCollision(const char* source, const char* dest)
{
	return CookMesh(source, dest);
}