using System.Collections.Generic;

namespace THUtils.THShader
{
	public abstract class PipelineStateEnumKeyword : PipelineStateKeyword
	{
		#region Properties
		
		#endregion

		#region Constructors

		internal PipelineStateEnumKeyword(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		internal override void Write(ShaderGenerationContext context)
		{
			context.WriteLine($"{Name} {FirstLineArguments}");
		}
	}
}