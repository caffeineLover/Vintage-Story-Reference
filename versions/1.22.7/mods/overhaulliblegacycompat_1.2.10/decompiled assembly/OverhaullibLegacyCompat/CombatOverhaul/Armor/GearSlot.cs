using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200014A RID: 330
	[NullableContext(1)]
	[Nullable(0)]
	public class GearSlot : ClothesSlot
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0004E6E8 File Offset: 0x0004C8E8
		// (set) Token: 0x06000FFE RID: 4094 RVA: 0x0004E6F0 File Offset: 0x0004C8F0
		public string SlotType { get; set; }

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000FFF RID: 4095 RVA: 0x0004E6F9 File Offset: 0x0004C8F9
		// (set) Token: 0x06001000 RID: 4096 RVA: 0x0004E701 File Offset: 0x0004C901
		public bool Enabled { get; set; } = true;

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x0004E70A File Offset: 0x0004C90A
		// (set) Token: 0x06001002 RID: 4098 RVA: 0x0004E712 File Offset: 0x0004C912
		[Nullable(2)]
		public ItemSlot ParentSlot { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x0004E71B File Offset: 0x0004C91B
		// (set) Token: 0x06001004 RID: 4100 RVA: 0x0004E723 File Offset: 0x0004C923
		[Nullable(2)]
		public SlotConfig Config { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x06001005 RID: 4101 RVA: 0x0004E72C File Offset: 0x0004C92C
		public GearSlot(string slotType, InventoryBase inventory) : base(-1, inventory)
		{
			this.SlotType = slotType;
			if (slotType.StartsWith("add"))
			{
				this.Enabled = false;
			}
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0004E758 File Offset: 0x0004C958
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

		// Token: 0x06001007 RID: 4103 RVA: 0x0004E890 File Offset: 0x0004CA90
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

		// Token: 0x06001008 RID: 4104 RVA: 0x0004E93C File Offset: 0x0004CB3C
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

		// Token: 0x06001009 RID: 4105 RVA: 0x0004EA70 File Offset: 0x0004CC70
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

		// Token: 0x0600100A RID: 4106 RVA: 0x0004EB01 File Offset: 0x0004CD01
		public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = 0)
		{
			return this.CanHold(sourceSlot) && base.CanTakeFrom(sourceSlot, priority) && this.CanTake();
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0004EB20 File Offset: 0x0004CD20
		public override bool CanHold(ItemSlot sourceSlot)
		{
			base.ResolveStack((sourceSlot != null) ? sourceSlot.Itemstack : null);
			return this.Enabled && GearSlot.IsGearType((sourceSlot != null) ? sourceSlot.Itemstack : null, this.SlotType) && this.CanHoldConfig(sourceSlot);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0004EB60 File Offset: 0x0004CD60
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

		// Token: 0x0600100D RID: 4109 RVA: 0x0004ECA2 File Offset: 0x0004CEA2
		public static bool IsGearType([Nullable(2)] IItemStack itemStack, string gearType)
		{
			return GearSlot.IsGearType((itemStack != null) ? itemStack.Collectible : null, gearType);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0004ECB8 File Offset: 0x0004CEB8
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
