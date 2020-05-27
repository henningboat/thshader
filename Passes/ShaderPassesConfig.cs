using System.Collections;
using System.Collections.Generic;
using THUtils.THShader.Keywords;
using UnityEngine;

namespace THUtils.THShader.Passes
{
	public abstract class ShaderModel
	{
		#region Properties

		public virtual List<ShaderModelTexture> OptionalShaderModelTextures => new List<ShaderModelTexture>(0);
		public virtual string SubShaderHeader => null;

		#endregion

		#region Constructors

		public ShaderModel()
		{
		}

		#endregion

		#region Public methods

		public abstract void GeneratePasses(ShaderGenerationContext context, out ShaderPass mainPass, out List<ShaderPass> additionalPasses);

		public void Write(ShaderGenerationContext context)
		{
			context.KeywordMap.GetKeyword<KeywordQueue>().Write(context);
			//todo workaround for now
			context.KeywordMap.GetKeyword<KeywordHasNormalMap>().Write(context);

			context.WriteLine("HLSLINCLUDE");

			context.WriteLine("#include \"Packages/com.henningboat.thshader/ShaderLibrary/Common.cginc\"");

			foreach (ShaderModelTexture modelTexture in OptionalShaderModelTextures)
			{
				modelTexture.Write(context);
			}

			context.KeywordMap.GetMultiKeywords<KeywordDefine>().ForEach(keyword => keyword.Write(context));
			context.KeywordMap.GetMultiKeywords<KeywordProperty>().ForEach(keyword => keyword.Write(context, false));

			context.WriteLine(ShaderPass.ReadSourceFile(context, SubShaderHeader));

			context.WriteLine("ENDHLSL");

			GeneratePasses(context, out ShaderPass mainPass, out List<ShaderPass> additionalPasses);

			if (context.SourceMap.CustomPasses.Count > 0)
			{
				foreach (SourceMap.ShaderPassSource customPass in context.SourceMap.CustomPasses)
				{
					context.WriteIndented(buildContext => mainPass.WritePass(buildContext, this, customPass));
				}
			}
			else
			{
				context.WriteIndented(buildContext => mainPass.WritePass(buildContext, this, null));
			}

			foreach (ShaderPass pass in additionalPasses)
			{
				context.WriteIndented(buildContext => pass.WritePass(buildContext, this, null));
			}
		}

		#endregion
	}
}