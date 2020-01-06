using System;
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

		private PropertyType _propertyType;
		private string _propertyName;
		private string _defaultValue = "";

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
				switch (_propertyType)
				{
					case PropertyType.Texture:
						context.WriteLine($"{_propertyName} (\"{_propertyName}\", 2D) = \"\"{{}}");
						break;
					case PropertyType.Float:
						context.WriteLine($"{_propertyName} (\"{_propertyName}\", Float) = {_defaultValue}");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				switch (_propertyType)
				{
					case PropertyType.Texture:
						context.WriteLine($"sampler2D {_propertyName};");
						break;
					case PropertyType.Float:
						context.WriteLine($"float {_propertyName};");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		#endregion

		#region Protected methods

		protected override void HandleMultiLineKeyword(Queue<string> lines)
		{
			base.HandleMultiLineKeyword(lines);
			string[] arguments = FirstLineArguments.Split(' ');
			if (arguments.Length != 2 && arguments.Length != 3)
			{
				return;
			}

			_propertyType = (PropertyType) Enum.Parse(typeof(PropertyType), arguments[0]);
			_propertyName = arguments[1];
			if (arguments.Length == 3)
			{
				_defaultValue = arguments[2];
            }
			else
			{
				if (_propertyType == PropertyType.Float)
				{
					_defaultValue = "0";
				}
			}
		}

		#endregion
	}

	public enum PropertyType
	{
		Texture,
		Float,
	}
}