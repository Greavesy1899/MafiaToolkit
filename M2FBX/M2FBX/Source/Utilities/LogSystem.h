#pragma once

#include <fstream>
#include <iostream>

using namespace std;

enum class ELogType
{
	eInfo,
	eWarning,
	eError
};

class LogSystem
{
public:

	static void Construct(const std::string& name);
	static void Destroy();

	static void Printf(const ELogType Error, const char* Text, ...);
	static void WriteLine(const ELogType Error, const char* Text);

private:

	static bool IsOpen();

	static void Append();
	static void Write(const ELogType Error, const char* Text);
};
