using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200013E RID: 318
	[NullableContext(1)]
	[Nullable(0)]
	public class GearSlot : ClothesSlot
	{
		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0004CDE0 File Offset: 0x0004AFE0
		// (set) Token: 0x06000F8A RID: 3978 RVA: 0x0004CDE8 File Offset: 0x0004AFE8
		public string SlotType { get; set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0004CDF1 File Offset: 0x0004AFF1
		// (set) Token: 0x06000F8C RID: 3980 RVA: 0x0004CDF9 File Offset: 0x0004AFF9
		public bool Enabled { get; set; } = true;

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0004CE02 File Offset: 0x0004B002
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x0004CE0A File Offset: 0x0004B00A
		[Nullable(2)]
		public ItemSlot ParentSlot { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0004CE13 File Offset: 0x0004B013
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x0004CE1B File Offset: 0x0004B01B
		[Nullable(2)]
		public SlotConfig Config { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x06000F91 RID: 3985 RVA: 0x0004CE24 File Offset: 0x0004B024
		public GearSlot(string slotType, InventoryBase inventory) : base(-1, inventory)
		{
			this.SlotType = slotType;
			if (slotType.StartsWith("add"))
			{
				this.Enabled = false;
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0004CE50 File Offset: 0x0004B050
		public virtual void SetParentSlot(InventoryBase inventory)
		{
			string slotType = this.SlotType;
			if (slotType != null)
			{
				int length = slotType.Length;
				if (length != 11)
				{
					if (length != 12)
					{
						return;
					}
					char c = slotType.get_Chars(11);
					switch (c)
					{
					case '1':
						if (!(slotType == "addBackpack1"))
						{
							return;
						}
						break;
					case '2':
						if (!(slotType == "addBackpack2"))
						{
							return;
						}
						break;
					case '3':
						if (!(slotType == "addBackpack3"))
						{
							return;
						}
						break;
					case '4':
						if (!(slotType == "addBackpack4"))
						{
							return;
						}
						break;
					default:
						if (c != 't')
						{
							return;
						}
						if (!(slotType == "addBeltRight") && !(slotType == "addBeltFront"))
						{
							return;
						}
						goto IL_C9;
					}
					this.ParentSlot = Enumerable.FirstOrDefault<GearSlot>(Enumerable.OfType<GearSlot>(inventory), (GearSlot slot) => slot.SlotType == "backgear");
					return;
				}
				else
				{
					char c = slotType.get_Chars(7);
					if (c != 'B')
					{
						if (c != 'L')
						{
							return;
						}
						if (!(slotType == "addBeltLeft"))
						{
							return;
						}
					}
					else if (!(slotType == "addBeltBack"))
					{
						return;
					}
				}
				IL_C9:
				this.ParentSlot = Enumerable.FirstOrDefault<GearSlot>(Enumerable.OfType<GearSlot>(inventory), (GearSlot slot) => slot.SlotType == "waistgear");
				return;
			}
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0004CF88 File Offset: 0x0004B188
		public virtual IEnumerable<ItemSlot> GetChildSlots(InventoryBase inventory)
		{
			string slotType = this.SlotType;
			string[] childSlotCodes2;
			if (!(slotType == "waistgear"))
			{
				if (!(slotType == "backgear"))
				{
					childSlotCodes2 = Array.Empty<string>();
				}
				else
				{
					childSlotCodes2 = new string[]
					{
						"addBackpack1",
						"addBackpack2",
						"addBackpack3",
						"addBackpack4"
					};
				}
			}
			else
			{
				childSlotCodes2 = new string[]
				{
					"addBeltLeft",
					"addBeltRight",
					"addBeltBack",
					"addBeltFront"
				};
			}
			string[] childSlotCodes = childSlotCodes2;
			return Enumerable.Where<GearSlot>(Enumerable.OfType<GearSlot>(inventory), (GearSlot slot) => ArrayExtensions.Contains<string>(childSlotCodes, slot.SlotType));
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0004D034 File Offset: 0x0004B234
		public virtual void CheckParentSlot()
		{
			if (this.ParentSlot == null)
			{
				return;
			}
			ItemStack itemstack = this.ParentSlot.Itemstack;
			IEnableAdditionalSlots enableAdditionalSlots;
			if (itemstack == null)
			{
				enableAdditionalSlots = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				enableAdditionalSlots = ((collectible != null) ? collectible.GetCollectibleInterface<IEnableAdditionalSlots>() : null);
			}
			IEnableAdditionalSlots parent = enableAdditionalSlots;
			if (this.ParentSlot.Itemstack == null || parent == null)
			{
				ArmorInventory armorInventory = this.inventory as ArmorInventory;
				if (armorInventory != null && armorInventory.ModifyColors)
				{
					this.HexBackgroundColor = "#999999";
					base.PreviousColor = this.HexBackgroundColor;
					this.BackgroundIcon = null;
				}
				this.Config = null;
				this.Enabled = false;
				return;
			}
			this.BackgroundIcon = parent.GetIcon(this.ParentSlot.Itemstack, this.inventory, this.SlotType);
			this.Enabled = parent.GetIfEnabled(this.ParentSlot.Itemstack, this.inventory, this.SlotType);
			this.Config = parent.GetConfig(this.ParentSlot.Itemstack, this.inventory, this.SlotType);
			ArmorInventory armorInventory2 = this.inventory as ArmorInventory;
			if (armorInventory2 != null && armorInventory2.ModifyColors)
			{
				this.HexBackgroundColor = (this.Enabled ? null : "#999999");
				base.PreviousColor = this.HexBackgroundColor;
			}
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0004D168 File Offset: 0x0004B368
		public override bool CanTake()
		{
			if (Enumerable.Any<ItemSlot>(this.GetChildSlots(this.inventory), (ItemSlot slot) => !slot.Empty))
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
					coreClientAPI.TriggerIngameError(this, "canttakeout", "Cannot take out item. Some other items attached to it.");
				}
				return false;
			}
			return base.CanTake();
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0004D1F9 File Offset: 0x0004B3F9
		public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = 0)
		{
			return this.CanHold(sourceSlot) && base.CanTakeFrom(sourceSlot, priority) && this.CanTake();
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0004D218 File Offset: 0x0004B418
		public override bool CanHold(ItemSlot sourceSlot)
		{
			base.ResolveStack((sourceSlot != null) ? sourceSlot.Itemstack : null);
			return this.Enabled && GearSlot.IsGearType((sourceSlot != null) ? sourceSlot.Itemstack : null, this.SlotType) && this.CanHoldConfig(sourceSlot);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0004D258 File Offset: 0x0004B458
		[NullableContext(2)]
		public virtual bool CanHoldConfig(ItemSlot sourceSlot)
		{
			base.ResolveStack((sourceSlot != null) ? sourceSlot.Itemstack : null);
			if (this.Config == null || sourceSlot == null)
			{
				return true;
			}
			ItemStack itemstack = sourceSlot.Itemstack;
			AssetLocation assetLocation;
			if (itemstack == null)
			{
				assetLocation = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				assetLocation = ((collectible != null) ? collectible.Code : null);
			}
			if (assetLocation == null)
			{
				return false;
			}
			bool flag = WildcardUtil.Match(this.Config.CanHoldWildcards, sourceSlot.Itemstack.Collectible.Code.Path);
			bool matchWithDomain = WildcardUtil.Match(this.Config.CanHoldWildcards, sourceSlot.Itemstack.Collectible.Code.ToString());
			bool matchWithTags = false;
			ItemStack itemstack2 = sourceSlot.Itemstack;
			if (((itemstack2 != null) ? itemstack2.Item : null) != null && this.Config.CanHoldItemTags.Length != 0)
			{
				matchWithTags = ItemTagRule.ContainsAllFromAtLeastOne(sourceSlot.Itemstack.Item.Tags, this.Config.CanHoldItemTags);
			}
			ItemStack itemstack3 = sourceSlot.Itemstack;
			if (((itemstack3 != null) ? itemstack3.Block : null) != null && this.Config.CanHoldBlockTags.Length != 0 && !matchWithTags)
			{
				matchWithTags = BlockTagRule.ContainsAllFromAtLeastOne(sourceSlot.Itemstack.Block.Tags, this.Config.CanHoldBlockTags);
			}
			bool matchWithAttributes = this.Config.MatchesItemAttributes(sourceSlot.Itemstack);
			return flag || matchWithDomain || matchWithTags || matchWithAttributes;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0004D39A File Offset: 0x0004B59A
		public static bool IsGearType([Nullable(2)] IItemStack itemStack, string gearType)
		{
			return GearSlot.IsGearType((itemStack != null) ? itemStack.Collectible : null, gearType);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0004D3B0 File Offset: 0x0004B5B0
		public static bool IsGearType([Nullable(2)] CollectibleObject collectible, string gearType)
		{
			if (((collectible != null) ? collectible.Attributes : null) == null)
			{
				return false;
			}
			string stackDressType = collectible.Attributes["clothescategory"].AsString(null) ?? collectible.Attributes["attachableToEntity"]["categoryCode"].AsString(null);
			string[] stackDressTypes = collectible.Attributes["clothescategories"].AsObject<string[]>(null) ?? collectible.Attributes["attachableToEntity"]["categoryCodes"].AsObject<string[]>(null);
			bool flag = stackDressType != null && string.Equals(stackDressType, gearType, 3);
			bool multipleTypes = stackDressTypes != null && ArrayExtensions.Contains<string>(stackDressTypes, (string value) => string.Equals(value, gearType, 3));
			return flag || multipleTypes;
		}
	}
}
