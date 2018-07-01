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
int ARAImport::DoImport(const TCHAR* filename, ImpInterface* importerInt, Interface* ip, BOOL suppressPrompts) { return FALSE; }