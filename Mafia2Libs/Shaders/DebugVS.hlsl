cbuffer MatrixBuffer
{
	matrix worldMatrix;
	matrix viewProjectionMatrix;
};

// Instance transformation matrix buffer
StructuredBuffer<matrix> InstanceBuffer : register(t0);

struct VS_INPUT
{
	float4 Position : POSITION;
    float4 Colour : COLOR;
};

struct VS_OUTPUT
{
    float4 Position : SV_POSITION;
    float4 Colour : Colour;
};

VS_OUTPUT DebugVertexShader(VS_INPUT input)
{
	VS_OUTPUT output;

	// Change the position vector to be 4 units for proper matrix calculations.
	input.Position.w = 1.0f;

	// Calculate the position of the vertex against the world, view, and projection matrices.
	output.Position = mul(input.Position, worldMatrix);
	output.Position = mul(output.Position, viewProjectionMatrix);
    output.Colour = input.Colour.bgra;

	return output;
}

VS_OUTPUT DebugInstanceVertexShader(VS_INPUT input, uint InstanceId : SV_InstanceID)
{
	VS_OUTPUT output;

	// Fetch the instance transformation matrix using InstanceID
	matrix instanceMatrix = InstanceBuffer[InstanceId];

	// Change the position vector to be 4 units for proper matrix calculations.
	input.Position.w = 1.0f;

	// Calculate the position of the vertex against the instance, view, and projection matrices.
	output.Position = mul(input.Position, instanceMatrix);
	output.Position = mul(output.Position, viewProjectionMatrix);
	output.Colour = input.Colour.bgra;

	return output;
}