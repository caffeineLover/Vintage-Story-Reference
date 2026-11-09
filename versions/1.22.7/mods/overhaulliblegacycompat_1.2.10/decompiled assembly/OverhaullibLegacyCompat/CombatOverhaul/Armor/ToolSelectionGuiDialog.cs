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
	// Token: 0x02000158 RID: 344
	[NullableContext(1)]
	[Nullable(0)]
	public class ToolSelectionGuiDialog : GuiDialog
	{
		// Token: 0x060010E1 RID: 4321 RVA: 0x00052759 File Offset: 0x00050959
		public ToolSelectionGuiDialog(ICoreClientAPI api, ToolBagSelectionSystemClient system) : base(api)
		{
			this.Api = api;
			this.System = system;
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x00052770 File Offset: 0x00050970
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x00052777 File Offset: 0x00050977
		public override double DrawOrder
		{
			get
			{
				return 0.21;
			}
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00052784 File Offset: 0x00050984
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

		// Token: 0x060010E5 RID: 4325 RVA: 0x000527E4 File Offset: 0x000509E4
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

		// Token: 0x060010E6 RID: 4326 RVA: 0x00052904 File Offset: 0x00050B04
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

		// Token: 0x060010E7 RID: 4327 RVA: 0x000529BC File Offset: 0x00050BBC
		protected void OnSkillItemPressed(ToolSlotData data)
		{
			this.System.TriggerSlots(new <>z__ReadOnlySingleElementList<ToolSlotData>(data));
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x000529D0 File Offset: 0x00050BD0
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

		// Token: 0x060010E9 RID: 4329 RVA: 0x00052A36 File Offset: 0x00050C36
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

		// Token: 0x060010EA RID: 4330 RVA: 0x00052A60 File Offset: 0x00050C60
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

		// Token: 0x040007CA RID: 1994
		private const double OverlayDrawOrder = 0.21;

		// Token: 0x040007CB RID: 1995
		private const double BottomOffset = -190.0;

		// Token: 0x040007CC RID: 1996
		protected readonly ICoreClientAPI Api;

		// Token: 0x040007CD RID: 1997
		protected readonly ToolBagSelectionSystemClient System;
	}
}
