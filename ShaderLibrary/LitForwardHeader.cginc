#pragma prefer_hlslcc gles
#pragma exclude_renderers d3d11_9x

#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile _ _SHADOWS_SOFT
#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

#pragma multi_compile _ DIRLIGHTMAP_COMBINED
#pragma multi_compile _ LIGHTMAP_ON

#pragma multi_compile_fog


void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
{
	inputData = (InputData)0;

#ifdef _ADDITIONAL_LIGHTS
	inputData.positionWS = input.positionWS;
#endif

	half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
	inputData.normalWS = TransformTangentToWorld(normalTS,
		half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz));

	inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
	viewDirWS = SafeNormalize(viewDirWS);

	inputData.viewDirectionWS = viewDirWS;
#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
	inputData.shadowCoord = input.shadowCoord;
#else
	inputData.shadowCoord = float4(0, 0, 0, 0);
#endif
	inputData.fogCoord = input.fogFactorAndVertexLight.x;
	inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
	inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
}

half3 SampleNormalFromDefaultBumpMap(float2 uv, half scale = 1.0h)
{
#if __HASETEXTURE_BUMPMAP
	half4 n = __SAMPLETEXTURE_BUMPMAP(uv);
	return UnpackNormal(n);
#else
	return half3(0, 0, 1);
#endif
}

void InitializeLitSurfaceData(float2 uv, out SurfaceData outSurfaceData) {
	half4 albedoAlpha = __SAMPLETEXTURE_BASEMAP(uv);
	outSurfaceData.alpha = albedoAlpha.a;

	outSurfaceData.albedo = albedoAlpha.rgb;

	outSurfaceData.metallic = 0;
	outSurfaceData.specular = 0;
	outSurfaceData.smoothness = 0.5;
	//outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
	outSurfaceData.normalTS = SampleNormalFromDefaultBumpMap(uv);
	outSurfaceData.occlusion = 1;
	outSurfaceData.emission = 0;
}
