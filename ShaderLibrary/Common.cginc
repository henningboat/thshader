#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

VertexPositionInputs GetVertexPositionInputsFromWS(float3 positionWS)
{
	VertexPositionInputs input;
	input.positionWS = positionWS;
	input.positionVS = TransformWorldToView(input.positionWS);
	input.positionCS = TransformWorldToHClip(input.positionWS);

	float4 ndc = input.positionCS * 0.5f;
	input.positionNDC.xy = float2(ndc.x, ndc.y * _ProjectionParams.x) + ndc.w;
	input.positionNDC.zw = input.positionCS.zw;

	return input;
}

#define SET_VERTEX_POSITION_WS(positionWS) __vertexPositionInputs = GetVertexPositionInputsFromWS(positionWS); 
#define SET_VERTEX_POSITION_OS(positionOS) __vertexPositionInputs = GetVertexPositionInputs(positionOS);