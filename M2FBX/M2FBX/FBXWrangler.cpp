#include "FbxWrangler.h"
#include "FbxUtilities.h"

bool SaveDocument(FbxManager* pManager, FbxDocument* pDocument, const char* pFilename, int pFileFormat = -1, bool pEmbedMedia = false)
{
	int lMajor, lMinor, lRevision;
	bool lStatus = true;

	// Create an exporter.
	FbxExporter* lExporter = FbxExporter::Create(pManager, "");

	if (pFileFormat < 0 || pFileFormat >= pManager->GetIOPluginRegistry()->GetWriterFormatCount())
	{
		// Write in fall back format if pEmbedMedia is true
		pFileFormat = pManager->GetIOPluginRegistry()->GetNativeWriterFormat();

		if (!pEmbedMedia)
		{
			//Try to export in ASCII if possible
			int lFormatIndex, lFormatCount = pManager->GetIOPluginRegistry()->GetWriterFormatCount();

			for (lFormatIndex = 0; lFormatIndex < lFormatCount; lFormatIndex++)
			{
				if (pManager->GetIOPluginRegistry()->WriterIsFBX(lFormatIndex))
				{
					FbxString lDesc = pManager->GetIOPluginRegistry()->GetWriterFormatDescription(lFormatIndex);
					const char *lASCII = "ascii";
					if (lDesc.Find(lASCII) >= 0)
					{
						pFileFormat = lFormatIndex;
						break;
					}
				}
			}
		}
	}

	// Set the export states. By default, the export states are always set to 
	// true except for the option eEXPORT_TEXTURE_AS_EMBEDDED. The code below 
	// shows how to change these states.
	IOS_REF.SetBoolProp(EXP_FBX_MATERIAL, true);
	IOS_REF.SetBoolProp(EXP_FBX_TEXTURE, true);
	IOS_REF.SetBoolProp(EXP_FBX_EMBEDDED, pEmbedMedia);
	IOS_REF.SetBoolProp(EXP_FBX_ANIMATION, true);
	IOS_REF.SetBoolProp(EXP_FBX_GLOBAL_SETTINGS, true);

	// Initialize the exporter by providing a filename.
	if (!lExporter->Initialize(pFilename, pFileFormat, pManager->GetIOSettings()))
	{
		FBXSDK_printf("Call to FbxExporter::Initialize() failed.\n");
		FBXSDK_printf("Error returned: %s\n\n", lExporter->GetStatus().GetErrorString());
		return false;
	}

	FbxManager::GetFileFormatVersion(lMajor, lMinor, lRevision);
	FBXSDK_printf("FBX version number for this version of the FBX SDK is %d.%d.%d\n\n", lMajor, lMinor, lRevision);

	// Export the scene.
	lStatus = lExporter->Export(pDocument);

	// Destroy the exporter.
	lExporter->Destroy();
	return lStatus;
}
int ConvertM2T(const char* pSource, const char* pDest)
{
	FBXSDK_printf("Converting M2T to FBX.\n");

	FbxManager* lSdkManager = nullptr;
	FbxScene* lScene = nullptr;
	bool lResult = false;

	//Open stream.
	FILE* stream;
	fopen_s(&stream, pSource, "rb");

	if (!stream)
		return 0;

	// Load Model data, and close stream.
	ModelStructure file = ModelStructure();
	file.ReadFromStream(stream);
	fclose(stream);

	// Prepare the FBX SDK.
	InitializeSdkObjects(lSdkManager);

	// create the main document
	lScene = FbxScene::Create(lSdkManager, "Scene");

	// Create the scene.
	lResult = CreateDocument(lSdkManager, lScene, file);
	if (lResult)
	{
		//Save the document
		lResult = SaveDocument(lSdkManager, lScene, pDest);
		if (!lResult) FBXSDK_printf("\n\nAn error occurred while saving the document...\n");
	}
	else FBXSDK_printf("\n\nAn error occurred while creating the document...\n");

	// Destroy all objects created by the FBX SDK.
	DestroySdkObjects(lSdkManager, lResult);
}
bool CreateDocument(FbxManager* pManager, FbxScene* pScene, ModelStructure model)
{
	int lCount;

	// create document info
	FbxDocumentInfo* lDocInfo = FbxDocumentInfo::Create(pManager, "DocInfo");
	lDocInfo->mTitle = "Example document";
	lDocInfo->mSubject = "Illustrates the creation of FbxDocument with geometries, materials and lights.";
	lDocInfo->mAuthor = "ExportDocument.exe sample program.";
	lDocInfo->mRevision = "rev. 1.0";
	lDocInfo->mKeywords = "Fbx document";
	lDocInfo->mComment = "no particular comments required.";

	// add the documentInfo
	pScene->SetDocumentInfo(lDocInfo);
	// NOTE: Objects created directly in the SDK Manager are not visible
	// to the disk save routines unless they are manually connected to the
	// documents (see below). Ideally, one would directly use the FbxScene/FbxDocument
	// during the creation of objects so they are automatically connected and become visible
	// to the disk save routines.

	FbxNode* lPlane = CreatePlane(pManager, model.GetName().c_str(), model);

	// add the geometry to the main document.	
	FbxNode* node = pScene->GetRootNode();
	node->AddChild(lPlane);
	pScene->AddRootMember(lPlane);

	lCount = pScene->GetRootMemberCount();  // lCount = 1: only the lPlane
	lCount = pScene->GetMemberCount();      // lCount = 3: the FbxNode - lPlane; FbxMesh belongs to lPlane; Material that connect to lPlane

	//Create sub document to contain lights
	//FbxDocument* lLightDocument = FbxDocument::Create(pManager,"Light");
	//CreateLightDocument(pManager, lLightDocument);
	//Connect the light sub document to main document
	//pScene->AddMember(lLightDocument);

	lCount = pScene->GetMemberCount();       // lCount = 5 : 3 add two sub document

	//document can contain animation. Please refer to other sample about how to set animation
	//pScene->CreateAnimStack("PlanAnim");
	//lCount = pScene->GetRootMemberCount();  // lCount = 1: only the lPlane
	//lCount = pScene->GetMemberCount();      // lCount = 7: 5 add AnimStack and AnimLayer
	//lCount = pScene->GetMemberCount<FbxDocument>();    // lCount = 2

	return true;
}
void CreateLightDocument(FbxManager* pManager, FbxDocument* pLightDocument)
{
	// create document info
	FbxDocumentInfo* lDocInfo = FbxDocumentInfo::Create(pManager, "DocInfo");
	lDocInfo->mTitle = "Sub document for lights";
	lDocInfo->mSubject = "Illustrates the creation of sub-FbxDocument with lights.";
	lDocInfo->mAuthor = "ExportDocument.exe sample program.";
	lDocInfo->mRevision = "rev. 1.0";
	lDocInfo->mKeywords = "Fbx light document";
	lDocInfo->mComment = "no particular comments required.";

	// add the documentInfo
	pLightDocument->SetDocumentInfo(lDocInfo);

	// add light objects to the sub document
	pLightDocument->AddMember(CreateLight(pManager, FbxLight::eSpot));
	pLightDocument->AddMember(CreateLight(pManager, FbxLight::ePoint));
}
FbxNode* CreatePlane(FbxManager* pManager, const char* pName, ModelStructure model)
{
	FbxMesh* lMesh = FbxMesh::Create(pManager, pName);

	ModelPart part = model.GetParts()[0];
	std::vector<Point3> vertices = part.GetVertices();
	std::vector<Int3> triangles = part.GetIndices();
	std::vector<Point3> normals = part.GetNormals();
	std::vector<Point3> tangents = part.GetTangents();
	std::vector<UVVert> uvs0 = part.GetUV0s();
	std::vector<UVVert> uvs1 = part.GetUV1s();
	std::vector<UVVert> uvs2 = part.GetUV2s();
	std::vector<UVVert> uvs7 = part.GetUV7s();
	std::vector<short> matIDs = part.GetMatIDs();

	lMesh->InitControlPoints(vertices.size());
	FbxVector4* lControlPoints = lMesh->GetControlPoints();

	FbxGeometryElementVertexColor* lUVVCElement = nullptr;
	FbxGeometryElementUV* lUVOMElement = nullptr;

	for (size_t i = 0; i < vertices.size(); i++)
		lControlPoints[i] = FbxVector4(vertices[i].x, vertices[i].y, vertices[i].z);

	// We want to have one normal for each vertex (or control point),
	// so we set the mapping mode to eByControlPoint.
	if (part.GetHasNormals())
	{
		FbxGeometryElementNormal* lElementNormal = lMesh->CreateElementNormal();
		lElementNormal->SetMappingMode(FbxGeometryElement::eByControlPoint);
		lElementNormal->SetReferenceMode(FbxGeometryElement::eDirect);
		for (size_t i = 0; i < vertices.size(); i++)
			lElementNormal->GetDirectArray().Add(FbxVector4(normals[i].x, normals[i].y, normals[i].z));
	}
	if (part.GetHasTangents())
	{
		FbxGeometryElementTangent* lElementTangent = lMesh->CreateElementTangent();
		lElementTangent->SetMappingMode(FbxGeometryElement::eByControlPoint);
		lElementTangent->SetReferenceMode(FbxGeometryElement::eDirect);
		for (size_t i = 0; i < vertices.size(); i++)
			lElementTangent->GetDirectArray().Add(FbxVector4(tangents[i].x, tangents[i].y, tangents[i].z));
	}

	// Create UV for Diffuse channel.
	FbxGeometryElementUV* lUVDiffuseElement = lMesh->CreateElementUV("DiffuseUV");
	FBX_ASSERT(lUVDiffuseElement != nullptr);
	lUVDiffuseElement->SetMappingMode(FbxGeometryElement::eByControlPoint);
	lUVDiffuseElement->SetReferenceMode(FbxGeometryElement::eDirect);

	for (size_t i = 0; i < vertices.size(); i++)
	{
		if(part.GetHasUV0())
			lUVDiffuseElement->GetDirectArray().Add(FbxVector2(uvs0[i].x, uvs0[i].y));
		else
			lUVDiffuseElement->GetDirectArray().Add(FbxVector2(0.0, 1.0));
	}

	//Now we have set the UVs as eIndexToDirect reference and in eByPolygonVertex  mapping mode
	//we must update the size of the index array.
	lUVDiffuseElement->GetIndexArray().SetCount(triangles.size() * 3);

	if (part.GetHasUV1() && part.GetHasUV2())
	{
		//Create VC in channels;
		lUVVCElement = lMesh->CreateElementVertexColor();
		FBX_ASSERT(lUVVCElement != nullptr);
		lUVVCElement->SetMappingMode(FbxGeometryElement::eByControlPoint);
		lUVVCElement->SetReferenceMode(FbxGeometryElement::eDirect);

		for (size_t i = 0; i < vertices.size(); i++)
			lUVVCElement->GetDirectArray().Add(FbxColor(uvs1[i].x, uvs1[i].y, uvs2[i].x, uvs2[i].y));

	}

	if (part.GetHasUV7())
	{
		// Create UV for OM channel.
		lUVOMElement = lMesh->CreateElementUV("OMUV", FbxLayerElement::eTextureAmbient);
		FBX_ASSERT(lUVOMElement != nullptr);
		lUVOMElement->SetMappingMode(FbxGeometryElement::eByControlPoint);
		lUVOMElement->SetReferenceMode(FbxGeometryElement::eDirect);

		for (size_t i = 0; i < vertices.size(); i++)
			lUVOMElement->GetDirectArray().Add(FbxVector2(uvs7[i].x, uvs7[i].y));

		//Now we have set the UVs as eIndexToDirect reference and in eByPolygonVertex  mapping mode
		//we must update the size of the index array.
		//lUVOMElement->GetIndexArray().SetCount(triangles.size() * 3);
	}

	//Now we have set the UVs as eIndexToDirect reference and in eByPolygonVertex  mapping mode
	//we must update the size of the index array.
	

	// Create polygons. Assign texture and texture UV indices.
	// all faces of the cube have the same texture

	lMesh->BeginPolygon(-1, -1, -1, false);

	for (size_t i = 0; i != triangles.size(); i++)
	{
		// Control point index
		lMesh->AddPolygon(triangles[i].i1);
		lMesh->AddPolygon(triangles[i].i2);
		lMesh->AddPolygon(~triangles[i].i3);

		// update the index array of the UVs that map the texture to the face
		//lUVDiffuseElement->GetIndexArray().SetAt(i, 0);

		if(part.GetHasUV1() && part.GetHasUV2())
			lUVVCElement->GetIndexArray().SetAt(i, 0);
	}

	lMesh->EndPolygon();


	// create a FbxNode
	FbxNode* lNode = FbxNode::Create(pManager, pName);

	// set the node attribute
	lNode->SetNodeAttribute(lMesh);

	// set the shading mode to view texture
	lNode->SetShadingMode(FbxNode::eTextureShading);

	// rotate the plane
	lNode->LclRotation.Set(FbxVector4(-90, 0, 0));

	FbxGeometryElementMaterial* lMaterialElement = nullptr;
	FbxGeometryElementMaterial* lOMElement = nullptr;

	// Set material mapping.
	lMaterialElement = lMesh->CreateElementMaterial();
	lMaterialElement->SetName("Diffuse Mapping");
	lMaterialElement->SetMappingMode(FbxGeometryElement::eByPolygon);
	lMaterialElement->SetReferenceMode(FbxGeometryElement::eIndexToDirect);
	if (!lMesh->GetElementMaterial(0))
		return nullptr;

	// We are in eByPolygon, so there's only need for index (a plane has 1 polygon).
	lMaterialElement->GetIndexArray().SetCount(lMesh->GetPolygonSize(0)/3);

	if (part.GetHasUV7())
	{
		lOMElement = lMesh->CreateElementMaterial();
		lOMElement->SetName("AO/OM Mapping");
		lOMElement->SetMappingMode(FbxGeometryElement::eByPolygon);
		lOMElement->SetReferenceMode(FbxGeometryElement::eIndexToDirect);
		if (!lMesh->GetElementMaterial(1))
			return nullptr;

		// We are in eByPolygon, so there's only need for index (a plane has 1 polygon).
		lOMElement->GetIndexArray().SetCount(lMesh->GetPolygonSize(0)/3);
	}

	for (int i = 0; i != part.GetSubMeshCount(); i++)
	{
		SubMesh sub = part.GetSubMeshes()[i];
		for (int x = sub.GetStartIndex()/3; x != (sub.GetStartIndex() / 3) + (sub.GetNumFaces()); x++)
			lMaterialElement->GetIndexArray().SetAt(x, i);
	}

	if (part.GetHasUV7())
	{
		for (int i = 0; i < lMesh->GetPolygonSize(0) / 3; ++i)
			lOMElement->GetIndexArray().SetAt(i, 0);
	}

	for (int i = 0; i < part.GetSubMeshCount(); i++)
	{
		SubMesh sub = part.GetSubMeshes()[i];
		lNode->AddMaterial(CreateMaterial(pManager, sub.GetMatName().c_str()));
	}

	// return the FbxNode
	return lNode;
}


// Create a texture
FbxTexture*  CreateTexture(FbxManager* pManager, const char* pName)
{
	FbxFileTexture* lTexture = FbxFileTexture::Create(pManager, pName);

	// Resource file must be in the application's directory.
	FbxString lPath = FbxGetApplicationDirectory();
	FbxString lTexPath = pName;

	// Set texture properties.
	lTexture->SetFileName(lTexPath.Buffer());
	lTexture->SetName(pName);
	lTexture->SetTextureUse(FbxTexture::eStandard);
	lTexture->SetMappingType(FbxTexture::eUV);
	lTexture->SetMaterialUse(FbxFileTexture::eModelMaterial);
	lTexture->SetSwapUV(false);
	lTexture->SetAlphaSource(FbxTexture::eNone);
	lTexture->SetTranslation(0.0, 0.0);
	lTexture->SetScale(1.0, 1.0);
	lTexture->SetRotation(0.0, 0.0);

	return lTexture;
}

// Create material.
// FBX scene must connect materials FbxNode, otherwise materials will not be exported.
// FBX document don't need connect materials to FbxNode, it can export standalone materials.
FbxSurfacePhong* CreateMaterial(FbxManager* pManager, const char* pName)
{
	FbxString lMaterialName = pName;
	FbxString lShadingName = "Phong";
	FbxDouble3 lBlack(0.0, 0.0, 0.0);
	FbxDouble3 lRed(1.0, 0.0, 0.0);
	FbxDouble3 lDiffuseColor(0.75, 0.75, 0.0);
	FbxSurfacePhong* lMaterial = FbxSurfacePhong::Create(pManager, lMaterialName.Buffer());

	// Generate primary and secondary colors.
	lMaterial->Emissive.Set(lBlack);
	lMaterial->Ambient.Set(lRed);
	lMaterial->AmbientFactor.Set(1.);
	// Add texture for diffuse channel
	lMaterial->Diffuse.ConnectSrcObject(CreateTexture(pManager, pName));
	lMaterial->DiffuseFactor.Set(1.);
	lMaterial->TransparencyFactor.Set(0.4);
	lMaterial->ShadingModel.Set(lShadingName);
	lMaterial->Shininess.Set(0.5);
	lMaterial->Specular.Set(lBlack);
	lMaterial->SpecularFactor.Set(0.3);

	return lMaterial;
}

// Create light.
FbxNode* CreateLight(FbxManager* pManager, FbxLight::EType pType)
{
	FbxString lLightName;
	FbxDouble val;

	switch (pType)
	{
	case FbxLight::eSpot:
		lLightName = "SpotLight";
		break;
	case FbxLight::ePoint:
		lLightName = "PointLight";
		break;
	case FbxLight::eDirectional:
		lLightName = "DirectionalLight";
		break;
	default:
		break;
	}

	FbxLight* lFbxLight = FbxLight::Create(pManager, lLightName.Buffer());

	lFbxLight->LightType.Set(pType);

	// parameters for spot light
	if (pType == FbxLight::eSpot)
	{
		lFbxLight->InnerAngle.Set(40.0);
		val = lFbxLight->InnerAngle.Get(); // val = 40

		lFbxLight->OuterAngle.Set(40);
		val = lFbxLight->OuterAngle.Get(); // val = 40
	}

	//
	// Light Color...
	//
	FbxDouble3 lColor;
	lColor[0] = 0.0;
	lColor[1] = 1.0;
	lColor[2] = 0.5;
	lFbxLight->Color.Set(lColor);
	FbxDouble3 val3 = lFbxLight->Color.Get(); // val3 = (0, 1, 0.5) 

	//
	// Light Intensity...
	//
	lFbxLight->Intensity.Set(100.0);
	val = lFbxLight->Intensity.Get(); // val = 100

	// create a FbxNode
	FbxNode* lNode = FbxNode::Create(pManager, lLightName + "Node");

	// set the node attribute
	lNode->SetNodeAttribute(lFbxLight);
	lNode->LclTranslation.Set(FbxDouble3(20, 30, 100));
	val3 = lNode->LclTranslation.Get(); // val3 = (20, 30, 100) 

	return lNode;
}