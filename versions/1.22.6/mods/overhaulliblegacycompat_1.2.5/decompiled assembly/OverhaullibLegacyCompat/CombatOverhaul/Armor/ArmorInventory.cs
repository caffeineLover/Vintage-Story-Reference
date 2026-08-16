using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Integration;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200013B RID: 315
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorInventory : InventoryCharacter
	{
		// Token: 0x06000F34 RID: 3892 RVA: 0x0004AE64 File Offset: 0x00049064
		public ArmorInventory(string className, string playerUID, ICoreAPI api)
		{
			Dictionary<EnumCharacterDressType, string> dictionary = new Dictionary<EnumCharacterDressType, string>();
			dictionary.Add(4, "boots");
			dictionary.Add(5, "gloves");
			dictionary.Add(1, "cape");
			dictionary.Add(0, "hat");
			dictionary.Add(3, "trousers");
			dictionary.Add(2, "shirt");
			dictionary.Add(11, "pullover");
			dictionary.Add(6, "necklace");
			dictionary.Add(10, "bracers");
			dictionary.Add(9, "belt");
			dictionary.Add(7, "medal");
			dictionary.Add(8, "mask");
			dictionary.Add(12, "armorhead");
			dictionary.Add(13, "armorbody");
			dictionary.Add(14, "armorlegs");
			this._clothesSlotsIcons = dictionary;
			this._armorSlotsIcons = new Dictionary<ArmorType, string>();
			this._gearSlotsIcons = new Dictionary<string, string>();
			base..ctor(className, playerUID, api);
			this._api = api;
			ArmorInventory._disableVanillaArmorSlots = (this._api.ModLoader.IsModEnabled("combatoverhaul") || this._api.ModLoader.IsModEnabled("combatoverhaulfork"));
			this.FillArmorIconsDict(api);
			this._slots = base.GenEmptySlots(ArmorInventory._totalSlotsNumber);
			for (int index = 0; index < this._slots.Length; index++)
			{
				if (ArmorInventory._disableVanillaArmorSlots && ArmorInventory.IsVanillaArmorSlot(index))
				{
					if (this.ModifyColors)
					{
						this._slots[index].DrawUnavailable = true;
						this._slots[index].HexBackgroundColor = "#884444";
					}
					this._slots[index].MaxSlotStackSize = 0;
				}
			}
			this.FillSlotsOwnerAndWorld();
			this.RefreshGearSlots(false);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0004B02C File Offset: 0x0004922C
		public ArmorInventory(string inventoryID, ICoreAPI api)
		{
			Dictionary<EnumCharacterDressType, string> dictionary = new Dictionary<EnumCharacterDressType, string>();
			dictionary.Add(4, "boots");
			dictionary.Add(5, "gloves");
			dictionary.Add(1, "cape");
			dictionary.Add(0, "hat");
			dictionary.Add(3, "trousers");
			dictionary.Add(2, "shirt");
			dictionary.Add(11, "pullover");
			dictionary.Add(6, "necklace");
			dictionary.Add(10, "bracers");
			dictionary.Add(9, "belt");
			dictionary.Add(7, "medal");
			dictionary.Add(8, "mask");
			dictionary.Add(12, "armorhead");
			dictionary.Add(13, "armorbody");
			dictionary.Add(14, "armorlegs");
			this._clothesSlotsIcons = dictionary;
			this._armorSlotsIcons = new Dictionary<ArmorType, string>();
			this._gearSlotsIcons = new Dictionary<string, string>();
			base..ctor(inventoryID, api);
			this._api = api;
			ArmorInventory._disableVanillaArmorSlots = (this._api.ModLoader.IsModEnabled("combatoverhaul") || this._api.ModLoader.IsModEnabled("combatoverhaulfork"));
			this.FillArmorIconsDict(api);
			this._slots = base.GenEmptySlots(ArmorInventory._totalSlotsNumber);
			for (int index = 0; index < this._slots.Length; index++)
			{
				if (ArmorInventory._disableVanillaArmorSlots && ArmorInventory.IsVanillaArmorSlot(index))
				{
					if (this.ModifyColors)
					{
						this._slots[index].DrawUnavailable = true;
						this._slots[index].HexBackgroundColor = "#884444";
					}
					this._slots[index].MaxSlotStackSize = 0;
				}
			}
			this.FillSlotsOwnerAndWorld();
			this.RefreshGearSlots(false);
		}

		// Token: 0x170003C6 RID: 966
		public override ItemSlot this[int slotId]
		{
			get
			{
				return this._slots[slotId];
			}
			set
			{
				LoggerUtil.Warn(this.Api, this, "Armor slots cannot be set");
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0004B20E File Offset: 0x0004940E
		public override int Count
		{
			get
			{
				return ArmorInventory._totalSlotsNumber;
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000F39 RID: 3897 RVA: 0x0004B218 File Offset: 0x00049418
		// (remove) Token: 0x06000F3A RID: 3898 RVA: 0x0004B250 File Offset: 0x00049450
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event ArmorInventory.SlotModifiedDelegate OnSlotModified;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000F3B RID: 3899 RVA: 0x0004B288 File Offset: 0x00049488
		// (remove) Token: 0x06000F3C RID: 3900 RVA: 0x0004B2C0 File Offset: 0x000494C0
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event ArmorInventory.SlotModifiedDelegate OnArmorSlotModified;

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x0004B2F5 File Offset: 0x000494F5
		// (set) Token: 0x06000F3E RID: 3902 RVA: 0x0004B2FD File Offset: 0x000494FD
		public virtual bool ModifyColors { get; set; } = true;

		// Token: 0x06000F3F RID: 3903 RVA: 0x0004B308 File Offset: 0x00049508
		public override void FromTreeAttributes(ITreeAttribute tree)
		{
			this._slots = base.GenEmptySlots(ArmorInventory._totalSlotsNumber);
			for (int index = 0; index < this._slots.Length; index++)
			{
				ITreeAttribute treeAttribute = tree.GetTreeAttribute("slots");
				ItemStack itemStack = (treeAttribute != null) ? treeAttribute.GetItemstack(index.ToString() ?? "", null) : null;
				if (itemStack != null)
				{
					ICoreAPI api = this.Api;
					if (((api != null) ? api.World : null) != null)
					{
						itemStack.ResolveBlockOrItem(this.Api.World);
					}
					this._slots[index].Itemstack = itemStack;
				}
				if (ArmorInventory._disableVanillaArmorSlots && ArmorInventory.IsVanillaArmorSlot(index))
				{
					if (this.ModifyColors)
					{
						this._slots[index].DrawUnavailable = true;
						this._slots[index].HexBackgroundColor = "#884444";
					}
					this._slots[index].MaxSlotStackSize = 0;
				}
			}
			this.FillSlotsOwnerAndWorld();
			this.RefreshGearSlots(false);
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0004B3F4 File Offset: 0x000495F4
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			tree.SetInt("qslots", ArmorInventory._vanillaSlots);
			TreeAttribute treeAttribute = new TreeAttribute();
			for (int index = 0; index < this._slots.Length; index++)
			{
				if (this._slots[index].Itemstack != null)
				{
					treeAttribute.SetItemstack(index.ToString() ?? "", this._slots[index].Itemstack.Clone());
				}
			}
			tree["slots"] = treeAttribute;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0004B470 File Offset: 0x00049670
		public override void OnItemSlotModified(ItemSlot slot)
		{
			base.OnItemSlotModified(slot);
			if (this._api.Side == 1)
			{
				this.ClearArmorSlots();
			}
			GearSlot gearSlotModified = slot as GearSlot;
			if (gearSlotModified != null)
			{
				try
				{
					this.RefreshGearSlots(true);
				}
				catch (Exception exception)
				{
					ICoreAPI api = this.Api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(59, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Error while processing gear slots after slot was modified: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
					return;
				}
				int previousItemId = gearSlotModified.PreviousItemId;
				ItemStack itemstack = gearSlotModified.Itemstack;
				if (previousItemId != ((itemstack != null) ? itemstack.Id : 0) && this._api.Side == 2)
				{
					this._api.Event.EnqueueMainThreadTask(delegate()
					{
						GuiDialogInventory guiDialogInventoryInstance = GuiDialogPatches.GuiDialogInventoryInstance;
						if (guiDialogInventoryInstance == null)
						{
							return;
						}
						guiDialogInventoryInstance.ComposeGui(false);
					}, "");
				}
				gearSlotModified.PreviousEmpty = gearSlotModified.Empty;
			}
			ClothesSlot clothesSlot = slot as ClothesSlot;
			if (clothesSlot != null)
			{
				ItemStack itemstack2 = slot.Itemstack;
				int? num;
				if (itemstack2 == null)
				{
					num = default(int?);
				}
				else
				{
					Item item = itemstack2.Item;
					num = ((item != null) ? new int?(item.ItemId) : default(int?));
				}
				int? num2 = num;
				int currentItemId = num2.GetValueOrDefault();
				bool itemChanged = currentItemId != clothesSlot.PreviousItemId;
				clothesSlot.PreviousItemId = currentItemId;
				ItemStack itemstack3 = clothesSlot.Itemstack;
				object obj;
				if (itemstack3 == null)
				{
					obj = null;
				}
				else
				{
					CollectibleObject collectible = itemstack3.Collectible;
					obj = ((collectible != null) ? collectible.GetCollectibleInterface<IHeldBag>() : null);
				}
				bool containsBag = obj != null;
				if (itemChanged && (clothesSlot.PreviouslyHeldBag || containsBag))
				{
					this.ReloadBagInventory();
				}
				clothesSlot.PreviouslyHeldBag = containsBag;
				bool durabilityChanged = false;
				if (!itemChanged)
				{
					ItemStack itemstack4 = slot.Itemstack;
					int? num3;
					if (itemstack4 == null)
					{
						num3 = default(int?);
					}
					else
					{
						Item item2 = itemstack4.Item;
						num3 = ((item2 != null) ? new int?(item2.GetRemainingDurability(slot.Itemstack)) : default(int?));
					}
					num2 = num3;
					int currentDurability = num2.GetValueOrDefault();
					durabilityChanged = (currentDurability != clothesSlot.PreviousDurability);
					clothesSlot.PreviousDurability = currentDurability;
				}
				else
				{
					ItemStack itemstack5 = slot.Itemstack;
					int? num4;
					if (itemstack5 == null)
					{
						num4 = default(int?);
					}
					else
					{
						Item item3 = itemstack5.Item;
						num4 = ((item3 != null) ? new int?(item3.GetRemainingDurability(slot.Itemstack)) : default(int?));
					}
					num2 = num4;
					int currentDurability2 = num2.GetValueOrDefault();
					clothesSlot.PreviousDurability = currentDurability2;
				}
				ArmorInventory.SlotModifiedDelegate onSlotModified = this.OnSlotModified;
				if (onSlotModified != null)
				{
					onSlotModified(itemChanged, durabilityChanged, false);
				}
			}
			ArmorSlot armorSlot = slot as ArmorSlot;
			if (armorSlot != null)
			{
				ItemStack itemstack6 = slot.Itemstack;
				int? num5;
				if (itemstack6 == null)
				{
					num5 = default(int?);
				}
				else
				{
					Item item4 = itemstack6.Item;
					num5 = ((item4 != null) ? new int?(item4.ItemId) : default(int?));
				}
				int? num2 = num5;
				int currentItemId2 = num2.GetValueOrDefault();
				bool itemChanged2 = currentItemId2 != armorSlot.PreviousItemId;
				armorSlot.PreviousItemId = currentItemId2;
				ItemStack itemstack7 = armorSlot.Itemstack;
				object obj2;
				if (itemstack7 == null)
				{
					obj2 = null;
				}
				else
				{
					CollectibleObject collectible2 = itemstack7.Collectible;
					obj2 = ((collectible2 != null) ? collectible2.GetCollectibleInterface<IHeldBag>() : null);
				}
				bool containsBag2 = obj2 != null;
				if (itemChanged2 && (armorSlot.PreviouslyHeldBag || containsBag2))
				{
					this.ReloadBagInventory();
				}
				armorSlot.PreviouslyHeldBag = containsBag2;
				bool durabilityChanged2 = false;
				if (!itemChanged2)
				{
					ItemStack itemstack8 = slot.Itemstack;
					int? num6;
					if (itemstack8 == null)
					{
						num6 = default(int?);
					}
					else
					{
						Item item5 = itemstack8.Item;
						num6 = ((item5 != null) ? new int?(item5.GetRemainingDurability(slot.Itemstack)) : default(int?));
					}
					num2 = num6;
					int currentDurability3 = num2.GetValueOrDefault();
					durabilityChanged2 = (currentDurability3 != armorSlot.PreviousDurability);
					armorSlot.PreviousDurability = currentDurability3;
				}
				else
				{
					ItemStack itemstack9 = slot.Itemstack;
					int? num7;
					if (itemstack9 == null)
					{
						num7 = default(int?);
					}
					else
					{
						Item item6 = itemstack9.Item;
						num7 = ((item6 != null) ? new int?(item6.GetRemainingDurability(slot.Itemstack)) : default(int?));
					}
					num2 = num7;
					int currentDurability4 = num2.GetValueOrDefault();
					armorSlot.PreviousDurability = currentDurability4;
				}
				foreach (ArmorSlot armorSlot2 in Enumerable.OfType<ArmorSlot>(this))
				{
					if (!this.ModifyColors)
					{
						break;
					}
					if (!this.IsSlotAvailable(armorSlot2.ArmorType) && armorSlot2.Empty)
					{
						if (!(armorSlot2.HexBackgroundColor == "#5fbed4"))
						{
							armorSlot2.HexBackgroundColor = "#777777";
						}
					}
					else if (!(armorSlot2.HexBackgroundColor == "#5fbed4"))
					{
						armorSlot2.HexBackgroundColor = null;
					}
				}
				GuiDialogPatches.RecalculateArmorStatsForGui();
				ArmorInventory.SlotModifiedDelegate onSlotModified2 = this.OnSlotModified;
				if (onSlotModified2 != null)
				{
					onSlotModified2(itemChanged2, durabilityChanged2, true);
				}
				ArmorInventory.SlotModifiedDelegate onArmorSlotModified = this.OnArmorSlotModified;
				if (onArmorSlotModified != null)
				{
					onArmorSlotModified(itemChanged2, durabilityChanged2, true);
				}
			}
			IGearSlotModifiedListener gearSlotModifiedListener;
			if (slot == null)
			{
				gearSlotModifiedListener = null;
			}
			else
			{
				ItemStack itemstack10 = slot.Itemstack;
				if (itemstack10 == null)
				{
					gearSlotModifiedListener = null;
				}
				else
				{
					CollectibleObject collectible3 = itemstack10.Collectible;
					gearSlotModifiedListener = ((collectible3 != null) ? collectible3.GetCollectibleInterface<IGearSlotModifiedListener>() : null);
				}
			}
			IGearSlotModifiedListener listener = gearSlotModifiedListener;
			if (listener != null)
			{
				try
				{
					listener.OnSlotModified(slot, this, base.Owner as EntityPlayer);
				}
				catch (Exception exception2)
				{
					ICoreAPI api2 = this.Api;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(88, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Error while calling 'IGearSlotModifiedListener.OnSlotModified' on slot modified for '");
					AssetLocation assetLocation;
					if (slot == null)
					{
						assetLocation = null;
					}
					else
					{
						ItemStack itemstack11 = slot.Itemstack;
						if (itemstack11 == null)
						{
							assetLocation = null;
						}
						else
						{
							CollectibleObject collectible4 = itemstack11.Collectible;
							assetLocation = ((collectible4 != null) ? collectible4.Code : null);
						}
					}
					defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>(assetLocation);
					defaultInterpolatedStringHandler2.AppendLiteral("':\n");
					defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception2);
					LoggerUtil.Error(api2, this, defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			this.RefreshWearableStats();
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0004B9B4 File Offset: 0x00049BB4
		public override object ActivateSlot(int slotId, ItemSlot sourceSlot, ref ItemStackMoveOperation op)
		{
			object result = base.ActivateSlot(slotId, sourceSlot, ref op);
			if (slotId < ArmorInventory._clothesSlotsCount)
			{
				this.ReloadBagInventory();
			}
			return result;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0004B9CD File Offset: 0x00049BCD
		public override void DiscardAll()
		{
			base.DiscardAll();
			this.ReloadBagInventory();
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0004B9DB File Offset: 0x00049BDB
		public override void DropAll(Vec3d pos, int maxStackSize = 0)
		{
			base.DropAll(pos, maxStackSize);
			this.ReloadBagInventory();
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0004B9EC File Offset: 0x00049BEC
		public static int IndexFromArmorType(ArmorLayers layer, DamageZone zone)
		{
			int zonesCount = Enum.GetValues<DamageZone>().Length - 1;
			return ArmorInventory._vanillaSlots + ArmorInventory.IndexFromArmorLayer(layer) * zonesCount + ArmorInventory.IndexFromDamageZone(zone);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0004BA18 File Offset: 0x00049C18
		public static int IndexFromArmorType(ArmorType type)
		{
			return ArmorInventory.IndexFromArmorType(type.Layers, type.Slots);
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0004BA2C File Offset: 0x00049C2C
		public IEnumerable<ArmorType> GetSlotsBlockedSlot(ArmorType armorType)
		{
			return Enumerable.Select<KeyValuePair<ArmorType, ArmorSlot>, ArmorType>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(this._slotsByType, (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Value.ArmorType.Intersect(armorType)), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Key);
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0004BA81 File Offset: 0x00049C81
		public IEnumerable<int> GetSlotsBlockedSlotIndices(ArmorType armorType)
		{
			IEnumerable<ArmorType> slotsBlockedSlot = this.GetSlotsBlockedSlot(armorType);
			Func<ArmorType, int> func;
			if ((func = ArmorInventory.<>O.<0>__IndexFromArmorType) == null)
			{
				func = (ArmorInventory.<>O.<0>__IndexFromArmorType = new Func<ArmorType, int>(ArmorInventory.IndexFromArmorType));
			}
			return Enumerable.Select<ArmorType, int>(slotsBlockedSlot, func);
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0004BAAC File Offset: 0x00049CAC
		public IEnumerable<ArmorType> GetSlotBlockingSlots(ArmorType armorType)
		{
			return Enumerable.Select<KeyValuePair<ArmorType, ArmorSlot>, ArmorType>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(this._slotsByType, (KeyValuePair<ArmorType, ArmorSlot> entry) => !entry.Value.Empty), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Value.StoredArmoredType.Intersect(armorType)), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Key);
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0004BB25 File Offset: 0x00049D25
		public ArmorType GetSlotBlockingSlot(ArmorType armorType)
		{
			return Enumerable.FirstOrDefault<ArmorType>(this.GetSlotBlockingSlots(armorType), ArmorType.Empty);
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0004BB38 File Offset: 0x00049D38
		public ArmorType GetSlotBlockingSlot(ArmorLayers layer, DamageZone zone)
		{
			return this.GetSlotBlockingSlot(new ArmorType(layer, zone));
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0004BB47 File Offset: 0x00049D47
		public IEnumerable<int> GetSlotBlockingSlotsIndices(ArmorType armorType)
		{
			IEnumerable<ArmorType> slotBlockingSlots = this.GetSlotBlockingSlots(armorType);
			Func<ArmorType, int> func;
			if ((func = ArmorInventory.<>O.<0>__IndexFromArmorType) == null)
			{
				func = (ArmorInventory.<>O.<0>__IndexFromArmorType = new Func<ArmorType, int>(ArmorInventory.IndexFromArmorType));
			}
			return Enumerable.Select<ArmorType, int>(slotBlockingSlots, func);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0004BB70 File Offset: 0x00049D70
		public int GetSlotBlockingSlotIndex(ArmorType armorType)
		{
			return ArmorInventory.IndexFromArmorType(this.GetSlotBlockingSlot(armorType));
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0004BB7E File Offset: 0x00049D7E
		public int GetSlotBlockingSlotIndex(ArmorLayers layer, DamageZone zone)
		{
			return ArmorInventory.IndexFromArmorType(this.GetSlotBlockingSlot(new ArmorType(layer, zone)));
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0004BB94 File Offset: 0x00049D94
		public ArmorType GetFittingSlot(ArmorType armorType)
		{
			return Enumerable.FirstOrDefault<ArmorType>(Enumerable.Where<ArmorType>(this._slotsByType.Keys, (ArmorType slot) => slot.Intersect(armorType)), ArmorType.Empty);
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0004BBD4 File Offset: 0x00049DD4
		public int GetFittingSlotIndex(ArmorType armorType)
		{
			return ArmorInventory.IndexFromArmorType(this.GetFittingSlot(armorType));
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0004BBE4 File Offset: 0x00049DE4
		public bool IsSlotAvailable(ArmorType armorType)
		{
			return !Enumerable.Any<KeyValuePair<ArmorType, ArmorSlot>>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(this._slotsByType, (KeyValuePair<ArmorType, ArmorSlot> entry) => !entry.Value.Empty), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Value.StoredArmoredType.Intersect(armorType));
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0004BC3C File Offset: 0x00049E3C
		public bool IsSlotAvailable(ArmorLayers layer, DamageZone zone)
		{
			return this.IsSlotAvailable(new ArmorType(layer, zone));
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0004BC4B File Offset: 0x00049E4B
		public bool IsSlotAvailable(int index)
		{
			return this.IsSlotAvailable(ArmorInventory.ArmorTypeFromIndex(index));
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0004BC5C File Offset: 0x00049E5C
		public bool CanHoldArmorPiece(ArmorType armorType)
		{
			return !Enumerable.Any<KeyValuePair<ArmorType, ArmorSlot>>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(this._slotsByType, (KeyValuePair<ArmorType, ArmorSlot> entry) => !entry.Value.Empty), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Value.StoredArmoredType.Intersect(armorType));
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0004BCB4 File Offset: 0x00049EB4
		public bool CanHoldArmorPiece(IArmor armor)
		{
			return this.CanHoldArmorPiece(armor.ArmorType);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0004BCC2 File Offset: 0x00049EC2
		public bool CanHoldArmorPiece(ArmorLayers layer, DamageZone zone)
		{
			return this.CanHoldArmorPiece(new ArmorType(layer, zone));
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0004BCD4 File Offset: 0x00049ED4
		public IEnumerable<ArmorSlot> GetNotEmptyZoneSlots(DamageZone zone)
		{
			List<ArmorSlot> slots = new List<ArmorSlot>();
			ArmorSlot outer = this.GetSlotForArmorType(ArmorLayers.Outer, zone);
			ArmorSlot middle = this.GetSlotForArmorType(ArmorLayers.Middle, zone);
			ArmorSlot skin = this.GetSlotForArmorType(ArmorLayers.Skin, zone);
			if (outer != null && !outer.Empty)
			{
				slots.Add(outer);
			}
			if (middle != null && !middle.Empty && middle != outer)
			{
				slots.Add(middle);
			}
			if (skin != null && !skin.Empty && skin != outer && skin != middle)
			{
				slots.Add(skin);
			}
			return slots;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0004BD45 File Offset: 0x00049F45
		internal ItemSlot CreateNewSlot(int slotId)
		{
			return this.NewSlot(slotId);
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0004BD50 File Offset: 0x00049F50
		protected override ItemSlot NewSlot(int slotId)
		{
			if (slotId < ArmorInventory._clothesSlotsCount)
			{
				ClothesSlot slot = new ClothesSlot(slotId, this);
				this._clothesSlotsIcons.TryGetValue(slotId, ref slot.BackgroundIcon);
				return slot;
			}
			if (slotId < ArmorInventory._vanillaSlots)
			{
				if (ArmorInventory._disableVanillaArmorSlots)
				{
					return new ArmorSlot(this, ArmorType.Empty)
					{
						DrawUnavailable = true
					};
				}
				ClothesSlot slot2 = new ClothesSlot(slotId, this);
				this._clothesSlotsIcons.TryGetValue(slotId, ref slot2.BackgroundIcon);
				return slot2;
			}
			else
			{
				if (slotId < ArmorInventory._armorSlotsLastIndex)
				{
					ArmorType armorType = ArmorInventory.ArmorTypeFromIndex(slotId);
					ArmorSlot slot3 = new ArmorSlot(this, armorType);
					this._slotsByType[armorType] = slot3;
					this._armorSlotsIcons.TryGetValue(armorType, ref slot3.BackgroundIcon);
					return slot3;
				}
				if (slotId < ArmorInventory._gearSlotsLastIndex)
				{
					string slotType = ArmorInventory.GearSlotTypes[slotId - ArmorInventory._armorSlotsLastIndex];
					GearSlot slot4 = new GearSlot(slotType, this);
					this._gearSlots[slotType] = slot4;
					this._gearSlotsIcons.TryGetValue(slotType, ref slot4.BackgroundIcon);
					return slot4;
				}
				return new ItemSlot(this);
			}
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0004BE4C File Offset: 0x0004A04C
		private void FillSlotsOwnerAndWorld()
		{
			foreach (ItemSlot slot2 in this._slots)
			{
				ClothesSlot clothesSlot = slot2 as ClothesSlot;
				if (clothesSlot != null)
				{
					clothesSlot.OwnerUUID = this.playerUID;
					clothesSlot.World = this.Api.World;
				}
				else
				{
					ArmorSlot armorSlot = slot2 as ArmorSlot;
					if (armorSlot != null)
					{
						armorSlot.OwnerUUID = this.playerUID;
						armorSlot.World = this.Api.World;
					}
				}
			}
			EnumerableExtensions.Foreach<GearSlot>(Enumerable.OfType<GearSlot>(this._slots), delegate(GearSlot slot)
			{
				slot.SetParentSlot(this);
			});
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0004BEE4 File Offset: 0x0004A0E4
		internal void RefreshGearSlots(bool dropDisabledContents = false)
		{
			foreach (GearSlot gearSlot in Enumerable.OfType<GearSlot>(this._slots))
			{
				gearSlot.CheckParentSlot();
				if (dropDisabledContents && !gearSlot.Enabled && !gearSlot.Empty)
				{
					IInventory targetInv = base.Player.InventoryManager.GetOwnInventory("ground");
					gearSlot.TryPutInto(base.Player.Entity.Api.World, targetInv[0], 1);
					gearSlot.MarkDirty();
				}
			}
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0004BF88 File Offset: 0x0004A188
		private void FillArmorIconsDict(ICoreAPI api)
		{
			foreach (ArmorLayers layer in Enum.GetValues<ArmorLayers>())
			{
				foreach (DamageZone zone in Enum.GetValues<DamageZone>())
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(45, 2);
					defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:textures/gui/icons/armor-");
					defaultInterpolatedStringHandler.AppendFormatted<ArmorLayers>(layer);
					defaultInterpolatedStringHandler.AppendLiteral("-");
					defaultInterpolatedStringHandler.AppendFormatted<DamageZone>(zone);
					defaultInterpolatedStringHandler.AppendLiteral(".svg");
					string iconPath = defaultInterpolatedStringHandler.ToStringAndClear();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(22, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("combatoverhaul-armor-");
					defaultInterpolatedStringHandler2.AppendFormatted<ArmorLayers>(layer);
					defaultInterpolatedStringHandler2.AppendLiteral("-");
					defaultInterpolatedStringHandler2.AppendFormatted<DamageZone>(zone);
					string iconCode = defaultInterpolatedStringHandler2.ToStringAndClear();
					if (api.Assets.Exists(new AssetLocation(iconPath)))
					{
						this._armorSlotsIcons.Add(new ArmorType(layer, zone), iconCode);
					}
				}
			}
			foreach (string slotType in ArmorInventory.GearSlotTypes)
			{
				string iconPath2 = "combatoverhaul:textures/sloticons/gear-" + slotType + ".svg";
				string iconCode2 = "combatoverhaul:gear-" + slotType;
				if (api.Assets.Exists(new AssetLocation(iconPath2)))
				{
					this._gearSlotsIcons.TryAdd(slotType, iconCode2);
				}
			}
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0004C10C File Offset: 0x0004A30C
		internal static bool IsVanillaArmorSlot(int index)
		{
			return index >= ArmorInventory._clothesSlotsCount && index < ArmorInventory._clothesSlotsCount + 3;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0004C122 File Offset: 0x0004A322
		internal static bool IsModdedArmorSlot(int index)
		{
			return index >= ArmorInventory._vanillaSlots && index < ArmorInventory._armorSlotsLastIndex;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0004C138 File Offset: 0x0004A338
		internal static ArmorType ArmorTypeFromIndex(int index)
		{
			int zonesCount = Enum.GetValues<DamageZone>().Length - 1;
			if (index < ArmorInventory._vanillaSlots)
			{
				return ArmorType.Empty;
			}
			ArmorLayers layer = ArmorInventory.ArmorLayerFromIndex((index - ArmorInventory._vanillaSlots) / zonesCount);
			DamageZone zone = ArmorInventory.DamageZoneFromIndex(index - ArmorInventory._vanillaSlots - ArmorInventory.IndexFromArmorLayer(layer) * zonesCount);
			return new ArmorType(layer, zone);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0004C18C File Offset: 0x0004A38C
		private static ArmorLayers ArmorLayerFromIndex(int index)
		{
			ArmorLayers result;
			switch (index)
			{
			case 0:
				result = ArmorLayers.Skin;
				break;
			case 1:
				result = ArmorLayers.Middle;
				break;
			case 2:
				result = ArmorLayers.Outer;
				break;
			default:
				result = ArmorLayers.None;
				break;
			}
			return result;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x0004C1BC File Offset: 0x0004A3BC
		private static int IndexFromArmorLayer(ArmorLayers layer)
		{
			switch (layer)
			{
			case ArmorLayers.None:
				return 0;
			case ArmorLayers.Skin:
				return 0;
			case ArmorLayers.Middle:
				return 1;
			case ArmorLayers.Outer:
				return 2;
			}
			return 0;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0004C1F8 File Offset: 0x0004A3F8
		private static DamageZone DamageZoneFromIndex(int index)
		{
			DamageZone result;
			switch (index)
			{
			case 0:
				result = DamageZone.Head;
				break;
			case 1:
				result = DamageZone.Face;
				break;
			case 2:
				result = DamageZone.Neck;
				break;
			case 3:
				result = DamageZone.Torso;
				break;
			case 4:
				result = DamageZone.Arms;
				break;
			case 5:
				result = DamageZone.Hands;
				break;
			case 6:
				result = DamageZone.Legs;
				break;
			case 7:
				result = DamageZone.Feet;
				break;
			default:
				result = DamageZone.None;
				break;
			}
			return result;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0004C258 File Offset: 0x0004A458
		private static int IndexFromDamageZone(DamageZone index)
		{
			if (index <= DamageZone.Arms)
			{
				switch (index)
				{
				case DamageZone.Head:
					return 0;
				case DamageZone.Face:
					return 1;
				case DamageZone.Head | DamageZone.Face:
					break;
				case DamageZone.Neck:
					return 2;
				default:
					if (index == DamageZone.Torso)
					{
						return 3;
					}
					if (index == DamageZone.Arms)
					{
						return 4;
					}
					break;
				}
			}
			else
			{
				if (index == DamageZone.Hands)
				{
					return 5;
				}
				if (index == DamageZone.Legs)
				{
					return 6;
				}
				if (index == DamageZone.Feet)
				{
					return 7;
				}
			}
			return 0;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0004C2C4 File Offset: 0x0004A4C4
		[NullableContext(2)]
		private ArmorSlot GetSlotForArmorType(ArmorLayers layer, DamageZone zone)
		{
			ArmorType skinSlotType = this.GetSlotBlockingSlot(layer, zone);
			if (skinSlotType.Slots != DamageZone.None && skinSlotType.Layers != ArmorLayers.None)
			{
				return this._slotsByType[this.GetSlotBlockingSlot(layer, zone)];
			}
			return null;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0004C2FF File Offset: 0x0004A4FF
		[NullableContext(2)]
		private InventoryPlayerBackpacks GetBackpackInventory()
		{
			IPlayer player = base.Player;
			return ((player != null) ? player.InventoryManager.GetOwnInventory("backpack") : null) as InventoryPlayerBackpacks;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0004C324 File Offset: 0x0004A524
		private void ReloadBagInventory()
		{
			InventoryPlayerBackpacks backpack = this.GetBackpackInventory();
			if (backpack == null)
			{
				return;
			}
			InventoryPlayerBackPacksCombatOverhaul combatOverhaulBackpack = backpack as InventoryPlayerBackPacksCombatOverhaul;
			if (combatOverhaulBackpack != null)
			{
				combatOverhaulBackpack.ReloadBagInventory();
				return;
			}
			FieldInfo backpackBagInventory = ArmorInventory._backpackBagInventory;
			BagInventory bag = (BagInventory)((backpackBagInventory != null) ? backpackBagInventory.GetValue(backpack) : null);
			FieldInfo backpackBagSlots = ArmorInventory._backpackBagSlots;
			ItemSlot[] bagSlots = (ItemSlot[])((backpackBagSlots != null) ? backpackBagSlots.GetValue(backpack) : null);
			if (bag == null || bagSlots == null)
			{
				return;
			}
			try
			{
				bag.ReloadBagInventory(backpack, bagSlots);
			}
			catch (Exception exception)
			{
				ICoreAPI api = this.Api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error while trying to reload bag inventory: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0004C3D8 File Offset: 0x0004A5D8
		private void ClearArmorSlots()
		{
			if (!this._clearedArmorSlots && ArmorInventory._disableVanillaArmorSlots)
			{
				for (int index = ArmorInventory._clothesSlotsCount; index < ArmorInventory._vanillaSlots; index++)
				{
					ItemSlot slotToEmpty = this[index];
					if (!slotToEmpty.Empty)
					{
						try
						{
							IPlayer player = this._api.World.PlayerByUid(this.playerUID);
							Vec3d vec3d;
							if (player == null)
							{
								vec3d = null;
							}
							else
							{
								EntityPlayer entity = player.Entity;
								vec3d = ((entity != null) ? entity.ServerPos.XYZ.Clone() : null);
							}
							Vec3d playerPosition = vec3d;
							if (playerPosition == null)
							{
								return;
							}
							this._api.World.SpawnItemEntity(slotToEmpty.Itemstack.Clone(), playerPosition, new Vec3d(0.0, 0.1, 0.0));
							slotToEmpty.TakeOutWhole();
							slotToEmpty.MarkDirty();
						}
						catch (Exception exception)
						{
							ICoreAPI api = this._api;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
							defaultInterpolatedStringHandler..ctor(56, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Error while clearing vanilla armor slot ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(index);
							defaultInterpolatedStringHandler.AppendLiteral(" for player '");
							defaultInterpolatedStringHandler.AppendFormatted(this.playerUID);
							defaultInterpolatedStringHandler.AppendLiteral("':\n");
							defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
							LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
							return;
						}
					}
				}
				this._clearedArmorSlots = true;
			}
			if (!this._clearedArmorSlots && !ArmorInventory._disableVanillaArmorSlots)
			{
				for (int index2 = ArmorInventory._vanillaSlots; index2 < ArmorInventory._armorSlotsLastIndex; index2++)
				{
					ItemSlot slotToEmpty2 = this[index2];
					if (!slotToEmpty2.Empty)
					{
						try
						{
							IPlayer player2 = this._api.World.PlayerByUid(this.playerUID);
							Vec3d vec3d2;
							if (player2 == null)
							{
								vec3d2 = null;
							}
							else
							{
								EntityPlayer entity2 = player2.Entity;
								vec3d2 = ((entity2 != null) ? entity2.ServerPos.XYZ.Clone() : null);
							}
							Vec3d playerPosition2 = vec3d2;
							if (playerPosition2 == null)
							{
								return;
							}
							this._api.World.SpawnItemEntity(slotToEmpty2.Itemstack.Clone(), playerPosition2, new Vec3d(0.0, 0.1, 0.0));
							slotToEmpty2.TakeOutWhole();
							slotToEmpty2.MarkDirty();
						}
						catch (Exception exception2)
						{
							ICoreAPI api2 = this._api;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
							defaultInterpolatedStringHandler2..ctor(57, 3);
							defaultInterpolatedStringHandler2.AppendLiteral("Error while clearing overhaul armor slot ");
							defaultInterpolatedStringHandler2.AppendFormatted<int>(index2);
							defaultInterpolatedStringHandler2.AppendLiteral(" for player '");
							defaultInterpolatedStringHandler2.AppendFormatted(this.playerUID);
							defaultInterpolatedStringHandler2.AppendLiteral("':\n");
							defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception2);
							LoggerUtil.Error(api2, this, defaultInterpolatedStringHandler2.ToStringAndClear());
							return;
						}
					}
				}
				this._clearedArmorSlots = true;
			}
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0004C690 File Offset: 0x0004A890
		private void RefreshWearableStats()
		{
			EntityPlayer entityPlayer;
			if ((entityPlayer = (base.Owner as EntityPlayer)) == null)
			{
				IPlayer player = base.Player;
				entityPlayer = ((player != null) ? player.Entity : null);
			}
			EntityPlayer entityPlayer2 = entityPlayer;
			if (entityPlayer2 == null)
			{
				return;
			}
			WearableStatsBehavior behavior = entityPlayer2.GetBehavior<WearableStatsBehavior>();
			if (behavior == null)
			{
				return;
			}
			behavior.RefreshStatsNow();
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0004C6C8 File Offset: 0x0004A8C8
		// Note: this type is marked as 'beforefieldinit'.
		unsafe static ArmorInventory()
		{
			int num = 17;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount<string>(list, num);
			Span<string> span = CollectionsMarshal.AsSpan<string>(list);
			*span[0] = "headgear";
			*span[1] = "frontgear";
			*span[2] = "backgear";
			*span[3] = "rightshouldergear";
			*span[4] = "leftshouldergear";
			*span[5] = "waistgear";
			*span[6] = "miscgear";
			*span[7] = "miscgear";
			*span[8] = "miscgear";
			*span[9] = "addBeltLeft";
			*span[10] = "addBeltRight";
			*span[11] = "addBeltBack";
			*span[12] = "addBeltFront";
			*span[13] = "addBackpack1";
			*span[14] = "addBackpack2";
			*span[15] = "addBackpack3";
			*span[16] = "addBackpack4";
			ArmorInventory.GearSlotTypes = list;
			ArmorInventory._moddedArmorSlotsCount = (Enum.GetValues<ArmorLayers>().Length - 1) * (Enum.GetValues<DamageZone>().Length - 1);
			ArmorInventory._clothesSlotsCount = Enum.GetValues<EnumCharacterDressType>().Length - 3 - 1;
			ArmorInventory._vanillaSlots = ArmorInventory._clothesSlotsCount + 3;
			ArmorInventory._armorSlotsLastIndex = ArmorInventory._vanillaSlots + ArmorInventory._moddedArmorSlotsCount;
			ArmorInventory._gearSlotsCount = ArmorInventory.GearSlotTypes.Count;
			ArmorInventory._gearSlotsLastIndex = ArmorInventory._armorSlotsLastIndex + ArmorInventory._gearSlotsCount;
			ArmorInventory._totalSlotsNumber = ArmorInventory._clothesSlotsCount + 3 + ArmorInventory._moddedArmorSlotsCount + ArmorInventory._gearSlotsCount;
			ArmorInventory._backpackBagInventory = typeof(InventoryPlayerBackpacks).GetField("bagInv", 36);
			ArmorInventory._backpackBagSlots = typeof(InventoryPlayerBackpacks).GetField("bagSlots", 36);
		}

		// Token: 0x0400073A RID: 1850
		public static readonly List<string> GearSlotTypes;

		// Token: 0x0400073B RID: 1851
		internal ItemSlot[] _slots;

		// Token: 0x0400073C RID: 1852
		internal readonly Dictionary<string, GearSlot> _gearSlots = new Dictionary<string, GearSlot>();

		// Token: 0x0400073D RID: 1853
		internal readonly Dictionary<ArmorType, ArmorSlot> _slotsByType = new Dictionary<ArmorType, ArmorSlot>();

		// Token: 0x0400073E RID: 1854
		internal readonly Dictionary<EnumCharacterDressType, string> _clothesSlotsIcons;

		// Token: 0x0400073F RID: 1855
		internal readonly Dictionary<ArmorType, string> _armorSlotsIcons;

		// Token: 0x04000740 RID: 1856
		internal readonly Dictionary<string, string> _gearSlotsIcons;

		// Token: 0x04000741 RID: 1857
		internal const int _clothesArmorSlots = 3;

		// Token: 0x04000742 RID: 1858
		internal static readonly int _moddedArmorSlotsCount;

		// Token: 0x04000743 RID: 1859
		internal static readonly int _clothesSlotsCount;

		// Token: 0x04000744 RID: 1860
		internal static readonly int _vanillaSlots;

		// Token: 0x04000745 RID: 1861
		internal static readonly int _armorSlotsLastIndex;

		// Token: 0x04000746 RID: 1862
		internal static readonly int _gearSlotsCount;

		// Token: 0x04000747 RID: 1863
		internal static readonly int _gearSlotsLastIndex;

		// Token: 0x04000748 RID: 1864
		internal static readonly int _totalSlotsNumber;

		// Token: 0x04000749 RID: 1865
		[Nullable(2)]
		internal static readonly FieldInfo _backpackBagInventory;

		// Token: 0x0400074A RID: 1866
		[Nullable(2)]
		internal static readonly FieldInfo _backpackBagSlots;

		// Token: 0x0400074B RID: 1867
		internal readonly ICoreAPI _api;

		// Token: 0x0400074C RID: 1868
		internal static bool _disableVanillaArmorSlots;

		// Token: 0x0400074D RID: 1869
		internal bool _clearedArmorSlots;

		// Token: 0x02000283 RID: 643
		// (Invoke) Token: 0x06001774 RID: 6004
		[NullableContext(0)]
		public delegate void SlotModifiedDelegate(bool itemChanged, bool durabilityChanged, bool isArmorSlot);

		// Token: 0x02000284 RID: 644
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D3B RID: 3387
			[Nullable(0)]
			public static Func<ArmorType, int> <0>__IndexFromArmorType;
		}
	}
}
