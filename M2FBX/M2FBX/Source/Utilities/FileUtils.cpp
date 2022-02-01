#include "FileUtils.h"

// Utility function to read std::string
void FileUtils::ReadString(FILE* InStream, std::string* OutString)
{
	char StringLength;
	FileUtils::Read(InStream, &StringLength);

	for (int i = 0; i < StringLength; i++)
	{
		char nChar;
		FileUtils::Read(InStream, &nChar);
		OutString->push_back(nChar);
	}
}

// Utlity function to write std::string
void FileUtils::WriteString(FILE* OutStream, const std::string& StringToWrite)
{
	char StringLength = StringToWrite.size();
	FileUtils::Write(OutStream, StringLength);

	for (int i = 0; i < StringLength; i++)
	{
		FileUtils::Write(OutStream, StringToWrite[i]);
	}
}