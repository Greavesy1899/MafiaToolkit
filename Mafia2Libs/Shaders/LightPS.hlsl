//////////////////////
////   GLOBALS
//////////////////////
Texture2D textures[3];
SamplerState SampleType;

cbuffer LightBuffer
{
	float4 ambientColor;
	float4 diffuseColor;
	float3 lightDirection;
	float specularPower;
	float4 specularColor;
};

cbuffer EditorParameterBuffer
{
    float3 selectionColour;
    int renderMode;
};

cbuffer Shader_601151254Params
{
    float4 C002MaterialColour;
};

//TODO: Make this shader specific
cbuffer ExtraParameterBuffer
{
    int hasTangentSpace;
    float3 padding;
};

cbuffer Shader_50760736Params
{
	float4 C005_EmissiveFacadeColorAndIntensity;
};

//////////////////////
////   TYPES
//////////////////////
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

float4 GetDiffuseColour(VS_OUTPUT input)
{
    return textures[0].Sample(SampleType, input.TexCoord0);
}

float4 GetEmissiveColour(VS_OUTPUT input)
{
    return textures[1].Sample(SampleType, input.TexCoord0) * C005_EmissiveFacadeColorAndIntensity;
}

float4 GetAOColour(VS_OUTPUT input)
{
    float4 sampled = textures[2].Sample(SampleType, input.TexCoord7);
    return float4(sampled.xy, sampled.x, 1.0f);
}

float3 CalculateFromNormalMap(VS_OUTPUT input)
{
	//Load normal from normal map
	float4 normalMap = textures[1].Sample(SampleType, input.TexCoord0);

	//Change normal map range from [0, 1] to [-1, 1]
	normalMap = (2.0f * normalMap) - 1.0f;

	//Make sure tangent is completely orthogonal to normal
	input.Tangent = normalize(input.Tangent - dot(input.Tangent, input.Normal) * input.Normal);

	//Convert normal from normal map to texture space and store in input.normal
    float3 bumpNormal = (normalMap.x * input.Tangent) + (normalMap.y * input.Binormal)/* + (normalMap.z * input.Normal)*/;
    bumpNormal = normalize(bumpNormal);
    return bumpNormal;
}

float4 CalculateColor(VS_OUTPUT input, float4 color)
{
    float3 lightDir;
    float lightIntensity;
    float3 reflection;
    float4 specular;
    float3 normal = float3(1.0f, 1.0f, 1.0f);
    
    if(hasTangentSpace == 1)
    {
        normal = CalculateFromNormalMap(input);
    }

    // Set the default output color to the ambient light value for all pixels.
    color = ambientColor;

	// Initialize the specular color.
    specular = float4(0.0f, 0.0f, 0.0f, 0.0f);

	// Invert the light direction for calculations.
    lightDir = -lightDirection;

	// Calculate the amount of the light on this pixel.
    lightIntensity = saturate(dot(input.Normal * normal, lightDir));

    // Determine the final diffuse color based on the diffuse color and the amount of the light intensity.
    color += (diffuseColor * lightIntensity);
	// Saturate the ambient and diffuse color.
    color = saturate(color);

	// Calculate the reflection vector based on the light intensity, normal vector, and light direction.
	reflection = normalize(2 * lightIntensity * input.Normal - lightDir);

	// Determine the amount of the specular light based on the reflection vector, viewing direction, and specular power.
	specular = pow(saturate(dot(reflection, input.viewDirection)), specularPower);

    color = saturate(color + specular);
    color.a = 1.0f;
    return color;
}

float4 LightPixelShader(VS_OUTPUT input) : SV_TARGET
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);
    float4 diffuseTextureColor = GetDiffuseColour(input);
    float4 aoTextureColor = GetAOColour(input);
    
    color = CalculateColor(input, color);
    if(input.instanceID==0)
    {
        color *= float4(selectionColour.xyz, 1.0f);
    }
    else
    {
        color *= float4(1.0f, 0.0f, 0.0f, 1.0f);
    }
    
    //if(renderMode == 2)
    //{
        color *= (diffuseTextureColor * aoTextureColor);
    //}

	return color;
}

float4 PS_601151254(VS_OUTPUT input) : SV_TARGET
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);

    color = CalculateColor(input, color);
    color *= float4(selectionColour.xyz, 1.0f);
    if(input.instanceID==0)
    {
        color *= float4(selectionColour.xyz, 1.0f);
    }
    else
    {
        color *= float4(1.0f, 0.0f, 0.0f, 1.0f);
    }
    
    //if(renderMode == 2)
    //{
        color *= C002MaterialColour;
    //}

    return color;
}

float4 PS_50760736(VS_OUTPUT input) : SV_TARGET
{
	float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);
    float4 diffuseTextureColor = GetDiffuseColour(input);
    float4 emissiveTextureColor = GetEmissiveColour(input);
    float4 aoTextureColor = GetAOColour(input);
    
    color = CalculateColor(input, color);
    color *= float4(selectionColour.xyz, 1.0f);
    if(input.instanceID==0)
    {
        color *= float4(selectionColour.xyz, 1.0f);
    }
    else
    {
        color *= float4(1.0f, 0.0f, 0.0f, 1.0f);
    }
    
    //if (renderMode == 2)
    //{
        color *= (diffuseTextureColor * aoTextureColor);
    //}
    
	return color;
}
