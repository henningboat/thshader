using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader.Passes
{
	public class ShaderPassLitMeta : ShaderPass
	{
		#region Properties

		protected override string LibraryBaseName => "ShaderLibrary/Meta";
		public override string LightMode => "Meta";

		public override List<AttributeConfig> RequiredFragmentAttributes => new List<AttributeConfig>()
		                                                                    {
			                                                                    new AttributeConfig(AttributeType.Position, DataType.float4, "vertex"),
		                                                                    };

		public override List<string> RequiredFragmentKeywords => new List<string>();

		public override List<AttributeConfig> RequiredVertexAttributes => new List<AttributeConfig>()
		                                                                  {
			                                                                  new AttributeConfig(AttributeType.Position, DataType.float4, "vertex"),
			                                                                  new AttributeConfig(AttributeType.Normal, DataType.float3, "normal"),
			                                                                  new AttributeConfig(AttributeType.TexCoord1, DataType.float4, "texcoord1"),
			                                                                  new AttributeConfig(AttributeType.TexCoord2, DataType.float4, "texcoord2"),
		                                                                  };

		public override List<string> RequiredVertexKeywords => new List<string>();

		#endregion
	}
}