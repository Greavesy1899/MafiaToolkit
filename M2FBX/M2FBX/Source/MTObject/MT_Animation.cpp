#include "MT_Animation.h"

#include "Utilities/FileUtils.h"

// CPP
#include <stdint.h>

bool MT_AnimTrack::ReadFromFile(FILE* InStream)
{
	FileUtils::Read(InStream, &BoneID);
	FileUtils::ReadString(InStream, &BoneName);
	FileUtils::Read(InStream, &Duration);

	uint16_t NumRotKeys = 0;
	uint16_t NumPosKeys = 0;
	FileUtils::Read(InStream, &NumPosKeys);
	FileUtils::Read(InStream, &NumRotKeys);
	RotationKeyFrames.reserve(NumRotKeys);
	PositionKeyFrames.reserve(NumPosKeys);

	for (uint16_t i = 0; i < NumRotKeys; i++)
	{
		MT_RotKey KeyFrame = {};
		FileUtils::Read(InStream, &KeyFrame.Time);
		FileUtils::Read(InStream, &KeyFrame.Value);

		RotationKeyFrames.emplace_back(KeyFrame);
	}

	for (uint16_t i = 0; i < NumPosKeys; i++)
	{
		MT_PosKey KeyFrame = {};
		FileUtils::Read(InStream, &KeyFrame.Time);
		FileUtils::Read(InStream, &KeyFrame.Value);

		PositionKeyFrames.emplace_back(KeyFrame);
	}

	return true;
}

void MT_AnimTrack::WriteToFile(FILE* OutStream) const
{

}

bool MT_Animation::ReadFromFile(FILE* InStream)
{
	uint16_t NumTracks = 0;
	FileUtils::Read(InStream, &NumTracks);

	Tracks.reserve(NumTracks);
	for (uint32_t i = 0; i < NumTracks; i++)
	{
		MT_AnimTrack NewTrack = {};
		NewTrack.ReadFromFile(InStream);

		Tracks.emplace_back(NewTrack);
	}

	return true;
}

void MT_Animation::WriteToFile(FILE* OutStream) const
{


}
