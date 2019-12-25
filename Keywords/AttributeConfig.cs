using System;

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

		public int Dimensions
		{
			get
			{
				switch (DataType)
				{
					case DataType.@float:
					case DataType.@uint:
					case DataType.@int:
					case DataType.half:
						return 1;

					case DataType.float2:
					case DataType.uint2:
					case DataType.int2:
					case DataType.half2:
						return 2;

					case DataType.float3:
					case DataType.uint3:
					case DataType.int3:
					case DataType.half3:
						return 3;

					case DataType.float4:
					case DataType.uint4:
					case DataType.int4:
					case DataType.half4:
						return 4;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
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

		public void Write(ShaderGenerationContext context, bool forVertexShader)
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