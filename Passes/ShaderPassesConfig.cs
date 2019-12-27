using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader.Passes
{
	public abstract class ShaderPassesConfig
	{
		#region Properties

		public abstract List<ShaderProperty> DefinedShaderProperties { get; }
		public virtual string SubShaderHeader => null;

		#endregion

		#region Constructors

		public ShaderPassesConfig()
		{
		}

		#endregion

		#region Public methods

		public abstract List<ShaderPass> GeneratePasses(ShaderGenerationContext context);

		public void Write(ShaderGenerationContext context)
		{
			//todo workaround for now
			context.KeywordMap.GetKeyword<KeywordHasNormalMap>().Write(context);

			context.WriteLine("\r\n#include \"Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl\"");

			if (SubShaderHeader != null)
			{
				context.WriteLine(ShaderPass.ReadSourceFile(SubShaderHeader));
			}

			foreach (ShaderPass pass in GeneratePasses(context))
			{
				context.WriteIndented(buildContext => pass.WritePass(buildContext, this));
			}
		}

		#endregion
	}

	//todo add datatype
}