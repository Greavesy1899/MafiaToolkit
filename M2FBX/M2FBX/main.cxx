#include <fbxsdk.h>
#include "FbxWrangler.h"
#include "M2TWrangler.h"
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
int main(int argc, char** argv)
{
	//check argument count.
	if (argc > 4)
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
