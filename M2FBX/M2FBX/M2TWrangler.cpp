#include "M2TWrangler.h"
#include "FbxUtilities.h"
#include "M2Model.h"
#include <conio.h>

void BuildModelPart(FbxNode* pNode, ModelPart &pPart)
{
	FbxMesh* pMesh = (FbxMesh*)pNode->GetNodeAttribute();
	FbxGeometryElementNormal* pElementNormal = pMesh->GetElementNormal(0);
	FbxGeometryElementTangent* pElementTangent = pMesh->GetElementTangent(0);
	FbxGeometryElementUV* pElementUV = pMesh->GetElementUV(0);
	FbxGeometryElementUV* pElementOM = pMesh->GetElementUV("OMUV");
	FbxGeometryElementMaterial* pElementMaterial = pMesh->GetElementMaterial(0);
	FbxGeometryElementVertexColor* pElementVC = pMesh->GetElementVertexColor(0);

	pMesh->ComputeBBox();

	for (int i = 0; i != 3; i++)
	{
		double val = pMesh->BBoxMin.Get()[i];

		if (val < -32768 || val > 32768) {
			FBXSDK_printf("Boundary Box exceeds Mafia II's limits! Cannot continue!\n");
			exit(-100);
		}

		val = pMesh->BBoxMax.Get()[i];

		if (val < -32768 || val > 32768) {
			FBXSDK_printf("Boundary Box exceeds Mafia II's limits! Cannot continue!\n");
			exit(-100);
		}
	}
	

	pPart.SetHasPositions(true);
	pPart.SetHasNormals(pElementNormal);
	pPart.SetHasTangents(pElementTangent);
	pPart.SetHasUV0(pElementUV && pElementMaterial);
	pPart.SetHasUV1(pElementVC);
	pPart.SetHasUV2(pElementVC);
	pPart.SetHasUV7(pElementOM);

	if (pPart.GetHasNormals())
	{
		//Gotta make sure the normals are correctly set up.
		if (pElementNormal->GetReferenceMode() != FbxGeometryElement::eDirect) {
			FBXSDK_printf("pElementNormal->GetReferenceMode() did not equal eDirect.. Cannot continue.\n");
			exit(-99);
		}
		if ((pElementNormal->GetMappingMode() <= FbxGeometryElement::eByControlPoint) && (pElementNormal->GetMappingMode() >= FbxGeometryElement::eByPolygonVertex)) {
			FBXSDK_printf("pElementNormal->GetMappingMode() did not equal eByControlPoint or eByPolygonVertex.. Cannot continue.\n");
			exit(-98);
		}
	}

	std::vector<Point3> vertices = std::vector<Point3>();
	std::vector<Point3> normals = std::vector<Point3>();
	std::vector<Point3> tangents = std::vector<Point3>();
	std::vector<UVVert> uvs = std::vector<UVVert>();
	std::vector<UVVert> uvs1 = std::vector<UVVert>();
	std::vector<UVVert> uvs2 = std::vector<UVVert>();
	std::vector<UVVert> uvs7 = std::vector<UVVert>();

	for (int i = 0; i != pMesh->GetControlPointsCount(); i++) {
		Point3 vert;
		UVVert uvCoords;
		FbxVector4 vec4 = pMesh->GetControlPointAt(i);
		FbxColor color;

		//do vert stuff.
		vert.x = vec4.mData[0];
		vert.y = vec4.mData[1];
		vert.z = vec4.mData[2];
		vertices.push_back(vert);

		//do normal stuff.
		if (pPart.GetHasNormals()) {
			vec4 = pElementNormal->GetDirectArray().GetAt(i);
			vert.x = vec4.mData[0];
			vert.y = vec4.mData[1];
			vert.z = vec4.mData[2];
			normals.push_back(vert);
		}

		//do tangent stuff.
		if (pPart.GetHasTangents()) {
			vec4 = pElementTangent->GetDirectArray().GetAt(i);
			vert.x = vec4.mData[0];
			vert.y = vec4.mData[1];
			vert.z = vec4.mData[2];
			tangents.push_back(vert);
		}

		//do UV stuff.
		if (pPart.GetHasUV0()) {
			vec4 = pElementUV->GetDirectArray().GetAt(i);
			uvCoords.x = vec4.mData[0];
			uvCoords.y = vec4.mData[1];
			uvs.push_back(uvCoords);
		}

		//Colours
		if (pPart.GetHasUV1()) {
			color = pElementVC->GetDirectArray().GetAt(i);
			uvCoords.x = color.mRed;
			uvCoords.y = color.mBlue;
			uvs1.push_back(uvCoords);
		}
		if (pPart.GetHasUV2()) {
			color = pElementVC->GetDirectArray().GetAt(i);
			uvCoords.x = color.mBlue;
			uvCoords.y = color.mAlpha;
			uvs2.push_back(uvCoords);
		}
		if (pPart.GetHasUV7()) {
			vec4 = pElementOM->GetDirectArray().GetAt(i);
			uvCoords.x = vec4.mData[0];
			uvCoords.y = vec4.mData[1];
			uvs7.push_back(uvCoords);
		}
	}

	//update the part with the latest data.
	pPart.SetVertices(vertices, true);
	pPart.SetNormals(normals);
	pPart.SetTangents(tangents);
	pPart.SetUV0s(uvs);
	pPart.SetUV1s(uvs1);
	pPart.SetUV2s(uvs2);
	pPart.SetUV7s(uvs7);

	//Gotta be triangulated.
	if (!pMesh->IsTriangleMesh()) {
		FBXSDK_printf("pMesh->IsTriangleMesh() did not equal true.. Cannot continue.\n");
		exit(-97);
	}

	//begin getting triangles
	std::vector<Int3> indices = std::vector<Int3>();
	std::vector<short> matIDs = std::vector<short>();

	SubMesh* subMeshes = new SubMesh[pNode->GetMaterialCount()];

	if (pNode->GetMaterialCount() != 0)
	{
		for (int i = 0; i != pNode->GetMaterialCount(); i++)
		{
			SubMesh sub = SubMesh();
			FbxSurfaceMaterial* mat = pNode->GetMaterial(i);
			sub.SetMatName(std::string(mat->GetName()));
			subMeshes[i] = sub;
		}
	}
	else
	{
		FBXSDK_printf("Missing material nodes on this FBX Model!\n");
	}

	int subIDX = 0;
	int* subNumFacesCount = new int[pNode->GetMaterialCount()];
	for (size_t i = 0; i != pNode->GetMaterialCount(); i++)
		subNumFacesCount[i] = 0;

	for (int i = 0; i != pMesh->GetPolygonCount(); i++)
	{
		Int3 triangle;
		short matID = 0;
		triangle.i1 = pMesh->GetPolygonVertex(i, 0);
		triangle.i2 = pMesh->GetPolygonVertex(i, 1);
		triangle.i3 = pMesh->GetPolygonVertex(i, 2);

		if (pElementMaterial != NULL)
		{
			if (subIDX == pElementMaterial->GetIndexArray().GetAt(i))
			{
				subNumFacesCount[subIDX]++;
			}
			else
			{
				subIDX = pElementMaterial->GetIndexArray().GetAt(i);
				subNumFacesCount[subIDX]++;
			}
		}
			//matID = pElementMaterial->GetIndexArray().GetAt(i);

		indices.push_back(triangle);
		matIDs.push_back(matID);
	}

	int total = pMesh->GetPolygonCount();
	int calcTotal = 0;

	for (size_t i = 0; i != pNode->GetMaterialCount(); i++)
		calcTotal += subNumFacesCount[i];

	if (calcTotal != total)
		FBXSDK_printf("Potential error when splitting faces!\n");

	int curTotal = 0;
	for (size_t i = 0; i != pNode->GetMaterialCount(); i++)
	{
		int faces = subNumFacesCount[i];
		subMeshes[i].SetNumFaces(faces);
		subMeshes[i].SetStartIndex(curTotal);
		curTotal += faces*3;
	}
	//Update data to do with triangles.
	pPart.SetIndices(indices, true);
	pPart.SetMatIDs(matIDs);
	pPart.SetSubMeshes(subMeshes);
	pPart.SetSubMeshCount(pNode->GetMaterialCount());
	delete[] subNumFacesCount;
}


int DetermineNodeAttribute(FbxNode* node)
{
	if (node->GetNodeAttribute() == NULL)
		FBXSDK_printf("NULL Node Attribute\n\n");
	else
		return node->GetNodeAttribute()->GetAttributeType();

	return NULL;
}

void BuildModel(ModelStructure* structure, FbxNode* node)
{
	char size = 1;
	structure->SetPartSize(size);
	ModelPart* parts = new ModelPart[1];
	structure->SetName(std::string(node->GetName()));
	FBXSDK_printf("Converting Mesh..\n");
	ModelPart Part = ModelPart();
	BuildModelPart(node, Part);
	parts[0] = Part;
	structure->SetParts(parts);
	FBXSDK_printf("Built Model..\n");
}

int ConvertFBX(const char* pSource, const char* pDest, const char* doScene)
{
	FBXSDK_printf("Converting FBX to M2T.\n");

	if (strcmp(doScene, "1") == 0)
		FBXSDK_printf("Converting to a scene!\n");
	else
		FBXSDK_printf("Converting a single model!\n");

	FbxManager* lSdkManager = NULL;
	FbxImporter* lImporter = NULL;
	FbxScene* lScene = NULL;

	//Prepare SDK..
	InitializeSdkObjects(lSdkManager);
	lImporter = FbxImporter::Create(lSdkManager, "");
	printf("Loading FBX File.. \n");
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

	//Get Geometry..
	FbxNode* Root = lScene->GetRootNode();

	if (Root) {
		FrameClass frame = FrameClass();
		std::vector<FrameEntry> entries = std::vector<FrameEntry>();
		
		for (int i = 0; i != Root->GetChildCount(); i++) {
			FbxNode* pNode = Root->GetChild(i);
			FrameEntry entry = FrameEntry();
			if (DetermineNodeAttribute(pNode) == FbxNodeAttribute::eMesh) {
				ModelStructure* Structure = new ModelStructure();
				BuildModel(Structure, pNode);

				std::vector<std::string> names = std::vector<std::string>();
				Point3 pos = Point3();
				pos.x = pNode->LclTranslation.Get().mData[0];
				pos.z = pNode->LclTranslation.Get().mData[1];
				pos.y = -pNode->LclTranslation.Get().mData[2];
				entry.SetPosition(pos);
				FBXSDK_printf("Position is: %f, %f, %f\n", pos.x, pos.y, pos.z);

				names.push_back(Structure->GetName());

				FILE* stream;
				std::string dest = pDest;
				if (strcmp(doScene, "1") == 0) {
					dest += Structure->GetName();
					dest += ".m2t";
				}
				fopen_s(&stream, dest.c_str(), "wb");
				Structure->WriteToStream(stream);
				fclose(stream);

				entry.SetLodNames(names);
				entries.push_back(entry);
				FBXSDK_printf("Exported %s\n", Structure->GetName().c_str());
			}
		}
		frame.SetEntries(entries);
		FILE* frameStream;
		std::string frameDest = pDest;
		frameDest += "frame.edd";

		if (strcmp(doScene, "1") == 0) {
			fopen_s(&frameStream, frameDest.c_str(), "wb");
			frame.WriteToStream(frameStream);
			fclose(frameStream);
			FBXSDK_printf("Saved frame..");
		}
	}

	return 0;
}