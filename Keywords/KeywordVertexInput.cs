using System.Collections.Generic;
using System.Linq;

namespace THUtils.THShader.Keywords
{
	internal class KeywordVertexInput : KeywordAttributeStruct
	{
		#region Properties

		protected override string AttributeStructName => "Attributes";

		#endregion

		#region Constructors

		public KeywordVertexInput(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public override void Write(ShaderGenerationContext context)
		{
			if (IsDefault)
			{
				AddFragmentShaderRequiredAttributes(context);
			}

			if (_attributes.Any(pair => pair.AttributeType == AttributeType.Anonymous))
			{
				throw new KeywordMap.ShaderGenerationException("Can't Vertex Attribute needs an explicit AttributeType");
			}

			base.Write(context);
		}

		public bool HasPositionAttribute()
		{
			return _attributes.Any(config => config.AttributeType == AttributeType.Position);
		}

		#endregion

		#region Protected methods

		protected override List<string> GetRequiredPassKeywords(ShaderGenerationContext context)
		{
			return context.CurrentPass.RequiredVertexKeywords;
		}

		protected override List<AttributeConfig> GetRequiredPassAttributes(ShaderGenerationContext context)
		{
			return context.CurrentPass.RequiredVertexAttributes;
		}

		#endregion

		#region Private methods

		private void AddFragmentShaderRequiredAttributes(ShaderGenerationContext context)
		{
			var fragmentAttributes = context.KeywordMap.GetKeyword<KeywordFragmentInput>().GetAttributes();

			foreach (AttributeConfig fragmentAttribute in fragmentAttributes)
			{
				if (fragmentAttribute.AttributeType == AttributeType.Anonymous)
				{
					continue;
				}
			}
		}

		#endregion
	}
}