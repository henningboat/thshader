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
			TrimSource(source);
			ShaderGenerationContext context = null;
			try
			{
				RemoveComments(source);
				SourceMap sourceMap = new SourceMap(source);

				context = new ShaderGenerationContext(sourceMap, new StringBuilder());

				GenerateOuter(context, (s) =>
				                       {
					                       AppendProperties(s);

					                       context.KeywordMap.GetKeyword<KeywordShaderModel>().GeneratePasses(s);
				                       });

				GeneratedShader += context.BuildString();
			}
			catch (Exception e)
			{
				Debug.LogWarning(e);
				if (context != null)
				{
					GeneratedShader += context.BuildString();
				}

				GeneratedShader += "\n #error";
				GeneratedShader += e;
				Debug.LogWarning("Failed to import thshader");
			}
		}

		#endregion

		#region Private methods

		private void TrimSource(List<string> source)
		{
			for (int i = 0; i < source.Count; i++)
			{
				source[i] = source[i].Trim();
			}
		}

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

	public class SourceMap
	{
		#region Private Fields

		private List<ShaderPassSource> _customPasses;
		private List<string> _defaultPassSource;

		#endregion

		#region Properties

		public List<string> DefaultPassSource => _defaultPassSource;
		public IReadOnlyCollection<ShaderPassSource> CustomPasses => _customPasses;

		#endregion

		#region Constructors

		public SourceMap(List<string> source)
		{
			_defaultPassSource = new List<string>();
			_customPasses = new List<ShaderPassSource>();
			
			Queue<string> sourceCodeQueue = new Queue<string>(source);
			while (sourceCodeQueue.Count > 0)
			{
				string line = sourceCodeQueue.Dequeue();
				if (line.StartsWith("CustomPass "))
				{
					string shaderPassName = line.Replace("ShaderPass ", "");
					ShaderPassSource shaderPassSource = new ShaderPassSource(shaderPassName);

					while (true)
					{
						string nextLine = sourceCodeQueue.Dequeue();
						if (nextLine != "EndPass")
						{
							shaderPassSource.AddLine(nextLine);
						}
						else
						{
							break;
						}
					}

					_customPasses.Add(shaderPassSource);
				}
				else
				{
					_defaultPassSource.Add(line);
				}
			}
		}

		#endregion

		#region Nested type: ShaderPass

		public class ShaderPassSource
		{
			#region Public Fields

			public readonly string Name;

			#endregion

			#region Private Fields

			List<string> _lines;

			#endregion

			#region Properties

			public IReadOnlyCollection<string> Lines => _lines;

			#endregion

			#region Constructors

			public ShaderPassSource(string name)
			{
				Name = name;
				_lines = new List<string>();
			}

			#endregion

			#region Public methods

			public void AddLine(string line)
			{
				_lines.Add(line);
			}

			#endregion
		}

		#endregion
	}
}