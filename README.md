# thshader

A shader generation makup language designed to make allow writing Universal Renderpipeline compatible shaders without having to handle all the boilerplate code. This project is currently in a very early phase

# conceps
## keywords
A thshader file is essetially a list of keywords, followed by arguments. For example, you can write _Cull Off_ to disable culling. Some keywords, like VertexInput, VertexShader, FragmentInput and FragmentShader support multi line arguments.
## everything has a default behaviour
The idea behind this language is that every keyword has a default value. If you don't use the _Cull_ keyword, code generator will internally treat it as if you would have written _Cull Back_.
Because of this, an empty string will always be compiled into a working shader
## writing vertex shaders is optional
if you define a FragmentInput with an attribute, for example, _float2 lightmapUV TEXCOORD1_, the auto generated vertex shader will automatically populate it. Because of this, in most cases you will only have to write a fragment shader, unless you wan't to do vertex animmations.

# how to use
Simply add the following line to your projects Packages/magifest.json file
```
"com.henningboat/thshader": "https://github.com/henningboat/thshader.git,
```

# example
```
ShaderModel Lit

Property Texture _Texture

VertexInput
float2 uv Texcoord0
float4 positionOS POSITION

VertexShader
output.uv = input.uv;  
SET_VERTEX_POSITION_OS(lerp(input.positionOS, float3(input.uv.x - 0.5,0,input.uv.y-0.5), saturate(frac(_Time.y*0.5)*3-1)));
ENDCG

FragmentInput
float2 uv

FragmentShader
output.albedo = tex2D(_Texture, input.uv);
ENDCG
```
