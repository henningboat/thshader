using System;
using System.Collections.Generic;
using System.Linq;

namespace THUtils.THShader.Keywords
{
	internal abstract class KeywordAttributeStruct : SingletonKeyword
	{
		#region Protected Fields

		protected List<AttributeConfig> _attributes = new List<AttributeConfig>();
		protected List<string> _keywords = new List<string>();

		#endregion

		#region Properties

		public abstract string AttributeStructName { get; }
		public override string DefaultLineArguments => null;
		public override bool MultiLine => true;

		#endregion

		#region Constructors

		protected KeywordAttributeStruct(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public AttributeConfig GetAttribute(AttributeType type)
		{
			return _attributes.First(config => config.AttributeType == type);
		}

		public virtual void Write(ShaderGenerationContext context)
		{
			AddPassRequiredAttributes(GetRequiredPassAttributes(context));
			AddPassKeywords(GetRequiredPassKeywords(context));

			context.WriteLine($"struct {AttributeStructName}");
			context.WriteLine("{");

			context.WriteIndented((c) =>
			                      {
				                      for (int i = 0; i < _attributes.Count; i++)
				                      {
					                      AttributeConfig config = _attributes[i];
					                      if (config.AttributeType != AttributeType.Anonymous)
					                      {
						                      c.WriteLine($"#define __{AttributeStructName.ToUpper()}{config.AttributeType.ToString().ToUpper()}  {config.Name}");
					                      }

					                      config.WriteAttributeDefinition(c, i, this is KeywordFragmentInput);
				                      }
			                      });

			foreach (string keyword in _keywords)
			{
				context.WriteLine(keyword);
			}

			context.WriteLine("};");
			context.Newine();
		}

		#endregion

		#region Protected methods

		protected override void HandleMultiLineKeyword(Queue<string> lines)
		{
			while (lines.Count > 0 && lines.Peek() != "")
			{
				string line = lines.Dequeue();
				var parts = line.Split(' ');

				AttributeType attribute;

				if (parts.Length == 3)
				{
					if (!Enum.TryParse(parts[2], true, out attribute))
					{
						throw new Exception($"Failed to parse Vertex Attribute in following line: \"{line}\"");
					}
				}
				else if (parts.Length == 2)
				{
					attribute = AttributeType.Anonymous;
				}
				else
				{
					throw new Exception($"Parsing Error in line: \"{line}\"");
				}

				ParseDataTypeAndDimension(parts[0], out DataType dataType, out uint dimension);

				string name = parts[1];

				if (_attributes.Any(config => attribute != AttributeType.Anonymous && config.AttributeType == attribute))
				{
					throw new Exception($"Duplicate Attribute definition in following line: \"{line}\"");
				}

				_attributes.Add(new AttributeConfig(attribute, dataType, dimension, name, true));
			}
		}

		protected abstract List<string> GetRequiredPassKeywords(ShaderGenerationContext context);
		protected abstract List<AttributeConfig> GetRequiredPassAttributes(ShaderGenerationContext context);

		#endregion

		#region Private methods

		private string GetAttributeTypeString(AttributeType attributeType)
		{
			string attributeName = attributeType.ToString();
			if (attributeType == AttributeType.Position && this is KeywordFragmentInput)
			{
				attributeName = "SV_Position";
			}

			return attributeName;
		}

		private void ParseDataTypeAndDimension(string input, out DataType dataType, out uint dimensions)
		{
			char lastChar = input.Last();
			if (lastChar == '2' || lastChar == '3' || lastChar == '4')
			{
				dimensions = uint.Parse(lastChar.ToString());
				input = input.Substring(0, input.Length - 1);
			}
			else
			{
				dimensions = 1;
			}

			if (!Enum.TryParse(input, true, out dataType))
			{
				throw new Exception($"Failed to parse Data Type in following line: \"{input}\"");
			}
		}

		private void AddPassKeywords(List<string> keywords)
		{
			foreach (string keyword in keywords)
			{
				_keywords.Add(keyword);
			}
		}

		private void AddPassRequiredAttributes(List<AttributeConfig> requiredAttributes)
		{
			foreach (AttributeConfig requiredAttribute in requiredAttributes)
			{
				bool needsToBeAdded = false;
				if (requiredAttribute.AttributeType == AttributeType.Anonymous)
				{
					needsToBeAdded = true;
				}
				else
				{
					if (_attributes.All(config => config.AttributeType != requiredAttribute.AttributeType))
					{
						needsToBeAdded = true;
					}
					else
					{
						AttributeConfig existingAttribute = _attributes.First(config => config.AttributeType == requiredAttribute.AttributeType);
						if (existingAttribute.DataType == requiredAttribute.DataType && existingAttribute.Dimensions == requiredAttribute.Dimensions)
						{
							if (existingAttribute.Name != requiredAttribute.Name)
							{
								needsToBeAdded = true;
							}
						}
						else
						{
							throw new KeywordMap.ShaderGenerationException($"Attribute {existingAttribute.Name} of type {existingAttribute.AttributeType} needs to be a {requiredAttribute.DataType} ({requiredAttribute.Dimensions}) in this shader model");
						}
					}
				}

				if (needsToBeAdded)
				{
					_attributes.Add(requiredAttribute);
				}
			}
		}

		#endregion

		internal List<AttributeConfig> GetAttributes()
		{
			return _attributes.ToList();
		}
	}
}