using System.Collections.Generic;
using UnityEngine;

namespace THUtils.THShader.Passes
{
	// ReSharper disable once UnusedMember.Global
	public class ShaderModelLit : ShaderModel
	{
		#region Properties

		public override List<ShaderModelTexture> OptionalShaderModelTextures => new List<ShaderModelTexture>()
		                                                                        {
			                                                                        new ShaderModelTexture("_BaseMap", Vector4.one),
			                                                                        new ShaderModelTexture("_BumpMap", new Vector4(0, 1, 0)),
		                                                                        };

		public override string SubShaderHeader => "ShaderLibrary/LitHeader.cginc";

		#endregion

		#region Public methods

		public override void GeneratePasses(ShaderGenerationContext context, out ShaderPass mainPass, out List<ShaderPass> additionalPasses)
		{
			mainPass = new ShaderPassLitForward();
			additionalPasses = new List<ShaderPass>()
			                   {
				                   new ShaderPassShadow(),
				                   new ShaderPassDepthOnly(),
				                   new ShaderPassLitMeta(),
			                   };
		}

		#endregion
	}
}