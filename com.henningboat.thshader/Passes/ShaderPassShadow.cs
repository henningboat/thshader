using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader.Passes
{
	public class ShaderPassShadow : ShaderPass
	{
		#region Properties

		protected override string LibraryBaseName => "ShaderLibrary/Shadow";
		public override string LightMode => "ShadowCaster";

		public override List<AttributeConfig> RequiredFragmentAttributes => new List<AttributeConfig>()
		                                                                    {
			                                                                    new AttributeConfig(AttributeType.Position, DataType.float4, "vertex"),
			                                                                    new AttributeConfig(AttributeType.Normal, DataType.float3, "normal"),
		                                                                    };

		public override List<string> RequiredFragmentKeywords => new List<string>();

		public override List<AttributeConfig> RequiredVertexAttributes => new List<AttributeConfig>()
		                                                                  {
			                                                                  new AttributeConfig(AttributeType.Position, DataType.float4, "vertex"),
			                                                                  new AttributeConfig(AttributeType.Normal, DataType.float3, "normal"),
		                                                                  };

		public override List<string> RequiredVertexKeywords => new List<string>();
		protected override string UsePassName => "Universal Render Pipeline/Lit/ShadowCaster";

		#endregion
	}
}