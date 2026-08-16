using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000140 RID: 320
	[NullableContext(1)]
	[Nullable(0)]
	public class InventoryPlayerBackPacksCombatOverhaul : InventoryPlayerBackpacks
	{
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0004DB18 File Offset: 0x0004BD18
		public BagInventory BagInventory
		{
			get
			{
				return this.bagInv;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0004DB20 File Offset: 0x0004BD20
		public ItemSlot[] BackpackSlots
		{
			get
			{
				return this.bagSlots;
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0004DB28 File Offset: 0x0004BD28
		public InventoryPlayerBackPacksCombatOverhaul(string className, string playerUID, ICoreAPI api) : base(className, playerUID, api)
		{
			this._api = api;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0004DB3A File Offset: 0x0004BD3A
		public InventoryPlayerBackPacksCombatOverhaul(string inventoryId, ICoreAPI api) : base(inventoryId, api)
		{
			this._api = api;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0004DB4B File Offset: 0x0004BD4B
		public void ReloadBagInventory()
		{
			this.ReloadResolvedBagInventory();
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0004DB53 File Offset: 0x0004BD53
		public override void AfterBlocksLoaded(IWorldAccessor world)
		{
			base.AfterBlocksLoaded(world);
			this.ReloadResolvedBagInventory();
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0004DB64 File Offset: 0x0004BD64
		public override void OnItemSlotModified(ItemSlot slot)
		{
			ItemSlotBagContent bagContentSlot = slot as ItemSlotBagContent;
			if (bagContentSlot != null)
			{
				if (this.CanSaveSlotIntoResolvedBag(bagContentSlot))
				{
					base.OnItemSlotModified(slot);
					return;
				}
			}
			else
			{
				this.ReloadResolvedBagInventory();
				if (this.Api.Side == 1)
				{
					IServerPlayer serverPlayer = this.Api.World.PlayerByUid(this.playerUID) as IServerPlayer;
					if (serverPlayer == null)
					{
						return;
					}
					serverPlayer.BroadcastPlayerData(false);
				}
			}
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0004DBC8 File Offset: 0x0004BDC8
		public override object ActivateSlot(int slotId, ItemSlot sourceSlot, ref ItemStackMoveOperation op)
		{
			bool flag = slotId < this.bagSlots.Length && this.bagSlots[slotId].Itemstack == null;
			object packet = base.ActivateSlot(slotId, sourceSlot, ref op);
			if (flag)
			{
				this.ReloadResolvedBagInventory();
			}
			return packet;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0004DC08 File Offset: 0x0004BE08
		public override void DiscardAll()
		{
			for (int i = 0; i < this.bagSlots.Length; i++)
			{
				if (this.bagSlots[i].Itemstack != null)
				{
					this.dirtySlots.Add(i);
				}
				this.bagSlots[i].Itemstack = null;
			}
			this.ReloadResolvedBagInventory();
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0004DC58 File Offset: 0x0004BE58
		public override void DropAll(Vec3d pos, int maxStackSize = 0)
		{
			IPlayer player = base.Player;
			JsonObject jsonObject;
			if (player == null)
			{
				jsonObject = null;
			}
			else
			{
				EntityPlayer entity = player.Entity;
				jsonObject = ((entity != null) ? entity.Properties.Attributes : null);
			}
			JsonObject attr = jsonObject;
			int timer = (attr == null) ? GlobalConstants.TimeToDespawnPlayerInventoryDrops : attr["droppedItemsOnDeathTimer"].AsInt(GlobalConstants.TimeToDespawnPlayerInventoryDrops);
			for (int i = 0; i < this.bagSlots.Length; i++)
			{
				ItemSlot slot = this.bagSlots[i];
				if (slot.Itemstack != null)
				{
					EnumHandling handling = 0;
					slot.Itemstack.Collectible.OnHeldDropped(this.Api.World, base.Player, slot, slot.StackSize, ref handling);
					if (handling == null)
					{
						this.dirtySlots.Add(i);
						base.spawnItemEntity(slot.Itemstack, pos, timer);
						slot.Itemstack = null;
					}
				}
			}
			this.ReloadResolvedBagInventory();
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0004DD26 File Offset: 0x0004BF26
		private void ReloadResolvedBagInventory()
		{
			this.bagInv.ReloadBagInventory(this, this.GetResolvedBagInventorySlots(this.bagSlots));
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0004DD40 File Offset: 0x0004BF40
		private ItemSlot[] GetResolvedBagInventorySlots(ItemSlot[] backpackSlots)
		{
			InventoryBase gearInventory = InventoryPlayerBackPacksCombatOverhaul.GetGearInventory(base.Owner);
			return Enumerable.ToArray<ItemSlot>(Enumerable.Select<ItemSlot, ItemSlot>(Enumerable.Concat<ItemSlot>(((gearInventory != null) ? Enumerable.ToArray<ItemSlot>(gearInventory) : null) ?? Array.Empty<ItemSlot>(), backpackSlots), new Func<ItemSlot, ItemSlot>(this.GetResolvedReloadSlot)));
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0004DD80 File Offset: 0x0004BF80
		private ItemSlot GetResolvedReloadSlot([Nullable(2)] ItemSlot slot)
		{
			ItemStack stack = (slot != null) ? slot.Itemstack : null;
			if (stack == null)
			{
				return slot ?? new DummySlot();
			}
			this.ResolveStack(stack);
			if (stack.Collectible == null)
			{
				this.LogUnresolvedStack(slot, stack, "reload");
				return new DummySlot();
			}
			return slot ?? new DummySlot();
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0004DDD4 File Offset: 0x0004BFD4
		private bool CanSaveSlotIntoResolvedBag(ItemSlotBagContent slot)
		{
			ItemSlot bagSlot = (slot.BagIndex >= 0 && slot.BagIndex < this.bagInv.BagSlots.Length) ? this.bagInv.BagSlots[slot.BagIndex] : null;
			ItemStack stack = (bagSlot != null) ? bagSlot.Itemstack : null;
			if (stack == null)
			{
				return false;
			}
			this.ResolveStack(stack);
			CollectibleObject collectible = stack.Collectible;
			if (((collectible != null) ? collectible.GetCollectibleInterface<IHeldBag>() : null) == null)
			{
				this.LogUnresolvedStack(bagSlot, stack, "save");
				return false;
			}
			return true;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0004DE52 File Offset: 0x0004C052
		private void ResolveStack(ItemStack stack)
		{
			if (stack.Collectible == null && this._api.World != null)
			{
				stack.ResolveBlockOrItem(this._api.World);
			}
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0004DE7C File Offset: 0x0004C07C
		private void LogUnresolvedStack([Nullable(2)] ItemSlot slot, ItemStack stack, string action)
		{
			string text;
			if (slot == null)
			{
				text = null;
			}
			else
			{
				InventoryBase inventory = slot.Inventory;
				text = ((inventory != null) ? inventory.InventoryID : null);
			}
			string inventoryId = text ?? "<unknown>";
			int slotId = InventoryPlayerBackPacksCombatOverhaul.GetSlotIndex(slot);
			ICoreAPI api = this._api;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(96, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Skipping unresolved stack while trying to ");
			defaultInterpolatedStringHandler.AppendFormatted(action);
			defaultInterpolatedStringHandler.AppendLiteral(" bag inventory: inventory=");
			defaultInterpolatedStringHandler.AppendFormatted(inventoryId);
			defaultInterpolatedStringHandler.AppendLiteral(", slot=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(slotId);
			defaultInterpolatedStringHandler.AppendLiteral(", itemId=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(stack.Id);
			defaultInterpolatedStringHandler.AppendLiteral(", stackSize=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(stack.StackSize);
			LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0004DF40 File Offset: 0x0004C140
		[NullableContext(2)]
		private static int GetSlotIndex(ItemSlot slot)
		{
			if (((slot != null) ? slot.Inventory : null) == null)
			{
				return -1;
			}
			for (int index = 0; index < slot.Inventory.Count; index++)
			{
				if (slot.Inventory[index] == slot)
				{
					return index;
				}
			}
			return -1;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0004DF85 File Offset: 0x0004C185
		[return: Nullable(2)]
		private static InventoryBase GetGearInventory(Entity entity)
		{
			if (entity == null)
			{
				return null;
			}
			EntityBehaviorPlayerInventory behavior = entity.GetBehavior<EntityBehaviorPlayerInventory>();
			if (behavior == null)
			{
				return null;
			}
			return behavior.Inventory;
		}

		// Token: 0x04000763 RID: 1891
		private ICoreAPI _api;
	}
}
