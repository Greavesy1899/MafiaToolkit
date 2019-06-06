#include "Utilities.h"
#include <algorithm>

std::string ReadString(FILE* stream, std::string string)
{
	char numBytes;

	fread(&numBytes, sizeof(char), 1, stream);

	for (int i = 0; i != numBytes; i++) {
		char nChar;
		fread(&nChar, sizeof(char), 1, stream);
		string += nChar;
	}
	//remove illegal character.
	string.erase(std::remove(string.begin(), string.end(), '?'), string.end());
	return string;
}

void WriteString(FILE* stream, std::string string)
{
	char numBytes = string.size();

	fwrite(&numBytes, sizeof(char), 1, stream);

	for (int i = 0; i != numBytes; i++) {
		fwrite(&string[i], sizeof(char), 1, stream);
	}
}

void WriteLine(const char *format, ...)
{
	char buf[255];
	va_list va;
	va_start(va, format);
	_vsnprintf_s(buf, sizeof(buf), format, va);
	va_end(va);

	FBXSDK_printf(buf);
	FBXSDK_printf("\n");
}