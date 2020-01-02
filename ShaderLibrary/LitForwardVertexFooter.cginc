VertexNormalInputs normalInput = GetVertexNormalInputs(input.__ATTRIBUTESNORMAL, input.__ATTRIBUTESTANGENT);
half3 __viewDirWS = GetCameraPositionWS() - __vertexPositionInputs.positionWS;
half3 __vertexLight = VertexLighting(__vertexPositionInputs.positionWS, normalInput.normalWS);
half fogFactor = ComputeFogFactor(__vertexPositionInputs.positionCS.z);

#ifdef _NORMALMAP
output.normalWS = half4(normalInput.normalWS, __viewDirWS.x);
output.tangentWS = half4(normalInput.tangentWS, __viewDirWS.y);
output.bitangentWS = half4(normalInput.bitangentWS, __viewDirWS.z);
#else
output.normalWS.xyz = NormalizeNormalPerVertex(normalInput.normalWS.xyz);
output.viewDirWS = __viewDirWS.xyz;
#endif

OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

output.fogFactorAndVertexLight = half4(fogFactor, __vertexLight);

#ifdef _ADDITIONAL_LIGHTS
output.positionWS = __vertexPositionInputs.positionWS;
#endif

#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
output.shadowCoord = GetShadowCoord(__vertexPositionInputs);
#endif