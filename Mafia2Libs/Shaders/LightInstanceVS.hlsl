cbuffer MatrixBuffer : register(b0)
{
    matrix worldMatrix;        // World matrix for object transformation
    matrix viewProjectionMatrix;
};

cbuffer CameraBuffer : register(b1)
{
    float3 cameraPosition;       // Camera position
    float padding;               // Padding for alignment
};

cbuffer HighlightBuffer : register(b2)
{
    uint highlightedInstanceIndex; // Index of instance we want to be selected
    uint s;
    uint ss;
    uint sss;
};


// Instance transformation matrix buffer
StructuredBuffer<matrix> InstanceBuffer : register(t0);

struct VS_INPUT
{
    float4 Position : POSITION;   // Vertex position
    float3 Normal : NORMAL;       // Vertex normal
    float3 Tangent : TANGENT;     // Vertex tangent
    float3 Binormal : BINORMAL;   // Vertex binormal
    float2 TexCoord0 : TEXCOORD0; // Texture coordinate 0
    float2 TexCoord7 : TEXCOORD1; // Texture coordinate 1
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
    uint instanceID : INSTANCEID;
};

VS_OUTPUT LightInstanceVertexShader(VS_INPUT input, uint InstanceId : SV_InstanceID)
{
    VS_OUTPUT output;

    input.Position.w = 1.0f;
    input.TexCoord0.y = -input.TexCoord0.y;
    input.TexCoord7.y = -input.TexCoord7.y;

    // Fetch the instance transformation matrix using InstanceID
    matrix instanceMatrix = InstanceBuffer[InstanceId];

    // Transform position by instance matrix
    float4 worldPosition = mul(input.Position, instanceMatrix);
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

    output.instanceID = 0;
    if(InstanceId==highlightedInstanceIndex)
    {
        output.instanceID =1;
    }

    return output;
}