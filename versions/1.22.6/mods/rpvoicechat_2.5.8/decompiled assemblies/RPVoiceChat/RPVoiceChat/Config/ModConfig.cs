using System;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace RPVoiceChat.Config
{
	// Token: 0x02000081 RID: 129
	public static class ModConfig
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00016531 File Offset: 0x00014731
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x00016538 File Offset: 0x00014738
		public static RPVoiceChatClientConfig ClientConfig { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00016540 File Offset: 0x00014740
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x00016547 File Offset: 0x00014747
		public static RPVoiceChatServerConfig ServerConfig { get; private set; }

		// Token: 0x060003D3 RID: 979 RVA: 0x0001654F File Offset: 0x0001474F
		private static string GetConfigDirectory()
		{
			return Path.Combine(GamePaths.ModConfig, "RPVoiceChat");
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00016560 File Offset: 0x00014760
		private static void EnsureConfigDirectoryExists()
		{
			try
			{
				string configDir = ModConfig.GetConfigDirectory();
				if (!Directory.Exists(configDir))
				{
					Directory.CreateDirectory(configDir);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00016598 File Offset: 0x00014798
		public static void Init(ICoreAPI api)
		{
			ModConfig.EnsureConfigDirectoryExists();
			if (api.Side == 2)
			{
				ModConfig.ClientConfig = ModConfig.ReadConfig<RPVoiceChatClientConfig>(api, "rpvoicechat-client.json");
				ModConfig.ServerConfig = ModConfig.ReadConfig<RPVoiceChatServerConfig>(api, "rpvoicechat-server.json");
				ServerConfigManager.ValidateConfig();
				return;
			}
			ModConfig.ServerConfig = ModConfig.ReadConfig<RPVoiceChatServerConfig>(api, "rpvoicechat-server.json");
			ServerConfigManager.ValidateConfig();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000165EE File Offset: 0x000147EE
		public static void SaveClient(ICoreAPI api)
		{
			if (api.Side == 2 && ModConfig.ClientConfig != null)
			{
				ModConfig.WriteConfig<RPVoiceChatClientConfig>(api, "rpvoicechat-client.json", ModConfig.ClientConfig);
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00016610 File Offset: 0x00014810
		public static void SaveServer(ICoreAPI api)
		{
			if (api.Side == 1 && ModConfig.ServerConfig != null)
			{
				ModConfig.WriteConfig<RPVoiceChatServerConfig>(api, "rpvoicechat-server.json", ModConfig.ServerConfig);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00016634 File Offset: 0x00014834
		private static T ReadConfig<T>(ICoreAPI api, string jsonConfig) where T : class, IModConfig
		{
			T config;
			try
			{
				config = ModConfig.LoadConfig<T>(api, jsonConfig);
				if (config == null)
				{
					api.Logger.Notification("[RPVoiceChat] No " + typeof(T).Name + " found, generating default one.");
					ModConfig.GenerateConfig<T>(api, jsonConfig, default(T));
					config = ModConfig.LoadConfig<T>(api, jsonConfig);
				}
			}
			catch (Exception e)
			{
				api.Logger.Warning("[RPVoiceChat] Error loading " + typeof(T).Name + ": " + e.Message);
				api.Logger.Warning("[RPVoiceChat] Generating default config.");
				ModConfig.GenerateConfig<T>(api, jsonConfig, default(T));
				config = ModConfig.LoadConfig<T>(api, jsonConfig);
			}
			return config;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00016700 File Offset: 0x00014900
		private static void WriteConfig<T>(ICoreAPI api, string jsonConfig, T config) where T : class, IModConfig
		{
			try
			{
				ModConfig.GenerateConfig<T>(api, jsonConfig, config);
			}
			catch (Exception e)
			{
				api.Logger.Error("[RPVoiceChat] Failed to save " + typeof(T).Name + ": " + e.Message);
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001675C File Offset: 0x0001495C
		private static T LoadConfig<T>(ICoreAPI api, string jsonConfig) where T : class, IModConfig
		{
			T result;
			try
			{
				string configPath = Path.Combine(ModConfig.GetConfigDirectory(), jsonConfig);
				if (File.Exists(configPath))
				{
					result = JsonConvert.DeserializeObject<T>(File.ReadAllText(configPath));
				}
				else
				{
					try
					{
						T oldConfig = api.LoadModConfig<T>(jsonConfig);
						if (oldConfig != null)
						{
							ILogger logger = api.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
							defaultInterpolatedStringHandler.AppendLiteral("[RPVoiceChat] Migrating ");
							defaultInterpolatedStringHandler.AppendFormatted(jsonConfig);
							defaultInterpolatedStringHandler.AppendLiteral(" to new location in ");
							defaultInterpolatedStringHandler.AppendFormatted("RPVoiceChat");
							defaultInterpolatedStringHandler.AppendLiteral(" folder");
							logger.Notification(defaultInterpolatedStringHandler.ToStringAndClear());
							ModConfig.GenerateConfig<T>(api, jsonConfig, oldConfig);
							try
							{
								string oldConfigPath = Path.Combine(GamePaths.ModConfig, jsonConfig);
								if (File.Exists(oldConfigPath))
								{
									File.Delete(oldConfigPath);
									api.Logger.Notification("[RPVoiceChat] Deleted old config file: " + oldConfigPath);
								}
							}
							catch (Exception deleteEx)
							{
								api.Logger.Warning("[RPVoiceChat] Failed to delete old config file: " + deleteEx.Message);
							}
							return oldConfig;
						}
					}
					catch
					{
					}
					result = default(T);
				}
			}
			catch (Exception e)
			{
				api.Logger.Warning("[RPVoiceChat] Failed to load " + jsonConfig + ": " + e.Message);
				result = default(T);
			}
			return result;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000168E8 File Offset: 0x00014AE8
		private static void GenerateConfig<T>(ICoreAPI api, string jsonConfig, T previousConfig = default(T)) where T : class, IModConfig
		{
			try
			{
				ModConfig.EnsureConfigDirectoryExists();
				T configToSave = ModConfig.CloneConfig<T>(api, previousConfig);
				string text = Path.Combine(ModConfig.GetConfigDirectory(), jsonConfig);
				string jsonContent = JsonConvert.SerializeObject(configToSave, 1);
				File.WriteAllText(text, jsonContent);
			}
			catch (Exception e)
			{
				api.Logger.Error("[RPVoiceChat] Failed to generate " + jsonConfig + ": " + e.Message);
				throw;
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00016958 File Offset: 0x00014B58
		private static T CloneConfig<T>(ICoreAPI api, T config = default(T)) where T : class, IModConfig
		{
			T result;
			try
			{
				result = (T)((object)Activator.CreateInstance(typeof(T), new object[]
				{
					api,
					config
				}));
			}
			catch (Exception e)
			{
				api.Logger.Error("[RPVoiceChat] Failed to clone config " + typeof(T).Name + ": " + e.Message);
				result = (T)((object)Activator.CreateInstance(typeof(T)));
			}
			return result;
		}

		// Token: 0x040001F1 RID: 497
		private const string ConfigSubfolder = "RPVoiceChat";

		// Token: 0x040001F2 RID: 498
		public const string ClientConfigName = "rpvoicechat-client.json";

		// Token: 0x040001F3 RID: 499
		public const string ServerConfigName = "rpvoicechat-server.json";
	}
}
