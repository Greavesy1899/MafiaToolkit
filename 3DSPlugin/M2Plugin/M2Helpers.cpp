#include "M2Helpers.h"

std::wstring ReadString(FILE* stream, std::wstring &string)
{
	byte numBytes;

	fread(&numBytes, sizeof(byte), 1, stream);

	for (int i = 0; i != numBytes; i++) {
		char nChar;
		fread(&nChar, sizeof(char), 1, stream);
		string += nChar;
	}
	return string;
}