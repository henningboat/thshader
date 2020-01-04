# thshader

A shader generation makup language designed to make allow writing Universal Renderpipeline compatible shaders without having to handle all the boilerplate code. This project is currently in a very early phase

# how to use
Simply add the following line to your projects Packages/magifest.json file
"com.henningboat/thshader": "https://github.com/henningboat/thshader.git,

# example
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
