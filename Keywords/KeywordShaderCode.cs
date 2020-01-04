using System;
using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	internal class KeywordShaderCode : SingletonKeyword
	{
		#region Protected Fields

		protected string _parsedCode;

		#endregion

		#region Properties

		public override string DefaultLineArguments => null;
		public override bool MultiLine => true;

		#endregion

		#region Constructors

		public KeywordShaderCode(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		protected override void HandleMultiLineKeyword(Queue<string> lines)
		{
			while (lines.Count > 0)
			{
				string line = lines.Dequeue();

				if (line == "ENDCG")
				{
					return;
				}
				else
				{
					_parsedCode += line + Environment.NewLine;
				}
			}
		}

		public virtual void Write(ShaderGenerationContext context)
		{
			context.WriteLine(_parsedCode);
		}

		#endregion

		protected void WriteIncludeFile(string getVertexShaderHeader)
		{
			throw new NotImplementedException();
		}
	}
}