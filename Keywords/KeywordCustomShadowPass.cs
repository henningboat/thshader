using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	//todo allow explicitly disabling passes
	internal class KeywordCustomShadowPass : SingletonKeyword
	{
		#region Public Fields

		public readonly bool CustomShadowPass;

		#endregion

		#region Properties

		public override string DefaultLineArguments => "False";

		#endregion

		#region Constructors

		public KeywordCustomShadowPass(Queue<string> sourceCode) : base(sourceCode)
		{
			CustomShadowPass = bool.Parse(FirstLineArguments.ToLower());
		}

		#endregion
	}
}