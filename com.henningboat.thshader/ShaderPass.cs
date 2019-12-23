using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using THUtils.THShader.Keywords;
using THUtils.THShader.Passes;
using UnityEngine;

namespace THUtils.THShader
{
	public abstract class ShaderPass
	{
		#region Static Stuff

		public static string ReadSourceFile(string path)
		{
			if (path == null)
			{
				return "";
			}

			string combine = Path.Combine(Application.dataPath, "THUtils/THShader", path);
			try
			{
				return File.ReadAllText(combine);
			}
			catch (Exception e)
			{
				throw new KeywordMap.ShaderGenerationException("failed reading source file " + combine);
			}
		}

		#endregion

		#region Properties

		protected virtual string FragmentShaderFooterPath => $"{LibraryBaseName}FragmentFooter.cginc";
		protected virtual string FragmentShaderHeaderPath => $"{LibraryBaseName}FragmentHeader.cginc";
		protected abstract string LibraryBaseName { get; }
		public abstract string LightMode { get; }
		public abstract List<AttributeConfig> RequiredFragmentAttributes { get; }
		public abstract List<string> RequiredFragmentKeywords { get; }
		public abstract List<AttributeConfig> RequiredVertexAttributes { get; }
		public abstract List<string> RequiredVertexKeywords { get; }
		protected virtual string ShaderHeaderPath => $"{LibraryBaseName}Header.cginc";
		protected virtual string UsePassName => null;
		protected virtual string VertexShaderFooterPath => $"{LibraryBaseName}VertexFooter.cginc";
		protected virtual string VertexShaderHeaderPath => $"{LibraryBaseName}VertexHeader.cginc";

		#endregion

		#region Public methods

		public string GetShaderHeader()
		{
			return ReadSourceFile(ShaderHeaderPath);
		}

		public string GetFragmentShaderHeader()
		{
			return ReadSourceFile(FragmentShaderHeaderPath);
		}

		public string GetFragmentShaderFooter()
		{
			return ReadSourceFile(FragmentShaderFooterPath);
		}

		public string GetVertexShaderHeader()
		{
			return ReadSourceFile(VertexShaderHeaderPath);
		}

		public string GetVertexShaderFooter()
		{
			return ReadSourceFile(VertexShaderFooterPath);
		}

		#endregion

		#region Private methods

		private void WriteInnerPass(ShaderBuildContext context)
		{
			var pipelineState = new PipelineState();
			pipelineState.Generate(context);

			context.WriteLine("HLSLPROGRAM");
			context.WriteLine("#pragma vertex vert");
			context.WriteLine("#pragma fragment frag");


			context.KeywordMap.GetMultiKeywords<KeywordProperty>().ForEach(keyword => keyword.Write(context, false));
			context.KeywordMap.GetMultiKeywords<KeywordCodeBlock>().ForEach(block => block.Write(context));

			context.KeywordMap.GetKeyword<KeywordVertexInput>().Write(context);
			context.KeywordMap.GetKeyword<KeywordFragmentInput>().Write(context);

			context.WriteLine(GetShaderHeader());

            context.KeywordMap.GetKeyword<KeywordVertexShader>().Write(context);
			context.KeywordMap.GetKeyword<KeywordFragmentShader>().Write(context);

			context.WriteLine("ENDHLSL");
		}

		private bool ShouldWriteUsePassInstead(ShaderBuildContext context)
		{
			return !context.KeywordMap.GetKeyword<KeywordCustomShadowPass>().CustomShadowPass;

			bool hasUserDefinedVertexPosition = context.KeywordMap.GetKeyword<KeywordVertexInput>().GetAttributes().Any(config => config.AttributeType == AttributeType.Position && config.UserDefined);

			return !(hasUserDefinedVertexPosition);
		}

		#endregion

		#region Protected methods

		internal virtual void WritePass(ShaderBuildContext context, ShaderPassesConfig config)
		{
			if (UsePassName != null)
			{
				if (ShouldWriteUsePassInstead(context))
				{
					new UsePass(UsePassName).WritePass(context, config);
					return;
				}
			}

			context.WriteLine("Pass{");

			var shaderPassContext = context.CreatePassContext(context, this, config);
			shaderPassContext.WriteIndented(WriteInnerPass);

			context.WriteLine("}");
		}

		#endregion
	}
}