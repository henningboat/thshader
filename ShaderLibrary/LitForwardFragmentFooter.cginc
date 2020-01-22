InputData __inputData;
InitializeInputData(input, output.normalTS, __inputData);


half4 __color = UniversalFragmentPBR(__inputData, output.albedo, output.metallic, output.specular, output.smoothness, output.occlusion, output.emission, output.alpha);

__color.rgb = MixFog(__color.rgb, __inputData.fogCoord);