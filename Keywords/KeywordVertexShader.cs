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

		public override void Write(ShaderGenerationContext context)
		{
			context.WriteLine("#define UNITY_SHADER_NO_UPGRADE 1");


			var vertexInput = context.KeywordMap.GetKeyword<KeywordVertexInput>();
			var fragmentInput = context.KeywordMap.GetKeyword<KeywordFragmentInput>();

			context.WriteLine($"void ExecuteUserVertexCode({vertexInput.UserStructName} input, inout {fragmentInput.UserStructName} output)");
			context.WriteLine("{");
			context.WriteIndented(base.Write);
			context.WriteLine("}");

			context.Newine();

			context.WriteLine("Varyings vert(Attributes input){");
			context.WriteLineIndented("	Varyings output = (Varyings)0;");

			context.WriteLineIndented($"{vertexInput.UserStructName} userInput = Initialize{vertexInput.UserStructName}(input);");
			context.WriteLineIndented($"{fragmentInput.UserStructName} userOutput = ({fragmentInput.UserStructName})0;");

			//context.Newine();
			//WriteDefaultCode(context);
			//context.Newine();

			context.WriteLine(context.CurrentPass.GetVertexShaderHeader());
			context.Newine();

			context.WriteLineIndented("ExecuteUserVertexCode(userInput, userOutput);");

			context.WriteLine(context.CurrentPass.GetVertexShaderFooter());

			context.WriteLineIndented("	return output;");
			context.WriteLine("}");
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