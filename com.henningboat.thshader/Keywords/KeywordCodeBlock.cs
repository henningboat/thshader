using System;
using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public class KeywordCodeBlock : MultiKeyword
	{
		#region Private Fields

		private string _parsedCode;

		#endregion

		#region Properties

		public override string DefaultLineArguments => null;
		public override bool MultiLine => true;

		#endregion

		#region Constructors

		public KeywordCodeBlock(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public override void HandleMultiLineKeyword(Queue<string> lines)
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

		public virtual void Write(ShaderBuildContext context)
		{
			context.WriteLine(_parsedCode);
		}

		#endregion
	}
}