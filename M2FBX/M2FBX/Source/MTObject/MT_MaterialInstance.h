#pragma once

#include <string>

class MT_ObjectHandler;

enum MT_MaterialInstanceFlags
{
	IsCollision = 1,
	HasDiffuse = 2
};

class MT_MaterialInstance
{
	friend MT_ObjectHandler;

public:

	void Cleanup();

	bool HasMaterialFlag(const MT_MaterialInstanceFlags Flag) const;

	// Accessors
	const std::string& GetName() const { return Name; }
	const std::string& GetTextureName() const { return DiffuseTexture; }

	// Setters
	void SetMaterialFlags(const MT_MaterialInstanceFlags InMaterialFlags) { MaterialFlags = InMaterialFlags; }
	void SetName(const std::string& InName) { Name = InName; }
	void SetTextureName(const std::string& InTexture) { DiffuseTexture = InTexture; }

	// IO
	void ReadFromFile(FILE* InStream);
	void WriteToFile(FILE* OutStream) const;

private:

	MT_MaterialInstanceFlags MaterialFlags;
	std::string Name = "";

	// Only exists if HasDiffuse is present
	std::string DiffuseTexture = "";

};
