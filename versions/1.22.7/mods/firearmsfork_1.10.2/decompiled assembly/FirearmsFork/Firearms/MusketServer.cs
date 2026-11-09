using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace Firearms
{
	// Token: 0x0200001C RID: 28
	[NullableContext(1)]
	[Nullable(0)]
	public class MusketServer : MuzzleloaderServer
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00005839 File Offset: 0x00003A39
		public MusketServer(ICoreServerAPI api, Item item) : base(api, item)
		{
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005850 File Offset: 0x00003A50
		public override bool Reload(IServerPlayer player, ItemSlot slot, [Nullable(2)] ItemSlot ammoSlot, ReloadPacket packet)
		{
			switch (MuzzleloaderServer.GetLoadingStage<MusketLoadingStage>(packet))
			{
			case MusketLoadingStage.AttachBayonet:
			{
				if (((ammoSlot != null) ? ammoSlot.Itemstack : null) == null)
				{
					return false;
				}
				ItemStack bayonet = ammoSlot.TakeOut(1);
				if (bayonet == null || bayonet.StackSize <= 0)
				{
					return false;
				}
				bayonet.ResolveBlockOrItem(this.Api.World);
				this.BayonetInventory.Read(slot, "bayonet");
				if (this.BayonetInventory.Items.Count > 0)
				{
					this.BayonetInventory.Clear();
					return false;
				}
				this.BayonetInventory.Items.Add(bayonet);
				this.BayonetInventory.Write(slot);
				this.BayonetInventory.Clear();
				int renderVariant;
				if (slot.Itemstack != null && FirearmRenderVariantUtil.TryGetBayonetRenderVariant(slot.Itemstack, bayonet, out renderVariant))
				{
					slot.Itemstack.Attributes.SetInt("renderVariant", renderVariant);
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					if (itemstack != null)
					{
						ITreeAttribute attributes = itemstack.Attributes;
						if (attributes != null)
						{
							attributes.RemoveAttribute("renderVariant");
						}
					}
					ILogger logger = this.Api.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(160, 2);
					defaultInterpolatedStringHandler.AppendLiteral("[maltiezfirearms] Could not resolve a valid renderVariant for bayonet '");
					CollectibleObject collectible = bayonet.Collectible;
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>((collectible != null) ? collectible.Code : null);
					defaultInterpolatedStringHandler.AppendLiteral("' on firearm '");
					ItemStack itemstack2 = slot.Itemstack;
					AssetLocation assetLocation;
					if (itemstack2 == null)
					{
						assetLocation = null;
					}
					else
					{
						CollectibleObject collectible2 = itemstack2.Collectible;
						assetLocation = ((collectible2 != null) ? collectible2.Code : null);
					}
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(assetLocation);
					defaultInterpolatedStringHandler.AppendLiteral("'. The attachment was kept but the alternate firearm model was not applied.");
					logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				slot.MarkDirty();
				ammoSlot.MarkDirty();
				return true;
			}
			case MusketLoadingStage.DamageBayonet:
			{
				this.BayonetInventory.Read(slot, "bayonet");
				if (this.BayonetInventory.Items.Count == 0)
				{
					this.BayonetInventory.Clear();
					return false;
				}
				ItemStack itemStack = this.BayonetInventory.Items[0];
				itemStack.ResolveBlockOrItem(this.Api.World);
				DummySlot dummySlot = new DummySlot(itemStack);
				itemStack.Item.DamageItem(this.Api.World, player.Entity, dummySlot, 1, true);
				if (dummySlot.Empty)
				{
					this.BayonetInventory.Items.Clear();
				}
				this.BayonetInventory.Write(slot);
				this.BayonetInventory.Clear();
				return true;
			}
			case MusketLoadingStage.DetachBayonet:
			{
				this.BayonetInventory.Read(slot, "bayonet");
				if (this.BayonetInventory.Items.Count == 0)
				{
					this.BayonetInventory.Clear();
					ItemStack itemstack3 = slot.Itemstack;
					if (itemstack3 != null)
					{
						ITreeAttribute attributes2 = itemstack3.Attributes;
						if (attributes2 != null)
						{
							attributes2.RemoveAttribute("renderVariant");
						}
					}
					slot.MarkDirty();
					return false;
				}
				ItemStack bayonet2 = this.BayonetInventory.Items[0];
				bayonet2.ResolveBlockOrItem(this.Api.World);
				this.BayonetInventory.Items.Clear();
				this.BayonetInventory.Write(slot);
				this.BayonetInventory.Clear();
				if (!player.Entity.TryGiveItemStack(bayonet2))
				{
					this.Api.World.SpawnItemEntity(bayonet2, player.Entity.Pos.AsBlockPos, null);
				}
				ItemStack itemstack4 = slot.Itemstack;
				if (itemstack4 != null)
				{
					ITreeAttribute attributes3 = itemstack4.Attributes;
					if (attributes3 != null)
					{
						attributes3.RemoveAttribute("renderVariant");
					}
				}
				slot.MarkDirty();
				return true;
			}
			default:
				return base.Reload(player, slot, ammoSlot, packet);
			}
		}

		// Token: 0x0400007A RID: 122
		protected readonly ItemInventoryBuffer BayonetInventory = new ItemInventoryBuffer();

		// Token: 0x0400007B RID: 123
		protected const string BayonetInventoryId = "bayonet";
	}
}
