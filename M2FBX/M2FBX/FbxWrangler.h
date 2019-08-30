#ifndef FBX_WRANGER_HEADER
#define FBX_WRANGER_HEADER
#include "Common.h"
#include "M2Model.h"

int ConvertM2T(const char* pSource, const char* pDest, unsigned char isBin);
bool CreateDocument(FbxManager* pManager, FbxScene* pScene, ModelStructure model);
FbxNode* CreatePlane(FbxManager* pManager, const char* pName, ModelPart model);
FbxSurfacePhong* CreateMaterial(FbxManager* pManager, const char* pName);
FbxTexture*  CreateTexture(FbxManager* pManager, const char* pName);
#endif