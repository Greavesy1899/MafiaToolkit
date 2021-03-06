#include "PhysXStream.h"
#include <Nxf.h>
#include <array>

NxU8 PhysXStream::readByte() const
{
	NxU8 Value = 0;
	size_t Read = fread(&Value, sizeof(NxU8), 1, Stream);
	NX_ASSERT(Read);
	return Value;
}

NxU16 PhysXStream::readWord() const
{
	NxU16 Value = 0;
	size_t Read = fread(&Value, sizeof(NxU16), 1, Stream);
	NX_ASSERT(Read);
	return Value;
}

NxU32 PhysXStream::readDword() const
{
	NxU32 Value = 0;
	size_t Read = fread(&Value, sizeof(NxU32), 1, Stream);
	NX_ASSERT(Read);
	return Value;
}

NxF32 PhysXStream::readFloat() const
{
	NxF32 Value = 0.0f;
	size_t Read = fread(&Value, sizeof(NxF32), 1, Stream);
	NX_ASSERT(Read);
	return Value;
}

NxF64 PhysXStream::readDouble() const
{
	NxF64 Value = 0.0f;
	size_t Read = fread(&Value, sizeof(NxF64), 1, Stream);
	NX_ASSERT(Read);
	return Value;
}

void PhysXStream::readBuffer(void* buffer, NxU32 size) const
{
	size_t Read = fread(buffer, size, 1, Stream);
	NX_ASSERT(Read);
}

NxStream& PhysXStream::storeByte(NxU8 b)
{
	size_t Written = fwrite(&b, sizeof(NxU8), 1, Stream);
	NX_ASSERT(Written);
	return *this;
}

NxStream& PhysXStream::storeWord(NxU16 w)
{
	size_t Written = fwrite(&w, sizeof(NxU16), 1, Stream);
	NX_ASSERT(Written);
	return *this;
}

NxStream& PhysXStream::storeDword(NxU32 d)
{
	size_t Written = fwrite(&d, sizeof(NxU32), 1, Stream);
	NX_ASSERT(Written);
	return *this;
}

NxStream& PhysXStream::storeFloat(NxF32 f)
{
	size_t Written = fwrite(&f, sizeof(NxReal), 1, Stream);
	NX_ASSERT(Written);
	return *this;
}

NxStream& PhysXStream::storeDouble(NxF64 f)
{
	size_t Written = fwrite(&f, sizeof(NxF64), 1, Stream);
	NX_ASSERT(Written);
	return *this;
}

NxStream& PhysXStream::storeBuffer(const void* buffer, NxU32 size)
{
	size_t Written = fwrite(buffer, size, 1, Stream);
	NX_ASSERT(Written);
	return *this;
}

bool PhysXStream::OpenStream(const char* FileName, const char* Mode)
{
	fopen_s(&Stream, FileName, Mode);
	return true;
}

void PhysXStream::CloseStream()
{
	if (Stream)
	{
		fclose(Stream);
		Stream = nullptr;
	}
}
