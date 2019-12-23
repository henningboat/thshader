using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public abstract class SingletonKeyword : Keyword
	{
		#region Constructors

		protected SingletonKeyword(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}