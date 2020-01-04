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

		public override List<ShaderPass> GeneratePasses(ShaderGenerationContext context)
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