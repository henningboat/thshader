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

		private List<AttributeMapping> _attributeMappings = new List<AttributeMapping>();

		#endregion

		#region Properties

		public override string DefaultLineArguments => null;
		public string ModelStructName => $"Model{AttributeStructName}";
		public override bool MultiLine => true;
		protected abstract string AttributeStructName { get; }
		public string UserStructName => $"User{AttributeStructName}";

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

			BuildAttributeMap();

			context.WriteLine($"struct {AttributeStructName}");
			context.WriteLine("{");

			context.WriteIndented(WriteAttributeComponents);

			foreach (string keyword in _keywords)
			{
				context.WriteLine(keyword);
			}

			context.Newine();

			context.WriteLine("};");

			GenerateStruct(context, UserStructName, _attributes.Where(config => config.UserDefined).ToList());
			GenerateStruct(context, ModelStructName, _attributes.Where(config => config.ShaderModelDefined).ToList());
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

		protected virtual void WriteUserDefinedAttributes(ShaderGenerationContext context, List<AttributeConfig> attributesToUse)
		{
			foreach (AttributeConfig attribute in attributesToUse)
			{
				context.WriteLine($"{attribute.DataTypeAndDimensionsString} {attribute.Name};");
			}
		}

		#endregion

		#region Private methods

		private void GenerateStruct(ShaderGenerationContext context, string structName, List<AttributeConfig> includedAttributes)
		{
			context.WriteLine($"struct {structName}");
			context.WriteLine("{");

			context.WriteIndented((c) => WriteUserDefinedAttributes(c, includedAttributes));

			context.WriteLine("};");

			context.Newine();
			BuildStructCopyFunction(context, "Initialize", structName, includedAttributes, false);
			BuildStructCopyFunction(context, "ReadBack", structName, includedAttributes, true);
		}

		private void BuildStructCopyFunction(ShaderGenerationContext context, string functionName, string structName, List<AttributeConfig> includedAttributes, bool isReadBack)
		{
			if (!isReadBack)
			{
				context.WriteLine($"void {functionName}{structName}({AttributeStructName} input, inout {structName} output)");
			}
			else
			{
				context.WriteLine($"void {functionName}{structName}({structName} input, inout {AttributeStructName} output)");
			}

			context.WriteLine("{");
			context.WriteIndented((c) => { SetupUserStructCode(c, includedAttributes, isReadBack, structName); });
			context.WriteLine("}");
		}

		protected virtual void SetupUserStructCode(ShaderGenerationContext context, List<AttributeConfig> attributes, bool isReadback, string structName)
		{
			List<AttributeAliasMapEntry> allEntries = new List<AttributeAliasMapEntry>();
			foreach (AttributeMapping mapping in _attributeMappings)
			{
				allEntries.AddRange(mapping.Entries);
			}

			if (isReadback)
			{
				foreach (AttributeAliasMapEntry entry in allEntries)
				{
					if (attributes.Any(a => entry.AttributeConfig.Equals(a)))
					{
						context.WriteLine($"output.{entry.AttributeMapping.Name}[{entry.IndexInAttribute}] = input.{entry.AttributeConfig.Name}[{entry.Component}];");
					}
				}
			}
			else
            {
				//todo this can be simplified by doing it in the same way as the attributes
	            foreach (AttributeConfig attributeConfig in attributes)
	            {
		            context.Write($"output.{attributeConfig.Name} = {attributeConfig.DataTypeAndDimensionsString}(");

		            for (int i = 0; i < attributeConfig.Dimensions; i++)
		            {
			            if (i > 0)
			            {
				            context.Write(",");
			            }

			            AttributeAliasMapEntry mappingEntry;
			            if (attributeConfig.AttributeType == AttributeType.Anonymous)
			            {
				            mappingEntry = allEntries.First(entry => entry.AttributeConfig.Name == attributeConfig.Name && entry.Component == i);
			            }
			            else
			            {
				            mappingEntry = allEntries.First(entry => entry.AttributeType == attributeConfig.AttributeType && entry.Component == i);
			            }

			            context.Write($"input.{mappingEntry.AttributeName}[{mappingEntry.IndexInAttribute}]");
		            }

		            context.Write(");");
		            context.Newine();
	            }
            }
		}

		private void WriteAttributeComponents(ShaderGenerationContext context)
		{
			for (int i = 0; i < _attributeMappings.Count; i++)
			{
				AttributeMapping mapping = _attributeMappings[i];

				if (mapping.AttributeType == AttributeType.Anonymous)
				{
					string log = "Content:";

					for (int j = 0; j < mapping.Entries.Count; j++)
					{
						AttributeAliasMapEntry entry = mapping.Entries[j];
						log += $" {entry.AttributeConfig.Name}[{entry.Component}],";
					}

					context.Log(log);
				}

				string dataTypeString;
				string attributeTypeString;
				string nameString = mapping.Name;

				if (mapping.Dimensions == 1)
				{
					dataTypeString = $"{mapping.DataType}";
				}
				else
				{
					dataTypeString = $"{mapping.DataType}{mapping.Dimensions}";
				}

				if (mapping.AttributeType == AttributeType.Anonymous)
				{
					attributeTypeString = $"Color{i + 10}";
				}
				else
				{
					attributeTypeString = GetAttributeTypeString(mapping.AttributeType);
				}

				context.WriteLine($"{dataTypeString} {nameString} : {attributeTypeString};");
			}
		}

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
					if (!_attributes.Any(config => config.AttributeType == requiredAttribute.AttributeType))
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

		private void BuildAttributeMap()
		{
			_attributeMappings = new List<AttributeMapping>();
			foreach (AttributeConfig attributeConfig in _attributes)
			{
				for (int i = 0; i < attributeConfig.Dimensions; i++)
				{
					AttributeMapping mapping = GetOrCreateAttributeMap(attributeConfig.DataType, attributeConfig.AttributeType);
					mapping.AddEntry(new AttributeAliasMapEntry(mapping, i, attributeConfig));
				}
			}
		}

		private AttributeMapping GetOrCreateAttributeMap(DataType dataType, AttributeType attributeType)
		{
			AttributeMapping mapping = _attributeMappings.LastOrDefault(attributeMapping => attributeMapping.DataType == dataType && attributeMapping.AttributeType == attributeType);

			if (mapping == null || mapping.IsFull)
			{
				if (mapping != null && attributeType != AttributeType.Anonymous)
				{
					_attributeMappings.Remove(mapping);
				}

				mapping = new AttributeMapping(dataType, attributeType, _attributeMappings.Count + 1);
				_attributeMappings.Add(mapping);
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

			public readonly AttributeConfig AttributeConfig;
			public readonly AttributeMapping AttributeMapping;
			public readonly int Component;

			#endregion

			#region Properties

			public AttributeType AttributeType => AttributeMapping.AttributeType;
			public DataType DataTyp => AttributeMapping.DataType;
			public string AttributeName { get; private set; }
			public int IndexInAttribute { get; set; }

			#endregion

			#region Constructors

			internal AttributeAliasMapEntry(AttributeMapping mapping, int component, AttributeConfig attributeConfig)
			{
				AttributeConfig = attributeConfig;
				AttributeMapping = mapping;
				Component = component;
			}

			#endregion

			#region Public methods

			public void SetAttributeName(string name)
			{
				AttributeName = name;
			}

			public void SetIndexInAttribute(int index)
			{
				IndexInAttribute = index;
			}

			#endregion
		}

		#endregion

		#region Nested type: AttributeMapping

		internal class AttributeMapping
		{
			#region Public Fields

			public readonly AttributeType AttributeType;
			public readonly DataType DataType;
			public readonly List<AttributeAliasMapEntry> Entries;
			public readonly string Name;

			#endregion

			#region Properties

			public int Dimensions => Entries.Count;
			public bool IsFull => Dimensions >= 4;

			#endregion

			#region Constructors

			public AttributeMapping(DataType dataType, AttributeType attributeType, int index)
			{
				DataType = dataType;
				AttributeType = attributeType;
				Entries = new List<AttributeAliasMapEntry>(4);
				if (attributeType == AttributeType.Anonymous)
				{
					Name = $"PackedData{index}";
				}
				else
				{
					Name = $"{attributeType.ToString()}Data";
				}
			}

			#endregion

			#region Public methods

			public void AddEntry(AttributeAliasMapEntry entry)
			{
				if (IsFull)
				{
					throw new ArgumentOutOfRangeException();
				}

				entry.SetAttributeName(Name);
				entry.SetIndexInAttribute(Entries.Count);

				Entries.Add(entry);
			}

			#endregion
		}

		#endregion
	}
}