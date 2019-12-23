using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public class KeywordZTest : PipelineStateEnumKeyword
	{
		#region Properties

		public override string DefaultLineArguments => "LEqual";

		#endregion

		#region Constructors

		public KeywordZTest(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}