using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader
{
	internal class KeywordLightMode : SingletonKeyword
	{
		#region Properties

		public override string DefaultLineArguments => "";
		public string OverwriteLightMode => IsDefault ? null : FirstLineArguments;

		#endregion

		#region Constructors

		public KeywordLightMode(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}