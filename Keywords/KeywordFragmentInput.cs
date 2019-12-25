using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	internal class KeywordFragmentInput : KeywordAttributeStruct
	{
		#region Properties

		protected override string StructName => "Varyings";

		#endregion

		#region Constructors

		public KeywordFragmentInput(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Protected methods

		protected override List<string> GetRequiredPassKeywords(ShaderGenerationContext context)
		{
			return context.CurrentPass.RequiredFragmentKeywords;
		}

		protected override List<AttributeConfig> GetRequiredPassAttributes(ShaderGenerationContext context)
		{
			return context.CurrentPass.RequiredFragmentAttributes;
		}

		#endregion
	}
}