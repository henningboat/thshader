InputData inputData;
InitializeInputData(i, o.normalTS, inputData);


half4 color = UniversalFragmentPBR(inputData, o.albedo, o.metallic, o.specular, o.smoothness, o.occlusion, o.emission, o.alpha);

color.rgb = MixFog(color.rgb, inputData.fogCoord);
return color;