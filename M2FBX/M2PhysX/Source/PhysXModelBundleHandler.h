#pragma once

#include "PhysXModelBundle.h"

class PhysXModelBundle;
class PhysXModel;

/*
* Handler class for PhysXModelBundle.
* It allows the ability to load and save bundles.
*/
class PhysXModelBundleHandler
{
public:

	/** Load the bundle from the provided file.*/
	static PhysXModelBundle* LoadBundle(const char* BundleFile);

	/** Save the bundle to the provided file. */
	static bool SaveBundle(const PhysXModelBundle& BundleObject, const char* NameOfFile);

	/** Load the Model from the provided file.*/
	static PhysXModel* LoadModel(const char* ModelFile);

	/** Save the Model to the provided file. */
	static bool SaveModel(const PhysXModel& ModelObject, const char* NameOfFile);
};

