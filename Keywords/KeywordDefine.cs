using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	internal class KeywordDefine : MultiKeyword
	{
		#region Properties

		public override string DefaultLineArguments => "";

		#endregion

		#region Constructors

		public KeywordDefine(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public void Write(ShaderGenerationContext context)
		{
			context.WriteLine($"#define {FirstLineArguments}");
		}

		#endregion
	}
}