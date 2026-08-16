using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cairo;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200014C RID: 332
	[NullableContext(1)]
	[Nullable(0)]
	public class ToolSelectionGuiDialog : GuiDialog
	{
		// Token: 0x0600106D RID: 4205 RVA: 0x00050E51 File Offset: 0x0004F051
		public ToolSelectionGuiDialog(ICoreClientAPI api, ToolBagSelectionSystemClient system) : base(api)
		{
			this.Api = api;
			this.System = system;
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x00050E68 File Offset: 0x0004F068
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x00050E6F File Offset: 0x0004F06F
		public override double DrawOrder
		{
			get
			{
				return 0.21;
			}
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00050E7C File Offset: 0x0004F07C
		public override void OnGuiOpened()
		{
			try
			{
				bool successful = this.ComposeDialog();
			}
			catch (Exception exception)
			{
				ICoreAPI api = this.Api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(45, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error while composing tool selection dialog:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00050EDC File Offset: 0x0004F0DC
		protected virtual bool ComposeDialog()
		{
			base.ClearComposers();
			IEnumerable<ToolSlotData> slots = this.System.GetSlotsForToolDialog();
			List<SkillItem> skillItems = Enumerable.ToList<SkillItem>(Enumerable.Select<ToolSlotData, SkillItem>(slots, (ToolSlotData config) => this.GetSkillItem(config.Stack, config.Icon, config.Color)));
			if (skillItems.Count == 0)
			{
				return false;
			}
			ElementBounds mainBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(7).WithFixedAlignmentOffset(0.0, -190.0);
			double slotSize = GuiElementPassiveItemSlot.unscaledSlotSize + GuiElementItemSlotGridBase.unscaledSlotPadding;
			ElementBounds skillGridBounds = ElementBounds.Fixed(20.0, 20.0, (double)skillItems.Count * slotSize, slotSize);
			mainBounds = mainBounds.WithChild(skillGridBounds);
			base.SingleComposer = this.Api.Gui.CreateCompo("combatoverhaul:toolselection", mainBounds);
			GuiComposerHelpers.AddShadedDialogBG(base.SingleComposer, ElementStdBounds.DialogBackground().WithFixedPadding(GuiStyle.ElementToDialogPadding / 2.0), false, 5.0, 0.75f);
			base.SingleComposer.BeginChildElements();
			this.ComposeSkillItemRow(slots, "all", skillGridBounds);
			base.SingleComposer.EndChildElements();
			base.SingleComposer.Compose(true);
			return true;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00050FFC File Offset: 0x0004F1FC
		protected void ComposeSkillItemRow(IEnumerable<ToolSlotData> slots, string code, ElementBounds bounds)
		{
			List<SkillItem> skillItems = new List<SkillItem>();
			List<ToolSlotData> slotsData = new List<ToolSlotData>();
			foreach (ToolSlotData data in slots)
			{
				skillItems.Add(this.GetSkillItem(data.Stack, data.Icon, data.Color));
				slotsData.Add(data);
			}
			GuiComposerHelpers.AddSkillItemGrid(base.SingleComposer, skillItems, skillItems.Count, 1, delegate(int index)
			{
				this.OnSkillItemPressed(slotsData[index]);
			}, bounds, "skill-item-grid-" + code);
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x000510B4 File Offset: 0x0004F2B4
		protected void OnSkillItemPressed(ToolSlotData data)
		{
			this.System.TriggerSlots(new <>z__ReadOnlySingleElementList<ToolSlotData>(data));
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x000510C8 File Offset: 0x0004F2C8
		protected SkillItem GetSkillItem([Nullable(2)] ItemStack stack, string icon, string color)
		{
			if (stack == null)
			{
				return ToolSelectionGuiDialog.GetSkillItemWithIcon(this.Api, icon, color);
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
				RenderHandler = ToolSelectionGuiDialog.GetItemStackRenderCallback(temporarySlot, this.Api, -1)
			};
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0005112E File Offset: 0x0004F32E
		protected static RenderSkillItemDelegate GetItemStackRenderCallback(ItemSlot slot, ICoreClientAPI clientApi, int color)
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

		// Token: 0x06001076 RID: 4214 RVA: 0x00051158 File Offset: 0x0004F358
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

		// Token: 0x040007A4 RID: 1956
		private const double OverlayDrawOrder = 0.21;

		// Token: 0x040007A5 RID: 1957
		private const double BottomOffset = -190.0;

		// Token: 0x040007A6 RID: 1958
		protected readonly ICoreClientAPI Api;

		// Token: 0x040007A7 RID: 1959
		protected readonly ToolBagSelectionSystemClient System;
	}
}
