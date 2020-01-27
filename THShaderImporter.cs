using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace THUtils.THShader
{
	[ScriptedImporter(2, "thshader")]
	public class THShaderImporter : ScriptedImporter
	{
		#region Public methods

		public override void OnImportAsset(AssetImportContext ctx)
		{
			string sourceFilePath = Path.Combine(Application.dataPath + "\\..\\", ctx.assetPath);
            var data = File.ReadAllLines(sourceFilePath).ToList();

			var generator = new ShaderGenerator(data);

			Debug.Log(generator.GeneratedShader);

			string path = sourceFilePath.Replace(".thshader", ".shader");
			File.WriteAllText(path, generator.GeneratedShader);

			AssetDatabase.ImportAsset(assetPath.Replace(".thshader", ".shader"));

			//var shader = ShaderUtil.CreateShaderAsset(generator.GeneratedShader, false);
			//ctx.AddObjectToAsset("MainAsset", shader);
			//ctx.SetMainObject(shader);
		}

		public void OpenShaderInTempFile()
		{
			var shader = AssetDatabase.LoadAssetAtPath<Shader>(assetPath);

			var data = File.ReadAllLines(Path.Combine(Application.dataPath + "\\..\\", assetPath)).ToList();

			var generator = new ShaderGenerator(data);

			TempFileHandler.CreateAndOpenTempFile(shader.name + "_temp", generator.GeneratedShader);
		}

		#endregion
	}

	[CustomEditor(typeof(THShaderImporter))]
	public class THShaderImporterEditor : UnityEditor.Editor
	{
		#region Public methods

		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Open Generated Shader"))
			{
				(target as THShaderImporter).OpenShaderInTempFile();
			}
		}

		#endregion
	}
}