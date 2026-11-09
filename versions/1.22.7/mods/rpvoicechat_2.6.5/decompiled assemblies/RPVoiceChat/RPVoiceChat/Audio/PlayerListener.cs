using System;
using RPVoiceChat.Config;
using Vintagestory.API.Client;

namespace RPVoiceChat.Audio
{
	// Token: 0x02000106 RID: 262
	public static class PlayerListener
	{
		// Token: 0x060009B5 RID: 2485 RVA: 0x00035E25 File Offset: 0x00034025
		public static void Init(ICoreClientAPI api)
		{
			PlayerListener.capi = api;
			PlayerListener.VoiceGain = ModConfig.ClientConfig.OutputVoice;
			PlayerListener.BlockGain = ModConfig.ClientConfig.OutputBlock;
			PlayerListener.ItemGain = ModConfig.ClientConfig.OutputItem;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00035E5A File Offset: 0x0003405A
		public static void SetVoiceGain(float gain)
		{
			if (gain == PlayerListener.VoiceGain)
			{
				return;
			}
			PlayerListener.VoiceGain = gain;
			PlayerListener.ApplyVoiceGain();
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00035E70 File Offset: 0x00034070
		public static void SetBlockGain(float gain)
		{
			if (gain == PlayerListener.BlockGain)
			{
				return;
			}
			PlayerListener.BlockGain = gain;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00035E81 File Offset: 0x00034081
		public static void SetItemGain(float gain)
		{
			if (gain == PlayerListener.ItemGain)
			{
				return;
			}
			PlayerListener.ItemGain = gain;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x00035E94 File Offset: 0x00034094
		private static void ApplyVoiceGain()
		{
			ISettingsClass<int> @int = PlayerListener.capi.Settings.Int;
			@int["soundLevel"] = @int["soundLevel"] + 1;
			@int = PlayerListener.capi.Settings.Int;
			@int["soundLevel"] = @int["soundLevel"] - 1;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00035EF4 File Offset: 0x000340F4
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

		// Token: 0x060009BB RID: 2491 RVA: 0x00035F49 File Offset: 0x00034149
		public static void Dispose()
		{
			PlayerListener.capi = null;
			PlayerListener.VoiceGain = 1f;
			PlayerListener.BlockGain = 1f;
			PlayerListener.ItemGain = 1f;
		}

		// Token: 0x04000465 RID: 1125
		public static float VoiceGain = 1f;

		// Token: 0x04000466 RID: 1126
		public static float BlockGain = 1f;

		// Token: 0x04000467 RID: 1127
		public static float ItemGain = 1f;

		// Token: 0x04000468 RID: 1128
		private static ICoreClientAPI capi;
	}
}
