cbuffer MatrixBuffer
{
    matrix viewMatrix;
    matrix projectionMatrix;
};

cbuffer InstanceBuffer : register(b1) 
{
    matrix InstanceTransforms[100]; // Adjust size based on maximum instances
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
    uint InstanceID : INSTANCEID; // Instance ID to access the transformation
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

    // Ensure proper w component for position vector
    input.Position.w = 1.0f;
    input.TexCoord0.y = -input.TexCoord0.y;
    input.TexCoord7.y = -input.TexCoord7.y;

    // Get the instance transformation matrix using the InstanceID
    matrix instanceMatrix = InstanceTransforms[input.InstanceID];

    // Transform the vertex position using the instance matrix
    worldPosition = mul(input.Position, instanceMatrix);
    
    // Transform to clip space
    output.Position = mul(worldPosition, viewMatrix);
    output.Position = mul(output.Position, projectionMatrix);

    // Pass through texture coordinates
    output.TexCoord0 = input.TexCoord0;
    output.TexCoord7 = input.TexCoord7;

    // Transform normals, tangents, and binormals using the instance matrix
    output.Normal = normalize(mul(input.Normal, (float3x3)instanceMatrix));
    output.Tangent = normalize(mul(input.Tangent, (float3x3)instanceMatrix));
    output.Binormal = normalize(mul(input.Binormal, (float3x3)instanceMatrix));

    // Calculate the world position for the view direction
    worldPosition = mul(input.Position, instanceMatrix);

    // Calculate view direction
    output.viewDirection = normalize(cameraPosition - worldPosition.xyz);

    return output;
}
