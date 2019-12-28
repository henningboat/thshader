using System;
using System.Collections.Generic;
using THUtils.THShader.Passes;
using UnityEditor;

namespace THUtils.THShader.Keywords
{
	public class KeywordShaderModel : SingletonKeyword
	{
		#region Properties

		public override string DefaultLineArguments => "Unlit";

		#endregion

		#region Constructors

		public KeywordShaderModel(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Private methods

		private ShaderPassesConfig GetPassesConfig()
		{
			string passConfigTypeName = "ShaderPassesConfig" + FirstLineArguments;
			var possibleConfigs = TypeCache.GetTypesDerivedFrom<ShaderPassesConfig>();

			foreach (Type config in possibleConfigs)
			{
				if (config.Name == passConfigTypeName)
				{
					return (ShaderPassesConfig) Activator.CreateInstance(config);
				}
			}

			throw new KeywordMap.ShaderGenerationException($"Invalid Passes: " + DefaultLineArguments);
		}

		#endregion

		internal void GeneratePasses(ShaderGenerationContext context)
		{
			context.WriteLine("SubShader{");

			var passes = GetPassesConfig();
			context.WriteIndented(passes.Write);

			context.WriteLine("}");
		}
	}
}