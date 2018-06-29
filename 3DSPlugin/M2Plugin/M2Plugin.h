#pragma once

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
// DESCRIPTION: Includes for Plugins
// AUTHOR: 
//***************************************************************************/

#include "3dsmaxsdk_preinclude.h"
#include "resource.h"
#include <istdplug.h>
#include <iparamb2.h>
#include <iparamm2.h>
#include <maxtypes.h>
#include <vector>
//SIMPLE TYPE


#include <impexp.h>
#include <direct.h>
#include <commdlg.h>


extern TCHAR *GetString(int id);

extern HINSTANCE hInstance;

class EDMImport : public SceneImport
{
public:
	//Constructor/Destructor
	EDMImport();
	virtual ~EDMImport();

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

class EDMWorkClass {
private:
	FILE *stream;

public:
	EDMWorkClass(const TCHAR* filename, const TCHAR* mode)
	{
		if (mode != nullptr && (mode[0] == _T('w') || mode[0] == _T('a') || (mode[0] == _T('r') && mode[1] == _T('+'))))
		{
			MaxSDK::Util::Path storageNamePath(filename);
			storageNamePath.SaveBaseFile();
		}
		stream = _tfopen(filename, mode);
	}
	~EDMWorkClass() { if (stream) fclose(stream); stream = NULL; };
	FILE *			Stream() { return stream; };
};

//EDM STRUCTURES.
typedef struct {
	int s1;
	int s2;
	int s3;
} Int3;

class EDMPart {
public:
	std::string name;
	int vertSize;
	std::vector<Point3> vertices;
	int uvSize;
	std::vector<UVVert> uvs;
	int indicesSize;
	std::vector<Int3> indices;

	EDMPart();
	~EDMPart();
};

class EDMStructure {
private:
public:
	const char * name;
	int partSize;
	std::vector<EDMPart> parts;
	EDMStructure();
	~EDMStructure();
	
};