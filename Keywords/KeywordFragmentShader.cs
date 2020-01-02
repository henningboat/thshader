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
			
			//todo it migh be more performant to only output a float3 for opaque shaders
			context.WriteLine("float4 frag(Varyings input) : COLOR{");

			context.WriteIndented(WriteFragmentShaderHeader);

			base.Write(context);

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