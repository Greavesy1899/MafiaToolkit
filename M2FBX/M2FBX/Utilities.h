#ifndef GEN_UTILS_HEADER
#define GEN_UTILS_HEADER
#include "Common.h"

std::string ReadString(FILE* stream, std::string string);
void WriteString(FILE* stream, std::string string);
void WriteLine(const char* format, ...);

#endif