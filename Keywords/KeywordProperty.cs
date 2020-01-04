using System.Collections.Generic;

namespace THUtils.THShader.Keywords
{
	public class KeywordProperty : MultiKeyword
	{
		#region Static Stuff

		public static bool HasProperty(ShaderGenerationContext context, PropertyType propertyType, string name)
		{
			var allProperties = context.KeywordMap.GetMultiKeywords<KeywordProperty>();
			foreach (KeywordProperty property in allProperties)
			{
				if (property._propertyName == name)
				{
					return true;
				}
			}

			return false;
		}

		#endregion

		#region Private Fields

		private string _propertyType;
		private string _propertyName;

		#endregion

		#region Properties

		public override string DefaultLineArguments { get; }

		#endregion

		#region Constructors

		public KeywordProperty(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public void Write(ShaderGenerationContext context, bool propertyDefinition)
		{
			if (IsDefault)
				return;

			if (propertyDefinition)
			{
				context.WriteLine($"{_propertyName} (\"{_propertyName}\", 2D) = \"\"{{}}");
			}
			else
			{
				context.WriteLine($"sampler2D {_propertyName};");
			}
		}

		#endregion

		#region Protected methods

		protected override void HandleMultiLineKeyword(Queue<string> lines)
		{
			base.HandleMultiLineKeyword(lines);
			string[] arguments = FirstLineArguments.Split(' ');
			if (arguments.Length != 2)
			{
				return;
			}

			_propertyType = arguments[0];
			_propertyName = arguments[1];
		}

		#endregion
	}

	public enum PropertyType
	{
		Texture2D,
	}
}