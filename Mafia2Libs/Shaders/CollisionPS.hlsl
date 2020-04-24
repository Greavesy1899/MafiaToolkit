//////////////////////
////   GLOBALS
//////////////////////
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

//////////////////////
////   TYPES
//////////////////////
struct VS_OUTPUT
{
	float4 Position : SV_POSITION;
    float3 Normal : NORMAL;
    float4 Colour : COLOR;
	float3 viewDirection : TEXCOORD0;
};

float4 CalculateColor(VS_OUTPUT input, float4 color)
{
    float3 lightDir;
    float lightIntensity;
    float3 reflection;
    float4 specular;
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

float4 CollisionShader(VS_OUTPUT input) : SV_TARGET
{
    float4 color = float4(0.0f, 0.0f, 0.0f, 1.0f);
    color = CalculateColor(input, color);
    color = color * input.Colour * selectionColour;
    color.a = 1.0f;
	return color;
}