using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Cairo;
using CombatOverhaul.DamageSystems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200012E RID: 302
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GuiDialogArmorInventory : GuiDialog
	{
		// Token: 0x06000EAA RID: 3754 RVA: 0x0004822C File Offset: 0x0004642C
		public GuiDialogArmorInventory(ICoreClientAPI api) : base(api)
		{
			this._api = api;
			foreach (GuiDialogCharacter characterDialog in Enumerable.OfType<GuiDialogCharacter>(api.Gui.LoadedGuis))
			{
				characterDialog.OnOpened += new Action(this.OnOpenedEvent);
				characterDialog.OnClosed += new Action(this.OnClosedEvent);
				this._characterDialog = characterDialog;
			}
			this._dummyInventory = new RealDummyInventory(api, ArmorInventory._totalSlotsNumber);
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000EAB RID: 3755 RVA: 0x000482DC File Offset: 0x000464DC
		public override bool PrefersUngrabbedMouse
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x000482DF File Offset: 0x000464DF
		[Nullable(2)]
		public override string ToggleKeyCombinationCode
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x000482E2 File Offset: 0x000464E2
		private void OnOpenedEvent()
		{
			this.TryOpen();
			this.ComposeDialog();
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x000482F1 File Offset: 0x000464F1
		private void OnClosedEvent()
		{
			this.TryClose();
			GuiComposer composer = this._composer;
			if (composer == null)
			{
				return;
			}
			composer.Dispose();
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0004830C File Offset: 0x0004650C
		private void RecomposeDialog(bool itemChanged, bool durabilityChanged, bool isArmorSlot)
		{
			if (!this.opened || this._recomposeDialog || !itemChanged || !isArmorSlot)
			{
				return;
			}
			this._recomposeDialog = true;
			this._api.World.RegisterCallback(delegate(float _)
			{
				this.RecomposeDialogCallback();
			}, 200);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00048359 File Offset: 0x00046559
		private void RecomposeDialogCallback()
		{
			this._recomposeDialog = false;
			this.ComposeDialog();
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00048368 File Offset: 0x00046568
		private void ComposeDialog()
		{
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00048378 File Offset: 0x00046578
		private void AddSlot(ArmorInventory inv, ArmorLayers layers, DamageZone zone, ref ElementBounds bounds, double gap)
		{
			int slotIndex = ArmorInventory.IndexFromArmorType(layers, zone);
			if (inv.IsSlotAvailable(slotIndex) || !inv[slotIndex].Empty)
			{
				GuiComposerHelpers.AddItemSlotGrid(this._composer, inv, new Action<object>(this.SendInvPacket), 1, new int[]
				{
					slotIndex
				}, GuiDialogArmorInventory.BelowCopySet(ref bounds, 0.0, gap, 0.0, 0.0), null);
				return;
			}
			inv[slotIndex].HexBackgroundColor = "#AAAAAA";
			this._dummyInventory[slotIndex].HexBackgroundColor = "#AAAAAA";
			this._dummyInventory[slotIndex].Itemstack = inv[inv.GetSlotBlockingSlotIndex(layers, zone)].Itemstack;
			GuiComposerHelpers.AddItemSlotGrid(this._composer, this._dummyInventory, delegate(object _)
			{
			}, 1, new int[]
			{
				slotIndex
			}, GuiDialogArmorInventory.BelowCopySet(ref bounds, 0.0, gap, 0.0, 0.0), null);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x000484A0 File Offset: 0x000466A0
		private void OnDrawOuterIcon(Context ctx, ImageSurface surface, ElementBounds currentBounds)
		{
			IAsset asset = this.capi.Assets.TryGet(AssetLocation.Create("combatoverhaul:textures/icons/outer.svg", "game"), true);
			if (asset == null)
			{
				return;
			}
			this.capi.Gui.DrawSvg(asset, surface, (int)currentBounds.drawX, (int)currentBounds.drawY, (int)GuiElement.scaled(currentBounds.fixedWidth), (int)GuiElement.scaled(currentBounds.fixedHeight), default(int?));
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00048514 File Offset: 0x00046714
		private void OnDrawMiddleIcon(Context ctx, ImageSurface surface, ElementBounds currentBounds)
		{
			IAsset asset = this.capi.Assets.TryGet(AssetLocation.Create("combatoverhaul:textures/icons/middle.svg", "game"), true);
			if (asset == null)
			{
				return;
			}
			this.capi.Gui.DrawSvg(asset, surface, (int)currentBounds.drawX, (int)currentBounds.drawY, (int)GuiElement.scaled(currentBounds.fixedWidth), (int)GuiElement.scaled(currentBounds.fixedHeight), default(int?));
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00048588 File Offset: 0x00046788
		private void OnDrawSkinIcon(Context ctx, ImageSurface surface, ElementBounds currentBounds)
		{
			IAsset asset = this.capi.Assets.TryGet(AssetLocation.Create("combatoverhaul:textures/icons/skin.svg", "game"), true);
			if (asset == null)
			{
				return;
			}
			this.capi.Gui.DrawSvg(asset, surface, (int)currentBounds.drawX, (int)currentBounds.drawY, (int)GuiElement.scaled(currentBounds.fixedWidth), (int)GuiElement.scaled(currentBounds.fixedHeight), default(int?));
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x000485FB File Offset: 0x000467FB
		private void SendInvPacket(object packet)
		{
			this.capi.Network.SendPacketClient(packet);
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00048610 File Offset: 0x00046810
		private static ElementBounds BelowCopySet(ref ElementBounds bounds, double fixedDeltaX = 0.0, double fixedDeltaY = 0.0, double fixedDeltaWidth = 0.0, double fixedDeltaHeight = 0.0)
		{
			ElementBounds result;
			bounds = (result = bounds.BelowCopy(fixedDeltaX, fixedDeltaY, fixedDeltaWidth, fixedDeltaHeight));
			return result;
		}

		// Token: 0x0400071C RID: 1820
		public const string DialogName = "CombatOverhaul:armor-inventory-dialog";

		// Token: 0x0400071D RID: 1821
		public readonly string DialogTitle = Lang.Get("combatoverhaul:armor-inventory-dialog-title", Array.Empty<object>());

		// Token: 0x0400071E RID: 1822
		private readonly RealDummyInventory _dummyInventory;

		// Token: 0x0400071F RID: 1823
		private bool _inventoryLinked;

		// Token: 0x04000720 RID: 1824
		[Nullable(2)]
		private GuiComposer _composer;

		// Token: 0x04000721 RID: 1825
		[Nullable(2)]
		private readonly GuiDialogCharacter _characterDialog;

		// Token: 0x04000722 RID: 1826
		private readonly ICoreClientAPI _api;

		// Token: 0x04000723 RID: 1827
		private bool _recomposeDialog;

		// Token: 0x04000724 RID: 1828
		private const int _recomposeDelay = 200;
	}
}
