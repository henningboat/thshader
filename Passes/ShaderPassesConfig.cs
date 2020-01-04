using System.Collections.Generic;
using THUtils.THShader.Keywords;

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

		public abstract List<ShaderPass> GeneratePasses(ShaderGenerationContext context);

		public void Write(ShaderGenerationContext context)
		{
			//todo workaround for now
			context.KeywordMap.GetKeyword<KeywordHasNormalMap>().Write(context);

			context.WriteLine("HLSLINCLUDE");
			context.WriteLine("#include \"Packages/com.henningboat.thshader/ShaderLibrary/Common.cginc\"");

			foreach (ShaderModelTexture modelTexture in OptionalShaderModelTextures)
			{
				modelTexture.Write(context);
			}
			context.WriteLine(ShaderPass.ReadSourceFile(context, SubShaderHeader));

            context.WriteLine("ENDHLSL");


			foreach (ShaderPass pass in GeneratePasses(context))
			{
				context.WriteIndented(buildContext => pass.WritePass(buildContext, this));
			}
		}

		#endregion
	}
}