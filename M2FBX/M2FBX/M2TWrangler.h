#ifndef M2T_WRANGLER_HEADER
#define M2T_WRANGLER_HEADER

#include "M2Model.h"
#include "Common.h"

int ConvertFBX(const char* pSource, const char* pDest);
void BuildUVsFromMesh(FbxMesh* pMesh, Vertex* vertices, int triIndex, ModelPart& pPart);

#endif