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
				new AttributeConfig(AttributeType.Anonymous, DataType.@float, 4, "texcoord0"),
				new AttributeConfig(AttributeType.Anonymous, DataType.@float, 3, "positionWS"),
				new AttributeConfig(AttributeType.Anonymous, DataType.@float, 4, "normalWS"),
				new AttributeConfig(AttributeType.Anonymous, DataType.@float, 4, "tangentWS"),
				new AttributeConfig(AttributeType.Anonymous, DataType.@float, 4, "bitangentWS"),
				new AttributeConfig(AttributeType.Anonymous, DataType.@float, 4, "fogFactorAndVertexLight"),
				new AttributeConfig(AttributeType.Anonymous, DataType.@float, 4, "shadowCoord"),
				new AttributeConfig(AttributeType.Anonymous, DataType.@float, 3, "viewDirWS"),
				new AttributeConfig(AttributeType.Position, DataType.@float, 4, "positionCS"),
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
				new AttributeConfig(AttributeType.Position, DataType.@float, 4, "vertex"),
				new AttributeConfig(AttributeType.Normal, DataType.@float, 3, "normalOS"),
				new AttributeConfig(AttributeType.Tangent, DataType.@float, 4, "tangentOS"),
				new AttributeConfig(AttributeType.TexCoord0, DataType.@float, 2, "texcoord0"),
				new AttributeConfig(AttributeType.TexCoord1, DataType.@float, 2, "lightmapUV"),
			};

		public override List<string> RequiredVertexKeywords => new List<string>()
		                                                       {
			                                                       "UNITY_VERTEX_INPUT_INSTANCE_ID"
		                                                       };

		#endregion
	}
}