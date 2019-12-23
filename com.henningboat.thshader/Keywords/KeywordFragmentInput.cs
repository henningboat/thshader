using System.Collections;
using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	internal class KeywordFragmentInput : KeywordAttributeStruct
	{
		#region Properties

		protected override string StructName => "v2f";

		#endregion

		#region Constructors

		public KeywordFragmentInput(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Protected methods

		protected override List<string> GetRequiredPassKeywords(ShaderBuildContext context)
		{
			return context.CurrentPass.RequiredFragmentKeywords;
		}

		protected override List<AttributeConfig> GetRequiredPassAttributes(ShaderBuildContext context)
		{
			return context.CurrentPass.RequiredFragmentAttributes;
		}

		#endregion

	}
}