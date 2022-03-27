#include "LogSystem.h"

#include <cstdarg>

namespace LogFileUtils
{
	const std::string InfoString = "[INFO]";
	const std::string WarningString = "[WARNING]";
	const std::string ErrorString = "[ERROR]";
}

std::ofstream mFile;

void LogSystem::Construct(const std::string& name)
{
	if (!IsOpen())
	{
		mFile.open(name.data(), ios::out | ios::trunc);
	}
}

void LogSystem::Destroy()
{
	if (IsOpen())
	{
		mFile.close();
	}
}

void LogSystem::Printf(const ELogType Error, const char* Text, ...)
{
	if (IsOpen()) 
	{
		char buffer[4096]{ NULL };

		va_list va;
		va_start(va, Text);
		vsprintf_s(buffer, Text, va);
		va_end(va);
		WriteLine(Error, buffer);
	}
}

void LogSystem::WriteLine(const ELogType Error, const char* Text)
{
	if (IsOpen())
	{
		Write(Error, Text);
		Append();
	}
}

bool LogSystem::IsOpen()
{
	return mFile.is_open();
}

void LogSystem::Append()
{
	if (IsOpen())
	{
		mFile << std::endl;
	}
}

void LogSystem::Write(const ELogType Error, const char* Text)
{
	// TODO: We can probably separate this to another function
	std::string ErrorType = "";
	switch (Error)
	{
	case ELogType::eError:
	{
		ErrorType = LogFileUtils::ErrorString;
		break;
	}
	case ELogType::eInfo:
	{
		ErrorType = LogFileUtils::InfoString;
		break;
	}
	case ELogType::eWarning:
	{
		ErrorType = LogFileUtils::WarningString;
		break;
	}
	default:
	{
		break;
	}
	}

	if (IsOpen())
	{
		mFile << ErrorType << " ";
		mFile << Text;
	}

	printf("%s %s\n", ErrorType.data(), Text);
}
