using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000149 RID: 329
	[NullableContext(2)]
	[Nullable(0)]
	public class ClothesSlot : ItemSlotCharacter, IClickableSlot
	{
		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x0004E1A9 File Offset: 0x0004C3A9
		// (set) Token: 0x06000FE2 RID: 4066 RVA: 0x0004E1B1 File Offset: 0x0004C3B1
		public IWorldAccessor World { get; set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x0004E1BA File Offset: 0x0004C3BA
		// (set) Token: 0x06000FE4 RID: 4068 RVA: 0x0004E1C2 File Offset: 0x0004C3C2
		public string OwnerUUID { get; set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0004E1CB File Offset: 0x0004C3CB
		// (set) Token: 0x06000FE6 RID: 4070 RVA: 0x0004E1D3 File Offset: 0x0004C3D3
		public bool PreviouslyHeldBag { get; set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0004E1DC File Offset: 0x0004C3DC
		// (set) Token: 0x06000FE8 RID: 4072 RVA: 0x0004E1E4 File Offset: 0x0004C3E4
		public int PreviousItemId { get; set; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x0004E1ED File Offset: 0x0004C3ED
		// (set) Token: 0x06000FEA RID: 4074 RVA: 0x0004E1F5 File Offset: 0x0004C3F5
		public int PreviousDurability { get; set; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000FEB RID: 4075 RVA: 0x0004E1FE File Offset: 0x0004C3FE
		// (set) Token: 0x06000FEC RID: 4076 RVA: 0x0004E206 File Offset: 0x0004C406
		public string PreviousColor { get; set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0004E20F File Offset: 0x0004C40F
		// (set) Token: 0x06000FEE RID: 4078 RVA: 0x0004E217 File Offset: 0x0004C417
		public bool PreviousEmpty { get; set; }

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0004E220 File Offset: 0x0004C420
		// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x0004E228 File Offset: 0x0004C428
		public IClickableSlot.SlotClickedDelegate OnSlotClicked { get; set; }

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0004E231 File Offset: 0x0004C431
		[NullableContext(1)]
		public ClothesSlot(EnumCharacterDressType type, InventoryBase inventory) : base(type, inventory)
		{
			this.MaxSlotStackSize = 1;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0004E244 File Offset: 0x0004C444
		[NullableContext(1)]
		public override void ActivateSlot(ItemSlot sourceSlot, ref ItemStackMoveOperation op)
		{
			IClickableSlot.SlotClickedDelegate onSlotClicked = this.OnSlotClicked;
			if (onSlotClicked != null && onSlotClicked(this, sourceSlot, ref op))
			{
				return;
			}
			try
			{
				this.ResolveStack((sourceSlot != null) ? sourceSlot.Itemstack : null);
				this.ResolveStack(base.Itemstack);
				base.ActivateSlot(sourceSlot, ref op);
				this.OnItemSlotModified(null);
			}
			catch (Exception exception)
			{
				IWorldAccessor world = this.World;
				ICoreAPI api = (world != null) ? world.Api : null;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("(ActivateSlot) Exception: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Debug(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0004E2EC File Offset: 0x0004C4EC
		public override ItemStack TakeOutWhole()
		{
			if (!this.CanTake())
			{
				return null;
			}
			ItemStack itemStack = base.TakeOutWhole();
			if (itemStack != null)
			{
				this.EmptyBag(itemStack);
			}
			return itemStack;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0004E318 File Offset: 0x0004C518
		[NullableContext(1)]
		public override int TryPutInto(ItemSlot sinkSlot, ref ItemStackMoveOperation op)
		{
			int movedQuantity;
			if (this.TryPutIntoBagContentBeforeReload(sinkSlot, ref op, out movedQuantity))
			{
				return movedQuantity;
			}
			return base.TryPutInto(sinkSlot, ref op);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0004E33C File Offset: 0x0004C53C
		public override ItemStack TakeOut(int quantity)
		{
			if (!this.CanTake())
			{
				return null;
			}
			ItemStack stack = base.TakeOut(quantity);
			this.EmptyBag(stack);
			return stack;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0004E364 File Offset: 0x0004C564
		public override bool CanTake()
		{
			this.ResolveStack(this.itemstack);
			ItemStack itemstack = this.itemstack;
			IHeldBag heldBag;
			if (itemstack == null)
			{
				heldBag = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				heldBag = ((collectible != null) ? collectible.GetCollectibleInterface<IHeldBag>() : null);
			}
			IHeldBag bag = heldBag;
			if (bag != null && !bag.IsEmpty(this.itemstack))
			{
				InventoryBasePlayer inventoryBasePlayer = this.inventory as InventoryBasePlayer;
				object obj;
				if (inventoryBasePlayer == null)
				{
					obj = null;
				}
				else
				{
					IPlayer player = inventoryBasePlayer.Player;
					if (player == null)
					{
						obj = null;
					}
					else
					{
						EntityPlayer entity = player.Entity;
						obj = ((entity != null) ? entity.Api : null);
					}
				}
				ICoreClientAPI coreClientAPI = obj as ICoreClientAPI;
				if (coreClientAPI != null)
				{
					coreClientAPI.TriggerIngameError(this, "canttakeout", "Cannot take out item. Empty its contents before removing it.");
				}
				return false;
			}
			return base.CanTake();
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0004E400 File Offset: 0x0004C600
		[NullableContext(1)]
		protected override void FlipWith(ItemSlot itemSlot)
		{
			if (!this.CanTake())
			{
				return;
			}
			this.ResolveStack((itemSlot != null) ? itemSlot.Itemstack : null);
			base.FlipWith(itemSlot);
			ItemStack stack = itemSlot.Itemstack;
			if (stack != null)
			{
				this.EmptyBag(stack);
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0004E440 File Offset: 0x0004C640
		[NullableContext(1)]
		private bool TryPutIntoBagContentBeforeReload(ItemSlot sinkSlot, ref ItemStackMoveOperation op, out int movedQuantity)
		{
			movedQuantity = 0;
			if (!(sinkSlot is ItemSlotBagContent) || sinkSlot is ItemSlotTakeOutOnly)
			{
				return false;
			}
			ItemSlotBagContentWithWildcardMatch itemSlotBagContentWithWildcardMatch = sinkSlot as ItemSlotBagContentWithWildcardMatch;
			if (itemSlotBagContentWithWildcardMatch != null)
			{
				ItemStack sourceBag = itemSlotBagContentWithWildcardMatch.SourceBag;
				if (sourceBag != null && sourceBag == this.itemstack)
				{
					return true;
				}
			}
			if (this.itemstack == null || !this.CanTake() || !sinkSlot.Empty || !sinkSlot.CanTakeFrom(this, 0))
			{
				return false;
			}
			int quantity = Math.Min(sinkSlot.GetRemainingSlotSpace(this.itemstack), op.RequestedQuantity);
			if (quantity <= 0 || quantity < this.itemstack.StackSize)
			{
				return false;
			}
			ItemStack movedStack = this.itemstack;
			sinkSlot.Itemstack = movedStack;
			movedQuantity = (op.MovedQuantity = (op.MovableQuantity = movedStack.StackSize));
			sinkSlot.OnItemSlotModified(sinkSlot.Itemstack);
			this.itemstack = null;
			this.OnItemSlotModified(movedStack);
			return true;
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0004E51C File Offset: 0x0004C71C
		[NullableContext(1)]
		protected void EmptyBag(ItemStack stack)
		{
			this.ResolveStack(stack);
			IHeldBag heldBag;
			if (stack == null)
			{
				heldBag = null;
			}
			else
			{
				Item item = stack.Item;
				heldBag = ((item != null) ? item.GetCollectibleInterface<IHeldBag>() : null);
			}
			IHeldBag bag = heldBag;
			try
			{
				if (bag != null && this.World != null)
				{
					IPlayer player = this.World.PlayerByUid(this.OwnerUUID);
					if (((player != null) ? player.Entity : null) != null)
					{
						ItemStack[] bagContent = bag.GetContents(stack, this.World);
						if (bagContent != null)
						{
							foreach (ItemStack bagContentStack in bagContent)
							{
								if (bagContentStack != null)
								{
									IWorldAccessor world = this.World;
									ItemStack itemStack = bagContentStack;
									IPlayer player2 = this.World.PlayerByUid(this.OwnerUUID);
									BlockPos blockPos;
									if (player2 == null)
									{
										blockPos = null;
									}
									else
									{
										EntityPlayer entity = player2.Entity;
										blockPos = ((entity != null) ? entity.SidedPos.AsBlockPos : null);
									}
									world.SpawnItemEntity(itemStack, blockPos, null);
								}
							}
						}
						bag.Clear(stack);
					}
				}
			}
			catch (Exception exception)
			{
				IWorldAccessor world2 = this.World;
				ICoreAPI api = (world2 != null) ? world2.Api : null;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(27, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error on emptying bag '");
				AssetLocation assetLocation;
				if (stack == null)
				{
					assetLocation = null;
				}
				else
				{
					CollectibleObject collectible = stack.Collectible;
					assetLocation = ((collectible != null) ? collectible.Code : null);
				}
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(assetLocation);
				defaultInterpolatedStringHandler.AppendLiteral("': \n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0004E668 File Offset: 0x0004C868
		protected void ModifyBackpackSlot()
		{
			InventoryPlayerBackpacks backpack = this.GetBackpackInventory();
			if (backpack != null)
			{
				backpack[0].MarkDirty();
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0004E68B File Offset: 0x0004C88B
		protected InventoryPlayerBackpacks GetBackpackInventory()
		{
			IWorldAccessor world = this.World;
			object obj;
			if (world == null)
			{
				obj = null;
			}
			else
			{
				IPlayer player = world.PlayerByUid(this.OwnerUUID);
				obj = ((player != null) ? player.InventoryManager.GetOwnInventory("backpack") : null);
			}
			return obj as InventoryPlayerBackpacks;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0004E6C0 File Offset: 0x0004C8C0
		protected void ResolveStack(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null && this.World != null && stack != null)
			{
				stack.ResolveBlockOrItem(this.World);
			}
		}
	}
}
