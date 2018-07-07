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

void WriteString(FILE* stream, std::wstring &string)
{
	byte numBytes = string.size();

	fwrite(&numBytes, sizeof(byte), 1, stream);

	for (int i = 0; i != numBytes; i++) {
		fwrite(&string[i], sizeof(char), 1, stream);
	}
}