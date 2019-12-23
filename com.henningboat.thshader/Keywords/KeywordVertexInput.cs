using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	internal class KeywordVertexInput : KeywordAttributeStruct
	{
		#region Properties

		protected override string StructName => "appdata";

		#endregion

		#region Constructors

		public KeywordVertexInput(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public override void Write(ShaderBuildContext context)
		{
			if (IsDefault)
			{
				AddFragmentShaderRequiredAttributes(context);
			}

			base.Write(context);
		}

		public bool HasPositionAttribute()
		{
			return _attributes.ContainsKey(AttributeType.Position);
		}

		#endregion

		#region Protected methods

		protected override List<string> GetRequiredPassKeywords(ShaderBuildContext context)
		{
			return context.CurrentPass.RequiredVertexKeywords;
		}

		protected override List<AttributeConfig> GetRequiredPassAttributes(ShaderBuildContext context)
		{
			return context.CurrentPass.RequiredVertexAttributes;
		}

		#endregion

		#region Private methods

		private void AddFragmentShaderRequiredAttributes(ShaderBuildContext context)
		{
			var fragmentAttributes = context.KeywordMap.GetKeyword<KeywordFragmentInput>().GetAttributes();

			foreach (AttributeConfig fragmentAttribute in fragmentAttributes)
			{
				if (!_attributes.ContainsKey(fragmentAttribute.AttributeType))
				{
					_attributes[fragmentAttribute.AttributeType] = fragmentAttribute;
				}
			}
		}

		#endregion
	}
}