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

// Count buffer to hold the number of instances
Buffer<uint> CountBuffer : register(t0);

// Instance transformation matrix buffer
StructuredBuffer<matrix> InstanceBuffer : register(t1);

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
    float4 Position : SV_POSITION; // Clip space position
    float3 Normal : NORMAL;        // Normal vector
    float3 Tangent : TANGENT;     // Tangent vector
    float3 Binormal : BINORMAL;    // Binormal vector
    float2 TexCoord0 : TEXCOORD0; // Texture coordinate 0
    float2 TexCoord7 : TEXCOORD1; // Texture coordinate 1
    float3 viewDirection : TEXCOORD2; // View direction for lighting calculations
};

VS_OUTPUT LightVertexShader(VS_INPUT input, uint InstanceId : SV_InstanceID)
{
    VS_OUTPUT output;

    input.Position.w = 1.0f;
    input.TexCoord0.y = -input.TexCoord0.y;
    input.TexCoord7.y = -input.TexCoord7.y;

    // Retrieve the instance count from CountBuffer
    uint instanceCount = CountBuffer.Load(0);

    // Check if the InstanceBuffer is empty
    if (instanceCount > 0)
    {
        // Fetch the instance transformation matrix using InstanceID
        matrix instanceMatrix = InstanceBuffer[InstanceId];

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
    }
    else
    {
        // Calculate the position of the vertex against the world, view, and projection matrices.
        output.Position = mul(input.Position, worldMatrix);
        output.Position = mul(output.Position, viewMatrix);
        output.Position = mul(output.Position, projectionMatrix);

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

        // Calculate the position of the vertex in the world.
        float4 worldPosition = mul(input.Position, worldMatrix);

        // Determine the viewing direction based on the position of the camera and the position of the vertex in the world.
        output.viewDirection = cameraPosition.xyz - worldPosition.xyz;

        // Normalize the viewing direction vector.
        output.viewDirection = normalize(output.viewDirection);
    }

    return output;
}