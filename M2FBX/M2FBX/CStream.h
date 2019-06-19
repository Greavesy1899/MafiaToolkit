#ifndef CSTREAM_H
#define CSTREAM_H
#include "Common.h"
#include "NxStream.h"


//This class is from the "CookedMeshReader" training solution, but modified for my needs and just to help my OCD.
//TODO: Made FBX friendly.
class CStream : public NxStream
{
public:
	CStream(const char* filename, bool load);
	virtual						~CStream();

	virtual		bool			isOpen();
	virtual		void			closeStream();
	virtual		bool			advanceStream(NxU32 nbBytes);

	virtual		NxU8			readByte()								const;
	virtual		NxU16			readWord()								const;
	virtual		NxU32			readDword()								const;
	virtual		float			readFloat()								const;
	virtual		double			readDouble()							const;
	virtual		void			readBuffer(void* buffer, NxU32 size)	const;

	virtual		NxStream& storeByte(NxU8 b);
	virtual		NxStream& storeWord(NxU16 w);
	virtual		NxStream& storeDword(NxU32 d);
	virtual		NxStream& storeFloat(NxReal f);
	virtual		NxStream& storeDouble(NxF64 f);
	virtual		NxStream& storeBuffer(const void* buffer, NxU32 size);

	FILE* fp;
};

#endif