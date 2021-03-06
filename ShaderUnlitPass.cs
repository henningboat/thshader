using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader
{
	internal class ShaderUnlitPass : ShaderPass
	{
		#region Properties

		public override bool IsMainPass => true;
		protected override string LibraryBaseName => "Packages/com.henningboat.thshader/ShaderLibrary/Unlit";
		public override string LightMode => null;

		public override List<AttributeConfig> RequiredFragmentAttributes =>
			new List<AttributeConfig>()
			{
				new AttributeConfig(AttributeType.Position, DataType.@float, 4, "positionCS")
			};

		public override List<string> RequiredFragmentKeywords => new List<string>();

		public override List<AttributeConfig> RequiredVertexAttributes =>
			new List<AttributeConfig>()
			{
				new AttributeConfig(AttributeType.Position, DataType.@float, 4, "positionOS")
			};

		public override List<string> RequiredVertexKeywords => new List<string>();
		protected override string VertexShaderFooterPath => null;
		protected override string VertexShaderHeaderPath => null;

		#endregion
	}
}