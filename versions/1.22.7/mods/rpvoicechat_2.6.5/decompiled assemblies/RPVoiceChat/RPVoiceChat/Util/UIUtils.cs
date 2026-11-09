using System;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace RPVoiceChat.Util
{
	// Token: 0x0200002C RID: 44
	public static class UIUtils
	{
		// Token: 0x06000112 RID: 274 RVA: 0x0000886C File Offset: 0x00006A6C
		public static string I18n(string key)
		{
			return Lang.Get(RPVoiceChatMod.modID + ":" + key, Array.Empty<object>());
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00008888 File Offset: 0x00006A88
		public static string I18n(string key, params object[] args)
		{
			return Lang.Get(RPVoiceChatMod.modID + ":" + key, args);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000088A0 File Offset: 0x00006AA0
		public static string TrimText(string text, double width, CairoFont font)
		{
			string suffix = "...";
			double suffixWidth = font.GetTextExtents(suffix).Width;
			double targetWidth = width - suffixWidth;
			double textWidth = font.GetTextExtents(text).Width;
			if (textWidth <= width)
			{
				return text;
			}
			int cutLocation = (int)(Math.Min(targetWidth / textWidth, 1.0) * (double)text.Length);
			return text.Substring(0, cutLocation) + suffix;
		}
	}
}
