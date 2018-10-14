#ifndef M2_HELPERS_HEADER
#define M2_HELPERS_HEADER
#include <iostream>

std::string ReadString(FILE* stream, std::string &string);
void WriteString(FILE* stream, std::string &string);

#endif