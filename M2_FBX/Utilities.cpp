#include "Utilities.h"

std::wstring ReadString(FILE* stream, std::wstring &string)
{
	char numBytes;

	fread(&numBytes, sizeof(char), 1, stream);

	for (int i = 0; i != numBytes; i++) {
		char nChar;
		fread(&nChar, sizeof(char), 1, stream);
		string += nChar;
	}
	return string;
}

void WriteString(FILE* stream, std::wstring &string)
{
	char numBytes = string.size();

	fwrite(&numBytes, sizeof(char), 1, stream);

	for (int i = 0; i != numBytes; i++) {
		fwrite(&string[i], sizeof(char), 1, stream);
	}
}