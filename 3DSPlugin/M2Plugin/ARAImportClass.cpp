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
#include "ARAImportClass.h"
#include "triobj.h"
#include <impapi.h>
#include <dummy.h>


#define ARA_IMPORT_CLASS_ID	Class_ID(0xa8135ba, 0x70746c0b)


class ARAImportClassDesc : public ClassDesc2
{
public:
	virtual int IsPublic() { return TRUE; }
	virtual void* Create(BOOL /*loading = FALSE*/) { return new ARAImport(); }
	virtual const TCHAR *	ClassName() { return GetString(IDS_ARA_IMPORT_CLASS); }
	virtual SClass_ID SuperClassID() { return SCENE_IMPORT_CLASS_ID; }
	virtual Class_ID ClassID() { return ARA_IMPORT_CLASS_ID; }
	virtual const TCHAR* Category() { return GetString(IDS_CATEGORY); }

	virtual const TCHAR* InternalName() { return _T("ARAImporter"); }	// returns fixed parsable name (scripter-visible name)
	virtual HINSTANCE HInstance() { return hInstance; }					// returns owning module handle
};

ClassDesc2* GetARAImportDesc() {
	static ARAImportClassDesc ARAImportDesc;
	return &ARAImportDesc;
}

ARAImport::ARAImport() {
}
ARAImport::~ARAImport() {
}
int ARAImport::ExtCount() {
	return 1;
}
const TCHAR* ARAImport::Ext(int n) {
	switch (n) {
	case 0:
		return _T("ARA");
	}
	return _T("");
}
const TCHAR* ARAImport::LongDesc() {
	return GetString(IDS_ARA_L_DESC);
}
const TCHAR* ARAImport::ShortDesc() {
	return GetString(IDS_ARA_S_DESC);
}
const TCHAR* ARAImport::AuthorName() {
	return GetString(IDS_AUTHOR);
}
const TCHAR* ARAImport::CopyrightMessage() {
	return _T("");
}
const TCHAR* ARAImport::OtherMessage1() {
	return _T("");
}
const TCHAR* ARAImport::OtherMessage2() {
	return _T("");
}
unsigned int ARAImport::Version() {
	return 1;
}
void ARAImport::ShowAbout(HWND hWnd) {}
int ARAImport::DoImport(const TCHAR* filename, ImpInterface* importerInt, Interface* ip, BOOL suppressPrompts) { 
	
	TriObject* object = CreateNewTriObject();
	Mesh &mesh = object->GetMesh();

	mesh.setNumVerts(8);
	mesh.setNumFaces(12);

	mesh.setVert(0, Point3(-617.4632f, 104.8727f, -39.01695f));
	mesh.setVert(1, Point3(650.7374f, 104.8727f, -39.01695f));
	mesh.setVert(2, Point3(-617.4632f, 254.7083f, -39.01695f));
	mesh.setVert(3, Point3(650.7374f, 254.7083f, -39.01695f));
	mesh.setVert(4, Point3(-617.4632f, 104.8727f, 39.01695f));
	mesh.setVert(5, Point3(650.7374f, 104.8727f, 39.01695f));
	mesh.setVert(6, Point3(-617.4632f, 254.7083f, 39.01695f));
	mesh.setVert(7, Point3(650.7374f, 254.7083f, 39.01695f));

	mesh.faces[0].setVerts(0, 2, 3);
	mesh.faces[1].setVerts(3, 1, 0);
	mesh.faces[2].setVerts(4, 5, 7);
	mesh.faces[3].setVerts(7, 6, 4);
	mesh.faces[4].setVerts(0, 1, 5);
	mesh.faces[5].setVerts(5, 4, 0);
	mesh.faces[6].setVerts(1, 3, 7);
	mesh.faces[7].setVerts(7, 5, 1);
	mesh.faces[8].setVerts(3, 2, 6);
	mesh.faces[9].setVerts(6, 7, 3);
	mesh.faces[10].setVerts(2, 0, 4);
	mesh.faces[11].setVerts(4, 6, 2);
	mesh.InvalidateGeomCache();
	mesh.InvalidateTopologyCache();

	ImpNode* node = importerInt->CreateNode();
	Matrix3 matrix = Matrix3();
	matrix.IdentityMatrix();
	matrix.SetTrans(Point3(635.7573f, -969.9177f, 13.732f));
	node->SetTransform(0, matrix);
	node->Reference(object);
	node->SetName(_T("Part"));
	node->GetINode()->WorldAlignPivot(0, TRUE);
	node->GetINode()->CenterPivot(0, TRUE);
	node->GetINode()->SetObjectRef(object);
	importerInt->AddNodeToScene(node);
	importerInt->RedrawViews();

	return TRUE; 
}