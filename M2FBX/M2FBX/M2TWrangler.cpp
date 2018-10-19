#include "M2TWrangler.h"
#include "FbxUtilities.h"
#include <conio.h>

void BuildModelPart(FbxNode* pNode, ModelPart* pPart)
{
	FbxMesh* pMesh = (FbxMesh*)pNode->GetNodeAttribute();
	FbxGeometryElementNormal* pElementNormal = pMesh->GetElementNormal(0);
	FbxGeometryElementTangent* pElementTangent = pMesh->GetElementTangent(0);
	FbxGeometryElementUV* pElementUV = pMesh->GetElementUV(0);

	pPart->SetHasPositions(true);
	pPart->SetHasNormals(pElementNormal);
	pPart->SetHasTangents(pElementTangent);
	pPart->SetHasUV0(pElementUV);

	//Gotta make sure the normals are correctly set up.
	if (pElementNormal->GetReferenceMode() != FbxGeometryElement::eDirect) {
		FBXSDK_printf("pElementNormal->GetReferenceMode() did not equal eDirect.. Cannot continue.\n");
		_getch();
		exit(-1);
	}
	if (pElementNormal->GetMappingMode() != FbxGeometryElement::eByControlPoint) {
		FBXSDK_printf("pElementNormal->GetMappingMode() did not equal eByControlPoint.. Cannot continue.\n");
		_getch();
		exit(-1);
	}

	std::vector<Point3> vertices = std::vector<Point3>();
	std::vector<Point3> normals = std::vector<Point3>();
	std::vector<Point3> tangents = std::vector<Point3>();
	std::vector<UVVert> uvs = std::vector<UVVert>();

	for (int i = 0; i != pMesh->GetControlPointsCount(); i++) {
		Point3 vert;
		UVVert uvCoords;
		FbxVector4 vec4 = pMesh->GetControlPointAt(i);

		//do vert stuff.
		vert.x = vec4.mData[0];
		vert.y = vec4.mData[1];
		vert.z = vec4.mData[2];
		vertices.push_back(vert);

		//do normal stuff.
		if (pPart->GetHasNormals()) {
			vec4 = pElementNormal->GetDirectArray().GetAt(i);
			vert.x = vec4.mData[0];
			vert.y = vec4.mData[1];
			vert.z = vec4.mData[2];
			normals.push_back(vert);
		}

		//do tangent stuff.
		if (pPart->GetHasTangents()) {
			vec4 = pElementTangent->GetDirectArray().GetAt(i);
			vert.x = vec4.mData[0];
			vert.y = vec4.mData[1];
			vert.z = vec4.mData[2];
			tangents.push_back(vert);
		}

		//do UV stuff.
		if (pPart->GetHasUV0()) {
			vec4 = pElementUV->GetDirectArray().GetAt(i);
			uvCoords.x = vec4.mData[0];
			uvCoords.y = vec4.mData[1];
			uvs.push_back(uvCoords);
		}
	}

	//update the part with the latest data.
	pPart->SetVertices(vertices, true);
	pPart->SetNormals(normals);
	pPart->SetTangents(tangents);
	pPart->SetUVs(uvs);

	//Gotta be triangulated.
	if (!pMesh->IsTriangleMesh()) {
		FBXSDK_printf("pMesh->IsTriangleMesh() did not equal true.. Cannot continue.\n");
		_getch();
		exit(-1);
	}

	//begin getting triangles
	FbxGeometryElementMaterial* pElementMaterial = pMesh->GetElementMaterial(0);
	std::vector<Int3> indices = std::vector<Int3>();
	std::vector<char> matIDs = std::vector<char>();

	for (int i = 0; i != pMesh->GetPolygonCount(); i++)
	{
		Int3 triangle;
		char matID;
		triangle.i1 = pMesh->GetPolygonVertex(i, 0);
		triangle.i2 = pMesh->GetPolygonVertex(i, 1);
		triangle.i3 = pMesh->GetPolygonVertex(i, 2);
		matID = pElementMaterial->GetIndexArray().GetAt(i);
		indices.push_back(triangle);
		matIDs.push_back(matID);
	}

	//Update data to do with triangles.
	pPart->SetIndices(indices, true);
	pPart->SetMatIDs(matIDs);

	std::vector<std::string> names = std::vector<std::string>();

	//Get Material Names.
	for (int i = 0; i != pNode->GetMaterialCount(); i++)
	{
		FbxSurfaceMaterial* mat = pNode->GetMaterial(i);
		names.push_back(mat->GetName());
	}

	pPart->SetMatNames(names, true);
}


int DetermineNodeAttribute(FbxNode* node)
{
	FbxNodeAttribute::EType lAttributeType;
	int i;

	if (node->GetNodeAttribute() == NULL)
		FBXSDK_printf("NULL Node Attribute\n\n");
	else
		return node->GetNodeAttribute()->GetAttributeType();
}

int ConvertFBX(const char* pSource, const char* pDest)
{
	FBXSDK_printf("Converting FBX to M2T.\n");

	FbxManager* lSdkManager = NULL;
	FbxImporter* lImporter = NULL;
	FbxScene* lScene = NULL;

	//Prepare SDK..
	InitializeSdkObjects(lSdkManager);
	lImporter = FbxImporter::Create(lSdkManager, "");

	//Init importer. if it fails, it will print error code.
	if (!lImporter->Initialize(pSource, -1, lSdkManager->GetIOSettings())) {
		FBXSDK_printf("Error occured while initializing importer:\n");
		FBXSDK_printf("%s\n", lImporter->GetStatus().GetErrorString());
		exit(-1);
	}

	FBXSDK_printf("Importing %s...\n", pSource);

	//Populate scene and destroy importer.
	lScene = FbxScene::Create(lSdkManager, "scene");
	lImporter->Import(lScene);
	lImporter->Destroy();

	//dump info for user to see.
	FBXSDK_printf("Geometry Count: %i\n", lScene->GetGeometryCount());
	FBXSDK_printf("Material Count: %i\n", lScene->GetMaterialCount());
	FBXSDK_printf("Node Count: %i\n", lScene->GetNodeCount());
	 
	//Create Model..
	ModelStructure Structure = ModelStructure();
	Structure.SetPartSize(1);
	Structure.SetName("Model");
	std::vector<ModelPart> parts = std::vector<ModelPart>();

	//Get Geometry..
	FbxNode* Root = lScene->GetRootNode();

	if (Root) {
		for (int i = 0; i != Root->GetChildCount(); i++) {
			FbxNode* pNode = Root->GetChild(i);
			if (DetermineNodeAttribute(pNode) == FbxNodeAttribute::eMesh) {
				FBXSDK_printf("Converting Mesh..\n");
				ModelPart Part = ModelPart();
				ModelPart* pPart = &Part;
				BuildModelPart(pNode, pPart);
				parts.push_back(Part);
				FBXSDK_printf("Built Part..\n");
			}
		}
	}

	Structure.SetParts(parts, true);

	FILE* stream;
	fopen_s(&stream, pDest, "wb");
	Structure.WriteToStream(stream);
	fclose(stream);
}