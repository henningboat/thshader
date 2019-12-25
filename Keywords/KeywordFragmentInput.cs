using System;
using System.Collections.Generic;
using System.Linq;

namespace THUtils.THShader.Keywords
{
	internal class KeywordFragmentInput : KeywordAttributeStruct
	{
		#region Private Fields

		private List<AttributeAliasMap> _attributeAliasMaps = new List<AttributeAliasMap>();

		#endregion

		#region Properties

		protected override string StructName => "Varyings";

		#endregion

		#region Constructors

		public KeywordFragmentInput(Queue<string> sourceCode) : base(sourceCode)
		{
		}

		#endregion

		#region Public methods

		protected override void WriteAttributeComponents(ShaderGenerationContext context)
		{
			foreach (AttributeAliasMap aliasMap in _attributeAliasMaps)
			{
				context.WriteLine($"float{aliasMap.Dimensions} {aliasMap.Name} : {aliasMap.Name}");
			}
		}

		#endregion

		#region Protected methods

		protected override void HandleMultiLineKeyword(Queue<string> lines)
		{
			base.HandleMultiLineKeyword(lines);
			BuildAttributeAliasMap();
		}

		protected override List<string> GetRequiredPassKeywords(ShaderGenerationContext context)
		{
			return context.CurrentPass.RequiredFragmentKeywords;
		}

		protected override List<AttributeConfig> GetRequiredPassAttributes(ShaderGenerationContext context)
		{
			return context.CurrentPass.RequiredFragmentAttributes;
		}

		#endregion

		#region Private methods

		private void BuildAttributeAliasMap()
		{
			List<AttributeAliasMap> anonymousFloatAttributes = new List<AttributeAliasMap>();
			anonymousFloatAttributes.Add(new AttributeAliasMap("Color10"));

			foreach (AttributeConfig attributeConfig in _attributes.Values)
			{
				for (int i = 0; i < attributeConfig.Dimensions; i++)
				{
					if (anonymousFloatAttributes.Last().IsFull)
					{
						anonymousFloatAttributes.Add(new AttributeAliasMap($"Color{anonymousFloatAttributes.Count + 10}"));
					}

					anonymousFloatAttributes.Last().AddEntry(new AttributeAliasMapEntry(attributeConfig.Name, i));
				}
			}
		}

		#endregion

		#region Nested type: AttributeAliasMap

		private class AttributeAliasMap
		{
			#region Public Fields

			public readonly List<AttributeAliasMapEntry> Entries;
			public readonly string Name;

			#endregion

			#region Properties

			public int Dimensions => Entries.Count;
			public bool IsFull => Dimensions >= 4;

			#endregion

			#region Constructors

			public AttributeAliasMap(string name)
			{
				Name = name;
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
	}
}