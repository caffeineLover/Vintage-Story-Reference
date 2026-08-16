using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200013F RID: 319
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorSlot : ItemSlot, IClickableSlot
	{
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0004D47E File Offset: 0x0004B67E
		public ArmorType ArmorType { get; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0004D486 File Offset: 0x0004B686
		public ArmorType StoredArmoredType
		{
			get
			{
				return this.GetStoredArmorType();
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0004D48E File Offset: 0x0004B68E
		public DamageZone DamageZone
		{
			get
			{
				return this.ArmorType.Slots;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0004D49B File Offset: 0x0004B69B
		public ArmorLayers Layer
		{
			get
			{
				return this.ArmorType.Layers;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0004D4A8 File Offset: 0x0004B6A8
		[Obsolete("Use GerResists(DamageZone zone) instead")]
		public DamageResistData Resists
		{
			get
			{
				return this.GetResists();
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0004D4B0 File Offset: 0x0004B6B0
		public override int MaxSlotStackSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0004D4B3 File Offset: 0x0004B6B3
		public bool Available
		{
			get
			{
				return this._inventory.IsSlotAvailable(this.ArmorType);
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0004D4C6 File Offset: 0x0004B6C6
		public List<int> SlotsWithSameItem { get; } = new List<int>();

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0004D4CE File Offset: 0x0004B6CE
		// (set) Token: 0x06000FA4 RID: 4004 RVA: 0x0004D4D6 File Offset: 0x0004B6D6
		[Nullable(2)]
		public IWorldAccessor World { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x0004D4DF File Offset: 0x0004B6DF
		// (set) Token: 0x06000FA6 RID: 4006 RVA: 0x0004D4E7 File Offset: 0x0004B6E7
		[Nullable(2)]
		public string OwnerUUID { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x0004D4F0 File Offset: 0x0004B6F0
		// (set) Token: 0x06000FA8 RID: 4008 RVA: 0x0004D4F8 File Offset: 0x0004B6F8
		public bool PreviouslyHeldBag { get; set; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0004D501 File Offset: 0x0004B701
		// (set) Token: 0x06000FAA RID: 4010 RVA: 0x0004D509 File Offset: 0x0004B709
		public int PreviousItemId { get; set; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x0004D512 File Offset: 0x0004B712
		// (set) Token: 0x06000FAC RID: 4012 RVA: 0x0004D51A File Offset: 0x0004B71A
		public int PreviousDurability { get; set; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x0004D523 File Offset: 0x0004B723
		// (set) Token: 0x06000FAE RID: 4014 RVA: 0x0004D52B File Offset: 0x0004B72B
		[Nullable(2)]
		public IClickableSlot.SlotClickedDelegate OnSlotClicked { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x06000FAF RID: 4015 RVA: 0x0004D534 File Offset: 0x0004B734
		public ArmorSlot(InventoryBase inventory, ArmorType armorType) : base(inventory)
		{
			this.MaxSlotStackSize = 1;
			this.ArmorType = armorType;
			ArmorInventory armorInventory = inventory as ArmorInventory;
			if (armorInventory == null)
			{
				throw new Exception();
			}
			this._inventory = armorInventory;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0004D56C File Offset: 0x0004B76C
		public DamageResistData GetResists(DamageZone zone)
		{
			return this.GetResists(new ArmorType(this.ArmorType.Layers, zone));
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0004D588 File Offset: 0x0004B788
		public override bool CanHold(ItemSlot sourceSlot)
		{
			this.ResolveStack((sourceSlot != null) ? sourceSlot.Itemstack : null);
			CollectibleObject collectibleObject;
			if (sourceSlot == null)
			{
				collectibleObject = null;
			}
			else
			{
				ItemStack itemstack = sourceSlot.Itemstack;
				collectibleObject = ((itemstack != null) ? itemstack.Collectible : null);
			}
			CollectibleObject collectible = collectibleObject;
			IArmor armor;
			return !this.DrawUnavailable && collectible != null && base.CanHold(sourceSlot) && ArmorSlot.IsArmor(collectible, out armor) && armor != null && this._inventory.CanHoldArmorPiece(armor) && armor.ArmorType.Intersect(this.ArmorType);
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0004D608 File Offset: 0x0004B808
		public override void ActivateSlot(ItemSlot sourceSlot, ref ItemStackMoveOperation op)
		{
			IClickableSlot.SlotClickedDelegate onSlotClicked = this.OnSlotClicked;
			if (onSlotClicked != null && onSlotClicked(this, sourceSlot, ref op))
			{
				return;
			}
			this.ResolveStack((sourceSlot != null) ? sourceSlot.Itemstack : null);
			this.ResolveStack(base.Itemstack);
			if (base.Itemstack != null)
			{
				this.EmptyBag(base.Itemstack);
			}
			try
			{
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

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0004D6C4 File Offset: 0x0004B8C4
		[NullableContext(2)]
		public override ItemStack TakeOutWhole()
		{
			ItemStack itemStack = base.TakeOutWhole();
			if (itemStack != null)
			{
				this.EmptyBag(itemStack);
			}
			return itemStack;
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0004D6E4 File Offset: 0x0004B8E4
		[NullableContext(2)]
		public override ItemStack TakeOut(int quantity)
		{
			ItemStack stack = base.TakeOut(quantity);
			this.EmptyBag(stack);
			return stack;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0004D704 File Offset: 0x0004B904
		protected override void FlipWith(ItemSlot itemSlot)
		{
			this.ResolveStack((itemSlot != null) ? itemSlot.Itemstack : null);
			base.FlipWith(itemSlot);
			ItemStack stack = itemSlot.Itemstack;
			if (stack != null)
			{
				this.EmptyBag(stack);
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0004D73C File Offset: 0x0004B93C
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

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0004D888 File Offset: 0x0004BA88
		protected void ModifyBackpackSlot()
		{
			InventoryPlayerBackpacks backpack = this.GetBackpackInventory();
			if (backpack != null)
			{
				backpack[0].MarkDirty();
			}
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0004D8AB File Offset: 0x0004BAAB
		[NullableContext(2)]
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

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0004D8E0 File Offset: 0x0004BAE0
		[NullableContext(2)]
		protected void ResolveStack(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null && this.World != null && stack != null)
			{
				stack.ResolveBlockOrItem(this.World);
			}
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0004D908 File Offset: 0x0004BB08
		private ArmorType GetStoredArmorType()
		{
			ItemStack itemstack = base.Itemstack;
			IArmor armor;
			if (((itemstack != null) ? itemstack.Item : null) != null && ArmorSlot.IsArmor(base.Itemstack.Collectible, out armor) && armor != null)
			{
				return armor.ArmorType;
			}
			return ArmorType.Empty;
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0004D94C File Offset: 0x0004BB4C
		private DamageResistData GetResists(ArmorType armorType)
		{
			ItemStack itemstack = base.Itemstack;
			IModularArmor modularArmor;
			DamageResistData resists;
			if (((itemstack != null) ? itemstack.Item : null) != null && ArmorSlot.IsModularArmor(base.Itemstack.Collectible, out modularArmor) && modularArmor != null)
			{
				resists = modularArmor.GetResists(this, armorType);
			}
			else
			{
				ItemStack itemstack2 = base.Itemstack;
				IArmor armor;
				if (((itemstack2 != null) ? itemstack2.Item : null) == null || !ArmorSlot.IsArmor(base.Itemstack.Collectible, out armor) || armor == null)
				{
					return DamageResistData.Empty;
				}
				resists = armor.Resists;
			}
			return QuenchableStatUtil.ApplyArmorProtection(base.Itemstack, resists);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0004D9D8 File Offset: 0x0004BBD8
		private DamageResistData GetResists()
		{
			ItemStack itemstack = base.Itemstack;
			IModularArmor modularArmor;
			DamageResistData resists;
			if (((itemstack != null) ? itemstack.Item : null) != null && ArmorSlot.IsModularArmor(base.Itemstack.Collectible, out modularArmor) && modularArmor != null)
			{
				resists = modularArmor.GetResists(this, this.ArmorType);
			}
			else
			{
				ItemStack itemstack2 = base.Itemstack;
				IArmor armor;
				if (((itemstack2 != null) ? itemstack2.Item : null) == null || !ArmorSlot.IsArmor(base.Itemstack.Collectible, out armor) || armor == null)
				{
					return DamageResistData.Empty;
				}
				resists = armor.Resists;
			}
			return QuenchableStatUtil.ApplyArmorProtection(base.Itemstack, resists);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0004DA68 File Offset: 0x0004BC68
		private static bool IsArmor(CollectibleObject item, [Nullable(2)] out IArmor armor)
		{
			IArmor armorItem = item as IArmor;
			if (armorItem != null)
			{
				armor = armorItem;
				return true;
			}
			IArmor armorBehavior = Enumerable.FirstOrDefault<CollectibleBehavior>(item.CollectibleBehaviors, (CollectibleBehavior x) => x is IArmor) as IArmor;
			if (armorBehavior == null)
			{
				armor = null;
				return false;
			}
			armor = armorBehavior;
			return true;
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0004DAC0 File Offset: 0x0004BCC0
		private static bool IsModularArmor(CollectibleObject item, [Nullable(2)] out IModularArmor armor)
		{
			IModularArmor armorItem = item as IModularArmor;
			if (armorItem != null)
			{
				armor = armorItem;
				return true;
			}
			IModularArmor armorBehavior = Enumerable.FirstOrDefault<CollectibleBehavior>(item.CollectibleBehaviors, (CollectibleBehavior x) => x is IModularArmor) as IModularArmor;
			if (armorBehavior == null)
			{
				armor = null;
				return false;
			}
			armor = armorBehavior;
			return true;
		}

		// Token: 0x04000762 RID: 1890
		private readonly ArmorInventory _inventory;
	}
}
