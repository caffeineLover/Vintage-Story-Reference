using System;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace RPVoiceChat.Config
{
	// Token: 0x020000AF RID: 175
	internal static class WorldConfig
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x0001E710 File Offset: 0x0001C910
		public static void Init(ICoreAPI api)
		{
			if (WorldConfig.api != null && api.Side == 2)
			{
				return;
			}
			WorldConfig.api = api;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001E72C File Offset: 0x0001C92C
		public static void Set(VoiceLevel voiceLevel, int distance)
		{
			string key;
			if (WorldConfig.configKeyByVoiceLevel.TryGetValue(voiceLevel, ref key))
			{
				WorldConfig.Set(key, distance);
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001E74F File Offset: 0x0001C94F
		public static void Set(string key, int value)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI == null)
			{
				return;
			}
			coreAPI.World.Config.SetInt(WorldConfig.Key(key), value);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001E771 File Offset: 0x0001C971
		public static void Set(string key, bool value)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI == null)
			{
				return;
			}
			coreAPI.World.Config.SetBool(WorldConfig.Key(key), value);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001E793 File Offset: 0x0001C993
		public static void Set(string key, float value)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI == null)
			{
				return;
			}
			coreAPI.World.Config.SetFloat(WorldConfig.Key(key), value);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001E7B5 File Offset: 0x0001C9B5
		public static void Set(string key, string value)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI == null)
			{
				return;
			}
			coreAPI.World.Config.SetString(WorldConfig.Key(key), value);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001E7D8 File Offset: 0x0001C9D8
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

		// Token: 0x06000597 RID: 1431 RVA: 0x0001E828 File Offset: 0x0001CA28
		public static int GetInt(string key, int defaultValue = 0)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI != null && coreAPI.World.Config.HasAttribute(WorldConfig.Key(key)))
			{
				return WorldConfig.api.World.Config.GetInt(WorldConfig.Key(key), 0);
			}
			return defaultValue;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001E878 File Offset: 0x0001CA78
		public static bool GetBool(string key, bool defaultValue = false)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI != null && coreAPI.World.Config.HasAttribute(WorldConfig.Key(key)))
			{
				return WorldConfig.api.World.Config.GetBool(WorldConfig.Key(key), false);
			}
			return defaultValue;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001E8C5 File Offset: 0x0001CAC5
		public static bool GetPlayerNametagTargetedOnly()
		{
			return WorldConfig.GetBool("player-nametag-targeted-only", true);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001E8D2 File Offset: 0x0001CAD2
		public static bool GetForceSpeakerNametag()
		{
			return WorldConfig.GetBool("force-speaker-nametag", WorldConfig.GetBool("force-render-name-tags", true));
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001E8EC File Offset: 0x0001CAEC
		public static float GetFloat(string key, float defaultValue = 0f)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI != null && coreAPI.World.Config.HasAttribute(WorldConfig.Key(key)))
			{
				return WorldConfig.api.World.Config.GetFloat(WorldConfig.Key(key), 0f);
			}
			return defaultValue;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001E940 File Offset: 0x0001CB40
		public static string GetString(string key, string defaultValue = null)
		{
			ICoreAPI coreAPI = WorldConfig.api;
			if (coreAPI != null && coreAPI.World.Config.HasAttribute(WorldConfig.Key(key)))
			{
				return WorldConfig.api.World.Config.GetString(WorldConfig.Key(key), null);
			}
			return defaultValue;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001E98D File Offset: 0x0001CB8D
		private static string Key(string key)
		{
			return "rpvoicechat:" + key;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001E99A File Offset: 0x0001CB9A
		// Note: this type is marked as 'beforefieldinit'.
		static WorldConfig()
		{
			Dictionary<VoiceLevel, string> dictionary = new Dictionary<VoiceLevel, string>();
			dictionary.Add(VoiceLevel.Whispering, "distance-whisper");
			dictionary.Add(VoiceLevel.Talking, "distance-talk");
			dictionary.Add(VoiceLevel.Shouting, "distance-shout");
			WorldConfig.configKeyByVoiceLevel = dictionary;
		}

		// Token: 0x040002D4 RID: 724
		private const string modPrefix = "rpvoicechat";

		// Token: 0x040002D5 RID: 725
		private static ICoreAPI api;

		// Token: 0x040002D6 RID: 726
		private static readonly Dictionary<VoiceLevel, string> configKeyByVoiceLevel;
	}
}
