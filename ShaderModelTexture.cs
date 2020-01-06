using THUtils.THShader.Keywords;
using UnityEngine;

namespace THUtils.THShader.Passes
{
	public class ShaderModelTexture
	{
		#region Public Fields

		public readonly Vector4 DefaultValue;
		public readonly string Name;

		#endregion

		#region Constructors

		public ShaderModelTexture(string name, Vector4 defaultValue)
		{
			Name = name;
			DefaultValue = defaultValue;
		}

		public void Write(ShaderGenerationContext context)
		{
			context.WriteLine($"#define __SAMPLETEXTURE{Name.ToUpper()}(uv) ");
			if (KeywordProperty.HasProperty(context, PropertyType.Texture, Name))
			{
				context.Write($"tex2D({Name}, uv)");
				context.WriteLine($"#define __HASETEXTURE{Name.ToUpper()} True");
            }
			else
			{
				context.Write($" float4({DefaultValue.x}, {DefaultValue.y}, {DefaultValue.z}, {DefaultValue.w})");
			}
		}

		#endregion
	}
}