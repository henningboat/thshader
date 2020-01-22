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
			context.LogShaderSection("Fragment Shader");

			//todo it migh be more performant to only output a float3 for opaque shaders
			context.WriteLine("float4 frag(Varyings input) : COLOR{");

			context.WriteIndented(WriteFragmentShaderHeader);

			base.Write(context);

			context.WriteIndented(WriteFragmentShaderFooter);

			context.KeywordMap.GetKeyword<KeywordModifyFinalColor>().Write(context);

			context.WriteLineIndented("return __color;");

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