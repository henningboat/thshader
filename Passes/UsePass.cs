using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader.Passes
{
	public class UsePass : ShaderPass
	{
		#region Private Fields

		private readonly string _passName;

		#endregion

		#region Properties

		protected override string FragmentShaderFooterPath { get; }
		protected override string FragmentShaderHeaderPath { get; }
		public override bool IsMainPass => false;
		protected override string LibraryBaseName { get; }
		public override string LightMode { get; }
		public override List<AttributeConfig> RequiredFragmentAttributes { get; }
		public override List<string> RequiredFragmentKeywords { get; }
		public override List<AttributeConfig> RequiredVertexAttributes { get; }
		public override List<string> RequiredVertexKeywords { get; }
		protected override string ShaderHeaderPath { get; }
		protected override string VertexShaderFooterPath { get; }
		protected override string VertexShaderHeaderPath { get; }

		#endregion

		#region Constructors

		public UsePass(string passName)
		{
			_passName = passName;
		}

		#endregion

		internal override void WritePass(ShaderGenerationContext context, ShaderModel config, SourceMap.ShaderPassSource passSpecificCode)
		{
			context.WriteLine($"UsePass \"{_passName}\"");
		}
	}
}