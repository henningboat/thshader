float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
float3 normalWS = TransformObjectToWorldDir(v.normal);

float4 clipPos = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));

#if UNITY_REVERSED_Z
clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
#else
clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
#endif
o.vertex = clipPos;

return o;