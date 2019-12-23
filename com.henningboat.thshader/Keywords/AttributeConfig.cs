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

		#region Constructors

		public AttributeConfig(AttributeType attributeType, DataType dataType, string name, bool userDefined = false)
		{
			DataType = dataType;
			Name = name;
			AttributeType = attributeType;
			UserDefined = userDefined;
		}

		#endregion

		#region Public methods

		public void Write(ShaderBuildContext context, bool forVertexShader)
		{
			string attributeName = AttributeType.ToString();
			if (AttributeType == AttributeType.Position && !forVertexShader)
			{
				attributeName = "SV_Position";
			}

			context.WriteLineIndented($"{DataType} {Name} : {attributeName};");
		}

		#endregion

		internal readonly DataType DataType;
		internal readonly string Name;
		internal readonly AttributeType AttributeType;
		internal readonly bool UserDefined;
	}
}