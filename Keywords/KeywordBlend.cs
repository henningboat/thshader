using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public class KeywordBlend : PipelineStateEnumKeyword
	{
		#region Properties

		public override string DefaultLineArguments => "One Zero";

		#endregion

		#region Constructors

		public KeywordBlend(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}