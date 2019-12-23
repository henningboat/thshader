VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
VertexNormalInputs normalInput = GetVertexNormalInputs(v.normalOS, v.tangentOS);
half3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
half3 vertexLight = VertexLighting(vertexInput.positionWS, normalInput.normalWS);
half fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

o.texcoord0 = v.texcoord0;

#ifdef _NORMALMAP
o.normalWS = half4(normalInput.normalWS, viewDirWS.x);
o.tangentWS = half4(normalInput.tangentWS, viewDirWS.y);
o.bitangentWS = half4(normalInput.bitangentWS, viewDirWS.z);
#else
o.normalWS.xyz = NormalizeNormalPerVertex(normalInput.normalWS.xyz);
o.viewDirWS = viewDirWS.xyz;
#endif

OUTPUT_LIGHTMAP_UV(v.lightmapUV, unity_LightmapST, o.lightmapUV);
OUTPUT_SH(o.normalWS.xyz, o.vertexSH);

o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

#ifdef _ADDITIONAL_LIGHTS
o.positionWS = vertexInput.positionWS;
#endif

#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
o.shadowCoord = GetShadowCoord(vertexInput);
#endif

o.positionCS = vertexInput.positionCS;

return o;