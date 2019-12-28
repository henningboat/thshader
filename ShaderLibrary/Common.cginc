#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

#define SET_VERTEX_POSITION_OS(positionOS) output.vertexPositionInputs = GetVertexPositionInputs(positionOS);