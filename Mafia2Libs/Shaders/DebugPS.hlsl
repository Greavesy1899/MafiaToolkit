//////////////////////
////   TYPES
//////////////////////
struct VS_OUTPUT
{
	float4 Position : SV_POSITION;
    float4 Colour : Colour;
};

float4 DebugPixelShader(VS_OUTPUT input) : SV_TARGET
{
    return float4(input.Colour.r, input.Colour.g, input.Colour.b, input.Colour.w);
}