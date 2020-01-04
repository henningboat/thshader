using System;
using System.Collections.Generic;
using System.IO;
using THUtils.THShader.Keywords;
using THUtils.THShader.Passes;

namespace THUtils.THShader
{
	public abstract class ShaderPass
	{
		#region Static Stuff

		public static string ReadSourceFile(ShaderGenerationContext context, string path)
		{
			if (path == null)
			{
				return "";
			}

			if (!context.KeywordMap.GetKeyword<KeywordDebugMode>().IsDebug)
			{
				return $"#include \"Packages/com.henningboat.thshader/{path}\"";
			}
			else
			{
				string combine = Path.GetFullPath(Path.Combine("Packages/com.henningboat.thshader", path));
				try
				{
					return File.ReadAllText(combine);
				}
				catch (Exception e)
				{
					throw new KeywordMap.ShaderGenerationException("failed reading source file " + combine);
				}
			}
		}

		#endregion

		#region Private Fields

		private ShaderGenerationContext _context;

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

		public string GetFragmentShaderHeader()
		{
			return ReadSourceFile(_context, FragmentShaderHeaderPath);
		}

		public string GetFragmentShaderFooter()
		{
			return ReadSourceFile(_context, FragmentShaderFooterPath);
		}

		public string GetVertexShaderHeader()
		{
			return ReadSourceFile(_context, VertexShaderHeaderPath);
		}

		public string GetVertexShaderFooter()
		{
			return ReadSourceFile(_context, VertexShaderFooterPath);
		}

		#endregion

		#region Private methods

		private string GetShaderHeader()
		{
			return ReadSourceFile(_context, ShaderHeaderPath);
		}

		private void WriteInnerPass(ShaderGenerationContext context)
		{
			_context = context;
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

		private bool NeedsCustomShadowPass(ShaderGenerationContext context)
		{
			return context.KeywordMap.GetKeyword<KeywordCustomShadowPass>().CustomShadowPass || context.KeywordMap.GetKeyword<KeywordVertexShader>().ModifiesVertexPosition;
		}

		#endregion

		#region Protected methods

		internal virtual void WritePass(ShaderGenerationContext context, ShaderModel config)
		{
			if (UsePassName != null)
			{
				if (!NeedsCustomShadowPass(context))
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