namespace THUtils.THShader.Keywords
{
	public struct AttributeConfig
	{
		#region Static Stuff

		public static bool operator ==(AttributeConfig a, AttributeConfig b)
		{
			return a.DataType == b.DataType && a.Name == b.Name && a.AttributeType == b.AttributeType;
		}

		public static bool operator !=(AttributeConfig a, AttributeConfig b)
		{
			return !(a == b);
		}

		#endregion

		#region Properties

		public string DataTypeAndDimensionsString
		{
			get
			{
				if (Dimensions == 1)
				{
					return DataType.ToString();
				}
				else
				{
					return $"{DataType}{Dimensions}";
				}
			}
		}

		#endregion

		#region Constructors

		public AttributeConfig(AttributeType attributeType, DataType dataType, uint dimensions, string name, bool userDefined = false)
		{
			DataType = dataType;
			Name = name;
			AttributeType = attributeType;
			UserDefined = userDefined;

			if (dimensions > 4 || dimensions == 0)
			{
				throw new KeywordMap.ShaderGenerationException("Attribute dimensions needs to be between 1 and 4");
			}

			Dimensions = dimensions;
		}

		#endregion

		internal readonly DataType DataType;
		internal readonly uint Dimensions;
		internal readonly string Name;
		internal readonly AttributeType AttributeType;
		internal readonly bool UserDefined;
	}
}