using System;
using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public class KeywordModifyFinalColor : SingletonKeyword
	{
		#region Private Fields

		private string _parsedCode;

		#endregion

		#region Properties

		public override string DefaultLineArguments => null;
		public override bool MultiLine => true;

		#endregion

		#region Constructors
		

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
			if (IsDefault || !context.CurrentPass.IsMainPass)
				return;
			context.LogShaderSection("FinalColor Code Block");
			context.WriteLine("float4 finalColor = __color;");
			context.WriteLine(_parsedCode);
			context.WriteLine("__color = finalColor;");
		}

		#endregion

		public KeywordModifyFinalColor(Queue<string> sourceCode) : base(sourceCode)
		{
		}
	}
}