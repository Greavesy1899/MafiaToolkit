#include "3dsmaxsdk_preinclude.h"
#include "EDMExportClass.h"
#include "Mafia2ModifierClasses.h"
#include "M2EDM.h"
#include "MeshNormalSpec.h"
#include "dummy.h"
#include "triobj.h"
#include "gutil.h"
#include "modstack.h"

#define EDM_EXPORT_CLASS_ID	Class_ID(0x14fe2fdc, 0x102f11a2)

class EDMExportClassDesc : public ClassDesc2
{
public:
	virtual int IsPublic() { return TRUE; }
	virtual void* Create(BOOL /*loading = FALSE*/) { return new EDMExport(); }
	virtual const TCHAR *	ClassName() { return GetString(IDS_EDM_EXPORT_CLASS); }
	virtual SClass_ID SuperClassID() { return SCENE_EXPORT_CLASS_ID; }
	virtual Class_ID ClassID() { return EDM_EXPORT_CLASS_ID; }
	virtual const TCHAR* Category() { return GetString(IDS_CATEGORY_EXPORT); }

	virtual const TCHAR* InternalName() { return _T("M2Plugin"); }	// returns fixed parsable name (scripter-visible name)
	virtual HINSTANCE HInstance() { return hInstance; }					// returns owning module handle


};
ClassDesc2* GetEDMExportDesc() {
	static EDMExportClassDesc EDMExportDesc;
	return &EDMExportDesc;
}

static FILE *stream = NULL;

//EDM EXPORT SECTION
//=========================

EDMExport::EDMExport() {
}
EDMExport::~EDMExport() {
}
int EDMExport::ExtCount() {
	return 1;
}
const TCHAR* EDMExport::Ext(int n) {
	switch (n) {
	case 0:
		return _T("M2T");
	}
	return _T("");
}
const TCHAR* EDMExport::LongDesc() {
	return GetString(IDS_EDM_L_DESC);
}
const TCHAR* EDMExport::ShortDesc() {
	return GetString(IDS_EDM_S_DESC);
}
const TCHAR* EDMExport::AuthorName() {
	return GetString(IDS_AUTHOR);
}
const TCHAR* EDMExport::CopyrightMessage() {
	return _T("");
}
const TCHAR* EDMExport::OtherMessage1() {
	return _T("");
}
const TCHAR* EDMExport::OtherMessage2() {
	return _T("");
}
unsigned int EDMExport::Version() {
	return 1;
}
void EDMExport::ShowAbout(HWND hWnd) {}
int EDMExport::DoExport(const MCHAR *name, ExpInterface *ei, Interface *i, BOOL suppressPrompts, DWORD options)
{
	//Check if nodes are selected.
	if (i->GetSelNodeCount() < 1) {
		MessageBox(NULL, _T("Select the root node of a mesh."), _T("Error!"), MB_OK);
		return FALSE;
	}

	//define file to write to.
	EDMExportWorkFile theFile(name, _T("wb"));
	FILE *stream = theFile.Stream();

	INode* parentNode = i->GetSelNode(0);

	//check if dummy.
	if (parentNode->GetObjOrWSMRef()->ClassID() == dummyClassID) {
		MessageBox(NULL, _T("Select a dummy node containing a mesh"), _T("Error!"), MB_OK);
		return FALSE;
	}
	IDerivedObject *DerivedObjectPtr = (IDerivedObject *)(parentNode->GetObjOrWSMRef());

	int ModStackIndex = 0;
	DerivedObjectPtr->NumModifiers();
	M2Modifier* modifier = nullptr;
	while (ModStackIndex < DerivedObjectPtr->NumModifiers()) {
		// Get current modifier.
		Modifier* ModifierPtr = DerivedObjectPtr->GetModifier(ModStackIndex);

		// Is this the mafia modifier?
		if (ModifierPtr->ClassID() == M2_MODIFIER_CLASS_ID) {
			modifier = (M2Modifier*)ModifierPtr;
		}
		ModStackIndex++;
	}
	if (modifier == nullptr) {
		MessageBox(NULL, _T("Could not find the Mafia 2 Modifier on the root dummy."), _T("Error!"), MB_OK);
		return FALSE;
	}

	BOOL hasNormals;
	BOOL hasTangents;
	BOOL hasUVs;

	modifier->GetParamBlock(0)->GetValue(1, 0, hasNormals, FOREVER);
	modifier->GetParamBlock(0)->GetValue(2, 0, hasTangents, FOREVER);
	modifier->GetParamBlock(0)->GetValue(3, 0, hasUVs, FOREVER);

	////build file structure
	EDMStructure fileStructure = EDMStructure();
	fileStructure.SetName(parentNode->GetName());
	fileStructure.SetPartSize(parentNode->NumberOfChildren());

	std::vector<EDMPart> parts = std::vector<EDMPart>(fileStructure.GetPartSize());

	for (int i = 0; i != parts.size(); i++)
	{
		parts[i] = EDMPart();

		//Get child nodes (1 for now)
		INode* child = parentNode->GetChildNode(i);

		//get TriObject for mesh
		TriObject* object = static_cast<TriObject*>(child->GetObjOrWSMRef());
		Mesh &mesh = object->mesh;
		
		//get verts and normals from mesh and save.
		parts[i].SetVertSize(mesh.numVerts);
		parts[i].SetIndicesSize(mesh.numFaces);
		//invert from BOOL;
		parts[i].SetHasPositions(1);
		parts[i].SetHasNormals(hasNormals);
		parts[i].SetHasTangents(hasTangents);
		parts[i].SetHasBlendData(0);
		parts[i].SetHasFlag0x80(0);
		parts[i].SetHasUV0(hasUVs);
		parts[i].SetHasUV1(0);
		parts[i].SetHasUV2(0);
		parts[i].SetHasUV7(0);
		parts[i].SetHasUV1(0);
		parts[i].SetHasUV2(0);
		parts[i].SetHasFlag0x20000(0);
		parts[i].SetHasFlag0x40000(0);
		parts[i].SetHasDamage(0);

		//init vectors
		std::vector<Point3> verts = std::vector<Point3>(mesh.numVerts);
		std::vector<Point3> normals = std::vector<Point3>(mesh.numVerts);
		std::vector<Point3> tangents = std::vector<Point3>(mesh.numVerts);
		std::vector<Point3> uvs = std::vector<Point3>(mesh.numVerts);

		MeshNormalSpec* normalSpec = mesh.GetSpecifiedNormals();
		MeshMap &map = mesh.Map(1);

		for (int c = 0; c != mesh.numVerts; c++) {
			if (parts[i].GetHasPositions())
				verts[c] = mesh.getVert(c);

			if (parts[i].GetHasNormals())
				normals[c] = normalSpec->Normal(c);

			if(parts[i].GetHasTangents())
				tangents[c] = ComputeTangent(&map.tv[c], &verts[c]);

			if (parts[i].GetHasUV0())
				uvs[c] = map.tv[c];
		}

		MultiMtl* mtl = static_cast<MultiMtl*>(child->GetMtl());
		parts[i].SetSubMeshCount(mtl->NumSubMtls());
		std::vector<std::wstring> matNames = std::vector <std::wstring>(parts[i].GetSubMeshCount());

		for (int c = 0; c != matNames.size(); c++) {
			Mtl* subtex = mtl->GetSubMtl(c);
			matNames[c] = subtex->GetName();
		}

		std::vector<Int3> indices = std::vector<Int3>(mesh.numFaces);
		std::vector<byte> matids = std::vector<byte>(mesh.numFaces);

		for (int c = 0; c != indices.size(); c++)
		{
			Int3 ind;
			ind.i1 = mesh.faces[c].v[0];
			ind.i2 = mesh.faces[c].v[1];
			ind.i3 = mesh.faces[c].v[2];
			indices[c] = ind;
			matids[c] = (byte)mesh.faces[c].getMatID();
		}

		parts[i].SetVertices(verts);
		parts[i].SetNormals(normals);
		parts[i].SetTangents(tangents);
		parts[i].SetUVs(uvs);
		parts[i].SetMatNames(matNames);
		parts[i].SetIndices(indices);
		parts[i].SetMatIDs(matids);
	}
	fileStructure.SetParts(parts);
	fileStructure.WriteToStream(stream);

	return TRUE;
}

