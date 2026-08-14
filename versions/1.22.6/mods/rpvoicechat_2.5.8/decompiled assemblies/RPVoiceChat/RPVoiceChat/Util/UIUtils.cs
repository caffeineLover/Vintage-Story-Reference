using System;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace RPVoiceChat.Util
{
	// Token: 0x0200002B RID: 43
	public static class UIUtils
	{
		// Token: 0x06000106 RID: 262 RVA: 0x00007E4F File Offset: 0x0000604F
		public static string I18n(string key)
		{
			return Lang.Get(RPVoiceChatMod.modID + ":" + key, Array.Empty<object>());
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00007E6B File Offset: 0x0000606B
		public static string I18n(string key, params object[] args)
		{
			return Lang.Get(RPVoiceChatMod.modID + ":" + key, args);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00007E84 File Offset: 0x00006084
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
