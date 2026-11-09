using System;
using System.IO;
using System.Reflection;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Inventory;
using RPVoiceChat.Gui.CustomElements;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Gui
{
	// Token: 0x020000A4 RID: 164
	public class WarningBeaconDialog : GuiDialogBlockEntityInventory
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0001BA83 File Offset: 0x00019C83
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001BA86 File Offset: 0x00019C86
		public WarningBeaconDialog(string title, InventoryLucerne inventory, BlockPos blockPos, ICoreClientAPI capi, BlockEntityLucerne be, Action onClose) : base(title, inventory, blockPos, 2, capi)
		{
			this._title = title;
			this._blockPos = blockPos;
			this._be = be;
			this._onClose = onClose;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001BAB4 File Offset: 0x00019CB4
		private void SendInvPacket(object packet)
		{
			byte[] data;
			using (MemoryStream ms = new MemoryStream())
			{
				TreeAttribute tree = new TreeAttribute();
				base.Inventory.ToTreeAttributes(tree);
				tree.ToBytes(new BinaryWriter(ms));
				data = ms.ToArray();
			}
			this.capi.Network.SendBlockEntityPacket(this._blockPos, BlockEntityLucerne.PacketIdInventory, data);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001BB24 File Offset: 0x00019D24
		public override void OnGuiOpened()
		{
			base.OnGuiOpened();
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			ElementBounds slotGridBounds = ElementBounds.Fixed(0.0, 40.0, 160.0, 70.0);
			ElementBounds progressBounds = ElementBounds.Fixed(0.0, 112.0, 300.0, 25.0);
			ElementBounds buttonBounds = ElementBounds.Fixed(0.0, 147.0, 200.0, 35.0);
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			bgBounds.WithChildren(new ElementBounds[]
			{
				slotGridBounds,
				progressBounds,
				buttonBounds
			});
			this._burnGauge = new LinearGauge(this.capi, progressBounds, false);
			base.SingleComposer = GuiComposerHelpers.AddButton(GuiElementDynamicTextHelper.AddDynamicText(GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("warningbeacon", dialogBounds), bgBounds, true, 5.0, 0.75f), this._title, delegate()
			{
				this.TryClose();
			}, null, null, null), base.Inventory, new Action<object>(this.SendInvPacket), 2, slotGridBounds, "slots").AddInteractiveElement(this._burnGauge.Element, "progressBar"), "", CairoFont.WhiteSmallText().WithFontSize(14f).WithOrientation(2), progressBounds.FlatCopy(), "hoursLabel"), Lang.Get(RPVoiceChatMod.modID + ":WarningBeacon.LightButton", Array.Empty<object>()) ?? "Allumer", delegate()
			{
				this.OnLightClicked();
				return true;
			}, buttonBounds, 2, "lightButton").Compose(true);
			this._lightButton = GuiComposerHelpers.GetButton(base.SingleComposer, "lightButton");
			IClientWorldAccessor world = this.capi.World;
			Item fat = world.GetItem(new AssetLocation("game", "fat"));
			Item firewood = world.GetItem(new AssetLocation("game", "firewood"));
			if (fat != null)
			{
				this._ghostFat = new ItemStack(fat, 1);
			}
			if (firewood != null)
			{
				this._ghostFirewood = new ItemStack(firewood, 1);
			}
			this.UpdateState();
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001BD68 File Offset: 0x00019F68
		private void OnLightClicked()
		{
			this.capi.Network.SendBlockEntityPacket(this._blockPos, BlockEntityLucerne.PacketIdLightBeacon, Array.Empty<byte>());
			this.TryClose();
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001BD91 File Offset: 0x00019F91
		public override void OnRenderGUI(float deltaTime)
		{
			base.OnRenderGUI(deltaTime);
			this.UpdateState();
			this.DrawSlotGhosts();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001BDA8 File Offset: 0x00019FA8
		private void DrawSlotGhosts()
		{
			InventoryLucerne inv = base.Inventory as InventoryLucerne;
			if (inv == null || base.SingleComposer == null)
			{
				return;
			}
			GuiElement slotEl = base.SingleComposer.GetElement("slots");
			if (((slotEl != null) ? slotEl.Bounds : null) == null)
			{
				return;
			}
			double gx = slotEl.Bounds.renderX;
			double gy = slotEl.Bounds.renderY;
			double cellW = slotEl.Bounds.OuterWidth / 2.0;
			double cellH = slotEl.Bounds.OuterHeight;
			AllowedItemGhostDraw.DrawInRect(this.capi, inv.FatSlot, this._ghostFat, gx, gy, cellW, cellH, 24f, 1442840575, -1f, 0f);
			AllowedItemGhostDraw.DrawInRect(this.capi, inv.FirewoodSlot, this._ghostFirewood, gx + cellW, gy, cellW, cellH, 24f, 1442840575, -32f, 0f);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001BE90 File Offset: 0x0001A090
		private void UpdateState()
		{
			BlockEntityLucerne be = this.capi.World.BlockAccessor.GetBlockEntity(this._blockPos) as BlockEntityLucerne;
			if (be == null || this._lightButton == null)
			{
				return;
			}
			double remainingHours = be.BurnTimeRemainingGameHours;
			double fillRatio = be.IsBurning ? Math.Max(0.0, Math.Min(1.0, remainingHours / 24.0)) : 0.0;
			LinearGauge burnGauge = this._burnGauge;
			if (burnGauge != null)
			{
				burnGauge.SetRatio01((float)fillRatio);
			}
			GuiComposer singleComposer = base.SingleComposer;
			GuiElementDynamicText hoursLabel = (singleComposer != null) ? GuiElementDynamicTextHelper.GetDynamicText(singleComposer, "hoursLabel") : null;
			if (hoursLabel != null)
			{
				if (be.IsBurning)
				{
					hoursLabel.SetNewText(Lang.Get(RPVoiceChatMod.modID + ":WarningBeacon.HoursRemaining", new object[]
					{
						Math.Max(0, (int)Math.Ceiling(remainingHours))
					}), false, false, false);
				}
				else
				{
					hoursLabel.SetNewText("", false, false, false);
				}
			}
			bool canLight = !be.IsBurning && ((InventoryLucerne)base.Inventory).FatSlot.StackSize >= 2 && ((InventoryLucerne)base.Inventory).FirewoodSlot.StackSize >= 32;
			WarningBeaconDialog.SetButtonEnabled(this._lightButton, canLight);
			string btnText = be.IsBurning ? Lang.Get(RPVoiceChatMod.modID + ":WarningBeacon.BurningButton", Array.Empty<object>()) : Lang.Get(RPVoiceChatMod.modID + ":WarningBeacon.LightButton", Array.Empty<object>());
			WarningBeaconDialog.SetButtonText(this._lightButton, btnText);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001C023 File Offset: 0x0001A223
		private static void SetButtonEnabled(GuiElement button, bool enabled)
		{
			if (button == null)
			{
				return;
			}
			PropertyInfo property = button.GetType().GetProperty("Enabled", 20);
			if (property == null)
			{
				return;
			}
			property.SetValue(button, enabled);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001C04C File Offset: 0x0001A24C
		private static void SetButtonText(GuiElement button, string text)
		{
			if (button == null)
			{
				return;
			}
			Type t = button.GetType();
			MethodInfo setNewText = t.GetMethod("SetNewText", new Type[]
			{
				typeof(string)
			});
			if (setNewText != null)
			{
				setNewText.Invoke(button, new object[]
				{
					text ?? ""
				});
				return;
			}
			MethodInfo method = t.GetMethod("SetText", new Type[]
			{
				typeof(string)
			});
			if (method == null)
			{
				return;
			}
			method.Invoke(button, new object[]
			{
				text ?? ""
			});
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001C0E5 File Offset: 0x0001A2E5
		public override void OnGuiClosed()
		{
			base.OnGuiClosed();
			BlockEntityLucerne be = this._be;
			if (be != null)
			{
				be.RefreshBonfireRenderer();
			}
			Action onClose = this._onClose;
			if (onClose == null)
			{
				return;
			}
			onClose.Invoke();
		}

		// Token: 0x0400027A RID: 634
		private readonly string _title;

		// Token: 0x0400027B RID: 635
		private readonly BlockPos _blockPos;

		// Token: 0x0400027C RID: 636
		private readonly BlockEntityLucerne _be;

		// Token: 0x0400027D RID: 637
		private readonly Action _onClose;

		// Token: 0x0400027E RID: 638
		private GuiElement _lightButton;

		// Token: 0x0400027F RID: 639
		private LinearGauge _burnGauge;

		// Token: 0x04000280 RID: 640
		private ItemStack _ghostFat;

		// Token: 0x04000281 RID: 641
		private ItemStack _ghostFirewood;
	}
}
