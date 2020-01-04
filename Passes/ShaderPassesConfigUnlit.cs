using System.Collections.Generic;

namespace THUtils.THShader.Passes
{
	// ReSharper disable once UnusedMember.Global
	public class ShaderModelUnlit : ShaderModel
	{
		#region Properties

		#endregion

		#region Public methods

		public override List<ShaderPass> GeneratePasses(ShaderGenerationContext context)
		{
			return new List<ShaderPass>
			       {
				       new ShaderUnlitPass()
			       };
		}

		#endregion
	}
}