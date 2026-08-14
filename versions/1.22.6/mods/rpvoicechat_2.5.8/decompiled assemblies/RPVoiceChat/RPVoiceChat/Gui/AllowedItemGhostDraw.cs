using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000068 RID: 104
	public static class AllowedItemGhostDraw
	{
		// Token: 0x060002DE RID: 734 RVA: 0x0000F768 File Offset: 0x0000D968
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

		// Token: 0x04000153 RID: 339
		public const int DefaultGhostColor = 1442840575;

		// Token: 0x04000154 RID: 340
		public const float SlotContentSize = 48f;
	}
}
