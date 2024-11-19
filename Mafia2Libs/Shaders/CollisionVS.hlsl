cbuffer MatrixBuffer
{
	matrix worldMatrix;
	matrix viewProjectionMatrix;
};

cbuffer CameraBuffer
{
	float3 cameraPosition;
	float padding;
};

struct VS_INPUT
{
	float4 Position : POSITION;
    float3 Normal : NORMAL;
    float4 Colour : COLOR;
};

struct VS_OUTPUT
{
    float4 Position : SV_POSITION;
    float3 Normal : NORMAL;
    float4 Colour : COLOR;
    float3 viewDirection : TEXCOORD0;
};

VS_OUTPUT CollisionShader(VS_INPUT input)
{
	VS_OUTPUT output;
	float4 worldPosition;
    output.Colour = input.Colour.bgra;

	// Change the position vector to be 4 units for proper matrix calculations.
	input.Position.w = 1.0f;

	// Calculate the position of the vertex against the world, view, and projection matrices.
	output.Position = mul(input.Position, worldMatrix);
	output.Position = mul(output.Position, viewProjectionMatrix);

	// Calculate the normal vector against the world matrix only.
	output.Normal = mul(input.Normal, (float3x3)worldMatrix);

	// Normalize the normal vector.
	output.Normal = normalize(output.Normal);

	// Calculate the position of the vertex in the world.
	worldPosition = mul(input.Position, worldMatrix);

	// Determine the viewing direction based on the position of the camera and the position of the vertex in the world.
	output.viewDirection = cameraPosition.xyz - worldPosition.xyz;

	// Normalize the viewing direction vector.
	output.viewDirection = normalize(output.viewDirection);

	return output;
}