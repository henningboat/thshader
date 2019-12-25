using System.Collections.Generic;
using THUtils.THShader.Keywords;

namespace THUtils.THShader
{
	public abstract class PipelineStateKeyword : SingletonKeyword
	{
		#region Constructors

		protected PipelineStateKeyword(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		internal abstract void Write(ShaderGenerationContext context);
	}
}