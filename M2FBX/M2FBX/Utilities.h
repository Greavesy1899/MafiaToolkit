#ifndef M2_HELPERS_HEADER
#define M2_HELPERS_HEADER
#include <iostream>

std::wstring ReadString(FILE* stream, std::wstring &string);
void WriteString(FILE* stream, std::wstring &string);

#endif