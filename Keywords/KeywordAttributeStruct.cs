using System;
using System.Collections.Generic;
using System.Linq;

namespace THUtils.THShader.Keywords
{
	internal abstract class KeywordAttributeStruct : SingletonKeyword
	{
		#region Protected Fields

		protected Dictionary<AttributeType, AttributeConfig> _attributes = new Dictionary<AttributeType, AttributeConfig>();
		protected List<string> _keywords = new List<string>();

		#endregion

		#region Properties

		public override string DefaultLineArguments => null;
		public override bool MultiLine => true;
		protected abstract string StructName { get; }

		#endregion

		#region Constructors

		protected KeywordAttributeStruct(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		public AttributeConfig GetAttribute(AttributeType type)
		{
			return _attributes[type];
		}

		public virtual void Write(ShaderGenerationContext context)
		{
			AddPassRequiredAttributes(GetRequiredPassAttributes(context));
			AddPassKeywords(GetRequiredPassKeywords(context));

			context.WriteLine($"struct {StructName}");
			context.WriteLine("{");

			WriteAttributeComponents(context);

			foreach (string keyword in _keywords)
			{
				context.WriteLine(keyword);
			}

			context.WriteLine("};");
		}

		protected virtual void WriteAttributeComponents(ShaderGenerationContext context)
		{
			foreach (var attribute in _attributes.Values)
			{
				attribute.Write(context, this is KeywordVertexInput);
			}
		}

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

				if (!Enum.TryParse(parts[0], true, out DataType dataType))
				{
					throw new Exception($"Failed to parse Data Type in following line: \"{line}\"");
				}

				string name = parts[1];

				if (_attributes.ContainsKey(attribute))
				{
					throw new Exception($"Duplicate Attribute definition in following line: \"{line}\"");
				}

				_attributes[attribute] = new AttributeConfig(attribute, dataType, name, true);
			}
		}

		#endregion

		#region Protected methods

		protected abstract List<string> GetRequiredPassKeywords(ShaderGenerationContext context);
		protected abstract List<AttributeConfig> GetRequiredPassAttributes(ShaderGenerationContext context);

		#endregion

		#region Private methods

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
				if (!_attributes.ContainsKey(requiredAttribute.AttributeType))
				{
					needsToBeAdded = true;
				}
				else
				{
					var existingAttribute = _attributes[requiredAttribute.AttributeType];
					if (existingAttribute.DataType != requiredAttribute.DataType)
					{
						throw new KeywordMap.ShaderGenerationException($"Attribute {existingAttribute.Name} of typ {existingAttribute.AttributeType} has to be defined with data type {requiredAttribute.DataType}");
					}

					if (existingAttribute.Name != requiredAttribute.Name)
					{
						needsToBeAdded = true;
					}
				}

				if (needsToBeAdded)
				{
					_attributes.Add(requiredAttribute.AttributeType, requiredAttribute);
				}
			}
		}

		#endregion

		internal List<AttributeConfig> GetAttributes()
		{
			return _attributes.Values.ToList();
		}
	}
}