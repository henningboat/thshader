using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader
{
	internal class KeywordName : SingletonKeyword
	{
		#region Properties

		public override string DefaultLineArguments => "Hidden/NoName";
		public string ShaderName => FirstLineArguments;

		#endregion

		#region Constructors

		public KeywordName(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}