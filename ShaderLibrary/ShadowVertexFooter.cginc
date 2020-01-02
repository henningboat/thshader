float3 __normalWS = TransformObjectToWorldDir(input.__ATTRIBUTESNORMAL);

float4 __clipPos = TransformWorldToHClip(ApplyShadowBias(__vertexPositionInputs.positionWS, __normalWS, _LightDirection));

#if UNITY_REVERSED_Z
__clipPos.z = min(__clipPos.z, __clipPos.w * UNITY_NEAR_CLIP_VALUE);
#else
__clipPos.z = max(__clipPos.z, __clipPos.w * UNITY_NEAR_CLIP_VALUE);
#endif
__vertexPositionInputs.positionCS = __clipPos;