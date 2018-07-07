#include "3dsmaxsdk_preinclude.h"
#include "EDMExportClass.h"
#include "M2EDM.h"
#include "MeshNormalSpec.h"

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
		return _T("EDM");
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
	

	EDMExportWorkFile theFile(name, _T("wb"));
	FILE *stream = theFile.Stream();
	return TRUE;
}

