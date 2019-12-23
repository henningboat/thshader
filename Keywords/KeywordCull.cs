using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	// ReSharper disable once UnusedMember.Global
	public class KeywordCull : PipelineStateEnumKeyword
	{
		#region Properties

		public override string DefaultLineArguments => "Back";

		#endregion

		#region Constructors

		public KeywordCull(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}