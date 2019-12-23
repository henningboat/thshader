using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	//todo allow explicitly disabling passes
	internal class KeywordCustomShadowPass : KeywordPasses
	{
		#region Properties

		public bool CustomShadowPass
		{
			get
			{
				if (IsDefault)
				{
					return false;
				}
				else
				{
					return FirstLineArguments == "True";
				}
			}
		}

		#endregion

		#region Constructors

		public KeywordCustomShadowPass(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion
	}
}