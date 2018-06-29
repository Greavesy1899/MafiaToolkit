#include "M2Helpers.h";

const char* ReadString(FILE* stream)
{
	byte numBytes;
	fread(&numBytes, sizeof(byte), 1, stream);
	//char* tempChars = new char[numBytes];
	fseek(stream, numBytes, SEEK_CUR);
	//fread(&tempChars, 1, numBytes, stream);

	return "null";
}