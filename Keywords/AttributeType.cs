namespace THUtils.THShader.Keywords
{
	public enum AttributeType
	{
		//Anonymous is used for data passed from vertex to fragment shader.
		//The shader generator will automatically assign an attribute in that case
		Anonymous = 0,
		Position = 1,
		SV_VertexID = 2,
		SV_InstanceID = 3,
		Normal = 4,
		Tangent = 5,
		Color = 6,
		TexCoord0 = 7,
		TexCoord1 = 8,
		TexCoord2 = 9,
		TexCoord3 = 10,
		TexCoord4 = 11,
		TexCoord5 = 12,
		TexCoord6 = 13,
		TexCoord7 = 14,
	}
}