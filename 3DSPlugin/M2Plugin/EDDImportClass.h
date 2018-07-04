#ifndef M2_EDDIMPORT_HEADER
#define M2_EDDIMPORT_HEADER
#include "3dsmaxsdk_preinclude.h"
#include "resource.h"
#include "M2EDM.h"
#include "M2Plugin.h"
#include <impexp.h>
#include "M2EDM.h"
#include "M2Helpers.h"
#include <filesystem>

class EDDImport : public SceneImport
{
public:
	//Constructor/Destructor
	EDDImport();
	virtual ~EDDImport();

	virtual int				ExtCount();					// Number of extensions supported
	virtual const TCHAR *	Ext(int n);					// Extension #n (i.e. "3DS")
	virtual const TCHAR *	LongDesc();					// Long ASCII description (i.e. "Autodesk 3D Studio File")
	virtual const TCHAR *	ShortDesc();				// Short ASCII description (i.e. "3D Studio")
	virtual const TCHAR *	AuthorName();				// ASCII Author name
	virtual const TCHAR *	CopyrightMessage();			// ASCII Copyright message
	virtual const TCHAR *	OtherMessage1();			// Other message #1
	virtual const TCHAR *	OtherMessage2();			// Other message #2
	virtual unsigned int	Version();					// Version number * 100 (i.e. v3.01 = 301)
	virtual void			ShowAbout(HWND hWnd);		// Show DLL's "About..." box
	virtual int				DoImport(const TCHAR *name, ImpInterface *i, Interface *gi, BOOL suppressPrompts = FALSE);	// Import file
};

class EDDWorkClass {
private:
	FILE * stream;

public:
	EDDWorkClass(const TCHAR* filename, const TCHAR* mode)
	{
		if (mode != nullptr && (mode[0] == _T('w') || mode[0] == _T('a') || (mode[0] == _T('r') && mode[1] == _T('+'))))
		{
			MaxSDK::Util::Path storageNamePath(filename);
			storageNamePath.SaveBaseFile();
		}
		stream = _tfopen(filename, mode);
	}
	~EDDWorkClass() { if (stream) fclose(stream); stream = NULL; };
	FILE *			Stream() { return stream; };
};
#endif