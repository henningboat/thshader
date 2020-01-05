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

			context.LogShaderSection("Vertex Shader");

			context.WriteIndented(WriteInnerVertexShader);

			context.WriteLine("}");
		}

		private void WriteInnerVertexShader(ShaderGenerationContext context)
		{
			context.WriteLine("Varyings vert(Attributes input){");
			context.WriteLine("	Varyings output = (Varyings)0;");

			context.WriteLine("VertexPositionInputs __vertexPositionInputs = GetVertexPositionInputs(input.__ATTRIBUTESPOSITION);");

			InitializeVaryingsWithDefaultValues(context);

			context.WriteLine(context.CurrentPass.GetVertexShaderHeader());

			base.Write(context);

			context.WriteLine(context.CurrentPass.GetVertexShaderFooter());

			context.WriteLine("output.__VARYINGSPOSITION = __vertexPositionInputs.positionCS;");

			context.WriteLine("	return output;");
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

		private void InitializeVaryingsWithDefaultValues(ShaderGenerationContext context)
		{
			var vertexInput = context.KeywordMap.GetKeyword<KeywordVertexInput>();
			var fragmentInput = context.KeywordMap.GetKeyword<KeywordFragmentInput>();

			bool hasDefinedFragmentShader = !IsDefault;

			var attributesToInit = new List<AttributeConfig>();

			var vertexAttributes = vertexInput.GetAttributes();

			foreach (var fragmentAttribute in fragmentInput.GetAttributes())
			{
				if (fragmentAttribute.AttributeType == AttributeType.Anonymous || fragmentAttribute.AttributeType == AttributeType.Position)
					continue;

				if (vertexAttributes.Any(vertexAttribute => fragmentAttribute == vertexAttribute))
				{
					attributesToInit.Add(fragmentAttribute);
				}
			}

			foreach (AttributeConfig config in attributesToInit)
			{
				context.WriteLineIndented($"output.__VARYINGS{config.AttributeType.ToString().ToUpper()} = input.__ATTRIBUTES{config.AttributeType.ToString().ToUpper()};");
			}
		}

		#endregion
	}
}