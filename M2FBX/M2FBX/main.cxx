#include <fbxsdk.h>
#include "FbxWrangler.h"
#include "M2TWrangler.h"
#include "CPhysXCooking.h"
#include "M2Model.h"
#include <conio.h>

extern "C" int  __declspec(dllexport) _stdcall RunConvertFBX(const char* source, const char* dest);
extern "C" int  __declspec(dllexport) _stdcall RunConvertM2T(const char* source, const char* dest);
extern "C" int  __declspec(dllexport) _stdcall RunCookTriangleCollision(const char* source, const char* dest);
extern "C" int  __declspec(dllexport) _stdcall RunCookConvexCollision(const char* source, const char* dest);

extern int _stdcall RunConvertFBX(const char* source, const char* dest)
{
	return ConvertFBX(source, dest);
}
extern int _stdcall RunConvertM2T(const char* source, const char* dest)
{
	return ConvertM2T(source, dest);
}
extern int _stdcall RunCookTriangleCollision(const char* source, const char* dest)
{
	return CookTriangle(source, dest);
}
extern int _stdcall RunCookConvexCollision(const char* source, const char* dest)
{
	return CookConvex(source, dest);
}

int main(int argc, char** argv)
{
	printf("Working?");
	_getch();
	return 0;
}