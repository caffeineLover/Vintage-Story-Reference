using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cairo;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000030 RID: 48
	[NullableContext(1)]
	[Nullable(0)]
	public static class ToolModesUtils
	{
		// Token: 0x06000347 RID: 839 RVA: 0x0000F4C0 File Offset: 0x0000D6C0
		public static SkillItem[] GetModesFromSlots(ICoreClientAPI api, IEnumerable<ItemSlot> slots, Func<ItemSlot, string> descriptionGetter)
		{
			List<SkillItem> modes = new List<SkillItem>();
			int index = 0;
			foreach (ItemSlot slot in slots)
			{
				if (slot.Itemstack != null)
				{
					List<SkillItem> list = modes;
					SkillItem skillItem = new SkillItem();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(slot.Itemstack.Collectible.Code);
					defaultInterpolatedStringHandler.AppendLiteral("-");
					defaultInterpolatedStringHandler.AppendFormatted<int>(index++);
					skillItem.Code = new AssetLocation(defaultInterpolatedStringHandler.ToStringAndClear());
					skillItem.Name = descriptionGetter.Invoke(slot);
					skillItem.Data = slot;
					list.Add(skillItem);
					List<SkillItem> list2 = modes;
					list2[list2.Count - 1].RenderHandler = ToolModesUtils.GetItemStackRenderCallback(slot, api, -1);
				}
			}
			return modes.ToArray();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000F5A4 File Offset: 0x0000D7A4
		public static RenderSkillItemDelegate GetItemStackRenderCallback(ItemSlot slot, ICoreClientAPI clientApi, int color)
		{
			return delegate(AssetLocation code, float dt, double posX, double posY)
			{
				double scaledSize = GuiElement.scaled(GuiElementPassiveItemSlot.unscaledSlotSize + GuiElementItemSlotGridBase.unscaledSlotPadding - 5.0);
				ICoreClientAPI clientApi2 = clientApi;
				if (clientApi2 == null)
				{
					return;
				}
				clientApi2.Render.RenderItemstackToGui(slot, posX + scaledSize / 2.0, posY + scaledSize / 2.0, 100.0, (float)GuiElement.scaled(GuiElementPassiveItemSlot.unscaledItemSize), color, true, false, true);
			};
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000F5CC File Offset: 0x0000D7CC
		public static SkillItem GetSkillItemWithIcon(ICoreClientAPI clientApi, string iconCode, string color)
		{
			SkillItem skillItem = new SkillItem();
			double[] colorArray = ColorUtil.Hex2Doubles(color);
			skillItem.Texture = clientApi.Gui.Icons.GenTexture(48, 48, delegate(Context ctx, ImageSurface surface)
			{
				clientApi.Gui.Icons.DrawIcon(ctx, iconCode, 5.0, 5.0, 38.0, 38.0, colorArray);
			});
			return skillItem;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000F62C File Offset: 0x0000D82C
		public static SkillItem GetSkillItemWithItemStack(ICoreClientAPI clientApi, [Nullable(2)] ItemStack stack, string icon, string color)
		{
			if (stack == null)
			{
				return ToolModesUtils.GetSkillItemWithIcon(clientApi, icon, color);
			}
			DummySlot temporarySlot = new DummySlot(stack)
			{
				BackgroundIcon = icon
			};
			return new SkillItem
			{
				Code = "",
				Name = "",
				Data = temporarySlot,
				RenderHandler = ToolModesUtils.GetItemStackRenderCallback(temporarySlot, clientApi, -1)
			};
		}
	}
}
