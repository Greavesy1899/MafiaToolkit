#ifndef FBX_WRANGER_HEADER
#define FBX_WRANGER_HEADER
#include "Common.h"
#include "M2Model.h"

int ConvertType(const char* pSource, const char* pDest);
int ConvertM2T(const char* pSource, const char* pDest, unsigned char isBin);
bool CreateDocument(FbxManager* pManager, FbxScene* pScene, ModelStructure model);
void CreateLightDocument(FbxManager* pManager, FbxDocument* pLightDocument);
void SetupGlobalSettings(FbxScene* pScene);
FbxNode* CreateBoneNode(FbxManager* pManger, const char* pName);
FbxNode* CreatePlane(FbxManager* pManager, const char* pName, ModelPart model);
FbxSurfacePhong* CreateMaterial(FbxManager* pManager, const char* pName);
FbxTexture*  CreateTexture(FbxManager* pManager, const char* pName);
FbxGeometryElementVertexColor* CreateVertexColor(FbxMesh* pMesh, const char* pName, ModelPart& pModel);
FbxGeometryElementUV* CreateUVElement(FbxMesh* pMesh, const char* pName, ModelPart& pModel);
FbxGeometryElementMaterial* CreateMaterialElement(FbxMesh* pMesh, const char* pName, ModelPart& pModel);
#endif