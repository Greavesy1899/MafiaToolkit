#pragma once

#include <string>

namespace FileUtils
{
	// Utility function to templatize 'fread'
	template <class TType>
	size_t Read(FILE* InStream, TType* Buffer)
	{
		return fread(Buffer, sizeof(TType), 1, InStream);
	}

	// Utility function to templatize 'fwrite'
	template <class TType>
	size_t Write(FILE* OutStream, TType Buffer)
	{
		return fwrite(&Buffer, sizeof(TType), 1, OutStream);
	}

	// Utility function to read std::string
	void ReadString(FILE* InStream, std::string* OutString);

	// Utlity function to write std::string
	void WriteString(FILE* OutStream, const std::string& StringToWrite);
}