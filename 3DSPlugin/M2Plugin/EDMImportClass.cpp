#include "3dsmaxsdk_preinclude.h"
#include "EDMImportClass.h"
#include "M2EDM.h"

#define EDM_IMPORT_CLASS_ID	Class_ID(0xac9aa34b, 0xbb4578d1)

class EDMImportClassDesc : public ClassDesc2
{
public:
	virtual int IsPublic() { return TRUE; }
	virtual void* Create(BOOL /*loading = FALSE*/) { return new EDMImport(); }
	virtual const TCHAR *	ClassName() { return GetString(IDS_EDM_IMPORT_CLASS); }
	virtual SClass_ID SuperClassID() { return SCENE_IMPORT_CLASS_ID; }
	virtual Class_ID ClassID() { return EDM_IMPORT_CLASS_ID; }
	virtual const TCHAR* Category() { return GetString(IDS_CATEGORY); }

	virtual const TCHAR* InternalName() { return _T("M2Plugin"); }	// returns fixed parsable name (scripter-visible name)
	virtual HINSTANCE HInstance() { return hInstance; }					// returns owning module handle


};
ClassDesc2* GetEDMImportDesc() {
	static EDMImportClassDesc EDMImportDesc;
	return &EDMImportDesc;
}


INT_PTR CALLBACK M2PluginOptionsDlgProc(HWND hWnd, UINT message, WPARAM, LPARAM lParam) {
	static EDMImport* imp = nullptr;

	switch (message) {
	case WM_INITDIALOG:
		imp = (EDMImport *)lParam;
		CenterWindow(hWnd, GetParent(hWnd));
		return TRUE;

	case WM_CLOSE:
		EndDialog(hWnd, 0);
		return 1;
	}
	return 0;
}
static FILE *stream = NULL;
//EDM IMPORT SECTION
//=========================

EDMImport::EDMImport() {
}
EDMImport::~EDMImport() {
}
int EDMImport::ExtCount() {
	return 1;
}
const TCHAR* EDMImport::Ext(int n) {
	switch (n) {
	case 0:
		return _T("EDM");
	}
	return _T("");
}
const TCHAR* EDMImport::LongDesc() {
	return GetString(IDS_EDM_L_DESC);
}
const TCHAR* EDMImport::ShortDesc() {
	return GetString(IDS_EDM_S_DESC);
}
const TCHAR* EDMImport::AuthorName() {
	return GetString(IDS_AUTHOR);
}
const TCHAR* EDMImport::CopyrightMessage() {
	return _T("");
}
const TCHAR* EDMImport::OtherMessage1() {
	return _T("");
}
const TCHAR* EDMImport::OtherMessage2() {
	return _T("");
}
unsigned int EDMImport::Version() {
	return 1;
}
void EDMImport::ShowAbout(HWND hWnd) {}
int EDMImport::DoImport(const TCHAR* filename, ImpInterface* importerInt, Interface* ip, BOOL suppressPrompts)
{
	EDMWorkClass edm(filename, _T("rb"));
	stream = edm.Stream();

	//begin reading
	EDMStructure file = EDMStructure();
	file.ReadFromStream(stream);
	//set up parts and nodes
	std::vector<EDMPart> parts = file.GetParts();
	DummyObject* parentDummy = new DummyObject();
	INode* parent = ip->CreateObjectNode(parentDummy);
	parent->SetName(file.GetName().c_str());

	//lets goo.
	for (int i = 0; i != parts.size(); i++)
	{
		TriObject* triObject = CreateNewTriObject();
		Mesh &mesh = triObject->GetMesh();

		EDMPart part = parts[i];

		std::vector<Point3> verts = part.GetVertices();
		std::vector<UVVert> uvs = part.GetUVs();
		std::vector<Int3> indices = part.GetIndices();

		mesh.setNumVerts(part.GetVertSize());
		for (int i = 0; i != mesh.numVerts; i++) {
			mesh.setVert(i, verts[i]);
		}

		mesh.setNumMaps(2);
		mesh.setMapSupport(1, true);
		MeshMap &map = mesh.Map(1);
		map.setNumVerts(part.GetUVSize());

		for (int i = 0; i != part.GetUVSize(); i++) {
			map.tv[i].x = uvs[i].x;
			map.tv[i].y = uvs[i].y;
			map.tv[i].z = 0.0f;
		}

		mesh.setNumFaces(part.GetIndicesSize());
		map.setNumFaces(part.GetIndicesSize());

		for (int i = 0; i != mesh.numFaces; i++) {
			mesh.faces[i].setVerts(indices[i].i1, indices[i].i2, indices[i].i3);
			mesh.faces[i].setMatID(1);
			mesh.faces[i].setEdgeVisFlags(1, 1, 1);
			map.tf[i].setTVerts(indices[i].i1, indices[i].i2, indices[i].i3);
		}

		mesh.InvalidateGeomCache();
		mesh.InvalidateTopologyCache();

		INode* nPart = ip->CreateObjectNode(triObject);
		nPart->SetName(part.GetName().c_str());
		parent->AttachChild(nPart, 0);
	}
	importerInt->RedrawViews();


	//if(!suppressPrompts)
	//	DialogBoxParam(hInstance, 
	//			MAKEINTRESOURCE(IDD_PANEL), 
	//			GetActiveWindow(), 
	//			M2PluginOptionsDlgProc, (LPARAM)this);

	return TRUE;
}

