#ifndef GEN_UTILS_HEADER
#define GEN_UTILS_HEADER
#include <iostream>

std::string ReadString(FILE* stream, std::string string);
void WriteString(FILE* stream, std::string string);
#endif