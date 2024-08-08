#pragma once

#include "DataTypes.h"

// CPP
#include <vector>

struct MT_RotKey
{
public:

	// Time to apply value
	float Time = 0.0f;

	// value to apply to bone
	Quaternion Value;
};

struct MT_PosKey
{
public:

	// Time to apply value
	float Time = 0.0f;

	// value to apply to bone
	Point3 Value;
};

class MT_AnimTrack
{
public:

	// IO
	bool ReadFromFile(FILE* InStream);
	void WriteToFile(FILE* OutStream) const;

	// getters
	uint16_t GetBoneID() const { return BoneID; }
	std::string GetBoneName() const { return BoneName; }
	float GetDuration() const { return Duration; }
	const std::vector<MT_RotKey>& GetRotatationKeys() const { return RotationKeyFrames; }
	const std::vector<MT_PosKey>& GetPositionKeys() const { return PositionKeyFrames; }

private:

	uint16_t BoneID = 0;

	std::string BoneName;

	float Duration = 0.0f;

	std::vector<MT_RotKey> RotationKeyFrames;

	std::vector<MT_PosKey> PositionKeyFrames;

};

class MT_Animation
{
public:

	// IO
	bool ReadFromFile(FILE* InStream);
	void WriteToFile(FILE* OutStream) const;

	// getters
	const std::vector<MT_AnimTrack> GetAnimTracks() const { return Tracks; }

private:

	std::vector<MT_AnimTrack> Tracks;

};

