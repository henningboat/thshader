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

		#region Private Fields

		private List<AttributeMapping> _attributeAliasMaps = new List<AttributeMapping>();
		private List<AttributeMapping> _explicitAttributeMappings = new List<AttributeMapping>();

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
			return _attributes.First(config => config.AttributeType == type);
		}

		public virtual void Write(ShaderGenerationContext context)
		{
			AddPassRequiredAttributes(GetRequiredPassAttributes(context));
			AddPassKeywords(GetRequiredPassKeywords(context));

			context.WriteLine($"struct {StructName}");
			context.WriteLine("{");

			context.WriteIndented(WriteAttributeComponents);

			foreach (string keyword in _keywords)
			{
				context.WriteLine(keyword);
			}

			context.WriteLine("};");
		}

		#endregion

		#region Protected methods

		private void WriteAttributeComponents(ShaderGenerationContext context)
		{
			for (int i = 0; i < _attributeAliasMaps.Count; i++)
			{
				AttributeMapping mapping = _attributeAliasMaps[i];

				string log = "Content:";

				for (int j = 0; j < mapping.Entries.Count; j++)
				{
					AttributeAliasMapEntry entry = mapping.Entries[j];
					log += $" {entry.Name}[{entry.Component}],";
				}

				context.Log(log);

				if (mapping.Dimensions == 1)
				{
					context.WriteLine($"{mapping.DataType} PackedData{i} : Color{i + 10}");
				}
				else
				{
					context.WriteLine($"{mapping.DataType}{mapping.Dimensions} PackedData{i} : Color{i + 10}");
				}
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

				ParseDataTypeAndDimension(parts[0], out DataType dataType, out uint dimension);

				string name = parts[1];

				if (_attributes.Any(config => attribute != AttributeType.Anonymous && config.AttributeType == attribute))
				{
					throw new Exception($"Duplicate Attribute definition in following line: \"{line}\"");
				}

				_attributes.Add(new AttributeConfig(attribute, dataType, dimension, name, true));
			}

			RebuildAttributeAliasMap();
		}

		protected abstract List<string> GetRequiredPassKeywords(ShaderGenerationContext context);
		protected abstract List<AttributeConfig> GetRequiredPassAttributes(ShaderGenerationContext context);

		#endregion

		#region Private methods

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
			//todo
			//foreach (AttributeConfig requiredAttribute in requiredAttributes)
			//{
			//	bool needsToBeAdded = false;
			//	//todo
			//	if (!_attributes.ContainsKey(requiredAttribute.AttributeType))
			//	{
			//		needsToBeAdded = true;
			//	}
			//	else
			//	{
			//		var existingAttribute = _attributes[requiredAttribute.AttributeType];
			//		if (existingAttribute.DataType != requiredAttribute.DataType)
			//		{
			//			throw new KeywordMap.ShaderGenerationException($"Attribute {existingAttribute.Name} of typ {existingAttribute.AttributeType} has to be defined with data type {requiredAttribute.DataType}");
			//		}

			//		if (existingAttribute.Name != requiredAttribute.Name)
			//		{
			//			needsToBeAdded = true;
			//		}
			//	}

			//	if (needsToBeAdded)
			//	{
			//		_attributes.Add(requiredAttribute.AttributeType, requiredAttribute);
			//	}
			//}
		}

		private void RebuildAttributeAliasMap()
		{
			_attributeAliasMaps = new List<AttributeMapping>();
			foreach (AttributeConfig attributeConfig in _attributes)
			{
				for (int i = 0; i < attributeConfig.Dimensions; i++)
				{
					GetOrCreateAttributeMap(attributeConfig.DataType).AddEntry(new AttributeAliasMapEntry(attributeConfig.Name, i));
				}
			}
		}

		private AttributeMapping GetOrCreateAttributeMap(DataType dataType)
		{
			AttributeMapping mapping = _attributeAliasMaps.LastOrDefault(attributeMapping => attributeMapping.DataType == dataType);
			if (mapping == null || mapping.IsFull)
			{
				mapping = new AttributeMapping(dataType);
				_attributeAliasMaps.Add(mapping);
			}

			return mapping;
		}

		#endregion

		internal List<AttributeConfig> GetAttributes()
		{
			return _attributes.ToList();
		}

		#region Nested type: AttributeAliasMapEntry

		internal class AttributeAliasMapEntry
		{
			#region Public Fields

			public readonly int Component;
			public readonly string Name;

			#endregion

			#region Constructors

			public AttributeAliasMapEntry(string name, int component)
			{
				Component = component;
				Name = name;
			}

			#endregion
		}

		#endregion

		#region Nested type: AttributeMapping

		private class AttributeMapping
		{
			#region Public Fields

			public readonly DataType DataType;
			public readonly List<AttributeAliasMapEntry> Entries;

			#endregion

			#region Properties

			public int Dimensions => Entries.Count;
			public bool IsFull => Dimensions >= 4;

			#endregion

			#region Constructors

			public AttributeMapping(DataType dataType)
			{
				DataType = dataType;
				Entries = new List<AttributeAliasMapEntry>(4);
			}

			#endregion

			#region Public methods

			public void AddEntry(AttributeAliasMapEntry entry)
			{
				if (IsFull)
				{
					throw new ArgumentOutOfRangeException();
				}

				Entries.Add(entry);
			}

			#endregion
		}

		#endregion
	}
}