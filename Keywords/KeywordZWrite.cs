using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public class KeywordZWrite : KeywordBoolPipelineState
	{
		#region Properties

		public override string DefaultLineArguments => "True";

		#endregion

		#region Constructors

		public KeywordZWrite(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		internal override void Write(ShaderGenerationContext context)
		{
			context.WriteLine($"{Name} True");
		}
	}
}