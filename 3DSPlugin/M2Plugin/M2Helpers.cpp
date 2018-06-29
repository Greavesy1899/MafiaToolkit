#include "M2Helpers.h";

void ReadString(FILE* stream, const wchar_t* string)
{
	byte numBytes;
	std::wstring name;

	fread(&numBytes, sizeof(byte), 1, stream);

	for (int i = 0; i != numBytes; i++) {
		char nChar;
		fread(&nChar, sizeof(char), 1, stream);
		name += nChar;
	}

	string = name.c_str();
}