#ifndef FBX_UTILS_HEADER
#define FBX_UTILS_HEADER
#include <fbxsdk.h>
#include <iomanip>

#ifdef IOS_REF
#undef  IOS_REF
#define IOS_REF (*(pManager->GetIOSettings()))
#endif

void InitializeSdkObjects(FbxManager*& pManager);
void DestroySdkObjects(FbxManager* pManager, bool pExitStatus);
#endif