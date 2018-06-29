//**************************************************************************/
// Copyright (c) 1998-2007 Autodesk, Inc.
// All rights reserved.
// 
// These coded instructions, statements, and computer programs contain
// unpublished proprietary information written by Autodesk, Inc., and are
// protected by Federal copyright law. They may not be disclosed to third
// parties or copied or duplicated in any form, in whole or in part, without
// the prior written consent of Autodesk, Inc.
//**************************************************************************/
// DESCRIPTION: Appwizard generated plugin
// AUTHOR: 
//***************************************************************************/

#include "M2Plugin.h"
#include "M2Helpers.h"
#include "triobj.h"
#include <impapi.h>

#define M2Plugin_CLASS_ID	Class_ID(0xac9aa34b, 0xbb4578d1)

class M2PluginClassDesc : public ClassDesc2 
{
public:
	virtual int IsPublic() 							{ return TRUE; }
	virtual void* Create(BOOL /*loading = FALSE*/) 		{ return new EDMImport(); }
	virtual const TCHAR *	ClassName() 			{ return GetString(IDS_CLASS_NAME); }
	virtual SClass_ID SuperClassID() 				{ return SCENE_IMPORT_CLASS_ID; }
	virtual Class_ID ClassID() 						{ return M2Plugin_CLASS_ID; }
	virtual const TCHAR* Category() 				{ return GetString(IDS_CATEGORY); }

	virtual const TCHAR* InternalName() 			{ return _T("M2Plugin"); }	// returns fixed parsable name (scripter-visible name)
	virtual HINSTANCE HInstance() 					{ return hInstance; }					// returns owning module handle
	

};


ClassDesc2* GetM2PluginDesc() { 
	static M2PluginClassDesc M2PluginDesc;
	return &M2PluginDesc; 
}

INT_PTR CALLBACK M2PluginOptionsDlgProc(HWND hWnd,UINT message,WPARAM ,LPARAM lParam) {
	static EDMImport* imp = nullptr;

	switch(message) {
		case WM_INITDIALOG:
			imp = (EDMImport *)lParam;
			CenterWindow(hWnd,GetParent(hWnd));
			return TRUE;

		case WM_CLOSE:
			EndDialog(hWnd, 0);
			return 1;
	}
	return 0;
}

static FILE *stream = NULL;

//EDM STRUCTURES
//=========================

EDMStructure::EDMStructure() {}
EDMStructure::~EDMStructure() {}
EDMPart::EDMPart() {}
EDMPart::~EDMPart() {}

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
	return GetString(IDS_EDM_AUTHOR);
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

	EDMStructure file = EDMStructure();
	file.name = ReadString(stream);
	fread(&file.partSize, sizeof(int), 1, stream);

	std::vector<EDMPart> parts = std::vector<EDMPart>(file.partSize);

	for (int i = 0; i != parts.size(); i++) {
		parts[i].name = ReadString(stream);
		fread(&parts[i].vertSize, sizeof(int), 1, stream);
		parts[i].vertices = std::vector<Point3>(parts[i].vertSize);
		for (int c = 0; c != parts[i].vertSize; c++) {
			fread(&parts[i].vertices[c].x, sizeof(float), 1, stream);
			fread(&parts[i].vertices[c].y, sizeof(float), 1, stream);
			fread(&parts[i].vertices[c].z, sizeof(float), 1, stream);
		}
		fread(&parts[i].uvSize, sizeof(int), 1, stream);
		parts[i].uvs = std::vector<UVVert>(parts[i].uvSize);
		for (int c = 0; c != parts[i].uvSize; c++) {
			fread(&parts[i].uvs[c].x, sizeof(float), 1, stream);
			fread(&parts[i].uvs[c].y, sizeof(float), 1, stream);
		}
		fread(&parts[i].indicesSize, sizeof(int), 1, stream);
		parts[i].indices = std::vector<Int3>(parts[i].indicesSize);
		for (int c = 0; c != parts[i].indicesSize; c++) {
			fread(&parts[i].indices[c].s1, sizeof(int), 1, stream);
			fread(&parts[i].indices[c].s2, sizeof(int), 1, stream);
			fread(&parts[i].indices[c].s3, sizeof(int), 1, stream);

			Int3 &indices = parts[i].indices[c];
			indices.s1 -= 1;
			indices.s2 -= 1;
			indices.s3 -= 1;
		}
	}

	file.parts = parts;

	DebugPrint(_T("Starting mesh %s\n"), file.name);

	TriObject* triObject = CreateNewTriObject();
	Mesh &mesh = triObject->GetMesh();

	mesh.setNumVerts(file.parts[0].vertSize);
	mesh.setNumFaces(file.parts[0].indicesSize);
	mesh.setNumTVerts(file.parts[0].uvSize);

	for (int i = 0; i != mesh.numVerts; i++) {
		mesh.setVert(i, file.parts[0].vertices[i]);
	}
	for (int i = 0; i != mesh.numFaces; i++) {
		mesh.faces[i].setVerts(file.parts[0].indices[i].s1, file.parts[0].indices[i].s2, file.parts[0].indices[i].s3);
		mesh.faces[i].setEdgeVisFlags(1, 1, 1);
		mesh.faces[i].setMatID(1);
	}
	for (int i = 0; i != mesh.numTVerts; i++) {
		mesh.setTVert(i, file.parts[0].uvs[i]);
	}

	mesh.InvalidateGeomCache();
	mesh.InvalidateTopologyCache();

	ImpNode* node = importerInt->CreateNode();

	if (!node) {
		delete triObject;
		return FALSE;
	}
	node->Reference(triObject);
	importerInt->AddNodeToScene(node);
	importerInt->RedrawViews();
	TimeValue t(0);
	Matrix3 tm(1);


	//if(!suppressPrompts)
	//	DialogBoxParam(hInstance, 
	//			MAKEINTRESOURCE(IDD_PANEL), 
	//			GetActiveWindow(), 
	//			M2PluginOptionsDlgProc, (LPARAM)this);

	return TRUE;
}
	
