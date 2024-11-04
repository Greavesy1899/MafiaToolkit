cbuffer MatrixBuffer : register(b0)
{
    float4x4 worldMatrix;        // Changed to float4x4
    float4x4 viewMatrix;         // Changed to float4x4
    float4x4 projectionMatrix;   // Changed to float4x4
};

cbuffer CameraBuffer : register(b1)
{
    float3 cameraPosition;
    float padding; // padding to align to 16 bytes
};

// Instance transformation matrix buffer
cbuffer InstanceBuffer : register(b2)
{
    float4x4 instanceTransforms[100]; // Adjust size as needed
};

struct VS_INPUT
{
    float4 Position : POSITION;
    float3 Normal : NORMAL;
    float3 Tangent : TANGENT;
    float3 Binormal : BINORMAL;
    float2 TexCoord0 : TEXCOORD0;
    float2 TexCoord7 : TEXCOORD1;
    uint InstanceID : INSTANCEID; // This must match the semantic name and index
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

    // Fetch the instance transformation matrix using InstanceID
    //float4x4 instanceMatrix = instanceTransforms[input.InstanceID];

    // Transform position by instance matrix
    float4 worldPosition = mul(input.Position, 0);
    output.Position = mul(worldPosition, viewMatrix); // Apply view matrix
    output.Position = mul(output.Position, projectionMatrix); // Apply projection matrix

    // Store the texture coordinates for the pixel shader
    output.TexCoord0 = input.TexCoord0;
    output.TexCoord7 = input.TexCoord7;

    // Calculate normals, tangents, and binormals using the instance matrix
    output.Normal = normalize(mul(input.Normal, (float3x3)0));
    output.Tangent = normalize(mul(input.Tangent, (float3x3)0));
    output.Binormal = normalize(mul(input.Binormal, (float3x3)0));

    // Determine the world position for view direction calculation
    worldPosition = mul(input.Position, 0);

    // Calculate the viewing direction
    output.viewDirection = normalize(cameraPosition - worldPosition.xyz); // Normalize the view direction

    return output;
}