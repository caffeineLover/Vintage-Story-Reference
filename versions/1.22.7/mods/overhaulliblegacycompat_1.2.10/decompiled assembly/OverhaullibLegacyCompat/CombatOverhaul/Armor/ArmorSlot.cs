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
	// Token: 0x0200014B RID: 331
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorSlot : ItemSlot, IClickableSlot
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600100F RID: 4111 RVA: 0x0004ED86 File Offset: 0x0004CF86
		public ArmorType ArmorType { get; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x0004ED8E File Offset: 0x0004CF8E
		public ArmorType StoredArmoredType
		{
			get
			{
				return this.GetStoredArmorType();
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001011 RID: 4113 RVA: 0x0004ED96 File Offset: 0x0004CF96
		public DamageZone DamageZone
		{
			get
			{
				return this.ArmorType.Slots;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001012 RID: 4114 RVA: 0x0004EDA3 File Offset: 0x0004CFA3
		public ArmorLayers Layer
		{
			get
			{
				return this.ArmorType.Layers;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x0004EDB0 File Offset: 0x0004CFB0
		[Obsolete("Use GerResists(DamageZone zone) instead")]
		public DamageResistData Resists
		{
			get
			{
				return this.GetResists();
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x0004EDB8 File Offset: 0x0004CFB8
		public override int MaxSlotStackSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x0004EDBB File Offset: 0x0004CFBB
		public bool Available
		{
			get
			{
				return this._inventory.IsSlotAvailable(this.ArmorType);
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001016 RID: 4118 RVA: 0x0004EDCE File Offset: 0x0004CFCE
		public List<int> SlotsWithSameItem { get; } = new List<int>();

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001017 RID: 4119 RVA: 0x0004EDD6 File Offset: 0x0004CFD6
		// (set) Token: 0x06001018 RID: 4120 RVA: 0x0004EDDE File Offset: 0x0004CFDE
		[Nullable(2)]
		public IWorldAccessor World { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001019 RID: 4121 RVA: 0x0004EDE7 File Offset: 0x0004CFE7
		// (set) Token: 0x0600101A RID: 4122 RVA: 0x0004EDEF File Offset: 0x0004CFEF
		[Nullable(2)]
		public string OwnerUUID { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x0004EDF8 File Offset: 0x0004CFF8
		// (set) Token: 0x0600101C RID: 4124 RVA: 0x0004EE00 File Offset: 0x0004D000
		public bool PreviouslyHeldBag { get; set; }

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x0004EE09 File Offset: 0x0004D009
		// (set) Token: 0x0600101E RID: 4126 RVA: 0x0004EE11 File Offset: 0x0004D011
		public int PreviousItemId { get; set; }

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x0600101F RID: 4127 RVA: 0x0004EE1A File Offset: 0x0004D01A
		// (set) Token: 0x06001020 RID: 4128 RVA: 0x0004EE22 File Offset: 0x0004D022
		public int PreviousDurability { get; set; }

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001021 RID: 4129 RVA: 0x0004EE2B File Offset: 0x0004D02B
		// (set) Token: 0x06001022 RID: 4130 RVA: 0x0004EE33 File Offset: 0x0004D033
		[Nullable(2)]
		public IClickableSlot.SlotClickedDelegate OnSlotClicked { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x06001023 RID: 4131 RVA: 0x0004EE3C File Offset: 0x0004D03C
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

		// Token: 0x06001024 RID: 4132 RVA: 0x0004EE74 File Offset: 0x0004D074
		public DamageResistData GetResists(DamageZone zone)
		{
			return this.GetResists(new ArmorType(this.ArmorType.Layers, zone));
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0004EE90 File Offset: 0x0004D090
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

		// Token: 0x06001026 RID: 4134 RVA: 0x0004EF10 File Offset: 0x0004D110
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

		// Token: 0x06001027 RID: 4135 RVA: 0x0004EFCC File Offset: 0x0004D1CC
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

		// Token: 0x06001028 RID: 4136 RVA: 0x0004EFEC File Offset: 0x0004D1EC
		[NullableContext(2)]
		public override ItemStack TakeOut(int quantity)
		{
			ItemStack stack = base.TakeOut(quantity);
			this.EmptyBag(stack);
			return stack;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0004F00C File Offset: 0x0004D20C
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

		// Token: 0x0600102A RID: 4138 RVA: 0x0004F044 File Offset: 0x0004D244
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

		// Token: 0x0600102B RID: 4139 RVA: 0x0004F190 File Offset: 0x0004D390
		protected void ModifyBackpackSlot()
		{
			InventoryPlayerBackpacks backpack = this.GetBackpackInventory();
			if (backpack != null)
			{
				backpack[0].MarkDirty();
			}
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0004F1B3 File Offset: 0x0004D3B3
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

		// Token: 0x0600102D RID: 4141 RVA: 0x0004F1E8 File Offset: 0x0004D3E8
		[NullableContext(2)]
		protected void ResolveStack(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null && this.World != null && stack != null)
			{
				stack.ResolveBlockOrItem(this.World);
			}
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0004F210 File Offset: 0x0004D410
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

		// Token: 0x0600102F RID: 4143 RVA: 0x0004F254 File Offset: 0x0004D454
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

		// Token: 0x06001030 RID: 4144 RVA: 0x0004F2E0 File Offset: 0x0004D4E0
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

		// Token: 0x06001031 RID: 4145 RVA: 0x0004F370 File Offset: 0x0004D570
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

		// Token: 0x06001032 RID: 4146 RVA: 0x0004F3C8 File Offset: 0x0004D5C8
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

		// Token: 0x04000788 RID: 1928
		private readonly ArmorInventory _inventory;
	}
}
