using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000132 RID: 306
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WearableStatsBehavior : EntityBehavior, IDisposable
	{
		// Token: 0x06000EDA RID: 3802 RVA: 0x0004840C File Offset: 0x0004660C
		public WearableStatsBehavior(Entity entity) : base(entity)
		{
			EntityPlayer entityPlayer = entity as EntityPlayer;
			if (entityPlayer == null)
			{
				throw new InvalidDataException("This is player behavior");
			}
			this._player = entityPlayer;
			this._system = this._player.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			this._system.OnDispose += new Action(this.Dispose);
			if (this._player.Api.Side == 2)
			{
				WearableStatsBehavior previousBehavior;
				if (WearableStatsBehavior._existingBehaviors.TryGetValue(this._player.PlayerUID, ref previousBehavior))
				{
					previousBehavior.PartialDispose();
				}
				WearableStatsBehavior._existingBehaviors[this._player.PlayerUID] = this;
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000484C2 File Offset: 0x000466C2
		public override string PropertyName()
		{
			return "CombatOverhaul:WearableStats";
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x000484C9 File Offset: 0x000466C9
		public Dictionary<string, float> Stats { get; } = new Dictionary<string, float>();

		// Token: 0x06000EDD RID: 3805 RVA: 0x000484D1 File Offset: 0x000466D1
		public override void AfterInitialized(bool onFirstSpawn)
		{
			this.TryInitialize();
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x000484DA File Offset: 0x000466DA
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.DisposeCore(false);
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x000484EA File Offset: 0x000466EA
		public override void OnGameTick(float deltaTime)
		{
			this.TryInitialize();
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x000484F3 File Offset: 0x000466F3
		public void RefreshStatsNow()
		{
			if (this._disposed)
			{
				return;
			}
			this.TryInitialize();
			this.UpdateStatsValuesConditional(true, true, true);
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00048510 File Offset: 0x00046710
		private bool TryInitialize()
		{
			if (this._initialized || this._disposed)
			{
				return this._initialized;
			}
			InventoryBase inventory = WearableStatsBehavior.GetGearInventory(this._player);
			if (inventory == null)
			{
				return false;
			}
			ArmorInventory armorInventory = inventory as ArmorInventory;
			if (armorInventory != null)
			{
				armorInventory.OnSlotModified += this.UpdateStatsValuesConditional;
				this._subscribedInventory = armorInventory;
				this._subscribedArmorInventory = true;
			}
			else
			{
				inventory.SlotModified += new Action<int>(this.UpdateStatsValues);
				this._subscribedInventory = inventory;
				this._subscribedArmorInventory = false;
			}
			this.UpdateStatsValues(0);
			this._initialized = true;
			return true;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x000485A0 File Offset: 0x000467A0
		[return: Nullable(2)]
		private static InventoryBase GetGearInventory(Entity entity)
		{
			EntityBehaviorPlayerInventory behavior = entity.GetBehavior<EntityBehaviorPlayerInventory>();
			if (behavior == null)
			{
				return null;
			}
			return behavior.Inventory;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x000485B3 File Offset: 0x000467B3
		private void UpdateStatsValues(int slotId)
		{
			this.UpdateStatsValuesConditional(true, true, true);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x000485C0 File Offset: 0x000467C0
		private void UpdateStatsValuesConditional(bool itemChanged, bool durabilityChanged, bool isArmorSlot)
		{
			InventoryBase inventory = WearableStatsBehavior.GetGearInventory(this._player);
			if (inventory == null)
			{
				return;
			}
			bool anyStatsChangedItem = itemChanged;
			bool anyStatsChangedBehavior = itemChanged;
			if (!itemChanged)
			{
				foreach (ItemSlot itemSlot in Enumerable.Where<ItemSlot>(Enumerable.Where<ItemSlot>(inventory, delegate(ItemSlot slot)
				{
					object obj3;
					if (slot == null)
					{
						obj3 = null;
					}
					else
					{
						ItemStack itemstack3 = slot.Itemstack;
						obj3 = ((itemstack3 != null) ? itemstack3.Item : null);
					}
					return obj3 != null;
				}), (ItemSlot slot) => slot.Itemstack.Item.GetRemainingDurability(slot.Itemstack) > 0))
				{
					object obj;
					if (itemSlot == null)
					{
						obj = null;
					}
					else
					{
						ItemStack itemstack = itemSlot.Itemstack;
						obj = ((itemstack != null) ? itemstack.Item : null);
					}
					IAffectsPlayerStats item = obj as IAffectsPlayerStats;
					if (item != null && item.StatsChanged)
					{
						anyStatsChangedItem = true;
						item.StatsChanged = false;
					}
				}
				foreach (ItemSlot itemSlot2 in Enumerable.Where<ItemSlot>(Enumerable.Where<ItemSlot>(inventory, delegate(ItemSlot slot)
				{
					object obj3;
					if (slot == null)
					{
						obj3 = null;
					}
					else
					{
						ItemStack itemstack3 = slot.Itemstack;
						obj3 = ((itemstack3 != null) ? itemstack3.Item : null);
					}
					return obj3 != null;
				}), (ItemSlot slot) => slot.Itemstack.Item.GetRemainingDurability(slot.Itemstack) > 0))
				{
					IAffectsPlayerStats behavior3 = Enumerable.FirstOrDefault<CollectibleBehavior>(Enumerable.Where<CollectibleBehavior>(itemSlot2.Itemstack.Item.CollectibleBehaviors, (CollectibleBehavior behavior) => behavior is IAffectsPlayerStats), null) as IAffectsPlayerStats;
					if (behavior3 != null && behavior3.StatsChanged)
					{
						anyStatsChangedBehavior = true;
						behavior3.StatsChanged = false;
					}
				}
			}
			if (!anyStatsChangedItem && !anyStatsChangedBehavior)
			{
				return;
			}
			string text;
			float num;
			foreach (KeyValuePair<string, float> keyValuePair in this.Stats)
			{
				keyValuePair.Deconstruct(ref text, ref num);
				string stat = text;
				this._player.Stats.Remove(stat, "CombatOverhaul:Armor");
			}
			this.Stats.Clear();
			if (anyStatsChangedItem)
			{
				foreach (ItemSlot slot3 in Enumerable.Where<ItemSlot>(Enumerable.Where<ItemSlot>(inventory, delegate(ItemSlot slot)
				{
					object obj3;
					if (slot == null)
					{
						obj3 = null;
					}
					else
					{
						ItemStack itemstack3 = slot.Itemstack;
						obj3 = ((itemstack3 != null) ? itemstack3.Item : null);
					}
					return obj3 != null;
				}), (ItemSlot slot) => slot.Itemstack.Item.GetRemainingDurability(slot.Itemstack) > 0 || slot.Itemstack.Item.GetMaxDurability(slot.Itemstack) == 0))
				{
					object obj2;
					if (slot3 == null)
					{
						obj2 = null;
					}
					else
					{
						ItemStack itemstack2 = slot3.Itemstack;
						obj2 = ((itemstack2 != null) ? itemstack2.Item : null);
					}
					IAffectsPlayerStats item2 = obj2 as IAffectsPlayerStats;
					if (item2 != null)
					{
						foreach (KeyValuePair<string, float> keyValuePair in item2.PlayerStats(slot3, this._player))
						{
							keyValuePair.Deconstruct(ref text, ref num);
							string stat2 = text;
							float value = num;
							this.AddStatValue(stat2, value);
						}
					}
				}
			}
			if (anyStatsChangedBehavior)
			{
				foreach (ItemSlot slot2 in Enumerable.Where<ItemSlot>(Enumerable.Where<ItemSlot>(inventory, delegate(ItemSlot slot)
				{
					object obj3;
					if (slot == null)
					{
						obj3 = null;
					}
					else
					{
						ItemStack itemstack3 = slot.Itemstack;
						obj3 = ((itemstack3 != null) ? itemstack3.Item : null);
					}
					return obj3 != null;
				}), (ItemSlot slot) => slot.Itemstack.Item.GetRemainingDurability(slot.Itemstack) > 0 || slot.Itemstack.Item.GetMaxDurability(slot.Itemstack) == 0))
				{
					IAffectsPlayerStats behavior2 = Enumerable.FirstOrDefault<CollectibleBehavior>(Enumerable.Where<CollectibleBehavior>(slot2.Itemstack.Item.CollectibleBehaviors, (CollectibleBehavior behavior) => behavior is IAffectsPlayerStats), null) as IAffectsPlayerStats;
					if (behavior2 != null)
					{
						foreach (KeyValuePair<string, float> keyValuePair in behavior2.PlayerStats(slot2, this._player))
						{
							keyValuePair.Deconstruct(ref text, ref num);
							string stat3 = text;
							float value2 = num;
							this.AddStatValue(stat3, value2);
						}
					}
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair in this.Stats)
			{
				keyValuePair.Deconstruct(ref text, ref num);
				string stat4 = text;
				float value3 = num;
				this._player.Stats.Set(stat4, "CombatOverhaul:Armor", value3, false);
			}
			this._player.walkSpeed = this._player.Stats.GetBlended("walkspeed");
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00048AA4 File Offset: 0x00046CA4
		private void AddStatValue(string stat, float value)
		{
			if (stat == "walkspeed" && value < 0f)
			{
				value *= this._player.Stats.GetBlended("armorWalkSpeedAffectedness");
			}
			if (stat == "hungerrate" && value > 0f)
			{
				value *= this._player.Stats.GetBlended("armorHungerRateAffectedness");
			}
			if (stat == "manipulationSpeed" && value < 0f)
			{
				value *= this._player.Stats.GetBlended("armorManipulationSpeedAffectedness");
			}
			if (!this.Stats.ContainsKey(stat))
			{
				this.Stats[stat] = value;
				return;
			}
			Dictionary<string, float> stats = this.Stats;
			stats[stat] += value;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00048B70 File Offset: 0x00046D70
		private void PartialDispose()
		{
			this.DisposeCore(false);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00048B79 File Offset: 0x00046D79
		public void Dispose()
		{
			this.DisposeCore(true);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00048B84 File Offset: 0x00046D84
		private void DisposeCore(bool clearExisting)
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			this.UnsubscribeInventory();
			this._system.OnDispose -= new Action(this.Dispose);
			if (clearExisting)
			{
				WearableStatsBehavior._existingBehaviors.Clear();
				return;
			}
			WearableStatsBehavior behavior;
			if (WearableStatsBehavior._existingBehaviors.TryGetValue(this._player.PlayerUID, ref behavior) && behavior == this)
			{
				WearableStatsBehavior._existingBehaviors.Remove(this._player.PlayerUID);
			}
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00048C00 File Offset: 0x00046E00
		private void UnsubscribeInventory()
		{
			InventoryBase inventory = this._subscribedInventory ?? WearableStatsBehavior.GetGearInventory(this._player);
			if (inventory == null)
			{
				return;
			}
			if (this._subscribedArmorInventory)
			{
				ArmorInventory armorInventory = inventory as ArmorInventory;
				if (armorInventory != null)
				{
					armorInventory.OnSlotModified -= this.UpdateStatsValuesConditional;
					goto IL_52;
				}
			}
			inventory.SlotModified -= new Action<int>(this.UpdateStatsValues);
			IL_52:
			this._subscribedInventory = null;
		}

		// Token: 0x04000725 RID: 1829
		private readonly EntityPlayer _player;

		// Token: 0x04000726 RID: 1830
		private readonly CombatOverhaulSystem _system;

		// Token: 0x04000727 RID: 1831
		private const string _statsCategory = "CombatOverhaul:Armor";

		// Token: 0x04000728 RID: 1832
		private bool _initialized;

		// Token: 0x04000729 RID: 1833
		private bool _disposed;

		// Token: 0x0400072A RID: 1834
		[Nullable(2)]
		private InventoryBase _subscribedInventory;

		// Token: 0x0400072B RID: 1835
		private bool _subscribedArmorInventory;

		// Token: 0x0400072C RID: 1836
		private static readonly Dictionary<string, WearableStatsBehavior> _existingBehaviors = new Dictionary<string, WearableStatsBehavior>();
	}
}
