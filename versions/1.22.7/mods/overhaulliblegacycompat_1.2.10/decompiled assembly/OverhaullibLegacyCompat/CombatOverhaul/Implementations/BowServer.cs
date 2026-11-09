using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Compatibility;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000076 RID: 118
	[NullableContext(1)]
	[Nullable(0)]
	public class BowServer : RangeWeaponServer
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x00020B38 File Offset: 0x0001ED38
		public BowServer(ICoreServerAPI api, Item item) : base(api, item)
		{
			ProjectileSystemServer serverProjectileSystem = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).ServerProjectileSystem;
			if (serverProjectileSystem == null)
			{
				throw new Exception();
			}
			this.ProjectileSystem = serverProjectileSystem;
			this.Stats = item.Attributes.AsObject<BowStats>(null);
			this.ArcheringCompat = new ArcheringBowCompat(api);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00020B98 File Offset: 0x0001ED98
		public override bool Reload(IServerPlayer player, ItemSlot slot, [Nullable(2)] ItemSlot ammoSlot, ReloadPacket packet)
		{
			GeneralUtils.MarkItemStack(slot);
			bool flag;
			if (ammoSlot == null)
			{
				flag = (null != null);
			}
			else
			{
				ItemStack itemstack = ammoSlot.Itemstack;
				flag = (((itemstack != null) ? itemstack.Item : null) != null);
			}
			if (flag && ammoSlot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.Stats.ArrowWildcard, ammoSlot.Itemstack.Item.Code.ToString()))
			{
				this.ArrowSlots[player.Entity.EntityId] = new ValueTuple<InventoryBase, int>(ammoSlot.Inventory, ammoSlot.Inventory.GetSlotId(ammoSlot));
				return true;
			}
			if (ammoSlot == null)
			{
				this.ArrowSlots.Remove(player.Entity.EntityId);
				return true;
			}
			return false;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00020C4C File Offset: 0x0001EE4C
		public override bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			GeneralUtils.MarkItemStack(slot);
			if (!this.ArrowSlots.ContainsKey(player.Entity.EntityId))
			{
				return false;
			}
			ValueTuple<InventoryBase, int> valueTuple = this.ArrowSlots[player.Entity.EntityId];
			InventoryBase inventory = valueTuple.Item1;
			int slotId = valueTuple.Item2;
			if (inventory.Count <= slotId)
			{
				return false;
			}
			ItemSlot arrowSlot = inventory[slotId];
			if (((arrowSlot != null) ? arrowSlot.Itemstack : null) == null || arrowSlot.Itemstack.StackSize < 1)
			{
				return false;
			}
			ProjectileBehavior collectibleBehavior = arrowSlot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true);
			ProjectileStats stats = (collectibleBehavior != null) ? collectibleBehavior.GetStats(arrowSlot.Itemstack) : null;
			if (stats == null)
			{
				this.ArrowSlots.Remove(player.Entity.EntityId);
				return false;
			}
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
			this.ArcheringCompat.ApplyArrowBreakChance(slot.Itemstack.Item, arrowSlot.Itemstack.Item, stats);
			float archeringDamageMultiplier = this.ArcheringCompat.GetBowDamageMultiplier(slot.Itemstack.Item) * this.ArcheringCompat.GetDrawTimeArrowDamageMultiplier(slot.Itemstack.Item);
			Vector3d playerVelocity;
			playerVelocity..ctor(player.Entity.ServerPos.Motion.X, player.Entity.ServerPos.Motion.Y, player.Entity.ServerPos.Motion.Z);
			ProjectileSpawnStats spawnStats = new ProjectileSpawnStats
			{
				ProducerEntityId = player.Entity.EntityId,
				DamageMultiplier = this.Stats.ArrowDamageMultiplier * stackStats.DamageMultiplier * archeringDamageMultiplier,
				DamageTier = this.Stats.ArrowDamageTier + stackStats.DamageTierBonus,
				Position = new Vector3d(packet.Position[0], packet.Position[1], packet.Position[2]),
				Velocity = base.GetDirectionWithDispersion(packet.Velocity, new float[]
				{
					this.Stats.DispersionMOA[0] * stackStats.DispersionMultiplier,
					this.Stats.DispersionMOA[1] * stackStats.DispersionMultiplier
				}) * (double)this.Stats.ArrowVelocity * (double)stackStats.ProjectileSpeed * (double)this.ArcheringCompat.GetVelocityMultiplier(slot.Itemstack.Item) + playerVelocity,
				GravityFactorMultiplier = this.ArcheringCompat.GetGravityFactorMultiplier(slot.Itemstack.Item)
			};
			this.ProjectileSystem.SpawnFromWeaponSlot(packet.ProjectileId[0], stats, spawnStats, arrowSlot.TakeOut(1), slot, shooter);
			slot.Itemstack.Item.DamageItem(player.Entity.World, player.Entity, slot, 1 + stats.AdditionalDurabilityCost, true);
			slot.MarkDirty();
			arrowSlot.MarkDirty();
			return true;
		}

		// Token: 0x04000267 RID: 615
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		protected readonly Dictionary<long, ValueTuple<InventoryBase, int>> ArrowSlots = new Dictionary<long, ValueTuple<InventoryBase, int>>();

		// Token: 0x04000268 RID: 616
		protected readonly BowStats Stats;

		// Token: 0x04000269 RID: 617
		private readonly ArcheringBowCompat ArcheringCompat;
	}
}
