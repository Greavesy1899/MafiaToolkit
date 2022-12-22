#pragma once

#include "MT_MaterialInstance.h"

typedef unsigned int uint;

class MT_ObjectHandler;

class MT_FaceGroup
{

	friend MT_ObjectHandler;

public:

	MT_FaceGroup();

	void Cleanup();

	// Accessors
	const uint GetStartIndex() const { return StartIndex; }
	const uint GetNumFaces() const { return NumFaces; }
	const MT_MaterialInstance& GetMaterialInstance() const { return *MaterialInstance; }

	// Setters
	void SetStartIndex(const uint InStartIndex) { StartIndex = InStartIndex; }
	void SetNumFaces(const uint InNumFaces) { NumFaces = InNumFaces; }
	void SetMatInstance(MT_MaterialInstance* InMatInstance) { MaterialInstance = InMatInstance; }

	// IO
	void ReadFromFile(FILE* InStream);
	void WriteToFile(FILE* OutStream) const;

private:

	uint StartIndex = 0;
	uint NumFaces = 0;
	MT_MaterialInstance* MaterialInstance = nullptr;

};
