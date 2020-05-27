using System.Collections.Generic;

namespace THUtils.THShader.Passes
{
	// ReSharper disable once UnusedMember.Global
	public class ShaderModelUnlit : ShaderModel
	{
		#region Properties

		#endregion

		#region Public methods


		#endregion

		public override void GeneratePasses(ShaderGenerationContext context, out ShaderPass mainPass, out List<ShaderPass> additionalPasses)
		{
			mainPass = new ShaderUnlitPass();
			additionalPasses = new List<ShaderPass>();
		}
	}
}