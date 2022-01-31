#pragma once

#include <string>

class MT_ObjectBundle;
class MT_Object;

class MT_ObjectHandler
{
public:

	static MT_Object* ReadObjectFromFile(const std::string& FileName);

	static MT_ObjectBundle* ReadBundleFromFile(const std::string& FileName);

	static void WriteObjectToFile(const std::string& FileName, const MT_Object& Object);

	static void WriteBundleToFile(const std::string& FileName, const MT_ObjectBundle& Bundle);

private:

	static bool ReadObjectFromFile(FILE* InStream, MT_Object* NewObject);
	static void WriteObjectToFile(FILE* OutStream, const MT_Object& Object);
};
