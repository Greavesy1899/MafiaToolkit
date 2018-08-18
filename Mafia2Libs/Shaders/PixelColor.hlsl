//////////////////////
////   TYPES
//////////////////////
struct PixelInputType
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};

//////////////////////
////   Pixel Shader
/////////////////////
float4 ColorPixelShader(PixelInputType input) : SV_TARGET
{
	// EX: 5 - Change pixel shader output to half brightness.
	// input.color.g *= 0.5f;
	return input.color;
}