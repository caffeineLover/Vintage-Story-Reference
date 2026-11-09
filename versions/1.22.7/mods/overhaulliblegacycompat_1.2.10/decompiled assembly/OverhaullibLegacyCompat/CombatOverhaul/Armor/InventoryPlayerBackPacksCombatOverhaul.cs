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
	// Token: 0x0200014C RID: 332
	[NullableContext(1)]
	[Nullable(0)]
	public class InventoryPlayerBackPacksCombatOverhaul : InventoryPlayerBackpacks
	{
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0004F420 File Offset: 0x0004D620
		public BagInventory BagInventory
		{
			get
			{
				return this.bagInv;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0004F428 File Offset: 0x0004D628
		public ItemSlot[] BackpackSlots
		{
			get
			{
				return this.bagSlots;
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0004F430 File Offset: 0x0004D630
		public InventoryPlayerBackPacksCombatOverhaul(string className, string playerUID, ICoreAPI api) : base(className, playerUID, api)
		{
			this._api = api;
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0004F442 File Offset: 0x0004D642
		public InventoryPlayerBackPacksCombatOverhaul(string inventoryId, ICoreAPI api) : base(inventoryId, api)
		{
			this._api = api;
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0004F453 File Offset: 0x0004D653
		public void ReloadBagInventory()
		{
			this.ReloadResolvedBagInventory();
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0004F45B File Offset: 0x0004D65B
		public override void AfterBlocksLoaded(IWorldAccessor world)
		{
			base.AfterBlocksLoaded(world);
			this.ReloadResolvedBagInventory();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0004F46C File Offset: 0x0004D66C
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

		// Token: 0x0600103A RID: 4154 RVA: 0x0004F4D0 File Offset: 0x0004D6D0
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

		// Token: 0x0600103B RID: 4155 RVA: 0x0004F510 File Offset: 0x0004D710
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

		// Token: 0x0600103C RID: 4156 RVA: 0x0004F560 File Offset: 0x0004D760
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

		// Token: 0x0600103D RID: 4157 RVA: 0x0004F62E File Offset: 0x0004D82E
		private void ReloadResolvedBagInventory()
		{
			this.bagInv.ReloadBagInventory(this, this.GetResolvedBagInventorySlots(this.bagSlots));
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0004F648 File Offset: 0x0004D848
		private ItemSlot[] GetResolvedBagInventorySlots(ItemSlot[] backpackSlots)
		{
			InventoryBase gearInventory = InventoryPlayerBackPacksCombatOverhaul.GetGearInventory(base.Owner);
			return Enumerable.ToArray<ItemSlot>(Enumerable.Select<ItemSlot, ItemSlot>(Enumerable.Concat<ItemSlot>(((gearInventory != null) ? Enumerable.ToArray<ItemSlot>(gearInventory) : null) ?? Array.Empty<ItemSlot>(), backpackSlots), new Func<ItemSlot, ItemSlot>(this.GetResolvedReloadSlot)));
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0004F688 File Offset: 0x0004D888
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

		// Token: 0x06001040 RID: 4160 RVA: 0x0004F6DC File Offset: 0x0004D8DC
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

		// Token: 0x06001041 RID: 4161 RVA: 0x0004F75A File Offset: 0x0004D95A
		private void ResolveStack(ItemStack stack)
		{
			if (stack.Collectible == null && this._api.World != null)
			{
				stack.ResolveBlockOrItem(this._api.World);
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0004F784 File Offset: 0x0004D984
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

		// Token: 0x06001043 RID: 4163 RVA: 0x0004F848 File Offset: 0x0004DA48
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

		// Token: 0x06001044 RID: 4164 RVA: 0x0004F88D File Offset: 0x0004DA8D
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

		// Token: 0x04000789 RID: 1929
		private ICoreAPI _api;
	}
}
