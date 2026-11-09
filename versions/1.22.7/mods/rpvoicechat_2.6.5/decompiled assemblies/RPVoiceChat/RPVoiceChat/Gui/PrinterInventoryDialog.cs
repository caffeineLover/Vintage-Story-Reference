using System;
using System.IO;
using System.Runtime.CompilerServices;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Inventory;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Gui
{
	// Token: 0x02000099 RID: 153
	public class PrinterInventoryDialog : GuiDialogBlockEntityInventory
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x000174DD File Offset: 0x000156DD
		public PrinterInventoryDialog(string dialogTitle, InventoryBase inventory, BlockPos blockEntityPos, int cols, ICoreClientAPI capi, Action onCloseCallback) : base(dialogTitle, inventory, blockEntityPos, cols, capi)
		{
			this._onCloseCallback = onCloseCallback;
			this._blockPos = blockEntityPos;
			this._title = dialogTitle;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00017504 File Offset: 0x00015704
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
			this.capi.Network.SendBlockEntityPacket(this._blockPos, BlockEntityPrinter.PacketIdPrinterInventory, data);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00017574 File Offset: 0x00015774
		public override void OnGuiOpened()
		{
			ElementBounds dialogBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(6);
			int cols = 5;
			int rows = 2;
			ElementBounds slotGridBounds = ElementBounds.Fixed(0.0, 40.0, (double)(cols * 48), (double)(rows * 48));
			ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
			bgBounds.BothSizing = 2;
			bgBounds.WithChildren(new ElementBounds[]
			{
				slotGridBounds
			});
			base.SingleComposer = GuiComposerHelpers.AddItemSlotGrid(GuiComposerHelpers.AddDialogTitleBar(GuiComposerHelpers.AddShadedDialogBG(this.capi.Gui.CreateCompo("printerinv", dialogBounds), bgBounds, true, 5.0, 0.75f), this._title, delegate()
			{
				this.TryClose();
			}, null, null, null), base.Inventory, new Action<object>(this.SendInvPacket), cols, slotGridBounds, "slots").Compose(true);
			IClientWorldAccessor world = this.capi.World;
			Item paper = world.GetItem(new AssetLocation(RPVoiceChatMod.modID, "paperslip"));
			Item telegram = world.GetItem(new AssetLocation(RPVoiceChatMod.modID, "telegram"));
			if (paper != null)
			{
				this._ghostPaper = new ItemStack(paper, 1);
			}
			if (telegram != null)
			{
				this._ghostTelegram = new ItemStack(telegram, 1);
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000176AA File Offset: 0x000158AA
		public override void OnRenderGUI(float deltaTime)
		{
			base.OnRenderGUI(deltaTime);
			this.DrawSlotGhosts();
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x000176BC File Offset: 0x000158BC
		private void DrawSlotGhosts()
		{
			PrinterInventoryDialog.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			InventoryPrinter inv = base.Inventory as InventoryPrinter;
			if (inv == null || base.SingleComposer == null)
			{
				return;
			}
			GuiElement slotEl = base.SingleComposer.GetElement("slots");
			if (((slotEl != null) ? slotEl.Bounds : null) == null)
			{
				return;
			}
			CS$<>8__locals1.gx = slotEl.Bounds.renderX;
			CS$<>8__locals1.gy = slotEl.Bounds.renderY;
			this.<DrawSlotGhosts>g__DrawSlot|9_0(inv.PaperSlot, this._ghostPaper, 0, 12f, 13f, ref CS$<>8__locals1);
			for (int i = 0; i < inv.TelegramSlots.Length; i++)
			{
				this.<DrawSlotGhosts>g__DrawSlot|9_0(inv.TelegramSlots[i], this._ghostTelegram, i + 1, 13f, 13f, ref CS$<>8__locals1);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00017782 File Offset: 0x00015982
		public override void OnGuiClosed()
		{
			base.OnGuiClosed();
			Action onCloseCallback = this._onCloseCallback;
			if (onCloseCallback == null)
			{
				return;
			}
			onCloseCallback.Invoke();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x000177A4 File Offset: 0x000159A4
		[CompilerGenerated]
		private void <DrawSlotGhosts>g__DrawSlot|9_0(ItemSlot slot, ItemStack ghost, int slotIndex, float ox = 0f, float oy = 0f, ref PrinterInventoryDialog.<>c__DisplayClass9_0 A_6)
		{
			if (ghost == null)
			{
				return;
			}
			int col = slotIndex % 5;
			int row = slotIndex / 5;
			double rectX = A_6.gx + (double)col * 48.0;
			double rectY = A_6.gy + (double)row * 48.0;
			float oxTotal = ox + (float)col * 3f;
			AllowedItemGhostDraw.DrawInRect(this.capi, slot, ghost, rectX, rectY, 48.0, 48.0, 24f, 1442840575, oxTotal, oy);
		}

		// Token: 0x04000237 RID: 567
		private readonly Action _onCloseCallback;

		// Token: 0x04000238 RID: 568
		private readonly BlockPos _blockPos;

		// Token: 0x04000239 RID: 569
		private readonly string _title;

		// Token: 0x0400023A RID: 570
		private ItemStack _ghostPaper;

		// Token: 0x0400023B RID: 571
		private ItemStack _ghostTelegram;
	}
}
