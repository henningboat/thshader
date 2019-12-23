BRDFData brdfData;
InitializeBRDFData(o.albedo, o.metallic, o.specular, o.smoothness, o.alpha, brdfData);

MetaInput metaInput;
metaInput.Albedo = brdfData.diffuse + brdfData.specular * brdfData.roughness * 0.5;
metaInput.SpecularColor = o.specular;
metaInput.Emission = o.emission;

return MetaFragment(metaInput);