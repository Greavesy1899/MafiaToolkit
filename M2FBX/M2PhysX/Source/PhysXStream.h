#pragma once

#include <cstdio>
#include <NxSimpleTypes.h>
#include <NxStream.h>

#include <vector>

class PhysXStream : public NxStream
{

public:

	// ~Begin NxStream Interface
	NxU8 readByte() const override;
	NxU16 readWord() const override;
	NxU32 readDword() const override;
	NxF32 readFloat() const override;
	NxF64 readDouble() const override;
	void readBuffer(void* buffer, NxU32 size) const override;
	NxStream& storeByte(NxU8 b) override;
	NxStream& storeWord(NxU16 w) override;
	NxStream& storeDword(NxU32 d) override;
	NxStream& storeFloat(NxF32 f) override;
	NxStream& storeDouble(NxF64 f) override;
	NxStream& storeBuffer(const void* buffer, NxU32 size) override;
	// ~End NxStream Interface

	bool OpenStream(const char* FileName, const char* Mode);
	void CloseStream();

	// broken
	char* GetContentsAsBuffer(NxU32& Size);

private:

	FILE* Stream = nullptr;

	std::vector<byte> StreamArray;
};
