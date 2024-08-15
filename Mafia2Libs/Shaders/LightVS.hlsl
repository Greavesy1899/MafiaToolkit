cbuffer MatrixBuffer
{
	matrix worldMatrix;
	matrix viewMatrix;
	matrix projectionMatrix;
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
	float3 Tangent : TANGENT;
    float3 Binormal : BINORMAL;
	float2 TexCoord0 : TEXCOORD0;
	float2 TexCoord7 : TEXCOORD1;
};

struct VS_OUTPUT
{
	float4 Position : SV_POSITION;
    float3 Normal : NORMAL;
	float3 Tangent : TANGENT;
	float3 Binormal : BINORMAL;
	float2 TexCoord0 : TEXCOORD0;
	float2 TexCoord7 : TEXCOORD1;
    float3 viewDirection : TEXCOORD2;
};

VS_OUTPUT LightVertexShader(VS_INPUT input)
{
	VS_OUTPUT output;
	float4 worldPosition;

	// Change the position vector to be 4 units for proper matrix calculations.
	input.Position.w = 1.0f;

	// Calculate the position of the vertex against the world, view, and projection matrices.
	output.Position = mul(input.Position, worldMatrix);
	output.Position = mul(output.Position, viewMatrix);
	output.Position = mul(output.Position, projectionMatrix);

	// Store the texture coordinates for the pixel shader.
	output.TexCoord0 = input.TexCoord0;
	output.TexCoord7 = input.TexCoord7;
    
	// Calculate the normal vector against the world matrix only.
    output.Normal = mul(input.Normal, (float3x3) worldMatrix);
    output.Normal = normalize(output.Normal);
    
    output.Tangent = mul(input.Tangent, (float3x3) worldMatrix);
    output.Tangent = normalize(output.Tangent);
    
    output.Binormal = mul(input.Binormal, (float3x3) worldMatrix);
    output.Binormal = normalize(output.Binormal);

	// Calculate the position of the vertex in the world.
	worldPosition = mul(input.Position, worldMatrix);

	// Determine the viewing direction based on the position of the camera and the position of the vertex in the world.
	output.viewDirection = cameraPosition.xyz - worldPosition.xyz;

	// Normalize the viewing direction vector.
	output.viewDirection = normalize(output.viewDirection);

	return output;
}