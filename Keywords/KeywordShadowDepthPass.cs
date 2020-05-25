using System;
using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	//todo allow explicitly disabling passes
	internal class KeywordShadowDepthPass : SingletonKeyword
	{
		#region Public Fields

		public readonly ShadowDepthPassMode Mode;

		#endregion

		#region Properties

		public override string DefaultLineArguments => "DefaultPass";

		#endregion

		#region Constructors

		public KeywordShadowDepthPass(Queue<string> sourceCode) : base(sourceCode)
		{
			Mode = (ShadowDepthPassMode) Enum.Parse(typeof(ShadowDepthPassMode), FirstLineArguments, true);
		}

		public enum ShadowDepthPassMode
		{
			Off,
			On,
			DefaultPass
		}

		#endregion
	}
}