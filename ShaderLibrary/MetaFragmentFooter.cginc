BRDFData __brdfData;
InitializeBRDFData(output.albedo, output.metallic, output.specular, output.smoothness, output.alpha, __brdfData);

MetaInput __metaInput;
__metaInput.Albedo = __brdfData.diffuse + __brdfData.specular * __brdfData.roughness * 0.5;
__metaInput.SpecularColor = output.specular;
__metaInput.Emission = output.emission;

float4 __color = MetaFragment(__metaInput);