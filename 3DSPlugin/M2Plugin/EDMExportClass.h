#ifndef M2_EDMEXPORT_HEADER
#define M2_EDMEXPORT_HEADER
#include "3dsmaxsdk_preinclude.h"
#include "resource.h"
#include <istdplug.h>
#include <impexp.h>
#include "M2Plugin.h"

class EDMExport : public SceneExport
{
public:
	//Constructor/Destructor
	EDMExport();
	virtual ~EDMExport();

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
	virtual int				DoExport(const MCHAR *name, ExpInterface *ei, Interface *i, BOOL suppressPrompts, DWORD options);	// Export File
};
//
class EDMExportWorkFile {
private:
	FILE * stream;

public:
	EDMExportWorkFile(const TCHAR *filename, const TCHAR *mode) { stream = NULL; Open(filename, mode); };
	~EDMExportWorkFile() { Close(); };
	FILE *			Stream() { return stream; };
	int				Close() { int result = 0; if (stream) result = fclose(stream); stream = NULL; return result; }
	void			Open(const TCHAR *filename, const TCHAR *mode)
	{
		Close();
		// for a360 support - allows binary diff syncing
		if (mode != nullptr && (mode[0] == _T('w') || mode[0] == _T('a') || (mode[0] == _T('r') && mode[1] == _T('+'))))
		{
			MaxSDK::Util::Path storageNamePath(filename);
			storageNamePath.SaveBaseFile();
		}
		stream = _tfopen(filename, mode);
	}
};
#endif