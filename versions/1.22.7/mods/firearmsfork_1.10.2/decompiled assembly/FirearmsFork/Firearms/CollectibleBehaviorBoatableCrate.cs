using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Firearms
{
	// Token: 0x0200000E RID: 14
	[NullableContext(1)]
	[Nullable(0)]
	public class CollectibleBehaviorBoatableCrate : CollectibleBehaviorHeldBag, IAttachedInteractions, IAttachedListener
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00003091 File Offset: 0x00001291
		public CollectibleBehaviorBoatableCrate(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000309A File Offset: 0x0000129A
		public override void OnLoaded(ICoreAPI api)
		{
			this.Api = api;
			base.OnLoaded(api);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000030AA File Offset: 0x000012AA
		public override bool IsEmpty(ItemStack bagstack)
		{
			return base.IsEmpty(bagstack);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000030B8 File Offset: 0x000012B8
		public override int GetQuantitySlots(ItemStack bagstack)
		{
			BlockCrate crate = this.collObj as BlockCrate;
			if (crate == null)
			{
				return 0;
			}
			string type = bagstack.Attributes.GetString("type", null) ?? crate.Props.DefaultType;
			return crate.Props[type].QuantitySlots;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003108 File Offset: 0x00001308
		public override void OnInteract(ItemSlot bagSlot, int slotIndex, Entity onEntity, EntityAgent byEntity, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled, Action onRequireSave)
		{
			IMountableSeat mountedOn = byEntity.MountedOn;
			if ((((mountedOn != null) ? mountedOn.Controls : null) ?? byEntity.Controls).Sprint)
			{
				return;
			}
			bool put = byEntity.Controls.ShiftKey;
			bool take = !put;
			bool bulk = byEntity.Controls.CtrlKey;
			IPlayer byPlayer = (byEntity as EntityPlayer).Player;
			AttachedContainerWorkspace ws = base.getOrCreateContainerWorkspace(slotIndex, onEntity, onRequireSave);
			BlockFacing face = BlockFacing.UP;
			Vec3d Pos = byEntity.Pos.XYZ;
			if (!ws.TryLoadInv(bagSlot, slotIndex, onEntity))
			{
				return;
			}
			ItemSlot ownSlot = ws.WrapperInv.FirstNonEmptySlot;
			ItemSlot hotbarslot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (take && ownSlot != null)
			{
				ItemStack stack = bulk ? ownSlot.TakeOutWhole() : ownSlot.TakeOut(1);
				int quantity = bulk ? stack.StackSize : 1;
				if (!byPlayer.InventoryManager.TryGiveItemstack(stack, true))
				{
					this.Api.World.SpawnItemEntity(stack, Pos.Add((double)(0.5f + face.Normalf.X), (double)(0.5f + face.Normalf.Y), (double)(0.5f + face.Normalf.Z)), null);
				}
				else
				{
					this.didMoveItems(stack, byPlayer);
				}
				this.Api.World.Logger.Audit("{0} Took {1}x{2} from Boat crate at {3}.", new object[]
				{
					byPlayer.PlayerName,
					quantity,
					stack.Collectible.Code,
					Pos
				});
				ws.BagInventory.SaveSlotIntoBag((ItemSlotBagContent)ownSlot);
			}
			if (put && !hotbarslot.Empty)
			{
				int quantity2 = bulk ? hotbarslot.StackSize : 1;
				if (ownSlot == null)
				{
					if (hotbarslot.TryPutInto(this.Api.World, ws.WrapperInv[0], quantity2) > 0)
					{
						this.didMoveItems(ws.WrapperInv[0].Itemstack, byPlayer);
						this.Api.World.Logger.Audit("{0} Put {1}x{2} into Boat crate at {3}.", new object[]
						{
							byPlayer.PlayerName,
							quantity2,
							ws.WrapperInv[0].Itemstack.Collectible.Code,
							Pos
						});
					}
					ws.BagInventory.SaveSlotIntoBag((ItemSlotBagContent)ws.WrapperInv[0]);
				}
				else if (hotbarslot.Itemstack.Equals(this.Api.World, ownSlot.Itemstack, GlobalConstants.IgnoredStackAttributes))
				{
					List<ItemSlot> skipSlots = new List<ItemSlot>();
					while (hotbarslot.StackSize > 0 && skipSlots.Count < ws.WrapperInv.Count)
					{
						WeightedSlot wslot = ws.WrapperInv.GetBestSuitedSlot(hotbarslot, null, skipSlots);
						if (wslot.slot == null)
						{
							break;
						}
						if (hotbarslot.TryPutInto(this.Api.World, wslot.slot, quantity2) > 0)
						{
							this.didMoveItems(wslot.slot.Itemstack, byPlayer);
							ws.BagInventory.SaveSlotIntoBag((ItemSlotBagContent)wslot.slot);
							this.Api.World.Logger.Audit("{0} Put {1}x{2} into Boat crate at {3}.", new object[]
							{
								byPlayer.PlayerName,
								quantity2,
								wslot.slot.Itemstack.Collectible.Code,
								Pos
							});
							if (!bulk)
							{
								break;
							}
						}
						skipSlots.Add(wslot.slot);
					}
				}
				hotbarslot.MarkDirty();
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000034B4 File Offset: 0x000016B4
		protected void didMoveItems(ItemStack stack, IPlayer byPlayer)
		{
			ICoreClientAPI coreClientAPI = this.Api as ICoreClientAPI;
			if (coreClientAPI != null)
			{
				coreClientAPI.World.Player.TriggerFpAnimation(2);
			}
			this.Api.World.PlaySoundAt(new AssetLocation("sounds/player/build"), byPlayer.Entity, byPlayer, true, 16f, 1f);
		}

		// Token: 0x04000042 RID: 66
		private ICoreAPI Api;
	}
}
