using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	internal class KeywordFragmentShader : KeywordShaderCode
	{
		#region Constructors

		public KeywordFragmentShader(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public override void Write(ShaderGenerationContext context)
		{
			var vertexInput = context.KeywordMap.GetKeyword<KeywordVertexInput>();
			var fragmentInput = context.KeywordMap.GetKeyword<KeywordFragmentInput>();

			context.WriteLine($"void ExecuteUserFragmentCode({fragmentInput.UserStructName} input, inout UserFragmentOutput output)");
			context.WriteLine("{");
			context.WriteIndented(base.Write);
			context.WriteLine("}");

			//todo it migh be more performant to only output a float3 for opaque shaders
			context.WriteLine("float4 frag(Varyings input) : COLOR{");
			context.WriteLineIndented("UserVaryings userInput = (UserVaryings)0;");
			context.WriteLineIndented("InitializeUserVaryings(input, userInput);");

			context.WriteIndented(WriteFragmentShaderHeader);

			context.WriteLineIndented("ExecuteUserFragmentCode(userInput, userOutput);");

			context.WriteIndented(WriteFragmentShaderFooter);

			context.WriteLine("}");
		}

		#endregion

		#region Private methods

		private void WriteFragmentShaderFooter(ShaderGenerationContext context)
		{
			context.WriteLine(context.CurrentPass.GetFragmentShaderFooter());
			context.Newine();
		}

		private void WriteFragmentShaderHeader(ShaderGenerationContext context)
		{
			context.WriteLine(context.CurrentPass.GetFragmentShaderHeader());
			context.Newine();
		}

		#endregion
	}
}