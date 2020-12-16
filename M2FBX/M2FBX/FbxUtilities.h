#ifndef FBX_UTILS_HEADER
#define FBX_UTILS_HEADER
#include "Common.h"

#ifdef IOS_REF
#undef  IOS_REF
#define IOS_REF (*(pManager->GetIOSettings()))
#endif

void InitializeSdkObjects(FbxManager*& pManager);
void DestroySdkObjects(FbxManager* pManager, bool pExitStatus);

FbxVector4 ConvertVector3(const Point3& Vector3);

#endif