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
	// Token: 0x02000147 RID: 327
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorInventory : InventoryCharacter
	{
		// Token: 0x06000FA8 RID: 4008 RVA: 0x0004C764 File Offset: 0x0004A964
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

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0004C92C File Offset: 0x0004AB2C
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

		// Token: 0x170003E8 RID: 1000
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

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x0004CB0E File Offset: 0x0004AD0E
		public override int Count
		{
			get
			{
				return ArmorInventory._totalSlotsNumber;
			}
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000FAD RID: 4013 RVA: 0x0004CB18 File Offset: 0x0004AD18
		// (remove) Token: 0x06000FAE RID: 4014 RVA: 0x0004CB50 File Offset: 0x0004AD50
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event ArmorInventory.SlotModifiedDelegate OnSlotModified;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000FAF RID: 4015 RVA: 0x0004CB88 File Offset: 0x0004AD88
		// (remove) Token: 0x06000FB0 RID: 4016 RVA: 0x0004CBC0 File Offset: 0x0004ADC0
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event ArmorInventory.SlotModifiedDelegate OnArmorSlotModified;

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0004CBF5 File Offset: 0x0004ADF5
		// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x0004CBFD File Offset: 0x0004ADFD
		public virtual bool ModifyColors { get; set; } = true;

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0004CC08 File Offset: 0x0004AE08
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

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0004CCF4 File Offset: 0x0004AEF4
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

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0004CD70 File Offset: 0x0004AF70
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

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0004D2B4 File Offset: 0x0004B4B4
		public override object ActivateSlot(int slotId, ItemSlot sourceSlot, ref ItemStackMoveOperation op)
		{
			object result = base.ActivateSlot(slotId, sourceSlot, ref op);
			if (slotId < ArmorInventory._clothesSlotsCount)
			{
				this.ReloadBagInventory();
			}
			return result;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0004D2CD File Offset: 0x0004B4CD
		public override void DiscardAll()
		{
			base.DiscardAll();
			this.ReloadBagInventory();
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0004D2DB File Offset: 0x0004B4DB
		public override void DropAll(Vec3d pos, int maxStackSize = 0)
		{
			base.DropAll(pos, maxStackSize);
			this.ReloadBagInventory();
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0004D2EC File Offset: 0x0004B4EC
		public static int IndexFromArmorType(ArmorLayers layer, DamageZone zone)
		{
			int zonesCount = Enum.GetValues<DamageZone>().Length - 1;
			return ArmorInventory._vanillaSlots + ArmorInventory.IndexFromArmorLayer(layer) * zonesCount + ArmorInventory.IndexFromDamageZone(zone);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0004D318 File Offset: 0x0004B518
		public static int IndexFromArmorType(ArmorType type)
		{
			return ArmorInventory.IndexFromArmorType(type.Layers, type.Slots);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0004D32C File Offset: 0x0004B52C
		public IEnumerable<ArmorType> GetSlotsBlockedSlot(ArmorType armorType)
		{
			return Enumerable.Select<KeyValuePair<ArmorType, ArmorSlot>, ArmorType>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(this._slotsByType, (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Value.ArmorType.Intersect(armorType)), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Key);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0004D381 File Offset: 0x0004B581
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

		// Token: 0x06000FBD RID: 4029 RVA: 0x0004D3AC File Offset: 0x0004B5AC
		public IEnumerable<ArmorType> GetSlotBlockingSlots(ArmorType armorType)
		{
			return Enumerable.Select<KeyValuePair<ArmorType, ArmorSlot>, ArmorType>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(this._slotsByType, (KeyValuePair<ArmorType, ArmorSlot> entry) => !entry.Value.Empty), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Value.StoredArmoredType.Intersect(armorType)), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Key);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0004D425 File Offset: 0x0004B625
		public ArmorType GetSlotBlockingSlot(ArmorType armorType)
		{
			return Enumerable.FirstOrDefault<ArmorType>(this.GetSlotBlockingSlots(armorType), ArmorType.Empty);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0004D438 File Offset: 0x0004B638
		public ArmorType GetSlotBlockingSlot(ArmorLayers layer, DamageZone zone)
		{
			return this.GetSlotBlockingSlot(new ArmorType(layer, zone));
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0004D447 File Offset: 0x0004B647
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

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0004D470 File Offset: 0x0004B670
		public int GetSlotBlockingSlotIndex(ArmorType armorType)
		{
			return ArmorInventory.IndexFromArmorType(this.GetSlotBlockingSlot(armorType));
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0004D47E File Offset: 0x0004B67E
		public int GetSlotBlockingSlotIndex(ArmorLayers layer, DamageZone zone)
		{
			return ArmorInventory.IndexFromArmorType(this.GetSlotBlockingSlot(new ArmorType(layer, zone)));
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0004D494 File Offset: 0x0004B694
		public ArmorType GetFittingSlot(ArmorType armorType)
		{
			return Enumerable.FirstOrDefault<ArmorType>(Enumerable.Where<ArmorType>(this._slotsByType.Keys, (ArmorType slot) => slot.Intersect(armorType)), ArmorType.Empty);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0004D4D4 File Offset: 0x0004B6D4
		public int GetFittingSlotIndex(ArmorType armorType)
		{
			return ArmorInventory.IndexFromArmorType(this.GetFittingSlot(armorType));
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0004D4E4 File Offset: 0x0004B6E4
		public bool IsSlotAvailable(ArmorType armorType)
		{
			return !Enumerable.Any<KeyValuePair<ArmorType, ArmorSlot>>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(this._slotsByType, (KeyValuePair<ArmorType, ArmorSlot> entry) => !entry.Value.Empty), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Value.StoredArmoredType.Intersect(armorType));
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x0004D53C File Offset: 0x0004B73C
		public bool IsSlotAvailable(ArmorLayers layer, DamageZone zone)
		{
			return this.IsSlotAvailable(new ArmorType(layer, zone));
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0004D54B File Offset: 0x0004B74B
		public bool IsSlotAvailable(int index)
		{
			return this.IsSlotAvailable(ArmorInventory.ArmorTypeFromIndex(index));
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0004D55C File Offset: 0x0004B75C
		public bool CanHoldArmorPiece(ArmorType armorType)
		{
			return !Enumerable.Any<KeyValuePair<ArmorType, ArmorSlot>>(Enumerable.Where<KeyValuePair<ArmorType, ArmorSlot>>(this._slotsByType, (KeyValuePair<ArmorType, ArmorSlot> entry) => !entry.Value.Empty), (KeyValuePair<ArmorType, ArmorSlot> entry) => entry.Value.StoredArmoredType.Intersect(armorType));
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0004D5B4 File Offset: 0x0004B7B4
		public bool CanHoldArmorPiece(IArmor armor)
		{
			return this.CanHoldArmorPiece(armor.ArmorType);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0004D5C2 File Offset: 0x0004B7C2
		public bool CanHoldArmorPiece(ArmorLayers layer, DamageZone zone)
		{
			return this.CanHoldArmorPiece(new ArmorType(layer, zone));
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0004D5D4 File Offset: 0x0004B7D4
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

		// Token: 0x06000FCC RID: 4044 RVA: 0x0004D645 File Offset: 0x0004B845
		internal ItemSlot CreateNewSlot(int slotId)
		{
			return this.NewSlot(slotId);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0004D650 File Offset: 0x0004B850
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

		// Token: 0x06000FCE RID: 4046 RVA: 0x0004D74C File Offset: 0x0004B94C
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

		// Token: 0x06000FCF RID: 4047 RVA: 0x0004D7E4 File Offset: 0x0004B9E4
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

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0004D888 File Offset: 0x0004BA88
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

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0004DA0C File Offset: 0x0004BC0C
		internal static bool IsVanillaArmorSlot(int index)
		{
			return index >= ArmorInventory._clothesSlotsCount && index < ArmorInventory._clothesSlotsCount + 3;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0004DA22 File Offset: 0x0004BC22
		internal static bool IsModdedArmorSlot(int index)
		{
			return index >= ArmorInventory._vanillaSlots && index < ArmorInventory._armorSlotsLastIndex;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0004DA38 File Offset: 0x0004BC38
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

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0004DA8C File Offset: 0x0004BC8C
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

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0004DABC File Offset: 0x0004BCBC
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

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0004DAF8 File Offset: 0x0004BCF8
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

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0004DB58 File Offset: 0x0004BD58
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

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0004DBC4 File Offset: 0x0004BDC4
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

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0004DBFF File Offset: 0x0004BDFF
		[NullableContext(2)]
		private InventoryPlayerBackpacks GetBackpackInventory()
		{
			IPlayer player = base.Player;
			return ((player != null) ? player.InventoryManager.GetOwnInventory("backpack") : null) as InventoryPlayerBackpacks;
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0004DC24 File Offset: 0x0004BE24
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

		// Token: 0x06000FDB RID: 4059 RVA: 0x0004DCD8 File Offset: 0x0004BED8
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

		// Token: 0x06000FDC RID: 4060 RVA: 0x0004DF90 File Offset: 0x0004C190
		private void RefreshWearableStats()
		{
			IPlayer player2 = base.Player;
			EntityPlayer player = (player2 != null) ? player2.Entity : null;
			if (((player != null) ? player.SidedProperties : null) == null)
			{
				return;
			}
			WearableStatsBehavior behavior = player.GetBehavior<WearableStatsBehavior>();
			if (behavior == null)
			{
				return;
			}
			behavior.RefreshStatsNow();
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0004DFD0 File Offset: 0x0004C1D0
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

		// Token: 0x04000760 RID: 1888
		public static readonly List<string> GearSlotTypes;

		// Token: 0x04000761 RID: 1889
		internal ItemSlot[] _slots;

		// Token: 0x04000762 RID: 1890
		internal readonly Dictionary<string, GearSlot> _gearSlots = new Dictionary<string, GearSlot>();

		// Token: 0x04000763 RID: 1891
		internal readonly Dictionary<ArmorType, ArmorSlot> _slotsByType = new Dictionary<ArmorType, ArmorSlot>();

		// Token: 0x04000764 RID: 1892
		internal readonly Dictionary<EnumCharacterDressType, string> _clothesSlotsIcons;

		// Token: 0x04000765 RID: 1893
		internal readonly Dictionary<ArmorType, string> _armorSlotsIcons;

		// Token: 0x04000766 RID: 1894
		internal readonly Dictionary<string, string> _gearSlotsIcons;

		// Token: 0x04000767 RID: 1895
		internal const int _clothesArmorSlots = 3;

		// Token: 0x04000768 RID: 1896
		internal static readonly int _moddedArmorSlotsCount;

		// Token: 0x04000769 RID: 1897
		internal static readonly int _clothesSlotsCount;

		// Token: 0x0400076A RID: 1898
		internal static readonly int _vanillaSlots;

		// Token: 0x0400076B RID: 1899
		internal static readonly int _armorSlotsLastIndex;

		// Token: 0x0400076C RID: 1900
		internal static readonly int _gearSlotsCount;

		// Token: 0x0400076D RID: 1901
		internal static readonly int _gearSlotsLastIndex;

		// Token: 0x0400076E RID: 1902
		internal static readonly int _totalSlotsNumber;

		// Token: 0x0400076F RID: 1903
		[Nullable(2)]
		internal static readonly FieldInfo _backpackBagInventory;

		// Token: 0x04000770 RID: 1904
		[Nullable(2)]
		internal static readonly FieldInfo _backpackBagSlots;

		// Token: 0x04000771 RID: 1905
		internal readonly ICoreAPI _api;

		// Token: 0x04000772 RID: 1906
		internal static bool _disableVanillaArmorSlots;

		// Token: 0x04000773 RID: 1907
		internal bool _clearedArmorSlots;

		// Token: 0x02000296 RID: 662
		// (Invoke) Token: 0x06001819 RID: 6169
		[NullableContext(0)]
		public delegate void SlotModifiedDelegate(bool itemChanged, bool durabilityChanged, bool isArmorSlot);

		// Token: 0x02000297 RID: 663
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D87 RID: 3463
			[Nullable(0)]
			public static Func<ArmorType, int> <0>__IndexFromArmorType;
		}
	}
}
