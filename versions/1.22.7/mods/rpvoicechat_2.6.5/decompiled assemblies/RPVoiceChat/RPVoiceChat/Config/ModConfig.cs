using System;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace RPVoiceChat.Config
{
	// Token: 0x020000AA RID: 170
	public static class ModConfig
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001C5C5 File Offset: 0x0001A7C5
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x0001C5CC File Offset: 0x0001A7CC
		public static RPVoiceChatClientConfig ClientConfig { get; private set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001C5D4 File Offset: 0x0001A7D4
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x0001C5DB File Offset: 0x0001A7DB
		public static RPVoiceChatServerConfig ServerConfig { get; private set; }

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001C5E3 File Offset: 0x0001A7E3
		private static string GetConfigDirectory()
		{
			return Path.Combine(GamePaths.ModConfig, "RPVoiceChat");
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001C5F4 File Offset: 0x0001A7F4
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

		// Token: 0x060004DB RID: 1243 RVA: 0x0001C62C File Offset: 0x0001A82C
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

		// Token: 0x060004DC RID: 1244 RVA: 0x0001C682 File Offset: 0x0001A882
		public static void SaveClient(ICoreAPI api)
		{
			if (api.Side == 2 && ModConfig.ClientConfig != null)
			{
				ModConfig.WriteConfig<RPVoiceChatClientConfig>(api, "rpvoicechat-client.json", ModConfig.ClientConfig);
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001C6A4 File Offset: 0x0001A8A4
		public static void SaveServer(ICoreAPI api)
		{
			if (api.Side == 1 && ModConfig.ServerConfig != null)
			{
				ModConfig.WriteConfig<RPVoiceChatServerConfig>(api, "rpvoicechat-server.json", ModConfig.ServerConfig);
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001C6C8 File Offset: 0x0001A8C8
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

		// Token: 0x060004DF RID: 1247 RVA: 0x0001C794 File Offset: 0x0001A994
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

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001C7F0 File Offset: 0x0001A9F0
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

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001C97C File Offset: 0x0001AB7C
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

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001C9EC File Offset: 0x0001ABEC
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

		// Token: 0x0400028C RID: 652
		private const string ConfigSubfolder = "RPVoiceChat";

		// Token: 0x0400028D RID: 653
		public const string ClientConfigName = "rpvoicechat-client.json";

		// Token: 0x0400028E RID: 654
		public const string ServerConfigName = "rpvoicechat-server.json";
	}
}
