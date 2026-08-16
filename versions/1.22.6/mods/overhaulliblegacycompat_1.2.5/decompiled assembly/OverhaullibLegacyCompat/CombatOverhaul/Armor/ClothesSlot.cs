using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200013D RID: 317
	[NullableContext(2)]
	[Nullable(0)]
	public class ClothesSlot : ItemSlotCharacter, IClickableSlot
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x0004C8A1 File Offset: 0x0004AAA1
		// (set) Token: 0x06000F6E RID: 3950 RVA: 0x0004C8A9 File Offset: 0x0004AAA9
		public IWorldAccessor World { get; set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0004C8B2 File Offset: 0x0004AAB2
		// (set) Token: 0x06000F70 RID: 3952 RVA: 0x0004C8BA File Offset: 0x0004AABA
		public string OwnerUUID { get; set; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0004C8C3 File Offset: 0x0004AAC3
		// (set) Token: 0x06000F72 RID: 3954 RVA: 0x0004C8CB File Offset: 0x0004AACB
		public bool PreviouslyHeldBag { get; set; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0004C8D4 File Offset: 0x0004AAD4
		// (set) Token: 0x06000F74 RID: 3956 RVA: 0x0004C8DC File Offset: 0x0004AADC
		public int PreviousItemId { get; set; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000F75 RID: 3957 RVA: 0x0004C8E5 File Offset: 0x0004AAE5
		// (set) Token: 0x06000F76 RID: 3958 RVA: 0x0004C8ED File Offset: 0x0004AAED
		public int PreviousDurability { get; set; }

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0004C8F6 File Offset: 0x0004AAF6
		// (set) Token: 0x06000F78 RID: 3960 RVA: 0x0004C8FE File Offset: 0x0004AAFE
		public string PreviousColor { get; set; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x0004C907 File Offset: 0x0004AB07
		// (set) Token: 0x06000F7A RID: 3962 RVA: 0x0004C90F File Offset: 0x0004AB0F
		public bool PreviousEmpty { get; set; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x0004C918 File Offset: 0x0004AB18
		// (set) Token: 0x06000F7C RID: 3964 RVA: 0x0004C920 File Offset: 0x0004AB20
		public IClickableSlot.SlotClickedDelegate OnSlotClicked { get; set; }

		// Token: 0x06000F7D RID: 3965 RVA: 0x0004C929 File Offset: 0x0004AB29
		[NullableContext(1)]
		public ClothesSlot(EnumCharacterDressType type, InventoryBase inventory) : base(type, inventory)
		{
			this.MaxSlotStackSize = 1;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0004C93C File Offset: 0x0004AB3C
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

		// Token: 0x06000F7F RID: 3967 RVA: 0x0004C9E4 File Offset: 0x0004ABE4
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

		// Token: 0x06000F80 RID: 3968 RVA: 0x0004CA10 File Offset: 0x0004AC10
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

		// Token: 0x06000F81 RID: 3969 RVA: 0x0004CA34 File Offset: 0x0004AC34
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

		// Token: 0x06000F82 RID: 3970 RVA: 0x0004CA5C File Offset: 0x0004AC5C
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

		// Token: 0x06000F83 RID: 3971 RVA: 0x0004CAF8 File Offset: 0x0004ACF8
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

		// Token: 0x06000F84 RID: 3972 RVA: 0x0004CB38 File Offset: 0x0004AD38
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

		// Token: 0x06000F85 RID: 3973 RVA: 0x0004CC14 File Offset: 0x0004AE14
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

		// Token: 0x06000F86 RID: 3974 RVA: 0x0004CD60 File Offset: 0x0004AF60
		protected void ModifyBackpackSlot()
		{
			InventoryPlayerBackpacks backpack = this.GetBackpackInventory();
			if (backpack != null)
			{
				backpack[0].MarkDirty();
			}
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0004CD83 File Offset: 0x0004AF83
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

		// Token: 0x06000F88 RID: 3976 RVA: 0x0004CDB8 File Offset: 0x0004AFB8
		protected void ResolveStack(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null && this.World != null && stack != null)
			{
				stack.ResolveBlockOrItem(this.World);
			}
		}
	}
}
