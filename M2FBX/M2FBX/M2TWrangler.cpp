#include "M2TWrangler.h"
#include "FbxUtilities.h"
#include <conio.h>

int BuildModelPart(FbxNode* pNode, ModelPart& pPart)
{
	FbxMesh* pMesh = (FbxMesh*)pNode->GetNodeAttribute();
	FbxGeometryElementNormal* pElementNormal = pMesh->GetElementNormal(0);
	FbxGeometryElementTangent* pElementTangent = pMesh->GetElementTangent(0);
	FbxGeometryElementUV* pElementDiffuseUV = pMesh->GetElementUV("DiffuseUV");
	FbxGeometryElementUV* pElementOneUV = pMesh->GetElementUV("UV1");
	FbxGeometryElementUV* pElementTwoUV = pMesh->GetElementUV("UV2");
	FbxGeometryElementUV* pElementOMUV = pMesh->GetElementUV("OMUV");
	FbxGeometryElementMaterial* pElementMaterial = pMesh->GetElementMaterial(0);
	FbxGeometryElementVertexColor* pElementColor0 = pMesh->GetElementVertexColor(0);
	FbxGeometryElementVertexColor* pElementColor1 = pMesh->GetElementVertexColor(1);

	// Ideally this should have to be done. But it does.
	// TODO:: Figure out why three vertex colour elements are added to the mesh.
	// TODO CONT:: This could be because we don't add to the layer but directly to the mesh?
	for (int i = 0; i < pMesh->GetElementVertexColorCount(); i++)
	{
		FbxGeometryElementVertexColor* VertexColor = pMesh->GetElementVertexColor(i);
		auto MappingMode = VertexColor->GetMappingMode();
		auto ReferenceMode = VertexColor->GetReferenceMode();

		if (MappingMode == FbxLayerElement::eByPolygonVertex && ReferenceMode == FbxLayerElement::eIndexToDirect)
		{
			pElementColor0 = VertexColor;
		}
		WriteLine("Vertex Colour Channel: %s", VertexColor->GetName());
	}

	pMesh->ComputeBBox();

	for (int i = 0; i != 3; i++)
	{
		double val = pMesh->BBoxMin.Get()[i];

		if (val < -32768 || val > 32768) {
			WriteLine("Boundary Box exceeds Mafia II's limits! Cannot continue!\n");
			return -100;
		}

		val = pMesh->BBoxMax.Get()[i];

		if (val < -32768 || val > 32768) {
			FBXSDK_printf("Boundary Box exceeds Mafia II's limits! Cannot continue!\n");
			return -100;
		}
	}

	//auto test = pMesh->GetPolygonCount();
	//if(pMesh->GetPolygonCount() * 3 > )
	//if (pMesh->GetControlPoints()->Length() > 65535)
	//{
	//	FBXSDK_printf("Vertex count > ushort max value! This model cannot be used in Mafia II\n");
	//	return -97;
	//}

	pPart.SetVertexFlag(VertexFlags::Position);
	pPart.SetVertexFlag(pElementNormal ? VertexFlags::Normals : VertexFlags::None);
	pPart.SetVertexFlag(pElementTangent ? VertexFlags::Tangent : VertexFlags::None);
	pPart.SetVertexFlag((pElementDiffuseUV && pElementMaterial) ? VertexFlags::TexCoords0 : VertexFlags::None);
	pPart.SetVertexFlag(pElementOneUV ? VertexFlags::TexCoords1 : VertexFlags::None);
	pPart.SetVertexFlag(pElementTwoUV ? VertexFlags::TexCoords2 : VertexFlags::None);
	pPart.SetVertexFlag(pElementOMUV ? VertexFlags::ShadowTexture : VertexFlags::None);
	pPart.SetVertexFlag(pElementColor0 ? VertexFlags::Color : VertexFlags::None);
	//pPart.SetVertexFlag(pElementColor1 ? VertexFlags::Color1 : VertexFlags::None);

	if (pPart.HasVertexFlag(VertexFlags::Normals))
	{
		//Gotta make sure the normals are correctly set up.
		if (pElementNormal->GetReferenceMode() != FbxGeometryElement::eDirect) {
			WriteLine("pElementNormal->GetReferenceMode() did not equal eDirect.. Cannot continue.");
			return -99;
		}
		if ((pElementNormal->GetMappingMode() != FbxGeometryElement::eByControlPoint)) {
			pMesh->SetBoundaryRule(FbxMesh::EBoundaryRule::eCreaseEdge);
			pMesh->SetMeshSmoothness(FbxMesh::ESmoothness::eFine);
			pMesh->BuildMeshEdgeArray();
			pMesh->GenerateNormals(true, true, false);
		}
	}

	if (pPart.HasVertexFlag(VertexFlags::Tangent))
	{
		if (pElementTangent->GetReferenceMode() != FbxGeometryElement::eDirect) {
			WriteLine("pElementTangent->GetReferenceMode() did not equal eDirect.. Cannot continue.");
			return -99;
		}
		if ((pElementTangent->GetMappingMode() != FbxGeometryElement::eByControlPoint)) {
			pMesh->GenerateTangentsData("DiffuseUV", true);
		}
	}

	uint numVertices = pMesh->GetControlPointsCount();
	Vertex* vertices = new Vertex[numVertices];

	for (int i = 0; i != numVertices; i++) {
		Vertex vertice;
		Point3 vert;
		UVVert uvCoords;
		FbxVector4 vec4 = pMesh->GetControlPointAt(i);
		FbxColor color;

		//do vert stuff.
		if (pPart.HasVertexFlag(VertexFlags::Position)) {
			vert.x = vec4.mData[0];
			vert.y = vec4.mData[1];
			vert.z = vec4.mData[2];
			vertice.position = vert;
		}

		//do normal stuff.
		if (pPart.HasVertexFlag(VertexFlags::Normals)) {
			vec4 = pElementNormal->GetDirectArray().GetAt(i);
			vert.x = vec4.mData[0];
			vert.y = vec4.mData[1];
			vert.z = vec4.mData[2];
			vertice.normals = vert;
		}

		//do tangent stuff.
		if (pPart.HasVertexFlag(VertexFlags::Tangent)) {
			vec4 = pElementTangent->GetDirectArray().GetAt(i);
			vert.x = vec4.mData[0];
			vert.y = vec4.mData[1];
			vert.z = vec4.mData[2];
			vertice.tangent = vert;
		}
		//do UV stuff.
		if (pPart.HasVertexFlag(VertexFlags::TexCoords0) && (pElementDiffuseUV->GetMappingMode() == FbxGeometryElement::eByControlPoint)) {
			vec4 = pElementDiffuseUV->GetDirectArray().GetAt(i);
			uvCoords.x = vec4.mData[0];
			uvCoords.y = vec4.mData[1];
			vertice.uv0 = uvCoords;
		}
		if (pPart.HasVertexFlag(VertexFlags::TexCoords1) && (pElementOneUV->GetMappingMode() == FbxGeometryElement::eByControlPoint)) {
			vec4 = pElementOneUV->GetDirectArray().GetAt(i);
			uvCoords.x = vec4.mData[0];
			uvCoords.y = vec4.mData[1];
			vertice.uv1 = uvCoords;
		}
		if (pPart.HasVertexFlag(VertexFlags::TexCoords2) && (pElementTwoUV->GetMappingMode() == FbxGeometryElement::eByControlPoint)) {
			vec4 = pElementTwoUV->GetDirectArray().GetAt(i);
			uvCoords.x = vec4.mData[0];
			uvCoords.y = vec4.mData[1];
			vertice.uv2 = uvCoords;
		}
		if (pPart.HasVertexFlag(VertexFlags::ShadowTexture) && (pElementOMUV->GetMappingMode() == FbxGeometryElement::eByControlPoint)) {
			vec4 = pElementOMUV->GetDirectArray().GetAt(i);
			uvCoords.x = vec4.mData[0];
			uvCoords.y = vec4.mData[1];
			vertice.uv3 = uvCoords;
		}
		vertices[i] = vertice;
	}

	//Gotta be triangulated.
	if (!pMesh->IsTriangleMesh()) {
		WriteLine("pMesh->IsTriangleMesh() did not equal true.. Cannot continue.");
		return -25;
	}

	//begin getting triangles
	std::vector<Int3> indices = std::vector<Int3>();

	auto matCount = pNode->GetMaterialCount() == 0 ? 1 : pNode->GetMaterialCount();
	std::vector<SubMesh> subMeshes = std::vector<SubMesh>();

	if (pNode->GetMaterialCount() > 0)
	{
		for (int i = 0; i != matCount; i++)
		{
			SubMesh sub = SubMesh();
			FbxSurfaceMaterial* mat = pNode->GetMaterial(i);
			sub.SetMatName(std::string(mat->GetName()));
			subMeshes.push_back(sub);
		}
	}
	else
	{
		SubMesh sub = SubMesh();
		sub.SetMatName("Material0");
		subMeshes.push_back(sub);
		WriteLine("Missing material nodes on this FBX Model, Implemented whole material");
	}

	int subIDX = 0;
	std::vector<std::vector<Int3>> segments = std::vector<std::vector<Int3>>();
	int* subNumFacesCount = new int[matCount];
	for (size_t i = 0; i != matCount; i++)
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
		if (pElementDiffuseUV->GetMappingMode() != FbxGeometryElement::eByControlPoint) {
			BuildUVsFromMesh(pMesh, vertices, i, pPart);
		}
		if (pElementMaterial != nullptr)
		{
			auto matID = pElementMaterial->GetIndexArray().GetAt(i);
			segments[matID].push_back(triangle);
		}
		else
		{
			segments[0].push_back(triangle);
		}

		//Colours
		if (pPart.HasVertexFlag(VertexFlags::Color)) {
			FbxColor color = pElementColor0->GetDirectArray().GetAt(triangle.i1);
			vertices[triangle.i1].color0[0] = (color.mRed * 255.0f);
			vertices[triangle.i1].color0[1] = (color.mGreen * 255.0f);
			vertices[triangle.i1].color0[2] = (color.mBlue * 255.0f);
			//vertice.color0[3] = (color.mAlpha * 255.0f);
			vertices[triangle.i1].color0[3] = 0;

			color = pElementColor0->GetDirectArray().GetAt(triangle.i2);
			vertices[triangle.i2].color0[0] = (color.mRed * 255.0f);
			vertices[triangle.i2].color0[1] = (color.mGreen * 255.0f);
			vertices[triangle.i2].color0[2] = (color.mBlue * 255.0f);
			//vertice.color0[3] = (color.mAlpha * 255.0f);
			vertices[triangle.i2].color0[3] = 0;

			color = pElementColor0->GetDirectArray().GetAt(triangle.i3);
			vertices[triangle.i3].color0[0] = (color.mRed * 255.0f);
			vertices[triangle.i3].color0[1] = (color.mGreen * 255.0f);
			vertices[triangle.i3].color0[2] = (color.mBlue * 255.0f);
			//vertice.color0[3] = (color.mAlpha * 255.0f);
			vertices[triangle.i3].color0[3] = 0;
		}
		/* TODO:: Removed because I think we can only have one vertex colour layer per geometry.
		if (pPart.HasVertexFlag(VertexFlags::Color1)) {
			color = pElementColor1->GetDirectArray().GetAt(i);
			vertice.color1[0] = (color.mRed * 255.0f);
			vertice.color1[1] = (color.mGreen * 255.0f);
			vertice.color1[2] = (color.mBlue * 255.0f);
			//vertice.color1[3] = (color.mAlpha * 255.0f);
			vertice.color1[3] = 0;
		} */
	}

	for (size_t i = 0; i != matCount; i++)
	{
		subNumFacesCount[i] = segments[i].size();			
		indices.insert(indices.end(), segments[i].begin(), segments[i].end());
	}


	int total = pMesh->GetPolygonCount();
	int calcTotal = 0;

	for (size_t i = 0; i != matCount; i++)
	{
		calcTotal += subNumFacesCount[i];
	}

	if (calcTotal != total)
	{
		WriteLine("Potential error when splitting faces!");
	}

	int curTotal = 0;

	//remove any empty materials. 
	auto it = subMeshes.begin();
	int index = 0;
	while (it != subMeshes.end())
	{
		if (subNumFacesCount[index] == 0)
		{
			WriteLine("Removing material: %s", it->GetMatName().data());
			it = subMeshes.erase(it);
		}
		else
		{
			int faces = subNumFacesCount[index];
			it->SetNumFaces(faces);
			it->SetStartIndex(curTotal);
			curTotal += faces * 3;
			it++;
		}	
		index++;
	}

	//Update data to do with triangles and vertices
	pPart.SetVertices(vertices, numVertices);
	pPart.SetIndices(indices, indices.size());
	pPart.SetSubMeshes(subMeshes);
	delete[] subNumFacesCount;
	return 0;
}


int DetermineNodeAttribute(FbxNode* node)
{
	WriteLine("%s node has attribute of: %s", node->GetName(), node->GetNodeAttribute());
	if (node->GetNodeAttribute() != NULL)
	{
		return node->GetNodeAttribute()->GetAttributeType();
	}
	return NULL;
}

int BuildLodGroup(ModelStructure* structure, FbxNode* node)
{
	char numModels = node->GetChildCount();
	structure->SetPartSize(numModels);
	ModelPart* parts = new ModelPart[numModels];
	structure->SetName(std::string(node->GetName()));
	printf("%i", numModels);

	for (int i = 0; i < numModels; i++)
	{
		WriteLine("Converting Mesh %i", i);
		ModelPart Part = ModelPart();
		FbxNode* child = node->GetChild(i);
		int result = BuildModelPart(child, Part);
		if (result != 0)
			return result;
		parts[i] = Part;
	}
	structure->SetParts(parts);
	WriteLine("Converted Model!");
	return 0;
}
int BuildModel(ModelStructure* structure, FbxNode* node)
{
	char size = 1;
	structure->SetPartSize(size);
	ModelPart* parts = new ModelPart[1];
	structure->SetName(std::string(node->GetName()));
	WriteLine("Converting Mesh..");
	ModelPart Part = ModelPart();

	int result = BuildModelPart(node, Part);
	if (result != 0)
		return result;

	parts[0] = Part;
	structure->SetParts(parts);
	structure->SetIsSkinned(false);
	WriteLine("Converted Model!");
	return 0;
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
		return -50;
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


	ModelStructure* Structure = new ModelStructure();

	//Get Geometry or LODGroup..
	FbxNode* Root = lScene->GetRootNode();
	for (int i = 0; i != Root->GetChildCount(); i++) {
		FbxNode* pNode = Root->GetChild(i);
		if (DetermineNodeAttribute(pNode) == FbxNodeAttribute::eNull) {
			int result = BuildLodGroup(Structure, pNode);
			if (result != 0)
				return result;
			break;
		}
		else if (DetermineNodeAttribute(pNode) == FbxNodeAttribute::eMesh) {
			int result = BuildModel(Structure, pNode);
			if (result != 0)
				return result;
			break;
		}
		else if (pNode->FindChild("LOD0"))
		{
			int result = BuildLodGroup(Structure, pNode);
			if (result != 0)
				return result;
			break;
		}
	}

	if (Structure->GetPartSize() == 0)
		return -94;

	FILE* stream;
	std::string dest = pDest;
	if (!dest.find(".m2t"))
		dest += ".m2t";
	fopen_s(&stream, dest.c_str(), "wb");
	Structure->WriteToStream(stream);
	if (stream != NULL) fclose(stream);

	WriteLine("Exported %s", Structure->GetName().c_str());
	delete Structure;
	fclose(stream);
	lScene->Destroy(true);
	return 0;
}

void BuildUVsFromMesh(FbxMesh* pMesh, Vertex* vertices, int index, ModelPart& pPart)
{
	for (int i = 0; i < 3; i++)
	{
		FbxVector2 vector;
		UVVert uv;
		bool isUnmapped;
		if (pPart.HasVertexFlag(VertexFlags::TexCoords0)) {
			bool result = pMesh->GetPolygonVertexUV(index, i, "DiffuseUV", vector, isUnmapped);
			int vertIndex = pMesh->GetPolygonVertex(index, i);
			uv.x = vector[0];
			uv.y = vector[1];
			vertices[vertIndex].uv0 = uv;
		}
		if (pPart.HasVertexFlag(VertexFlags::TexCoords1)) {
			bool result = pMesh->GetPolygonVertexUV(index, i, "UV1", vector, isUnmapped);
			int vertIndex = pMesh->GetPolygonVertex(index, i);
			uv.x = vector[0];
			uv.y = vector[1];
			vertices[vertIndex].uv1 = uv;
		}
		if (pPart.HasVertexFlag(VertexFlags::TexCoords2)) {
			bool result = pMesh->GetPolygonVertexUV(index, i, "UV2", vector, isUnmapped);
			int vertIndex = pMesh->GetPolygonVertex(index, i);
			uv.x = vector[0];
			uv.y = vector[1];
			vertices[vertIndex].uv2 = uv;
		}		
		if (pPart.HasVertexFlag(VertexFlags::ShadowTexture)) {
			bool result = pMesh->GetPolygonVertexUV(index, i, "OMUV", vector, isUnmapped);
			int vertIndex = pMesh->GetPolygonVertex(index, i);
			uv.x = vector[0];
			uv.y = vector[1];
			vertices[vertIndex].uv3 = uv;
		}
	}
}
