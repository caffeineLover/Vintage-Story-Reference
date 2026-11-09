using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x0200008B RID: 139
	public static class AllowedItemGhostDraw
	{
		// Token: 0x06000393 RID: 915 RVA: 0x00013310 File Offset: 0x00011510
		public static void DrawInRect(ICoreClientAPI capi, ItemSlot watchSlot, ItemStack ghostStack, double rectX, double rectY, double rectW, double rectH, float size = 24f, int colorArgb = 1442840575, float offsetX = 0f, float offsetY = 0f)
		{
			if (ghostStack == null || watchSlot == null || !watchSlot.Empty)
			{
				return;
			}
			double x = rectX + (rectW - (double)size) / 2.0 + (double)offsetX;
			double y = rectY + (rectH - (double)size) / 2.0 + (double)offsetY;
			capi.Render.RenderItemstackToGui(ghostStack, x, y, 100.0, size, colorArgb, false, false, false);
		}

		// Token: 0x040001BF RID: 447
		public const int DefaultGhostColor = 1442840575;

		// Token: 0x040001C0 RID: 448
		public const float SlotContentSize = 48f;
	}
}
