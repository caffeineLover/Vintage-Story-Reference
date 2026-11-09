using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace RPVoiceChat.Config
{
	// Token: 0x020000AE RID: 174
	public class VoiceBanConfigManager
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x0001E533 File Offset: 0x0001C733
		public VoiceBanConfigManager(ICoreServerAPI sapi)
		{
			this.api = sapi;
			this.InitializePaths();
			this.EnsureConfigDirectoryExists();
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001E550 File Offset: 0x0001C750
		private void InitializePaths()
		{
			string configPath = GamePaths.ModConfig;
			this.configDirectory = Path.Combine(configPath, "RPVoiceChat");
			this.bannedFilePath = Path.Combine(this.configDirectory, "rpvoicechat-banned.json");
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001E58C File Offset: 0x0001C78C
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

		// Token: 0x0600058E RID: 1422 RVA: 0x0001E608 File Offset: 0x0001C808
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

		// Token: 0x0600058F RID: 1423 RVA: 0x0001E698 File Offset: 0x0001C898
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

		// Token: 0x040002CF RID: 719
		private const string ConfigSubfolder = "RPVoiceChat";

		// Token: 0x040002D0 RID: 720
		private const string BannedFileName = "rpvoicechat-banned.json";

		// Token: 0x040002D1 RID: 721
		private string configDirectory;

		// Token: 0x040002D2 RID: 722
		private string bannedFilePath;

		// Token: 0x040002D3 RID: 723
		private ICoreServerAPI api;

		// Token: 0x02000161 RID: 353
		private class BannedConfig
		{
			// Token: 0x170001C5 RID: 453
			// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0003A634 File Offset: 0x00038834
			// (set) Token: 0x06000B31 RID: 2865 RVA: 0x0003A63C File Offset: 0x0003883C
			public List<string> BannedPlayers { get; set; } = new List<string>();
		}
	}
}
