using System.Collections.Generic;
using System.Linq;

namespace THUtils.THShader.Keywords
{
	internal class KeywordVertexShader : KeywordShaderCode
	{
		#region Constructors

		public KeywordVertexShader(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public override void Write(ShaderBuildContext context)
		{
			context.WriteLine("#define UNITY_SHADER_NO_UPGRADE 1");
			context.WriteLine("v2f vert(appdata v){");
			context.WriteLineIndented("	v2f o = (v2f)0;");

			var vertexInput = context.KeywordMap.GetKeyword<KeywordVertexInput>();
			var fragmentInput = context.KeywordMap.GetKeyword<KeywordFragmentInput>();
			if (vertexInput.HasPositionAttribute())
			{
				string vertexPositionName = vertexInput.GetAttribute(AttributeType.Position).Name;
				string fragmentPositionName = fragmentInput.GetAttribute(AttributeType.Position).Name;
				context.WriteLineIndented($"o.{fragmentPositionName}= mul(UNITY_MATRIX_MVP, v.{vertexPositionName});");
			}

			context.Newine();
			WriteDefaultCode(context);
			context.Newine();

			context.WriteLine(context.CurrentPass.GetVertexShaderHeader());
			context.Newine();

			context.WriteIndented(base.Write);
			context.Newine();

			context.WriteLine(context.CurrentPass.GetVertexShaderFooter());

			context.WriteLineIndented("	return o;");
			context.WriteLine("}");
		}

		#endregion

		#region Private methods

		private void WriteDefaultCode(ShaderBuildContext context)
		{
			var vertexInput = context.KeywordMap.GetKeyword<KeywordVertexInput>();
			var fragmentInput = context.KeywordMap.GetKeyword<KeywordFragmentInput>();

			bool hasDefinedFragmentShader = !IsDefault;

			var attributesToInit = new List<AttributeConfig>();

			var vertexAttributes = vertexInput.GetAttributes();

			foreach (var fragmentAttribute in fragmentInput.GetAttributes())
			{
				if (!fragmentAttribute.UserDefined)
					continue;
				if (hasDefinedFragmentShader)
				{
					if (vertexAttributes.Any(vertexAttribute => fragmentAttribute == vertexAttribute))
					{
						attributesToInit.Add(fragmentAttribute);
					}
				}
				else
				{
					attributesToInit.Add(fragmentAttribute);
				}
			}

			foreach (AttributeConfig config in attributesToInit)
			{
				context.WriteLineIndented($"o.{config.Name} = v.{config.Name};");
			}
		}

		#endregion
	}
}