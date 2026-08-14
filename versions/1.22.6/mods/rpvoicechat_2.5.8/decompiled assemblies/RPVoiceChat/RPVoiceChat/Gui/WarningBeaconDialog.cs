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
	// Token: 0x0200007B RID: 123
	public class WarningBeaconDialog : GuiDialogBlockEntityInventory
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060003AC RID: 940 RVA: 0x000159EF File Offset: 0x00013BEF
		public override string ToggleKeyCombinationCode
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000159F2 File Offset: 0x00013BF2
		public WarningBeaconDialog(string title, InventoryLucerne inventory, BlockPos blockPos, ICoreClientAPI capi, BlockEntityLucerne be, Action onClose) : base(title, inventory, blockPos, 2, capi)
		{
			this._title = title;
			this._blockPos = blockPos;
			this._be = be;
			this._onClose = onClose;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00015A20 File Offset: 0x00013C20
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

		// Token: 0x060003AF RID: 943 RVA: 0x00015A90 File Offset: 0x00013C90
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

		// Token: 0x060003B0 RID: 944 RVA: 0x00015CD4 File Offset: 0x00013ED4
		private void OnLightClicked()
		{
			this.capi.Network.SendBlockEntityPacket(this._blockPos, BlockEntityLucerne.PacketIdLightBeacon, Array.Empty<byte>());
			this.TryClose();
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00015CFD File Offset: 0x00013EFD
		public override void OnRenderGUI(float deltaTime)
		{
			base.OnRenderGUI(deltaTime);
			this.UpdateState();
			this.DrawSlotGhosts();
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00015D14 File Offset: 0x00013F14
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

		// Token: 0x060003B3 RID: 947 RVA: 0x00015DFC File Offset: 0x00013FFC
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

		// Token: 0x060003B4 RID: 948 RVA: 0x00015F8F File Offset: 0x0001418F
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

		// Token: 0x060003B5 RID: 949 RVA: 0x00015FB8 File Offset: 0x000141B8
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

		// Token: 0x060003B6 RID: 950 RVA: 0x00016051 File Offset: 0x00014251
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

		// Token: 0x040001DF RID: 479
		private readonly string _title;

		// Token: 0x040001E0 RID: 480
		private readonly BlockPos _blockPos;

		// Token: 0x040001E1 RID: 481
		private readonly BlockEntityLucerne _be;

		// Token: 0x040001E2 RID: 482
		private readonly Action _onClose;

		// Token: 0x040001E3 RID: 483
		private GuiElement _lightButton;

		// Token: 0x040001E4 RID: 484
		private LinearGauge _burnGauge;

		// Token: 0x040001E5 RID: 485
		private ItemStack _ghostFat;

		// Token: 0x040001E6 RID: 486
		private ItemStack _ghostFirewood;
	}
}
