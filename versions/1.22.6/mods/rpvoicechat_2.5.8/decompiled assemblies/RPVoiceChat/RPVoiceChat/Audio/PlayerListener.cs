using System;
using RPVoiceChat.Config;
using Vintagestory.API.Client;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000CD RID: 205
	public static class PlayerListener
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x0002A195 File Offset: 0x00028395
		public static void Init(ICoreClientAPI api)
		{
			PlayerListener.capi = api;
			PlayerListener.VoiceGain = ModConfig.ClientConfig.OutputVoice;
			PlayerListener.BlockGain = ModConfig.ClientConfig.OutputBlock;
			PlayerListener.ItemGain = ModConfig.ClientConfig.OutputItem;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002A1CA File Offset: 0x000283CA
		public static void SetVoiceGain(float gain)
		{
			if (gain == PlayerListener.VoiceGain)
			{
				return;
			}
			PlayerListener.VoiceGain = gain;
			PlayerListener.ApplyVoiceGain();
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0002A1E0 File Offset: 0x000283E0
		public static void SetBlockGain(float gain)
		{
			if (gain == PlayerListener.BlockGain)
			{
				return;
			}
			PlayerListener.BlockGain = gain;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0002A1F1 File Offset: 0x000283F1
		public static void SetItemGain(float gain)
		{
			if (gain == PlayerListener.ItemGain)
			{
				return;
			}
			PlayerListener.ItemGain = gain;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0002A204 File Offset: 0x00028404
		private static void ApplyVoiceGain()
		{
			ISettingsClass<int> @int = PlayerListener.capi.Settings.Int;
			@int["soundLevel"] = @int["soundLevel"] + 1;
			@int = PlayerListener.capi.Settings.Int;
			@int["soundLevel"] = @int["soundLevel"] - 1;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0002A264 File Offset: 0x00028464
		public static float GetGainForSource(string sourceType)
		{
			float result;
			if (!(sourceType == "voice"))
			{
				if (!(sourceType == "block"))
				{
					if (!(sourceType == "item"))
					{
						result = 1f;
					}
					else
					{
						result = PlayerListener.ItemGain;
					}
				}
				else
				{
					result = PlayerListener.BlockGain;
				}
			}
			else
			{
				result = PlayerListener.VoiceGain;
			}
			return result;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0002A2B9 File Offset: 0x000284B9
		public static void Dispose()
		{
			PlayerListener.capi = null;
			PlayerListener.VoiceGain = 1f;
			PlayerListener.BlockGain = 1f;
			PlayerListener.ItemGain = 1f;
		}

		// Token: 0x0400037D RID: 893
		public static float VoiceGain = 1f;

		// Token: 0x0400037E RID: 894
		public static float BlockGain = 1f;

		// Token: 0x0400037F RID: 895
		public static float ItemGain = 1f;

		// Token: 0x04000380 RID: 896
		private static ICoreClientAPI capi;
	}
}
