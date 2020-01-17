using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader
{
	internal class KeywordLightMode : SingletonKeyword
	{
		#region Properties

		public override string DefaultLineArguments => "";

		#endregion

		#region Constructors

		public KeywordLightMode(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public void Write(ShaderGenerationContext context)
		{
			if (!IsDefault)
			{
				context.WriteLineIndented($"Tags{{\"LightMode\"=\"{FirstLineArguments}\" }}");
			}
		}

		#endregion
	}
}