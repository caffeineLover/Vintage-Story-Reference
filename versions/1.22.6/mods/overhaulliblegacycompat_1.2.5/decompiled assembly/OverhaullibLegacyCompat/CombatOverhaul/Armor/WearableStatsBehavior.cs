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
	// Token: 0x02000126 RID: 294
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WearableStatsBehavior : EntityBehavior, IDisposable
	{
		// Token: 0x06000E66 RID: 3686 RVA: 0x00046B0C File Offset: 0x00044D0C
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

		// Token: 0x06000E67 RID: 3687 RVA: 0x00046BC2 File Offset: 0x00044DC2
		public override string PropertyName()
		{
			return "CombatOverhaul:WearableStats";
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x00046BC9 File Offset: 0x00044DC9
		public Dictionary<string, float> Stats { get; } = new Dictionary<string, float>();

		// Token: 0x06000E69 RID: 3689 RVA: 0x00046BD1 File Offset: 0x00044DD1
		public override void AfterInitialized(bool onFirstSpawn)
		{
			this.TryInitialize();
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00046BDA File Offset: 0x00044DDA
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.DisposeCore(false);
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00046BEA File Offset: 0x00044DEA
		public override void OnGameTick(float deltaTime)
		{
			this.TryInitialize();
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00046BF3 File Offset: 0x00044DF3
		public void RefreshStatsNow()
		{
			if (this._disposed)
			{
				return;
			}
			this.TryInitialize();
			this.UpdateStatsValuesConditional(true, true, true);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00046C10 File Offset: 0x00044E10
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

		// Token: 0x06000E6E RID: 3694 RVA: 0x00046CA0 File Offset: 0x00044EA0
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

		// Token: 0x06000E6F RID: 3695 RVA: 0x00046CB3 File Offset: 0x00044EB3
		private void UpdateStatsValues(int slotId)
		{
			this.UpdateStatsValuesConditional(true, true, true);
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00046CC0 File Offset: 0x00044EC0
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

		// Token: 0x06000E71 RID: 3697 RVA: 0x000471A4 File Offset: 0x000453A4
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

		// Token: 0x06000E72 RID: 3698 RVA: 0x00047270 File Offset: 0x00045470
		private void PartialDispose()
		{
			this.DisposeCore(false);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00047279 File Offset: 0x00045479
		public void Dispose()
		{
			this.DisposeCore(true);
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00047284 File Offset: 0x00045484
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

		// Token: 0x06000E75 RID: 3701 RVA: 0x00047300 File Offset: 0x00045500
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

		// Token: 0x040006FF RID: 1791
		private readonly EntityPlayer _player;

		// Token: 0x04000700 RID: 1792
		private readonly CombatOverhaulSystem _system;

		// Token: 0x04000701 RID: 1793
		private const string _statsCategory = "CombatOverhaul:Armor";

		// Token: 0x04000702 RID: 1794
		private bool _initialized;

		// Token: 0x04000703 RID: 1795
		private bool _disposed;

		// Token: 0x04000704 RID: 1796
		[Nullable(2)]
		private InventoryBase _subscribedInventory;

		// Token: 0x04000705 RID: 1797
		private bool _subscribedArmorInventory;

		// Token: 0x04000706 RID: 1798
		private static readonly Dictionary<string, WearableStatsBehavior> _existingBehaviors = new Dictionary<string, WearableStatsBehavior>();
	}
}
