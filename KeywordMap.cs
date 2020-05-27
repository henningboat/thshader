using System;
using System.Collections.Generic;
using System.Linq;
using THUtils.THShader.Keywords;
using UnityEditor;

namespace THUtils.THShader
{
	public class KeywordMap
	{
		#region Private Fields

		private Dictionary<Type, SingletonKeyword> _singletonKeywords = new Dictionary<Type, SingletonKeyword>();
		private Dictionary<Type, List<MultiKeyword>> _multiKeywords = new Dictionary<Type, List<MultiKeyword>>();

		#endregion

		#region Constructors

		internal KeywordMap(Queue<string> sourceLines)
		{
			var keywordTypes = TypeCache.GetTypesDerivedFrom<Keyword>().Select(type => type).Where(type => !type.IsAbstract).ToList();

			while (sourceLines.Count > 0)
			{
				ParseKeyword(sourceLines, keywordTypes, false);
			}

			foreach (Type type in keywordTypes)
			{
				if (!_singletonKeywords.ContainsKey(type))
				{
					_singletonKeywords[type] = Activator.CreateInstance(type, (Queue<string>) null) as SingletonKeyword;
				}
			}
		}

		#endregion

		#region Public methods

		public T GetKeyword<T>() where T : SingletonKeyword
		{
			return (T) _singletonKeywords[typeof(T)];
		}

		public List<T> GetMultiKeywords<T>() where T : MultiKeyword
		{
			if (!_multiKeywords.ContainsKey(typeof(T)))
			{
				return new List<T>();
			}

			return _multiKeywords[typeof(T)].Cast<T>().ToList();
		}

		public List<T> GetKeywords<T>() where T : SingletonKeyword
		{
			List<T> results = new List<T>();
			foreach (KeyValuePair<Type, SingletonKeyword> keyword in _singletonKeywords)
			{
				if (typeof(T).IsAssignableFrom(keyword.Key))
				{
					results.Add((T) keyword.Value);
				}
			}

			return results;
		}

		public void AddPassCode(SourceMap.ShaderPassSource shaderPassSource)
		{
			var keywordTypes = TypeCache.GetTypesDerivedFrom<Keyword>().Select(type => type).Where(type => !type.IsAbstract).ToList();

			var sourceLines = new Queue<string>(shaderPassSource.Lines);

			while (sourceLines.Count > 0)
			{
				ParseKeyword(sourceLines, keywordTypes, true);
			}
		}

		#endregion

		#region Private methods

		private void ParseKeyword(Queue<string> sourceLines, List<Type> keywordTypes, bool allowOverwrite)
		{
			string[] lineComponents = ShaderGenerator.SplitLine(sourceLines.Peek());
			if (lineComponents.Length == 0 || lineComponents[0] == "")
			{
				sourceLines.Dequeue();
				return;
			}

			string keywordTypeName = "Keyword" + lineComponents[0];

			foreach (Type type in keywordTypes)
			{
				if (type.Name == keywordTypeName)
				{
					Keyword keyword = Activator.CreateInstance(type, sourceLines) as Keyword;

					if (typeof(SingletonKeyword).IsAssignableFrom(type))
					{
						if (_singletonKeywords.ContainsKey(type) && !allowOverwrite)
						{
							throw new ShaderGenerationException("Duplicated keyword of type " + type);
						}

						_singletonKeywords[type] = keyword as SingletonKeyword;
					}
					else
					{
						if (!_multiKeywords.ContainsKey(type))
						{
							_multiKeywords[type] = new List<MultiKeyword>();
						}

						_multiKeywords[type].Add(keyword as MultiKeyword);
					}

					return;
				}
			}

			throw new ShaderGenerationException(keywordTypeName);
		}

		#endregion

		#region Nested type: ShaderGenerationException

		internal class ShaderGenerationException : Exception
		{
			#region Constructors

			internal ShaderGenerationException(string message) : base(message)
			{
			}

			#endregion
		}

		#endregion
	}
}