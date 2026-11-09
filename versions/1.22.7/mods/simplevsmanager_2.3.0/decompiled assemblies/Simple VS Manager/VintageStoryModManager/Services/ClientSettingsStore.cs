using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Threading;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ClientSettingsStore
	{
		// Token: 0x060008A9 RID: 2217 RVA: 0x00034834 File Offset: 0x00032A34
		public ClientSettingsStore(string dataDirectory)
		{
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", "dataDirectory");
			}
			this.DataDirectory = Path.GetFullPath(dataDirectory);
			Directory.CreateDirectory(this.DataDirectory);
			this.SettingsPath = Path.Combine(this.DataDirectory, "clientsettings.json");
			this._tempPath = Path.Combine(this.DataDirectory, "clientsettings.tmp");
			this._backupPath = Path.Combine(this.DataDirectory, "clientsettings.bkp");
			this.InitializeFromRoot(this.LoadOrCreateRoot());
			this.EnsureDefaultModPaths();
			this.SearchBaseCandidates = Enumerable.ToList<string>(this.BuildSearchBases());
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00034904 File Offset: 0x00032B04
		public string DataDirectory { get; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0003490C File Offset: 0x00032B0C
		public string SettingsPath { get; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00034914 File Offset: 0x00032B14
		public ReadOnlyCollection<string> DisabledEntries
		{
			get
			{
				object syncRoot = this._syncRoot;
				ReadOnlyCollection<string> result;
				lock (syncRoot)
				{
					result = new ReadOnlyCollection<string>(Enumerable.ToList<string>(this._disabledMods));
				}
				return result;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x00034960 File Offset: 0x00032B60
		public ReadOnlyCollection<string> ModPaths
		{
			get
			{
				object syncRoot = this._syncRoot;
				ReadOnlyCollection<string> result;
				lock (syncRoot)
				{
					result = new ReadOnlyCollection<string>(Enumerable.ToList<string>(this._modPaths));
				}
				return result;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x000349AC File Offset: 0x00032BAC
		[Nullable(2)]
		public string PlayerUid
		{
			[NullableContext(2)]
			get
			{
				return this.TryGetStringSettingValue("playeruid");
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x000349B9 File Offset: 0x00032BB9
		[Nullable(2)]
		public string PlayerName
		{
			[NullableContext(2)]
			get
			{
				return this.TryGetStringSettingValue("playername");
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x000349C6 File Offset: 0x00032BC6
		public IReadOnlyList<string> SearchBaseCandidates { get; }

		// Token: 0x060008B1 RID: 2225 RVA: 0x000349D0 File Offset: 0x00032BD0
		public IReadOnlyList<string> GetDisabledEntriesSnapshot()
		{
			object syncRoot = this._syncRoot;
			IReadOnlyList<string> result;
			lock (syncRoot)
			{
				result = this._disabledMods.ToArray();
			}
			return result;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00034A18 File Offset: 0x00032C18
		[NullableContext(2)]
		public bool TryReload(out string error)
		{
			object syncRoot = this._syncRoot;
			bool result;
			lock (syncRoot)
			{
				try
				{
					this.InitializeFromRoot(this.LoadOrCreateRoot());
					this.EnsureDefaultModPaths();
					error = null;
					result = true;
				}
				catch (Exception ex)
				{
					error = ex.Message;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00034A84 File Offset: 0x00032C84
		public bool IsDisabled(string modId, [Nullable(2)] string version)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return false;
			}
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				if (this._disabledLookup.Contains(modId))
				{
					return true;
				}
				if (!string.IsNullOrWhiteSpace(version))
				{
					return this._disabledLookup.Contains(modId + "@" + version);
				}
			}
			return false;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00034B00 File Offset: 0x00032D00
		[NullableContext(2)]
		public bool TrySetActive([Nullable(1)] string modId, string version, bool isActive, out string error)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				error = "Mod ID is missing.";
				return false;
			}
			return this.TryPersistDisabledStateMutation(delegate
			{
				string text = modId.Trim();
				string text2 = (!string.IsNullOrWhiteSpace(version)) ? (text + "@" + version.Trim()) : null;
				bool flag = false;
				if (isActive)
				{
					if (text2 != null)
					{
						flag |= this.RemoveDisabledEntry(text2);
					}
					flag |= this.RemoveDisabledEntry(text);
				}
				else
				{
					string text3 = text2 ?? text;
					if (!this._disabledLookup.Contains(text3))
					{
						this._disabledLookup.Add(text3);
						this._disabledMods.Add(text3);
						flag = true;
					}
				}
				return flag;
			}, out error);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00034B5C File Offset: 0x00032D5C
		public bool TryApplyDisabledEntries(IEnumerable<string> entries, [Nullable(2)] out string error)
		{
			List<string> sanitized = new List<string>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			if (entries != null)
			{
				foreach (string text in entries)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						string text2 = text.Trim();
						if (hashSet.Add(text2))
						{
							sanitized.Add(text2);
						}
					}
				}
			}
			return this.TryPersistDisabledStateMutation(delegate
			{
				if (sanitized.Count == this._disabledMods.Count && Enumerable.SequenceEqual<string>(sanitized, this._disabledMods, StringComparer.OrdinalIgnoreCase))
				{
					return false;
				}
				this._disabledMods.Clear();
				this._disabledLookup.Clear();
				foreach (string text3 in sanitized)
				{
					this._disabledMods.Add(text3);
					this._disabledLookup.Add(text3);
				}
				return true;
			}, out error);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00034C00 File Offset: 0x00032E00
		[NullableContext(2)]
		public bool TryUpdateDisabledEntry([Nullable(1)] string modId, string previousVersion, string newVersion, bool shouldDisable, out string error)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				error = "Mod ID is missing.";
				return false;
			}
			string normalizedModId = modId.Trim();
			string previousKey = ClientSettingsStore.ComposeVersionKey(normalizedModId, previousVersion);
			string newKey = ClientSettingsStore.ComposeVersionKey(normalizedModId, newVersion);
			return this.TryPersistDisabledStateMutation(delegate
			{
				bool flag = this._disabledLookup.Contains(normalizedModId);
				bool flag2 = false;
				if (previousKey != null)
				{
					flag2 |= this.RemoveDisabledEntry(previousKey);
				}
				if (!shouldDisable)
				{
					flag2 |= this.RemoveDisabledEntry(normalizedModId);
					if (newKey != null)
					{
						flag2 |= this.RemoveDisabledEntry(newKey);
					}
				}
				else if (flag)
				{
					if (!this._disabledLookup.Contains(normalizedModId))
					{
						this._disabledLookup.Add(normalizedModId);
						this._disabledMods.Add(normalizedModId);
						flag2 = true;
					}
				}
				else
				{
					string text = newKey ?? normalizedModId;
					if (!this._disabledLookup.Contains(text))
					{
						this._disabledLookup.Add(text);
						this._disabledMods.Add(text);
						flag2 = true;
					}
				}
				return flag2;
			}, out error);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00034C78 File Offset: 0x00032E78
		private bool TryPersistDisabledStateMutation(Func<bool> mutation, [Nullable(2)] out string error)
		{
			object syncRoot = this._syncRoot;
			bool result;
			lock (syncRoot)
			{
				ClientSettingsStore.DisabledStateSnapshot snapshot = this.CaptureDisabledState();
				if (!mutation.Invoke())
				{
					error = null;
					result = true;
				}
				else
				{
					try
					{
						this.Persist();
						error = null;
						result = true;
					}
					catch (Exception ex)
					{
						this.RestoreDisabledState(snapshot);
						error = "Failed to save Vintage Story client settings at '" + this.SettingsPath + "': " + ex.Message;
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00034D0C File Offset: 0x00032F0C
		private ClientSettingsStore.DisabledStateSnapshot CaptureDisabledState()
		{
			List<string> disabledMods = Enumerable.ToList<string>(this._disabledMods);
			HashSet<string> disabledLookup = new HashSet<string>(this._disabledLookup, StringComparer.OrdinalIgnoreCase);
			JsonNode jsonNode = this._stringListSettings["disabledMods"];
			return new ClientSettingsStore.DisabledStateSnapshot(disabledMods, disabledLookup, (jsonNode != null) ? jsonNode.DeepClone() : null);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00034D4C File Offset: 0x00032F4C
		private void RestoreDisabledState(ClientSettingsStore.DisabledStateSnapshot snapshot)
		{
			this._disabledMods = snapshot.DisabledMods;
			this._disabledLookup = snapshot.DisabledLookup;
			if (snapshot.DisabledModsJson == null)
			{
				this._stringListSettings.Remove("disabledMods");
				return;
			}
			this._stringListSettings["disabledMods"] = snapshot.DisabledModsJson;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00034DA4 File Offset: 0x00032FA4
		public bool RemoveInvalidDisabledEntries()
		{
			object syncRoot = this._syncRoot;
			bool result;
			lock (syncRoot)
			{
				bool flag2 = false;
				foreach (string text in Enumerable.ToList<string>(this._disabledMods))
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						string text2 = text.Trim();
						if (!text2.Contains('@'))
						{
							flag2 |= this.RemoveDisabledEntry(text2);
						}
					}
				}
				if (!flag2)
				{
					result = false;
				}
				else
				{
					this.Persist();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00034E5C File Offset: 0x0003305C
		[NullableContext(2)]
		private static string ComposeVersionKey([Nullable(1)] string modId, string version)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				return null;
			}
			return modId + '@' + version.Trim();
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00034E7C File Offset: 0x0003307C
		private bool RemoveDisabledEntry(string key)
		{
			if (!this._disabledLookup.Remove(key))
			{
				return false;
			}
			this._disabledMods.RemoveAll((string value) => string.Equals(value, key, 5));
			return true;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00034EC4 File Offset: 0x000330C4
		private JsonObject LoadOrCreateRoot()
		{
			if (!File.Exists(this.SettingsPath))
			{
				return this.CreateDefaultSettings();
			}
			JsonObject result;
			try
			{
				using (FileStream fileStream = File.OpenRead(this.SettingsPath))
				{
					JsonNode jsonNode = JsonNode.Parse(fileStream, default(JsonNodeOptions?), default(JsonDocumentOptions));
					result = (((jsonNode != null) ? jsonNode.AsObject() : null) ?? this.CreateDefaultSettings());
				}
			}
			catch (JsonException)
			{
				result = this.CreateDefaultSettings();
			}
			return result;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00034F54 File Offset: 0x00033154
		private JsonObject CreateDefaultSettings()
		{
			JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
			jsonObject["boolSettings"] = new JsonObject(default(JsonNodeOptions?));
			jsonObject["intSettings"] = new JsonObject(default(JsonNodeOptions?));
			jsonObject["floatSettings"] = new JsonObject(default(JsonNodeOptions?));
			jsonObject["stringSettings"] = new JsonObject(default(JsonNodeOptions?));
			string text = "stringListSettings";
			JsonObject jsonObject2 = new JsonObject(default(JsonNodeOptions?));
			jsonObject2["disabledMods"] = new JsonArray(default(JsonNodeOptions?));
			string text2 = "modPaths";
			JsonArray jsonArray = new JsonArray(default(JsonNodeOptions?));
			jsonArray.Add<string>("Mods");
			jsonArray.Add<string>(Path.Combine(this.DataDirectory, "Mods"));
			jsonObject2[text2] = jsonArray;
			jsonObject[text] = jsonObject2;
			return jsonObject;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00035040 File Offset: 0x00033240
		private static JsonObject GetOrCreateObject(JsonObject owner, string propertyName)
		{
			foreach (KeyValuePair<string, JsonNode> keyValuePair in owner)
			{
				if (string.Equals(keyValuePair.Key, propertyName, 5))
				{
					JsonObject jsonObject = keyValuePair.Value as JsonObject;
					if (jsonObject != null)
					{
						return jsonObject;
					}
					JsonObject jsonObject2 = new JsonObject(default(JsonNodeOptions?));
					owner[propertyName] = jsonObject2;
					return jsonObject2;
				}
			}
			JsonObject jsonObject3 = new JsonObject(default(JsonNodeOptions?));
			owner[propertyName] = jsonObject3;
			return jsonObject3;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x000350E4 File Offset: 0x000332E4
		private static List<string> ExtractStringList(JsonObject container, string propertyName)
		{
			foreach (KeyValuePair<string, JsonNode> keyValuePair in container)
			{
				if (string.Equals(keyValuePair.Key, propertyName, 5))
				{
					JsonArray jsonArray = keyValuePair.Value as JsonArray;
					if (jsonArray != null)
					{
						return Enumerable.ToList<string>(Enumerable.Select<JsonNode, string>(jsonArray, (JsonNode node) => ((node != null) ? node.GetValue<string>() : null) ?? string.Empty));
					}
					break;
				}
			}
			List<string> result = new List<string>();
			container[propertyName] = new JsonArray(default(JsonNodeOptions?));
			return result;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00035190 File Offset: 0x00033390
		private void InitializeFromRoot(JsonObject root)
		{
			this._root = root;
			this._stringListSettings = ClientSettingsStore.GetOrCreateObject(this._root, "stringListSettings");
			this._stringSettings = ClientSettingsStore.GetOrCreateObject(this._root, "stringSettings");
			this._disabledMods = ClientSettingsStore.ExtractStringList(this._stringListSettings, "disabledMods");
			this._disabledLookup = new HashSet<string>(this._disabledMods, StringComparer.OrdinalIgnoreCase);
			this._modPaths = ClientSettingsStore.ExtractStringList(this._stringListSettings, "modPaths");
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00035214 File Offset: 0x00033414
		private void EnsureDefaultModPaths()
		{
			object syncRoot = this._syncRoot;
			bool flag = false;
			try
			{
				Monitor.Enter(syncRoot, ref flag);
				string expectedDataModsPath = Path.Combine(this.DataDirectory, "Mods");
				bool flag2 = false;
				if (this._modPaths.Count == 0)
				{
					this._modPaths.Add("Mods");
					this._modPaths.Add(expectedDataModsPath);
					flag2 = true;
				}
				else
				{
					if (!string.Equals(this._modPaths[0], "Mods", 5))
					{
						int num = this._modPaths.FindIndex((string path) => string.Equals(path, "Mods", 5));
						if (num >= 0)
						{
							this._modPaths.RemoveAt(num);
						}
						this._modPaths.Insert(0, "Mods");
						flag2 = true;
					}
					if (this._modPaths.Count < 2)
					{
						this._modPaths.Insert(1, expectedDataModsPath);
						flag2 = true;
					}
					else if (!ClientSettingsStore.PathsEqual(this._modPaths[1], expectedDataModsPath))
					{
						int num2 = this._modPaths.FindIndex((string path) => ClientSettingsStore.PathsEqual(path, expectedDataModsPath));
						if (num2 >= 0)
						{
							this._modPaths.RemoveAt(num2);
						}
						this._modPaths[1] = expectedDataModsPath;
						flag2 = true;
					}
				}
				if (flag2)
				{
					this.Persist();
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(syncRoot);
				}
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0003539C File Offset: 0x0003359C
		[NullableContext(2)]
		private static bool PathsEqual(string left, string right)
		{
			if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
			{
				return string.Equals((left != null) ? left.Trim() : null, (right != null) ? right.Trim() : null, 5);
			}
			string text = ClientSettingsStore.NormalizePath(left);
			string text2 = ClientSettingsStore.NormalizePath(right);
			return string.Equals(text, text2, 5);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x000353EC File Offset: 0x000335EC
		[NullableContext(2)]
		private static string NormalizePath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return path;
			}
			string result;
			try
			{
				result = Path.GetFullPath(path);
			}
			catch (Exception)
			{
				result = path.Trim();
			}
			return result;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00035428 File Offset: 0x00033628
		private IEnumerable<string> BuildSearchBases()
		{
			List<string> list = new List<string>();
			list.Add(this.DataDirectory);
			list.Add(AppContext.BaseDirectory);
			list.Add(Directory.GetCurrentDirectory());
			return Enumerable.ToList<string>(Enumerable.Distinct<string>(list, StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00035460 File Offset: 0x00033660
		[return: Nullable(2)]
		private string TryGetStringSettingValue(string settingName)
		{
			if (string.IsNullOrWhiteSpace(settingName))
			{
				return null;
			}
			object syncRoot = this._syncRoot;
			string result;
			lock (syncRoot)
			{
				foreach (KeyValuePair<string, JsonNode> keyValuePair in this._stringSettings)
				{
					if (string.Equals(keyValuePair.Key, settingName, 5))
					{
						if (keyValuePair.Value == null)
						{
							return null;
						}
						string value;
						try
						{
							value = keyValuePair.Value.GetValue<string>();
						}
						catch (InvalidOperationException)
						{
							return null;
						}
						return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00035538 File Offset: 0x00033738
		private void Persist()
		{
			object syncRoot = this._syncRoot;
			lock (syncRoot)
			{
				JsonNode stringListSettings = this._stringListSettings;
				string text = "disabledMods";
				JsonNode[] array = Enumerable.ToArray<JsonValue>(Enumerable.Select<string, JsonValue>(this._disabledMods, (string value) => JsonValue.Create(value, default(JsonNodeOptions?))));
				stringListSettings[text] = new JsonArray(array);
				JsonNode stringListSettings2 = this._stringListSettings;
				string text2 = "modPaths";
				array = Enumerable.ToArray<JsonValue>(Enumerable.Select<string, JsonValue>(this._modPaths, (string path) => JsonValue.Create(path, default(JsonNodeOptions?))));
				stringListSettings2[text2] = new JsonArray(array);
				string text3 = this._root.ToJsonString(this._serializerOptions);
				File.WriteAllText(this._tempPath, text3);
				if (File.Exists(this.SettingsPath))
				{
					File.Replace(this._tempPath, this.SettingsPath, this._backupPath);
				}
				else
				{
					File.Move(this._tempPath, this.SettingsPath);
				}
			}
		}

		// Token: 0x04000444 RID: 1092
		private readonly string _backupPath;

		// Token: 0x04000445 RID: 1093
		private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver()
		};

		// Token: 0x04000446 RID: 1094
		private readonly object _syncRoot = new object();

		// Token: 0x04000447 RID: 1095
		private readonly string _tempPath;

		// Token: 0x04000448 RID: 1096
		private HashSet<string> _disabledLookup;

		// Token: 0x04000449 RID: 1097
		private List<string> _disabledMods;

		// Token: 0x0400044A RID: 1098
		private List<string> _modPaths;

		// Token: 0x0400044B RID: 1099
		private JsonObject _root;

		// Token: 0x0400044C RID: 1100
		private JsonObject _stringListSettings;

		// Token: 0x0400044D RID: 1101
		private JsonObject _stringSettings;

		// Token: 0x02000264 RID: 612
		[Nullable(0)]
		private sealed class DisabledStateSnapshot : IEquatable<ClientSettingsStore.DisabledStateSnapshot>
		{
			// Token: 0x06001559 RID: 5465 RVA: 0x0007E12E File Offset: 0x0007C32E
			public DisabledStateSnapshot(List<string> DisabledMods, HashSet<string> DisabledLookup, [Nullable(2)] JsonNode DisabledModsJson)
			{
				this.DisabledMods = DisabledMods;
				this.DisabledLookup = DisabledLookup;
				this.DisabledModsJson = DisabledModsJson;
				base..ctor();
			}

			// Token: 0x1700046D RID: 1133
			// (get) Token: 0x0600155A RID: 5466 RVA: 0x0007E14B File Offset: 0x0007C34B
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ClientSettingsStore.DisabledStateSnapshot);
				}
			}

			// Token: 0x1700046E RID: 1134
			// (get) Token: 0x0600155B RID: 5467 RVA: 0x0007E157 File Offset: 0x0007C357
			// (set) Token: 0x0600155C RID: 5468 RVA: 0x0007E15F File Offset: 0x0007C35F
			public List<string> DisabledMods { get; set; }

			// Token: 0x1700046F RID: 1135
			// (get) Token: 0x0600155D RID: 5469 RVA: 0x0007E168 File Offset: 0x0007C368
			// (set) Token: 0x0600155E RID: 5470 RVA: 0x0007E170 File Offset: 0x0007C370
			public HashSet<string> DisabledLookup { get; set; }

			// Token: 0x17000470 RID: 1136
			// (get) Token: 0x0600155F RID: 5471 RVA: 0x0007E179 File Offset: 0x0007C379
			// (set) Token: 0x06001560 RID: 5472 RVA: 0x0007E181 File Offset: 0x0007C381
			[Nullable(2)]
			public JsonNode DisabledModsJson { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x06001561 RID: 5473 RVA: 0x0007E18C File Offset: 0x0007C38C
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("DisabledStateSnapshot");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001562 RID: 5474 RVA: 0x0007E1D8 File Offset: 0x0007C3D8
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("DisabledMods = ");
				builder.Append(this.DisabledMods);
				builder.Append(", DisabledLookup = ");
				builder.Append(this.DisabledLookup);
				builder.Append(", DisabledModsJson = ");
				builder.Append(this.DisabledModsJson);
				return true;
			}

			// Token: 0x06001563 RID: 5475 RVA: 0x0007E236 File Offset: 0x0007C436
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ClientSettingsStore.DisabledStateSnapshot left, ClientSettingsStore.DisabledStateSnapshot right)
			{
				return !(left == right);
			}

			// Token: 0x06001564 RID: 5476 RVA: 0x0007E242 File Offset: 0x0007C442
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ClientSettingsStore.DisabledStateSnapshot left, ClientSettingsStore.DisabledStateSnapshot right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06001565 RID: 5477 RVA: 0x0007E258 File Offset: 0x0007C458
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.<DisabledMods>k__BackingField)) * -1521134295 + EqualityComparer<HashSet<string>>.Default.GetHashCode(this.<DisabledLookup>k__BackingField)) * -1521134295 + EqualityComparer<JsonNode>.Default.GetHashCode(this.<DisabledModsJson>k__BackingField);
			}

			// Token: 0x06001566 RID: 5478 RVA: 0x0007E2BA File Offset: 0x0007C4BA
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ClientSettingsStore.DisabledStateSnapshot);
			}

			// Token: 0x06001567 RID: 5479 RVA: 0x0007E2C8 File Offset: 0x0007C4C8
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ClientSettingsStore.DisabledStateSnapshot other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<List<string>>.Default.Equals(this.<DisabledMods>k__BackingField, other.<DisabledMods>k__BackingField) && EqualityComparer<HashSet<string>>.Default.Equals(this.<DisabledLookup>k__BackingField, other.<DisabledLookup>k__BackingField) && EqualityComparer<JsonNode>.Default.Equals(this.<DisabledModsJson>k__BackingField, other.<DisabledModsJson>k__BackingField));
			}

			// Token: 0x06001569 RID: 5481 RVA: 0x0007E341 File Offset: 0x0007C541
			[CompilerGenerated]
			private DisabledStateSnapshot(ClientSettingsStore.DisabledStateSnapshot original)
			{
				this.DisabledMods = original.<DisabledMods>k__BackingField;
				this.DisabledLookup = original.<DisabledLookup>k__BackingField;
				this.DisabledModsJson = original.<DisabledModsJson>k__BackingField;
			}

			// Token: 0x0600156A RID: 5482 RVA: 0x0007E36D File Offset: 0x0007C56D
			[CompilerGenerated]
			public void Deconstruct(out List<string> DisabledMods, out HashSet<string> DisabledLookup, [Nullable(2)] out JsonNode DisabledModsJson)
			{
				DisabledMods = this.DisabledMods;
				DisabledLookup = this.DisabledLookup;
				DisabledModsJson = this.DisabledModsJson;
			}
		}
	}
}
