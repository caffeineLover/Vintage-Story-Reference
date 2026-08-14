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
	// Token: 0x02000076 RID: 118
	public class PrinterInventoryDialog : GuiDialogBlockEntityInventory
	{
		// Token: 0x06000361 RID: 865 RVA: 0x00013225 File Offset: 0x00011425
		public PrinterInventoryDialog(string dialogTitle, InventoryBase inventory, BlockPos blockEntityPos, int cols, ICoreClientAPI capi, Action onCloseCallback) : base(dialogTitle, inventory, blockEntityPos, cols, capi)
		{
			this._onCloseCallback = onCloseCallback;
			this._blockPos = blockEntityPos;
			this._title = dialogTitle;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001324C File Offset: 0x0001144C
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

		// Token: 0x06000363 RID: 867 RVA: 0x000132BC File Offset: 0x000114BC
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

		// Token: 0x06000364 RID: 868 RVA: 0x000133F2 File Offset: 0x000115F2
		public override void OnRenderGUI(float deltaTime)
		{
			base.OnRenderGUI(deltaTime);
			this.DrawSlotGhosts();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00013404 File Offset: 0x00011604
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

		// Token: 0x06000366 RID: 870 RVA: 0x000134CA File Offset: 0x000116CA
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

		// Token: 0x06000368 RID: 872 RVA: 0x000134EC File Offset: 0x000116EC
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

		// Token: 0x040001BD RID: 445
		private readonly Action _onCloseCallback;

		// Token: 0x040001BE RID: 446
		private readonly BlockPos _blockPos;

		// Token: 0x040001BF RID: 447
		private readonly string _title;

		// Token: 0x040001C0 RID: 448
		private ItemStack _ghostPaper;

		// Token: 0x040001C1 RID: 449
		private ItemStack _ghostTelegram;
	}
}
