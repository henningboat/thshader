using System.Collections.Generic;

namespace THUtils.THShader.Passes
{
	// ReSharper disable once UnusedMember.Global
	public class ShaderPassesConfigLit : ShaderPassesConfig
	{
		#region Properties

		public override List<ShaderProperty> DefinedShaderProperties => new List<ShaderProperty>()
		                                                                {
			                                                                new ShaderProperty("_BaseMap"),
			                                                                new ShaderProperty("_BumpMap"),
		                                                                };

		public override string SubShaderHeader => "ShaderLibrary/LitHeader.cginc";

		#endregion

		#region Public methods

		public override List<ShaderPass> GeneratePasses(ShaderBuildContext context)
		{
			return new List<ShaderPass>()
			       {
				       new ShaderPassLitForward(),
				       new ShaderPassShadow(),
				       new ShaderPassDepthOnly(),
				       new ShaderPassLitMeta(),
			       };
		}

		#endregion
	}
}