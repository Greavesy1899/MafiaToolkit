cbuffer MatrixBuffer
{
	matrix worldMatrix;
	matrix viewMatrix;
	matrix projectionMatrix;
};

struct VS_INPUT
{
	float4 Position : POSITION;
    float3 Colour : COLOR;
};

struct VS_OUTPUT
{
    float4 Position : SV_POSITION;
    float3 Colour : Colour;
};

VS_OUTPUT DebugVertexShader(VS_INPUT input)
{
	VS_OUTPUT output;

	// Change the position vector to be 4 units for proper matrix calculations.
	input.Position.w = 1.0f;

	// Calculate the position of the vertex against the world, view, and projection matrices.
	output.Position = mul(input.Position, worldMatrix);
	output.Position = mul(output.Position, viewMatrix);
	output.Position = mul(output.Position, projectionMatrix);
    output.Colour = input.Colour;

	return output;
}