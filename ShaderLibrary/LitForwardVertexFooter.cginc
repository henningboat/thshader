VertexNormalInputs normalInput = GetVertexNormalInputs(input.__ATTRIBUTESNORMAL, input.__ATTRIBUTESTANGENT);
half3 __viewDirWS = GetCameraPositionWS() - __vertexPositionInputs.positionWS;
half3 __vertexLight = VertexLighting(__vertexPositionInputs.positionWS, normalInput.normalWS);
half fogFactor = ComputeFogFactor(__vertexPositionInputs.positionCS.z);

output.normalWS = normalInput.normalWS;
output.tangentWS = normalInput.tangentWS;
output.bitangentWS = normalInput.bitangentWS;
output.viewDirWS = __viewDirWS;

OUTPUT_LIGHTMAP_UV(input.__ATTRIBUTESTEXCOORD1, unity_LightmapST, output.lightmapUV);
OUTPUT_SH(output.normalWS.xyz, output.vertexSH);

output.fogFactorAndVertexLight = half4(fogFactor, __vertexLight);

#ifdef _ADDITIONAL_LIGHTS
output.positionWS = __vertexPositionInputs.positionWS;
#endif

#if defined(_MAIN_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
output.shadowCoord = GetShadowCoord(__vertexPositionInputs);
#endif