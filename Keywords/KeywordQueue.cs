using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public class KeywordQueue : SingletonKeyword
	{
		public KeywordQueue(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		public override string DefaultLineArguments => "Geometry";

		public void Write(ShaderGenerationContext context)
		{
			context.WriteLine($"Tags{{\"Queue\" = \"{FirstLineArguments}\"}}");
		}
	}
}