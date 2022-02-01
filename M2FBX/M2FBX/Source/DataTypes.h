#pragma once

#include "Common.h"

// TODO: Clean up this entire file

enum VertexFlags
{
	None = 0,
	Position = (1 << 0),
	Position2D = (1 << 1),
	Normals = (1 << 2),
	Tangent = (1 << 4), // 0x00000010
	Skin = (1 << 6), // 0x00000040
	Color = (1 << 7), // 0x00000080
	Texture = (1 << 8),
	TexCoords0 = 256, // 0x00000100
	TexCoords1 = 512, // 0x00000200
	TexCoords2 = 1024, // 0x00000400
	ShadowTexture = (1 << 15), // 0x00008000 
	Color1 = (1 << 17), // 0x00020000
	BBCoeffs = (1 << 18), // 0x00040000
	DamageGroup = (1 << 20), // 0x00100000
};

struct Int3 
{
	unsigned int i1;
	unsigned int i2;
	unsigned int i3;
};

struct Point3
{
	float x;
	float y;
	float z;
};

struct Quaternion 
{
	float x;
	float y;
	float z;
	float w;
};

struct UVVert
{	
	float x;
	float y;
};

struct Matrix 
{
	Point3 position;
	Quaternion rotation;
	Point3 scale;
};

struct Joint
{
	unsigned char parentID;
	Matrix transform;
};

struct Vertex
{
	Point3 position;
	Point3 normals;
	Point3 tangent;
	unsigned char boneIDs[4];
	float boneWeights[4];
	unsigned int damageGroup;
	unsigned char color0[4];
	unsigned char color1[4];
	UVVert uv0;
	UVVert uv1;
	UVVert uv2;
	UVVert uv3;
};