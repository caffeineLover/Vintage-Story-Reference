using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200008D RID: 141
	[NullableContext(1)]
	[Nullable(0)]
	public class SlingServer : RangeWeaponServer
	{
		// Token: 0x06000787 RID: 1927 RVA: 0x0002D0CC File Offset: 0x0002B2CC
		public SlingServer(ICoreServerAPI api, Item item) : base(api, item)
		{
			ProjectileSystemServer serverProjectileSystem = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).ServerProjectileSystem;
			if (serverProjectileSystem == null)
			{
				throw new Exception();
			}
			this.ProjectileSystem = serverProjectileSystem;
			this.Stats = item.Attributes.AsObject<SlingStats>(null);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002D120 File Offset: 0x0002B320
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
			if (flag && ammoSlot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this.Stats.BulletWildcard, ammoSlot.Itemstack.Item.Code.ToString()))
			{
				this.BulletSlots[player.Entity.EntityId] = new ValueTuple<InventoryBase, int>(ammoSlot.Inventory, ammoSlot.Inventory.GetSlotId(ammoSlot));
				return true;
			}
			if (ammoSlot == null)
			{
				this.BulletSlots.Remove(player.Entity.EntityId);
				return true;
			}
			return false;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002D1D4 File Offset: 0x0002B3D4
		public override bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			GeneralUtils.MarkItemStack(slot);
			if (!this.BulletSlots.ContainsKey(player.Entity.EntityId))
			{
				return false;
			}
			ValueTuple<InventoryBase, int> valueTuple = this.BulletSlots[player.Entity.EntityId];
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
				this.BulletSlots.Remove(player.Entity.EntityId);
				return false;
			}
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
			Vector3d playerVelocity;
			playerVelocity..ctor(player.Entity.ServerPos.Motion.X, player.Entity.ServerPos.Motion.Y, player.Entity.ServerPos.Motion.Z);
			float swingSpeed;
			float manipulationSpeed;
			this.GetDamageMultiplier(packet, out swingSpeed, out manipulationSpeed);
			float speedFactor = MathF.Sqrt(swingSpeed * manipulationSpeed);
			ProjectileSpawnStats spawnStats = new ProjectileSpawnStats
			{
				ProducerEntityId = player.Entity.EntityId,
				DamageMultiplier = this.Stats.BulletDamageMultiplier * stackStats.DamageMultiplier * swingSpeed * Math.Clamp(manipulationSpeed, 1f, 2f),
				DamageTier = this.Stats.BulletDamageTier + stackStats.DamageTierBonus,
				Position = new Vector3d(packet.Position[0], packet.Position[1], packet.Position[2]),
				Velocity = base.GetDirectionWithDispersion(packet.Velocity, new float[]
				{
					this.Stats.DispersionMOA[0] * stackStats.DispersionMultiplier,
					this.Stats.DispersionMOA[1] * stackStats.DispersionMultiplier
				}) * (double)this.Stats.BulletVelocity * (double)stackStats.ProjectileSpeed * (double)speedFactor + playerVelocity
			};
			this.ProjectileSystem.SpawnFromWeaponSlot(packet.ProjectileId[0], stats, spawnStats, arrowSlot.TakeOut(1), slot, shooter);
			slot.Itemstack.Item.DamageItem(player.Entity.World, player.Entity, slot, 1 + stats.AdditionalDurabilityCost, true);
			slot.MarkDirty();
			arrowSlot.MarkDirty();
			return true;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0002D460 File Offset: 0x0002B660
		protected virtual void GetDamageMultiplier(ShotPacket packet, out float swingSpeed, out float manipulationSpeed)
		{
			BinaryReader writer = new BinaryReader(new MemoryStream(packet.Data));
			swingSpeed = writer.ReadSingle();
			manipulationSpeed = writer.ReadSingle();
		}

		// Token: 0x0400035F RID: 863
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		protected readonly Dictionary<long, ValueTuple<InventoryBase, int>> BulletSlots = new Dictionary<long, ValueTuple<InventoryBase, int>>();

		// Token: 0x04000360 RID: 864
		protected readonly SlingStats Stats;
	}
}
