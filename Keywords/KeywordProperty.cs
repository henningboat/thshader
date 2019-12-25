using System.Collections.Generic;
using System.Linq;

namespace THUtils.THShader.Keywords
{
	public class KeywordProperty : MultiKeyword
	{
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

			string[] arguments = FirstLineArguments.Split(' ');
			if (arguments.Length != 2)
			{
				return;
			}

			string propertyType = arguments[0];
			string propertyName = arguments[1];

			if (propertyDefinition)
			{
				context.WriteLine($"{propertyName} (\"{propertyName}\", 2D) = \"\"{{}}");
			}
			else
			{
				if (context.CurrentPassConfig.DefinedShaderProperties.Any(property => property.Name == propertyName))
				{
					return;
				}

				context.WriteLine($"sampler2D {propertyName};");
			}
		}

		#endregion
	}
}