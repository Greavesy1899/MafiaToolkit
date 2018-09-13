#include "3dsmaxsdk_preinclude.h"
#include "EDDImportClass.h"
#include "M2EDD.h"
#include "M2EDM.h"
#include <vector>
#include "triobj.h"
#include <impapi.h>
#include "dummy.h"

#define EDD_IMPORT_CLASS_ID	Class_ID(0x13343918, 0x75cf75f5)
namespace fs = std::experimental::filesystem;

class EDDImportClassDesc : public ClassDesc2
{
public:
	virtual int IsPublic() { return TRUE; }
	virtual void* Create(BOOL /*loading = FALSE*/) { return new EDDImport(); }
	virtual const TCHAR *	ClassName() { return GetString(IDS_EDD_IMPORT_CLASS); }
	virtual SClass_ID SuperClassID() { return SCENE_IMPORT_CLASS_ID; }
	virtual Class_ID ClassID() { return EDD_IMPORT_CLASS_ID; }
	virtual const TCHAR* Category() { return GetString(IDS_CATEGORY); }

	virtual const TCHAR* InternalName() { return _T("M2Plugin"); }	// returns fixed parsable name (scripter-visible name)
	virtual HINSTANCE HInstance() { return hInstance; }					// returns owning module handle


};
ClassDesc2* GetEDDImportDesc() {
	static EDDImportClassDesc EDDImportDesc;
	return &EDDImportDesc;
}

static FILE *stream = NULL;
static FILE *edmStream = NULL;

//EDD IMPORT SECTION
//=========================

EDDImport::EDDImport() {
}
EDDImport::~EDDImport() {
}
int EDDImport::ExtCount() {
	return 1;
}
const TCHAR* EDDImport::Ext(int n) {
	switch (n) {
	case 0:
		return _T("EDD");
	}
	return _T("");
}
const TCHAR* EDDImport::LongDesc() {
	return GetString(IDS_EDD_L_DESC);
}
const TCHAR* EDDImport::ShortDesc() {
	return GetString(IDS_EDD_S_DESC);
}
const TCHAR* EDDImport::AuthorName() {
	return GetString(IDS_AUTHOR);
}
const TCHAR* EDDImport::CopyrightMessage() {
	return _T("");
}
const TCHAR* EDDImport::OtherMessage1() {
	return _T("");
}
const TCHAR* EDDImport::OtherMessage2() {
	return _T("");
}
unsigned int EDDImport::Version() {
	return 1;
}
void EDDImport::ShowAbout(HWND hWnd) {}
int EDDImport::DoImport(const TCHAR* filename, ImpInterface* importerInt, Interface* ip, BOOL suppressPrompts) {
	fs::path filePath = filename;
	fs::path parentPath = filePath.parent_path();

	EDDWorkClass edm(filename, _T("rb"));
	stream = edm.Stream();

	int magic;
	fread(&magic, sizeof(int), 1, stream);

	if (magic != 808535109)
		return FALSE;

	int entryCount = 0;
	fread(&entryCount, sizeof(int), 1, stream);

	for (int i = 0; i != entryCount; i++) {
		try
		{
			EDDEntry entry = EDDEntry();
			entry.ReadFromStream(stream);

			fs::path edmPath = parentPath / entry.GetLodNames()[0];

			if (!fs::exists(edmPath))
				return FALSE;

			const wchar_t* wedmPath = edmPath.c_str();
			edmStream = _tfopen(wedmPath, _T("rb"));

			if (edmStream == NULL)
				return FALSE;

			EDMStructure edmStructure = EDMStructure();
			edmStructure.ReadFromStream(edmStream);

			DummyObject* parentDummy = new DummyObject();
			ImpNode* parent = importerInt->CreateNode();
			parent->Reference(parentDummy);
			parent->SetName(entry.GetLodNames()[0].c_str());

			MtlBaseLib &mlib = ip->GetMaterialLibrary();

			for (int x = 0; x != edmStructure.GetPartSize(); x++) {
				EDMPart part = edmStructure.GetParts()[x];

				BitmapTex *texture = NewDefaultBitmapTex();
				std::wstring path = _T("C:/Users/Connor/Desktop/textures/");

				MultiMtl *multiMat = NewDefaultMultiMtl();
				multiMat->SetNumSubMtls(part.GetMatNames().size());
				for (int x = 0; x != part.GetMatNames().size(); x++)
				{
					BitmapTex *texture = NewDefaultBitmapTex();
					path += part.GetMatNames()[x];
					texture->SetMapName(path.c_str());
					texture->SetName(part.GetMatNames()[x].c_str());

					Mtl* mtl;
					mtl = multiMat->GetSubMtl(x);
					mtl->SetName(part.GetMatNames()[x].c_str());
					mtl->SetSubTexmap(1, texture);

					path = _T("C:/Users/Connor/Desktop/textures/");
				}

				//add to material library ONLY if it doesn't exist.
				int index = mlib.FindMtlByName(multiMat->GetName());
				if (index == -1)
					ip->GetMaterialLibrary().Add(multiMat);

				TriObject* triObject = CreateNewTriObject();
				Mesh &mesh = triObject->GetMesh();
				mesh = part.GetMesh();
				ImpNode *node = importerInt->CreateNode();
				INode *inode = node->GetINode();
				node->Reference(triObject);
				node->SetName(_T("Model"));
				inode->SetMtl(multiMat);

				importerInt->AddNodeToScene(node);
				parent->GetINode()->AttachChild(inode, 0);
				parent->GetINode()->SetMtl(multiMat);

				Matrix3 tm = entry.GetMatrix();
				parent->SetTransform(0, tm);
				importerInt->AddNodeToScene(parent);
			}
			edmStream = NULL;
		}
		catch(const std::exception& e)
		{
			MessageBox(NULL, _T("Failed to import entry"), _T("Error!"), MB_OK);
		}
	}
	importerInt->RedrawViews();

	return TRUE;
}

