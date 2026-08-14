using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ConfigLib.Formatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using YamlDotNet.Serialization;

namespace ConfigLib
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Config : IConfig, IDisposable
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00004268 File Offset: 0x00002468
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00004270 File Offset: 0x00002470
		public string ConfigFilePath { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00004279 File Offset: 0x00002479
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00004281 File Offset: 0x00002481
		public int Version { get; private set; }

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600008D RID: 141 RVA: 0x0000428C File Offset: 0x0000248C
		// (remove) Token: 0x0600008E RID: 142 RVA: 0x000042C4 File Offset: 0x000024C4
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<Config> ConfigSaved;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600008F RID: 143 RVA: 0x000042FC File Offset: 0x000024FC
		// (remove) Token: 0x06000090 RID: 144 RVA: 0x00004334 File Offset: 0x00002534
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<ISetting> SettingChanged;

		// Token: 0x06000091 RID: 145 RVA: 0x0000436C File Offset: 0x0000256C
		public Config(ICoreAPI api, string domain, string modName, JsonObject json)
		{
			this._api = api;
			this._domain = domain;
			this._modName = modName;
			this._json = json;
			this.RelativeFilePath = this._domain + ".yaml";
			this.ConfigFilePath = Path.Combine(this._api.DataBasePath, "ModConfig", this.RelativeFilePath);
			try
			{
				this.Parse(json, out this._settings, out this._configBlocks, out this._defaultYaml, domain, true);
				this._clientSideSettings = this._settings;
				this.WriteToFile();
				this._patches = new ConfigPatches(api, this);
				this.CreateFileWatcher();
				this.SubscribeToSettingsChanges();
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] (");
				defaultInterpolatedStringHandler.AppendFormatted(domain);
				defaultInterpolatedStringHandler.AppendLiteral(") Error on parsing config: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this._patches = new ConfigPatches(api, this);
				this._settings = new Dictionary<string, ConfigSetting>();
				this._configBlocks = new SortedDictionary<float, IConfigBlock>();
				this._defaultYaml = "";
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000044F8 File Offset: 0x000026F8
		public Config(ICoreAPI api, string domain, string modName, JsonObject json, Dictionary<string, ConfigSetting> serverSideSettings)
		{
			this._api = api;
			this._domain = domain;
			this._modName = modName;
			this._json = json;
			this.RelativeFilePath = this._domain + ".yaml";
			this.ConfigFilePath = Path.Combine(this._api.DataBasePath, "ModConfig", this.RelativeFilePath);
			try
			{
				this.Parse(json, out this._settings, out this._configBlocks, out this._defaultYaml, domain, false);
				this.DistributeSettingsBySides(serverSideSettings);
				this.WriteToFile();
				this._patches = new ConfigPatches(api, this);
				this.CreateFileWatcher();
				this.SubscribeToSettingsChanges();
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] (");
				defaultInterpolatedStringHandler.AppendFormatted(domain);
				defaultInterpolatedStringHandler.AppendLiteral(") Error on parsing config: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this._patches = new ConfigPatches(api, this);
				this._settings = new Dictionary<string, ConfigSetting>();
				this._configBlocks = new SortedDictionary<float, IConfigBlock>();
				this._defaultYaml = "";
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004680 File Offset: 0x00002880
		public Config(ICoreAPI api, string domain, string modName, JsonObject json, string file)
		{
			this._api = api;
			this._domain = domain;
			this._modName = modName;
			this._json = json;
			this.RelativeFilePath = file;
			this.ConfigFilePath = Path.Combine(this._api.DataBasePath, "ModConfig", file);
			this.JsonFilePath = this.ConfigFilePath;
			this._configType = Config.ConfigType.JSON;
			try
			{
				this.ParseJson(json, out this._settings, out this._configBlocks, out this._defaultJson, domain);
				this._clientSideSettings = this._settings;
				this._patches = new ConfigPatches(api, this);
				this.CreateFileWatcher();
				this.SubscribeToSettingsChanges();
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] (");
				defaultInterpolatedStringHandler.AppendFormatted(domain);
				defaultInterpolatedStringHandler.AppendLiteral(") Error on parsing config: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this._patches = new ConfigPatches(api, this);
				this._settings = new Dictionary<string, ConfigSetting>();
				this._configBlocks = new SortedDictionary<float, IConfigBlock>();
				this._defaultYaml = "";
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004808 File Offset: 0x00002A08
		public Config(ICoreAPI api, string domain, string modName, JsonObject json, string file, Dictionary<string, ConfigSetting> serverSideSettings)
		{
			this._api = api;
			this._domain = domain;
			this._modName = modName;
			this._json = json;
			this.RelativeFilePath = file;
			this.ConfigFilePath = Path.Combine(this._api.DataBasePath, "ModConfig", file);
			this.JsonFilePath = this.ConfigFilePath;
			this._configType = Config.ConfigType.JSON;
			this.JsonFilePath = file;
			try
			{
				this.ParseJson(json, out this._settings, out this._configBlocks, out this._defaultJson, domain);
				this.DistributeSettingsBySides(serverSideSettings);
				this._patches = new ConfigPatches(api, this);
				this.CreateFileWatcher();
				this.SubscribeToSettingsChanges();
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] (");
				defaultInterpolatedStringHandler.AppendFormatted(domain);
				defaultInterpolatedStringHandler.AppendLiteral(") Error on parsing config: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this._patches = new ConfigPatches(api, this);
				this._settings = new Dictionary<string, ConfigSetting>();
				this._configBlocks = new SortedDictionary<float, IConfigBlock>();
				this._defaultYaml = "";
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004994 File Offset: 0x00002B94
		public Config(ICoreAPI api, string domain, string modName, object configObject, string file)
		{
			this._api = api;
			this._domain = domain;
			this._modName = modName;
			this._json = this.DefinitionFromObject(configObject, domain);
			this.RelativeFilePath = file;
			this.ConfigFilePath = Path.Combine(this._api.DataBasePath, "ModConfig", file);
			this.JsonFilePath = this.ConfigFilePath;
			this._configType = Config.ConfigType.JSON;
			try
			{
				this.ParseJson(this._json, out this._settings, out this._configBlocks, out this._defaultJson, domain);
				this._clientSideSettings = this._settings;
				this._patches = new ConfigPatches(api, this);
				this.WriteToFile();
				this.CreateFileWatcher();
				this.SubscribeToSettingsChanges();
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] (");
				defaultInterpolatedStringHandler.AppendFormatted(domain);
				defaultInterpolatedStringHandler.AppendLiteral(") Error on parsing config: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this._patches = new ConfigPatches(api, this);
				this._settings = new Dictionary<string, ConfigSetting>();
				this._configBlocks = new SortedDictionary<float, IConfigBlock>();
				this._defaultYaml = "";
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004B2C File Offset: 0x00002D2C
		internal void Apply()
		{
			this._patches.Apply();
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004B39 File Offset: 0x00002D39
		internal JsonObject Definition
		{
			get
			{
				return this._json;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00004B41 File Offset: 0x00002D41
		internal SortedDictionary<float, IConfigBlock> ConfigBlocks
		{
			get
			{
				return this._configBlocks;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004B49 File Offset: 0x00002D49
		internal Dictionary<string, ConfigSetting> Settings
		{
			get
			{
				return this._settings;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004B51 File Offset: 0x00002D51
		internal Config.ConfigType FileType
		{
			get
			{
				return this._configType;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004B59 File Offset: 0x00002D59
		internal string JsonFilePath { get; } = "";

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004B61 File Offset: 0x00002D61
		internal string RelativeFilePath { get; } = "";

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004B69 File Offset: 0x00002D69
		internal string Domain
		{
			get
			{
				return this._domain;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004B71 File Offset: 0x00002D71
		internal string ModName
		{
			get
			{
				return this._modName;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004B79 File Offset: 0x00002D79
		[return: Nullable(2)]
		public ISetting GetSetting(string code)
		{
			if (!this._settings.ContainsKey(code))
			{
				return null;
			}
			return this._settings[code];
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004B98 File Offset: 0x00002D98
		public void WriteToFile()
		{
			try
			{
				string content = "";
				Config.ConfigType configType = this._configType;
				if (configType != Config.ConfigType.YAML)
				{
					if (configType == Config.ConfigType.JSON)
					{
						ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
						if (coreClientAPI != null && !coreClientAPI.IsSinglePlayer)
						{
							content = this.ToJson(this._settings.Values, this.ReadConfigFile(this._defaultJson, false), true);
						}
						else
						{
							content = this.ToJson(this._settings.Values, this.ReadConfigFile(this._defaultJson, false), false);
						}
					}
				}
				else
				{
					ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
					if (coreClientAPI != null && !coreClientAPI.IsSinglePlayer)
					{
						content = this.ToYaml(this._clientSideSettings.Values);
					}
					else
					{
						content = this.ToYaml(this._settings.Values);
					}
				}
				this.WriteConfigFile(content);
				Action<Config> configSaved = this.ConfigSaved;
				if (configSaved != null)
				{
					configSaved.Invoke(this);
				}
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(90, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception when trying to deserialize yaml and write it to file for '");
				defaultInterpolatedStringHandler.AppendFormatted(this._domain);
				defaultInterpolatedStringHandler.AppendLiteral("' config.\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004CE0 File Offset: 0x00002EE0
		public bool ReadFromFile()
		{
			return this.ReadFromFile(true);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004CEC File Offset: 0x00002EEC
		public bool ReadFromFile(bool overrideOnFail)
		{
			bool result;
			try
			{
				string content = this.ReadConfigFile(this._defaultYaml, overrideOnFail);
				Config.ConfigType configType = this._configType;
				if (configType != Config.ConfigType.YAML)
				{
					if (configType != Config.ConfigType.JSON)
					{
						result = false;
					}
					else
					{
						ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
						if (coreClientAPI != null && !coreClientAPI.IsSinglePlayer)
						{
							result = this.FromJson(this._settings.Values, content, true);
						}
						else
						{
							result = this.FromJson(this._settings.Values, content, false);
						}
					}
				}
				else
				{
					ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
					if (coreClientAPI != null && !coreClientAPI.IsSinglePlayer)
					{
						result = this.FromYaml(this._clientSideSettings.Values, content);
					}
					else
					{
						result = this.FromYaml(this._settings.Values, content);
					}
				}
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(76, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception when trying read YAML file and parse it for'");
				defaultInterpolatedStringHandler.AppendFormatted(this._domain);
				defaultInterpolatedStringHandler.AppendLiteral("' config.\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				result = false;
			}
			return result;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004E1C File Offset: 0x0000301C
		public bool TryReadFromFile()
		{
			bool result;
			try
			{
				string content;
				if (!this.ReadConfigFile(out content))
				{
					result = false;
				}
				else
				{
					Config.ConfigType configType = this._configType;
					if (configType != Config.ConfigType.YAML)
					{
						if (configType != Config.ConfigType.JSON)
						{
							result = false;
						}
						else
						{
							ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
							if (coreClientAPI != null && !coreClientAPI.IsSinglePlayer)
							{
								result = this.FromJson(this._settings.Values, content, true);
							}
							else
							{
								result = this.FromJson(this._settings.Values, content, false);
							}
						}
					}
					else
					{
						ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
						if (coreClientAPI != null && !coreClientAPI.IsSinglePlayer)
						{
							result = this.FromYaml(this._clientSideSettings.Values, content);
						}
						else
						{
							result = this.FromYaml(this._settings.Values, content);
						}
					}
				}
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(74, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Exception when trying read '");
				defaultInterpolatedStringHandler.AppendFormatted<Config.ConfigType>(this._configType);
				defaultInterpolatedStringHandler.AppendLiteral("' file and parse it for'");
				defaultInterpolatedStringHandler.AppendFormatted(this._domain);
				defaultInterpolatedStringHandler.AppendLiteral("' config.\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				result = false;
			}
			return result;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004F6C File Offset: 0x0000316C
		public void RestoreToDefaults()
		{
			try
			{
				Config.ConfigType configType = this._configType;
				if (configType != Config.ConfigType.YAML)
				{
					if (configType == Config.ConfigType.JSON)
					{
						ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
						if (coreClientAPI != null && !coreClientAPI.IsSinglePlayer)
						{
							this.FromJson(this._settings.Values, this._defaultJson, true);
						}
						else
						{
							this.FromJson(this._settings.Values, this._defaultJson, false);
						}
					}
				}
				else
				{
					ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
					if (coreClientAPI != null && !coreClientAPI.IsSinglePlayer)
					{
						this.FromYaml(this._clientSideSettings.Values, this._defaultYaml);
					}
					else
					{
						this.FromYaml(this._settings.Values, this._defaultYaml);
					}
				}
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(81, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception when trying to restore settings to defaults for '");
				defaultInterpolatedStringHandler.AppendFormatted(this._domain);
				defaultInterpolatedStringHandler.AppendLiteral("' config.\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005094 File Offset: 0x00003294
		public void AssignSettingsValues(object target)
		{
			Type type = target.GetType();
			IEnumerable<ValueTuple<string, FieldInfo>> fields = Enumerable.Select<FieldInfo, ValueTuple<string, FieldInfo>>(type.GetFields(52), (FieldInfo field) => new ValueTuple<string, FieldInfo>(ConfigSetting.NormalizeName(field.Name), field));
			IEnumerable<ValueTuple<string, PropertyInfo>> properties = Enumerable.Select<PropertyInfo, ValueTuple<string, PropertyInfo>>(Enumerable.Where<PropertyInfo>(type.GetProperties(52), (PropertyInfo property) => property.CanWrite), (PropertyInfo property) => new ValueTuple<string, PropertyInfo>(ConfigSetting.NormalizeName(property.Name), property));
			foreach (KeyValuePair<string, ConfigSetting> keyValuePair in this._settings)
			{
				string text;
				ConfigSetting configSetting;
				keyValuePair.Deconstruct(ref text, ref configSetting);
				ConfigSetting setting = configSetting;
				try
				{
					setting.AssignSettingValue(target, fields, properties);
				}
				catch (Exception exception)
				{
					ICoreAPI api = this._api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Exception on assigning value for setting '");
					defaultInterpolatedStringHandler.AppendFormatted(setting.YamlCode);
					defaultInterpolatedStringHandler.AppendLiteral("' for config '");
					defaultInterpolatedStringHandler.AppendFormatted(this._domain);
					defaultInterpolatedStringHandler.AppendLiteral("'.\nException: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000051F8 File Offset: 0x000033F8
		public void SyncFromServer(Config config, bool isSinglePlayer)
		{
			string text;
			ConfigSetting configSetting;
			if (isSinglePlayer)
			{
				using (Dictionary<string, ConfigSetting>.Enumerator enumerator = this._settings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, ConfigSetting> keyValuePair = enumerator.Current;
						keyValuePair.Deconstruct(ref text, ref configSetting);
						string code = text;
						if (config.Settings.ContainsKey(code) && !config.Settings[code].ClientSide)
						{
							this._settings[code].SetValueFrom(config.Settings[code]);
						}
					}
					return;
				}
			}
			this._clientSideSettings.Clear();
			foreach (KeyValuePair<string, ConfigSetting> keyValuePair in this._settings)
			{
				KeyValuePair<string, ConfigSetting> keyValuePair;
				keyValuePair.Deconstruct(ref text, ref configSetting);
				string code2 = text;
				ConfigSetting setting = configSetting;
				if (config.Settings.ContainsKey(code2) && !config.Settings[code2].ClientSide)
				{
					this._clientSideSettings.Add(code2, setting.Clone());
					this._settings[code2].SetValueFrom(config.Settings[code2]);
				}
				else
				{
					this._clientSideSettings.Add(code2, setting);
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000536C File Offset: 0x0000356C
		private void SubscribeToSettingsChanges()
		{
			foreach (KeyValuePair<string, ConfigSetting> keyValuePair in this._settings)
			{
				string text;
				ConfigSetting configSetting;
				keyValuePair.Deconstruct(ref text, ref configSetting);
				configSetting.SettingChanged += delegate(ConfigSetting setting)
				{
					Action<ISetting> settingChanged = this.SettingChanged;
					if (settingChanged == null)
					{
						return;
					}
					settingChanged.Invoke(setting);
				};
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000053D8 File Offset: 0x000035D8
		private void DistributeSettingsBySides(Dictionary<string, ConfigSetting> serverSideSettings)
		{
			foreach (KeyValuePair<string, ConfigSetting> keyValuePair in this._settings)
			{
				string text;
				ConfigSetting configSetting;
				keyValuePair.Deconstruct(ref text, ref configSetting);
				string code = text;
				ConfigSetting setting = configSetting;
				if (serverSideSettings.ContainsKey(code) && !serverSideSettings[code].ClientSide)
				{
					this._clientSideSettings.Add(code, setting.Clone());
					this._settings[code].SetValueFrom(serverSideSettings[code]);
				}
				else
				{
					this._clientSideSettings.Add(code, setting);
				}
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000548C File Offset: 0x0000368C
		private void Parse(JsonObject json, out Dictionary<string, ConfigSetting> settings, out SortedDictionary<float, IConfigBlock> configBlocks, out string defaultConfig, string domain, bool checkVersion = true)
		{
			this.Version = this.FromJsonDefinition(json, out settings, out configBlocks, domain);
			defaultConfig = this.ToYaml(settings.Values);
			string yamlConfig = this.ReadConfigFile(defaultConfig, true);
			bool valid = this.FromYaml(settings.Values, yamlConfig);
			if (checkVersion && !valid)
			{
				this.WriteConfigFile(defaultConfig);
				this.FromYaml(settings.Values, defaultConfig);
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000054F8 File Offset: 0x000036F8
		private void ParseJson(JsonObject json, out Dictionary<string, ConfigSetting> settings, out SortedDictionary<float, IConfigBlock> configBlocks, out string defaultConfig, string domain)
		{
			this.Version = this.FromJsonDefinition(json, out settings, out configBlocks, domain);
			string jsonConfig = this.ReadConfigFile("{}", false);
			JsonObject jsonConfigObject;
			try
			{
				jsonConfigObject = new JsonObject(JObject.Parse(jsonConfig));
			}
			catch (Exception exception)
			{
				ICoreAPI api = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(57, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[ParseJson] Error on parsing config file:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral("\nFile content:\n");
				defaultInterpolatedStringHandler.AppendFormatted(jsonConfig);
				LoggerUtil.Verbose(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				throw;
			}
			JsonObject jsonCopy = jsonConfigObject.Clone();
			foreach (ConfigSetting setting in settings.Values)
			{
				JsonObjectPath jsonObjectPath = new JsonObjectPath(setting.YamlCode);
				IEnumerable<JsonObject> value = jsonObjectPath.Get(jsonConfigObject);
				jsonObjectPath.Set(jsonCopy, setting.Value);
				setting.Value = (Enumerable.FirstOrDefault<JsonObject>(value) ?? setting.DefaultValue);
			}
			defaultConfig = jsonCopy.Token.ToString(1, Array.Empty<JsonConverter>());
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005620 File Offset: 0x00003820
		private JsonObject DefinitionFromObject(object configObject, string domain)
		{
			Type type = configObject.GetType();
			MemberInfo[] members = type.GetMembers(20);
			MemberInfo[] members2 = type.GetMembers(24);
			JObject root = new JObject();
			JArray settings = new JArray();
			root.Add("settings", settings);
			foreach (MemberInfo member in members2)
			{
				if (!(member.Name.ToLowerInvariant() != "version"))
				{
					PropertyInfo propertyInfo = member as PropertyInfo;
					object obj;
					if ((obj = ((propertyInfo != null) ? propertyInfo.GetValue(configObject) : null)) == null)
					{
						FieldInfo fieldInfo = member as FieldInfo;
						obj = ((fieldInfo != null) ? fieldInfo.GetValue(configObject) : null);
					}
					int? value = (int?)obj;
					if (value != null)
					{
						root.Add("version", value.Value);
					}
				}
			}
			MemberInfo[] array = members;
			for (int i = 0; i < array.Length; i++)
			{
				JObject definition;
				if (Config.SettingDefinitionFromObject(array[i], configObject, domain, out definition))
				{
					settings.Add(definition);
				}
			}
			return new JsonObject(root);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005710 File Offset: 0x00003910
		private static bool SettingDefinitionFromObject(MemberInfo info, object configObject, string domain, out JObject definition)
		{
			definition = new JObject();
			ConfigSettingType settingType = Config.GetSettingType(info);
			if (settingType == ConfigSettingType.None)
			{
				return false;
			}
			string code = info.Name;
			definition.Add("code", code);
			definition.Add("ingui", domain + ":setting-" + code);
			definition.Add("type", settingType.ToString().ToLowerInvariant());
			definition.Add("default", Config.GetDefaultValue(info, configObject, settingType));
			DescriptionAttribute description = CustomAttributeExtensions.GetCustomAttribute<DescriptionAttribute>(info);
			if (description != null)
			{
				definition.Add("comment", description.Description);
			}
			CategoryAttribute category = CustomAttributeExtensions.GetCustomAttribute<CategoryAttribute>(info);
			if (category != null)
			{
				foreach (string tag2 in Enumerable.Select<string, string>(category.Category.Replace(" ", "").Split(',', 0), (string tag) => tag.Replace("_", "").ToLowerInvariant()))
				{
					if (!(tag2 == "clientside"))
					{
						if (tag2 == "logarithmic")
						{
							definition.Add("logarithmic", true);
						}
					}
					else
					{
						definition.Add("clientSide", true);
					}
				}
			}
			switch (settingType)
			{
			case ConfigSettingType.Float:
				Config.SetFloatSettingDefinition(info, definition);
				break;
			case ConfigSettingType.Integer:
				Config.SetIntegerSettingDefinition(info, definition);
				break;
			case ConfigSettingType.String:
				Config.SetStringSettingDefinition(info, definition);
				break;
			}
			return true;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000058BC File Offset: 0x00003ABC
		private static ConfigSettingType GetSettingType(MemberInfo info)
		{
			PropertyInfo propertyInfo = info as PropertyInfo;
			Type type;
			if ((type = ((propertyInfo != null) ? propertyInfo.PropertyType : null)) == null)
			{
				FieldInfo fieldInfo = info as FieldInfo;
				type = ((fieldInfo != null) ? fieldInfo.FieldType : null);
			}
			Type valueType = type;
			if (valueType == null)
			{
				return ConfigSettingType.None;
			}
			if (valueType.IsEnum)
			{
				return ConfigSettingType.Integer;
			}
			if (valueType == typeof(float) || valueType == typeof(double))
			{
				return ConfigSettingType.Float;
			}
			if (valueType == typeof(string))
			{
				return ConfigSettingType.String;
			}
			if (valueType == typeof(bool))
			{
				return ConfigSettingType.Boolean;
			}
			if (valueType == typeof(int) || valueType == typeof(long) || valueType == typeof(short) || valueType == typeof(uint) || valueType == typeof(ulong) || valueType == typeof(ushort))
			{
				return ConfigSettingType.Integer;
			}
			return ConfigSettingType.None;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000059C4 File Offset: 0x00003BC4
		private static JValue GetDefaultValue(MemberInfo info, object configObject, ConfigSettingType settingType)
		{
			DefaultValueAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<DefaultValueAttribute>(info);
			object obj;
			if ((obj = ((customAttribute != null) ? customAttribute.Value : null)) == null)
			{
				PropertyInfo propertyInfo = info as PropertyInfo;
				if ((obj = ((propertyInfo != null) ? propertyInfo.GetValue(configObject) : null)) == null)
				{
					FieldInfo fieldInfo = info as FieldInfo;
					obj = ((fieldInfo != null) ? fieldInfo.GetValue(configObject) : null);
				}
			}
			object value = obj;
			if (value == null)
			{
				return new JValue(value);
			}
			JValue result;
			switch (settingType)
			{
			case ConfigSettingType.Boolean:
				result = new JValue((bool)value);
				break;
			case ConfigSettingType.Float:
				result = new JValue((float)value);
				break;
			case ConfigSettingType.Integer:
				result = new JValue((long)((int)value));
				break;
			case ConfigSettingType.String:
				result = new JValue((string)value);
				break;
			default:
				result = new JValue(value);
				break;
			}
			return result;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005A78 File Offset: 0x00003C78
		private static void SetFloatSettingDefinition(MemberInfo info, JObject definition)
		{
			RangeAttribute rangeAttribute = CustomAttributeExtensions.GetCustomAttribute<RangeAttribute>(info);
			if (rangeAttribute != null)
			{
				JObject jobject = new JObject();
				jobject.Add("min", Convert.ToSingle(rangeAttribute.Minimum));
				jobject.Add("max", Convert.ToSingle(rangeAttribute.Maximum));
				JObject range = jobject;
				definition.Add("range", range);
			}
			AllowedValuesAttribute allowedValuesAttribute = CustomAttributeExtensions.GetCustomAttribute<AllowedValuesAttribute>(info);
			if (allowedValuesAttribute != null)
			{
				IEnumerable<object> values2 = allowedValuesAttribute.Values;
				Func<object, float> func;
				if ((func = Config.<>O.<0>__ToSingle) == null)
				{
					func = (Config.<>O.<0>__ToSingle = new Func<object, float>(Convert.ToSingle));
				}
				JArray values = new JArray(Enumerable.Select<object, float>(values2, func));
				definition.Add("values", values);
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005B1C File Offset: 0x00003D1C
		private static void SetIntegerSettingDefinition(MemberInfo info, JObject definition)
		{
			RangeAttribute rangeAttribute = CustomAttributeExtensions.GetCustomAttribute<RangeAttribute>(info);
			if (rangeAttribute != null)
			{
				JObject jobject = new JObject();
				jobject.Add("min", Convert.ToInt32(rangeAttribute.Minimum));
				jobject.Add("max", Convert.ToInt32(rangeAttribute.Maximum));
				JObject range = jobject;
				definition.Add("range", range);
			}
			PropertyInfo propertyInfo = info as PropertyInfo;
			Type type;
			if ((type = ((propertyInfo != null) ? propertyInfo.PropertyType : null)) == null)
			{
				FieldInfo fieldInfo = info as FieldInfo;
				type = ((fieldInfo != null) ? fieldInfo.FieldType : null);
			}
			Type valueType = type;
			if (valueType != null && valueType.IsEnum)
			{
				string[] enumNames = valueType.GetEnumNames();
				int[] enumValues = (int[])valueType.GetEnumValues();
				if (enumNames.Length == enumValues.Length)
				{
					JObject mapping = new JObject();
					for (int index = 0; index < enumNames.Length; index++)
					{
						mapping.Add(enumNames[index], enumValues[index]);
					}
					definition.Add("mapping", mapping);
					int[] array = enumValues;
					JToken jtoken = definition["default"];
					int indexClamped = Math.Max(ArrayExtensions.IndexOf<int>(array, (jtoken != null) ? Extensions.Value<int>(jtoken) : 0), 0);
					definition.Remove("default");
					definition.Add("default", enumNames[indexClamped]);
				}
			}
			AllowedValuesAttribute allowedValuesAttribute = CustomAttributeExtensions.GetCustomAttribute<AllowedValuesAttribute>(info);
			if (allowedValuesAttribute != null)
			{
				IEnumerable<object> values2 = allowedValuesAttribute.Values;
				Func<object, int> func;
				if ((func = Config.<>O.<1>__ToInt32) == null)
				{
					func = (Config.<>O.<1>__ToInt32 = new Func<object, int>(Convert.ToInt32));
				}
				JArray values = new JArray(Enumerable.Select<object, int>(values2, func));
				definition.Add("values", values);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005CA0 File Offset: 0x00003EA0
		private static void SetStringSettingDefinition(MemberInfo info, JObject definition)
		{
			AllowedValuesAttribute allowedValuesAttribute = CustomAttributeExtensions.GetCustomAttribute<AllowedValuesAttribute>(info);
			if (allowedValuesAttribute != null)
			{
				IEnumerable<object> values2 = allowedValuesAttribute.Values;
				Func<object, string> func;
				if ((func = Config.<>O.<2>__ToString) == null)
				{
					func = (Config.<>O.<2>__ToString = new Func<object, string>(Convert.ToString));
				}
				JArray values = new JArray(Enumerable.Select<object, string>(values2, func));
				definition.Add("values", values);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005CF0 File Offset: 0x00003EF0
		private bool ReadConfigFile(out string config)
		{
			config = "";
			bool result;
			try
			{
				if (Path.Exists(this.ConfigFilePath))
				{
					try
					{
						using (StreamReader outputFile = new StreamReader(this.ConfigFilePath))
						{
							if (this._configFileWatcher != null)
							{
								this._configFileWatcher.EnableRaisingEvents = true;
							}
							config = outputFile.ReadToEnd();
							return true;
						}
					}
					catch
					{
						return false;
					}
				}
				result = false;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005D80 File Offset: 0x00003F80
		private string ReadConfigFile(string defaultConfig, bool overrideOnFail)
		{
			try
			{
				if (Path.Exists(this.ConfigFilePath))
				{
					try
					{
						using (StreamReader outputFile = new StreamReader(this.ConfigFilePath))
						{
							if (this._configFileWatcher != null)
							{
								this._configFileWatcher.EnableRaisingEvents = true;
							}
							return outputFile.ReadToEnd();
						}
					}
					catch
					{
						if (overrideOnFail)
						{
							this._api.Logger.Notification("[Config lib] [config domain: " + this._domain + "] Was not able to read settings, will create default settings file: " + this.ConfigFilePath);
							using (StreamWriter outputFile2 = new StreamWriter(this.ConfigFilePath))
							{
								outputFile2.Write(defaultConfig);
							}
						}
						goto IL_E0;
					}
				}
				this._api.Logger.Notification("[Config lib] [config domain: " + this._domain + "] Creating default settings file: " + this.ConfigFilePath);
				using (StreamWriter outputFile3 = new StreamWriter(this.ConfigFilePath))
				{
					outputFile3.Write(defaultConfig);
				}
				IL_E0:;
			}
			catch
			{
				this._api.Logger.Debug("[Config lib] [config domain: " + this._domain + "] Was not able to read/write settings file: " + this.ConfigFilePath);
			}
			return defaultConfig;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005EE0 File Offset: 0x000040E0
		private void WriteConfigFile(string content)
		{
			try
			{
				using (StreamWriter outputFile = new StreamWriter(this.ConfigFilePath))
				{
					outputFile.Write(content);
				}
			}
			catch (Exception exception)
			{
				ILogger logger = this._api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(107, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] [config domain: ");
				defaultInterpolatedStringHandler.AppendFormatted(this._domain);
				defaultInterpolatedStringHandler.AppendLiteral("] Exception when trying to deserialize yaml and write it to file.\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005F88 File Offset: 0x00004188
		private unsafe void CreateFileWatcher()
		{
			string directory = Path.GetDirectoryName(this.ConfigFilePath);
			if (directory == null)
			{
				LoggerUtil.Warn(this._api, this, "[config domain: " + this._domain + "] Unable to extract directory from: " + this.ConfigFilePath);
				return;
			}
			if (!Config._fileWatchers.TryGetValue(directory, ref this._configFileWatcher))
			{
				try
				{
					this._configFileWatcher = new FileSystemWatcher(directory);
					this._configFileWatcher.Changed += new FileSystemEventHandler(this.FileEventHandler);
					this._configFileWatcher.Created += new FileSystemEventHandler(this.FileEventHandler);
					this._configFileWatcher.NotifyFilter = 80;
					this._configFileWatcher.Error += delegate(object _, ErrorEventArgs e)
					{
					};
					this._configFileWatcher.EnableRaisingEvents = true;
					Config._fileWatchers.Add(directory, this._configFileWatcher);
				}
				catch (Exception exception)
				{
					string combined = Path.Combine(this._api.DataBasePath, "ModConfig", this._domain + ".yaml");
					LoggerUtil.Error(this._api, this, "Failed to create file watcher. Automatic updates when files are changed on disc will not work.");
					ICoreAPI api = this._api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(152, 4);
					defaultInterpolatedStringHandler.AppendLiteral("[config domain: ");
					defaultInterpolatedStringHandler.AppendFormatted(this._domain);
					defaultInterpolatedStringHandler.AppendLiteral("] Failed to create file watcher. Automatic updates when file is changed on disc will not work.\nPaths:\n  data: ");
					defaultInterpolatedStringHandler.AppendFormatted(this._api.DataBasePath);
					defaultInterpolatedStringHandler.AppendLiteral("\n  combined: ");
					defaultInterpolatedStringHandler.AppendFormatted(combined);
					defaultInterpolatedStringHandler.AppendLiteral("\nException:\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					defaultInterpolatedStringHandler.AppendLiteral("\n");
					LoggerUtil.Verbose(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
					Config._fileWatchers.Add(directory, null);
					return;
				}
			}
			int initialDelay = Math.Abs(Path.GetFileName(this.ConfigFilePath).GetHashCode()) % 2000;
			this._fileChangedListener = this._api.World.RegisterGameTickListener(delegate(float _)
			{
				this.OnFileChanged();
			}, 2000, initialDelay);
			List<Config> configs;
			if (Config._configsByPath.TryGetValue(this.ConfigFilePath, ref configs))
			{
				configs.Add(this);
				return;
			}
			Dictionary<string, List<Config>> configsByPath = Config._configsByPath;
			string configFilePath = this.ConfigFilePath;
			int num = 1;
			List<Config> list = new List<Config>(num);
			CollectionsMarshal.SetCount<Config>(list, num);
			Span<Config> span = CollectionsMarshal.AsSpan<Config>(list);
			int num2 = 0;
			*span[num2] = this;
			configsByPath.TryAdd(configFilePath, list);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000061F4 File Offset: 0x000043F4
		private bool CheckIfFileChanged()
		{
			object fileChangedLockObject = this._fileChangedLockObject;
			bool result;
			lock (fileChangedLockObject)
			{
				if (!this._fileChanged)
				{
					result = false;
				}
				else
				{
					this._fileChanged = false;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00006244 File Offset: 0x00004444
		private void FileEventHandler(object sender, FileSystemEventArgs eventArgs)
		{
			if (eventArgs.ChangeType != 4 && eventArgs.ChangeType != 1)
			{
				return;
			}
			this._api.Event.EnqueueMainThreadTask(delegate()
			{
				List<Config> configs;
				if (Config._configsByPath.TryGetValue(eventArgs.FullPath, ref configs))
				{
					foreach (Config config in configs)
					{
						object fileChangedLockObject = config._fileChangedLockObject;
						lock (fileChangedLockObject)
						{
							config._fileChanged = true;
						}
					}
				}
			}, "configlib");
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000629C File Offset: 0x0000449C
		private void OnFileChanged()
		{
			if (this.CheckIfFileChanged())
			{
				this.TryReadFromFile();
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000062B0 File Offset: 0x000044B0
		private bool FromYaml(IEnumerable<ConfigSetting> settings, string yaml)
		{
			Dictionary<string, JsonObject> values;
			this.ValuesFromYaml(out values, yaml);
			if (this.Version != -1 && (!values.ContainsKey("version") || this.ConvertVersion(values["version"].Token) != this.Version))
			{
				return false;
			}
			foreach (ConfigSetting setting in settings)
			{
				if (values.ContainsKey(setting.YamlCode))
				{
					Validation validation = setting.Validation;
					if (((validation != null) ? validation.Mapping : null) == null)
					{
						JToken converted = this.ConvertValue(values[setting.YamlCode].Token, setting.SettingType);
						setting.Value = new JsonObject(converted);
					}
					else
					{
						string key = values[setting.YamlCode].AsString("");
						Validation validation2 = setting.Validation;
						bool flag;
						if (validation2 == null)
						{
							flag = false;
						}
						else
						{
							Dictionary<string, JsonObject> mapping = validation2.Mapping;
							flag = ((mapping != null) ? new bool?(mapping.ContainsKey(key)) : default(bool?)).GetValueOrDefault();
						}
						if (flag)
						{
							setting.Value = setting.Validation.Mapping[key];
							setting.MappingKey = key;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000063F8 File Offset: 0x000045F8
		private string ToYaml(IEnumerable<ConfigSetting> settings)
		{
			return this.ConstructYaml(settings, this._configBlocks, this.Version);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00006410 File Offset: 0x00004610
		private int FromJsonDefinition(JsonObject json, out Dictionary<string, ConfigSetting> settings, out SortedDictionary<float, IConfigBlock> configBlocks, string domain)
		{
			settings = new Dictionary<string, ConfigSetting>();
			bool flag = json["settings"].IsArray();
			JsonObject jsonObject = json["version"];
			int version = (jsonObject != null) ? jsonObject.AsInt(0) : 0;
			if (flag)
			{
				this.SettingsAndFormattingFromJsonArray(settings, json["settings"].AsArray(), out configBlocks, domain);
			}
			else
			{
				this.SettingsFromJson(settings, json, ref version, domain);
				SortedDictionary<float, IConfigBlock> formatting;
				this.FormattingFromJson(json, out formatting, domain);
				configBlocks = this.CombineConfigBlocks(formatting, settings.Values);
			}
			if (json.KeyExists("constants"))
			{
				this.ParseConstants(settings, json["constants"]);
			}
			return version;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000064B8 File Offset: 0x000046B8
		private string ToJson(IEnumerable<ConfigSetting> settings, string defaultJson, bool onlyClientSide = false)
		{
			JsonObject config = new JsonObject(JObject.Parse(defaultJson));
			Func<ConfigSetting, bool> <>9__0;
			Func<ConfigSetting, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = ((ConfigSetting item) => !onlyClientSide || item.ClientSide));
			}
			foreach (ConfigSetting setting in Enumerable.Where<ConfigSetting>(settings, func))
			{
				JsonObjectPath jsonPath = new JsonObjectPath(setting.YamlCode);
				Validation validation = setting.Validation;
				if (((validation != null) ? validation.Mapping : null) == null)
				{
					if (jsonPath.Set(config, setting.Value) == 0 && !setting.YamlCode.Contains('\\'))
					{
						JObject jobject = config.Token as JObject;
						if (jobject != null)
						{
							jobject.Add(setting.YamlCode, setting.Value.Token);
						}
					}
				}
				else if (jsonPath.Set(config, new JsonObject(new JValue(setting.MappingKey))) == 0 && !setting.YamlCode.Contains('\\'))
				{
					JObject jobject2 = config.Token as JObject;
					if (jobject2 != null)
					{
						jobject2.Add(setting.YamlCode, setting.MappingKey);
					}
				}
			}
			return config.Token.ToString(1, Array.Empty<JsonConverter>());
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006614 File Offset: 0x00004814
		private bool FromJson(IEnumerable<ConfigSetting> settings, string json, bool onlyClientSide = false)
		{
			JsonObject jsonConfigObject = new JsonObject(JObject.Parse(json));
			Func<ConfigSetting, bool> <>9__0;
			Func<ConfigSetting, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = ((ConfigSetting item) => !onlyClientSide || item.ClientSide));
			}
			foreach (ConfigSetting setting in Enumerable.Where<ConfigSetting>(settings, func))
			{
				JsonObject value = Enumerable.FirstOrDefault<JsonObject>(new JsonObjectPath(setting.YamlCode).Get(jsonConfigObject), null) ?? setting.DefaultValue;
				Validation validation = setting.Validation;
				if (((validation != null) ? validation.Mapping : null) == null)
				{
					setting.Value = value;
				}
				else
				{
					string key = value.AsString("");
					Validation validation2 = setting.Validation;
					bool flag;
					if (validation2 == null)
					{
						flag = false;
					}
					else
					{
						Dictionary<string, JsonObject> mapping = validation2.Mapping;
						flag = ((mapping != null) ? new bool?(mapping.ContainsKey(key)) : default(bool?)).GetValueOrDefault();
					}
					if (flag)
					{
						setting.Value = setting.Validation.Mapping[key];
						setting.MappingKey = key;
					}
				}
			}
			return true;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000674C File Offset: 0x0000494C
		private void ParseConstants(Dictionary<string, ConfigSetting> settings, JsonObject constants)
		{
			foreach (JToken jtoken in constants.Token)
			{
				JProperty property = jtoken as JProperty;
				if (property != null)
				{
					string code = property.Name;
					ConfigSetting setting = new ConfigSetting(code, new JsonObject(property.Value), ConfigSettingType.Constant)
					{
						Hide = true
					};
					settings.Add(code, setting);
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000067C4 File Offset: 0x000049C4
		private SortedDictionary<float, IConfigBlock> CombineConfigBlocks(SortedDictionary<float, IConfigBlock> formatting, IEnumerable<ConfigSetting> settings)
		{
			float delta = 1E-10f;
			float increment = delta;
			SortedDictionary<float, IConfigBlock> configBlocks = new SortedDictionary<float, IConfigBlock>();
			foreach (KeyValuePair<float, IConfigBlock> keyValuePair in formatting)
			{
				float num;
				IConfigBlock configBlock;
				keyValuePair.Deconstruct(ref num, ref configBlock);
				float num2 = num;
				IConfigBlock block = configBlock;
				float weight = num2;
				if (configBlocks.ContainsKey(weight))
				{
					weight += increment;
					increment += delta;
				}
				configBlocks.Add(weight, block);
			}
			foreach (ConfigSetting setting in settings)
			{
				float weight2 = setting.SortingWeight;
				if (configBlocks.ContainsKey(weight2))
				{
					weight2 += increment;
					increment += delta;
				}
				configBlocks.Add(weight2, setting);
			}
			return configBlocks;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000068AC File Offset: 0x00004AAC
		private void FormattingFromJson(JsonObject json, out SortedDictionary<float, IConfigBlock> formatting, string domain)
		{
			formatting = new SortedDictionary<float, IConfigBlock>();
			float delta = 1E-08f;
			float increment = delta;
			if (!json.KeyExists("formatting") || !json["formatting"].IsArray())
			{
				return;
			}
			foreach (JsonObject block in json["formatting"].AsArray())
			{
				IFormattingBlock formattingBlock = this.ParseBlock(block, domain);
				float weight = formattingBlock.SortingWeight;
				if (formatting.ContainsKey(weight))
				{
					weight += increment;
					increment += delta;
				}
				formatting.Add(weight, formattingBlock);
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006940 File Offset: 0x00004B40
		private IFormattingBlock ParseBlock(JsonObject block, string domain)
		{
			JsonObject jsonObject = block["type"];
			if (((jsonObject != null) ? jsonObject.AsString(null) : null) == "separator")
			{
				return new Separator(block, domain, this._api);
			}
			return new Blank();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006979 File Offset: 0x00004B79
		private int ConvertVersion(JToken value)
		{
			return new JsonObject(this.ConvertValue(value, ConfigSettingType.Integer)).AsInt(0);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006990 File Offset: 0x00004B90
		private string ConstructYaml(IEnumerable<ConfigSetting> settings, SortedDictionary<float, IConfigBlock> formatting, int version)
		{
			SortedDictionary<float, string> yaml;
			this.SettingsToYaml(settings, out yaml);
			SortedDictionary<float, string> sortedDictionary = yaml;
			float num = -1f;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("version: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(version);
			sortedDictionary.Add(num, defaultInterpolatedStringHandler.ToStringAndClear());
			foreach (KeyValuePair<float, IConfigBlock> keyValuePair in Enumerable.Where<KeyValuePair<float, IConfigBlock>>(formatting, (KeyValuePair<float, IConfigBlock> entry) => entry.Value is IFormattingBlock))
			{
				float num2;
				IConfigBlock configBlock;
				keyValuePair.Deconstruct(ref num2, ref configBlock);
				float weight = num2;
				IConfigBlock block = configBlock;
				SortedDictionary<float, string> sortedDictionary2 = yaml;
				float num3 = weight;
				IFormattingBlock formattingBlock = block as IFormattingBlock;
				sortedDictionary2.Add(num3, ((formattingBlock != null) ? formattingBlock.Yaml : null) ?? "");
			}
			return Enumerable.Aggregate<string>(Enumerable.Select<KeyValuePair<float, string>, string>(yaml, (KeyValuePair<float, string> entry) => entry.Value), (string first, string second) => first + "\n" + second);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006AB0 File Offset: 0x00004CB0
		private void ValuesFromYaml(out Dictionary<string, JsonObject> values, string yaml)
		{
			JObject json;
			try
			{
				object yamlObject = new DeserializerBuilder().Build().Deserialize(yaml);
				json = JObject.Parse(new SerializerBuilder().JsonCompatible().Build().Serialize(yamlObject));
			}
			catch (Exception exception)
			{
				ICoreAPI api = this._api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(62, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[ValuesFromYaml] Error on parsing config file:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				defaultInterpolatedStringHandler.AppendLiteral("\nFile content:\n");
				defaultInterpolatedStringHandler.AppendFormatted(yaml);
				LoggerUtil.Verbose(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
				throw;
			}
			values = new Dictionary<string, JsonObject>();
			foreach (KeyValuePair<string, JToken> keyValuePair in json)
			{
				string text;
				JToken jtoken;
				keyValuePair.Deconstruct(ref text, ref jtoken);
				string code = text;
				JToken value = jtoken;
				if (value != null)
				{
					values.Add(code, new JsonObject(value));
				}
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006BA8 File Offset: 0x00004DA8
		private JToken ConvertValue([Nullable(2)] JToken value, ConfigSettingType type)
		{
			JValue jvalue = value as JValue;
			string strValue = (string)((jvalue != null) ? jvalue.Value : null);
			if (strValue == null)
			{
				return value ?? new JValue(strValue);
			}
			CultureInfo culture = new CultureInfo("en-US");
			switch (type)
			{
			case ConfigSettingType.Boolean:
				return new JValue(bool.Parse(strValue));
			case ConfigSettingType.Float:
				return new JValue(float.Parse(strValue, 167, culture));
			case ConfigSettingType.Integer:
				return new JValue((long)int.Parse(strValue, 7, culture));
			case ConfigSettingType.String:
				return new JValue(strValue);
			case ConfigSettingType.Color:
				return new JValue(strValue);
			default:
				return value ?? new JValue(strValue);
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006C4C File Offset: 0x00004E4C
		private void SettingsToYaml(IEnumerable<ConfigSetting> settings, out SortedDictionary<float, string> yaml)
		{
			float delta = 1E-10f;
			float increment = delta;
			yaml = new SortedDictionary<float, string>();
			foreach (ConfigSetting setting2 in Enumerable.Where<ConfigSetting>(settings, (ConfigSetting setting) => !setting.Hide))
			{
				float weight = (setting2.SortingWeight < 0f) ? 0f : setting2.SortingWeight;
				if (yaml.ContainsKey(weight))
				{
					weight += increment;
					increment += delta;
				}
				yaml.Add(weight, setting2.ToYaml());
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006D00 File Offset: 0x00004F00
		private void SettingsFromJson(Dictionary<string, ConfigSetting> settings, JsonObject definition, ref int version, string domain)
		{
			JsonObject jsonObject = definition["version"];
			version = ((jsonObject != null) ? jsonObject.AsInt(0) : 0);
			if (definition["settings"].KeyExists("boolean"))
			{
				this.ParseSettingsCategory(definition["settings"]["boolean"], settings, ConfigSettingType.Boolean, domain);
			}
			if (definition["settings"].KeyExists("integer"))
			{
				this.ParseSettingsCategory(definition["settings"]["integer"], settings, ConfigSettingType.Integer, domain);
			}
			if (definition["settings"].KeyExists("float"))
			{
				this.ParseSettingsCategory(definition["settings"]["float"], settings, ConfigSettingType.Float, domain);
			}
			if (definition["settings"].KeyExists("number"))
			{
				this.ParseSettingsCategory(definition["settings"]["number"], settings, ConfigSettingType.Float, domain);
			}
			if (definition["settings"].KeyExists("string"))
			{
				this.ParseSettingsCategory(definition["settings"]["string"], settings, ConfigSettingType.String, domain);
			}
			if (definition["settings"].KeyExists("other"))
			{
				this.ParseSettingsCategory(definition["settings"]["other"], settings, ConfigSettingType.Other, domain);
			}
			if (definition["settings"].KeyExists("color"))
			{
				this.ParseSettingsCategory(definition["settings"]["color"], settings, ConfigSettingType.Color, domain);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00006EA4 File Offset: 0x000050A4
		private void SettingsAndFormattingFromJsonArray(Dictionary<string, ConfigSetting> settings, JsonObject[] definition, out SortedDictionary<float, IConfigBlock> configBlocks, string domain)
		{
			configBlocks = new SortedDictionary<float, IConfigBlock>();
			float weight = 0f;
			foreach (JsonObject block in definition)
			{
				string type = block["type"].AsString(null);
				weight += 1f;
				if (!(type == "separator"))
				{
					if (type == null)
					{
						goto IL_176;
					}
					ConfigSettingType configSettingType;
					switch (type.Length)
					{
					case 5:
					{
						char c = type.get_Chars(0);
						if (c != 'c')
						{
							if (c != 'f')
							{
								if (c != 'o')
								{
									goto IL_176;
								}
								if (!(type == "other"))
								{
									goto IL_176;
								}
								configSettingType = ConfigSettingType.Other;
							}
							else
							{
								if (!(type == "float"))
								{
									goto IL_176;
								}
								configSettingType = ConfigSettingType.Float;
							}
						}
						else
						{
							if (!(type == "color"))
							{
								goto IL_176;
							}
							configSettingType = ConfigSettingType.Color;
						}
						break;
					}
					case 6:
					{
						char c = type.get_Chars(0);
						if (c != 'n')
						{
							if (c != 's')
							{
								goto IL_176;
							}
							if (!(type == "string"))
							{
								goto IL_176;
							}
							configSettingType = ConfigSettingType.String;
						}
						else
						{
							if (!(type == "number"))
							{
								goto IL_176;
							}
							configSettingType = ConfigSettingType.Float;
						}
						break;
					}
					case 7:
					{
						char c = type.get_Chars(0);
						if (c != 'b')
						{
							if (c != 'i')
							{
								goto IL_176;
							}
							if (!(type == "integer"))
							{
								goto IL_176;
							}
							configSettingType = ConfigSettingType.Integer;
						}
						else
						{
							if (!(type == "boolean"))
							{
								goto IL_176;
							}
							configSettingType = ConfigSettingType.Boolean;
						}
						break;
					}
					default:
						goto IL_176;
					}
					IL_179:
					ConfigSettingType settingType = configSettingType;
					ValueTuple<string, ConfigSetting> valueTuple = this.ParseSettingBlock(block, settingType, domain);
					string code = valueTuple.Item1;
					ConfigSetting setting = valueTuple.Item2;
					configBlocks.Add(weight, setting);
					settings.Add(code, setting);
					setting.SortingWeight = weight;
					goto IL_1B3;
					IL_176:
					configSettingType = ConfigSettingType.None;
					goto IL_179;
				}
				IConfigBlock formattingBlock = this.ParseFormattingBlock(type, block, domain);
				configBlocks.Add(weight, formattingBlock);
				IL_1B3:;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00007071 File Offset: 0x00005271
		private IConfigBlock ParseFormattingBlock(string type, JsonObject block, string domain)
		{
			return this.ParseBlock(block, domain);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000707C File Offset: 0x0000527C
		[return: TupleElementNames(new string[]
		{
			"code",
			"setting"
		})]
		[return: Nullable(new byte[]
		{
			0,
			1,
			1
		})]
		private ValueTuple<string, ConfigSetting> ParseSettingBlock(JsonObject block, ConfigSettingType settingType, string domain)
		{
			if (!block.KeyExists("code"))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] (");
				defaultInterpolatedStringHandler.AppendFormatted(domain);
				defaultInterpolatedStringHandler.AppendLiteral(") Setting has no code: ");
				defaultInterpolatedStringHandler.AppendFormatted<JsonObject>(block);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			string code = block["code"].AsString(null);
			ConfigSetting setting = ConfigSetting.FromJson(block, settingType, domain, code, this._api);
			return new ValueTuple<string, ConfigSetting>(code, setting);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00007100 File Offset: 0x00005300
		private void ParseSettingsCategory(JsonObject category, Dictionary<string, ConfigSetting> settings, ConfigSettingType settingType, string domain)
		{
			foreach (JToken jtoken in category.Token)
			{
				JProperty property = jtoken as JProperty;
				if (property != null)
				{
					string code = property.Name;
					ConfigSetting setting = ConfigSetting.FromJson(new JsonObject(property.Value), settingType, domain, code, this._api);
					settings.Add(code, setting);
				}
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00007178 File Offset: 0x00005378
		private void Dispose(bool disposing)
		{
			if (!this._disposedValue)
			{
				if (disposing)
				{
					FileSystemWatcher configFileWatcher = this._configFileWatcher;
					if (configFileWatcher != null)
					{
						configFileWatcher.Dispose();
					}
					this._configFileWatcher = null;
					Config._configsByPath.Clear();
					if (this._fileChangedListener != 0L)
					{
						this._api.World.UnregisterGameTickListener(this._fileChangedListener);
						this._fileChangedListener = 0L;
					}
				}
				this._disposedValue = true;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000071DF File Offset: 0x000053DF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000049 RID: 73
		private readonly ICoreAPI _api;

		// Token: 0x0400004A RID: 74
		private readonly string _domain;

		// Token: 0x0400004B RID: 75
		private readonly string _modName;

		// Token: 0x0400004C RID: 76
		private readonly Dictionary<string, ConfigSetting> _settings;

		// Token: 0x0400004D RID: 77
		private readonly Dictionary<string, ConfigSetting> _clientSideSettings = new Dictionary<string, ConfigSetting>();

		// Token: 0x0400004E RID: 78
		private readonly SortedDictionary<float, IConfigBlock> _configBlocks;

		// Token: 0x0400004F RID: 79
		private readonly JsonObject _json;

		// Token: 0x04000050 RID: 80
		private readonly ConfigPatches _patches;

		// Token: 0x04000051 RID: 81
		private readonly string _defaultYaml = "";

		// Token: 0x04000052 RID: 82
		private readonly string _defaultJson = "{}";

		// Token: 0x04000053 RID: 83
		private readonly Config.ConfigType _configType;

		// Token: 0x04000054 RID: 84
		[Nullable(2)]
		private FileSystemWatcher _configFileWatcher;

		// Token: 0x04000055 RID: 85
		private bool _disposedValue;

		// Token: 0x04000056 RID: 86
		private readonly object _fileChangedLockObject = new object();

		// Token: 0x04000057 RID: 87
		private bool _fileChanged;

		// Token: 0x04000058 RID: 88
		private long _fileChangedListener;

		// Token: 0x04000059 RID: 89
		private const int _fileChangeCheckIntervalMs = 2000;

		// Token: 0x0400005A RID: 90
		[Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		private static Dictionary<string, FileSystemWatcher> _fileWatchers = new Dictionary<string, FileSystemWatcher>();

		// Token: 0x0400005B RID: 91
		private static Dictionary<string, List<Config>> _configsByPath = new Dictionary<string, List<Config>>();

		// Token: 0x0200001B RID: 27
		[NullableContext(0)]
		internal enum ConfigType
		{
			// Token: 0x0400005D RID: 93
			YAML,
			// Token: 0x0400005E RID: 94
			JSON
		}

		// Token: 0x0200001C RID: 28
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400005F RID: 95
			[Nullable(0)]
			public static Func<object, float> <0>__ToSingle;

			// Token: 0x04000060 RID: 96
			[Nullable(0)]
			public static Func<object, int> <1>__ToInt32;

			// Token: 0x04000061 RID: 97
			[Nullable(0)]
			public static Func<object, string> <2>__ToString;
		}
	}
}
