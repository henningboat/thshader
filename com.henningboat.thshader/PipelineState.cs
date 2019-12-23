using System.Collections.Generic;

namespace THUtils.THShader
{
	internal class PipelineState
	{
		#region Public methods

		public void Generate(ShaderBuildContext context)
		{
			List<PipelineStateKeyword> keywords = context.KeywordMap.GetKeywords<PipelineStateKeyword>();

			if (context.CurrentPass != null && context.CurrentPass.LightMode != null)
			{
				context.WriteLine($"Tags{{\"LightMode\" = \"{context.CurrentPass.LightMode}\"}}");
			}

			foreach (PipelineStateKeyword keyword in keywords)
			{
				keyword.Write(context);
			}
		}

		#endregion
	}
}