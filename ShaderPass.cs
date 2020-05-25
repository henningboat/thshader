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
				return $"#include \"{path}\"";
			}
			else
			{
				string combine = Path.GetFullPath(path);
				try
				{
					context.LogShaderSection($"Imported include file: {Path.GetFileName(path)}");
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
		public abstract bool IsMainPass { get; }
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
			context.WriteLine($"#define {GetType().Name}");

			context.KeywordMap.GetKeyword<KeywordVertexInput>().Write(context);
			context.KeywordMap.GetKeyword<KeywordFragmentInput>().Write(context);

			context.KeywordMap.GetMultiKeywords<KeywordCodeBlock>().ForEach(block => block.Write(context));

			context.WriteLine(GetShaderHeader());

			context.KeywordMap.GetKeyword<KeywordVertexShader>().Write(context);
			context.KeywordMap.GetKeyword<KeywordFragmentShader>().Write(context);

			context.WriteLine("ENDHLSL");
		}

		private KeywordShadowDepthPass.ShadowDepthPassMode GetShadowDepthMode(ShaderGenerationContext context)
		{
			KeywordShadowDepthPass shadowDepthPass = context.KeywordMap.GetKeyword<KeywordShadowDepthPass>();
			if (shadowDepthPass.Mode == KeywordShadowDepthPass.ShadowDepthPassMode.DefaultPass)
			{
				return context.KeywordMap.GetKeyword<KeywordVertexShader>().ModifiesVertexPosition ? KeywordShadowDepthPass.ShadowDepthPassMode.On : KeywordShadowDepthPass.ShadowDepthPassMode.DefaultPass;
			}
			else
			{
				return shadowDepthPass.Mode;
			}
		}

		#endregion

		#region Protected methods

		internal virtual void WritePass(ShaderGenerationContext context, ShaderModel config)
		{
			var mode = GetShadowDepthMode(context);
			if (UsePassName != null)
			{
				if (mode == KeywordShadowDepthPass.ShadowDepthPassMode.DefaultPass)
				{
					new UsePass(UsePassName).WritePass(context, config);
					return;
				}

				if (mode == KeywordShadowDepthPass.ShadowDepthPassMode.Off)
				{
					return;
				}
			}

			context.LogShaderSection($"Shader Pass {GetType().Name}");

			context.WriteLine("Pass{");

			var shaderPassContext = context.CreatePassContext(context, this, config);
			shaderPassContext.WriteIndented(WriteInnerPass);

			context.WriteLine("}");
		}

		#endregion
	}
}