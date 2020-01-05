#pragma target 2.0

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

struct SurfaceData
{
	half3 albedo;
	half3 specular;
	half  metallic;
	half  smoothness;
	half3 normalTS;
	half3 emission;
	half  occlusion;
	half  alpha;
};


half3 SampleNormalFromDefaultBumpMap(float2 uv, half scale = 1.0h)
{
#ifdef __HASETEXTURE_BUMPMAP
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
	outSurfaceData.normalTS = SampleNormalFromDefaultBumpMap(uv);
	outSurfaceData.occlusion = 1;
	outSurfaceData.emission = 0;
}
