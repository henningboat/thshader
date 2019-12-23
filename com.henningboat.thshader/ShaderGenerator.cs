using System;
using System.Collections.Generic;
using System.Text;
using THUtils.THShader.Keywords;
using THUtils.THShader.Passes;
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
			try
			{
				RemoveComments(source);

				var context = new ShaderBuildContext(source, new StringBuilder());

				GenerateOuter(context, (s) =>
				                       {
					                       AppendProperties(s);

					                       context.KeywordMap.GetKeyword<KeywordPasses>().GeneratePasses(s);
				                       });

				GeneratedShader += context.BuildString();
			}
			catch(Exception e)
			{
				Debug.LogError(e);
				GeneratedShader = DefaultErrorShader;
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

		private void AppendProperties(ShaderBuildContext context)
		{
			context.WriteLine($"Properties");
			context.WriteLine($"{{");

			context.KeywordMap.GetMultiKeywords<KeywordProperty>().ForEach(keyword => keyword.Write(context, true));

			context.WriteLine($"");
			context.WriteLine($"}}");
		}

		private void GenerateOuter(ShaderBuildContext context, Action<ShaderBuildContext> action)
		{
			context.WriteLine($"Shader \"{context.KeywordMap.GetKeyword<KeywordName>().ShaderName}\" {{");
			context.WriteIndented(action);
			context.WriteLine($"}}");
		}

		#endregion
	}

	public class ShaderBuildContext
	{
		#region Public Fields

		public readonly KeywordMap KeywordMap;
		public ShaderPass CurrentPass;
		public ShaderPassesConfig CurrentPassConfig;

		#endregion

		#region Private Fields

		private readonly StringBuilder _stringBuilder;
		private int _indentCount;
		private IReadOnlyList<string> _keymapSource;

		#endregion

		#region Constructors

		public ShaderBuildContext(IReadOnlyList<string> source, StringBuilder stringBuilder)
		{
			_stringBuilder = stringBuilder;
			_keymapSource = source;
			KeywordMap = new KeywordMap(new Queue<string>(source));
		}

		#endregion

		#region Public methods

		public string BuildString()
		{
			return _stringBuilder.ToString();
		}

		public void WriteLine(string text)
		{
			if (text == null)
				return;

			var lines = text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
			foreach (string line in lines)
			{
				for (int i = 0; i < _indentCount; i++)
				{
					_stringBuilder.Append('\t');
				}

				_stringBuilder.AppendLine(line);
			}
		}

		public void WriteIndented(Action<ShaderBuildContext> action)
		{
			Indent();
			action(this);
			Unindent();
		}

		public void WriteLineIndented(string line)
		{
			Indent();
			WriteLine(line);
			Unindent();
		}

		public ShaderBuildContext CreatePassContext(ShaderBuildContext parent, ShaderPass pass, ShaderPassesConfig config)
		{
			return new ShaderBuildContext(parent._keymapSource, parent._stringBuilder)
			       {
				       _indentCount = parent._indentCount,
				       CurrentPass = pass,
				       CurrentPassConfig = config,
        };
		}

		public void Newine()
		{
			WriteLine(Environment.NewLine);
		}

		#endregion

		#region Private methods

		private void Indent()
		{
			_indentCount++;
		}

		private void Unindent()
		{
			_indentCount--;
		}

		#endregion
	}
}