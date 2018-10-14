#include <fbxsdk.h>
#include <iomanip>
#include <iostream>
#include "M2Model.h"

#ifdef IOS_REF
#undef  IOS_REF
#define IOS_REF (*(pManager->GetIOSettings()))
#endif

int Convert(const char* pSource, const char* pDest);
bool CreateDocument(FbxManager* pManager, FbxScene* pScene, ModelStructure model);
void CreateLightDocument(FbxManager* pManager, FbxDocument* pLightDocument);
FbxNode* CreatePlane(FbxManager* pManager, const char* pName, ModelStructure model);
FbxSurfacePhong* CreateMaterial(FbxManager* pManager, const char* pName);
FbxTexture*  CreateTexture(FbxManager* pManager, const char* pName);
FbxNode* CreateLight(FbxManager* pManager, FbxLight::EType pType);