using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	internal class KeywordFragmentInput : KeywordAttributeStruct
	{
		#region Properties

		public override string AttributeStructName => "Varyings";

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
			if (GetAttribute(AttributeType.Position).UserDefined)
			{
				throw new KeywordMap.ShaderGenerationException("You are not supposed to manipulate the clip space position manually. Use SETPOSITIONOS(float3) or SETPOSITIONWS(float3) instead to ensure compatibility with all templates");
			}
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