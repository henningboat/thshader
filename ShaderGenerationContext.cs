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
		public ShaderPassesConfig CurrentPassConfig;

		#endregion

		#region Private Fields

		private readonly StringBuilder _stringBuilder;
		private int _indentCount;
		private IReadOnlyList<string> _keymapSource;

		#endregion

		#region Constructors

		public ShaderGenerationContext(IReadOnlyList<string> source, StringBuilder stringBuilder)
		{
			_stringBuilder = stringBuilder;
			_keymapSource = source;
			KeywordMap = new KeywordMap(new Queue<string>(source));
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

		public void WriteLine(string text)
		{
			if (text == null)
				return;

			var lines = text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
			foreach (string line in lines)
			{
				for (int i = 0; i < _indentCount; i++)
				{
					_stringBuilder.Append('\t');
				}

				_stringBuilder.AppendLine(line);
			}
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

		public ShaderGenerationContext CreatePassContext(ShaderGenerationContext parent, ShaderPass pass, ShaderPassesConfig config)
		{
			return new ShaderGenerationContext(parent._keymapSource, parent._stringBuilder)
			       {
				       _indentCount = parent._indentCount,
				       CurrentPass = pass,
				       CurrentPassConfig = config,
			       };
		}

		public void Newine()
		{
			WriteLine(Environment.NewLine);
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