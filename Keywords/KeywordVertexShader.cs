using System.Collections.Generic;
using System.Linq;

// ReSharper disable StringLiteralTypo
namespace THUtils.THShader.Keywords
{
	internal class KeywordVertexShader : KeywordShaderCode
	{
		#region Properties

		public bool ModifiesVertexPosition { get; private set; }

		#endregion

		#region Constructors

		public KeywordVertexShader(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public override void Write(ShaderGenerationContext context)
		{
			context.WriteLine("#define UNITY_SHADER_NO_UPGRADE 1");

			var vertexInput = context.KeywordMap.GetKeyword<KeywordVertexInput>();
			var fragmentInput = context.KeywordMap.GetKeyword<KeywordFragmentInput>();

			context.Newine();

			context.WriteLine("Varyings vert(Attributes input){");
			context.WriteLine("	Varyings output = (Varyings)0;");

			context.WriteLine("VertexPositionInputs __vertexPositionInputs = GetVertexPositionInputs(input.__ATTRIBUTESPOSITION);");

			context.WriteLine(context.CurrentPass.GetVertexShaderHeader());

			base.Write(context);

			context.WriteLine(context.CurrentPass.GetVertexShaderFooter());

			context.WriteLine("output.__VARYINGSPOSITION = __vertexPositionInputs.positionCS;");

			context.WriteLine("	return output;");
			context.WriteLine("}");
		}

		#endregion

		#region Protected methods

		protected override void HandleMultiLineKeyword(Queue<string> lines)
		{
			base.HandleMultiLineKeyword(lines);
			ModifiesVertexPosition = _parsedCode.Contains("SET_VERTEX_POSITION");
		}

		#endregion

		#region Private methods

		private void WriteDefaultCode(ShaderGenerationContext context)
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