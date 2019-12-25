using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	internal class KeywordHasNormalMap : SingletonKeyword
	{
		#region Properties

		public override string DefaultLineArguments => "";

		#endregion

		#region Constructors

		public KeywordHasNormalMap(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public void Write(ShaderGenerationContext context)
		{
			if (!IsDefault)
			{
				context.WriteLine("HLSLINCLUDE");
				context.WriteLine("#define _NormalMap");
				context.WriteLine("ENDHLSL");
			}
		}

		#endregion
	}
}