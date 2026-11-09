using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using YamlDotNet.Serialization;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x0200004B RID: 75
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModConfigurationViewModel : ObservableObject
	{
		// Token: 0x06000764 RID: 1892 RVA: 0x0002ED23 File Offset: 0x0002CF23
		public ModConfigurationViewModel(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("Configuration file path is required.", "filePath");
			}
			this._filePath = ModConfigurationViewModel.NormalizePath(filePath);
			this.LoadConfiguration();
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0002ED60 File Offset: 0x0002CF60
		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.FilePath))
				{
					return Path.GetFileName(this.FilePath);
				}
				return "Config";
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0002ED80 File Offset: 0x0002CF80
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x0002ED88 File Offset: 0x0002CF88
		public string FilePath
		{
			get
			{
				return this._filePath;
			}
			private set
			{
				if (string.Equals(this._filePath, value, 5))
				{
					return;
				}
				this._filePath = value;
				base.OnPropertyChanged("FilePath");
				base.OnPropertyChanged("DisplayName");
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x0002EDB7 File Offset: 0x0002CFB7
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x0002EDBF File Offset: 0x0002CFBF
		public ObservableCollection<ModConfigNodeViewModel> RootNodes
		{
			get
			{
				return this._rootNodes;
			}
			private set
			{
				if (this._rootNodes == value)
				{
					return;
				}
				this._rootNodes = value;
				base.OnPropertyChanged("RootNodes");
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002EDE0 File Offset: 0x0002CFE0
		public void Save()
		{
			foreach (ModConfigNodeViewModel modConfigNodeViewModel in this.RootNodes)
			{
				modConfigNodeViewModel.ApplyChanges();
			}
			ModConfigurationViewModel.ModConfigFormat format = this._format;
			string text;
			if (format != ModConfigurationViewModel.ModConfigFormat.Json)
			{
				if (format != ModConfigurationViewModel.ModConfigFormat.Yaml)
				{
					throw new InvalidOperationException("Unsupported configuration format.");
				}
				text = ModConfigurationViewModel.SerializeYaml(this._rootNode);
			}
			else
			{
				text = ModConfigurationViewModel.SerializeJson(this._rootNode);
			}
			string text2 = text;
			File.WriteAllText(this._filePath, text2);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0002EE70 File Offset: 0x0002D070
		public void ReplaceConfigurationFile(string filePath)
		{
			string text = ModConfigurationViewModel.NormalizePath(filePath);
			this._format = ModConfigurationViewModel.DetermineFormat(text);
			ModConfigurationViewModel.ModConfigFormat format = this._format;
			JsonNode jsonNode;
			if (format != ModConfigurationViewModel.ModConfigFormat.Json)
			{
				if (format != ModConfigurationViewModel.ModConfigFormat.Yaml)
				{
					jsonNode = null;
				}
				else
				{
					jsonNode = ModConfigurationViewModel.LoadYamlNode(text);
				}
			}
			else
			{
				jsonNode = ModConfigurationViewModel.LoadJsonNode(text);
			}
			JsonNode jsonNode2 = jsonNode;
			JsonNode rootNode;
			if ((rootNode = jsonNode2) == null)
			{
				rootNode = new JsonObject(default(JsonNodeOptions?));
			}
			this._rootNode = rootNode;
			this.RootNodes = new ObservableCollection<ModConfigNodeViewModel>(ModConfigurationViewModel.CreateRootNodes(this._rootNode, delegate(JsonNode value)
			{
				this._rootNode = value;
			}));
			this.FilePath = text;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0002EEF9 File Offset: 0x0002D0F9
		private void LoadConfiguration()
		{
			this.ReplaceConfigurationFile(this._filePath);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002EF08 File Offset: 0x0002D108
		private static ModConfigurationViewModel.ModConfigFormat DetermineFormat(string filePath)
		{
			string extension = Path.GetExtension(filePath);
			if (extension.Equals(".yaml", 5) || extension.Equals(".yml", 5))
			{
				return ModConfigurationViewModel.ModConfigFormat.Yaml;
			}
			return ModConfigurationViewModel.ModConfigFormat.Json;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002EF3C File Offset: 0x0002D13C
		[return: Nullable(2)]
		private static JsonNode LoadJsonNode(string path)
		{
			JsonNode result;
			using (FileStream fileStream = File.OpenRead(path))
			{
				result = JsonNode.Parse(fileStream, default(JsonNodeOptions?), default(JsonDocumentOptions));
			}
			return result;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002EF88 File Offset: 0x0002D188
		[return: Nullable(2)]
		private static JsonNode LoadYamlNode(string path)
		{
			IDeserializer deserializer = new DeserializerBuilder().Build();
			JsonNode result;
			using (StreamReader streamReader = new StreamReader(path))
			{
				result = ModConfigurationViewModel.ConvertYamlToJsonNode(deserializer.Deserialize(streamReader));
			}
			return result;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0002EFD4 File Offset: 0x0002D1D4
		[NullableContext(2)]
		private static JsonNode ConvertYamlToJsonNode(object value)
		{
			if (value == null)
			{
				return null;
			}
			IDictionary dictionary = value as IDictionary;
			if (dictionary != null)
			{
				JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
				foreach (object obj in dictionary)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					object key = dictionaryEntry.Key;
					string text = ((key != null) ? key.ToString() : null) ?? string.Empty;
					jsonObject[text] = ModConfigurationViewModel.ConvertYamlToJsonNode(dictionaryEntry.Value);
				}
				return jsonObject;
			}
			IList list = value as IList;
			if (list != null)
			{
				JsonArray jsonArray = new JsonArray(default(JsonNodeOptions?));
				foreach (object value2 in list)
				{
					jsonArray.Add(ModConfigurationViewModel.ConvertYamlToJsonNode(value2));
				}
				return jsonArray;
			}
			return ModConfigurationViewModel.ConvertToJsonValue(value);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0002F0EC File Offset: 0x0002D2EC
		private static JsonNode ConvertToJsonValue(object value)
		{
			JsonValue result;
			if (value is bool)
			{
				bool flag = (bool)value;
				result = JsonValue.Create(flag, default(JsonNodeOptions?));
			}
			else
			{
				string text = value as string;
				if (text == null)
				{
					if (value is sbyte)
					{
						sbyte b = (sbyte)value;
						result = JsonValue.Create(b, default(JsonNodeOptions?));
					}
					else if (value is byte)
					{
						byte b2 = (byte)value;
						result = JsonValue.Create(b2, default(JsonNodeOptions?));
					}
					else if (value is short)
					{
						short num = (short)value;
						result = JsonValue.Create(num, default(JsonNodeOptions?));
					}
					else if (value is ushort)
					{
						ushort num2 = (ushort)value;
						result = JsonValue.Create(num2, default(JsonNodeOptions?));
					}
					else if (value is int)
					{
						int num3 = (int)value;
						result = JsonValue.Create(num3, default(JsonNodeOptions?));
					}
					else if (value is uint)
					{
						uint num4 = (uint)value;
						result = JsonValue.Create(num4, default(JsonNodeOptions?));
					}
					else if (value is long)
					{
						long num5 = (long)value;
						result = JsonValue.Create(num5, default(JsonNodeOptions?));
					}
					else if (value is ulong)
					{
						ulong num6 = (ulong)value;
						result = JsonValue.Create(num6, default(JsonNodeOptions?));
					}
					else if (value is float)
					{
						float num7 = (float)value;
						result = JsonValue.Create(num7, default(JsonNodeOptions?));
					}
					else if (value is double)
					{
						double num8 = (double)value;
						result = JsonValue.Create(num8, default(JsonNodeOptions?));
					}
					else if (value is decimal)
					{
						decimal num9 = (decimal)value;
						result = JsonValue.Create(num9, default(JsonNodeOptions?));
					}
					else
					{
						result = JsonValue.Create(value.ToString() ?? string.Empty, default(JsonNodeOptions?));
					}
				}
				else
				{
					result = JsonValue.Create(text, default(JsonNodeOptions?));
				}
			}
			return result;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0002F349 File Offset: 0x0002D549
		private static string SerializeJson([Nullable(2)] JsonNode node)
		{
			if (node != null)
			{
				return node.ToJsonString(ModConfigurationViewModel.JsonSerializationOptions);
			}
			return "null";
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0002F360 File Offset: 0x0002D560
		private static string SerializeYaml([Nullable(2)] JsonNode node)
		{
			object obj = ModConfigurationViewModel.ConvertJsonNodeToYamlObject(node);
			ISerializer serializer = new SerializerBuilder().Build();
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				serializer.Serialize(stringWriter, obj);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002F3B4 File Offset: 0x0002D5B4
		[NullableContext(2)]
		private static object ConvertJsonNodeToYamlObject(JsonNode node)
		{
			if (node == null)
			{
				return null;
			}
			JsonObject jsonObject = node as JsonObject;
			if (jsonObject != null)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
				foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject)
				{
					dictionary[keyValuePair.Key] = ModConfigurationViewModel.ConvertJsonNodeToYamlObject(keyValuePair.Value);
				}
				return dictionary;
			}
			JsonArray jsonArray = node as JsonArray;
			if (jsonArray != null)
			{
				List<object> list = new List<object>(jsonArray.Count);
				foreach (JsonNode node2 in jsonArray)
				{
					list.Add(ModConfigurationViewModel.ConvertJsonNodeToYamlObject(node2));
				}
				return list;
			}
			JsonValue jsonValue = node as JsonValue;
			if (jsonValue == null)
			{
				return null;
			}
			return ModConfigurationViewModel.ConvertJsonValueToPrimitive(jsonValue);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002F4A8 File Offset: 0x0002D6A8
		[return: Nullable(2)]
		private static object ConvertJsonValueToPrimitive(JsonValue value)
		{
			string text = value.ToJsonString(null);
			if (string.Equals(text, "null", 5))
			{
				return null;
			}
			bool flag;
			if (value.TryGetValue<bool>(ref flag))
			{
				return flag;
			}
			long num;
			if (value.TryGetValue<long>(ref num))
			{
				return num;
			}
			ulong num2;
			if (value.TryGetValue<ulong>(ref num2))
			{
				return num2;
			}
			decimal num3;
			if (value.TryGetValue<decimal>(ref num3))
			{
				return num3;
			}
			double num4;
			if (value.TryGetValue<double>(ref num4))
			{
				return num4;
			}
			string result;
			if (value.TryGetValue<string>(ref result))
			{
				return result;
			}
			return text;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0002F532 File Offset: 0x0002D732
		private static string NormalizePath(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("Configuration file path is required.", "filePath");
			}
			return Path.GetFullPath(filePath);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0002F554 File Offset: 0x0002D754
		private static IEnumerable<ModConfigNodeViewModel> CreateRootNodes(JsonNode node, [Nullable(new byte[]
		{
			1,
			2
		})] Action<JsonNode> rootSetter)
		{
			JsonObject obj = node as JsonObject;
			if (obj != null)
			{
				return Enumerable.ToList<ModConfigNodeViewModel>(Enumerable.Select<KeyValuePair<string, JsonNode>, ModConfigNodeViewModel>(obj, (KeyValuePair<string, JsonNode> pair) => ModConfigurationViewModel.CreateNode(pair.Key, pair.Value, delegate(JsonNode value)
				{
					obj[pair.Key] = value;
				}, null)));
			}
			JsonArray jsonArray = node as JsonArray;
			if (jsonArray != null)
			{
				return new ModConfigNodeViewModel[]
				{
					ModConfigurationViewModel.CreateArrayNode("(root)", jsonArray, "Items")
				};
			}
			return new ModConfigNodeViewModel[]
			{
				ModConfigurationViewModel.CreateValueNode("(root)", node, rootSetter, string.Empty)
			};
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002F5D8 File Offset: 0x0002D7D8
		private static ModConfigNodeViewModel CreateNode(string name, [Nullable(2)] JsonNode node, [Nullable(new byte[]
		{
			1,
			2
		})] Action<JsonNode> setter, [Nullable(2)] string displayName = null)
		{
			JsonObject obj = node as JsonObject;
			if (obj != null)
			{
				ObservableCollection<ModConfigNodeViewModel> children = new ObservableCollection<ModConfigNodeViewModel>(Enumerable.Select<KeyValuePair<string, JsonNode>, ModConfigNodeViewModel>(obj, (KeyValuePair<string, JsonNode> pair) => ModConfigurationViewModel.CreateNode(pair.Key, pair.Value, delegate(JsonNode value)
				{
					obj[pair.Key] = value;
				}, null)));
				return new ModConfigObjectNodeViewModel(name, children, displayName);
			}
			JsonArray jsonArray = node as JsonArray;
			if (jsonArray != null)
			{
				return ModConfigurationViewModel.CreateArrayNode(name, jsonArray, displayName);
			}
			return ModConfigurationViewModel.CreateValueNode(name, node, setter, displayName);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0002F644 File Offset: 0x0002D844
		private static ModConfigNodeViewModel CreateArrayNode(string name, JsonArray array, [Nullable(2)] string displayName = null)
		{
			ObservableCollection<ModConfigNodeViewModel> observableCollection = new ObservableCollection<ModConfigNodeViewModel>();
			for (int i = 0; i < array.Count; i++)
			{
				int index = i;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				ModConfigNodeViewModel modConfigNodeViewModel = ModConfigurationViewModel.CreateNode(defaultInterpolatedStringHandler.ToStringAndClear(), array[index], delegate(JsonNode value)
				{
					array[index] = value;
				}, displayName);
				observableCollection.Add(modConfigNodeViewModel);
			}
			return new ModConfigArrayNodeViewModel(name, observableCollection, () => array.Count, displayName);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002F707 File Offset: 0x0002D907
		private static ModConfigNodeViewModel CreateValueNode(string name, [Nullable(2)] JsonNode node, [Nullable(new byte[]
		{
			1,
			2
		})] Action<JsonNode> setter, [Nullable(2)] string displayName = null)
		{
			return new ModConfigValueNodeViewModel(name, node, setter, displayName);
		}

		// Token: 0x040003C6 RID: 966
		private string _filePath;

		// Token: 0x040003C7 RID: 967
		private ModConfigurationViewModel.ModConfigFormat _format;

		// Token: 0x040003C8 RID: 968
		[Nullable(2)]
		private JsonNode _rootNode;

		// Token: 0x040003C9 RID: 969
		private ObservableCollection<ModConfigNodeViewModel> _rootNodes = new ObservableCollection<ModConfigNodeViewModel>();

		// Token: 0x040003CA RID: 970
		private static readonly JsonSerializerOptions JsonSerializationOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver()
		};

		// Token: 0x02000250 RID: 592
		[NullableContext(0)]
		private enum ModConfigFormat
		{
			// Token: 0x04000E90 RID: 3728
			Json,
			// Token: 0x04000E91 RID: 3729
			Yaml
		}
	}
}
