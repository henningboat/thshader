using System.Collections.Generic;

namespace THUtils.THShader.Passes
{
	// ReSharper disable once UnusedMember.Global
	public class ShaderPassesConfigUnlit : ShaderPassesConfig
	{
		#region Properties

		public override List<ShaderProperty> DefinedShaderProperties => new List<ShaderProperty>();

		#endregion

		#region Public methods

		public override List<ShaderPass> GeneratePasses(ShaderBuildContext context)
		{
			return new List<ShaderPass>
			       {
				       new ShaderUnlitPass()
			       };
		}

		#endregion
	}
}