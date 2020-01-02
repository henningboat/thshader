using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public class KeywordDebugMode : SingletonKeyword
	{
		#region Properties

		public override string DefaultLineArguments { get; }
		public bool IsDebug => !IsDefault;

		#endregion

		#region Constructors

		public KeywordDebugMode(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}