using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace THUtils.THShader
{
	public static class TempFileHandler
	{
		#region Static Stuff

		public static void CreateAndOpenTempFile(string shaderName  ,string shaderCode)
		{
			string uniqueTempPathInProject = FileUtil.GetUniqueTempPathInProject();
			string tempFilePath = Path.Combine(Application.dataPath + "/../", Path.Combine(uniqueTempPathInProject, Path.GetFileNameWithoutExtension(shaderName) + ".shader"));
			string directoryPath = Path.GetDirectoryName(tempFilePath);
			Directory.CreateDirectory(directoryPath);

			File.WriteAllText(tempFilePath, shaderCode);
			
			Process.Start(tempFilePath);
		}

		#endregion

		#region Nested type: ShaderReference

		private class ShaderReference
		{
			#region Public Fields

			public readonly Shader Shader;
			public readonly FileSystemWatcher Watcher;
			public string ShaderFilePath;
			public string TempFilePath;

			#endregion

			#region Constructors

			public ShaderReference(FileSystemWatcher watcher, Shader shader, string tempFilePath, string shaderFilePath)
			{
				Shader = shader;
				TempFilePath = tempFilePath;
				ShaderFilePath = shaderFilePath;
				Watcher = watcher;
			}

			#endregion
		}

		#endregion
	}
}