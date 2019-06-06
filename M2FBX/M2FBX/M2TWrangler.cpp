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
			WriteLine("Boundary Box exceeds Mafia II's limits! Cannot continue!\n");
			exit(-100);
		}

		val = pMesh->BBoxMax.Get()[i];

		if (val < -32768 || val > 32768) {
			FBXSDK_printf("Boundary Box exceeds Mafia II's limits! Cannot continue!\n");
			exit(-100);
		}
	}

	if (pMesh->GetControlPoints()->Length() > 65535)
	{
		FBXSDK_printf("Vertex count > ushort max value! This model cannot be used in Mafia II\n");
		exit(-97);
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
			WriteLine("pElementNormal->GetReferenceMode() did not equal eDirect.. Cannot continue.");
			exit(-99);
		}
		if ((pElementNormal->GetMappingMode() <= FbxGeometryElement::eByControlPoint) && (pElementNormal->GetMappingMode() >= FbxGeometryElement::eByPolygonVertex)) {
			WriteLine("pElementNormal->GetMappingMode() did not equal eByControlPoint or eByPolygonVertex.. Cannot continue.");
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
		WriteLine("pMesh->IsTriangleMesh() did not equal true.. Cannot continue.");
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
		WriteLine("Missing material nodes on this FBX Model!");
	}

	int subIDX = 0;
	std::vector<std::vector<Int3>> segments = std::vector<std::vector<Int3>>();
	int* subNumFacesCount = new int[pNode->GetMaterialCount()];
	for (size_t i = 0; i != pNode->GetMaterialCount(); i++)
	{
		subNumFacesCount[i] = 0;
		segments.push_back(std::vector<Int3>());
	}

	for (int i = 0; i != pMesh->GetPolygonCount(); i++)
	{
		Int3 triangle;
		triangle.i1 = pMesh->GetPolygonVertex(i, 0);
		triangle.i2 = pMesh->GetPolygonVertex(i, 1);
		triangle.i3 = pMesh->GetPolygonVertex(i, 2);

		if (pElementMaterial != NULL)
		{
			auto matID = pElementMaterial->GetIndexArray().GetAt(i);
			segments[matID].push_back(triangle);
		}
		matIDs.push_back(0);
	}

	for (size_t i = 0; i != pNode->GetMaterialCount(); i++)
	{
		subNumFacesCount[i] = segments[i].size();			
		indices.insert(indices.end(), segments[i].begin(), segments[i].end());
	}


	int total = pMesh->GetPolygonCount();
	int calcTotal = 0;

	for (size_t i = 0; i != pNode->GetMaterialCount(); i++)
		calcTotal += subNumFacesCount[i];

	if (calcTotal != total)
		WriteLine("Potential error when splitting faces!");

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
	pPart.SetSubMeshes(subMeshes);
	pPart.SetSubMeshCount(pNode->GetMaterialCount());
	delete[] subNumFacesCount;
}


int DetermineNodeAttribute(FbxNode* node)
{
	if (node->GetNodeAttribute() == NULL)
		WriteLine("NULL Node Attribute\n");
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
	WriteLine("Converting Mesh..");
	ModelPart Part = ModelPart();
	BuildModelPart(node, Part);
	parts[0] = Part;
	structure->SetParts(parts);
	WriteLine("Built Model..");
}

int ConvertFBX(const char* pSource, const char* pDest)
{
	WriteLine("Converting FBX to M2T.");

	FbxManager* lSdkManager = NULL;
	FbxImporter* lImporter = NULL;
	FbxScene* lScene = NULL;

	//Prepare SDK..
	InitializeSdkObjects(lSdkManager);
	lImporter = FbxImporter::Create(lSdkManager, "");
	WriteLine("Loading FBX File..");
	//Init importer. if it fails, it will print error code.
	if (!lImporter->Initialize(pSource, -1, lSdkManager->GetIOSettings())) {
		WriteLine("Error occured while initializing importer:");
		WriteLine("%s", lImporter->GetStatus().GetErrorString());
		exit(-1);
	}

	WriteLine("Importing %s...", pSource);

	//Populate scene and destroy importer.
	lScene = FbxScene::Create(lSdkManager, "scene");
	lImporter->Import(lScene);
	lImporter->Destroy();

	//dump info for user to see.
	WriteLine("Geometry Count: %i", lScene->GetGeometryCount());
	WriteLine("Material Count: %i", lScene->GetMaterialCount());
	WriteLine("Node Count: %i", lScene->GetNodeCount());

	//Get Geometry..
	FbxNode* Root = lScene->GetRootNode();

	for (int i = 0; i != Root->GetChildCount(); i++) {
		FbxNode* pNode = Root->GetChild(i);
		if (DetermineNodeAttribute(pNode) == FbxNodeAttribute::eMesh) {
			ModelStructure* Structure = new ModelStructure();
			BuildModel(Structure, pNode);

			//Point3 pos = Point3();
			//pos.x = pNode->LclTranslation.Get().mData[0];
			//pos.z = pNode->LclTranslation.Get().mData[1];
			//pos.y = -pNode->LclTranslation.Get().mData[2];
			//entry.SetPosition(pos);
			//WriteLine("Position is: %f, %f, %f", pos.x, pos.y, pos.z);

			FILE* stream;
			std::string dest = pDest;
			if(!dest.find(".m2t"))
				dest += ".m2t";
			fopen_s(&stream, dest.c_str(), "wb");
			Structure->WriteToStream(stream);
			if (stream != NULL) fclose(stream);

			WriteLine("Exported %s", Structure->GetName().c_str());
		}
	}
	return 0;
}