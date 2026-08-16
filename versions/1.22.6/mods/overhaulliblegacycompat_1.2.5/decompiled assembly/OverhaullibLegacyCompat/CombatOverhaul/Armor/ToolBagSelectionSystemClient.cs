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
	// Token: 0x02000148 RID: 328
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ToolBagSelectionSystemClient
	{
		// Token: 0x06001045 RID: 4165 RVA: 0x000501EC File Offset: 0x0004E3EC
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

		// Token: 0x06001046 RID: 4166 RVA: 0x000502E0 File Offset: 0x0004E4E0
		public IEnumerable<ToolSlotData> GetToolSlots()
		{
			IInventory inventory = ToolBagSelectionSystemClient.GetBackpackInventory(this._api.World.Player);
			if (inventory == null)
			{
				return Array.Empty<ToolSlotData>();
			}
			return Enumerable.Select<ItemSlotBagContentWithWildcardMatch, ToolSlotData>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.OfType<ItemSlotBagContentWithWildcardMatch>(inventory), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.HandleHotkey || slot.Config.DisplayInToolDialog), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.BackpackCategoryCode != "ammunition"), (ItemSlotBagContentWithWildcardMatch slot) => new ToolSlotData(slot.Config, slot.Itemstack, slot.ToolBagId, slot.ToolBagIndex, slot.MainHand, slot.BackgroundIcon, slot.HexBackgroundColor ?? "", slot.SlotIndex));
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00050380 File Offset: 0x0004E580
		public IEnumerable<ToolSlotData> GetSlotsForToolDialog()
		{
			IInventory inventory = ToolBagSelectionSystemClient.GetBackpackInventory(this._api.World.Player);
			if (inventory == null)
			{
				return Array.Empty<ToolSlotData>();
			}
			return Enumerable.Select<ItemSlotBagContentWithWildcardMatch, ToolSlotData>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.OfType<ItemSlotBagContentWithWildcardMatch>(inventory), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.DisplayInToolDialog || slot.Config.HandleHotkey), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.BackpackCategoryCode != "ammunition"), (ItemSlotBagContentWithWildcardMatch slot) => new ToolSlotData(slot.Config, slot.Itemstack, slot.ToolBagId, slot.ToolBagIndex, slot.MainHand, slot.BackgroundIcon, slot.HexBackgroundColor ?? "", slot.SlotIndex));
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00050420 File Offset: 0x0004E620
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

		// Token: 0x06001049 RID: 4169 RVA: 0x000504B8 File Offset: 0x0004E6B8
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

		// Token: 0x0600104A RID: 4170 RVA: 0x0005051C File Offset: 0x0004E71C
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

		// Token: 0x0600104B RID: 4171 RVA: 0x00050568 File Offset: 0x0004E768
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

		// Token: 0x0600104C RID: 4172 RVA: 0x00050598 File Offset: 0x0004E798
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

		// Token: 0x0600104D RID: 4173 RVA: 0x000505C8 File Offset: 0x0004E7C8
		[return: Nullable(2)]
		private static IInventory GetBackpackInventory(IPlayer player)
		{
			return player.InventoryManager.GetOwnInventory("backpack");
		}

		// Token: 0x04000791 RID: 1937
		public const string HotkeyCode = "combatoverhaul:tool-selection";

		// Token: 0x04000792 RID: 1938
		public const string HotkeyLangCode = "combatoverhaul:tool-selection-hotkey";

		// Token: 0x04000793 RID: 1939
		private readonly ICoreClientAPI _api;

		// Token: 0x04000794 RID: 1940
		private readonly ToolSelectionGuiDialog _dialog;

		// Token: 0x04000795 RID: 1941
		private readonly ToolBagSystemClient _toolBagSystem;

		// Token: 0x04000796 RID: 1942
		private bool _dialogOpenQueued;

		// Token: 0x04000797 RID: 1943
		private bool _suppressOpenAfterClose;
	}
}
