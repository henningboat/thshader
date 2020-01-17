using System.Collections.Generic;

namespace THUtils.THShader
{
	internal class PipelineState
	{
		#region Public methods

		public void Generate(ShaderGenerationContext context)
		{
			List<PipelineStateKeyword> keywords = context.KeywordMap.GetKeywords<PipelineStateKeyword>();

			string lightMode = context.CurrentPass.LightMode;
			KeywordLightMode keywordLightMode = context.KeywordMap.GetKeyword<KeywordLightMode>();
			if (context.CurrentPass.IsMainPass && keywordLightMode.OverwriteLightMode!=null)
			{
				lightMode = keywordLightMode.OverwriteLightMode;
			}
			if (context.CurrentPass != null && lightMode != null)
			{
				context.WriteLine($"Tags{{\"LightMode\" = \"{lightMode}\"}}");
			}

			foreach (PipelineStateKeyword keyword in keywords)
			{
				keyword.Write(context);
			}
		}

		#endregion
	}
}