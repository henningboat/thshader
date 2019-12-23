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

		public override void Write(ShaderBuildContext context)
		{
			//todo it migh be more performant to only output a float3 for opaque shaders
			context.WriteLine("float4 frag(v2f i):COLOR{");
			context.WriteIndented(WriteFragmentShaderHeader);

			context.WriteIndented(base.Write);

			context.WriteIndented(WriteFragmentShaderFooter);

			context.WriteLine("}");
		}

		private void WriteFragmentShaderFooter(ShaderBuildContext context)
		{
			context.WriteLine(context.CurrentPass.GetFragmentShaderFooter());
			context.Newine();
        }

		private void WriteFragmentShaderHeader(ShaderBuildContext context)
		{
			context.WriteLine(context.CurrentPass.GetFragmentShaderHeader());
			context.Newine();
		}

		#endregion
	}
}