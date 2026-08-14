using System;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace RPVoiceChat.Config
{
	// Token: 0x02000086 RID: 134
	internal static class WorldConfig
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x00018234 File Offset: 0x00016434
		public static void Init(ICoreAPI api)
		{
			if (WorldConfig.api != null && api.Side == 2)
			{
				return;
			}
			WorldConfig.api = api;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00018250 File Offset: 0x00016450
		public static void Set(VoiceLevel voiceLevel, int distance)
		{
			string key;
			if (WorldConfig.configKeyByVoiceLevel.TryGetValue(voiceLevel, ref key))
			{
				WorldConfig.Set(key, distance);
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00018273 File Offset: 0x00016473
		public static void Set(string key, int value)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI == null)
			{
				return;
			}
			coreAPI.World.Config.SetInt(WorldConfig.Key(key), value);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00018295 File Offset: 0x00016495
		public static void Set(string key, bool value)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI == null)
			{
				return;
			}
			coreAPI.World.Config.SetBool(WorldConfig.Key(key), value);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000182B7 File Offset: 0x000164B7
		public static void Set(string key, float value)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI == null)
			{
				return;
			}
			coreAPI.World.Config.SetFloat(WorldConfig.Key(key), value);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000182D9 File Offset: 0x000164D9
		public static void Set(string key, string value)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI == null)
			{
				return;
			}
			coreAPI.World.Config.SetString(WorldConfig.Key(key), value);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000182FC File Offset: 0x000164FC
		public static int GetInt(VoiceLevel voiceLevel, int? defaultValue = null)
		{
			string key;
			if (WorldConfig.configKeyByVoiceLevel.TryGetValue(voiceLevel, ref key))
			{
				int fallbackDefault = defaultValue ?? ((int)voiceLevel);
				return WorldConfig.GetInt(key, fallbackDefault);
			}
			int? num = defaultValue;
			if (num == null)
			{
				return (int)voiceLevel;
			}
			return num.GetValueOrDefault();
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001834C File Offset: 0x0001654C
		public static int GetInt(string key, int defaultValue = 0)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI != null && coreAPI.World.Config.HasAttribute(WorldConfig.Key(key)))
			{
				return WorldConfig.api.World.Config.GetInt(WorldConfig.Key(key), 0);
			}
			return defaultValue;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001839C File Offset: 0x0001659C
		public static bool GetBool(string key, bool defaultValue = false)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI != null && coreAPI.World.Config.HasAttribute(WorldConfig.Key(key)))
			{
				return WorldConfig.api.World.Config.GetBool(WorldConfig.Key(key), false);
			}
			return defaultValue;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000183E9 File Offset: 0x000165E9
		public static bool GetPlayerNametagTargetedOnly()
		{
			return WorldConfig.GetBool("player-nametag-targeted-only", true);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000183F6 File Offset: 0x000165F6
		public static bool GetForceSpeakerNametag()
		{
			return WorldConfig.GetBool("force-speaker-nametag", WorldConfig.GetBool("force-render-name-tags", true));
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00018410 File Offset: 0x00016610
		public static float GetFloat(string key, float defaultValue = 0f)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI != null && coreAPI.World.Config.HasAttribute(WorldConfig.Key(key)))
			{
				return WorldConfig.api.World.Config.GetFloat(WorldConfig.Key(key), 0f);
			}
			return defaultValue;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00018464 File Offset: 0x00016664
		public static string GetString(string key, string defaultValue = null)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI != null && coreAPI.World.Config.HasAttribute(WorldConfig.Key(key)))
			{
				return WorldConfig.api.World.Config.GetString(WorldConfig.Key(key), null);
			}
			return defaultValue;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000184B1 File Offset: 0x000166B1
		private static string Key(string key)
		{
			return "rpvoicechat:" + key;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000184BE File Offset: 0x000166BE
		// Note: this type is marked as 'beforefieldinit'.
		static WorldConfig()
		{
			Dictionary<VoiceLevel, string> dictionary = new Dictionary<VoiceLevel, string>();
			dictionary.Add(VoiceLevel.Whispering, "distance-whisper");
			dictionary.Add(VoiceLevel.Talking, "distance-talk");
			dictionary.Add(VoiceLevel.Shouting, "distance-shout");
			WorldConfig.configKeyByVoiceLevel = dictionary;
		}

		// Token: 0x04000232 RID: 562
		private const string modPrefix = "rpvoicechat";

		// Token: 0x04000233 RID: 563
		private static ICoreAPI api;

		// Token: 0x04000234 RID: 564
		private static readonly Dictionary<VoiceLevel, string> configKeyByVoiceLevel;
	}
}
