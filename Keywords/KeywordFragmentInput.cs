using System.Collections.Generic;
using System.Linq;

namespace THUtils.THShader.Keywords
{
	internal class KeywordFragmentInput : KeywordAttributeStruct
	{
		#region Properties

		protected override string AttributeStructName => "Varyings";

		#endregion

		#region Constructors

		public KeywordFragmentInput(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public override void Write(ShaderGenerationContext context)
		{
			base.Write(context);
			
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

		protected override void WriteUserDefinedAttributes(ShaderGenerationContext context, List<AttributeConfig> attributesToUse)
		{
			base.WriteUserDefinedAttributes(context, attributesToUse);
			context.WriteLine("VertexPositionInputs vertexPositionInputs;");
		}

		#endregion
	}
}