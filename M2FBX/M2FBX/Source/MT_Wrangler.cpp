#include "MT_Wrangler.h"

#include "MTObject/MT_Collision.h"
#include "MTObject/MT_FaceGroup.h"
#include "MTObject/MT_Object.h"
#include "MTObject/MT_ObjectUtils.h"
#include "MTObject/MT_Skeleton.h"
#include "MTObject/MT_ObjectHandler.h"
#include "Utilities/FbxUtilities.h"
#include "Utilities/LogSystem.h"

#include <map>

namespace WranglerUtils
{
	std::map<uint, std::string> UVLookupMap
	{
		{0, "DiffuseUV"},
		{1, "UV0"},
		{2, "UV1"},
		{3, "OMUV"},
	};
}

MT_Wrangler::MT_Wrangler(const char* InName, const char* InDest)
{
	FbxName = InName;
	MTOName = InDest;

	Logger = new LogSystem();
	Logger->Construct("M2Fbx_FBX_MTB_log.txt");
}

MT_Wrangler::~MT_Wrangler()
{
	if (Scene)
	{
		Scene->Destroy(true);
		Scene = nullptr;
	}

	if (LoadedBundle)
	{
		LoadedBundle->Cleanup();
		delete LoadedBundle;
		LoadedBundle = nullptr;
	}

	if (Logger)
	{
		Logger->Destroy();
		delete Logger;
		Logger = nullptr;
	}

	Fbx_Utilities::DestroySdkObjects(SdkManager, true);

}

bool MT_Wrangler::SetupFbxManager()
{
	Fbx_Utilities::InitializeSdkObjects(SdkManager);
	if (!SdkManager)
	{
		Logger->WriteLine(ELogType::eError, "Failed to construct Fbx SDK Manager");
		return false;
	}

	return true;
}

bool MT_Wrangler::SetupImporter()
{
	FbxImporter* Importer = FbxImporter::Create(SdkManager, "");
	FBX_ASSERT(Importer);

	// Attempt to initialise the scene
	if (!Importer->Initialize(FbxName, -1, SdkManager->GetIOSettings()))
	{
		Logger->WriteLine(ELogType::eError, "Failed to initialise Fbx Importer, cannot continue with conversion.");
		return false;
	}

	// move imported data into scene
	Scene = FbxScene::Create(SdkManager, "Scene");
	if (!Scene)
	{
		Logger->WriteLine(ELogType::eError, "Failed to construct an FbxScene, cannot continue with conversion.");
		return false;
	}

	// Attempt to import the scene
	if (!Importer->Import(Scene))
	{
		Logger->WriteLine(ELogType::eError, "Failed to import Fbx file into the FbxScene, cannot continue with conversion.");
		return false;
	}

	// destroy importer
	Importer->Destroy();
	Importer = nullptr;

	Logger->Printf(ELogType::eInfo, "Imported file:- %s", FbxName);

	return true;
}

bool MT_Wrangler::ConstructMTBFromFbx()
{
	if (!SetupFbxManager())
	{
		Logger->WriteLine(ELogType::eError, "Failed setup FbxManager!");
		return false;
	}

	if (!SetupImporter())
	{
		Logger->WriteLine(ELogType::eError, "Failed to import Fbx file!");
		return false;
	}

	LoadedBundle = new MT_ObjectBundle();

	FbxNode* RootNode = Scene->GetRootNode();
	std::vector<MT_Object> ObjectsForBundle = {};
	for (int i = 0; i < RootNode->GetChildCount(); i++)
	{
		FbxNode* ChildNode = RootNode->GetChild(i);
		const FbxString& NodeName = ChildNode->GetName();
		FbxNodeAttribute::EType NodeAttribute = GetNodeType(ChildNode);

		Logger->Printf(ELogType::eInfo, "Handling FbxNode:- %s", NodeName);

		if (Fbx_Utilities::FindInString(NodeName, "[MESH]"))
		{
			Logger->Printf(ELogType::eInfo, "Detected as MESH");

			MT_Object* NewObject = ConstructMesh(ChildNode);
			if (NewObject)
			{
				ObjectsForBundle.push_back(*NewObject);
			}
		}
		else if (Fbx_Utilities::FindInString(NodeName, "[MODEL]"))
		{
			Logger->Printf(ELogType::eInfo, "Detected as RIGGED MODEL");
			// Convert to RIGGED Model.
		}	
		else 
		{
			Logger->Printf(ELogType::eInfo, "Did not detect any special types, exporting as base MT_Object.");

			// Attempt conversion to other Frame Type
			MT_Object* NewObject = ConstructBaseObject(ChildNode);
			if (NewObject)
			{
				ObjectsForBundle.push_back(*NewObject);
			}
		}

		Logger->Printf(ELogType::eInfo, "Finished FbxNode:- %s", NodeName);
	}

	LoadedBundle->SetObjects(ObjectsForBundle);

	return true;
}

bool MT_Wrangler::SaveBundleToFile()
{
	if (LoadedBundle)
	{
		Logger->Printf(ELogType::eInfo, "Saving MTB file:- %s", MTOName);
		MT_ObjectHandler::WriteBundleToFile(MTOName, *LoadedBundle);
		return true;
	}

	return false;
}

const FbxNodeAttribute::EType MT_Wrangler::GetNodeType(FbxNode* Node) const
{
	const FbxNodeAttribute* NodeAttribute = Node->GetNodeAttribute();
	if (NodeAttribute)
	{
		return NodeAttribute->GetAttributeType();
	}

	return FbxNodeAttribute::EType::eUnknown;
}

MT_Object* MT_Wrangler::ConstructBaseObject(FbxNode* Node)
{
	// Format name to respectful name
	FbxString NodeName = Node->GetName();
	std::string RawName = NodeName.Buffer();
	MT_ObjectUtils::RemoveMetaTagFromName(RawName);
	Logger->Printf(ELogType::eInfo, "Setup MT_Object called:- %s", RawName.data());

	// Construct object and set name
	MT_Object* ModelObject = new MT_Object();
	ModelObject->SetName(RawName);
	ModelObject->SetObjectFlags(MT_ObjectFlags::HasChildren);
	ModelObject->SetType(MT_ObjectUtils::GetTypeFromString(Node->GetName()));

	// Get Objects transform
	TransformStruct TransformObject = {};
	const FbxDouble3& Position = Node->LclTranslation;
	const FbxDouble3& Rotation = Node->LclRotation;
	const FbxDouble3& Scale = Node->LclScaling;
	TransformObject.Position = { (float)Position[0], (float)Position[1], (float)Position[2] };
	TransformObject.Rotation = { (float)Rotation[0], (float)Rotation[1], (float)Rotation[2] };
	TransformObject.Scale = { (float)Scale[0], (float)Scale[1], (float)Scale[2] };
	ModelObject->SetTransform(TransformObject);

	// Collision Conversion
	FbxNode* CollisionNode = Node->FindChild("COL");
	if (CollisionNode)
	{
		// checks are done in SetCollisions, flag is added too.
		Logger->WriteLine(ELogType::eInfo, "Detected STATIC collision, converting to MT_Collisions");
		ModelObject->SetCollisions(ConstructCollision(CollisionNode));
	}

	// Setup Children
	std::vector<MT_Object> Children = {};
	FbxInt32 NumChildren = Node->GetChildCount();
	Logger->Printf(ELogType::eInfo, "Detected %i children nodes.", NumChildren);
	for (int i = 0; i < NumChildren; i++)
	{
		MT_Object* NewChildObject;

		FbxNode* ChildNode = Node->GetChild(i);
		const FbxString& ChildName = ChildNode->GetName();
		if (Fbx_Utilities::FindInString(ChildName, "MESH"))
		{
			Logger->Printf(ELogType::eInfo, "CHILD: Constructing MESH:- %s", ChildName.Buffer());
			NewChildObject = ConstructMesh(ChildNode);
			Children.push_back(*NewChildObject);
		}
		else if(ChildName.Find('[') != std::string::npos && ChildName.Find(']') != std::string::npos)
		{
			Logger->Printf(ELogType::eInfo, "CHILD: Constructing MT_Object:- %s", ChildName.Buffer());
			NewChildObject = ConstructBaseObject(ChildNode);
			Children.push_back(*NewChildObject);
		}		
	}

	Logger->WriteLine(ELogType::eInfo, "Done handling children nodes.");

	// Update ModelObject's array
	ModelObject->SetChildren(Children);

	return ModelObject;
}

MT_Object* MT_Wrangler::ConstructMesh(FbxNode* Node)
{
	// Construct base object
	MT_Object* ModelObject = ConstructBaseObject(Node);

	// Model Conversion
	std::vector<MT_Lod> Lods = {};
	FbxInt32 NumLods = Node->GetChildCount();
	for (int i = 0; i < NumLods; i++)
	{
		FbxNode* LodNode = Node->GetChild(i);
		FbxString NodeName = LodNode->GetName();
		if (NodeName.Find("LOD") != -1)
		{
			Logger->Printf(ELogType::eInfo, "Constructing LOD index: %i", i);
			Lods.push_back(*ConstructFromLod(LodNode));
		}
	}

	ModelObject->SetLods(Lods);

	// Skeleton Conversion
	FbxNode* SkeletonNode = Node->FindChild("Root");
	if (SkeletonNode)
	{
		MT_Skeleton* SkeletonObject = ConstructSkeleton(SkeletonNode);
		ModelObject->SetSkeleton(SkeletonObject);
	}

	return ModelObject;
}

MT_Collision* MT_Wrangler::ConstructCollision(FbxNode* Node)
{
	FbxMesh* Mesh = Node->GetMesh();
	MT_Collision* Collision = new MT_Collision();

	std::vector<Point3> Vertices = {};
	Vertices.resize(Mesh->GetControlPointsCount());

	// Transfer vertices into Collision
	for (size_t i = 0; i < Vertices.size(); i++)
	{
		const FbxVector4& ControlPoint = Mesh->GetControlPointAt(i);
		Point3 NewVertex = { (float)(ControlPoint[0]), (float)(ControlPoint[1]), (float)(ControlPoint[2]) };
		Vertices[i] = NewVertex;
	}

	Collision->SetVertices(Vertices);

	// Transfer indices and facegroups into Collision
	std::vector<Int3> Indices = {};
	std::vector<MT_FaceGroup> FaceGroups = {};
	ConstructIndicesAndFaceGroupsFromNode(Node, &Indices, &FaceGroups);

	// Get Material name and texture from FbxSurfaceMaterials
	for (int i = 0; i < Node->GetMaterialCount(); i++)
	{
		MT_FaceGroup& FaceGroup = FaceGroups[i];
		FbxSurfaceMaterial* Material = Node->GetMaterial(i);
		FBX_ASSERT(Material);

		MT_MaterialInstance* NewInstance = new MT_MaterialInstance();
		NewInstance->SetName(Material->GetName());
		NewInstance->SetMaterialFlags(MT_MaterialInstanceFlags::IsCollision);
		FaceGroup.SetMatInstance(NewInstance);
	}

	Collision->SetIndices(Indices);
	Collision->SetFaceGroups(FaceGroups);

	return Collision;
}

MT_Lod* MT_Wrangler::ConstructFromLod(FbxNode* Lod)
{
	MT_Lod* LodObject = new MT_Lod();
	LodObject->ResetVertexFlags();

	FbxMesh* Mesh = Lod->GetMesh();

	FbxGeometryElementUV* DiffuseUVElement = GetUVElementByIndex(Mesh, 0);
	FbxGeometryElementUV* UV0Element = GetUVElementByIndex(Mesh, 1);
	FbxGeometryElementUV* UV1Element = GetUVElementByIndex(Mesh, 2);
	FbxGeometryElementUV* OMUVElement = GetUVElementByIndex(Mesh, 3);
	FbxGeometryElementNormal* NormalElement = Mesh->GetElementNormal(0);
	FbxGeometryElementTangent* TangentElement = Mesh->GetElementTangent(0);

	// Construct Vertex Array
	FbxInt32 NumVertices = Mesh->GetControlPointsCount();
	if (NumVertices == 0)
	{
		Logger->WriteLine(ELogType::eWarning, "This mesh has no vertices");

	}
	std::vector<Vertex> Vertices = {};
	Vertices.resize(NumVertices);
	for (int i = 0; i < NumVertices; i++)
	{
		Vertex NewVertex = {};

		FbxVector4 ControlPoint = Mesh->GetControlPointAt(i);

		NewVertex.position = { (float)ControlPoint[0], (float)ControlPoint[1], (float)ControlPoint[2] };

		// Add Normal element to Vertex
		if (NormalElement)
		{
			const FbxVector4& Value = NormalElement->GetDirectArray().GetAt(i);
			NewVertex.normals = { (float)Value[0], (float)Value[1], (float)Value[2] };
			LodObject->AddVertexFlag(Normals);
		}

		// Add Tangent element to Vertex
		if (TangentElement)
		{
			const FbxVector4& Value = TangentElement->GetDirectArray().GetAt(i);
			NewVertex.tangent = { (float)Value[0], (float)Value[1], (float)Value[2] };
			LodObject->AddVertexFlag(Tangent);
		}

		// Add Diffuse element to Vertex
		if (DiffuseUVElement)
		{
			const FbxVector2& Value = DiffuseUVElement->GetDirectArray().GetAt(i);
			NewVertex.uv0 = { (float)Value[0], (float)Value[1] };
			LodObject->AddVertexFlag(TexCoords0);
		}

		// Add UV0 element to Vertex
		if (UV0Element)
		{
			const FbxVector2& Value = UV0Element->GetDirectArray().GetAt(i);
			NewVertex.uv1 = { (float)Value[0], (float)Value[1] };
			LodObject->AddVertexFlag(TexCoords1);
		}

		// Add UV1 element to Vertex
		if (UV1Element)
		{
			const FbxVector2& Value = UV1Element->GetDirectArray().GetAt(i);
			NewVertex.uv2 = { (float)Value[0], (float)Value[1] };
			LodObject->AddVertexFlag(TexCoords2);
		}

		// Add OMUV element to Vertex
		if (OMUVElement)
		{
			const FbxVector2& Value = OMUVElement->GetDirectArray().GetAt(i);
			NewVertex.uv3 = { (float)Value[0], (float)Value[1] };
			LodObject->AddVertexFlag(ShadowTexture);
		}

		Vertices[i] = NewVertex;
	}

	LodObject->AddVertexFlag(Position);
	LodObject->SetVertices(Vertices);

	std::vector<Int3> Indices = {};
	std::vector<MT_FaceGroup> FaceGroups = {};
	ConstructIndicesAndFaceGroupsFromNode(Lod, &Indices, &FaceGroups);

	// Finish by setting indices
	LodObject->SetIndices(Indices);

	// Finally, set face groups.
	LodObject->SetFaceGroups(FaceGroups);

	return LodObject;
}

MT_Skeleton* MT_Wrangler::ConstructSkeleton(FbxNode* Node)
{
	MT_Skeleton* SkeletonObject = new MT_Skeleton();

	SkeletonState.JointLookupTable = {};
	SkeletonState.Joints = {};

	ConstructJoint(Node);

	for (size_t i = 0; i < SkeletonState.Joints.size(); i++)
	{
		MT_Joint& JointObject = SkeletonState.Joints[i];

		// Sort out Parent Index
		const std::string& Name = JointObject.GetParentName();
		const bool bExists = SkeletonState.JointLookupTable.find(Name) != SkeletonState.JointLookupTable.end();
		if (bExists)
		{
			int NodeIdx = SkeletonState.JointLookupTable[Name];
			JointObject.SetParentJointIndex(NodeIdx);
		}
		else
		{
			JointObject.SetParentJointIndex(0xFF);
		}
	}

	SkeletonObject->SetJoints(SkeletonState.Joints);

	return SkeletonObject;
}

MT_Joint* MT_Wrangler::ConstructJoint(FbxNode* Node)
{
	// Check FbxNodeAttribute
	if (FbxNodeAttribute* const NodeAttribute = Node->GetNodeAttribute())
	{
		if (NodeAttribute->GetAttributeType() != FbxNodeAttribute::eSkeleton)
		{
			return nullptr;
		}
	}

	// Set Name and Usage.
	MT_Joint* JointObject = new MT_Joint();
	JointObject->SetName(Node->GetName());
	JointObject->SetParentName(Node->GetParent()->GetName());
	JointObject->SetUsage(MT_JointUsage::LOD0);

	// Get Node Matrix -> Set Joint Transform
	JointMatrix Matrix = {};
	FbxAMatrix& lLocalTransform = Node->EvaluateLocalTransform();
	const FbxDouble3 Position = Node->LclTranslation.Get();
	Matrix.Position = { (float)Position[0], (float)Position[1], (float)Position[2] };
	const FbxDouble3 Scale = Node->LclScaling.Get();
	Matrix.Scale = { (float)Scale[0], (float)Scale[1], (float)Scale[2] };
	const FbxDouble4 Rotation = lLocalTransform.GetR();
	Matrix.Rotation = { (float)Rotation[0], (float)Rotation[1], (float)Rotation[2], (float)Rotation[3] };
	JointObject->SetTransform(Matrix);

	// Add to the state
	SkeletonState.JointLookupTable.insert(std::pair<std::string, int>(Node->GetName(), (int)SkeletonState.JointLookupTable.size()));
	SkeletonState.Joints.push_back(*JointObject);

	for (int i = 0; i < Node->GetChildCount(); i++)
	{
		FbxNode* const ChildNode = Node->GetChild(i);
		MT_Joint* const ChildJoint = ConstructJoint(ChildNode);
	}

	// return
	return JointObject;
}

void MT_Wrangler::ConstructIndicesAndFaceGroupsFromNode(FbxNode* TargetNode, std::vector<Int3>* Indices, std::vector<MT_FaceGroup>* FaceGroups)
{
	FbxMesh* Mesh = TargetNode->GetMesh();

	// Get DiffuseMaterial, we'll need to use it to create Indices & FaceGroup
	FbxGeometryElementMaterial* DiffuseMaterial = Mesh->GetElementMaterial(0);
	FBX_ASSERT(DiffuseMaterial);
	int MaterialCount = DiffuseMaterial ? TargetNode->GetMaterialCount() : 1;

	// Construct Indices & FaceGroups Array
	std::vector<std::vector<Int3>> FaceGroupLookup = {};
	FaceGroupLookup.resize(MaterialCount);
	for (int i = 0; i < Mesh->GetPolygonCount(); i++)
	{
		Int3 Triangle = {};
		Triangle.i1 = Mesh->GetPolygonVertex(i, 0);
		Triangle.i2 = Mesh->GetPolygonVertex(i, 1);
		Triangle.i3 = Mesh->GetPolygonVertex(i, 2);

		int MaterialAssignment = DiffuseMaterial ? DiffuseMaterial->GetIndexArray().GetAt(i) : 0;
		FaceGroupLookup[MaterialAssignment].push_back(Triangle);
	}

	// Reorder Indices by FaceGroups then push into LodObject
	FbxUInt32 CurrentTotal = 0;
	for (int i = 0; i < FaceGroupLookup.size(); i++)
	{
		const std::vector<Int3> FaceGroupIndices = FaceGroupLookup[i];
		if (FaceGroupIndices.size() == 0)
		{
			Logger->WriteLine(ELogType::eWarning, "A FaceGroup for this mesh has no indices/triangles/polygons");
		}

		// push into main indices array
		Indices->insert(Indices->end(), FaceGroupLookup[i].begin(), FaceGroupLookup[i].end());

		// setup FaceGroups
		// TODO: Improve validity of Fbx Material
		size_t NumFaces = FaceGroupLookup[i].size();
		if (NumFaces > 0)
		{
			// Construct the new FaceGroup
			MT_FaceGroup NewFaceGroup = {};
			NewFaceGroup.SetNumFaces(NumFaces);
			NewFaceGroup.SetStartIndex(CurrentTotal);

			// add MaterialInstance to our FaceGroup
			int MaterialCount = TargetNode->GetMaterialCount();
			FbxSurfaceMaterial* Material = TargetNode->GetMaterial(i);
			FBX_ASSERT(Material);

			const char* ExpectedName = Material ? Material->GetName() : "UNKNOWN";

			// Create a new MT_Material instance
			MT_MaterialInstance* NewInstance = new MT_MaterialInstance();
			NewInstance->SetName(ExpectedName);

			if (Material)
			{
				// Setup rest of MT_MaterialInstance
				const char* DiffuseTexture = Material->sDiffuse;
				NewInstance->SetTextureName(DiffuseTexture);
			}

			NewInstance->SetMaterialFlags(MT_MaterialInstanceFlags::HasDiffuse);
			NewFaceGroup.SetMatInstance(NewInstance);

			// Update CurrentTotal, then insert into array
			CurrentTotal += NumFaces * 3;
			FaceGroups->push_back(NewFaceGroup);

			// log Name
			Logger->Printf(ELogType::eInfo, "Setup material called:- %s", Material->GetName());
		}
	}

	//bool bDidGenerate = Mesh->GenerateNormals(true, true);
	//int i = 0;
}

FbxGeometryElementUV* MT_Wrangler::GetUVElementByIndex(FbxMesh* Mesh, uint ElementType) const
{
	std::map<uint, std::string>& LookupMap = WranglerUtils::UVLookupMap;
	std::string& ElementName = LookupMap[ElementType];
	FbxGeometryElementUV* Element = nullptr;

	if (!ElementName.empty())
	{
		Element = Mesh->GetElementUV(ElementName.data());
		if (Element)
		{
			Logger->Printf(ELogType::eWarning, "Found Vertex Element:- %s", ElementName.data());
			return Element;
		}
		else
		{
			Logger->Printf(ELogType::eWarning, "Did not find Vertex Element:- %s", ElementName.data());
		}
	}

	return nullptr;
}
