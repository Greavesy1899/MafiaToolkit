#include <fbxsdk.h>
#include "FbxWrangler.h"
#include "Utilities.h"
#include "M2Model.h"

int main(int argc, char** argv)
{
	if (FBXSDK_stricmp(argv[1], "-ConvertToFBX") == 0)
		Convert(argv[2], argv[3]);
	else if (FBXSDK_stricmp(argv[1], "-ConvertToM2T") == 0)
		FBXSDK_printf("Not Implemented.\n");

    return 0;
}
