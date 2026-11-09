using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ConfigLib.Patches;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.Common;

namespace ConfigLib
{
	// Token: 0x0200000F RID: 15
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ConfigLibModSystem : ModSystem, IConfigProvider
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000025FC File Offset: 0x000007FC
		public IEnumerable<string> Domains
		{
			get
			{
				return this._domains;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002604 File Offset: 0x00000804
		[return: Nullable(2)]
		public IConfig GetConfig(string domain)
		{
			return this.GetConfigImpl(domain);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000260D File Offset: 0x0000080D
		[return: Nullable(2)]
		public ISetting GetSetting(string domain, string code)
		{
			Config configImpl = this.GetConfigImpl(domain);
			if (configImpl == null)
			{
				return null;
			}
			return configImpl.GetSetting(code);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002622 File Offset: 0x00000822
		public void RegisterCustomConfig(string domain, Action<string, ControlButtons> drawDelegate)
		{
			this._customConfigs.TryAdd(domain, ConfigLibModSystem.DrawDelegateWrapper(drawDelegate));
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002637 File Offset: 0x00000837
		public void RegisterCustomConfig(string domain, Func<string, ControlButtons, ControlButtons> drawDelegate)
		{
			this._customConfigs.TryAdd(domain, drawDelegate);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002648 File Offset: 0x00000848
		[NullableContext(2)]
		public void RegisterCustomManagedConfig([Nullable(1)] string domain, [Nullable(1)] object configObject, string path = null, Action onSyncedFromServer = null, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> onSettingChanged = null, Action onConfigSaved = null)
		{
			ConfigLibModSystem.<>c__DisplayClass6_0 CS$<>8__locals1 = new ConfigLibModSystem.<>c__DisplayClass6_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.domain = domain;
			CS$<>8__locals1.configObject = configObject;
			CS$<>8__locals1.onSettingChanged = onSettingChanged;
			CS$<>8__locals1.onConfigSaved = onConfigSaved;
			CS$<>8__locals1.onSyncedFromServer = onSyncedFromServer;
			if (this._api == null)
			{
				return;
			}
			if (!this._canRegisterNewConfig)
			{
				LoggerUtil.Error(this._api, this, "Cant register custom managed config '" + CS$<>8__locals1.domain + "': too late, configs have been already sent to clients");
				return;
			}
			ConfigLibModSystem.<>c__DisplayClass6_0 CS$<>8__locals2 = CS$<>8__locals1;
			ICoreAPI api = this._api;
			string domain2 = CS$<>8__locals1.domain;
			Mod mod = this._api.ModLoader.GetMod(CS$<>8__locals1.domain);
			CS$<>8__locals2.config = new Config(api, domain2, ((mod != null) ? mod.Info.Name : null) ?? Lang.Get(CS$<>8__locals1.domain, Array.Empty<object>()), CS$<>8__locals1.configObject, path ?? (CS$<>8__locals1.domain + ".json"));
			this._configs.Add(CS$<>8__locals1.domain, CS$<>8__locals1.config);
			this._domains.Add(CS$<>8__locals1.domain);
			this._configsToRegister.Add(CS$<>8__locals1.domain, CS$<>8__locals1.config);
			if (this._api.Side == 1)
			{
				CS$<>8__locals1.config.ConfigSaved += new Action<Config>(this.OnConfigSaved);
				foreach (KeyValuePair<string, ConfigSetting> keyValuePair in CS$<>8__locals1.config.Settings)
				{
					string code2;
					ConfigSetting configSetting;
					keyValuePair.Deconstruct(ref code2, ref configSetting);
					string code = code2;
					ConfigSetting setting2 = configSetting;
					setting2.SettingChanged += delegate(ConfigSetting value)
					{
						CS$<>8__locals1.<>4__this.OnSettingChanged(CS$<>8__locals1.domain, code, value);
					};
					this.OnSettingLoaded(CS$<>8__locals1.domain, code, setting2);
				}
			}
			CS$<>8__locals1.config.SettingChanged += delegate(ISetting setting)
			{
				Action<string, IConfig, ISetting> settingChanged = CS$<>8__locals1.<>4__this.SettingChanged;
				if (settingChanged == null)
				{
					return;
				}
				settingChanged.Invoke(CS$<>8__locals1.domain, CS$<>8__locals1.config, setting);
			};
			CS$<>8__locals1.config.SettingChanged += delegate(ISetting setting)
			{
				setting.AssignSettingValue(CS$<>8__locals1.configObject);
				Action<string> onSettingChanged2 = CS$<>8__locals1.onSettingChanged;
				if (onSettingChanged2 == null)
				{
					return;
				}
				onSettingChanged2.Invoke(setting.YamlCode);
			};
			CS$<>8__locals1.config.ConfigSaved += delegate(Config config)
			{
				Action onConfigSaved2 = CS$<>8__locals1.onConfigSaved;
				if (onConfigSaved2 == null)
				{
					return;
				}
				onConfigSaved2.Invoke();
			};
			if (this._api.Side == 2)
			{
				this.ConfigsLoaded += delegate()
				{
					Action onSyncedFromServer2 = CS$<>8__locals1.onSyncedFromServer;
					if (onSyncedFromServer2 == null)
					{
						return;
					}
					onSyncedFromServer2.Invoke();
				};
			}
			this._customManagedConfigs.Add(CS$<>8__locals1.domain);
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600005F RID: 95 RVA: 0x0000289C File Offset: 0x00000A9C
		// (remove) Token: 0x06000060 RID: 96 RVA: 0x000028D4 File Offset: 0x00000AD4
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event Action ConfigWindowClosed;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000061 RID: 97 RVA: 0x0000290C File Offset: 0x00000B0C
		// (remove) Token: 0x06000062 RID: 98 RVA: 0x00002944 File Offset: 0x00000B44
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event Action ConfigWindowOpened;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000063 RID: 99 RVA: 0x0000297C File Offset: 0x00000B7C
		// (remove) Token: 0x06000064 RID: 100 RVA: 0x000029B4 File Offset: 0x00000BB4
		[Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})]
		public event Action<string, IConfig, ISetting> SettingChanged;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000065 RID: 101 RVA: 0x000029EC File Offset: 0x00000BEC
		// (remove) Token: 0x06000066 RID: 102 RVA: 0x00002A20 File Offset: 0x00000C20
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Obsolete]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public static event Action<ICoreAPI> ConfigsChanged;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000067 RID: 103 RVA: 0x00002A54 File Offset: 0x00000C54
		// (remove) Token: 0x06000068 RID: 104 RVA: 0x00002A8C File Offset: 0x00000C8C
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event Action ConfigsLoaded;

		// Token: 0x06000069 RID: 105 RVA: 0x000025FC File Offset: 0x000007FC
		internal HashSet<string> GetDomains()
		{
			return this._domains;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002AC1 File Offset: 0x00000CC1
		[return: Nullable(2)]
		internal Config GetConfigImpl(string domain)
		{
			Dictionary<string, Config> configs = this._configs;
			if (configs == null || !configs.ContainsKey(domain))
			{
				return null;
			}
			return this._configs[domain];
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002AE6 File Offset: 0x00000CE6
		[return: Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})]
		internal Dictionary<string, Func<string, ControlButtons, ControlButtons>> GetCustomConfigs()
		{
			return this._customConfigs;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002AEE File Offset: 0x00000CEE
		public override void StartPre(ICoreAPI api)
		{
			this._api = api;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public override void Start(ICoreAPI api)
		{
			this._registry = api.RegisterRecipeRegistry<ConfigRegistry>("configlib:configs");
			api.Event.RegisterEventBusListener(new EventBusListenerDelegate(this.ReloadJsonConfigs), 0.5, "configlib:config-reload");
			if (api.Side == 2)
			{
				PauseMenuPatch.Patch();
				ConfigRegistry.ConfigsLoaded += new Action<Dictionary<string, Config>>(this.ReloadConfigs);
				ICoreClientAPI coreClientAPI = api as ICoreClientAPI;
				this._eventsChannel = ((coreClientAPI != null) ? coreClientAPI.Network.RegisterChannel("configlib:events").RegisterMessageType<ConfigEventPacket>().RegisterMessageType<ServerSideSettingChanged>().SetMessageHandler<ServerSideSettingChanged>(new NetworkServerMessageHandler<ServerSideSettingChanged>(this.OnServerSettingChanged)) : null);
				return;
			}
			ICoreServerAPI coreServerAPI = api as ICoreServerAPI;
			this._eventsServerChannel = ((coreServerAPI != null) ? coreServerAPI.Network.RegisterChannel("configlib:events").RegisterMessageType<ConfigEventPacket>().SetMessageHandler<ConfigEventPacket>(new NetworkClientMessageHandler<ConfigEventPacket>(this.SendEvent)).RegisterMessageType<ServerSideSettingChanged>().SetMessageHandler<ServerSideSettingChanged>(new NetworkClientMessageHandler<ServerSideSettingChanged>(this.OnServerSettingChanged)) : null);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override void AssetsLoaded(ICoreAPI api)
		{
			this.LoadConfigs();
			ICoreAPI api2 = this._api;
			if (api2 != null)
			{
				ILogger logger = api2.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Configs loaded: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._configs.Count);
				logger.Notification(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			foreach (KeyValuePair<string, Config> keyValuePair in this._configs)
			{
				string text;
				Config config;
				keyValuePair.Deconstruct(ref text, ref config);
				config.Apply();
			}
			Action configsLoaded = this.ConfigsLoaded;
			if (configsLoaded != null)
			{
				configsLoaded.Invoke();
			}
			Action<ICoreAPI> configsChanged = ConfigLibModSystem.ConfigsChanged;
			if (configsChanged == null)
			{
				return;
			}
			configsChanged.Invoke(api);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public override double ExecuteOrder()
		{
			return 0.01;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public override void Dispose()
		{
			ICoreAPI api = this._api;
			if (api != null && api.Side == 2)
			{
				PauseMenuPatch.Patch();
			}
			foreach (KeyValuePair<string, Config> keyValuePair in this._configs)
			{
				string text;
				Config config;
				keyValuePair.Deconstruct(ref text, ref config);
				config.Dispose();
			}
			this._configs.Clear();
			this._domains.Clear();
			this._customConfigs.Clear();
			ICoreAPI api2 = this._api;
			if (api2 != null && api2.Side == 2 && this._guiManager != null)
			{
				this._guiManager.Dispose();
			}
			ICoreAPI api3 = this._api;
			if (api3 != null && api3.Side == 2)
			{
				ConfigRegistry.ConfigsLoaded -= new Action<Dictionary<string, Config>>(this.ReloadConfigs);
			}
			this._registry = null;
			this._guiManager = null;
			base.Dispose();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002DC0 File Offset: 0x00000FC0
		private static Func<string, ControlButtons, ControlButtons> DrawDelegateWrapper(Action<string, ControlButtons> callback)
		{
			return delegate(string domain, ControlButtons buttons)
			{
				callback.Invoke(domain, buttons);
				return new ControlButtons
				{
					Defaults = true,
					Reload = true,
					Restore = true,
					Save = true
				};
			};
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002DDC File Offset: 0x00000FDC
		private void ReloadConfigs(Dictionary<string, Config> configs)
		{
			ICoreAPI api = this._api;
			if (api != null)
			{
				ILogger logger = api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Configs received from server: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(configs.Count);
				logger.Notification(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			foreach (KeyValuePair<string, Config> keyValuePair in configs)
			{
				string text;
				Config config2;
				keyValuePair.Deconstruct(ref text, ref config2);
				string domain = text;
				Config config = config2;
				ConfigSetting configSetting;
				if (this._customManagedConfigs.Contains(domain))
				{
					Config config3 = this._configs[domain];
					Config config4 = config;
					ICoreClientAPI coreClientAPI = this._api as ICoreClientAPI;
					config3.SyncFromServer(config4, coreClientAPI != null && coreClientAPI.IsSinglePlayer);
					this._configs[domain].ConfigSaved += new Action<Config>(this.OnConfigSaved);
					using (Dictionary<string, ConfigSetting>.Enumerator enumerator2 = this._configs[domain].Settings.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<string, ConfigSetting> keyValuePair2 = enumerator2.Current;
							keyValuePair2.Deconstruct(ref text, ref configSetting);
							string code = text;
							ConfigSetting setting = configSetting;
							setting.SettingChanged += delegate(ConfigSetting value)
							{
								this.OnSettingChanged(domain, code, value);
							};
							this.OnSettingLoaded(domain, code, setting);
						}
						continue;
					}
				}
				this._domains.Add(domain);
				this._configs[domain] = config;
				config.Apply();
				config.ConfigSaved += new Action<Config>(this.OnConfigSaved);
				foreach (KeyValuePair<string, ConfigSetting> keyValuePair2 in config.Settings)
				{
					KeyValuePair<string, ConfigSetting> keyValuePair2;
					keyValuePair2.Deconstruct(ref text, ref configSetting);
					string code = text;
					ConfigSetting setting2 = configSetting;
					setting2.SettingChanged += delegate(ConfigSetting value)
					{
						this.OnSettingChanged(domain, code, value);
					};
					this.OnSettingLoaded(domain, code, setting2);
				}
			}
			ICoreClientAPI clientApi = this._api as ICoreClientAPI;
			if (clientApi != null)
			{
				try
				{
					this._guiManager = new GuiManager(clientApi);
					this._guiManager.ConfigWindowClosed += delegate()
					{
						Action configWindowClosed = this.ConfigWindowClosed;
						if (configWindowClosed == null)
						{
							return;
						}
						configWindowClosed.Invoke();
					};
					this._guiManager.ConfigWindowOpened += delegate()
					{
						Action configWindowOpened = this.ConfigWindowOpened;
						if (configWindowOpened == null)
						{
							return;
						}
						configWindowOpened.Invoke();
					};
				}
				catch (Exception exception)
				{
					ILogger logger2 = clientApi.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(107, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("[Config lib] Error on creating GUI manager. Probably missing ImGui or it has incorrect version.\nException:\n");
					defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception);
					logger2.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			Action configsLoaded = this.ConfigsLoaded;
			if (configsLoaded == null)
			{
				return;
			}
			configsLoaded.Invoke();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000313C File Offset: 0x0000133C
		private void LoadConfigs()
		{
			if (this._api == null)
			{
				return;
			}
			ConfigRegistry registry = this._registry ?? ConfigLibModSystem.GetRegistry(this._api);
			foreach (IAsset asset2 in Enumerable.Where<IAsset>(this._api.Assets.GetMany(AssetCategory.config.Code, null, true), (IAsset asset) => asset.Name == "configlib-patches.json"))
			{
				try
				{
					this.LoadConfig(asset2, registry);
				}
				catch (Exception exception)
				{
					this._api.Logger.Error("[Config lib] Error on loading config for " + asset2.Location.Domain + ".");
					ILogger logger = this._api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Error on loading config for ");
					defaultInterpolatedStringHandler.AppendFormatted(asset2.Location.Domain);
					defaultInterpolatedStringHandler.AppendLiteral(".\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					defaultInterpolatedStringHandler.AppendLiteral("\n");
					logger.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			if (this._api.Side == 1)
			{
				foreach (KeyValuePair<string, Config> keyValuePair in this._configsToRegister)
				{
					string text;
					Config config2;
					keyValuePair.Deconstruct(ref text, ref config2);
					string domain = text;
					Config config = config2;
					if (registry != null)
					{
						registry.Register(domain, config);
					}
				}
				this._configsToRegister.Clear();
			}
			ConfigRegistry.OnToBytes += delegate()
			{
				this._canRegisterNewConfig = false;
			};
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003310 File Offset: 0x00001510
		private void LoadConfig(IAsset asset, [Nullable(2)] ConfigRegistry registry)
		{
			ConfigLibModSystem.<>c__DisplayClass50_0 CS$<>8__locals1 = new ConfigLibModSystem.<>c__DisplayClass50_0();
			CS$<>8__locals1.<>4__this = this;
			if (this._api == null)
			{
				return;
			}
			CS$<>8__locals1.domain = asset.Location.Domain;
			byte[] data = asset.Data;
			data = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, data);
			string json = Encoding.Unicode.GetString(data);
			int startIndex = 0;
			if (json.Contains('{'))
			{
				startIndex = json.IndexOf('{');
			}
			json = json.Substring(startIndex, json.Length - startIndex);
			JsonObject parsedConfig = new JsonObject(JObject.Parse(json));
			if (parsedConfig.KeyExists("file"))
			{
				ConfigLibModSystem.<>c__DisplayClass50_0 CS$<>8__locals2 = CS$<>8__locals1;
				ICoreAPI api = this._api;
				string domain = CS$<>8__locals1.domain;
				Mod mod = this._api.ModLoader.GetMod(CS$<>8__locals1.domain);
				CS$<>8__locals2.config = new Config(api, domain, ((mod != null) ? mod.Info.Name : null) ?? Lang.Get(CS$<>8__locals1.domain, Array.Empty<object>()), parsedConfig, parsedConfig["file"].AsString(null));
			}
			else
			{
				ConfigLibModSystem.<>c__DisplayClass50_0 CS$<>8__locals3 = CS$<>8__locals1;
				ICoreAPI api2 = this._api;
				string domain2 = CS$<>8__locals1.domain;
				Mod mod2 = this._api.ModLoader.GetMod(CS$<>8__locals1.domain);
				CS$<>8__locals3.config = new Config(api2, domain2, ((mod2 != null) ? mod2.Info.Name : null) ?? Lang.Get(CS$<>8__locals1.domain, Array.Empty<object>()), parsedConfig);
			}
			this._configs.Add(CS$<>8__locals1.domain, CS$<>8__locals1.config);
			this._domains.Add(CS$<>8__locals1.domain);
			if (registry != null)
			{
				registry.Register(CS$<>8__locals1.domain, CS$<>8__locals1.config);
			}
			if (this._api.Side == 1)
			{
				CS$<>8__locals1.config.ConfigSaved += new Action<Config>(this.OnConfigSaved);
				foreach (KeyValuePair<string, ConfigSetting> keyValuePair in CS$<>8__locals1.config.Settings)
				{
					string code2;
					ConfigSetting configSetting;
					keyValuePair.Deconstruct(ref code2, ref configSetting);
					string code = code2;
					ConfigSetting setting2 = configSetting;
					setting2.SettingChanged += delegate(ConfigSetting value)
					{
						CS$<>8__locals1.<>4__this.OnSettingChanged(CS$<>8__locals1.domain, code, value);
					};
					this.OnSettingLoaded(CS$<>8__locals1.domain, code, setting2);
				}
			}
			CS$<>8__locals1.config.SettingChanged += delegate(ISetting setting)
			{
				Action<string, IConfig, ISetting> settingChanged = CS$<>8__locals1.<>4__this.SettingChanged;
				if (settingChanged == null)
				{
					return;
				}
				settingChanged.Invoke(CS$<>8__locals1.domain, CS$<>8__locals1.config, setting);
			};
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003580 File Offset: 0x00001780
		[return: Nullable(2)]
		private static ConfigRegistry GetRegistry(ICoreAPI api)
		{
			GameMain gameMain = api.World as GameMain;
			return ((gameMain != null) ? gameMain.GetRecipeRegistry("configlib:configs") : null) as ConfigRegistry;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000035A4 File Offset: 0x000017A4
		private void OnConfigSaved(Config config)
		{
			TreeAttribute eventDataTree = new TreeAttribute();
			eventDataTree.SetString("domain", config.Domain);
			string eventName = string.Format("configlib:{0}:config-saved", config.Domain);
			ICoreAPI api = this._api;
			if (api != null)
			{
				api.Event.PushEvent(eventName, eventDataTree);
			}
			this.SendEventToServer(eventName, eventDataTree);
			ICoreClientAPI clientApi = this._api as ICoreClientAPI;
			if (clientApi != null && clientApi.World.Player.HasPrivilege(Privilege.controlserver))
			{
				ServerSideSettingChanged packet = new ServerSideSettingChanged(config.Domain, Enumerable.ToDictionary<string, ConfigSetting>(Enumerable.Where<KeyValuePair<string, ConfigSetting>>(config.Settings, (KeyValuePair<string, ConfigSetting> setting) => setting.Value.ChangedSinceLastSave && !setting.Value.ClientSide)), clientApi.IsSinglePlayer);
				if (Enumerable.Any<KeyValuePair<string, ConfigSettingPacket>>(packet.Settings))
				{
					IClientNetworkChannel eventsChannel = this._eventsChannel;
					if (eventsChannel != null)
					{
						eventsChannel.SendPacket<ServerSideSettingChanged>(packet);
					}
				}
			}
			foreach (KeyValuePair<string, ConfigSetting> keyValuePair in config.Settings)
			{
				string text;
				ConfigSetting configSetting;
				keyValuePair.Deconstruct(ref text, ref configSetting);
				configSetting.ChangedSinceLastSave = false;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000036D4 File Offset: 0x000018D4
		private void OnSettingChanged(string domain, string code, ConfigSetting setting)
		{
			TreeAttribute eventDataTree = new TreeAttribute();
			eventDataTree.SetString("domain", domain);
			eventDataTree.SetString("setting", code);
			switch (setting.SettingType)
			{
			case ConfigSettingType.Boolean:
				eventDataTree.SetBool("value", setting.Value.AsBool(false));
				break;
			case ConfigSettingType.Float:
				eventDataTree.SetFloat("value", setting.Value.AsFloat(0f));
				break;
			case ConfigSettingType.Integer:
				eventDataTree.SetInt("value", setting.Value.AsInt(0));
				break;
			case ConfigSettingType.String:
				eventDataTree.SetString("value", setting.Value.AsString(null));
				break;
			case ConfigSettingType.Other:
				eventDataTree.SetAttribute("value", setting.Value.ToAttribute());
				break;
			}
			string eventName = string.Format("configlib:{0}:setting-changed", domain);
			ICoreAPI api = this._api;
			if (api != null)
			{
				api.Event.PushEvent(eventName, eventDataTree);
			}
			this.SendEventToServer(eventName, eventDataTree);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000037D4 File Offset: 0x000019D4
		private void OnSettingLoaded(string domain, string code, ConfigSetting setting)
		{
			TreeAttribute eventDataTree = new TreeAttribute();
			eventDataTree.SetString("domain", domain);
			eventDataTree.SetString("setting", code);
			switch (setting.SettingType)
			{
			case ConfigSettingType.Boolean:
				eventDataTree.SetBool("value", setting.Value.AsBool(false));
				break;
			case ConfigSettingType.Float:
				eventDataTree.SetFloat("value", setting.Value.AsFloat(0f));
				break;
			case ConfigSettingType.Integer:
				eventDataTree.SetInt("value", setting.Value.AsInt(0));
				break;
			case ConfigSettingType.String:
				eventDataTree.SetString("value", setting.Value.AsString(null));
				break;
			case ConfigSettingType.Other:
				eventDataTree.SetAttribute("value", setting.Value.ToAttribute());
				break;
			}
			string eventName = string.Format("configlib:{0}:setting-loaded", domain);
			ICoreAPI api = this._api;
			if (api == null)
			{
				return;
			}
			api.Event.PushEvent(eventName, eventDataTree);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000038CC File Offset: 0x00001ACC
		private void ReloadJsonConfigs(string eventName, ref EnumHandling handling, IAttribute data)
		{
			ITreeAttribute treeAttribute = data as ITreeAttribute;
			string domain = ((treeAttribute != null) ? treeAttribute.GetAsString("domain", null) : null) ?? "";
			this._configs[domain].ReadFromFile();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003910 File Offset: 0x00001B10
		private void OnServerSettingChanged(IServerPlayer player, ServerSideSettingChanged packet)
		{
			if (!Enumerable.Any<KeyValuePair<string, ConfigSettingPacket>>(packet.Settings))
			{
				return;
			}
			if (!player.HasPrivilege(Privilege.controlserver))
			{
				ICoreAPI api = this._api;
				if (api != null)
				{
					ILogger logger = api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(79, 3);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Player '");
					defaultInterpolatedStringHandler.AppendFormatted(player.PlayerName);
					defaultInterpolatedStringHandler.AppendLiteral("' without privilege '");
					defaultInterpolatedStringHandler.AppendFormatted(Privilege.controlserver);
					defaultInterpolatedStringHandler.AppendLiteral("' tried to change config for mod  '");
					defaultInterpolatedStringHandler.AppendFormatted(packet.ConfigDomain);
					defaultInterpolatedStringHandler.AppendLiteral("'.");
					logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ICoreAPI api2 = this._api;
				if (api2 == null)
				{
					return;
				}
				ILogger logger2 = api2.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(57, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("[Config lib] missing privilege to change config: '");
				defaultInterpolatedStringHandler2.AppendFormatted(player.PlayerName);
				defaultInterpolatedStringHandler2.AppendLiteral("' - '");
				defaultInterpolatedStringHandler2.AppendFormatted(packet.ConfigDomain);
				defaultInterpolatedStringHandler2.AppendLiteral("'.");
				logger2.Audit(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			else
			{
				Config config = this.GetConfigImpl(packet.ConfigDomain);
				if (config == null)
				{
					ICoreAPI api3 = this._api;
					if (api3 == null)
					{
						return;
					}
					ILogger logger3 = api3.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
					defaultInterpolatedStringHandler3..ctor(81, 2);
					defaultInterpolatedStringHandler3.AppendLiteral("[Config lib] Player '");
					defaultInterpolatedStringHandler3.AppendFormatted(player.PlayerName);
					defaultInterpolatedStringHandler3.AppendLiteral("' tried to change config '");
					defaultInterpolatedStringHandler3.AppendFormatted(packet.ConfigDomain);
					defaultInterpolatedStringHandler3.AppendLiteral("', but such config does not exist.");
					logger3.Error(defaultInterpolatedStringHandler3.ToStringAndClear());
					return;
				}
				else
				{
					string settingsChanged = "";
					foreach (KeyValuePair<string, ConfigSettingPacket> keyValuePair in packet.Settings)
					{
						string text;
						ConfigSettingPacket settings;
						keyValuePair.Deconstruct(ref text, ref settings);
						string settingCode = text;
						ConfigSetting settingFromClient = new ConfigSetting(settings);
						ConfigSetting serverSetting = (ConfigSetting)config.GetSetting(settingCode);
						if (serverSetting == null)
						{
							ICoreAPI api4 = this._api;
							if (api4 != null)
							{
								ILogger logger4 = api4.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(119, 3);
								defaultInterpolatedStringHandler4.AppendLiteral("[Config lib] Player '");
								defaultInterpolatedStringHandler4.AppendFormatted(player.PlayerName);
								defaultInterpolatedStringHandler4.AppendLiteral("' tried to change setting '");
								defaultInterpolatedStringHandler4.AppendFormatted(settingCode);
								defaultInterpolatedStringHandler4.AppendLiteral("' in config for mod '");
								defaultInterpolatedStringHandler4.AppendFormatted(packet.ConfigDomain);
								defaultInterpolatedStringHandler4.AppendLiteral("', but such setting does not exist in this config.");
								logger4.Error(defaultInterpolatedStringHandler4.ToStringAndClear());
							}
						}
						else if (!serverSetting.ClientSide)
						{
							serverSetting.Value = settingFromClient.Value;
							serverSetting.MappingKey = settingFromClient.MappingKey;
							if (settingsChanged != "")
							{
								settingsChanged += ", ";
							}
							settingsChanged += serverSetting.YamlCode;
						}
					}
					ICoreAPI api5 = this._api;
					if (api5 != null)
					{
						ILogger logger5 = api5.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5;
						defaultInterpolatedStringHandler5..ctor(37, 3);
						defaultInterpolatedStringHandler5.AppendLiteral("[Config lib] config changed: '");
						defaultInterpolatedStringHandler5.AppendFormatted(player.PlayerName);
						defaultInterpolatedStringHandler5.AppendLiteral("' - ");
						defaultInterpolatedStringHandler5.AppendFormatted(packet.ConfigDomain);
						defaultInterpolatedStringHandler5.AppendLiteral(" - ");
						defaultInterpolatedStringHandler5.AppendFormatted(settingsChanged);
						logger5.Audit(defaultInterpolatedStringHandler5.ToStringAndClear());
					}
					ICoreAPI api6 = this._api;
					if (api6 != null)
					{
						ILogger logger6 = api6.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6;
						defaultInterpolatedStringHandler6..ctor(79, 4);
						defaultInterpolatedStringHandler6.AppendLiteral("[Config lib] Player '");
						defaultInterpolatedStringHandler6.AppendFormatted(player.PlayerName);
						defaultInterpolatedStringHandler6.AppendLiteral("' changed settings: ");
						defaultInterpolatedStringHandler6.AppendFormatted(settingsChanged);
						defaultInterpolatedStringHandler6.AppendLiteral(", and saved config file for mod '");
						Mod mod = this._api.ModLoader.GetMod(packet.ConfigDomain);
						defaultInterpolatedStringHandler6.AppendFormatted((mod != null) ? mod.Info.Name : null);
						defaultInterpolatedStringHandler6.AppendLiteral(" (");
						defaultInterpolatedStringHandler6.AppendFormatted(packet.ConfigDomain);
						defaultInterpolatedStringHandler6.AppendLiteral(")'.");
						logger6.Notification(defaultInterpolatedStringHandler6.ToStringAndClear());
					}
					if (!packet.IsSinglePlayer)
					{
						config.WriteToFile();
					}
					IServerNetworkChannel eventsServerChannel = this._eventsServerChannel;
					if (eventsServerChannel == null)
					{
						return;
					}
					eventsServerChannel.BroadcastPacket<ServerSideSettingChanged>(packet, new IServerPlayer[]
					{
						player
					});
					return;
				}
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003D30 File Offset: 0x00001F30
		private void OnServerSettingChanged(ServerSideSettingChanged packet)
		{
			if (!Enumerable.Any<KeyValuePair<string, ConfigSettingPacket>>(packet.Settings))
			{
				return;
			}
			Config config = this.GetConfigImpl(packet.ConfigDomain);
			if (config == null)
			{
				return;
			}
			string settingsChanged = "";
			foreach (KeyValuePair<string, ConfigSettingPacket> keyValuePair in packet.Settings)
			{
				string text;
				ConfigSettingPacket settings;
				keyValuePair.Deconstruct(ref text, ref settings);
				string settingCode = text;
				ConfigSetting settingFromClient = new ConfigSetting(settings);
				ConfigSetting serverSetting = (ConfigSetting)config.GetSetting(settingCode);
				if (serverSetting != null && !serverSetting.ClientSide)
				{
					serverSetting.Value = settingFromClient.Value;
					serverSetting.MappingKey = settingFromClient.MappingKey;
					if (settingsChanged != "")
					{
						settingsChanged += ", ";
					}
					settingsChanged += serverSetting.YamlCode;
				}
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003E1C File Offset: 0x0000201C
		private void SendEventToServer(string eventName, TreeAttribute eventData)
		{
			ICoreClientAPI clientApi = this._api as ICoreClientAPI;
			if (clientApi == null || !clientApi.World.Player.HasPrivilege(Privilege.controlserver))
			{
				return;
			}
			ConfigEventPacket eventPacket = new ConfigEventPacket
			{
				EventName = eventName,
				Data = eventData.ToBytes()
			};
			IClientNetworkChannel eventsChannel = this._eventsChannel;
			if (eventsChannel == null)
			{
				return;
			}
			eventsChannel.SendPacket<ConfigEventPacket>(eventPacket);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003E7C File Offset: 0x0000207C
		private void SendEvent(IServerPlayer fromPlayer, ConfigEventPacket eventPacket)
		{
			if (eventPacket.EventName == null || !eventPacket.EventName.StartsWith("configlib:"))
			{
				ICoreAPI api = this._api;
				if (api != null)
				{
					ILogger logger = api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(74, 2);
					defaultInterpolatedStringHandler.AppendLiteral("[Config lib] Player '");
					defaultInterpolatedStringHandler.AppendFormatted(fromPlayer.PlayerName);
					defaultInterpolatedStringHandler.AppendLiteral("' tried to push event outside of ConfigLib scope: '");
					defaultInterpolatedStringHandler.AppendFormatted(eventPacket.EventName);
					defaultInterpolatedStringHandler.AppendLiteral("'.");
					logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ICoreAPI api2 = this._api;
				if (api2 == null)
				{
					return;
				}
				ILogger logger2 = api2.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(69, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("[Config lib] received event push outside of ConfigLib scope: '");
				defaultInterpolatedStringHandler2.AppendFormatted(fromPlayer.PlayerName);
				defaultInterpolatedStringHandler2.AppendLiteral("' - '");
				defaultInterpolatedStringHandler2.AppendFormatted(eventPacket.EventName);
				defaultInterpolatedStringHandler2.AppendLiteral("'.");
				logger2.Audit(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			else if (!fromPlayer.HasPrivilege(Privilege.controlserver))
			{
				ICoreAPI api3 = this._api;
				if (api3 != null)
				{
					ILogger logger3 = api3.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
					defaultInterpolatedStringHandler3..ctor(67, 3);
					defaultInterpolatedStringHandler3.AppendLiteral("[Config lib] Player '");
					defaultInterpolatedStringHandler3.AppendFormatted(fromPlayer.PlayerName);
					defaultInterpolatedStringHandler3.AppendLiteral("' without privilege '");
					defaultInterpolatedStringHandler3.AppendFormatted(Privilege.controlserver);
					defaultInterpolatedStringHandler3.AppendLiteral("' tried to push event '");
					defaultInterpolatedStringHandler3.AppendFormatted(eventPacket.EventName);
					defaultInterpolatedStringHandler3.AppendLiteral("'.");
					logger3.Warning(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				ICoreAPI api4 = this._api;
				if (api4 == null)
				{
					return;
				}
				ILogger logger4 = api4.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
				defaultInterpolatedStringHandler4..ctor(54, 2);
				defaultInterpolatedStringHandler4.AppendLiteral("[Config lib] missing privilege to push event: '");
				defaultInterpolatedStringHandler4.AppendFormatted(fromPlayer.PlayerName);
				defaultInterpolatedStringHandler4.AppendLiteral("' - '");
				defaultInterpolatedStringHandler4.AppendFormatted(eventPacket.EventName);
				defaultInterpolatedStringHandler4.AppendLiteral("'.");
				logger4.Audit(defaultInterpolatedStringHandler4.ToStringAndClear());
				return;
			}
			else
			{
				TreeAttribute eventDataTree = new TreeAttribute();
				eventDataTree.FromBytes(eventPacket.Data);
				eventDataTree.SetString("player", fromPlayer.PlayerUID);
				ICoreAPI api5 = this._api;
				if (api5 == null)
				{
					return;
				}
				api5.Event.PushEvent(eventPacket.EventName, eventDataTree);
				return;
			}
		}

		// Token: 0x04000014 RID: 20
		public const string ConfigSavedEvent = "configlib:{0}:config-saved";

		// Token: 0x04000015 RID: 21
		public const string ConfigChangedEvent = "configlib:{0}:setting-changed";

		// Token: 0x04000016 RID: 22
		public const string ConfigLoadedEvent = "configlib:{0}:setting-loaded";

		// Token: 0x04000017 RID: 23
		public const string ConfigReloadEvent = "configlib:config-reload";

		// Token: 0x04000019 RID: 25
		private readonly Dictionary<string, Config> _configs = new Dictionary<string, Config>();

		// Token: 0x0400001A RID: 26
		private readonly HashSet<string> _domains = new HashSet<string>();

		// Token: 0x0400001B RID: 27
		private readonly Dictionary<string, Func<string, ControlButtons, ControlButtons>> _customConfigs = new Dictionary<string, Func<string, ControlButtons, ControlButtons>>();

		// Token: 0x0400001C RID: 28
		private readonly Dictionary<string, Config> _configsToRegister = new Dictionary<string, Config>();

		// Token: 0x0400001D RID: 29
		private readonly HashSet<string> _customManagedConfigs = new HashSet<string>();

		// Token: 0x0400001E RID: 30
		[Nullable(2)]
		private GuiManager _guiManager;

		// Token: 0x0400001F RID: 31
		[Nullable(2)]
		private ICoreAPI _api;

		// Token: 0x04000020 RID: 32
		private const string _registryCode = "configlib:configs";

		// Token: 0x04000021 RID: 33
		[Nullable(2)]
		private ConfigRegistry _registry;

		// Token: 0x04000022 RID: 34
		[Nullable(2)]
		private IClientNetworkChannel _eventsChannel;

		// Token: 0x04000023 RID: 35
		[Nullable(2)]
		private IServerNetworkChannel _eventsServerChannel;

		// Token: 0x04000024 RID: 36
		private const string _channelName = "configlib:events";

		// Token: 0x04000025 RID: 37
		private bool _canRegisterNewConfig = true;
	}
}
