#include "Fbx_Wrangler.h"

#include "MTObject/MT_Collision.h"
#include "MTObject/MT_Object.h"
#include "MTObject/MT_ObjectUtils.h"
#include "MTObject/MT_ObjectHandler.h"
#include "MTObject/MT_Lod.h"
#include "MTObject/MT_Skeleton.h"
#include "Utilities/FbxUtilities.h"
#include "Utilities/LogSystem.h"

namespace WranglerUtils
{
	const std::string DiffuseUVName = "DiffuseUV";
	const std::string UV0Name = "UV0";
	const std::string UV1Name = "UV1";
	const std::string OMUVName = "OMUV";
	const std::string Empty = "";
}

Fbx_Wrangler::Fbx_Wrangler(const char* InName, const char* InDest)
{
	MTOName = InName;
	FbxName = InDest;

	Logger = new LogSystem();
	Logger->Construct("M2Fbx_MTB_FBX_log.txt");
}

Fbx_Wrangler::~Fbx_Wrangler()
{
	if (LoadedObject)
	{
		LoadedObject->Cleanup();
		delete LoadedObject;
		LoadedObject = nullptr;
	}

	if (Scene)
	{
		Scene->Destroy(true);
		Scene = nullptr;
	}

	if (Logger)
	{
		Logger->Destroy();
		delete Logger;
		Logger = nullptr;
	}

	MaterialLookup.clear();
	TextureLookup.clear();

	Fbx_Utilities::DestroySdkObjects(SdkManager, true);
}

bool Fbx_Wrangler::SetupFbxManager()
{
	Fbx_Utilities::InitializeSdkObjects(SdkManager);
	if (!SdkManager)
	{
		Logger->WriteLine(ELogType::eError, "Failed to load Fbx manager!");
		return false;
	}
	return true;
}

bool Fbx_Wrangler::ConstructScene()
{
	// Create document info
	FbxDocumentInfo* DocInfo = FbxDocumentInfo::Create(SdkManager, "DocInfo");
	DocInfo->mTitle = "FBX Model";
	DocInfo->mSubject = "FBX Created by M2FBX - Used by MafiaToolkit.";
	DocInfo->mAuthor = "Greavesy";
	DocInfo->mRevision = "rev. 0.50";
	DocInfo->mKeywords = "";
	DocInfo->mComment = "";

	Scene = FbxScene::Create(SdkManager, "ToolkitScene");
	if (!Scene)
	{
		Logger->WriteLine(ELogType::eError, "Failed to construct scene!");
		return false;
	}

	Scene->SetDocumentInfo(DocInfo);
	
	return true;
}

bool Fbx_Wrangler::ConvertObjectToFbx()
{
	SetupFbxManager();
	ConstructScene();

	MT_Object* Object = MT_ObjectHandler::ReadObjectFromFile(MTOName);
	if (!Object)
	{
		Logger->Printf(ELogType::eError, "Failed to read objects from file:- %s", MTOName);
		return false;
	}

	Logger->Printf(ELogType::eInfo, "Loaded MTB file:- %s", MTOName);

	// Attempt to convert to workable FbxNode.
	FbxNode* RootNode = nullptr;
	bool bResult = ConvertObjectToNode(*Object, RootNode);
	if (!bResult)
	{
		Logger->WriteLine(ELogType::eError, "Failed to convert MT_Object to viable Fbx model!");
		return false;
	}

	Logger->Printf(ELogType::eInfo, "Converted MTB file to Fbx");

	// Save document
	SaveDocument();
	return bResult;
}

bool Fbx_Wrangler::ConvertBundleToFbx()
{
	SetupFbxManager();
	ConstructScene();

	MT_ObjectBundle* Bundle = MT_ObjectHandler::ReadBundleFromFile(MTOName);
	if (!Bundle)
	{
		// Failed
		return false;
	}

	const std::vector<MT_Object>& Objects = Bundle->GetObjects();
	for (auto& Object : Objects)
	{
		FbxNode* ObjectNode = nullptr;
		ConvertObjectToNode(Object, ObjectNode);
	}

	SaveDocument();

	return true;
}

bool Fbx_Wrangler::ConvertObjectToNode(const MT_Object& Object, FbxNode*& RootNode)
{
	std::string ObjectName = Object.GetName();
	Logger->Printf(ELogType::eInfo, "Processing object:- %s", ObjectName.data());

	// Construct name
	std::string TypeEnclosed = {};
	MT_ObjectUtils::GetTypeAsStringClosed(Object.GetType(), TypeEnclosed);
	ObjectName += TypeEnclosed;

	RootNode = FbxNode::Create(SdkManager, ObjectName.data());
	
	// Setup transform of object
	const TransformStruct& Transform = Object.GetTransform();
	RootNode->LclTranslation = { Transform.Position.x, Transform.Position.y , Transform.Position.z };
	RootNode->LclRotation = { Transform.Rotation.x, Transform.Rotation.y , Transform.Rotation.z };
	RootNode->LclScaling = { Transform.Scale.x, Transform.Scale.y , Transform.Scale.z };

	FbxSkin* Skin = nullptr;
	if (Object.HasObjectFlag(HasSkinning))
	{
		const MT_Skeleton* Skeleton = Object.GetSkeleton();
		Skin = FbxSkin::Create(SdkManager, "Skin");
		FbxNode* Joint = nullptr;
		ConvertSkeletonToNode(*Skeleton, Skin, Joint, 0);
		RootNode->AddChild(Joint);
	}

	if (Object.HasObjectFlag(HasLODs))
	{
		const std::vector<MT_Lod>& ModelLods = Object.GetLods();
		for (size_t i = 0; i < 1; i++)
		{
			// Setup name and get lod
			std::string NodeName = "LOD";
			NodeName += std::to_string(i);
			const MT_Lod& Lod = ModelLods[i];

			// Create lod and convert to object
			FbxNode* NewLodNode = FbxNode::Create(SdkManager, NodeName.data());
			bool bResult = ConvertLodToNode(Lod, NewLodNode);
			RootNode->AddChild(NewLodNode);

			if (Object.HasObjectFlag(HasSkinning))
			{
				bResult = ApplySkinToMesh(Lod, Skin, NewLodNode);
			}

			Logger->Printf(ELogType::eInfo, "Added LODIndex [%i]", i);
		}
	}

	if (Object.HasObjectFlag(HasChildren))
	{
		const std::vector<MT_Object>& Children = Object.GetChildren();
		Logger->Printf(ELogType::eInfo, "Detected %i children for object [%s]", Children.size(), ObjectName.data());
		
		for (size_t i = 0; i < Children.size(); i++)
		{
			// Convert Node
			FbxNode* ChildNode = nullptr;
			ConvertObjectToNode(Children[i], ChildNode);

			// Add Child to the RootNode
			if (ChildNode)
			{
				RootNode->AddChild(ChildNode);
				Logger->Printf(ELogType::eInfo, "Added child called:= %s", ChildNode->GetName());
			}
		}
	}

	if (Object.HasObjectFlag(HasCollisions))
	{
		if (const MT_Collision* Collision = Object.GetCollision())
		{
			FbxNode* CollisionNode = FbxNode::Create(SdkManager, "COL");
			bool bResult = ConvertCollisionToNode(*Collision, CollisionNode);
			RootNode->AddChild(CollisionNode);

			Logger->Printf(ELogType::eInfo, "Added collision to object:- ", ObjectName.data());
		}
		else
		{
			Logger->Printf(ELogType::eError, "Collision flag is enabled on [%s], but no collision data is present", ObjectName.data());
		}
	}

	FbxNode* SceneRootNode = Scene->GetRootNode();
	SceneRootNode->AddChild(RootNode);

	return true;
}

bool Fbx_Wrangler::ApplySkinToMesh(const MT_Lod& LodObject, FbxSkin*& Skin, FbxNode* MeshNode)
{
	if (LodObject.HasVertexFlag(VertexFlags::Skin))
	{
		const std::vector<Vertex>& Vertices = LodObject.GetVertices();
		for (size_t x = 0; x < Vertices.size(); x++)
		{
			const Vertex& Vert = Vertices[x];
			for (int z = 0; z < 4; z++)
			{
				if (Vert.boneWeights[z] != 0.0f)
				{
					FbxCluster* Cluster = Skin->GetCluster(Vert.boneIDs[z]);
					Cluster->AddControlPointIndex(x, Vert.boneWeights[z]);
				}
			}
		}

		auto attribute = MeshNode->GetNodeAttribute();
		FbxGeometry* geometry = (FbxGeometry*)attribute;
		geometry->AddDeformer(Skin);
	}

	return true;
}

bool Fbx_Wrangler::ConvertLodToNode(const MT_Lod& Lod, FbxNode* LodNode)
{
	// Create new FbxNodes and then set generic settings
	FbxMesh* Mesh = FbxMesh::Create(SdkManager, "Mesh");
	LodNode->SetNodeAttribute(Mesh);
	LodNode->SetShadingMode(FbxNode::eTextureShading);

	// Create main layer
	FbxLayer* Layer0 = Mesh->GetLayer(0);
	if (!Layer0)
	{
		Mesh->CreateLayer();
		Layer0 = Mesh->GetLayer(0);
	}

	// Setup initial control point array
	const std::vector<Vertex>& Vertices = Lod.GetVertices();
	Mesh->InitControlPoints(Vertices.size());
	
	// Get ControlPoints and begin filling in our Vertices data.
	if (FbxVector4* ControlPoints = Mesh->GetControlPoints())
	{
		for (size_t i = 0; i < Vertices.size(); i++)
		{
			const Point3& VertexEntry = Vertices[i].position;
			ControlPoints[i] = { VertexEntry.x, VertexEntry.y, VertexEntry.z, 0.0f };
		}
	}

	// Construct normal information
	if (Lod.HasVertexFlag(VertexFlags::Normals))
	{
		FbxLayerElementNormal* LayerElementNormal = FbxLayerElementNormal::Create(Mesh, "");
		LayerElementNormal->SetMappingMode(FbxLayerElement::eByControlPoint);
		LayerElementNormal->SetReferenceMode(FbxLayerElement::eDirect);

		FbxLayerElementArrayTemplate<FbxVector4>& DirectArray = LayerElementNormal->GetDirectArray();
		for (size_t i = 0; i < Vertices.size(); i++)
		{
			const Point3& VertexEntry = Vertices[i].normals;
			DirectArray.Add({ VertexEntry.x, VertexEntry.y, VertexEntry.z });
		}

		Layer0->SetNormals(LayerElementNormal);
	}

	// Construct tangent information
	if (Lod.HasVertexFlag(VertexFlags::Tangent))
	{
		FbxLayerElementTangent* LayerElementTangent = FbxLayerElementTangent::Create(Mesh, "");
		LayerElementTangent->SetMappingMode(FbxLayerElement::eByControlPoint);
		LayerElementTangent->SetReferenceMode(FbxLayerElement::eDirect);

		FbxLayerElementArrayTemplate<FbxVector4>& DirectArray = LayerElementTangent->GetDirectArray();
		for (size_t i = 0; i < Vertices.size(); i++)
		{
			const Point3& VertexEntry = Vertices[i].tangent;
			DirectArray.Add({ VertexEntry.x, VertexEntry.y, VertexEntry.z });
		}

		Layer0->SetTangents(LayerElementTangent);
	}

	// Construct TexCoord0
	FbxGeometryElementUV* DiffuseUV = nullptr;
	FbxGeometryElementMaterial* DiffuseMat = nullptr;
	if (Lod.HasVertexFlag(VertexFlags::TexCoords0))
	{
		DiffuseUV = CreateUVElement(Mesh, UVElementType::UV_Diffuse);
		DiffuseMat = CreateMaterialElement(Mesh, "Diffuse Mapping");

		for (size_t i = 0; i < Vertices.size(); i++)
		{
			const UVVert& VertexEntry = Vertices[i].uv0;
			DiffuseUV->GetDirectArray().Add({ VertexEntry.x, VertexEntry.y });
		}
	}

	// Construct TexCoord0
	FbxGeometryElementUV* TexCoord1UV = nullptr;
	FbxGeometryElementMaterial* TexCoord1Mat = nullptr;
	if (Lod.HasVertexFlag(VertexFlags::TexCoords1))
	{
		TexCoord1UV = CreateUVElement(Mesh, UVElementType::UV_1);
		TexCoord1Mat = CreateMaterialElement(Mesh, "UV1 Mapping");

		for (size_t i = 0; i < Vertices.size(); i++)
		{
			const UVVert& VertexEntry = Vertices[i].uv1;
			TexCoord1UV->GetDirectArray().Add({ VertexEntry.x, VertexEntry.y });
		}
	}

	// Construct TexCoord2
	FbxGeometryElementUV* TexCoord2UV = nullptr;
	FbxGeometryElementMaterial* TexCoord2Mat = nullptr;
	if (Lod.HasVertexFlag(VertexFlags::TexCoords2))
	{
		TexCoord2UV = CreateUVElement(Mesh, UVElementType::UV_2);
		TexCoord2Mat = CreateMaterialElement(Mesh, "UV2 Mapping");

		for (size_t i = 0; i < Vertices.size(); i++)
		{
			const UVVert& VertexEntry = Vertices[i].uv2;
			TexCoord2UV->GetDirectArray().Add({ VertexEntry.x, VertexEntry.y });
		}
	}

	// Construct OMUV
	FbxGeometryElementUV* OmissiveUV = nullptr;
	FbxGeometryElementMaterial* OmmisiveMat = nullptr;
	if (Lod.HasVertexFlag(VertexFlags::ShadowTexture))
	{
		OmissiveUV = CreateUVElement(Mesh, UVElementType::UV_Omissive);
		OmmisiveMat = CreateMaterialElement(Mesh, "Omissive Mapping");

		for (size_t i = 0; i < Vertices.size(); i++)
		{
			const UVVert& VertexEntry = Vertices[i].uv3;
			OmissiveUV->GetDirectArray().Add({ VertexEntry.x, VertexEntry.y });
		}
	}

	// Construct Damage Group channel
	/*FbxGeometryElementVertexColor* DamageGroupChannel = nullptr;
	if (Lod.HasVertexFlag(VertexFlags::DamageGroup))
	{
		DamageGroupChannel = Mesh->CreateElementVertexColor();
		DamageGroupChannel->SetName("DamageGroup_VertexColours");
		DamageGroupChannel->SetMappingMode(FbxLayerElement::eByPolygonVertex);
		DamageGroupChannel->SetReferenceMode(FbxLayerElement::eDirect);
		DamageGroupChannel->GetIndexArray().SetCount(Lod.GetIndices().size());

		FbxLayerElementArrayTemplate<FbxColor>& ColourArray = DamageGroupChannel->GetDirectArray();
		for (size_t i = 0; i < Vertices.size(); i++)
		{
			const unsigned int DamageGroup = Vertices[i].damageGroup;
			float VertexColour = (DamageGroup / 10.0f);

			FbxColor colour = { VertexColour, VertexColour, VertexColour };
			ColourArray.Add(colour);
		}

		Layer0->SetVertexColors(DamageGroupChannel);
	}*/

	// Setup Indices of object
	const std::vector<MT_FaceGroup>& FaceGroups = Lod.GetFaceGroups();
	const std::vector<Int3>& Indices = Lod.GetIndices();
	for (size_t i = 0; i < FaceGroups.size(); i++)
	{
		const MT_FaceGroup& FaceGroup = FaceGroups[i];
		FbxSurfacePhong* FaceGroupMaterial = CreateMaterial(FaceGroup.GetMaterialInstance());
		LodNode->AddMaterial(FaceGroupMaterial);

		const size_t StartIndex = FaceGroup.GetStartIndex() / 3;
		const size_t NumFaces = FaceGroup.GetNumFaces();

		for (size_t x = StartIndex; x < StartIndex + NumFaces; x++)
		{
			const Int3& Tri = Indices[x];
			Mesh->BeginPolygon(i);
			Mesh->AddPolygon(Tri.i1);
			Mesh->AddPolygon(Tri.i2);
			Mesh->AddPolygon(Tri.i3);
			Mesh->EndPolygon();

			// Set DiffuseUV Mapping
			if (Lod.HasVertexFlag(VertexFlags::TexCoords0))
			{
				FBX_ASSERT(DiffuseUV);//
				DiffuseUV->GetIndexArray().Add(i);
			}

			// Set UV1 Mapping
			if (Lod.HasVertexFlag(VertexFlags::TexCoords1))
			{
				FBX_ASSERT(TexCoord1UV);
				TexCoord1UV->GetIndexArray().Add(0);
			}

			// Set UV2 Mapping
			if (Lod.HasVertexFlag(VertexFlags::TexCoords2))
			{
				FBX_ASSERT(TexCoord2UV);
				TexCoord2UV->GetIndexArray().Add(0);
			}

			// Set Omissive Mapping
			if (Lod.HasVertexFlag(VertexFlags::ShadowTexture))
			{
				FBX_ASSERT(OmissiveUV);
				OmissiveUV->GetIndexArray().Add(0);
			}
		}
	}

	return true;
}

bool Fbx_Wrangler::ConvertCollisionToNode(const MT_Collision& Collision, FbxNode* CollisionNode)
{
	// Create new FbxNodes then set attributes
	FbxMesh* Mesh = FbxMesh::Create(SdkManager, "CollisionMesh");
	CollisionNode->SetNodeAttribute(Mesh);
	CollisionNode->SetShadingMode(FbxNode::eHardShading);

	// Create MainLayer
	FbxLayer* Layer0 = Mesh->GetLayer(0);
	if (!Layer0)
	{
		Mesh->CreateLayer();
		Layer0 = Mesh->GetLayer(0);
	}

	// Setup Initial Control Point Array
	const std::vector<Point3>& Vertices = Collision.GetVertices();
	Mesh->InitControlPoints(Vertices.size());
	if (FbxVector4* ControlPoints = Mesh->GetControlPoints())
	{
		for (size_t i = 0; i < Vertices.size(); i++)
		{
			const Point3& VertexEntry = Vertices[i];
			ControlPoints[i] = FbxVector4{ VertexEntry.x, VertexEntry.y, VertexEntry.z };
		}
	}

	// Setup Triangle data
	const std::vector<MT_FaceGroup>& FaceGroups = Collision.GetFaceGroups();
	const std::vector<Int3>& Indices = Collision.GetIndices();
	CreateMaterialElement(Mesh, "CollisionMapping");

	for (size_t i = 0; i < FaceGroups.size(); i++)
	{
		const MT_FaceGroup& FaceGroup = FaceGroups[i];
		FbxSurfacePhong* FaceGroupMaterial = CreateMaterial(FaceGroup.GetMaterialInstance());
		CollisionNode->AddMaterial(FaceGroupMaterial);

		const size_t StartIndex = FaceGroup.GetStartIndex() / 3;
		const size_t NumFaces = FaceGroup.GetNumFaces();

		for (size_t x = StartIndex; x < StartIndex + NumFaces; x++)
		{
			const Int3& Tri = Indices[x];
			Mesh->BeginPolygon(i);
			Mesh->AddPolygon(Tri.i1);
			Mesh->AddPolygon(Tri.i2);
			Mesh->AddPolygon(Tri.i3);
			Mesh->EndPolygon();
		}
	}

	return true;
}

bool Fbx_Wrangler::ConvertSkeletonToNode(const MT_Skeleton& Skeleton, FbxSkin*& Skin, FbxNode*& BoneRoot, const int LODIndex)
{
	std::vector<FbxNode*> BoneNodes = {};
	std::vector<FbxCluster*> ClusterNodes = {};

	const std::vector<MT_Joint>& Joints = Skeleton.GetJoints();
	for (size_t i = 0; i < Joints.size(); i++)
	{
		const MT_Joint& JointObject = Joints[i];
		const std::string& Name = JointObject.GetName();
		FbxString SkeletonName = Name.data();
		FbxString ClusterName = Name.data();

		// Append names
		SkeletonName.Append("_Skeleton", 10);
		ClusterName.Append("_Cluster", 8);

		// Construct Transform 
		const JointMatrix& Transform = JointObject.GetTransform();

		FbxQuaternion quaterion = FbxQuaternion(Transform.Rotation.x, Transform.Rotation.y, Transform.Rotation.z, Transform.Rotation.w);
		FbxVector4 euler;
		euler.SetXYZ(quaterion);

		FbxAMatrix lTransformMatrix;
		lTransformMatrix.SetIdentity();

		lTransformMatrix.SetT(FbxVector4(Transform.Position.x, Transform.Position.y, Transform.Position.z));
		lTransformMatrix.SetQ(quaterion);
		lTransformMatrix.SetS(FbxVector4(Transform.Scale.x, Transform.Scale.y, Transform.Scale.z));

		// Construct Node to contain Skeleton and Link for Cluster
		// (Then Construct transform)
		FbxNode* JointNode = FbxNode::Create(SdkManager, Name.data());
		JointNode->LclTranslation.Set(lTransformMatrix.GetT());
		JointNode->LclRotation.Set(lTransformMatrix.GetR());
		JointNode->LclScaling.Set(lTransformMatrix.GetS());

		// Setup Parenting
		const int ParentIndex = JointObject.GetParentJointIndex();
		if (ParentIndex != 0xFF)
		{
			BoneNodes[ParentIndex]->AddChild(JointNode);
		}
		else
		{
			BoneRoot = JointNode;
		}

		// Create FbxSkeleton Joint
		FbxSkeleton* JointFbx = FbxSkeleton::Create(SdkManager, SkeletonName);
		JointFbx->SetSkeletonType(FbxSkeleton::EType::eLimbNode);
		JointNode->SetNodeAttribute(JointFbx);

		// Create FbxCluster
		FbxCluster* ClusterFbx = FbxCluster::Create(SdkManager, ClusterName);
		ClusterFbx->SetLinkMode(FbxCluster::eTotalOne);
		ClusterFbx->SetLink(JointNode);
		ClusterFbx->SetTransformLinkMatrix(JointNode->EvaluateGlobalTransform());
		Skin->AddCluster(ClusterFbx);

		BoneNodes.push_back(JointNode);
	}

	return true;
}

FbxGeometryElementUV* Fbx_Wrangler::CreateUVElement(FbxMesh* Mesh, const UVElementType Type)
{
	const std::string& UVName = GetNameByUVType(Type);
	return CreateUVElement(Mesh, UVName.data());
}

FbxGeometryElementUV* Fbx_Wrangler::CreateUVElement(FbxMesh* Mesh, const char* Name)
{
	if (FbxGeometryElementUV* UVElement = Mesh->CreateElementUV(Name))
	{
		Logger->Printf(ELogType::eInfo, "Setup UV element called:- %s", Name);

		UVElement->SetMappingMode(FbxGeometryElement::eByControlPoint);
		UVElement->SetReferenceMode(FbxGeometryElement::eDirect);

		return UVElement;
	}

	return nullptr;
}

FbxGeometryElementMaterial* Fbx_Wrangler::CreateMaterialElement(FbxMesh* pMesh, const char* pName)
{
	if (FbxGeometryElementMaterial* MatElement = pMesh->CreateElementMaterial())
	{
		Logger->Printf(ELogType::eInfo, "Setup material called:- %s", pName);

		MatElement->SetName(pName);
		MatElement->SetMappingMode(FbxLayerElement::eByPolygon);
		MatElement->SetReferenceMode(FbxLayerElement::eIndexToDirect);

		return MatElement;
	}

	return nullptr;
}

FbxSurfacePhong* Fbx_Wrangler::CreateMaterial(const MT_MaterialInstance& MaterialInstance)
{
	// Check if the Material exists first.
	// We don't want to duplicate Materials (otherwise Blender has tantrums)
	const uint64_t MaterialHash = Fbx_Utilities::FNV64_Hash(MaterialInstance.GetName());
	if (MaterialLookup.find(MaterialHash) != MaterialLookup.end())
	{
		return MaterialLookup[MaterialHash];
	}

	const FbxString& MaterialName = MaterialInstance.GetName().data();
	const FbxString& ShadingName = "Phong";

	// Try and construct a new material.
	if (FbxSurfacePhong* NewMaterial = FbxSurfacePhong::Create(SdkManager, MaterialName.Buffer()))
	{
		Logger->Printf(ELogType::eInfo, "Setup material called:- %s", MaterialName.Buffer());

		// Set shading (and possibly diffuse texture if we have material flag)
		NewMaterial->ShadingModel.Set(ShadingName);
		if (MaterialInstance.HasMaterialFlag(HasDiffuse))
		{
			NewMaterial->Diffuse.ConnectSrcObject(CreateTexture(MaterialInstance.GetTextureName()));
		}

		MaterialLookup.insert({ MaterialHash, NewMaterial });

		return NewMaterial;
	}

	return nullptr;
}

FbxTexture* Fbx_Wrangler::CreateTexture(const std::string& Name)
{
	// Check if the Texture exists first.
	// We don't want to duplicate Materials (otherwise Blender has tantrums)
	const uint64_t TextureHash = Fbx_Utilities::FNV64_Hash(Name);
	if (TextureLookup.find(TextureHash) != TextureLookup.end())
	{
		return TextureLookup[TextureHash];
	}

	FbxFileTexture* NewTexture = FbxFileTexture::Create(SdkManager, Name.data());
	const FbxString& Path = FbxGetApplicationDirectory();
	const FbxString& TextureString = Name.data();

	// Set texture properties.
	NewTexture->SetFileName(TextureString.Buffer());
	NewTexture->SetName(Name.data());
	NewTexture->SetTextureUse(FbxTexture::eStandard);
	NewTexture->SetMappingType(FbxTexture::eUV);
	NewTexture->SetMaterialUse(FbxFileTexture::eModelMaterial);
	NewTexture->SetSwapUV(false);
	NewTexture->SetAlphaSource(FbxTexture::eNone);
	NewTexture->SetTranslation(0.0, 0.0);
	NewTexture->SetScale(1.0, 1.0);
	NewTexture->SetRotation(0.0, 0.0);

	TextureLookup.insert({ TextureHash, NewTexture });
	return NewTexture;
}

const std::string& Fbx_Wrangler::GetNameByUVType(const UVElementType Type)
{
	switch (Type)
	{
	case UVElementType::UV_Diffuse:
	{
		return WranglerUtils::DiffuseUVName;
	}
	case UVElementType::UV_1:
	{
		return WranglerUtils::UV0Name;
	}
	case UVElementType::UV_2:
	{
		return WranglerUtils::UV1Name;
	}
	case UVElementType::UV_Omissive:
	{
		return WranglerUtils::OMUVName;
	}
	default:
	{
		// unhandled type
		return WranglerUtils::Empty;
	}
	}

	return WranglerUtils::Empty;
}

bool Fbx_Wrangler::SaveDocument()
{
	FbxExporter* Exporter = FbxExporter::Create(SdkManager, "");
	
	// Set the export states. By default, the export states are always set to 
	// true except for the option eEXPORT_TEXTURE_AS_EMBEDDED. The code below 
	// shows how to change these states.
	IOS_REF.SetBoolProp(EXP_FBX_MATERIAL, true);
	IOS_REF.SetBoolProp(EXP_FBX_TEXTURE, true);
	IOS_REF.SetBoolProp(EXP_FBX_EMBEDDED, false);
	IOS_REF.SetBoolProp(EXP_FBX_ANIMATION, false);
	IOS_REF.SetBoolProp(EXP_FBX_GLOBAL_SETTINGS, true);

	bool bWasSuccessful = false;

	// attempt to initialise
	if (Exporter->Initialize(FbxName, 0, SdkManager->GetIOSettings()))
	{
		// attempt to export
		if (Exporter->Export(Scene))
		{
			Logger->Printf(ELogType::eInfo, "Exported to file called:- %s", FbxName);
			bWasSuccessful = true;
		}

		// destroy
		Exporter->Destroy();
		Exporter = nullptr;

		return bWasSuccessful;
	}
	
	Logger->WriteLine(ELogType::eError, "Failed to export the file!");
	return bWasSuccessful;
}
