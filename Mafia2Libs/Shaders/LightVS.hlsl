cbuffer MatrixBuffer : register(b0)
{
    matrix worldMatrix;        // World matrix for object transformation
    matrix viewMatrix;         // View matrix
    matrix projectionMatrix;   // Projection matrix
};

cbuffer CameraBuffer : register(b1)
{
    float3 cameraPosition;       // Camera position
    float padding;               // Padding for alignment
};

// Instance transformation matrix buffer
cbuffer InstanceBuffer : register(b2)
{
    matrix instanceTransforms[1024]; // Buffer for instance transformation matrices
};

struct VS_INPUT
{
    float4 Position : POSITION;   // Vertex position
    float3 Normal : NORMAL;       // Vertex normal
    float3 Tangent : TANGENT;     // Vertex tangent
    float3 Binormal : BINORMAL;   // Vertex binormal
    float2 TexCoord0 : TEXCOORD0; // Texture coordinate 0
    float2 TexCoord7 : TEXCOORD1; // Texture coordinate 1
    uint InstanceID : INSTANCEID;  // Instance ID for instance buffer lookup
};

struct VS_OUTPUT
{
    float4 Position : SV_POSITION; // Clip space position
    float3 Normal : NORMAL;        // Normal vector
    float3 Tangent : TANGENT;     // Tangent vector
    float3 Binormal : BINORMAL;    // Binormal vector
    float2 TexCoord0 : TEXCOORD0; // Texture coordinate 0
    float2 TexCoord7 : TEXCOORD1; // Texture coordinate 1
    float3 viewDirection : TEXCOORD2; // View direction for lighting calculations
};

VS_OUTPUT LightVertexShader(VS_INPUT input)
{
    VS_OUTPUT output;

    // Fetch the instance transformation matrix using InstanceID
    matrix instanceMatrix = instanceTransforms[input.InstanceID];

    // Transform position by instance matrix
    float4 worldPosition = mul(input.Position, instanceMatrix);
    output.Position = mul(worldPosition, viewMatrix); // Apply view matrix
    output.Position = mul(output.Position, projectionMatrix); // Apply projection matrix

    // Store the texture coordinates for the pixel shader
    output.TexCoord0 = input.TexCoord0;
    output.TexCoord7 = input.TexCoord7;

    // Calculate normals, tangents, and binormals using the instance matrix
    output.Normal = normalize(mul(input.Normal, (float3x3)instanceMatrix));
    output.Tangent = normalize(mul(input.Tangent, (float3x3)instanceMatrix));
    output.Binormal = normalize(mul(input.Binormal, (float3x3)instanceMatrix));

    // Determine the world position for view direction calculation
    worldPosition = mul(input.Position, instanceMatrix);

    // Calculate the viewing direction
    output.viewDirection = normalize(cameraPosition - worldPosition.xyz); // Normalize the view direction

    return output;
}
