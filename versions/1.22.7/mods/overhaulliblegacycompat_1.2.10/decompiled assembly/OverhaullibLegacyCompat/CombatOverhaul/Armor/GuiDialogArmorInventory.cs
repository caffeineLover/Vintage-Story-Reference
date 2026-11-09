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
	// Token: 0x0200013A RID: 314
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GuiDialogArmorInventory : GuiDialog
	{
		// Token: 0x06000F1E RID: 3870 RVA: 0x00049B2C File Offset: 0x00047D2C
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

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00049BDC File Offset: 0x00047DDC
		public override bool PrefersUngrabbedMouse
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x00049BDF File Offset: 0x00047DDF
		[Nullable(2)]
		public override string ToggleKeyCombinationCode
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00049BE2 File Offset: 0x00047DE2
		private void OnOpenedEvent()
		{
			this.TryOpen();
			this.ComposeDialog();
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00049BF1 File Offset: 0x00047DF1
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

		// Token: 0x06000F23 RID: 3875 RVA: 0x00049C0C File Offset: 0x00047E0C
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

		// Token: 0x06000F24 RID: 3876 RVA: 0x00049C59 File Offset: 0x00047E59
		private void RecomposeDialogCallback()
		{
			this._recomposeDialog = false;
			this.ComposeDialog();
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00049C68 File Offset: 0x00047E68
		private void ComposeDialog()
		{
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00049C78 File Offset: 0x00047E78
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

		// Token: 0x06000F27 RID: 3879 RVA: 0x00049DA0 File Offset: 0x00047FA0
		private void OnDrawOuterIcon(Context ctx, ImageSurface surface, ElementBounds currentBounds)
		{
			IAsset asset = this.capi.Assets.TryGet(AssetLocation.Create("combatoverhaul:textures/icons/outer.svg", "game"), true);
			if (asset == null)
			{
				return;
			}
			this.capi.Gui.DrawSvg(asset, surface, (int)currentBounds.drawX, (int)currentBounds.drawY, (int)GuiElement.scaled(currentBounds.fixedWidth), (int)GuiElement.scaled(currentBounds.fixedHeight), default(int?));
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00049E14 File Offset: 0x00048014
		private void OnDrawMiddleIcon(Context ctx, ImageSurface surface, ElementBounds currentBounds)
		{
			IAsset asset = this.capi.Assets.TryGet(AssetLocation.Create("combatoverhaul:textures/icons/middle.svg", "game"), true);
			if (asset == null)
			{
				return;
			}
			this.capi.Gui.DrawSvg(asset, surface, (int)currentBounds.drawX, (int)currentBounds.drawY, (int)GuiElement.scaled(currentBounds.fixedWidth), (int)GuiElement.scaled(currentBounds.fixedHeight), default(int?));
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00049E88 File Offset: 0x00048088
		private void OnDrawSkinIcon(Context ctx, ImageSurface surface, ElementBounds currentBounds)
		{
			IAsset asset = this.capi.Assets.TryGet(AssetLocation.Create("combatoverhaul:textures/icons/skin.svg", "game"), true);
			if (asset == null)
			{
				return;
			}
			this.capi.Gui.DrawSvg(asset, surface, (int)currentBounds.drawX, (int)currentBounds.drawY, (int)GuiElement.scaled(currentBounds.fixedWidth), (int)GuiElement.scaled(currentBounds.fixedHeight), default(int?));
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00049EFB File Offset: 0x000480FB
		private void SendInvPacket(object packet)
		{
			this.capi.Network.SendPacketClient(packet);
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00049F10 File Offset: 0x00048110
		private static ElementBounds BelowCopySet(ref ElementBounds bounds, double fixedDeltaX = 0.0, double fixedDeltaY = 0.0, double fixedDeltaWidth = 0.0, double fixedDeltaHeight = 0.0)
		{
			ElementBounds result;
			bounds = (result = bounds.BelowCopy(fixedDeltaX, fixedDeltaY, fixedDeltaWidth, fixedDeltaHeight));
			return result;
		}

		// Token: 0x04000742 RID: 1858
		public const string DialogName = "CombatOverhaul:armor-inventory-dialog";

		// Token: 0x04000743 RID: 1859
		public readonly string DialogTitle = Lang.Get("combatoverhaul:armor-inventory-dialog-title", Array.Empty<object>());

		// Token: 0x04000744 RID: 1860
		private readonly RealDummyInventory _dummyInventory;

		// Token: 0x04000745 RID: 1861
		private bool _inventoryLinked;

		// Token: 0x04000746 RID: 1862
		[Nullable(2)]
		private GuiComposer _composer;

		// Token: 0x04000747 RID: 1863
		[Nullable(2)]
		private readonly GuiDialogCharacter _characterDialog;

		// Token: 0x04000748 RID: 1864
		private readonly ICoreClientAPI _api;

		// Token: 0x04000749 RID: 1865
		private bool _recomposeDialog;

		// Token: 0x0400074A RID: 1866
		private const int _recomposeDelay = 200;
	}
}
