#include <fbxsdk.h>
#include "FbxWrangler.h"
#include "M2TWrangler.h"
#include "Utilities.h"
#include "M2Model.h"
#include <conio.h>

void PrintHelp()
{
	FBXSDK_printf("Possible Commands:\n");
	FBXSDK_printf("\n");
	FBXSDK_printf("To convert M2T to FBX:\n");
	FBXSDK_printf("M2FBX.exe -ConvertToFBX \"model_source\" \"model_destination\"\n");
	FBXSDK_printf("\n");
	FBXSDK_printf("To convert FBX to M2T:\n");
	FBXSDK_printf("M2FBX.exe -ConvertToM2T \"model_source\" \"model_destination\"\n");
}

void PrintError(int code)
{
	switch (code)
	{
	case 0:
		FBXSDK_printf("ERROR: This program requires 4 arguments to run.\n");
		break;
	case 1:
		FBXSDK_printf("ERROR: No command inputted.. Press any key to continue.\n");
		break;
	default:
		FBXSDK_printf("ERROR: Unknown Error Code.\n");
	}
	PrintHelp();
	_getch();
}
int main(int argc, char** argv)
{
	//check argument count.
	if (argc != 4)
	{
		PrintError(0);
		return 0;
	}

	//check arguments.
	if (FBXSDK_stricmp(argv[1], "-ConvertToFBX") == 0)
		ConvertM2T(argv[2], argv[3]);
	else if (FBXSDK_stricmp(argv[1], "-ConvertToM2T") == 0)
		ConvertFBX(argv[2], argv[3]);
	else
	{
		PrintError(1);
		return 0;
	}

    return 0;
}
