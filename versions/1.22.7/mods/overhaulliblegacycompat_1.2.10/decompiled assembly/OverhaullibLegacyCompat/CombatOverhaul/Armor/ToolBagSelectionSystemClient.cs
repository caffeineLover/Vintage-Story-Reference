using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000154 RID: 340
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToolBagSelectionSystemClient
	{
		// Token: 0x060010B9 RID: 4281 RVA: 0x00051AF4 File Offset: 0x0004FCF4
		public ToolBagSelectionSystemClient(ICoreClientAPI api, ToolBagSystemClient toolBagSystem)
		{
			this._api = api;
			this._toolBagSystem = toolBagSystem;
			this._dialog = new ToolSelectionGuiDialog(api, this);
			try
			{
				this._api.Input.RegisterHotKeyFirst("combatoverhaul:tool-selection", Lang.Get("combatoverhaul:tool-selection-hotkey", Array.Empty<object>()), 88, 4, false, false, false);
			}
			catch
			{
				LoggerUtil.Notify(this._api, this, "Hotkey 'combatoverhaul:tool-selection' was already registered.");
			}
			this._api.Input.SetHotKeyHandler("combatoverhaul:tool-selection", new ActionConsumable<KeyCombination>(this.OnHotkeyPress));
			this._api.Event.RegisterCallback(delegate(float _)
			{
				this.MoveHotkeyToStart();
			}, 1000);
			GuiDialogToolMode toolModeDialog = Enumerable.FirstOrDefault<GuiDialogToolMode>(Enumerable.OfType<GuiDialogToolMode>(this._api.Gui.LoadedGuis));
			if (toolModeDialog != null)
			{
				toolModeDialog.OnClosed += delegate()
				{
					this._dialog.TryClose();
				};
			}
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00051BE8 File Offset: 0x0004FDE8
		public IEnumerable<ToolSlotData> GetToolSlots()
		{
			IInventory inventory = ToolBagSelectionSystemClient.GetBackpackInventory(this._api.World.Player);
			if (inventory == null)
			{
				return Array.Empty<ToolSlotData>();
			}
			return Enumerable.Select<ItemSlotBagContentWithWildcardMatch, ToolSlotData>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.OfType<ItemSlotBagContentWithWildcardMatch>(inventory), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.HandleHotkey || slot.Config.DisplayInToolDialog), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.BackpackCategoryCode != "ammunition"), (ItemSlotBagContentWithWildcardMatch slot) => new ToolSlotData(slot.Config, slot.Itemstack, slot.ToolBagId, slot.ToolBagIndex, slot.MainHand, slot.BackgroundIcon, slot.HexBackgroundColor ?? "", slot.SlotIndex));
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00051C88 File Offset: 0x0004FE88
		public IEnumerable<ToolSlotData> GetSlotsForToolDialog()
		{
			IInventory inventory = ToolBagSelectionSystemClient.GetBackpackInventory(this._api.World.Player);
			if (inventory == null)
			{
				return Array.Empty<ToolSlotData>();
			}
			return Enumerable.Select<ItemSlotBagContentWithWildcardMatch, ToolSlotData>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.OfType<ItemSlotBagContentWithWildcardMatch>(inventory), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.DisplayInToolDialog || slot.Config.HandleHotkey), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.BackpackCategoryCode != "ammunition"), (ItemSlotBagContentWithWildcardMatch slot) => new ToolSlotData(slot.Config, slot.Itemstack, slot.ToolBagId, slot.ToolBagIndex, slot.MainHand, slot.BackgroundIcon, slot.HexBackgroundColor ?? "", slot.SlotIndex));
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00051D28 File Offset: 0x0004FF28
		public void TriggerSlots(IEnumerable<ToolSlotData> slots)
		{
			foreach (ToolSlotData slotData in slots)
			{
				this._toolBagSystem.Send(slotData.ToolBagId, slotData.ToolBagIndex, slotData.MainHand, slotData.SlotIndex);
			}
			GuiDialogToolMode guiDialogToolMode = Enumerable.FirstOrDefault<GuiDialogToolMode>(Enumerable.OfType<GuiDialogToolMode>(this._api.Gui.LoadedGuis));
			if (guiDialogToolMode != null)
			{
				guiDialogToolMode.TryClose();
			}
			this._dialog.TryClose();
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x00051DC0 File Offset: 0x0004FFC0
		private void MoveHotkeyToStart()
		{
			HotKey hotkey;
			if (!this._api.Input.HotKeys.TryGetValue("combatoverhaul:tool-selection", ref hotkey))
			{
				return;
			}
			this._api.Input.HotKeys.Remove("combatoverhaul:tool-selection");
			this._api.Input.HotKeys.Insert(0, "combatoverhaul:tool-selection", hotkey);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00051E24 File Offset: 0x00050024
		private bool OnHotkeyPress(KeyCombination combination)
		{
			if (this._dialog.IsOpened())
			{
				this._dialog.TryClose();
				this.SuppressOpenAfterClose();
				return false;
			}
			if (this._suppressOpenAfterClose || !Enumerable.Any<ToolSlotData>(this.GetSlotsForToolDialog()))
			{
				return false;
			}
			this.QueueDialogOpen();
			return false;
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x00051E70 File Offset: 0x00050070
		private void SuppressOpenAfterClose()
		{
			if (this._suppressOpenAfterClose)
			{
				return;
			}
			this._suppressOpenAfterClose = true;
			this._api.Event.RegisterCallback(delegate(float _)
			{
				this._suppressOpenAfterClose = false;
			}, 0);
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00051EA0 File Offset: 0x000500A0
		private void QueueDialogOpen()
		{
			if (this._dialogOpenQueued)
			{
				return;
			}
			this._dialogOpenQueued = true;
			this._api.Event.RegisterCallback(delegate(float _)
			{
				this._dialogOpenQueued = false;
				if (this._dialog.IsOpened() || !Enumerable.Any<ToolSlotData>(this.GetSlotsForToolDialog()))
				{
					return;
				}
				this._dialog.TryOpen(false);
			}, 0);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00051ED0 File Offset: 0x000500D0
		[return: Nullable(2)]
		private static IInventory GetBackpackInventory(IPlayer player)
		{
			return player.InventoryManager.GetOwnInventory("backpack");
		}

		// Token: 0x040007B7 RID: 1975
		public const string HotkeyCode = "combatoverhaul:tool-selection";

		// Token: 0x040007B8 RID: 1976
		public const string HotkeyLangCode = "combatoverhaul:tool-selection-hotkey";

		// Token: 0x040007B9 RID: 1977
		private readonly ICoreClientAPI _api;

		// Token: 0x040007BA RID: 1978
		private readonly ToolSelectionGuiDialog _dialog;

		// Token: 0x040007BB RID: 1979
		private readonly ToolBagSystemClient _toolBagSystem;

		// Token: 0x040007BC RID: 1980
		private bool _dialogOpenQueued;

		// Token: 0x040007BD RID: 1981
		private bool _suppressOpenAfterClose;
	}
}
