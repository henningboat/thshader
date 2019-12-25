using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public abstract class KeywordBoolPipelineState : PipelineStateKeyword
	{
		#region Constructors

		protected KeywordBoolPipelineState(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}