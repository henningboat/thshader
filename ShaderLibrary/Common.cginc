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

VertexNormalInputs GetVertexNormalInputsWS(float3 normalWS, float4 tangentWS)
{
    VertexNormalInputs tbn;

    // mikkts space compliant. only normalize when extracting normal at frag.
    real sign = tangentWS.w * GetOddNegativeScale();
    tbn.normalWS =normalWS;
    tbn.tangentWS = tangentWS.xyz;
    tbn.bitangentWS = cross(tbn.normalWS, tbn.tangentWS) * sign;
    return tbn;
}


#define SET_VERTEX_POSITION_WS(positionWS) __vertexPositionInputs = GetVertexPositionInputsFromWS(positionWS); 
#define SET_VERTEX_POSITION_OS(positionOS) __vertexPositionInputs = GetVertexPositionInputs(positionOS);
#define SET_NORMAL_TANGENT_WS(normalWS, tangentWS) __normalInput = GetVertexNormalInputsWS(normalWS,tangentWS);