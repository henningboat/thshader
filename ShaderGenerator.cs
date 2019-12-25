using System;
using System.Collections.Generic;
using System.Text;
using THUtils.THShader.Keywords;
using UnityEngine;

namespace THUtils.THShader
{
	internal class ShaderGenerator
	{
		#region Static Stuff

		private const string DefaultErrorShader = @"
Shader ""Hidden/THShaderErrorShader2""
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON
            #include ""UnityCG.cginc""

            struct appdata_t {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1,0,1,1);
            }
            ENDCG
        }
    }
    Fallback Off
}";

		internal static string[] SplitLine(string line)
		{
			//todo support tabs, whitespaces etc
			return line.Split(' ');
		}

		#endregion

		#region Properties

		public string GeneratedShader { get; }

		#endregion

		#region Constructors

		internal ShaderGenerator(List<string> source)
		{
			ShaderGenerationContext context=null;
			try
			{
				RemoveComments(source);

				context = new ShaderGenerationContext(source, new StringBuilder());

				GenerateOuter(context, (s) =>
				                       {
					                       AppendProperties(s);

					                       context.KeywordMap.GetKeyword<KeywordPasses>().GeneratePasses(s);
				                       });

				GeneratedShader += context.BuildString();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				if (context != null)
				{
					GeneratedShader += context.BuildString();
				}
				GeneratedShader += "\n";
				GeneratedShader += e;
				Debug.LogError("Failed to import thshader");
			}
		}

		#endregion

		#region Private methods

		private void RemoveComments(List<string> source)
		{
			for (int i = 0; i < source.Count; i++)
			{
				source[i] = source[i].Split(new[] { "//" }, StringSplitOptions.None)[0];
			}
		}

		private void AppendProperties(ShaderGenerationContext context)
		{
			context.WriteLine($"Properties");
			context.WriteLine($"{{");

			context.KeywordMap.GetMultiKeywords<KeywordProperty>().ForEach(keyword => keyword.Write(context, true));

			context.WriteLine($"");
			context.WriteLine($"}}");
		}

		private void GenerateOuter(ShaderGenerationContext context, Action<ShaderGenerationContext> action)
		{
			context.WriteLine($"Shader \"{context.KeywordMap.GetKeyword<KeywordName>().ShaderName}\" {{");
			context.WriteIndented(action);
			context.WriteLine($"}}");
		}

		#endregion
	}
}