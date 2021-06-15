#include <fbxsdk.h>
#include "FbxWrangler.h"
#include "M2TWrangler.h"
//#include "CPhysXCooking.h"
#include "M2Model.h"
#include <conio.h>

extern "C" int  __declspec(dllexport) _stdcall RunConvertFBX(const char* source, const char* dest);
extern "C" int  __declspec(dllexport) _stdcall RunConvertM2T(const char* source, const char* dest, unsigned char isBin);
extern "C" int  __declspec(dllexport) _stdcall RunConvertType(const char* source, const char* dest);
//extern "C" int  __declspec(dllexport) _stdcall RunCookTriangleCollision(const char* source, const char* dest);
//extern "C" int  __declspec(dllexport) _stdcall RunCookConvexCollision(const char* source, const char* dest);

extern int _stdcall RunConvertFBX(const char* source, const char* dest)
{
	WriteLine("Called RunConvertFBX");
	return ConvertFBX(source, dest);
}
extern int _stdcall RunConvertM2T(const char* source, const char* dest, unsigned char isBin)
{
	WriteLine("Called RunConvertM2T");
	return ConvertM2T(source, dest, isBin);
}
int _stdcall RunConvertType(const char* source, const char* dest)
{
	WriteLine("Called RunConvertType");
	return ConvertType(source, dest);
}
/*extern int _stdcall RunCookTriangleCollision(const char* source, const char* dest)
{
	WriteLine("Called RunCookTriangleCollision");
	return CookTriangle(source, dest);
}
extern int _stdcall RunCookConvexCollision(const char* source, const char* dest)
{
	WriteLine("Called RunCookConvexCollision");
	return CookConvex(source, dest);
}*/

int main(int argc, char** argv)
{
	int result = 0;

	if ((strcmp(argv[1], "-ConvertM2T") == 0) && (argc >=4))
	{
		result = ConvertM2T(argv[2], argv[3], 0);
	}
	else if ((strcmp(argv[1], "-ConvertFBX") == 0) && (argc >= 4))
	{
		result = ConvertFBX(argv[2], argv[3]);
	}
	else if ((strcmp(argv[1], "-ConvertType") == 0) && (argc >= 4))
	{
		result = ConvertType(argv[2], argv[3]);
	}
	else if ((strcmp(argv[1], "-CookTriangle") == 0) && (argc >= 4))
	{
		//result = CookTriangle(argv[2], argv[3]);
	}
	else if ((strcmp(argv[1], "-CookConvex") == 0) && (argc >= 4))
	{
		//result = CookConvex(argv[2], argv[3]);
	}
	else
	{
		printf("M2FBX Initiated succesfully.");
	}
	return result;
}