cbuffer MatrixBuffer : register(b0)
{
	matrix worldMatrix;
	matrix viewProjectionMatrix;
};

cbuffer CameraBuffer : register(b1)
{
	float3 cameraPosition;
	float padding;
};

cbuffer HighlightBuffer : register(b2)
{
	int highlightedInstanceIndex; // Index of instance we want to be selected
	uint s;
	uint ss;
	uint sss;
};

// Instance transformation matrix buffer
StructuredBuffer<matrix> InstanceBuffer : register(t0);

struct VS_INPUT // Reduced size to fit into a single 64 byte cache line
{
	float3 Position : POSITION;
	float3 Normal : NORMAL;
	float3 Tangent : TANGENT;
	float3 Binormal : BINORMAL;
	float2 TexCoord0 : TEXCOORD0;
	float2 TexCoord7 : TEXCOORD1;
};

struct VS_OUTPUT
{
	float4 Position : SV_POSITION;
	half3 Normal : NORMAL;
	half3 Tangent : TANGENT;
	half3 Binormal : BINORMAL;
	float2 TexCoord0 : TEXCOORD0;
	float2 TexCoord7 : TEXCOORD1;
	half3 viewDirection : TEXCOORD2;
	bool instanceSelected : INSTANCESELECTED;
};

VS_OUTPUT LightVertexShader(VS_INPUT input)
{
	VS_OUTPUT output;
	float4 worldPosition;

	// Change the position vector to be 4 units for proper matrix calculations.
	float4 position;
	position.xyz = input.Position.xyz;
	position.w = 1.0f;
	input.TexCoord0.y = -input.TexCoord0.y;
	input.TexCoord7.y = -input.TexCoord7.y;

	// Calculate the position of the vertex against the world, view, and projection matrices.
	worldPosition = mul(position, worldMatrix);
	output.Position = mul(worldPosition, viewProjectionMatrix);

	// Store the texture coordinates for the pixel shader.
	output.TexCoord0 = input.TexCoord0;
	output.TexCoord7 = input.TexCoord7;

	// Calculate the normal vector against the world matrix only.
	output.Normal = mul(input.Normal, (float3x3)worldMatrix);
	output.Normal = normalize(output.Normal);

	output.Tangent = mul(input.Tangent, (float3x3)worldMatrix);
	output.Tangent = normalize(output.Tangent);

	output.Binormal = mul(input.Binormal, (float3x3)worldMatrix);
	output.Binormal = normalize(output.Binormal);

	// Determine the viewing direction based on the position of the camera and the position of the vertex in the world.
	output.viewDirection = cameraPosition.xyz - worldPosition.xyz;

	// Normalize the viewing direction vector.
	output.viewDirection = normalize(output.viewDirection);

	output.instanceSelected = false;

	return output;
}

VS_OUTPUT LightInstanceVertexShader(VS_INPUT input, uint InstanceId : SV_InstanceID)
{
	VS_OUTPUT output;

	float4 position;
	position.xyz = input.Position.xyz;
	position.w = 1.0f;
	input.TexCoord0.y = -input.TexCoord0.y;
	input.TexCoord7.y = -input.TexCoord7.y;

	// Fetch the instance transformation matrix using InstanceID
	matrix instanceMatrix = InstanceBuffer[InstanceId];

	// Transform position by instance matrix
	float4 worldPosition = mul(position, instanceMatrix);
	output.Position = mul(worldPosition, viewProjectionMatrix); // Apply view matrix

	// Store the texture coordinates for the pixel shader
	output.TexCoord0 = input.TexCoord0;
	output.TexCoord7 = input.TexCoord7;

	// Calculate normals, tangents, and binormals using the instance matrix
	output.Normal = normalize(mul(input.Normal, (float3x3)instanceMatrix));
	output.Tangent = normalize(mul(input.Tangent, (float3x3)instanceMatrix));
	output.Binormal = normalize(mul(input.Binormal, (float3x3)instanceMatrix));

	// Calculate the viewing direction
	output.viewDirection = normalize(cameraPosition - worldPosition.xyz); // Normalize the view direction

	output.instanceSelected = false;
	if (InstanceId == highlightedInstanceIndex)//since unselected are -1 and highlight is compared to index of instance, this is how we make unselect here
	{
		output.instanceSelected = true;
	}

	return output;
}