using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public class KeywordReceiveShadows : SingletonKeyword
	{
		#region Public Fields

		public readonly bool ReceiveShadows;

		#endregion

		#region Properties

		public override string DefaultLineArguments => "true";

		#endregion

		#region Constructors

		public KeywordReceiveShadows(Queue<string> sourceCode) : base(sourceCode)
		{
			ReceiveShadows = bool.Parse(FirstLineArguments.ToLower());
		}

		#endregion
	}
}