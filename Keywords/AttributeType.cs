namespace THUtils.THShader.Keywords
{
	public enum AttributeType
	{
		//Anonymous is used for data passed from vertex to fragment shader.
		//The shader generator will automatically assign an attribute in that case
		Anonymous = 0,
		POSITION = 1,
		SV_VERTEXID = 2,
		SV_INSTANCEID = 3,
		NORMAL = 4,
		TANGENT = 5,
		COLOR = 6,
		TEXCOORD0 = 7,
		TEXCOORD1 = 8,
		TEXCOORD2 = 9,
		TEXCOORD3 = 10,
		TEXCOORD4 = 11,
		TEXCOORD5 = 12,
		TEXCOORD6 = 13,
		TEXCOORD7 = 14,
	}
}