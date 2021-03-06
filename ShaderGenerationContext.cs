using System;
using System.Collections.Generic;
using System.Text;
using THUtils.THShader.Passes;

namespace THUtils.THShader
{
	public class ShaderGenerationContext
	{
		#region Public Fields

		public readonly KeywordMap KeywordMap;
		public ShaderPass CurrentPass;
		public ShaderModel CurrentShaderModel;

		#endregion

		#region Private Fields

		private readonly StringBuilder _stringBuilder;
		private int _indentCount;
		public readonly SourceMap SourceMap;

		#endregion

		#region Constructors

		public ShaderGenerationContext(SourceMap sourceMap, StringBuilder stringBuilder, SourceMap.ShaderPassSource shaderPassSource = null)
		{
			SourceMap = sourceMap;
			_stringBuilder = stringBuilder;
			KeywordMap = new KeywordMap(new Queue<string>(sourceMap.DefaultPassSource));
			if (shaderPassSource != null)
			{
				KeywordMap.AddPassCode(shaderPassSource);
			}
		}

		#endregion

		#region Public methods

		public string BuildString()
		{
			return _stringBuilder.ToString();
		}

		public void Log(string log)
		{
			WriteLine($"//{log}");
		}

		public void LogShaderSection(string sectionName)
		{
			_stringBuilder.Append($"{Environment.NewLine}{Environment.NewLine}//");

			float spaceCount = _indentCount * 4 - 2;

			for (int i = 0; i < spaceCount; i++)
			{
				_stringBuilder.Append("-");
			}

			_stringBuilder.Append(sectionName);

			for (int i = 0; i < 80 - spaceCount - sectionName.Length; i++)
			{
				_stringBuilder.Append("-");
			}
		}

		public void WriteLine(string text)
		{
			if (text == null)
				return;

			var lines = text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
			foreach (string line in lines)
			{
				_stringBuilder.Append(Environment.NewLine);
				for (int i = 0; i < _indentCount; i++)
				{
					_stringBuilder.Append('\t');
				}

				_stringBuilder.Append(line);
			}
		}

		public void Write(string text)
		{
			if (text == null)
				return;

			_stringBuilder.Append(text);
		}

		public void WriteIndented(Action<ShaderGenerationContext> action)
		{
			Indent();
			action(this);
			Unindent();
		}

		public void WriteLineIndented(string line)
		{
			Indent();
			WriteLine(line);
			Unindent();
		}

		public ShaderGenerationContext CreatePassContext(ShaderGenerationContext parent, ShaderPass pass, ShaderModel config, SourceMap.ShaderPassSource passSourceSource)
		{
			return new ShaderGenerationContext(parent.SourceMap, parent._stringBuilder,passSourceSource)
			       {
				       _indentCount = parent._indentCount,
				       CurrentPass = pass,
				       CurrentShaderModel = config,
			       };
		}

		public void Newine()
		{
			WriteLine("");
		}

		#endregion

		#region Private methods

		private void Indent()
		{
			_indentCount++;
		}

		private void Unindent()
		{
			_indentCount--;
		}

		#endregion
	}
}