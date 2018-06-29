#include "M2Helpers.h";


const char * ReadString(FILE* stream)
{
	byte numBytes;
	std::string name = "";

	fread(&numBytes, sizeof(byte), 1, stream);

	for (int i = 0; i != numBytes; i++) {
		char nChar;
		fread(&nChar, sizeof(char), 1, stream);
		name += nChar;
	}

	return name.c_str();
}