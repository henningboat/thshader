using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public abstract class MultiKeyword : Keyword
	{
		#region Constructors

		protected MultiKeyword(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}