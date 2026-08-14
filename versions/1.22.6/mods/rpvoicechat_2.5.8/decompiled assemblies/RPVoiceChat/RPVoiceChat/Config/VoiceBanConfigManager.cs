using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace RPVoiceChat.Config
{
	// Token: 0x02000085 RID: 133
	public class VoiceBanConfigManager
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x00018057 File Offset: 0x00016257
		public VoiceBanConfigManager(ICoreServerAPI sapi)
		{
			this.api = sapi;
			this.InitializePaths();
			this.EnsureConfigDirectoryExists();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00018074 File Offset: 0x00016274
		private void InitializePaths()
		{
			string configPath = GamePaths.ModConfig;
			this.configDirectory = Path.Combine(configPath, "RPVoiceChat");
			this.bannedFilePath = Path.Combine(this.configDirectory, "rpvoicechat-banned.json");
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000180B0 File Offset: 0x000162B0
		private void EnsureConfigDirectoryExists()
		{
			try
			{
				if (!Directory.Exists(this.configDirectory))
				{
					Directory.CreateDirectory(this.configDirectory);
					this.api.Logger.Notification("[RPVoiceChat] Created config directory: " + this.configDirectory);
				}
			}
			catch (Exception e)
			{
				this.api.Logger.Error("[RPVoiceChat] Failed to create config directory: " + e.Message);
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001812C File Offset: 0x0001632C
		public List<string> LoadBannedPlayers()
		{
			if (!File.Exists(this.bannedFilePath))
			{
				return new List<string>();
			}
			List<string> result;
			try
			{
				VoiceBanConfigManager.BannedConfig bannedConfig = JsonConvert.DeserializeObject<VoiceBanConfigManager.BannedConfig>(File.ReadAllText(this.bannedFilePath));
				result = (((bannedConfig != null) ? bannedConfig.BannedPlayers : null) ?? new List<string>());
			}
			catch (Exception e)
			{
				this.api.Logger.Warning("[RPVoiceChat] Error loading banned players from " + this.bannedFilePath + ": " + e.Message);
				result = new List<string>();
			}
			return result;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000181BC File Offset: 0x000163BC
		public void SaveBannedPlayers(List<string> bannedPlayers)
		{
			try
			{
				string jsonContent = JsonConvert.SerializeObject(new VoiceBanConfigManager.BannedConfig
				{
					BannedPlayers = (bannedPlayers ?? new List<string>())
				}, 1);
				File.WriteAllText(this.bannedFilePath, jsonContent);
			}
			catch (Exception e)
			{
				this.api.Logger.Error("[RPVoiceChat] Error saving banned players to " + this.bannedFilePath + ": " + e.Message);
			}
		}

		// Token: 0x0400022D RID: 557
		private const string ConfigSubfolder = "RPVoiceChat";

		// Token: 0x0400022E RID: 558
		private const string BannedFileName = "rpvoicechat-banned.json";

		// Token: 0x0400022F RID: 559
		private string configDirectory;

		// Token: 0x04000230 RID: 560
		private string bannedFilePath;

		// Token: 0x04000231 RID: 561
		private ICoreServerAPI api;

		// Token: 0x0200010F RID: 271
		private class BannedConfig
		{
			// Token: 0x17000162 RID: 354
			// (get) Token: 0x06000871 RID: 2161 RVA: 0x0002C8F0 File Offset: 0x0002AAF0
			// (set) Token: 0x06000872 RID: 2162 RVA: 0x0002C8F8 File Offset: 0x0002AAF8
			public List<string> BannedPlayers { get; set; } = new List<string>();
		}
	}
}
