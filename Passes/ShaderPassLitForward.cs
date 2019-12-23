using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader.Passes
{
	public class ShaderPassLitForward : ShaderPass
	{
		#region Properties

		protected override string LibraryBaseName => "ShaderLibrary/LitForward";
		public override string LightMode => "UniversalForward";

		public override List<AttributeConfig> RequiredFragmentAttributes =>
			new List<AttributeConfig>()
			{
				new AttributeConfig(AttributeType.TexCoord0, DataType.float4, "texcoord0"),
				new AttributeConfig(AttributeType.TexCoord2, DataType.float3, "positionWS"),
				new AttributeConfig(AttributeType.Normal, DataType.float4, "normalWS"), 
				new AttributeConfig(AttributeType.TexCoord4, DataType.float4, "tangentWS"),
				new AttributeConfig(AttributeType.TexCoord5, DataType.float4, "bitangentWS"),
				new AttributeConfig(AttributeType.TexCoord6, DataType.float4, "fogFactorAndVertexLight"),
				new AttributeConfig(AttributeType.TexCoord7, DataType.float4, "shadowCoord"),
				new AttributeConfig(AttributeType.Tangent, DataType.float3, "viewDirWS"),
				new AttributeConfig(AttributeType.Position, DataType.float4, "positionCS"),
			};

		public override List<string> RequiredFragmentKeywords => new List<string>()
		                                                         {
			                                                         "DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);",
			                                                         "UNITY_VERTEX_INPUT_INSTANCE_ID",
			                                                         "UNITY_VERTEX_OUTPUT_STEREO"
		                                                         };

		public override List<AttributeConfig> RequiredVertexAttributes =>
			new List<AttributeConfig>()
			{
				new AttributeConfig(AttributeType.Position, DataType.float4, "vertex"),
				new AttributeConfig(AttributeType.Normal, DataType.float3, "normalOS"),
				new AttributeConfig(AttributeType.Tangent, DataType.float4, "tangentOS"),
				new AttributeConfig(AttributeType.TexCoord0, DataType.float4, "texcoord0"),
				new AttributeConfig(AttributeType.TexCoord1, DataType.float4, "lightmapUV"),
			};

		public override List<string> RequiredVertexKeywords => new List<string>()
		                                                       {
			                                                       "UNITY_VERTEX_INPUT_INSTANCE_ID"
		                                                       };

		#endregion
	}
}