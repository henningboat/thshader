HLSLINCLUDE
#pragma target 2.0
#define _NORMALMAP
#define BUMP_SCALE_NOT_SUPPORTED 1

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

ENDHLSL