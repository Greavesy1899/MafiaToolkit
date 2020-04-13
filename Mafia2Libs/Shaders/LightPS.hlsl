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
    float4 selectionColour;
};

cbuffer Shader_601151254Params
{
    float4 C002MaterialColour;
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
    float3 Normal : NORMAL;
	float3 Tangent : TANGENT;
	float3 Binormal : BINORMAL;
	float2 TexCoord0 : TEXCOORD0;
	float2 TexCoord7 : TEXCOORD1;
	float3 viewDirection : TEXCOORD2;
};

void CalculateFromNormalMap(VS_OUTPUT input)
{
	//Load normal from normal map
	float4 normalMap = textures[1].Sample(SampleType, input.TexCoord0);

	//Change normal map range from [0, 1] to [-1, 1]
	normalMap = (2.0f * normalMap) - 1.0f;

	//Make sure tangent is completely orthogonal to normal
	input.Tangent = normalize(input.Tangent - dot(input.Tangent, input.Normal) * input.Normal);

	//Create the biTangent
	float3 biTangent = cross(input.Normal, input.Tangent);

	//Create the "Texture Space"
	float3x3 texSpace = float3x3(input.Tangent, biTangent, input.Normal);

	//Convert normal from normal map to texture space and store in input.normal
	input.Normal = normalize(mul(normalMap.xyz, texSpace));
}

float4 CalculateColor(VS_OUTPUT input, float4 color)
{
	CalculateFromNormalMap(input);

    float3 lightDir;
    float lightIntensity;
    float3 reflection;
    float4 specular;


    // Set the default output color to the ambient light value for all pixels.
    color = ambientColor;

	// Initialize the specular color.
    specular = float4(0.0f, 0.0f, 0.0f, 0.0f);

	// Invert the light direction for calculations.
    lightDir = -lightDirection;

	// Calculate the amount of the light on this pixel.
    lightIntensity = saturate(dot(input.Normal, lightDir));

	// Determine the final diffuse color based on the diffuse color and the amount of the light intensity.
	color += (diffuseColor * lightIntensity);
	// Saturate the ambient and diffuse color.
	color = saturate(color);

	// Calculate the reflection vector based on the light intensity, normal vector, and light direction.
	reflection = normalize(2 * lightIntensity * input.Normal - lightDir);

	// Determine the amount of the specular light based on the reflection vector, viewing direction, and specular power.
	specular = pow(saturate(dot(reflection, input.viewDirection)), specularPower);

    color = saturate(color + specular);
    return color;
}

float4 LightPixelShader(VS_OUTPUT input) : SV_TARGET
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);
	float4 diffuseTextureColor;
	float4 aoTextureColor;

    diffuseTextureColor = textures[0].Sample(SampleType, input.TexCoord0);
    aoTextureColor = textures[2].Sample(SampleType, input.TexCoord7);

    color = CalculateColor(input, color);
    color = (color * aoTextureColor * diffuseTextureColor * selectionColour);
	
	return color;
}

float4 PS_601151254(VS_OUTPUT input) : SV_TARGET
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);

    color = CalculateColor(input, color);
    color = color * C002MaterialColour * selectionColour;

    return color;
}

float4 PS_50760736(VS_OUTPUT input) : SV_TARGET
{
	float4 color = float4(0.0f, 0.0f, 0.0f, 0.0f);
	float4 diffuseTextureColor;
	float4 emissiveTextureColor;
	float4 aoTextureColor;

	diffuseTextureColor = textures[0].Sample(SampleType, input.TexCoord0);
	aoTextureColor = textures[2].Sample(SampleType, input.TexCoord7);
	emissiveTextureColor = (textures[1].Sample(SampleType, input.TexCoord0)* C005_EmissiveFacadeColorAndIntensity);
	color = CalculateColor(input, color);
    color = (color * aoTextureColor * diffuseTextureColor * selectionColour);

	return color;
}
