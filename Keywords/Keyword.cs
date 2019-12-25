using System.Collections.Generic;
using UnityEngine;

namespace THUtils.THShader.Keywords
{
	public abstract class Keyword
	{
		#region Public Fields

		public readonly bool IsDefault;

		#endregion

		#region Protected Fields

		protected readonly string FirstLineArguments;

		#endregion

		#region Properties

		public abstract string DefaultLineArguments { get; }
		public virtual bool MultiLine => false;
		public string Name => GetType().Name.Substring(7);

		#endregion

		#region Constructors

		internal Keyword(Queue<string> sourceCode)
		{
			if (sourceCode == null)
			{
				IsDefault = true;
				FirstLineArguments = DefaultLineArguments;
				return;
			}

			string line = sourceCode.Dequeue();
			FirstLineArguments = line.Substring(Mathf.Min(Name.Length + 1, line.Length));

			HandleMultiLineKeyword(sourceCode);
		}

		#endregion

		#region Public methods

		protected virtual void HandleMultiLineKeyword(Queue<string> lines)
		{
		}

		#endregion
	}
}